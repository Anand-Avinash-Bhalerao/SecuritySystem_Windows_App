using Firebase.Auth;
using Firebase.Auth.Providers;
using Firebase.Auth.Repository;
using Firebase.Database;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MVVMEssentials.Services;
using MVVMEssentials.Stores;
using MVVMEssentials.ViewModels;
using SecuritySystem.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Navigation;

namespace SecuritySystem
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly IHost _host;
        public App()
        {
            _host = Host
                .CreateDefaultBuilder()
                .ConfigureServices((context, service) =>
                {
                    string firebaseKey = "AIzaSyAYnGaFSaBsTcTYMHn8RzRijTvuvL_3484";
                    FirebaseAuthConfig config = new FirebaseAuthConfig
                    {
                        ApiKey = firebaseKey,
                        AuthDomain = "security-system-a7530.firebaseapp.com",
                        Providers = new FirebaseAuthProvider[]
                            {
                                new EmailProvider()
                            }
                    };

                    service.AddSingleton<FirebaseAuthClient>(new FirebaseAuthClient(config));

                    service.AddSingleton<NavigationStore>();
                    service.AddSingleton<ModalNavigationStore>();

                    service.AddSingleton<MainViewModel>();

                    service.AddSingleton<NavigationService<RegisterAdminViewModel>>(
                        (services) => new NavigationService<RegisterAdminViewModel>(
                            services.GetRequiredService<NavigationStore>(),
                            () => new RegisterAdminViewModel
                                (
                                    services.GetRequiredService<FirebaseAuthClient>(),
                                    services.GetRequiredService<FirebaseClient>()
                                )
                            ));

                    

                    service.AddSingleton<MainWindow>((services) => new MainWindow()
                    {
                        //DataContext = new RegisterAdminViewModel(services.GetRequiredService<FirebaseAuthClient>())
                        DataContext = services.GetRequiredService<MainViewModel>()

                    });

                    service.AddSingleton<FirebaseClient>(new FirebaseClient("https://security-system-a7530-default-rtdb.firebaseio.com"));


                })

                .Build();
        }
        protected override void OnStartup(StartupEventArgs e)
        {

            INavigationService navigationServie = _host.Services.GetRequiredService<NavigationService<RegisterAdminViewModel>>();
            navigationServie.Navigate();

            MainWindow = _host.Services.GetRequiredService<MainWindow>();

            MainWindow.Show();
            base.OnStartup(e);
        }
    }
}
