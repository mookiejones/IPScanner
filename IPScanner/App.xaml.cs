using CommunityToolkit.Mvvm.DependencyInjection;
using IPScanner.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System.Configuration;
using System.Data;
using System.Windows;

namespace IPScanner
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App:Application
    {

        /// <summary>
        /// Gets the current <see cref="App"/> instance in use
        /// </summary>
        public new static App Current => (App)Application.Current;

        /// <summary>
        /// Gets the <see cref="IServiceProvider"/> instance to resolve application services.
        /// </summary>
        public IServiceProvider Services { get; }
        public App()
        {
            Services = new ServiceCollection()
                .AddViewModels()
                .BuildServiceProvider();
            Ioc.Default.ConfigureServices(Services);
            //   DispatcherHelper.Initialize();

            var args = Environment.GetCommandLineArgs();

         
            for (var i = 1; i < args.Length; i++)
            {

             
            }

            if (args.Length > 1)
                Current.Shutdown();
        }
 
    }

}
