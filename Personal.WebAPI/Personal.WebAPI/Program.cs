using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Personal.WebAPI;
using Personal.WebAPI.Configurations;
using Personal.WebAPI.Context;
using Personal.WebAPI.Interfaces;
using Personal.WebAPI.Validators;
using System.Configuration;

var builder = WebApplication.CreateBuilder(args);
var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .Build();
// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.Configure<SmtpConfig>(builder.Configuration.GetSection("SmtpConfig"));
builder.Services.Configure<JwtConfig>(builder.Configuration.GetSection("JwtConfig"));
builder.Services.AddSingleton(sp => sp.GetRequiredService<IOptions<SmtpConfig>>().Value);
builder.Services.AddSingleton(sp => sp.GetRequiredService<IOptions<JwtConfig>>().Value);
builder.Services.AddScoped<IPersonal_Repository, Personal_Repository>();
//builder.Services.AddDbContext<Personal_Context>(options => {
//    options.UseMySql(configuration["mysqlConfig"], ServerVersion.AutoDetect(configuration["mysqlConfig"]));
//    options.EnableSensitiveDataLogging();
//});
builder.Services.AddDbContext<Personal_Context>(options =>
    options.UseMySQL(configuration["mysqlConfig"]));
var app = builder.Build();


var exemptEndpoints = new[]
  {
        "/api/login",
        "/swagger/index.html",
        "/swagger/v1/swagger.json",
        "/api/call/list",
        "/api/call/get"
    };

app.UseMiddleware<TokenValidator>(
    Options.Create(builder.Configuration.GetSection("JwtConfig").Get<JwtConfig>()),
    exemptEndpoints
);
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
