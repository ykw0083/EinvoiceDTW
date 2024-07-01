using EinvoiceDTW;
using EinvoiceDTW.Data;
using EinvoiceDTW.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using OfficeOpenXml;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSingleton<WeatherForecastService>();
// Register EPPlus
builder.Services.AddSingleton<IFileUploadService, FileUploadService>();
// Register database context
builder.Services.AddTransient<IDatabaseContext, DatabaseContext>();
// Register data service
builder.Services.AddSingleton<IDataService, DataService>();

var app = builder.Build();
ExcelPackage.LicenseContext = LicenseContext.Commercial;

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}


app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
