using WebConfig.IoC;
using WebMarkupMin.AspNetCore6;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
builder.Services.AddTedLearnContext(builder.Configuration);
builder.Services.AddMarkupMin();
builder.Services.AddWebAuthentication();

var app = builder.Build();

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
