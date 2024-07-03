//using AutoMapper;
//using MinimalAPIs.Data;
//using MinimalAPIs.Models;
//using MinimalAPIs.Models.DTOs;
//using FluentValidation;
//using System.Net;
//using MinimalAPIs.Endpoints;


//namespace MinimalAPIs
//{
//    public class Program
//    {
//        public static void Main(string[] args)
//        {
//            var builder = WebApplication.CreateBuilder(args);

//            // Add services to the container.

//            builder.Services.AddControllers();
//            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//            builder.Services.AddEndpointsApiExplorer();
//            builder.Services.AddSwaggerGen();
//            builder.Services.AddAutoMapper(typeof(MappingConfig));
//            builder.Services.AddValidatorsFromAssemblyContaining<Program>();


//            var app = builder.Build();

//            // Configure the HTTP request pipeline.
//            if (app.Environment.IsDevelopment())
//            {
//                app.UseSwagger();
//                app.UseSwaggerUI();
//            }

//            // using (var scope = app.Services.CreateScope())
//            // {
//            //     var context = scope.ServiceProvider.GetRequiredService<ProjectDBContext>();
//            //     context.Database.EnsureCreated();
//            // }
            

//            app.UseHttpsRedirection();

//            app.UseAuthorization();

//            app.MapControllers();

//            app.MapCouponEndpoints();

//            app.Run();
//        }
//    }
//}
