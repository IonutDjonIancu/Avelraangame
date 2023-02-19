using Avelraangame;
using Avelraangame.Factories;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

#region logger
var path = $"{Directory.GetCurrentDirectory()}\\Resources\\LogFiles\\Logs.txt"; // TODO change location
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.File(path, rollingInterval: RollingInterval.Day)
    .CreateLogger();
#endregion

#region cors
const string myAllowSpecificOrigins = "allowSpecificOrigins";
const string av_diceRoller_app = "https://avelraandiceroller.web.app";
const string av_client_app_local = "http://localhost:4200";
//const string av_client_app = ""; // TODO: to replace this url with app production url

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: myAllowSpecificOrigins,
        builder =>
        {
            builder.WithOrigins
                (
                    av_diceRoller_app,
                    av_client_app_local
                )
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});
#endregion

#region services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IAppSettingsConfigManager, AppSettingsConfigManager>();
builder.Services.AddSingleton<IFactoryManager, FactoryManager>();
#endregion

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(myAllowSpecificOrigins);

app.UseAuthorization();

app.MapControllers();

app.Run();
