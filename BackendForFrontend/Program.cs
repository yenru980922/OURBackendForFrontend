using AutoMapper;
using BackendForFrontend.Models.EFModels;
using BackendForFrontend.Models.Services;
using BackendForFrontend.Services;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MinimalAPIs.Endpoints;
using MinimalAPIs.Models.DTOs;
using MinimalAPIs.Services;
using MinimalAPIs.Validation;
using System.Text;

namespace MinimalAPIs
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var connectionString = builder.Configuration.GetConnectionString("AppDbContextConnection");
            builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
            builder.Services.AddTransient<EmailService>();

            // Add services to the container.
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer("Data Source=fuen31-t3.database.windows.net;Initial Catalog=ProjectDB;User ID=zlk;Password=fuen31t3!;TrustServerCertificate=true"));		
			
			//設置CORS
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll",
                    builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
            });
            
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddAutoMapper(typeof(MappingConfig));
            builder.Services.AddValidatorsFromAssemblyContaining<Program>();
            builder.Services.AddSingleton<JwtHelpers>();
            builder.Services.AddScoped<ICouponService, CouponService>();
			builder.Services.AddScoped<EcpayService>();
			builder.Services.AddAutoMapper(typeof(MappingConfig));
            builder.Services.AddScoped<IValidator<CouponCreateDTO>, CouponCreateValidation>();

            builder.Services
               .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
               .AddJwtBearer(options =>
               {
                   options.IncludeErrorDetails = true; //

                   options.TokenValidationParameters = new TokenValidationParameters
                   {
                       ValidateIssuer = true,
                       ValidIssuer = builder.Configuration["Jwt:Issuer"],
                       ValidateAudience = true,
                       ValidAudience = builder.Configuration["Jwt:Audience"],
                       ValidateLifetime = true,
                       ValidateIssuerSigningKey = true,
                       IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
                   };
               });

            builder.Services.AddSwaggerGen(options => {
                options.AddSecurityDefinition("BearerAuth", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    Description = "JWT Authorization header using the Bearer scheme."
                });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "BearerAuth"}
                        },
                        new string[] { }
                    }
                });
            });

			//
            builder.Services.AddDistributedSqlServerCache(options =>
            {
                options.ConnectionString = "Data Source=fuen31-t3.database.windows.net;Initial Catalog=ProjectDB;User ID=zlk;Password=fuen31t3!";
                options.SchemaName = "dbo";
                options.TableName = "MyCache";
            });

            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(20); //
                options.Cookie.HttpOnly = true; //
                options.Cookie.IsEssential = true; //
            });

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowFrontend", builder =>
                {
                    builder.WithOrigins("http://localhost:5173")
                           .AllowAnyHeader()
                           .AllowAnyMethod();
                });
            });


            // 注册 HttpClient 和 Google Books API 服务
            builder.Services.AddHttpClient<IGoogleBooksService, GoogleBooksService>(client =>
            {
                client.BaseAddress = new Uri("https://www.googleapis.com/books/v1/");
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            
            app.UseCors(option =>
                option.AllowAnyOrigin()
                      .AllowAnyMethod()
                      .AllowAnyHeader());

			
            // CORS策略
            app.UseCors("AllowFrontend");

            app.UseSession();

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseCors("AllowAll");
            app.UseAuthorization();

            app.UseStaticFiles();

            app.MapControllers();
            app.MapCouponEndpoints();

            app.Run();
        }
    }
}