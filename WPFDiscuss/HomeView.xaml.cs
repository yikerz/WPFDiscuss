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
    /// Interaction logic for HomeView.xaml
    /// </summary>
    public partial class HomeView : UserControl
    {
        SqlConnection conn;
        public HomeView()
        {
            InitializeComponent();
            string connectionString = ConfigurationManager.ConnectionStrings["WPFDiscuss.Properties.Settings.WpfDiscussConnectionString"].ConnectionString;
            conn = new SqlConnection(connectionString);

            DataTable dt = ListPosts();

            foreach (DataRow dr in dt.Rows)
            {
                int postId = int.Parse(dr["Id"].ToString());
                int userId = int.Parse(dr["UserId"].ToString());
                string author = GetUser(userId);
                string title = dr["Title"].ToString();
                string content = dr["Content"].ToString();
                DateTime createdDate = DateTime.Parse(dr["CreatedDate"].ToString());
                PostCard postCard = new PostCard(postId, author, createdDate, title, content);
                postWrapPanel.Children.Add(postCard);
            }

        }

        private DataTable ListPosts()
        {
            string query = "select * from Post order by CreatedDate desc";
            SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
            using (adapter)
            {
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                return dt;
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

        private void CreatePost_Click(object sender, RoutedEventArgs e)
        {
            Window currentWindow = Window.GetWindow(this);
            currentWindow.Content = new CreatePostView();
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Properties.Remove("Username");
            Window currentWindow = Window.GetWindow(this);
            currentWindow.Content = new LoginView();
        }
    }
}
