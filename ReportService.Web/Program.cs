using ReportService.Web.DataReaders.EmployeeServiceReaders;
using ReportService.Web.DataReaders.ReportServiceReaders;



var builder = WebApplication.CreateBuilder(args);

string _reportServiceUrl = builder.Configuration.GetValue<string>("ReportServiceUrl");
string _employeeServiceUrl = builder.Configuration.GetValue<string>("EmployeeServiceUrl");
// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSingleton(new ExchangeRateOnDateRangeReportReader(_reportServiceUrl));
builder.Services.AddSingleton(new PaymentOnDateRangeReportReader(_reportServiceUrl));
builder.Services.AddSingleton(new SalarySummaryReportReader(_reportServiceUrl));
builder.Services.AddSingleton(new CurrencyReader(_employeeServiceUrl));
builder.Services.AddSingleton(new OfficeReader(_employeeServiceUrl));
builder.Services.AddSingleton(new CountryReader(_employeeServiceUrl));
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

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
