using System;
using System.IO;
using System.Linq;
using System.Reflection;
using DocumentManagement.Api.Helpers;
using DocumentManagement.API.Helpers.Mapping;
using DocumentManagement.Data;
using DocumentManagement.Data.Dto;
using DocumentManagement.Domain;
using DocumentManagement.Helper;
using DocumentManagement.MediatR;
using DocumentManagement.Repository;
using FluentValidation;
using Hangfire;
using Hangfire.Dashboard;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;

namespace DocumentManagement.API;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
        var connectionString = Configuration.GetConnectionString("DocumentDbConnectionString");
        var assembly = AppDomain.CurrentDomain.Load("DocumentManagement.MediatR");
        var defaultUserId = Configuration.GetSection("DefaultUser").GetValue<Guid>("DefaultUserId");
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(assembly));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        services.AddValidatorsFromAssemblies(Enumerable.Repeat(assembly, 1));

        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        JwtSettings settings;
        settings = GetJwtSettings();
        services.AddSingleton(settings);

        services.AddSingleton(new PathHelper(Configuration));
        services.AddSingleton<IConnectionMappingRepository, ConnectionMappingRepository>();
        services.AddScoped(c => new UserInfoToken() { Id = defaultUserId });
        //services.AddDbContextPool<DocumentContext>(options =>
        //{
        //    var mysqlVersion = Configuration["MysqlVersion"];
        //    var serverVersion = new MySqlServerVersion(Version.Parse(mysqlVersion));
        //    options.UseMySql(connectionString, serverVersion).EnableSensitiveDataLogging();
        //    options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        //    options.ConfigureWarnings(builder =>
        //    {
        //        builder.Ignore(CoreEventId.PossibleIncorrectRequiredNavigationWithQueryFilterInteractionWarning);
        //    });
        //});
        services.AddDbContextPool<DocumentContext>(options =>
        {
            var serverVersion = ServerVersion.AutoDetect(connectionString);

            options.UseMySql(connectionString, serverVersion, mysqlOptions =>
            {
                mysqlOptions.EnableRetryOnFailure(
                    maxRetryCount: 2, // Maximum number of retry attempts
                    maxRetryDelay: TimeSpan.FromSeconds(10), // Maximum delay between retries
                    errorNumbersToAdd: null
                 );
            })
            .EnableSensitiveDataLogging();

            options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            options.ConfigureWarnings(builder =>
            {
                builder.Ignore(CoreEventId.PossibleIncorrectRequiredNavigationWithQueryFilterInteractionWarning);
            });
        });
        services.AddIdentity<User, Role>()
         .AddEntityFrameworkStores<DocumentContext>()
         .AddDefaultTokenProviders();

        services.Configure<IdentityOptions>(options =>
        {
            options.Password.RequireDigit = false;
            options.Password.RequiredLength = 5;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireUppercase = false;
            options.Password.RequireLowercase = false;
        });
        services.AddSingleton(MapperConfig.GetMapperConfigs());
        services.AddDependencyInjection();
        services.AddJwtAutheticationConfiguration(settings);
        services.AddCors(options =>
        {
            options.AddPolicy("ExposeResponseHeaders",
                builder =>
                {

                    var pathHelper = services.BuildServiceProvider().GetService<PathHelper>();
                    builder.WithOrigins(pathHelper.CorsUrls)
                           .WithExposedHeaders("X-Pagination", "LicenseKey", "PurchaseCode")
                           .AllowAnyHeader()
                           .AllowCredentials()
                           .WithMethods("POST", "PUT", "PATCH", "GET", "DELETE")
                           .SetIsOriginAllowed(host => true);
                });
        });

        services.AddSignalR();
        services.Configure<IISServerOptions>(options =>
        {
            options.AutomaticAuthentication = false;
        });

        services.Configure<FormOptions>(x =>
        {
            x.ValueLengthLimit = int.MaxValue;
            x.MultipartBodyLengthLimit = long.MaxValue; // In case of multipart
        });

        services.AddResponseCompression(options =>
        {
            options.Providers.Add<GzipCompressionProvider>();
        });

        services.AddControllers()
            .AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                options.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
            });
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "v1",
                Title = "Document Management API"
            });

            c.SwaggerDoc("intranet", new OpenApiInfo
            {
                Version = "1.0",
                Title = "OUK DMS — Intranet Integration API",
                Description = "Read-only API for the external intranet application. \r\n\r\n" +
                    "**Base URL:** `https://srv2.ouk.ac.ke/api/intranet` \r\n\r\n" +
                    "**Authentication:** Every request must include the intranet API key in the " +
                    "`X-Api-Key` request header. \r\n\r\n" +
                    "```\r\nX-Api-Key: <your-intranet-api-key>\r\n``` \r\n\r\n" +
                    "Only documents that have been explicitly flagged as \"Accessible on Intranet\" " +
                    "in the DMS are exposed. Non-flagged documents return `404 Not Found` regardless " +
                    "of their id. Missing or invalid keys return `401 Unauthorized`.",
                TermsOfService = new Uri("https://srv2.ouk.ac.ke"),
                Contact = new OpenApiContact
                {
                    Name = "OUK ICT",
                    Email = "ict@ouk.ac.ke",
                    Url = new Uri("https://srv2.ouk.ac.ke")
                }
            });

            // The intranet document only contains the intranet endpoints, so it
            // can be shared cleanly with the intranet development team.
            c.DocInclusionPredicate((docName, apiDescription) =>
            {
                if (docName == "v1")
                {
                    return true;
                }
                var relativePath = apiDescription.RelativePath ?? string.Empty;
                return relativePath.StartsWith("api/intranet", StringComparison.OrdinalIgnoreCase);
            });

            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\"",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer"
            });
            c.AddSecurityRequirement(new OpenApiSecurityRequirement {
               {
                 new OpenApiSecurityScheme
                 {
                   Reference = new OpenApiReference
                   {
                     Type = ReferenceType.SecurityScheme,
                     Id = "Bearer"
                   }
                  },
                  new string[] { }
                }
              });

            c.AddSecurityDefinition(IntranetApiKeyAuthenticationHandler.SchemeName, new OpenApiSecurityScheme
            {
                Description = "API key used by the intranet application. Enter the key in the text input below.\r\n\r\nExample: \"my-intranet-key\"",
                Name = IntranetApiKeyAuthenticationHandler.ApiKeyHeaderName,
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey
            });

            // Intranet operations always use the X-Api-Key scheme, regardless of
            // the document they appear in.
            c.OperationFilter<IntranetSwaggerSecurityOperationFilter>();

            //Set the comments path for the Swagger JSON and UI.
            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            c.IncludeXmlComments(xmlPath);

            // Include DTO documentation (DocumentManagement.Data) so schemas are
            // described in the generated JSON and UI.
            var dataXmlPath = Path.Combine(AppContext.BaseDirectory, "DocumentManagement.Data.xml");
            if (File.Exists(dataXmlPath))
            {
                c.IncludeXmlComments(dataXmlPath);
            }
        });


        //var jobService = sp.GetService<JobService>();
        //jobService.StartScheduler();
        SpaStartup.ConfigureServices(services);
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseExceptionHandler(appBuilder =>
            {
                appBuilder.Run(async context =>
                {
                    var exceptionHandlerFeature = context.Features.Get<IExceptionHandlerFeature>();
                    if (exceptionHandlerFeature != null)
                    {
                        var logger = loggerFactory.CreateLogger("Global exception logger");
                        logger.LogError(500,
                            exceptionHandlerFeature.Error,
                            exceptionHandlerFeature.Error.Message);
                    }
                    context.Response.StatusCode = 500;
                    await context.Response.WriteAsync("An unexpected fault happened. Try again later.");
                });
            });
        }
        app.UseSwagger(c =>
        {
            c.SerializeAsV2 = true;
        });
        app.UseSwaggerUI(c =>
        {
            c.DefaultModelsExpandDepth(-1);
            c.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.List);
            c.SwaggerEndpoint($"v1/swagger.json", "Document Management API");
            c.SwaggerEndpoint($"intranet/swagger.json", "Intranet Integration API");
            c.RoutePrefix = "swagger";
        });
        app.UseStaticFiles();

        app.UseCors("ExposeResponseHeaders");

        app.UseForwardedHeaders(new ForwardedHeadersOptions
        {
            ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
        });

        //app.UseHttpsRedirection();
        app.UseAuthentication();
        app.UseRouting();
        app.UseAuthorization();
        app.UseResponseCompression();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
            endpoints.MapHub<UserHub>("/userHub");
        });
        app.UseHangfireDashboard("/hangfire", new DashboardOptions
        {
            Authorization = new[] { new AllowAllUsers() } // Allow custom authorization
        });
        app.UseForwardedHeaders(new ForwardedHeadersOptions
        {
            ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
        });
        SpaStartup.Configure(app);
    }

    public JwtSettings GetJwtSettings()
    {
        JwtSettings settings = new JwtSettings();

        settings.Key = Configuration["JwtSettings:key"];
        settings.Audience = Configuration["JwtSettings:audience"];
        settings.Issuer = Configuration["JwtSettings:issuer"];
        settings.MinutesToExpiration =
         Convert.ToInt32(
            Configuration["JwtSettings:minutesToExpiration"]);

        return settings;
    }
}


public class AllowAllUsers : IDashboardAuthorizationFilter
{
    public bool Authorize(DashboardContext context)
    {
        return true; // Allow everyone to see the Hangfire Dashboard
    }
}
