using System.Text;

using FriendWatch.Application.Repositories;
using FriendWatch.Application.Services;
using FriendWatch.Middlewares;
using FriendWatch.Infrastructure.Persistence;
using FriendWatch.Infrastructure.Repositories;
using FriendWatch.Infrastructure.Services;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.Net.Http.Headers;

const string myCorsOriginName = "allowReactApp";
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddCors(options => options
    .AddPolicy(name: myCorsOriginName, policy => policy.WithOrigins("https://localhost:5173").AllowAnyMethod().AllowAnyHeader().AllowCredentials()));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ClockSkew = TimeSpan.Zero,
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
            ValidAudience = builder.Configuration["JWT:ValidAudience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["JWT:Secret"]!)
            ),
        };
        options.Events = new()
        {
            OnMessageReceived = (context) =>
            {
                context.Token = context.Request.Cookies["AccessToken"];
                return Task.CompletedTask;
            }
        };
    });
builder.Services.AddDbContext<FriendWatchDbContext>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ICircleRepository, CircleRepository>();
builder.Services.AddScoped<ICircleService, CircleService>();
builder.Services.AddScoped<IInvitationRepository, InvitationRepository>();
builder.Services.AddScoped<IInvitationService, InvitationService>();
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseMiddleware<RefreshTokenMiddleware>();

app.UseCors(myCorsOriginName);

app.MapControllers();

app.Run();
