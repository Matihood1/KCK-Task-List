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
    /// Interaction logic for TasksView.xaml
    /// </summary>
    public partial class TasksControl : UserControl
    {
        TasksDBConnection DB;

        GridViewColumn lastheaderclickedcolumn;
        ListSortDirection lastdirection = ListSortDirection.Descending;

        public TasksControl(TasksDBConnection DB)
        {
            this.DB = DB;
            InitializeComponent();

            TasksListView.ItemsSource = Tasks.taskslist;
            //Sort("creationdate", ListSortDirection.Descending);
            CreationDateColumn.HeaderTemplate = Resources["HeaderTemplateArrowDown"] as DataTemplate;
            lastheaderclickedcolumn = CreationDateColumn;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            TasksListView_SizeChanged(TasksListView, null);
            if (TasksListView.SelectedItem != null)
            {
                EditTaskButton.IsEnabled = true;
                DeleteTaskButton.IsEnabled = true;
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
                    Sort(GetSortString(lastheaderclickedcolumn), direction);

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

            /*dataview*/TasksListView.Items.SortDescriptions.Clear();
            /*dataview*/TasksListView.Items.SortDescriptions.Add(new SortDescription(sortby, direction));
            if (!sortby.Equals("title") && !sortby.Equals("creationdate"))
            {
                /*dataview*/TasksListView.Items.SortDescriptions.Add(new SortDescription("title", direction));
            }
            /*dataview*/TasksListView.Items.Refresh();
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

        private void TasksListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var item = ((FrameworkElement)e.OriginalSource).DataContext as Tasks.Task;
            if (item != null)
            {
                MessageBox.Show("Item's Double Click handled!");

            }
        }

        private void TasksListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (TasksListView.SelectedItem != null)
            {
                EditTaskButton.IsEnabled = true;
                DeleteTaskButton.IsEnabled = true;
            }
            else
            {
                EditTaskButton.IsEnabled = false;
                DeleteTaskButton.IsEnabled = false;
            }
        }

        private void AddTaskButton_Click(object sender, RoutedEventArgs e)
        {
            ManageTaskWindow managetaskwindow = new ManageTaskWindow(DB);
            managetaskwindow.Owner = Window.GetWindow(this);
            managetaskwindow.ShowDialog();
            if(managetaskwindow.DialogResult == true)
            {
                Sort(GetSortString(lastheaderclickedcolumn), lastdirection);
                //TasksListView.Items.Refresh();
            }
        }

        private void EditTaskButton_Click(object sender, RoutedEventArgs e)
        {
            if(TasksListView.SelectedItem != null)
            {
                ManageTaskWindow managetaskwindow = new ManageTaskWindow(DB);
                managetaskwindow.Owner = Window.GetWindow(this);
                managetaskwindow.SelectedTask = TasksListView.SelectedItem as Tasks.Task;
                managetaskwindow.ShowDialog();
                if (managetaskwindow.DialogResult == true)
                {
                    TasksListView.Items.Refresh();
                }
            }
        }

        private void DeleteTaskButton_Click(object sender, RoutedEventArgs e)
        {
            if (TasksListView.SelectedItem != null)
            {
                MessageBoxResult result = MessageBox.Show("Are you sure you want to delete this task?",
                    "Delete task " + ((Tasks.Task)TasksListView.SelectedItem).title, MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    DB.DeleteTask(Tasks.taskslist.FindIndex(x => x.id == ((Tasks.Task)TasksListView.SelectedItem).id));
                    Sort(GetSortString(lastheaderclickedcolumn), lastdirection); //Nie chce zawsze odświeżyć, trzeba sortować bo więcej operacji???
                    //TasksListView.Items.Refresh();
                }
            }
        }
    }
}
