using Microsoft.AspNetCore.Identity;
using TradingApp.Application;
using TradingApp.Application.DataTransferObjects.ConnectionId;
using TradingApp.Application.Realtime;
using TradingApp.Application.Services.IdentityService;
using TradingApp.BackgroundTasks;
using TradingApp.BackgroundTasks.CoinBackgroundJobs;
using TradingApp.Database;
using TradingApp.Database.Context;
using TradingApp.Database.TradingAppUsers;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
builder.Services.AddAuthorization();
builder.Services.AddAuthentication().AddCookie(IdentityConstants.ApplicationScheme).
    AddBearerToken(IdentityConstants.BearerScheme);
builder.Services.AddHttpContextAccessor();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSignalR(options =>
{
    options.EnableDetailedErrors = true;
});
builder.Services.AddMemoryCache();
// Add services to the container.

builder.Services.AddDatabase(configuration);

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "MyPolicy", policy =>
    {
        policy.WithOrigins("http://localhost:4200").AllowAnyMethod().AllowAnyHeader().AllowCredentials();
    });

});

builder.Services.AddIdentityCore<TradingAppUser>()
    .AddEntityFrameworkStores<TradingAppContext>()
    .AddUserManager<IdentityService>()
    .AddApiEndpoints();
builder.Services.AddBackgroundTasks();
builder.Services.AddControllers();
builder.Services.AddSingleton<ConnectionIdDto>();
builder.Services.AddApplication();
builder.Services.AddHttpClient();
builder.Services.AddHostedService<CoinBackgroundJob>();
var app = builder.Build();

// Configure the HTTP request pipeline.

if (app.Environment.IsDevelopment())
{

    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapHub<CoinListHub>("coinHub");
app.UseHttpsRedirection();
app.UseCors("MyPolicy");
app.UseAuthorization();
app.MapIdentityApi<TradingAppUser>();
app.MapControllers();
app.Run();
