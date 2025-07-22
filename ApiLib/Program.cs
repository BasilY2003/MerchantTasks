using CommonLib;
using CommonLib.Localization;
using CommonLib.Middlewares;
using DataLib;
using DataLib.Resources;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
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

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
options.InvalidModelStateResponseFactory = context =>
{
    var errors = context.ModelState
        .Where(ms => ms.Value?.Errors.Count > 0)
        .Select(ms => new
        {
            Field = ms.Key,
            Messages = ms.Value!.Errors.Select(e => e.ErrorMessage).ToArray()
        }).ToList();

    var errorResponse = new ErrorResponse
    {
        ErrorCode = ErrorCode.InvalidRequest,
        ErrorMessage = LocalizedMessage.GetMessage("ValidationFailed"), // e.g. "Validation failed"
        Details = errors
    };
    return new BadRequestObjectResult(errorResponse);
};
});


var app = builder.Build();
app.UseRequestLocalization(app.Services.GetRequiredService<IOptions<RequestLocalizationOptions>>().Value);

var localizer = app.Services.GetRequiredService<IStringLocalizer<SharedResource>>();
LocalizedMessage.Configure(localizer);

app.UseSwagger();
app.UseSwaggerUI();
app.UseMiddleware<LoggingMiddleware>(); 
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();