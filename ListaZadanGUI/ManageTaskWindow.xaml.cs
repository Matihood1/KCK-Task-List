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
    /// Interaction logic for ManageTaskWindow.xaml
    /// </summary>
    public partial class ManageTaskWindow : Window
    {
        TasksDBConnection DB;
        public Tasks.Task SelectedTask { get; set; }
        public ManageTaskWindow(TasksDBConnection DB)
        {
            this.DB = DB;
            InitializeComponent();
            PriorityComboBox.ItemsSource = Tasks.priorities.Values.ToList();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if(SelectedTask != null)
            {
                this.Title = "Edit task " + SelectedTask.title;
                TitleTextBox.Text = SelectedTask.title;
                EndDatePicker.DisplayDate = SelectedTask.enddate;
                EndDatePicker.SelectedDate = SelectedTask.enddate;
                IsDoneLabel.Visibility = Visibility.Visible;
                IsDoneCheckBox.Visibility = Visibility.Visible;
                IsDoneCheckBox.IsChecked = SelectedTask.isdone;
                PriorityComboBox.Text = Tasks.priorities[SelectedTask.priority];
                DescriptionTextBox.Text = SelectedTask.description;
            }
            else
            {
                EndDatePicker.DisplayDate = DateTime.Today.AddDays(1);
                EndDatePicker.SelectedDate = DateTime.Today.AddDays(1);
                PriorityComboBox.Text = Tasks.priorities[2];
            }
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            if(TitleTextBox.Text.Length > 0)
            {
                if(SelectedTask == null)
                {
                    DB.AddTask(new Tasks.Task(TitleTextBox.Text, DescriptionTextBox.Text, EndDatePicker.SelectedDate ?? DateTime.Today.AddDays(1),
                        PriorityComboBox.SelectedIndex));
                }
                else
                {
                    DB.EditTask(SelectedTask, TitleTextBox.Text, DescriptionTextBox.Text, IsDoneCheckBox.IsChecked.GetValueOrDefault(),
                        EndDatePicker.SelectedDate ?? DateTime.Today, PriorityComboBox.SelectedIndex);
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
