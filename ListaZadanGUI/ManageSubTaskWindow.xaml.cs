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
    /// Interaction logic for ManageSubTaskWindow.xaml
    /// </summary>
    public partial class ManageSubTaskWindow : Window
    {
        TasksDBConnection DB;
        public Tasks.Task SelectedTask { get; set; }
        public Tasks.SubTask SelectedSubTask { get; set; }
        public ManageSubTaskWindow(TasksDBConnection DB)
        {
            this.DB = DB;
            InitializeComponent();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (SelectedSubTask != null)
            {
                this.Title = "Edit subtask " + SelectedSubTask.title;
                TitleTextBox.Text = SelectedSubTask.title;
                EndDatePicker.DisplayDate = SelectedSubTask.enddate;
                EndDatePicker.SelectedDate = SelectedSubTask.enddate;
                IsDoneLabel.Visibility = Visibility.Visible;
                IsDoneCheckBox.Visibility = Visibility.Visible;
                IsDoneCheckBox.IsChecked = SelectedSubTask.isdone;
                DescriptionTextBox.Text = SelectedSubTask.description;
            }
            else
            {
                EndDatePicker.DisplayDate = DateTime.Today.AddDays(1);
                EndDatePicker.SelectedDate = DateTime.Today.AddDays(1);
            }
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            if (TitleTextBox.Text.Length > 0)
            {
                if (SelectedSubTask == null)
                {
                    DB.AddSubTask(SelectedTask, new Tasks.SubTask(TitleTextBox.Text, DescriptionTextBox.Text,
                        EndDatePicker.SelectedDate ?? DateTime.Today.AddDays(1)));
                }
                else
                {
                    DB.EditSubTask(SelectedTask, SelectedSubTask, TitleTextBox.Text, DescriptionTextBox.Text, IsDoneCheckBox.IsChecked.GetValueOrDefault(),
                        EndDatePicker.SelectedDate ?? DateTime.Today);
                }
                DialogResult = true;
            }
            else
            {
                MessageBox.Show("The Title must not be empty!", "Error!");
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
