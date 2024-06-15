using pdf_markdown_manager.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<DocumentsManagerContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("markdown_pdf")));
builder.Services.AddDbContext<IdentityDbManager>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("markdown_pdf")));

builder.Services.AddDefaultIdentity<AuthUser>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<IdentityDbManager>().AddDefaultUI();
//builder.Services.AddIdentityCore<IdentityUser>().AddEntityFrameworkStores<IdentityDbManager>().AddApiEndpoints();
// builder.Services.AddTransient<IDocumentsRepository, DocumentsRepository>();

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

app.MapIdentityApi<IdentityUser>();

app.MapRazorPages();

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Documents}/{action=Index}/{id?}");

app.Run();
