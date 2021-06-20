using BLL.Interfaces;
using BLL.Interfaces.Settings;
using BLL.Model.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Configuration;
using Microsoft.Extensions.Logging.EventLog;
using Services.External;
using Services.Internal;
using System;
using System.IO;
using System.Windows;

namespace Main
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static IServiceProvider ServiceProvider { get; private set; }

        public IConfiguration Configuration { get; private set; }
        public ILogger<App> Logger { get; private set; }
        public TournamentAssistantOptions Options { get; set; }

        public App()
        {
            var loggerFactory = new LoggerFactory();
            loggerFactory.AddProvider(new EventLogLoggerProvider());
            Logger = loggerFactory.CreateLogger<App>();

            try
            {
                var builder = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

                Configuration = builder.Build();

                ServiceCollection services = new ServiceCollection();
                ConfigureServices(services);
                ConfigureBoundOptions(services);
                ServiceProvider = services.BuildServiceProvider();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "App Error");
                throw ex;
            }
        }

        private void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<MainWindow>();
            services.AddSingleton<SingleWindow>();
            services.AddSingleton<SettingsWindow>();
            services.AddSingleton<ITournamentApiControlService, ChallongeApiService>();
            services.AddSingleton<IFileService, FileService>();
        }

        private void ConfigureBoundOptions(IServiceCollection services)
        {
            var config = new TournamentAssistantOptions();
            Configuration.Bind(nameof(TournamentAssistantOptions), config);

            var settingsModelFactory = new SettingsModelService(config);
            services.AddSingleton<ISettingsModelFactory>(provider => settingsModelFactory);
            services.AddSingleton<ISettingsModelUpdate>(provider => settingsModelFactory);
            services.AddScoped<TournamentAssistantOptions>(provider => settingsModelFactory.GetSettings());

            Options = config;
        }

        private void OnStartup(object sender, StartupEventArgs e)
        {
            try
            {
                var settingsWindow = ServiceProvider.GetService<SettingsWindow>();
                settingsWindow.Show();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                throw ex;
            }
        }
    }
}
