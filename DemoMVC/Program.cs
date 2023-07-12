using DemoMVC.Models;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Routing.Constraints;
using Microsoft.EntityFrameworkCore;
using System;
using System.Text.RegularExpressions;

var builder = WebApplication.CreateBuilder(args);
// This method gets called by the runtime. Use this method to add services to the container.
var connectString = builder.Configuration.GetConnectionString("AppMvcConnectionString");
builder.Services.AddDbContext<AppDbContext>(options => {
    options.UseSqlServer(connectString);
});
// Add services to the container.
builder.Services.AddControllersWithViews();
// add forder save view.htnl
builder.Services.Configure<RazorViewEngineOptions>(options => {

    // Tìm thêm View ở /MyView/ControllerName/ActionName.cshtml
    options.ViewLocationFormats.Add("/MyView/{1}/{0}" + RazorViewEngine.ViewExtension);

});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();
// Tạo các Endpoint
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

    endpoints.MapControllerRoute(
        name: "myroute",
        defaults: new { controller = "Home", action = "Index" },
        constraints: new
        {
            id= @"\d+",                                                    // id pha co va la so
            title = new RegexRouteConstraint(new Regex(@"^[a-z0-9-]*$"))   //title chi chua so, chu thuong va ki hieu
        },
        pattern: "{title:alpha:maxlength(8)}-{id:int}.html");
});

app.Run();
