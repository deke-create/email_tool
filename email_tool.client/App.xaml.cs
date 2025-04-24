using System.Net.Http;
using System.Windows;
using email_tool.client.Models;
using email_tool.client.Services;
using email_tool.client.Views;
using Microsoft.Extensions.DependencyInjection;
using email_tool.client.ViewModels;

namespace email_tool.client
{
    public partial class App : Application
    {
        private static ServiceProvider _serviceProvider;

        public App()
        {
            ConfigureServices();
        }

        private void ConfigureServices()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddTransient<HttpClient>();
            serviceCollection.AddSingleton<User>();
            serviceCollection.AddSingleton<AuthViewModel>();
            serviceCollection.AddSingleton<EmailViewModel>();
            serviceCollection.AddSingleton<EmailService>();
            serviceCollection.AddSingleton<AuthService>();
            
            _serviceProvider = serviceCollection.BuildServiceProvider();
        }

        public static T? GetService<T>() => _serviceProvider.GetService<T>();

        public static void NavigateToSendMessagePage()
        {
            var emailViewModel = GetService<EmailViewModel>();
            var sendMessagePage = new SendMessagePage(emailViewModel);
            var mainWindow = Current.MainWindow;
            
            sendMessagePage.DataContext = emailViewModel;
            mainWindow.Content = sendMessagePage;
        }
    }
}
