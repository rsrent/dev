using System;
using Microsoft.Extensions.DependencyInjection;
using Rent.Shared.Repositories;
using Rent.Shared.ViewModels;
using RentApp;
using RentApp.Shared.Repositories;
using RentAppProject;

namespace Rent.Shared
{
    public class IocContainer
    {
        //public static IServiceProvider Create() => ConfigureServices();

        public static IServiceProvider AddServices(IServiceCollection services)
        {
            //IServiceCollection services = new ServiceCollection();

            services.AddScoped<Settings>();
            services.AddScoped<HttpClientProvider>();

            services.AddScoped<CleaningTasksRepository>();
            services.AddScoped<CompletedCleaningTasksRepository>();
            services.AddScoped<QualityReportRepository>();
            services.AddScoped<CustomerRepository>();
            services.AddScoped<LocationRepository>();
            services.AddScoped<UserRepository>();
            services.AddScoped<ChatRepository>();
            services.AddScoped<PermissionRepository>();
            services.AddScoped<LoginRepository>();
            services.AddScoped<FloorAreaRepository>();
            services.AddScoped<RoleRepository>();

            services.AddScoped<LogRepository>();
            services.AddScoped<MoreWorkRepository>();
            services.AddScoped<EconomyRepository>();
            services.AddScoped<HoursRepository>();

            services.AddScoped<LoginVM>();
            services.AddScoped<UserVM>();
            services.AddScoped<NotificationToken>();

            /*
            services.AddScoped<CreateTaskVM>();
            services.AddScoped<LocationVM>();
            services.AddScoped<CleaningPlanVM>();
            services.AddScoped<QualityReportVM>();
            services.AddScoped<AddressSearchVM>();
            services.AddScoped<CreateUserVM>();
            services.AddScoped<CreateLocationVM>();
            services.AddScoped<EmployeeTableVM>();
            services.AddScoped<CustomerVM>();
            services.AddScoped<RootViewModel>();

            services.AddScoped<IErrorMessageHandler, ErrorMessageHandler>();


            services.AddScoped<IUserVM, UserVM>();

            services.AddScoped<NotificationToken>();

            services.AddScoped<ImageRepository>();
            services.AddScoped<DocumentRepository>();
            services.AddScoped<MessageHandler>();
            services.AddScoped<StorageRepository>();
            services.AddScoped<iOSImageRepository>();
            services.AddScoped<MyConversationSocketConnection>();


            //STORAGE
            services.AddScoped<AzureStorage>();
            services.AddScoped<AzureCredentials, MyAzureCredentials>();
            services.AddScoped<ILocalStorage<byte[]>, LocalStorage<byte[]>>();
            services.AddScoped<ILocalStorageSettings, MyLocalStorageSettings>();

*/

            return services.BuildServiceProvider();
        }
    }
}
