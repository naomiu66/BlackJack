using MassTransit;
using Telegram.Bot;
using TelegramService.Handlers.Abstractions;
using TelegramService.Handlers.Commands.Router;
using TelegramService.Handlers.Implementations;
using TelegramService.Publishers.Abstractions;
using TelegramService.Publishers.Implementations;
using TelegramService.Services.Abstractions;
using TelegramService.Services.Implementations;

namespace TelegramService
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Configuration
               .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
               .AddEnvironmentVariables();

            var botToken = builder.Configuration["BOT_TOKEN"]
                         ?? builder.Configuration["Telegram:BotToken"]
                         ?? throw new InvalidOperationException("Telegram bot token is missing");

            var rabbitHost = builder.Configuration["RABBITMQ_HOST"] ?? builder.Configuration["RabbitMq:Host"];
            var rabbitPort = builder.Configuration["RABBITMQ_PORT"] ?? builder.Configuration["RabbitMq:Port"];
            var rabbitUser = builder.Configuration["RABBITMQ_USER"] ?? builder.Configuration["RabbitMq:UserName"];
            var rabbitPass = builder.Configuration["RABBITMQ_PASSWORD"] ?? builder.Configuration["RabbitMq:Password"];

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            builder.Services.AddMassTransit(x =>
            {
                x.AddConsumers(typeof(Program).Assembly);

                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(rabbitHost, h =>
                    {
                        h.Username(rabbitUser ?? throw new InvalidOperationException("rabbitMq user is not specified"));
                        h.Password(rabbitPass ?? throw new InvalidOperationException("rabbitMq password is not specified"));
                    });

                    cfg.ConfigureEndpoints(context);
                });
            });

            builder.Services.AddSingleton<ITelegramBotClient>(sp =>
            {
                return new TelegramBotClient(botToken);
            });

            builder.Services.AddScoped<IGameServicePublisher, GameServicePublisher>();
            builder.Services.AddScoped<IUserServicePublisher, UserServicePublisher>();

            builder.Services.AddScoped<IUpdateService, UpdateService>();

            builder.Services.AddScoped<IMessageHandler, MessageHandler>();
            builder.Services.AddScoped<ICallbackHandler, CallbackHandler>();

            builder.Services.AddScoped<ICommandRouter, CommandRouter>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                await using (var scope = app.Services.CreateAsyncScope())
                {
                    var bot = scope.ServiceProvider.GetRequiredService<ITelegramBotClient>();
                    var config = scope.ServiceProvider.GetRequiredService<IConfiguration>();

                    var publicUrl = await Extensions.DevExtensions.GetNgrokUrlAsync();

                    if (string.IsNullOrEmpty(publicUrl))
                        throw new Exception("Could not get public ngrok URL");

                    var webhookUrl = $"{publicUrl}/telegram/webhook";

                    await bot.DeleteWebhook();
                    await bot.SetWebhook(webhookUrl);

                    var info = await bot.GetWebhookInfo();

                    Console.WriteLine($"Webhook status: {info.Url} - last error: {info.LastErrorMessage} : {info.LastErrorDate}");
                }
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
