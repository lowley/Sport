using PWA5.ApplicationFiles;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSignalR().AddAzureSignalR();

var app = builder.Build();

app.UseDefaultFiles();
app.UseRouting();
app.UseStaticFiles();
app.MapHub<MapPinHub>("/mappinhub");

app.UseHttpsRedirection();

app.MapGet("/test", () =>
{
    return "OK!";
});

app.Run();

