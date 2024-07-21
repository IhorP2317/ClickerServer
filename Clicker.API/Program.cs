using Clicker.API.Extensions;
using Clicker.BL.Abstractions;
using Hangfire;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureSqlConnection(builder);
builder.Services.ConfigureServices(builder.Configuration);
builder.Services.ConfigureHangfireConnection(builder);
builder.Services.ConfigureCors();
builder.Services.ConfigureAuth(builder.Configuration);
builder.Services.ConfigureHttpClient(builder.Configuration);




var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowAny");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.UseHangfireDashboard("/hangfire"); 
RecurringJob.AddOrUpdate<IEnergyRefillService>(
    "RefillEnergyJob",
    service => service.RefillEnergyAsync(CancellationToken.None),
    "0 0 * * *" 
);
app.Run();
