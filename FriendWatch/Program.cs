using System.Text;

using FriendWatch.Application.Repositories;
using FriendWatch.Application.Services;
using FriendWatch.Infrastructure.Persistence;
using FriendWatch.Infrastructure.Repositories;
using FriendWatch.Infrastructure.Services;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.Net.Http.Headers;
using Microsoft.AspNetCore.Authorization;
using FriendWatch.Middlewares;

const string myCorsOriginName = "allowReactApp";
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddCors(options => options
    .AddPolicy(name: myCorsOriginName, policy => policy.WithOrigins("https://localhost", "https://192.168.0.102").AllowAnyMethod().AllowAnyHeader().AllowCredentials()));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAuthorization(options =>
    {
        options.DefaultPolicy = new AuthorizationPolicyBuilder().RequireClaim("tokenType", "access").Build();
        options.AddPolicy("RefreshPolicy", policy => policy.RequireClaim("tokenType", "refresh"));

    });
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
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
                // check which type of token is required for current authorized action
                var authorizeAttribute = context.HttpContext.GetEndpoint()?.Metadata.GetMetadata<AuthorizeAttribute>();

                if (authorizeAttribute != null && authorizeAttribute.Policy == "RefreshPolicy")
                    context.Token = context.Request.Cookies["RefreshToken"];
                else if (authorizeAttribute != null)
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

app.UseCors(myCorsOriginName);

app.MapControllers();

//app.UseMiddleware<RefreshTokenMiddleware>();

app.Run();
