using CloudinaryDotNet;
using dotenv.net;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NetSolutions.Services;
using NetSolutions.WebApi.Data;
using NetSolutions.WebApi.Data.Interceptors;
using NetSolutions.WebApi.Models.Domain;
using NetSolutions.WebApi.Services;
using NetSolutions.WebApi.Tasks;
using System.Text;
using System.Text.Json.Serialization; // Add this using directive

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//✅ Configure JSON serialization to ignore reference loops and not include related entities by default
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        // Ignore cycles in object graphs
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        // Set default behavior to ignore entity references unless explicitly included
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    });

//✅ Register DBContext
string connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new NullReferenceException("DefaultConnection not found!");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(connectionString, sqlServerOptionsAction =>
    {
        sqlServerOptionsAction.EnableRetryOnFailure(3);
        sqlServerOptionsAction.MigrationsAssembly("NetSolutions.WebApi");
    });
    options.EnableDetailedErrors();
    options.EnableSensitiveDataLogging();
    options.AddInterceptors(new ChangesInterceptor());
    // Disable this warning in development for data generation
    options.ConfigureWarnings(warnings =>
    {
        warnings.Ignore(Microsoft.EntityFrameworkCore.Diagnostics.RelationalEventId.PendingModelChangesWarning);
    });
});

//✅Add Identity services
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.User.RequireUniqueEmail = true;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

//✅Bind BusinessProfile from appsettings.json
var businessProfile = builder.Configuration.GetSection(nameof(BusinessProfile)).Get<BusinessProfile>() ?? throw new NullReferenceException("BusinessProfile cannot be null");
builder.Services.AddSingleton(businessProfile);


//✅Add Authentication services
var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>() ?? throw new NullReferenceException("JwtSettings cannot be null");
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuers = jwtSettings.Issuers,
        ValidAudiences = jwtSettings.Audiences,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey))
    };
});

//✅ Add Authorization Middleware
builder.Services.AddAuthorization();

//✅ Register/Configure CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowNetSolutionOrigins",
        builder => builder.WithOrigins(jwtSettings.Audiences)
                          .AllowAnyMethod()
                          .AllowAnyHeader()
                          .AllowCredentials());
});


//✅ SmtpSettings as a Singleton to use it out of the box
builder.Services.AddSingleton(builder.Configuration.GetSection(nameof(SmtpSettings)).Get<SmtpSettings>() ?? throw new Exception("Error registering SmtpSettings"));
//✅ Register EmailSender service
builder.Services.AddTransient<IEmailSender, EmailSender>();

//✅ Register JWT service
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));//Register JWT settings using the Options Pattern
builder.Services.AddSingleton(sp => sp.GetRequiredService<IOptions<JwtSettings>>().Value);//Register JwtSettings as a singleton for direct access if needed
builder.Services.AddScoped<IJasonWebToken, JasonWebToken>();

//✅ Register GoogleDrive service
builder.Services.Configure<GoogleDriveCredentials>(builder.Configuration.GetSection("GoogleDriveCredentials"));// Bind GoogleDriveCredentials from appsettings.json
builder.Services.AddSingleton<IGoogleDrive, GoogleDrive>();

//✅ Register custom ilogger
builder.Services.AddScoped<ILogService, LogService>();

//✅ Register localFileManager service
builder.Services.AddScoped<ILocalFileManager, LocalFileManager>();

//✅ Register HttpClient service
builder.Services.AddHttpClient();

//✅ Register PayFast service
var payFastCreds = builder.Configuration.GetSection("PayFastCreds").Get<PayFastCreds>() ?? throw new InvalidOperationException("Failed to register PayFastCreds");
builder.Services.AddSingleton(payFastCreds);
builder.Services.AddScoped<IPayFast, PayFast>();

////✅ Register Redis Cache
//builder.Services.AddStackExchangeRedisCache(options =>
//{
//    options.Configuration = builder.Environment.IsDevelopment() ? jwtSettings.Issuers : jwtSettings.Issuers; // Your Redis server
//});
//builder.Services.AddHostedService<Redis>();


//// Set your Cloudinary credentials
////=================================
//DotEnv.Load(options: new DotEnvOptions(probeForEnv: true));
//Cloudinary cloudinary = new Cloudinary(Environment.GetEnvironmentVariable("CLOUDINARY_URL"));
//cloudinary.Api.Secure = true;
builder.Services.Configure<CloudinarySettings>(builder.Configuration.GetSection("Cloudinary"));
builder.Services.AddSingleton(sp =>
{
    var config = sp.GetRequiredService<IOptions<CloudinarySettings>>().Value;
    Account account = new Account(config.CloudName, config.ApiKey, config.ApiSecret);
    return new CloudinaryDotNet.Cloudinary(account);
});


var app = builder.Build();

// ✅ Serve static files before controllers
app.UseStaticFiles();

app.UseHttpsRedirection();

app.UseCors("AllowNetSolutionOrigins");

app.UseAuthorization();

app.MapControllers();

// ✅ Swagger for development
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.Run();

