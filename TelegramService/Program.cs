using System.Threading.Tasks;
using Telegram.Bot;

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

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            builder.Services.AddSingleton<ITelegramBotClient>(sp =>
            {
                return new TelegramBotClient(botToken);
            });

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
