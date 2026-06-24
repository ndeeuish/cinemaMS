using CinemaMS.API.Middlewares;
using CinemaMS.Application.Behaviors;
using CinemaMS.Infrastructure.Data;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using CinemaMS.Application.Interfaces.Security;
using CinemaMS.Infrastructure.Security;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using CinemaMS.API.Services;
using CinemaMS.Infrastructure.Services;
using CinemaMS.Application.Interfaces.Caching;
using CinemaMS.Application.Interfaces.Services;
using CinemaMS.Infrastructure.Settings;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
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
            Array.Empty<string>()
        }
    });
});

// Configure JWT
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection(JwtSettings.SectionName));
var jwtSettings = builder.Configuration.GetSection(JwtSettings.SectionName).Get<JwtSettings>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings!.Issuer,
            ValidAudience = jwtSettings.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Secret))
        };

        options.Events = new JwtBearerEvents
        {
            OnChallenge = context =>
            {
                context.HandleResponse();
                context.Response.StatusCode = 401;
                context.Response.ContentType = "application/json";
                var result = System.Text.Json.JsonSerializer.Serialize(new { message = "Unauthorized. Please ensure you have provided a valid Bearer token." });
                return context.Response.WriteAsync(result);
            },
            OnForbidden = context =>
            {
                context.Response.StatusCode = 403;
                context.Response.ContentType = "application/json";
                var result = System.Text.Json.JsonSerializer.Serialize(new { message = "Forbidden. You do not have permission to access this method." });
                return context.Response.WriteAsync(result);
            }
        };
    });

// Register DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register MediatR & Behaviors
builder.Services.AddMediatR(cfg => 
{
    cfg.RegisterServicesFromAssembly(typeof(ValidationBehavior<,>).Assembly);
    cfg.AddOpenBehavior(typeof(LoggingBehavior<,>));
    cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
});

// Register FluentValidation
builder.Services.AddValidatorsFromAssembly(typeof(ValidationBehavior<,>).Assembly);

// Register AutoMapper
builder.Services.AddAutoMapper(typeof(ValidationBehavior<,>).Assembly);

// Register Repositories
builder.Services.AddScoped<CinemaMS.Domain.Repositories.IUnitOfWork>(provider => provider.GetRequiredService<ApplicationDbContext>());
builder.Services.AddScoped<CinemaMS.Application.Interfaces.Data.IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());
builder.Services.AddScoped<CinemaMS.Domain.Repositories.ICinemaRepository, CinemaMS.Infrastructure.Repositories.CinemaRepository>();
builder.Services.AddScoped<CinemaMS.Domain.Repositories.IUserRepository, CinemaMS.Infrastructure.Repositories.UserRepository>();
builder.Services.AddScoped<CinemaMS.Domain.Repositories.IRoleRepository, CinemaMS.Infrastructure.Repositories.RoleRepository>();
builder.Services.AddScoped<CinemaMS.Domain.Repositories.IRoomRepository, CinemaMS.Infrastructure.Repositories.RoomRepository>();
builder.Services.AddScoped<CinemaMS.Domain.Repositories.ISeatRepository, CinemaMS.Infrastructure.Repositories.SeatRepository>();
builder.Services.AddScoped<CinemaMS.Domain.Repositories.IMovieRepository, CinemaMS.Infrastructure.Repositories.MovieRepository>();
builder.Services.AddScoped<CinemaMS.Domain.Repositories.IShowtimeRepository, CinemaMS.Infrastructure.Repositories.ShowtimeRepository>();
builder.Services.AddScoped<CinemaMS.Domain.Repositories.IBookingRepository, CinemaMS.Infrastructure.Repositories.BookingRepository>();
builder.Services.AddScoped<CinemaMS.Domain.Repositories.IPaymentRepository, CinemaMS.Infrastructure.Repositories.PaymentRepository>();
builder.Services.AddSingleton<IPasswordHasher, PasswordHasher>();
builder.Services.AddSingleton<IJwtProvider, JwtProvider>();

// Register Redis Cache
var redisConnectionString = builder.Configuration.GetConnectionString("RedisConnection");
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = redisConnectionString;
});
builder.Services.AddSingleton<IRedisCacheService, RedisCacheService>();

// Register Email Service
builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));
builder.Services.AddTransient<IEmailService, EmailService>();

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();

// Register Cloudinary Service
builder.Services.Configure<CloudinarySettings>(builder.Configuration.GetSection(CloudinarySettings.SectionName));
builder.Services.AddScoped<IImageService, CloudinaryService>();

builder.Services.AddHostedService<ExpiredBookingCleanupService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<GlobalExceptionMiddleware>();

app.UseHttpsRedirection();
app.UseCors("AllowAll");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
