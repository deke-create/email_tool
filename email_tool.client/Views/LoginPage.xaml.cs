
using System.Windows.Controls;
using email_tool.client.ViewModels;


namespace email_tool.client.Views
{
    public partial class LoginPage : Page
    {

        public LoginPage(AuthViewModel vm)
        {
            InitializeComponent();
            DataContext = vm;
        }
        
    }
}
