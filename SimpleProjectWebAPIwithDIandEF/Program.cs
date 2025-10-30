using SimpleProjectWebAPIwithDIandEF.Configuration;
using Serilog;
using SimpleProjectWebAPIwithDIandEF.CustomMiddleware;

var builder = WebApplication.CreateBuilder(args);
///////////*******************************
/////////// DON'T FORGET TO LOGIN WITH OTP
///////////*******************************

// Add services to the container.

builder.Host.UseSerilog((HostBuilderContext context, IServiceProvider services, LoggerConfiguration loggerConfiguration) => {

    loggerConfiguration
    .ReadFrom.Configuration(context.Configuration) //read configuration settings from built-in IConfiguration
    .ReadFrom.Services(services); //read out current app's services and make them available to serilog
});
builder.Services.ConfigureServices(builder.Configuration);

var app = builder.Build();
// to get http request and response details in logs 
app.UseHttpLogging();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()|| app.Environment.IsProduction())
{
    //app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseStaticFiles();
app.UseHttpsRedirection();
app.UseRouting();
app.UseCors();
//app.UseMiddleware<GenerateJwtTokenAutomaticallyByRefreshTokenMiddleware>();
app.UseAuthentication();

app.UseAuthorization();
app.MapControllers();

app.Run();
