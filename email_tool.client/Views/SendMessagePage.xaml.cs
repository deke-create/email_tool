using System.Windows;
using System.Windows.Controls;

namespace email_tool.client.Views
{
    public partial class SendMessagePage : Page
    {
        public SendMessagePage()
        {
            InitializeComponent();
        }

        private void OnSendClick(object sender, RoutedEventArgs e)
        {
            // Retrieve input values
            string senderEmail = SenderTextBox.Text;
            string recipientEmail = RecipientTextBox.Text;
            string subject = SubjectTextBox.Text;
            string body = BodyTextBox.Text;

            // Validate inputs
            if (string.IsNullOrWhiteSpace(senderEmail) || string.IsNullOrWhiteSpace(recipientEmail))
            {
                StatusTextBlock.Text = "Sender and recipient fields cannot be empty.";
                StatusTextBlock.Foreground = System.Windows.Media.Brushes.Red;
                StatusTextBlock.Visibility = Visibility.Visible;
                return;
            }


            try
            {

                StatusTextBlock.Text = "Message sent successfully!";
                StatusTextBlock.Foreground = System.Windows.Media.Brushes.Green;
            }
            catch
            {
                StatusTextBlock.Text = "Failed to send the message.";
                StatusTextBlock.Foreground = System.Windows.Media.Brushes.Red;
            }

            StatusTextBlock.Visibility = Visibility.Visible;
        }
    }
}
