using System;
using System.Collections.Generic;
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
using System.Data.SqlClient;
using System.Data;

namespace WPFDiscuss
{
    /// <summary>
    /// Interaction logic for LoginView.xaml
    /// </summary>
    public partial class LoginView : UserControl
    {
        SqlConnection conn;
        public LoginView()
        {
            InitializeComponent();

            string connectionString = ConfigurationManager.ConnectionStrings["WPFDiscuss.Properties.Settings.WpfDiscussConnectionString"].ConnectionString;
            conn = new SqlConnection(connectionString);
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
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
                bool loginResult = VerifyLogin(username, password);
                if (loginResult)
                {
                    Window currentWindow = Window.GetWindow(this);
                    currentWindow.Content = new HomeView();
                }
                else
                {
                    MessageBox.Show("Invalid password",
                    "Invalid Password",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
                }
            }

        }

        private bool VerifyLogin(string username, string password)
        {

            string query = "select * from [User] where Username=@username";
            SqlCommand cmd = new SqlCommand(query, conn);
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            using (adapter)
            {
                cmd.Parameters.AddWithValue("@username", username);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                string correctPw = dt.Rows[0]["Password"].ToString();
                if (correctPw == password)
                {
                    Application.Current.Properties["Username"] = username;
                    Application.Current.Properties["UserId"] = dt.Rows[0]["Id"].ToString();
                    return true;
                }
                else
                {
                    return false;
                }
            }

        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            Window currentWindow = Window.GetWindow(this);
            currentWindow.Content = new RegisterView();
        }
    }
}
