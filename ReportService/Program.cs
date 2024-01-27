using ReportService.Structures.Reports.ExchangeRateOnDateRange;
using ReportService.Structures.Reports.PaymentOnDateRange;
using ReportService.Structures.Reports.SalarySummary;


var builder = WebApplication.CreateBuilder(args);


string _exchangeRateApiUrl = builder.Configuration.GetValue<string>("ExchangeRateServiceUrl"); 
string _employeeApiUrl = builder.Configuration.GetValue<string>("EmployeeServiceUrl"); 
// Add services to the container.

builder.Services.AddControllers().AddJsonOptions(opts =>
{
    opts.JsonSerializerOptions.Converters.Add(new ExchangeRateOnDateRangeJsonConverter());
    opts.JsonSerializerOptions.Converters.Add(new PaymentOnDateRangeJsonConverter());
    opts.JsonSerializerOptions.Converters.Add(new SalarySummaryReportJsonConverter());
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton(new ExchangeRateOnDateRangeReportBuilder(_exchangeRateApiUrl));
builder.Services.AddSingleton(new SalarySummaryReportBuilder(_employeeApiUrl, _exchangeRateApiUrl));
builder.Services.AddSingleton(new PaymentOnDateRangeReportBuilder(_employeeApiUrl, _exchangeRateApiUrl));

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
