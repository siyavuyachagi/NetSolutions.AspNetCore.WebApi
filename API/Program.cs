using Application.Interfaces;
using CloudinaryDotNet;
using Domain.Entities;
using Infrastructure.Configurations;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Infrastructure.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using Serilog.Events;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//✅ Configure JSON serialization to ignore reference loops and not include related entities by default
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        // Ignore cycles in object graphs
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
        options.JsonSerializerOptions.MaxDepth = 64;
    });

//✅ Register DBContext
string connectionString = builder.Configuration.GetConnectionString("DbConnection") ?? throw new NullReferenceException("DbConnection not found!");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(connectionString, sqlServerOptionsAction =>
    {
        sqlServerOptionsAction.EnableRetryOnFailure(3);
        sqlServerOptionsAction.MigrationsAssembly("API");
    });
    options.EnableDetailedErrors();
    //options.EnableSensitiveDataLogging();
    //options.AddInterceptors();
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
    options.SignIn.RequireConfirmedEmail = true;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//builder.Services.AddAuthorization(options =>
//{
//    // By default, all incoming requests will be authorized according to the default policy.
//    options.FallbackPolicy = options.DefaultPolicy;
//});

builder.Services.AddAutoMapper(typeof(MappingProfile).Assembly);


//✅ Configure Serilog
builder.Host.UseSerilog();
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    // Debug only
    .WriteTo.Logger(lc => lc
        .Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Debug)
        .WriteTo.File($"Logs/Debug/Debug.log"))
    // Information only
    .WriteTo.Logger(lc => lc
        .Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Information)
        .WriteTo.File($"Logs/Info/Info.log"))
    // Warning only
    .WriteTo.Logger(lc => lc
        .Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Warning)
        .WriteTo.File($"Logs/Warnings/Warnings.log"))
    // Error only
    .WriteTo.Logger(lc => lc
        .Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Error)
        .WriteTo.File($"Logs/Errors/Errors.log"))
    .CreateLogger();

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


builder.Services.AddScoped<IAdministrator, AdministratorRepository>();
builder.Services.AddScoped<IApplicationUser, ApplicationUserRepository>();
builder.Services.AddScoped<IBusinessProfile, BusinessProfileRepository>();
builder.Services.AddScoped<IBusinessService, BusinessServiceRepository>();
builder.Services.AddScoped<IBusinessServicePackage, BusinessServicePackageRepository>();
builder.Services.AddScoped<IClient, ClientRepositotry>();

builder.Services.AddSingleton(builder.Configuration.GetSection(nameof(SmtpSettings)).Get<SmtpSettings>() ?? throw new Exception("Error registering SmtpSettings"));
builder.Services.AddSingleton(builder.Configuration.GetSection(nameof(EmailSettings)).Get<EmailSettings>() ?? throw new Exception("Error registering EmailSettings"));
builder.Services.AddScoped<IEmailSender, EmailSender>();

builder.Services.AddScoped<IFeedback, FeedbackRepository>();
builder.Services.AddScoped<IGuestUser, GuestUserRepository>();

builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection(nameof(JwtSettings)));//Register JWT settings using the Options Pattern
builder.Services.AddSingleton(sp => sp.GetRequiredService<IOptions<JwtSettings>>().Value);//Register JwtSettings as a singleton for direct access if needed
builder.Services.AddScoped<IJasonWebToken, JasonWebToken>();

builder.Services.AddScoped<ILogClearner, LogCleaner>();
builder.Services.AddScoped<IMessages, MessageRepository>();
builder.Services.AddScoped<IOrganization, OrganizationRepository>();

builder.Services.AddSingleton(builder.Configuration.GetSection(nameof(PayFastConfig)).Get<PayFastConfig>() 
    ?? throw new Exception("Error registering PayFastConfig"));
builder.Services.AddScoped<IPayFast, PayFast>();
builder.Services.AddScoped<IPaymentTransaction, PaymentTransactionRepository>();
builder.Services.AddScoped<IProject, ProjectsRepository>();
builder.Services.AddScoped<IProjectTeam, ProjectTeamRepository>();
builder.Services.AddScoped<IProjectTeamMember, ProjectTeamMemberRepository>();
builder.Services.AddScoped<IRazorLightEmailRenderer, RazorLightEmailRenderer>();
builder.Services.AddScoped<ISkills, SkillsRepository>();
builder.Services.AddScoped<ISolution, SolutionRepository>();
builder.Services.AddScoped<IStaff, StaffRepository>();
builder.Services.AddScoped<ISubscription, SubscriptionRepository>();
builder.Services.AddScoped<ITechnologyStack, TechnologyStackRepository>();

builder.Services.Configure<CloudinarySettings>(builder.Configuration.GetSection(nameof(CloudinarySettings)));
builder.Services.AddSingleton(sp =>
{
    var config = sp.GetRequiredService<IOptions<CloudinarySettings>>().Value;
    Account account = new Account(config.CloudName, config.ApiKey, config.ApiSecret);
    return new Cloudinary(account);
});

builder.Services.AddSingleton(builder.Configuration.GetSection(nameof(GithubConfig)).Get<GithubConfig>() ?? throw new Exception("Error registering GithubConfig"));

builder.Services.AddHostedService<LogCleanerWorker>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();

    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.UseCors("AllowNetSolutionOrigins");

app.UseStaticFiles();

app.Run();