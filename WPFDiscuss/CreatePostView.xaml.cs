using System;
using System.Collections.Generic;
using System.Configuration;
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

namespace WPFDiscuss
{
    /// <summary>
    /// Interaction logic for CreatePostView.xaml
    /// </summary>
    public partial class CreatePostView : UserControl
    {
        SqlConnection conn;
        public CreatePostView()
        {
            InitializeComponent();
            string connectionString = ConfigurationManager.ConnectionStrings["WPFDiscuss.Properties.Settings.WpfDiscussConnectionString"].ConnectionString;
            conn = new SqlConnection(connectionString);
        }

        private void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            CreatePost();
            Window currentWindow = Window.GetWindow(this);
            currentWindow.Content = new HomeView();
        }

        private void CreatePost()
        {
            try
            {
                int userId = int.Parse(Application.Current.Properties["UserId"].ToString());
                string title = tbTitle.Text;
                string content = tbContent.Text;

                string query = "insert into Post (UserId, Title, Content) values (@userId, @title, @content)";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                cmd.Parameters.AddWithValue("@userId", userId);
                cmd.Parameters.AddWithValue("@title", title);
                cmd.Parameters.AddWithValue("@content", content);
                cmd.ExecuteScalar();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                conn.Close();
            }

        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Window currentWindow = Window.GetWindow(this);
            currentWindow.Content = new HomeView();
        }
    }
}
