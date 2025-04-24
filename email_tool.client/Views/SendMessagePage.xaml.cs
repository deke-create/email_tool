using System.Windows;
using System.Windows.Controls;
using email_tool.client.ViewModels;

namespace email_tool.client.Views
{
    public partial class SendMessagePage : Page
    {
        public SendMessagePage(EmailViewModel? vm)
        {
            InitializeComponent();
            DataContext = vm;
        }
        
    }
}
