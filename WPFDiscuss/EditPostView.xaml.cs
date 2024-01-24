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
    /// Interaction logic for EditPostView.xaml
    /// </summary>
    public partial class EditPostView : UserControl
    {
        public Post CurrentPost { get; set; }
        SqlConnection conn;
        public EditPostView(Post post)
        {
            CurrentPost = post;
            InitializeComponent();
            string connectionString = ConfigurationManager.ConnectionStrings["WPFDiscuss.Properties.Settings.WpfDiscussConnectionString"].ConnectionString;
            conn = new SqlConnection(connectionString);
            tbTitle.Text = CurrentPost.Title;
            tbContent.Text = CurrentPost.Content;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Window currentWindow = Window.GetWindow(this);
            currentWindow.Content = new PostView(CurrentPost);
        }

        private void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            UpdatePost();
            Window currentWindow = Window.GetWindow(this);
            currentWindow.Content = new PostView(CurrentPost);
        }

        private void UpdatePost()
        {
            string newTitle = tbTitle.Text;
            string newContent = tbContent.Text;
            CurrentPost.Title = newTitle;
            CurrentPost.Content = newContent;

            try
            {
                string query = "update Post set Title = @newTitle, Content = @newContent where Id = @postId";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                cmd.Parameters.AddWithValue("@newTitle", newTitle);
                cmd.Parameters.AddWithValue("@newContent", newContent);
                cmd.Parameters.AddWithValue("@postId", CurrentPost.Id);
                cmd.ExecuteScalar();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            finally { conn.Close(); }

        }
    }

}
