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

namespace WPFDiscuss
{
    /// <summary>
    /// Interaction logic for PostCard.xaml
    /// </summary>
    public partial class PostCard : UserControl
    {
        Post post;
        public PostCard(int id, string author, DateTime createdDate, string title, string content)
        {
            post = new Post
            {
                Id = id,
                Author = author,
                Title = title,
                Content = content,
                CreatedDate = createdDate
            };
            InitializeComponent();
            this.DataContext = post;
        }

        private void Post_Click(object sender, RoutedEventArgs e)
        {
            Window currentWindow = Window.GetWindow(this);
            currentWindow.Content = new PostView(post);
        }
    }
}
