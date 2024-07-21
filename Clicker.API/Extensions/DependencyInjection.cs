using System.Net.Http.Headers;
using System.Text;
using System.Transactions;
using Clicker.API.Filters;
using Clicker.BL.Abstractions;
using Clicker.BL.Implementations;
using Clicker.DAL.Data;
using Clicker.Domain.Abstractions;
using Clicker.Domain.Constants;
using Clicker.Domain.Implementations;
using Hangfire;
using Hangfire.MySql;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Clicker.API.Extensions;

public static class DependencyInjection
{
   public static void ConfigureServices(this IServiceCollection services, IConfiguration configuration)
    { 
        services.AddScoped(typeof(IBaseEntityRepository<>), typeof(BaseEntityRepository<>)); 
        services.AddScoped<IUserChannelSubscriptionTaskRepository, UserChannelSubscriptionTaskRepository>();
        services.AddScoped<IUserOfferSubscriptionTaskRepository, UserOfferSubscriptionTaskRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<ICurrentUserService, CurrentUserService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IChannelSubscriptionTaskService, ChannelSubscriptionTaskService>();
        services.AddScoped<IOfferSubscriptionTaskService, OfferSubscriptionTaskService>();
        services.AddScoped<IUserChannelTaskService, UserChannelTaskService>();
        services.AddScoped<IUserOfferTaskService, UserOfferTaskService>();
        services.AddScoped<IEnergyRefillService, EnergyRefillService>();
        services.Configure<SecurityHttpClientConstants>(configuration.GetSection("SecurityHttpClient"));
        services.Configure<TelegramBotClientConstants>(configuration.GetSection("TelegramBotClient"));
        services.AddLogging();
        services.AddHttpContextAccessor();
        services.AddHttpClient();
        services.AddControllers(options =>
        {
            options.Filters.Add<GlobalExceptionHandlingFilter>();
        });
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        services.AddScoped<GlobalExceptionHandlingFilter>();
    }
    public static void ConfigureSqlConnection(this IServiceCollection services, WebApplicationBuilder builder)
    {
        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
        services.AddDbContext<ApplicationDbContext>(opts =>
            opts.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));
       
    }
    public static void ConfigureHangfireConnection(this IServiceCollection services, WebApplicationBuilder builder)
    {
        var connectionString = builder.Configuration.GetConnectionString("HangfireConnection");
        services.AddHangfire(config =>
            config.UseStorage(new MySqlStorage(
                connectionString, 
                new MySqlStorageOptions
                {
                    TransactionIsolationLevel = IsolationLevel.ReadCommitted, 
                    QueuePollInterval = TimeSpan.FromSeconds(15), 
                    JobExpirationCheckInterval = TimeSpan.FromHours(1), 
                    CountersAggregateInterval = TimeSpan.FromMinutes(5), 
                    PrepareSchemaIfNecessary = true, 
                    DashboardJobListLimit = 5000, 
                    TransactionTimeout = TimeSpan.FromMinutes(1)
                })));
        services.AddHangfireServer();
    }
    public static void ConfigureCors(this IServiceCollection services) 
    {
        services.AddCors(options => {
            options.AddPolicy("AllowAny", builder =>
            {
                builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            });
        });
    }
    public static void ConfigureAuth(this IServiceCollection services, IConfiguration configuration) {
        var secretKey = configuration["Auth:SecretKey"];
        if (string.IsNullOrEmpty(secretKey))
        {
            throw new ArgumentNullException(nameof(secretKey), "JWT Secret Key is not configured.");
        }

        services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options => {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters {
                    ValidateIssuer = true,
                    ValidIssuer = configuration["Auth:Issuer"] ?? throw new ArgumentNullException("Auth:Issuer"),

                    ValidateAudience = true,
                    ValidAudience = configuration["Auth:Audience"] ?? throw new ArgumentNullException("Auth:Audience"),

                    ValidateLifetime = true,

                    ValidateIssuerSigningKey = true,
                    ClockSkew = TimeSpan.Zero,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secretKey)),
                };
            });
    }

    public static void ConfigureHttpClient(this IServiceCollection services, IConfiguration configuration) {
        services.AddHttpClient(configuration["SecurityHttpClient:ClientName"], client => {
            client.BaseAddress = new Uri(configuration["SecurityHttpClient:BaseAddress"]);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/problem+json"));
            client.DefaultRequestHeaders.AcceptCharset.Add(new StringWithQualityHeaderValue("utf-8"));
        });
    }
    
}