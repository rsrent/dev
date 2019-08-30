using System;
using Microsoft.Extensions.DependencyInjection;
using ModuleLibrary.Shared.Storage;
using ModuleLibraryiOS.Storage;
using Rent.Shared;
using Rent.Shared.ViewModels;
using RentApp.Repository;
using RentApp.Shared.Models;
using RentApp.Shared.Repositories;
using RentApp.ViewModels;
using RentAppProject;

namespace RentApp
{
	public class iOSIoCContainer
	{
        
		public static IServiceProvider Create() => ConfigureServices();

		private static IServiceProvider ConfigureServices()
		{
			IServiceCollection services = new ServiceCollection();


            /*
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
            */


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


            //services.AddScoped<IUserVM, UserVM>();

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


            return IocContainer.AddServices(services);
			//return services.BuildServiceProvider();
		}
	}
}
