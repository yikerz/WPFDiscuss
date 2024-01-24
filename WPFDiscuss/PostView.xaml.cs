using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
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
    /// Interaction logic for PostView.xaml
    /// </summary>
    public partial class PostView : UserControl
    {
        SqlConnection conn;
        public Post CurrentPost { get; set; }
        public PostView(Post post)
        {
            CurrentPost = post;
            InitializeComponent();
            string connectionString = ConfigurationManager.ConnectionStrings["WPFDiscuss.Properties.Settings.WpfDiscussConnectionString"].ConnectionString;
            conn = new SqlConnection(connectionString);

            this.DataContext = CurrentPost;
            if (CurrentPost.Author == Application.Current.Properties["Username"].ToString())
            {
                removePostButton.Visibility = Visibility.Visible;
            }
            ShowComments();
        }

        private void ShowComments()
        {
            string query = "select * from Comment where PostId = @postId order by CreatedDate";
            SqlCommand cmd = new SqlCommand(query, conn);
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);

            using (adapter)
            {
                cmd.Parameters.AddWithValue("@postId", CurrentPost.Id);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                commentWrapPanel.Children.Clear();

                foreach (DataRow dr in dt.Rows)
                {
                    int userId = int.Parse(dr["UserId"].ToString());
                    string author = GetUser(userId);
                    string content = dr["Content"].ToString();
                    DateTime createdDate = DateTime.Parse(dr["CreatedDate"].ToString());
                    CommentCard commentCard = new CommentCard(author, content, createdDate);
                    commentWrapPanel.Children.Add(commentCard); 
                }
            }

        }

        private string GetUser(int userId)
        {
            string query = "select Username from [User] where Id=@userId";
            SqlCommand cmd = new SqlCommand(query, conn);
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            using (adapter)
            {
                cmd.Parameters.AddWithValue("@userId", userId);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                return dt.Rows[0]["Username"].ToString();
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            Window currentWindow = Window.GetWindow(this);
            currentWindow.Content = new HomeView();
        }

        private void AddComment_Click(object sender, RoutedEventArgs e)
        {
            string content = tbComment.Text;
            AddComment(content);
        }

        private void AddComment(string content)
        {
            int userId = int.Parse(Application.Current.Properties["UserId"].ToString());
            int postId = CurrentPost.Id;

            try
            {
                string query = "insert into Comment (UserId, PostId, Content) values (@userId, @postId, @content)";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                cmd.Parameters.AddWithValue("@userId", userId);
                cmd.Parameters.AddWithValue("@postId", postId);
                cmd.Parameters.AddWithValue("@content", content);
                cmd.ExecuteScalar();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            finally { 
                conn.Close();
                tbComment.Text = "";
                ShowComments();
            }
;
        }

        private void RemovePost_Click(object sender, RoutedEventArgs e)
        {
            DeletePost();
            Window currentWindow = Window.GetWindow(this);
            currentWindow.Content = new HomeView();
        }

        private void DeletePost()
        {
            try
            {
                string query = "delete from Post where Id = @postId";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                cmd.Parameters.AddWithValue("@postId", CurrentPost.Id);
                cmd.ExecuteScalar();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            finally { conn.Close(); }
        }

        private void EditPost_Click(object sender, RoutedEventArgs e)
        {
            Window currentWindow = Window.GetWindow(this);
            currentWindow.Content = new EditPostView(CurrentPost);
        }
    }
}
