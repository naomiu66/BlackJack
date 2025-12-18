using MassTransit;
using Microsoft.EntityFrameworkCore;
using UserService.Data;
using UserService.Data.Repositories.Abstractions;
using UserService.Data.Repositories.Implementations;
using UserService.Extensions;
using UserService.Services.Abstractions;
using UserService.Services.Implementations;

namespace UserService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Configuration
               .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
               .AddEnvironmentVariables();

            builder.Services.AddControllers();

            var dbConnectionString = builder.Configuration["POSTGRES_CONNECTION_STRING"]
                         ?? builder.Configuration.GetConnectionString("DbConnection");

            var rabbitHost = builder.Configuration["RABBITMQ_HOST"] ?? builder.Configuration["RabbitMq:Host"];
            var rabbitPort = builder.Configuration["RABBITMQ_PORT"] ?? builder.Configuration["RabbitMq:Port"];
            var rabbitUser = builder.Configuration["RABBITMQ_USER"] ?? builder.Configuration["RabbitMq:UserName"];
            var rabbitPass = builder.Configuration["RABBITMQ_PASSWORD"] ?? builder.Configuration["RabbitMq:Password"];

            builder.Services.AddDbContext<ApplicationContext>(options =>
                options.UseNpgsql(dbConnectionString ?? throw new InvalidOperationException("Connection string not found.")));

            // TODO : CORS policy should be configured more specifically in production
            builder.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                });
            });

            builder.Services.AddSwaggerGen(c =>
            {
                //c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                //{
                //    Description = "¬ведите токен в формате: Bearer {token}",
                //    Name = "Authorization",
                //    In = Microsoft.OpenApi.Models.ParameterLocation.Header,
                //    Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
                //    Scheme = "Bearer"
                //});

                //c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
                //{
                //    {
                //        new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                //        {
                //            Reference = new Microsoft.OpenApi.Models.OpenApiReference
                //            {
                //                Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                //                Id = "Bearer"
                //            }
                //        },
                //        Array.Empty<string>()
                //    }
                //});
            });

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

            // Add services to the container.

            builder.Services.AddScoped<IUsersRepository, UsersRepository>();
            builder.Services.AddScoped<IUsersService, UsersService>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
                app.MigrateDatabase<ApplicationContext>();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.UseCors();

            app.MapControllers();

            app.Run();
        }
    }
}
