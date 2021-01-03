using ListaZadanFunkcjonalnosc;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ListaZadanGUI
{
    /// <summary>
    /// Interaction logic for TaskDetailsControl.xaml
    /// </summary>
    public partial class TaskDetailsControl : UserControl
    {
        TasksDBConnection DB;

        GridViewColumn lastheaderclickedcolumn;
        ListSortDirection lastdirection = ListSortDirection.Descending;
        Tasks.Task selectedtask;

        public TaskDetailsControl(TasksDBConnection DB, Tasks.Task selectedtask)
        {
            this.DB = DB;
            InitializeComponent();

            this.selectedtask = selectedtask;
            SubTasksListView.ItemsSource = selectedtask.SubTasks;
            Sort("creationdate", ListSortDirection.Descending);
            CreationDateColumn.HeaderTemplate = Resources["HeaderTemplateArrowDown"] as DataTemplate;
            lastheaderclickedcolumn = CreationDateColumn;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            TasksListView_SizeChanged(SubTasksListView, null);
            TitleLabel.Content = selectedtask.title;
            CreationDateLabel.Content = selectedtask.creationdate.ToString("dd.MM.yyyy HH:mm");
            EndDateLabel.Content = selectedtask.creationdate.ToString("dd.MM.yyyy");
            PriorityLabel.Content = Tasks.priorities[selectedtask.priority];
            IsDoneLabel.Content = selectedtask.isdone ? "Yes" : "No";
            DescriptionLabel.Text = selectedtask.description;
            if (SubTasksListView.SelectedItem != null)
            {
                EditSubTaskButton.IsEnabled = true;
                DeleteSubTaskButton.IsEnabled = true;
            }
        }

        void GridViewColumnHeader_Click(object sender, RoutedEventArgs e)
        {
            var headerclicked = e.OriginalSource as GridViewColumnHeader;
            ListSortDirection direction;

            if (headerclicked != null)
            {
                if (headerclicked.Role != GridViewColumnHeaderRole.Padding)
                {
                    if (headerclicked.Column != lastheaderclickedcolumn)
                    {
                        direction = ListSortDirection.Ascending;
                    }
                    else
                    {
                        if (lastdirection == ListSortDirection.Ascending)
                        {
                            direction = ListSortDirection.Descending;
                        }
                        else
                        {
                            direction = ListSortDirection.Ascending;
                        }
                    }

                    //var columnbinding = headerclicked.Column.DisplayMemberBinding as Binding;
                    /*var sortby = columnbinding?.Path.Path ?? headerclicked.Column.Header as string;*/

                    //Sort(sortby, direction);
                    Sort(GetSortString(headerclicked.Column), direction);

                    if (direction == ListSortDirection.Ascending)
                    {
                        headerclicked.Column.HeaderTemplate = Resources["HeaderTemplateArrowUp"] as DataTemplate;
                    }
                    else
                    {
                        headerclicked.Column.HeaderTemplate = Resources["HeaderTemplateArrowDown"] as DataTemplate;
                    }

                    // Remove arrow from previously sorted header
                    if (lastheaderclickedcolumn != null && lastheaderclickedcolumn != headerclicked.Column)
                    {
                        lastheaderclickedcolumn.HeaderTemplate = null;
                    }

                    lastheaderclickedcolumn = headerclicked.Column;
                    lastdirection = direction;
                }
            }
        }

        private string GetSortString(GridViewColumn column)
        {
            var sortby = ((Binding)column.DisplayMemberBinding).Path.Path;
            if (sortby.Equals("DisplayPriority"))
            {
                sortby = "priority";
            }
            return sortby;
        }

        private void Sort(string sortby, ListSortDirection direction)
        {
            //ICollectionView dataview = CollectionViewSource.GetDefaultView(TasksListView.ItemsSource);

            /*dataview*/
            SubTasksListView.Items.SortDescriptions.Clear();
            /*dataview*/
            SubTasksListView.Items.SortDescriptions.Add(new SortDescription(sortby, direction));
            if (!sortby.Equals("title") && !sortby.Equals("creationdate"))
            {
                /*dataview*/
                SubTasksListView.Items.SortDescriptions.Add(new SortDescription("title", direction));
            }
            /*dataview*/
            SubTasksListView.Items.Refresh();
        }

        private void TasksListView_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            ListView listview = sender as ListView;
            GridView gridview = listview.View as GridView;
            var actualwidth = listview.ActualWidth - SystemParameters.VerticalScrollBarWidth;
            for (int i = 1; i < gridview.Columns.Count; i++)
            {
                actualwidth = actualwidth - gridview.Columns[i].ActualWidth;
            }
            gridview.Columns[0].Width = actualwidth;
        }

        private void SubTasksListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var item = ((FrameworkElement)e.OriginalSource).DataContext as Tasks.SubTask;
            if (item != null)
            {
                //MessageBox.Show("Item's Double Click handled!");
                new SubTaskDetailsWindow((Tasks.SubTask)SubTasksListView.SelectedItem).ShowDialog();
            }
        }

        private void SubTasksListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SubTasksListView.SelectedItem != null)
            {
                EditSubTaskButton.IsEnabled = true;
                DeleteSubTaskButton.IsEnabled = true;
            }
            else
            {
                EditSubTaskButton.IsEnabled = false;
                DeleteSubTaskButton.IsEnabled = false;
            }
        }

        private void AddSubTaskButton_Click(object sender, RoutedEventArgs e)
        {
            ManageSubTaskWindow managesubtaskwindow = new ManageSubTaskWindow(DB);
            managesubtaskwindow.Owner = Window.GetWindow(this);
            managesubtaskwindow.SelectedTask = selectedtask;
            managesubtaskwindow.ShowDialog();
            if (managesubtaskwindow.DialogResult == true)
            {
                Sort(GetSortString(lastheaderclickedcolumn), lastdirection);
                //TasksListView.Items.Refresh();
            }
        }

        private void EditSubTaskButton_Click(object sender, RoutedEventArgs e)
        {
            if (SubTasksListView.SelectedItem != null)
            {
                ManageSubTaskWindow managesubtaskwindow = new ManageSubTaskWindow(DB);
                managesubtaskwindow.Owner = Window.GetWindow(this);
                managesubtaskwindow.SelectedTask = selectedtask;
                managesubtaskwindow.SelectedSubTask = SubTasksListView.SelectedItem as Tasks.SubTask;
                managesubtaskwindow.ShowDialog();
                if (managesubtaskwindow.DialogResult == true)
                {
                    SubTasksListView.Items.Refresh();
                }
            }
        }

        private void DeleteSubTaskButton_Click(object sender, RoutedEventArgs e)
        {
            if (SubTasksListView.SelectedItem != null)
            {
                MessageBoxResult result = MessageBox.Show("Are you sure you want to delete this subtask?",
                    "Delete subtask " + ((Tasks.SubTask)SubTasksListView.SelectedItem).title, MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    DB.DeleteSubTask(selectedtask, selectedtask.SubTasks.FindIndex(x => x.id == ((Tasks.SubTask)SubTasksListView.SelectedItem).id));
                    Sort(GetSortString(lastheaderclickedcolumn), lastdirection); //Nie chce zawsze odświeżyć, trzeba sortować bo więcej operacji???
                    //TasksListView.Items.Refresh();
                }
            }
        }
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            ((MainWindow)Window.GetWindow(this)).ReturnToTasksList();
        }
    }
}
