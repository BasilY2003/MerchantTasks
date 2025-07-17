using CommonLib;
using CommonLib.Localization;
using CommonLib.Middlewares;
using DataLib;
using DataLib.Resources;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using NHibernate;
using Serilog;
using System.Globalization;



var builder = WebApplication.CreateBuilder(args);



builder.Services.AddDataLib(builder.Configuration);
builder.Services.AddCommonLib(builder.Configuration);
builder.Services.AddScoped<JwtAuthFilter>();


builder.Services.AddStackExchangeRedisCache(options =>
{
    builder.Configuration.GetSection("Redis").Bind(options);
});


Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();


builder.Host.UseSerilog();
builder.Services.AddLocalization();


builder.Services.AddControllers(options =>
{
    options.Filters.Add<GlobalExceptionFilter>(); 
})
.AddDataAnnotationsLocalization(options =>
{
    options.DataAnnotationLocalizerProvider = (type, factory) =>
        factory.Create(typeof(SharedResource));
});


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    var supportedCultures = new[] { new CultureInfo("en"), new CultureInfo("ar") };

    options.DefaultRequestCulture = new RequestCulture("en");
    options.SupportedCultures = supportedCultures;
    options.SupportedUICultures = supportedCultures;
});


var app = builder.Build();
app.UseRequestLocalization(app.Services.GetRequiredService<IOptions<RequestLocalizationOptions>>().Value);

var localizer = app.Services.GetRequiredService<IStringLocalizer<SharedResource>>();
LocalizedErrorHelper.Configure(localizer);


app.UseSwagger();
app.UseSwaggerUI();
app.UseMiddleware<LoggingMiddleware>(); 
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();