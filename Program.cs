using Andyyy.Controllers;
using Andyyy.Models;
using Microsoft.AspNetCore.Mvc;
using Serilog.Events;
using Serilog;
using Microsoft.AspNetCore.HttpLogging;
using StackExchange.Redis;
using Andyyy.Middleware;

namespace Andyyy
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //Log.Logger = new LoggerConfiguration()
            //.MinimumLevel.Information()
            //.WriteTo.Console()
            //.WriteTo.Seq("http://localhost:5341")
            //.CreateLogger();


            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddSingleton<DapperContext>();
            builder.Services.AddScoped<WorkSheetReposity>();
            builder.Services.AddControllers();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            app.UseMiddleware<WSExceptionMiddleware>();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }


            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();

            Log.CloseAndFlush();
        }
    }
}