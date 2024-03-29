﻿using ListaZadanFunkcjonalnosc;
using System;
using System.Globalization;
using System.Linq;
using Terminal.Gui;

namespace ListaRzeczyTUI
{
    class Program : Window
    {
        static TasksDBConnection DB;
        static private bool ManageTask(Tasks.Task selectedtask)
        {
            bool IsManaged = false;

            var TaskTitleLabel = new Label("Title: ")
            {
                X = 3,
                Y = 2
            };

            var TaskEndDateLabel = new Label("End Date: ")
            {
                X = Pos.Left(TaskTitleLabel),
                Y = Pos.Bottom(TaskTitleLabel) + 1
            };

            var TaskPriorityLabel = new Label("Priority: ")
            {
                X = Pos.Left(TaskEndDateLabel),
                Y = Pos.Bottom(TaskEndDateLabel) + 1
            };

            var TaskDescriptionLabel = new Label("Description: ")
            {
                X = Pos.Left(TaskPriorityLabel),
                Y = Pos.Bottom(TaskPriorityLabel) + 1
            };

            var TaskTitleField = new TextField()
            {
                X = Pos.Right(TaskDescriptionLabel),
                Y = Pos.Top(TaskTitleLabel),
                Width = Dim.Fill() - 3
            };

            var TaskEndDateField = new DateField()
            {
                X = Pos.Right(TaskDescriptionLabel),
                Y = Pos.Top(TaskEndDateLabel)
            };

            var TaskIsDoneLabel = new Label("Done: ")
            {
                X = Pos.Right(TaskEndDateField) + 1,
                Y = Pos.Top(TaskEndDateLabel)
            };

            var TaskIsDoneCheckBox = new CheckBox()
            {
                X = Pos.Right(TaskIsDoneLabel),
                Y = Pos.Top(TaskEndDateLabel)
            };

            var TaskPriorityField = new ComboBox()
            {
                X = Pos.Right(TaskDescriptionLabel),
                Y = Pos.Top(TaskPriorityLabel),
                Width = Dim.Fill() - 3
            };
            TaskPriorityField.SetSource(Tasks.priorities.Values.ToList());
            TaskPriorityField.Text = Tasks.priorities[2];

            var TaskDescriptionField = new TextView()
            {
                X = Pos.Right(TaskDescriptionLabel),
                Y = Pos.Top(TaskDescriptionLabel),
                Width = Dim.Fill() - 3,
                Height = Dim.Fill() - 2,
                ColorScheme = Colors.ColorSchemes["TextViewColor"]
            };

            var ButtonOK = new Button("Ok", is_default: true);

            ButtonOK.Clicked += delegate ()
            {
                if (TaskTitleField.Text.Length > 0 && TaskTitleField.Text.Length <= 50)
                {
                    IsManaged = true;
                    Application.RequestStop();
                }
                else
                {
                    TitleControlDialog();
                }
            };

            var ButtonCancel = new Button("Cancel");

            ButtonCancel.Clicked += delegate ()
            {
                Application.RequestStop();
            };

            var d = new Dialog("New Task", 60, 20, ButtonOK, ButtonCancel);

            if (selectedtask != null)
            {
                d.Title = "Edit Task " + Tasks.FormatTitle(selectedtask, 43);
                TaskTitleField.Text = selectedtask.title;
                TaskEndDateField.Date = selectedtask.enddate;
                TaskIsDoneCheckBox.Checked = selectedtask.isdone;
                TaskPriorityField.Text = Tasks.priorities[selectedtask.priority];
                TaskDescriptionField.Text = selectedtask.description/*.Replace("\r", "\n")*/; //Dopisuje "?" przed każdą nową linią; wina biblioteki
            }
            else
            {
                TaskEndDateField.Width = Dim.Fill() - 3;
                TaskEndDateField.Date = DateTime.Today.AddDays(1);
            }

            d.Add(TaskTitleLabel, TaskEndDateLabel, TaskPriorityLabel, TaskDescriptionLabel);
            d.Add(TaskTitleField, TaskEndDateField, TaskPriorityField, TaskDescriptionField);

            if (selectedtask != null)
            {
                d.Add(TaskIsDoneLabel, TaskIsDoneCheckBox);
            }

            Application.Run(d);

            if (IsManaged)
            {
                if (selectedtask == null)
                {
                    DB.AddTask(new Tasks.Task(TaskTitleField.Text.ToString(), TaskDescriptionField.Text.ToString(), TaskEndDateField.Date,
                        Tasks.prioritiesinv[TaskPriorityField.Text.ToString()]));
                }
                else
                {
                    DB.EditTask(selectedtask, TaskTitleField.Text.ToString(), TaskDescriptionField.Text.ToString(),
                        TaskIsDoneCheckBox.Checked, TaskEndDateField.Date, Tasks.prioritiesinv[TaskPriorityField.Text.ToString()]);
                }
            }

            return (IsManaged);
        }
        static private bool DeleteTask(int taskindex)
        {
            bool IsDelete = false;

            var ButtonYes = new Button("Yes");

            ButtonYes.Clicked += delegate ()
            {
                IsDelete = true;
                Application.RequestStop();
            };

            var ButtonNo = new Button("No", is_default: true);

            ButtonNo.Clicked += delegate ()
            {
                Application.RequestStop();
            };

            /*ButtonNo.MouseClick += delegate (MouseEventArgs args)
            {
                Application.RequestStop();
            };

            ButtonNo.KeyDown += delegate (KeyEventEventArgs args)
            {
                if (args.KeyEvent.Key == Key.Enter)
                {
                    Application.RequestStop();
                }
            };*/

            var d = new Dialog("Delete Task " + Tasks.taskslist[taskindex].title, 60, 10, ButtonYes, ButtonNo);

            var ConfirmationLabel = new Label("Are you sure you want to delete this task?")
            {
                X = Pos.Center(),
                Y = Pos.Center()
            };

            d.Add(ConfirmationLabel);

            Application.Run(d);

            if (IsDelete)
            {
                DB.DeleteTask(taskindex);
            }

            return (IsDelete);
        }
        static private bool ManageSubTask(Tasks.Task parenttask, Tasks.SubTask selectedsubtask)
        {
            bool IsManaged = false;

            var TaskTitleLabel = new Label("Title: ")
            {
                X = 3,
                Y = 2
            };

            var TaskEndDateLabel = new Label("End Date: ")
            {
                X = Pos.Left(TaskTitleLabel),
                Y = Pos.Bottom(TaskTitleLabel) + 1
            };

            var TaskDescriptionLabel = new Label("Description: ")
            {
                X = Pos.Left(TaskEndDateLabel),
                Y = Pos.Bottom(TaskEndDateLabel) + 1
            };

            var TaskTitleField = new TextField()
            {
                X = Pos.Right(TaskDescriptionLabel),
                Y = Pos.Top(TaskTitleLabel),
                Width = Dim.Fill() - 3
            };

            var TaskEndDateField = new DateField()
            {
                X = Pos.Right(TaskDescriptionLabel),
                Y = Pos.Top(TaskEndDateLabel)
            };

            var TaskIsDoneLabel = new Label("Done: ")
            {
                X = Pos.Right(TaskEndDateField) + 1,
                Y = Pos.Top(TaskEndDateLabel)
            };

            var TaskIsDoneCheckBox = new CheckBox()
            {
                X = Pos.Right(TaskIsDoneLabel),
                Y = Pos.Top(TaskEndDateLabel)
            };

            var TaskDescriptionField = new TextView()
            {
                X = Pos.Right(TaskDescriptionLabel),
                Y = Pos.Top(TaskDescriptionLabel),
                Width = Dim.Fill() - 3,
                Height = Dim.Fill() - 2,
                ColorScheme = Colors.ColorSchemes["TextViewColor"]
            };

            var ButtonOK = new Button("Ok", is_default: true);

            ButtonOK.Clicked += delegate ()
            {
                if (TaskTitleField.Text.Length > 0 && TaskTitleField.Text.Length <= 50)
                {
                    IsManaged = true;
                    Application.RequestStop();
                }
                else
                {
                    TitleControlDialog();
                }
            };

            var ButtonCancel = new Button("Cancel");

            ButtonCancel.Clicked += delegate ()
            {
                Application.RequestStop();
            };

            var d = new Dialog("New Subtask ", 60, 20, ButtonOK, ButtonCancel);

            if (selectedsubtask != null)
            {
                d.Title = "Edit Subtask " + Tasks.FormatTitle(selectedsubtask, 43);
                TaskTitleField.Text = selectedsubtask.title;
                TaskEndDateField.Date = selectedsubtask.enddate;
                TaskIsDoneCheckBox.Checked = selectedsubtask.isdone;
                TaskDescriptionField.Text = selectedsubtask.description/*.Replace("\r", "\n")*/; //Dopisuje "?" przed każdą nową linią; wina biblioteki
            }
            else
            {
                TaskEndDateField.Width = Dim.Fill() - 3;
                TaskEndDateField.Date = DateTime.Today.AddDays(1);
            }

            d.Add(TaskTitleLabel, TaskEndDateLabel, TaskDescriptionLabel);
            d.Add(TaskTitleField, TaskEndDateField, TaskDescriptionField);

            if (selectedsubtask != null)
            {
                d.Add(TaskIsDoneLabel, TaskIsDoneCheckBox);
            }

            Application.Run(d);

            if (IsManaged)
            {
                if (selectedsubtask == null)
                {
                    DB.AddSubTask(parenttask, new Tasks.SubTask(TaskTitleField.Text.ToString(), TaskDescriptionField.Text.ToString(), TaskEndDateField.Date));
                }
                else
                {
                    DB.EditSubTask(parenttask, selectedsubtask, TaskTitleField.Text.ToString(), TaskDescriptionField.Text.ToString(), TaskIsDoneCheckBox.Checked, TaskEndDateField.Date);
                }
            }

            return IsManaged;
        }
        static private bool DeleteSubTask(Tasks.Task selectedtask, int taskindex)
        {
            bool IsDelete = false;

            var ButtonYes = new Button("Yes");

            ButtonYes.Clicked += delegate ()
            {
                IsDelete = true;
                Application.RequestStop();
            };

            var ButtonNo = new Button("No", is_default: true);

            ButtonNo.Clicked += delegate ()
            {
                Application.RequestStop();
            };

            var d = new Dialog("Delete Subtask " + Tasks.taskslist[taskindex].title, 60, 10, ButtonYes, ButtonNo);

            var ConfirmationLabel = new Label("Are you sure you want to delete this subtask?")
            {
                X = Pos.Center(),
                Y = Pos.Center()
            };

            d.Add(ConfirmationLabel);

            Application.Run(d);

            if (IsDelete)
            {
                DB.DeleteSubTask(selectedtask, taskindex);
            }

            return (IsDelete);
        }

        private static void TitleControlDialog()
        {
            var ButtonOK = new Button("Ok", is_default: true);

            ButtonOK.Clicked += delegate ()
            {
                Application.RequestStop();
            };

            var d = new Dialog("Error!", 50, 10, ButtonOK);

            var ErrorLabel = new Label("The Title must be between 1 and 50 characters!")
            {
                X = Pos.Center(),
                Y = Pos.Center()
            };

            d.Add(ErrorLabel);

            Application.Run(d);
        }

        static private void TaskDetails(Tasks.Task selectedtask)
        {
            int lastsort = 3;
            selectedtask.SortSubTasks(lastsort);
            var NewTop = new Toplevel(Application.Top.Frame);

            Window win = new Window("Details:")
            {
                X = 0,
                Y = 1,
                Width = Dim.Fill(),
                Height = Dim.Fill()
            };

            var TaskDetailsFrameView = new FrameView()
            {
                X = 0,
                Y = Pos.Percent(50),
                Width = Dim.Percent(75),
                Height = Dim.Percent(50)
            };

            var TaskDetailsLabel = new Label(string.Format("Title: {0}\nCreation Date: {1}\nEnd Date: {2}\nPriority {3}\nDone: {4}\nDescription: {5}",
            Tasks.FormatTitle(selectedtask, 50), selectedtask.creationdate, selectedtask.enddate.ToString("dd.MM.yyyy"), Tasks.priorities[selectedtask.priority],
            selectedtask.isdone ? "Yes" : "No", selectedtask.description))
            {
                X = 1,
                Y = 0,
                Width = Dim.Percent(75),
                Height = Dim.Percent(50) - 1
            };

            win.Add(TaskDetailsLabel);

            FrameView SubTasksListFrameView = new FrameView("Subtasks")
            {
                X = 0,
                Y = Pos.Percent(50),
                Width = Dim.Percent(75),
                Height = Dim.Fill()
            };

            var SubTasksListLabel = new Label("Title                     End Date          Done")
            {
                X = 0,
                Y = 0
            };

            var SubTasksListView = new ListView(new TasksListDisplay(selectedtask.SubTasks))
            {
                X = 0,
                Y = 1,
                Width = Dim.Fill(),
                Height = Dim.Fill()
            };

            var ButtonExit = new Button("Exit")
            {
                X = Pos.Right(SubTasksListFrameView) + 1,
                Y = Pos.Bottom(win) - 4,
            };

            ButtonExit.Clicked += delegate ()
            {
                Application.RequestStop();
            };

            SubTasksListFrameView.Add(SubTasksListView, SubTasksListLabel);
            win.Add(SubTasksListFrameView, ButtonExit);

            SubTasksListView.OpenSelectedItem += delegate (ListViewItemEventArgs args)
            {
                if (selectedtask.SubTasks.Count > 0)
                {
                    SubTaskDetails(selectedtask.SubTasks[SubTasksListView.SelectedItem]);
                }
            };

            var menu = new MenuBar(new MenuBarItem[]
            {
                new MenuBarItem ("_File", new MenuItem []
                {
                    new MenuItem ("_Close", "", () => { NewTop.Running = false; }),
                    new MenuItem ("_Quit", "", () => { Application.Shutdown(); })
                }),
                new MenuBarItem ("_Order", new MenuItem []
                {
                    new MenuItem ("Title asc.", "", () =>
                    {
                        selectedtask.SortSubTasks(0);
                        lastsort = 0;
                        SubTasksListView.SetNeedsDisplay();
                    }),
                    new MenuItem ("Title desc.", "", () =>
                    {
                        selectedtask.SortSubTasks(1);
                        lastsort = 1;
                        SubTasksListView.SetNeedsDisplay();
                    }),
                    new MenuItem ("Creation Date asc.", "", () =>
                    {
                        selectedtask.SortSubTasks(2);
                        lastsort = 2;
                        SubTasksListView.SetNeedsDisplay();
                    }),
                    new MenuItem ("Creation Date desc.", "", () =>
                    {
                        selectedtask.SortSubTasks(3);
                        lastsort = 3;
                        SubTasksListView.SetNeedsDisplay();
                    }),
                    new MenuItem ("End Date asc.", "", () =>
                    {
                        selectedtask.SortSubTasks(4);
                        lastsort = 4;
                        SubTasksListView.SetNeedsDisplay();
                    }),
                    new MenuItem ("End Date desc.", "", () =>
                    {
                        selectedtask.SortSubTasks(5);
                        lastsort = 5;
                        SubTasksListView.SetNeedsDisplay();
                    }),
                    new MenuItem ("Done asc.", "", () =>
                    {
                        selectedtask.SortSubTasks(6);
                        lastsort = 6;
                        SubTasksListView.SetNeedsDisplay();
                    }),
                    new MenuItem ("Done desc.", "", () =>
                    {
                        selectedtask.SortSubTasks(7);
                        lastsort = 7;
                        SubTasksListView.SetNeedsDisplay();
                    }),
                })
            });
            NewTop.Add(menu);

            var ButtonsFrameView = new FrameView("Options")
            {
                X = Pos.Right(SubTasksListFrameView),
                Y = 0,
                Width = Dim.Percent(26),
                Height = 8
            };

            var ButtonAdd = new Button("Add Subtask")
            {
                X = 0,
                Y = 1
            };

            var ButtonEdit = new Button("Edit Subtask")
            {
                X = 0,
                Y = Pos.Bottom(ButtonAdd) + 1
            };

            var ButtonDelete = new Button("Delete Subtask")
            {
                X = 0,
                Y = Pos.Bottom(ButtonEdit) + 1,
            };

            ButtonAdd.Clicked += delegate ()
            {
                if (ManageSubTask(selectedtask, null) == true)
                {
                    selectedtask.SortSubTasks(lastsort);
                    SubTasksListView.SetNeedsDisplay();
                    ButtonEdit.ColorScheme = ButtonEdit.SuperView.ColorScheme;
                    ButtonDelete.ColorScheme = ButtonDelete.SuperView.ColorScheme;
                }
            };

            ButtonEdit.Clicked += delegate ()
            {
                if (selectedtask.SubTasks.Count > 0)
                {
                    if (ManageSubTask(selectedtask, selectedtask.SubTasks[SubTasksListView.SelectedItem]) == true)
                    {
                        SubTasksListView.SetNeedsDisplay();
                    }
                }
            };

            ButtonDelete.Clicked += delegate ()
            {
                if (selectedtask.SubTasks.Count > 0)
                {
                    if (DeleteSubTask(selectedtask, SubTasksListView.SelectedItem) == true)
                    {
                        if (SubTasksListView.SelectedItem > selectedtask.SubTasks.Count - 1)
                        {
                            SubTasksListView.MoveUp();
                        }
                        SubTasksListView.SetNeedsDisplay();
                    }
                    if (Tasks.taskslist.Count.Equals(0))
                    {
                        ButtonEdit.ColorScheme = Colors.ColorSchemes["Inactive"];
                        ButtonDelete.ColorScheme = Colors.ColorSchemes["Inactive"];
                    }
                }
            };

            ButtonsFrameView.Initialized += delegate (Object obj, EventArgs args)
            {
                if (selectedtask.SubTasks.Count.Equals(0))
                {
                    ButtonEdit.ColorScheme = Colors.ColorSchemes["Inactive"];
                    ButtonDelete.ColorScheme = Colors.ColorSchemes["Inactive"];
                }
            };

            ButtonsFrameView.Add(ButtonAdd, ButtonEdit, ButtonDelete);
            win.Add(ButtonsFrameView);

            NewTop.Add(win);
            Application.Run(NewTop);
        }
        private static void SubTaskDetails(Tasks.SubTask subtask)
        {
            var ButtonOK = new Button("Ok", is_default: true);

            ButtonOK.Clicked += delegate ()
            {
                Application.RequestStop();
            };

            var d = new Dialog("Details:", 60, 20, ButtonOK);

            var SubTaskDetailsLabel = new Label(string.Format("Title: {0}\nCreation Date: {1}\nEnd Date: {2}\nDone: {3}\nDescription: {4}",
            Tasks.FormatTitle(subtask, 50), subtask.creationdate, subtask.enddate.ToString("dd.MM.yyyy"), subtask.isdone ? "Yes" : "No", subtask.description))
            {
                X = Pos.Center(),
                Y = Pos.Center()
            };

            d.Add(SubTaskDetailsLabel);

            Application.Run(d);
        }

        static private void TasksWindow()
        {
            Application.Init();
            Colors.ColorSchemes.Add("TextViewColor", new ColorScheme());
            Colors.ColorSchemes.Add("Inactive", new ColorScheme());
            Colors.ColorSchemes["TextViewColor"].Normal = Application.Driver.MakeAttribute(Color.Black, Color.DarkGray);
            Colors.ColorSchemes["Inactive"].Normal = Application.Driver.MakeAttribute(Color.Gray, Color.Blue);
            Colors.ColorSchemes["Inactive"].Focus = Colors.ColorSchemes["Inactive"].Normal;
            Colors.ColorSchemes["Inactive"].HotNormal = Colors.ColorSchemes["Inactive"].Normal;
            Colors.ColorSchemes["Inactive"].HotFocus = Colors.ColorSchemes["Inactive"].Normal;
            int lastsort = 3;
            Tasks.SortTasks(lastsort);

            var top = Application.Top;
            ListView TasksListView;

            var win = new Window("Tasks List")
            {
                X = 0,
                Y = 1,

                Width = Dim.Fill(),
                Height = Dim.Fill()
            };

            top.Add(win);

            FrameView TasksListFrameView = new FrameView()
            {
                X = 0,
                Y = 0,
                Width = Dim.Percent(79),
                Height = Dim.Fill()
            };

            var TasksListLabel = new Label("Title                     End Date      Priority      Done")
            {
                X = 0,
                Y = 0
            };

            TasksListView = new ListView(new TasksListDisplay(Tasks.taskslist))
            {
                X = 0,
                Y = 1,
                Width = Dim.Fill(),
                Height = Dim.Fill()
            };

            TasksListFrameView.Add(TasksListView, TasksListLabel);

            var menu = new MenuBar(new MenuBarItem[]
            {
                new MenuBarItem ("_File", new MenuItem []
                {
                    new MenuItem ("_Quit", "", () => { Application.Shutdown(); })
                }),
                new MenuBarItem ("_Order", new MenuItem []
                {
                    new MenuItem ("Title asc.", "", () =>
                    {
                        Tasks.SortTasks(0);
                        lastsort = 0;
                        TasksListView.SetNeedsDisplay();
                    }),
                    new MenuItem ("Title desc.", "", () =>
                    {
                        Tasks.SortTasks(1);
                        lastsort = 1;
                        TasksListView.SetNeedsDisplay();
                    }),
                    new MenuItem ("Creation Date asc.", "", () =>
                    {
                        Tasks.SortTasks(2);
                        lastsort = 2;
                        TasksListView.SetNeedsDisplay();
                    }),
                    new MenuItem ("Creation Date desc.", "", () =>
                    {
                        Tasks.SortTasks(3);
                        lastsort = 3;
                        TasksListView.SetNeedsDisplay();
                    }),
                    new MenuItem ("End Date asc.", "", () =>
                    {
                        Tasks.SortTasks(4);
                        lastsort = 4;
                        TasksListView.SetNeedsDisplay();
                    }),
                    new MenuItem ("End Date desc.", "", () =>
                    {
                        Tasks.SortTasks(5);
                        lastsort = 5;
                        TasksListView.SetNeedsDisplay();
                    }),
                    new MenuItem ("Priority asc.", "", () =>
                    {
                        Tasks.SortTasks(6);
                        lastsort = 6;
                        TasksListView.SetNeedsDisplay();
                    }),
                    new MenuItem ("Priority desc.", "", () =>
                    {
                        Tasks.SortTasks(7);
                        lastsort = 7;
                        TasksListView.SetNeedsDisplay();
                    }),
                    new MenuItem ("Done asc.", "", () =>
                    {
                        Tasks.SortTasks(8);
                        lastsort = 8;
                        TasksListView.SetNeedsDisplay();
                    }),
                    new MenuItem ("Done desc.", "", () =>
                    {
                        Tasks.SortTasks(8);
                        lastsort = 9;
                        TasksListView.SetNeedsDisplay();
                    }),
                })
            });
            top.Add(menu);

            var ButtonsFrameView = new FrameView("Options")
            {
                X = Pos.Right(TasksListFrameView),
                Y = 0,
                Width = Dim.Percent(22),
                Height = 8
            };

            var ButtonAdd = new Button("Add Task")
            {
                X = 0,
                Y = 1
            };

            var ButtonEdit = new Button("Edit Task")
            {
                X = 0,
                Y = Pos.Bottom(ButtonAdd) + 1
            };

            var ButtonDelete = new Button("Delete Task")
            {
                X = 0,
                Y = Pos.Bottom(ButtonEdit) + 1,
            };

            ButtonAdd.Clicked += delegate ()
            {
                if (ManageTask(null) == true)
                {
                    Tasks.SortTasks(lastsort);
                    TasksListView.SetNeedsDisplay();
                    ButtonEdit.ColorScheme = ButtonEdit.SuperView.ColorScheme;
                    ButtonDelete.ColorScheme = ButtonDelete.SuperView.ColorScheme;
                }
            };

            ButtonEdit.Clicked += delegate ()
            {
                if (Tasks.taskslist.Count > 0)
                {
                    if (ManageTask(Tasks.taskslist[TasksListView.SelectedItem]) == true)
                    {
                        TasksListView.SetNeedsDisplay();
                    }
                }
            };

            ButtonDelete.Clicked += delegate ()
            {
                if (Tasks.taskslist.Count > 0)
                {
                    if (DeleteTask(TasksListView.SelectedItem) == true)
                    {
                        if (TasksListView.SelectedItem > Tasks.taskslist.Count - 1)
                        {
                            TasksListView.MoveUp();
                        }
                        TasksListView.SetNeedsDisplay();
                    }
                    if (Tasks.taskslist.Count.Equals(0))
                    {
                        ButtonEdit.ColorScheme = Colors.ColorSchemes["Inactive"];
                        ButtonDelete.ColorScheme = Colors.ColorSchemes["Inactive"];
                    }
                }
            };

            TasksListView.OpenSelectedItem += delegate (ListViewItemEventArgs args)
            {
                if (Tasks.taskslist.Count > 0)
                {
                    TaskDetails(Tasks.taskslist[TasksListView.SelectedItem]);
                }
            };

            ButtonsFrameView.Initialized += delegate (Object obj, EventArgs args)
            {
                if (Tasks.taskslist.Count.Equals(0))
                {
                    ButtonEdit.ColorScheme = Colors.ColorSchemes["Inactive"];
                    ButtonDelete.ColorScheme = Colors.ColorSchemes["Inactive"];
                }
            };

            var ASCIIlabel = new Label("       .@\n     ,@@@@\n   ,@@@.  &@@,\n @@@@   &@@@@@@@\n@@@@   &@&   @@@@\n '.@@@.   .@@@@'\n   '@@@@@@@@@'\n      '@@@'")
            {
                X = Pos.Right(TasksListFrameView),
                Y = Pos.Bottom(ButtonsFrameView) + 5
            };

            ButtonsFrameView.Add(ButtonAdd, ButtonEdit, ButtonDelete);
            win.Add(TasksListFrameView, ButtonsFrameView, ASCIIlabel);

            Application.Run();
        }

        static void Main()
        {
            CultureInfo.CurrentCulture = new CultureInfo("pl-PL");
            DB = new TasksDBConnection();

            TasksWindow();
        }
    }
}