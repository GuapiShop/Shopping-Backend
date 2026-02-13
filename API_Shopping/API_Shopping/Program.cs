using API_Shopping.Context;
using API_Shopping.Interfaces;
using API_Shopping.Services;
using API_Shopping.Middleware;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options => options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))); //

builder.Services.AddScoped<JwtService>();
builder.Services.AddAuthentication(config => // jwt configurations
{
    config.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme; // Barrer
    config.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

}).AddJwtBearer(config => { 
    config.RequireHttpsMetadata = true;
    config.SaveToken = true;
    config.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true, //validate the token signature is correct
        ValidateIssuer = false, // No validate the url when the token come (change in production)
        ValidateAudience = false, // No validate for who is created the token (change in production)
        ValidateLifetime = true, // Validate the token expiration
        ClockSkew = TimeSpan.Zero, 
        IssuerSigningKey = new SymmetricSecurityKey //validate the signature

        //ValidIssuer = builder.Configuration["JwtService:Issuer"],        //Enable in production
        //ValidAudience = builder.Configuration["JwtService:Audience"],    //Enable in production
        (Encoding.UTF8.GetBytes(builder.Configuration["JwtService:Key"]!))
    };
});

builder.Services.AddControllers(); 
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IDetailService, DetailService>(); 
builder.Services.AddScoped<IShoppingCartService, ShoppingCartService>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Use CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("NewPolicy", app=>{
        //app.WithOrigins("") // use in production, don't use AllowAnyOrigin()
        app.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod(); 
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseCors("NewPolicy");

app.UseAuthorization();

app.UseMiddleware<ExceptionMiddleware>();

app.MapControllers();

app.Run();
