using ReportService.Structures.Reports.ExchangeRateOnDateRange;
using ReportService.Structures.Reports.PaymentOnDateRange;
using ReportService.Structures.Reports.SalarySummary;

string _exchangeRateApiConnectionString = "https://localhost:44341/api/ExchangeRate";
 string _employeeApiConnectionString = "https://localhost:44316/api/EmployeeService";
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton(new ExchangeRateOnDateRangeReportBuilder(_exchangeRateApiConnectionString));
builder.Services.AddSingleton(new SalarySummaryReportBuilder(_employeeApiConnectionString, _exchangeRateApiConnectionString));
builder.Services.AddSingleton(new PaymentOnDateRangeReportBuilder(_employeeApiConnectionString, _exchangeRateApiConnectionString));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
