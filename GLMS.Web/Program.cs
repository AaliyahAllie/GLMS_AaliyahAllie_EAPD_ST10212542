using GLMS.Web.Data;
using GLMS.Web.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddHttpClient();

builder.Services.AddSession();

builder.Services.AddScoped<ICurrencyService, CurrencyService>();
builder.Services.AddScoped<IFileValidationService, FileValidationService>();
builder.Services.AddScoped<IContractWorkflowService, ContractWorkflowService>();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Public}/{action=Index}/{id?}");

app.Run();