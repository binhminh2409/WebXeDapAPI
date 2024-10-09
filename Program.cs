﻿using WebXeDapAPI.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using Microsoft.Extensions.FileProviders;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Mvc.NewtonsoftJson;
using Microsoft.Extensions.Logging;
using WebXeDapAPI.Service.Interfaces;
using WebXeDapAPI.Service;
using WebXeDapAPI.Repository.Interface;
using WebXeDapAPI.Helper;
using WebXeDapAPI.Repository;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
    
});

builder.Services.AddControllersWithViews()
    .AddNewtonsoftJson(options => options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

// Add services to the container.
builder.Services.AddScoped<IUserIService, UserService>();
builder.Services.AddScoped<IUserInterface, UserRepository>();
builder.Services.AddScoped<Token>();
builder.Services.AddScoped<ISlideIService, SlideService>();
builder.Services.AddScoped<ISlideInterface, SlideRepository>();
builder.Services.AddScoped<ITypeIService, TypeService>();
builder.Services.AddScoped<IBrandIService, BrandService>();
builder.Services.AddScoped<IProductsIService ,ProductsService>();
builder.Services.AddScoped<IProduct_DetailIService, Product_DetailService>();
builder.Services.AddScoped<IProductsInterface, ProductsRepository>();
builder.Services.AddScoped<IProducts_DrtailInterface, Products_DetailRepository>();
builder.Services.AddScoped<ICartInterface, CartRepository>();
builder.Services.AddScoped<IOrderIService, OrderService>();
builder.Services.AddScoped<ICartIService, CartService>();


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "XeDapAPI", Version = "v1" });
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});



builder.Services.AddSwaggerGen();

Console.WriteLine("ValidIssuer: " + builder.Configuration["Jwt:ValidIssuer"]);
Console.WriteLine("ValidAudience: " + builder.Configuration["Jwt:ValidAudience"]);
Console.WriteLine("Secret: " + builder.Configuration["Jwt:Secret"]);
builder.Services.AddAuthentication(option =>
{
    option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    option.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidIssuer = builder.Configuration["Jwt:ValidIssuer"],
        ValidAudience = builder.Configuration["Jwt:ValidAudience"],
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Secret"]))
    };
})
.AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
{
    options.Cookie.Name = "authenticationToken"; //Tên của cookie
    options.Cookie.HttpOnly = true;
    options.LoginPath = "/account/login"; //đường dẫn đến trang đăng nhập
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowOrigins", builder =>
    {
        builder.WithOrigins("http://localhost:5000")
            .AllowAnyHeader()
            .WithMethods("POST","PUT", "GET","DELETE", "OPTIONS")
            .AllowCredentials();
    });
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    dbContext.Database.EnsureCreated();
}

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(@"E:\Code\WebXeDap\WebXeDapAPI", "Image")),
    RequestPath = "" // Bỏ qua đường dẫn để có thể truy cập trực tiếp
});

// Configure the HTTP request pipeline.
app.UseSwagger();

app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseRouting();

app.UseStaticFiles();

app.UseCors("AllowOrigins");

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
