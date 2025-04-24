
using System.Windows.Controls;
using email_tool.client.ViewModels;


namespace email_tool.client.Views
{
    public partial class LoginPage
    {

        public LoginPage()
        {
            InitializeComponent();
            var vm = App.GetService<AuthViewModel>();
            DataContext = vm;
        }
        
    }
}
