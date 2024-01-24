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
    /// Interaction logic for CommentCard.xaml
    /// </summary>
    public partial class CommentCard : UserControl
    {
        Comment comment;
        public CommentCard(string author, string content, DateTime createdDate)
        {
            comment = new Comment
            {
                Author = author,
                Content = content,
                CreatedDate = createdDate
            };
            InitializeComponent();
            this.DataContext = comment;
        }
    }
}
