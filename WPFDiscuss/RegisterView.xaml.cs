using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Configuration;

namespace WPFDiscuss
{
    /// <summary>
    /// Interaction logic for RegisterView.xaml
    /// </summary>
    public partial class RegisterView : UserControl
    {
        SqlConnection conn;
        public RegisterView()
        {
            InitializeComponent();
            string connectionString = ConfigurationManager.ConnectionStrings["WPFDiscuss.Properties.Settings.WpfDiscussConnectionString"].ConnectionString;
            conn = new SqlConnection(connectionString);

        }

        private void CreateUser_Click(object sender, RoutedEventArgs e)
        {
            string username = tbUsername.Text;
            string password = tbPassword.Password;

            if (username.Length == 0)
            {
                MessageBox.Show("Please insert username",
                    "Invalid Username",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
            else
            {
                CreateUser(username, password);
                Window currentWindow = Window.GetWindow(this);
                currentWindow.Content = new LoginView();
            }

        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Window currentWindow = Window.GetWindow(this);
            currentWindow.Content = new LoginView();
        }

        private void CreateUser(string username, string password)
        {
            try
            {
                string query = "insert into [User] values (@username, @password)";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                cmd.Parameters.AddWithValue("@username", username);
                cmd.Parameters.AddWithValue("@password", password);
                cmd.ExecuteScalar();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Invalid username",
                    "Invalid Username",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
            finally { conn.Close(); }


        }
    }
}
