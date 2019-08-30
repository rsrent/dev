using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.WebSockets;
using Rent.Data;
using System.Net.WebSockets;
using Microsoft.AspNetCore.Http;
using System.Threading;
using Rent.Chat;
using System.Reflection;
using Swashbuckle.AspNetCore.Swagger;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Rent.Authorization;
using Rent.Helpers;
using Rent.Repositories;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Rewrite;
using Newtonsoft.Json.Serialization;
using Rent.Scheduler;
using Rent.PushNotifications;
using Rent.ContextPoint;
using Rent.Models;
using Rent.DTOAssemblers;
using Rent.Repositories.TimePlanning;
using Newtonsoft.Json;

namespace Rent
{
    public class Startup
    {
        public static bool Live = true;

        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            _configuration = configuration;
            Environment = env;
        }

        public IConfiguration _configuration { get; }
        public IHostingEnvironment Environment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<NotificationRepository>();

            services.AddScoped<EmailNotificationChannel>();
            services.AddScoped<SendGridEmail>();
            services.AddScoped<IEmailNotificationChannelSettings, MyEmailNotificationChannelSettings>();
            services.AddScoped<PushNotificationController>();

            services.AddScoped<LocationRepository>();
            services.AddScoped<ChatRepository>();
            services.AddScoped<DocumentRepository>();
            services.AddScoped<RatingRepository>();
            services.AddScoped<CleaningTaskRepository>();
            services.AddScoped<QualityReportRepository>();

            services.AddScoped<UserContext>();
            services.AddScoped<CustomerContext>();
            services.AddScoped<LocationContext>();
            services.AddScoped<LocationUserContext>();
            services.AddScoped<QualityReportContext>();
            services.AddScoped<QualityReportItemContext>();
            services.AddScoped<CleaningTaskContext>();

            services.AddScoped<UserPermissionContext>();
            services.AddScoped<TemplatePermissionContext>();
            services.AddScoped<RoleContext>();
            services.AddScoped<PermissionContext>();
            services.AddScoped<LoginContext>();

            services.AddScoped<UserRepository>();
            services.AddScoped<TokenRepository>();
            services.AddScoped<LoginRepository>();
            services.AddScoped<CustomerRepository>();
            services.AddScoped<PermissionRepository>();
            services.AddScoped<DashboardRepository>();
            services.AddScoped<EconomyRepository>();
            services.AddScoped<NewsRepository>();
            services.AddScoped<PropCondition>();

            /*
             Things added to the service container for time planning project
             */

            services.AddScoped<IAgreementRepository, AgreementRepository>();
            services.AddScoped<IAbsenceRepository, AbsenceRepository>();
            services.AddScoped<ContractRepository>();
            services.AddScoped<ContractAssembler>();
            services.AddScoped<AbsenceAssembler>();
            services.AddScoped<IRoleAuthenticationRepository, RoleAuthenticationRepository>();
            services.AddScoped<IRequestRepository, RequestRepository>();
            services.AddScoped<AbsenceReasonRepository>();
            services.AddScoped<AbsenceHelperRepository>();


            services.AddScoped<UserListDTOAssembler>();
            services.AddScoped<UserListDTOAssembler>();


            services.AddScoped<WorkContractListAssembler>();
            services.AddScoped<WorkListDTOAssembler>();
            services.AddScoped<WorkReplacementDTOAssembler>();

            services.AddScoped<WorkContractRepository>();
            services.AddScoped<WorkRepository>();
            services.AddScoped<WorkRegistrationRepository>();

            services.AddScoped<ProjectRepository>();


            services.AddScoped<HolidayRepository>();
            services.AddScoped<AgreementAssembler>();
            services.AddScoped<PostRepository>();
            services.AddScoped<AccidentReportRepository>();

            services.AddScoped<RequestRepository>();
            services.AddScoped<TaskCompletedRepository>();
            services.AddScoped<WorkInvitationRepository>();

            /*
            Things added to the service container for time planning project
            */

            services.AddScoped<FirestoreCommunicationRepository>();
            services.AddScoped<FirebaseNotificationRepository>();
            services.AddScoped<NotiRepository>();
            services.AddScoped<CommentRepository>();
            services.AddScoped<AddressRepository>();
            services.AddScoped<ProjectRoleRepository>();
            services.AddScoped<ClientRepository>();



            services.AddScoped<CleaningTaskContext.Rules>();
            services.AddScoped<UserContext.Rules>();
            services.AddScoped<CustomerContext.Rules>();
            services.AddScoped<LocationContext.Rules>();
            services.AddScoped<CleaningTaskContext.Rules>();
            services.AddScoped<QualityReportContext.Rules>();

            services.AddScoped<IStorage, AzureStorageRepository>();
            if (Environment.IsDevelopment())
                services.AddScoped<AzureCredentials, AzureDevCredentials>();
            else
                services.AddScoped<AzureCredentials, MyAzureCredentials>();

            if (Live)
                services.AddScoped<IPhoneNotificationSettings, LivePhoneNotificationSettings>();
            else
                services.AddScoped<IPhoneNotificationSettings, DevPhoneNotificationSettings>();

            services.AddWebSocketManager();
            services.AddDbContext<RentContext>(options =>
                                               //options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
                                               options.UseSqlServer(_configuration.GetConnectionString("DevelopmentConnection")));
            //options.UseSqlServer(Configuration.GetConnectionString(Environment.IsDevelopment() ? "DevelopmentConnection" : "DefaultConnection")));
            services.AddSingleton<IConfiguration>(_configuration);

            services.AddOptions();

            services.Configure<SymmetricKey>(_configuration.GetSection("SymmetricKey")); // Gets the key from appsettings.json
            var key = _configuration["SymmetricKey:Key"];
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(jwtBearerOptions =>
                {
                    jwtBearerOptions.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
                        ValidateIssuer = false,
                        ValidIssuer = "RentAPP",
                        ValidateLifetime = false, //validate the expiration and not before values in the token
                        ClockSkew = TimeSpan.FromMinutes(5), //5 minute tolerance for the expiration date
                        ValidateActor = false,
                        ValidateAudience = false,
                    };
                });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("tokenExpired", policy => { policy.Requirements.Add(new AuthRequirement()); });

            });

            //*
            services.AddSingleton<IAuthorizationHandler, LoginAuthorizationHandler>();
            services.AddMvc(config =>
            {
                config.Filters.Add(new AuthorizeFilter("tokenExpired"));
            }).SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            //*/

            //services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);



            if (Environment.IsDevelopment())
            {
                services.AddCors(o => o.AddPolicy("MyPolicy", builder =>
                {
                    builder.AllowAnyOrigin()
                           .AllowAnyMethod()
                           .AllowAnyHeader();
                }));
            }

            services.AddHttpsRedirection(options =>
            {
                options.RedirectStatusCode = StatusCodes.Status301MovedPermanently;
                options.HttpsPort = 5001;
            });

            services.AddMvc()
                .AddJsonOptions(options =>
                {
                    options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                    options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                });



            services.AddSingleton<IScheduledTask, CleaningTaskReport>();
            services.AddSingleton<IScheduledTask, UpdatePasswordsSchedule>();
            services.AddScheduler((sender, args) =>
            {
                Console.Write(args.Exception.Message);
                args.SetObserved();
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider serviceProvider)
        {
            app.UseAuthentication();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseCors("MyPolicy");
            }
            else
            {
                app.UseHsts();
            }


            app.UseHttpsRedirection();

            app.UseDefaultFiles();
            app.UseStaticFiles(new StaticFileOptions()
            {
                OnPrepareResponse = context =>
                {
                    context.Context.Response.Headers.Add("Cache-Control", "no-cache, no-store");
                    context.Context.Response.Headers.Add("Expires", "-1");
                }
            });

            app.UseMvc();

            // Accept web socket requests
            app.UseWebSockets();
            app.MapWebSocketManager("/chat", serviceProvider.GetService<ChatMessageHandler>());
        }
    }

    public static class Extensions
    {
        public static IApplicationBuilder MapWebSocketManager(this IApplicationBuilder app,
                                                              PathString path,
                                                              WebSocketHandler handler)
        {
            return app.Map(path, (_app) => _app.UseMiddleware<WebSocketManagerMiddleware>(handler));
        }

        public static IServiceCollection AddWebSocketManager(this IServiceCollection services)
        {
            services.AddScoped<WebSocketConnectionManager>();

            foreach (var type in Assembly.GetEntryAssembly().ExportedTypes)
            {
                if (type.GetTypeInfo().BaseType == typeof(WebSocketHandler))
                {
                    services.AddScoped(type);
                }
            }

            return services;
        }
    }
}