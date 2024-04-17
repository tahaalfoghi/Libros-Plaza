using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MvcApp.EFCore.Data;
using MvcApp.EFCore.Models;
using MvcApp.Services;
using MvcApp.Utility;



var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<AppDbContext>(op => op.UseSqlServer(
    builder.Configuration.GetConnectionString("constr")
    ));
builder.Services.AddRazorPages();

builder.Services.AddIdentity<IdentityUser,IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(op =>
{
    op.LoginPath = $"/Identity/Account/Login";
    op.LogoutPath = $"/Identity/Account/Logout";
    op.AccessDeniedPath = $"/Identity/Account/AccessDenied";
});
builder.Services.AddScoped(typeof(IRepository<>),typeof(RepositoryBase<>));
builder.Services.AddScoped(typeof(IUnitOfWork),typeof(UnitOfWork));
builder.Services.AddScoped<IEmailSender,EmailSender>();
builder.Services.AddScoped(typeof(CategoryRepository));
builder.Services.AddScoped(typeof(BookRepository));
builder.Services.AddScoped(typeof(OrderRepository));
builder.Services.AddScoped(typeof(OrderDetailsRepository));
builder.Services.AddScoped(typeof(ShoppingCartRepository));
builder.Services.AddAuthentication();
builder.Services.AddAuthorization();
var app = builder.Build();


if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles(); 

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{area=Customer}/{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();
app.Run();
