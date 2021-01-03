using ListaZadanFunkcjonalnosc;
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
using System.Windows.Shapes;

namespace ListaZadanGUI
{
    /// <summary>
    /// Interaction logic for SubTaskDetailsWindow.xaml
    /// </summary>
    public partial class SubTaskDetailsWindow : Window
    {
        public SubTaskDetailsWindow(Tasks.SubTask selectedsubtask)
        {
            InitializeComponent();

            TitleLabel.Content = selectedsubtask.title;
            CreationDateLabel.Content = selectedsubtask.creationdate.ToString("dd.MM.yyyy HH:mm");
            EndDateLabel.Content = selectedsubtask.creationdate.ToString("dd.MM.yyyy");
            IsDoneLabel.Content = selectedsubtask.isdone ? "Yes" : "No";
            DescriptionLabel.Text = selectedsubtask.description;
        }
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
