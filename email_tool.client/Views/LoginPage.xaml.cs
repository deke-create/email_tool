using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Windows;
using System.Windows.Controls;


namespace email_tool.client.Views
{
    public partial class LoginPage : Page
    {
        private readonly HttpClient _httpClient = new();

        public LoginPage()
        {
            InitializeComponent();
        }

        private async void OnLoginClick(object sender, RoutedEventArgs e)
        {
            var username = UsernameTextBox.Text;
            var password = PasswordBox.Password;

            var loginRequest = new { Username = username, Password = password };
            var content = new StringContent(JsonConvert.SerializeObject(loginRequest), Encoding.UTF8, "application/json");

            try
            {
                var response = await _httpClient.PostAsync("http://localhost:5000/api/Auth/login", content);
                if (response.IsSuccessStatusCode)
                {
                    App.NavigateToSendMessagePage();
                }
                else
                {
                    ErrorTextBlock.Text = "Invalid username or password.";
                    ErrorTextBlock.Visibility = Visibility.Visible;
                }
            }
            catch (Exception ex)
            {
                ErrorTextBlock.Text = $"Error: {ex.Message}";
                ErrorTextBlock.Visibility = Visibility.Visible;
            }
        }
    }
}
