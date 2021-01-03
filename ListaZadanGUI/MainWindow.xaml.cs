using ListaZadanFunkcjonalnosc;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ListaZadanGUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        TasksDBConnection DB;
        TasksControl TasksListUserControl;
        TaskDetailsControl TaskDetailsUserControl;
        public MainWindow()
        {
            CultureInfo.CurrentCulture = new CultureInfo("pl-PL");
            Language = XmlLanguage.GetLanguage(CultureInfo.CurrentCulture.IetfLanguageTag);
            DB = new TasksDBConnection();
            TasksListUserControl = new TasksControl(DB);

            InitializeComponent();
            MainWindowContentControl.Content = TasksListUserControl;
        }

        public void OpenTaskDetails(Tasks.Task selectedtask)
        {
            TaskDetailsUserControl = new TaskDetailsControl(DB, selectedtask);
            MainWindowContentControl.Content = TaskDetailsUserControl;
        }

        public void ReturnToTasksList()
        {
            MainWindowContentControl.Content = TasksListUserControl;
            TaskDetailsUserControl = null;
        }
    }
}
