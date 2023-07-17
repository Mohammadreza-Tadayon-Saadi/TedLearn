using FluentValidation.AspNetCore;
using WebConfig.Configurations;
using Services.CustomMappings.Configurations;
using WebConfig.IoC;
using WebConfig.Middlewares;
using WebMarkupMin.AspNetCore6;
using Services.Contracts.Services;

var builder = WebApplication.CreateBuilder(args);

var servicesAssembly = typeof(BaseServices<>).Assembly;
builder.Services.AddControllersWithViews()
    .AddFluentValidation(options =>
    {
        options.AutomaticValidationEnabled = true;
        options.RegisterValidatorsFromAssembly(servicesAssembly);
        options.DisableDataAnnotationsValidation = true;
        options.LocalizationEnabled = true;
    });

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
