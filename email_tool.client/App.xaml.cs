using System.Windows;
using email_tool.client.Views;

namespace email_tool.client
{
    public partial class App : Application
    {
        public static void NavigateToSendMessagePage()
        {
            var sendMessagePage = new SendMessagePage();
            var mainWindow = Current.MainWindow;
            mainWindow.Content = sendMessagePage;
        }
    }
}
