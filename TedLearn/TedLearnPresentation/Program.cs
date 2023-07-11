using WebConfig.Configurations;
using WebConfig.CustomMappings.Configurations;
using WebConfig.IoC;
using WebConfig.Middlewares;
using WebMarkupMin.AspNetCore6;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddTedLearnContext(builder.Configuration);
builder.Services.AddMarkupMin();
builder.Services.AddWebAuthentication();
builder.Host.AddSerilog();
AutoMapperConfiguration.InitializeAutoMapper();

builder.Services.AddCustomConfigurations();
builder.Services.RegisterServices();

var app = builder.Build();

app.UseNotFoundHandler();
app.UsePurchacableCoursesHandler();

if (app.Environment.IsDevelopment())
    app.UseDeveloperExceptionPage();
else
{
    app.UseCustomExceptionHandler();
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseWebMarkupMin();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "MyArea",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
