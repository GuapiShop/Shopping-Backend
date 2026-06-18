using API_Shopping.Context;
using API_Shopping.Interfaces;
using API_Shopping.Middlewares;
using API_Shopping.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));

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
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Use CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("NewPolicy", app=>{
        app.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod(); 
    });
});

// Global Exception
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.InvalidModelStateResponseFactory = context =>
    {
        var errors = context.ModelState
            .Where(e => e.Value.Errors.Count > 0)
            .SelectMany(e => e.Value.Errors.Select(err => err.ErrorMessage));

        return new BadRequestObjectResult(new ProblemDetails
        {
            Status = StatusCodes.Status400BadRequest,
            Title = "Invalid request",
            Detail = string.Join(" | ", errors),
            Instance = context.HttpContext.Request.Path
        });
    };
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
app.UseExceptionHandler();
app.MapControllers();

app.Run();
