using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terminal.Gui;
using ListaZadanFunkcjonalnosc;
using System.Globalization;

namespace ListaRzeczyTUI
{
    class Program : Window
    {
        static TasksDBConnection DB;
        static private bool AddTask()
        {
            bool IsCreate = false;

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

            var TaskEndDateField = new DateField(DateTime.Today.AddDays(1))
            {
                X = Pos.Right(TaskDescriptionLabel),
                Y = Pos.Top(TaskEndDateLabel),
                Width = Dim.Fill() - 3
            };

            var TaskPriorityField = new ComboBox()
            {
                X = Pos.Right(TaskDescriptionLabel),
                Y = Pos.Top(TaskPriorityLabel),
                Width = Dim.Fill() - 3
            };
            TaskPriorityField.SetSource(Tasks.priorities.Values.ToList());
            TaskPriorityField.Text = "Medium";

            var TaskDescriptionField = new TextView()
            {
                X = Pos.Right(TaskDescriptionLabel),
                Y = Pos.Top(TaskDescriptionLabel),
                Width = Dim.Fill() - 3,
                Height = Dim.Fill() - 2,
                ColorScheme = Colors.ColorSchemes["TextViewColor"]
            };

            var TaskDescriptionScrollBar = new ScrollBarView()
            {
                X = Pos.Right(TaskDescriptionField),
                Y = Pos.Top(TaskDescriptionField),
                Size = 1,
                Width = 1,
                Height = Dim.Fill() - 2,
                IsVertical = true
            };

            var ButtonOK = new Button("Ok", is_default: true);
            ButtonOK.MouseClick += delegate (MouseEventArgs args)
            {
                if (TaskTitleField.Text.Length > 0 && TaskTitleField.Text.Length <= 50)
                {
                    IsCreate = true;
                    Application.RequestStop();
                }
                else
                {
                    TitleControlDialog();
                }
            };

            ButtonOK.KeyDown += delegate (KeyEventEventArgs args)
            {
                if (args.KeyEvent.Key == Key.Enter)
                {
                    if (TaskTitleField.Text.Length > 0 && TaskTitleField.Text.Length <= 50)
                    {
                        IsCreate = true;
                        Application.RequestStop();
                    }
                    else
                    {
                        TitleControlDialog();
                    }
                }
            };

            var ButtonCancel = new Button("Cancel");
            ButtonCancel.MouseClick += delegate (MouseEventArgs args)
            {
                Application.RequestStop();
            };

            ButtonCancel.KeyDown += delegate (KeyEventEventArgs args)
            {
                if (args.KeyEvent.Key == Key.Enter)
                {
                    Application.RequestStop();
                }
            };

            var d = new Dialog("New Task", 60, 20, ButtonOK, ButtonCancel);

            d.Add(TaskTitleLabel, TaskEndDateLabel, TaskPriorityLabel, TaskDescriptionLabel);
            d.Add(TaskTitleField, TaskEndDateField, TaskPriorityField, TaskDescriptionField);

            Application.Run(d);

            if (IsCreate)
            {
                //Tasks.taskslist.Add(tmp);
                DB.AddTask(new Tasks.Task(TaskTitleField.Text.ToString(), TaskDescriptionField.Text.ToString(), TaskEndDateField.Date,
                    Tasks.prioritiesinv[TaskPriorityField.Text.ToString()]));

            }

            return IsCreate;
        }
        static private bool EditTask(Tasks.Task selectedtask)
        {
            bool IsEdit = false;

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
                Width = Dim.Fill() - 3,
                Text = selectedtask.title
            };

            var TaskEndDateField = new DateField(selectedtask.enddate)
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
                Y = Pos.Top(TaskEndDateLabel),
                Checked = selectedtask.isdone
            };

            var TaskPriorityField = new ComboBox()
            {
                X = Pos.Right(TaskDescriptionLabel),
                Y = Pos.Top(TaskPriorityLabel),
                Width = Dim.Fill() - 3
            };
            TaskPriorityField.SetSource(Tasks.priorities.Values.ToList());
            TaskPriorityField.Text = Tasks.priorities[selectedtask.priority];

            var TaskDescriptionField = new TextView()
            {
                X = Pos.Right(TaskDescriptionLabel),
                Y = Pos.Top(TaskDescriptionLabel),
                Width = Dim.Fill() - 3,
                Height = Dim.Fill() - 2,
                Text = selectedtask.description/*.Replace("\r", "\n")*/, //Dopisuje "?" przed każdą nową linią; wina biblioteki
                ColorScheme = Colors.ColorSchemes["TextViewColor"]
            };

            var ButtonOK = new Button("Ok", is_default: true);
            ButtonOK.MouseClick += delegate (MouseEventArgs args)
            {
                if (TaskTitleField.Text.Length > 0 && TaskTitleField.Text.Length <= 50)
                {
                    IsEdit = true;
                    Application.RequestStop();
                }
                else
                {
                    TitleControlDialog();
                }
            };
            ButtonOK.KeyDown += delegate (KeyEventEventArgs args)
            {
                if (args.KeyEvent.Key == Key.Enter)
                {
                    if (TaskTitleField.Text.Length > 0 && TaskTitleField.Text.Length <= 50)
                    {
                        IsEdit = true;
                        Application.RequestStop();
                    }
                    else
                    {
                        TitleControlDialog();
                    }
                }
            };

            var ButtonCancel = new Button("Cancel");
            ButtonCancel.MouseClick += delegate (MouseEventArgs args)
            {
                Application.RequestStop();
            };

            ButtonCancel.KeyDown += delegate (KeyEventEventArgs args)
            {
                if (args.KeyEvent.Key == Key.Enter)
                {
                    Application.RequestStop();
                }
            };

            var d = new Dialog("Edit Task " + Tasks.FormatTitle(selectedtask, 43), 60, 20, ButtonOK, ButtonCancel);

            d.Add(TaskTitleLabel, TaskEndDateLabel, TaskPriorityLabel, TaskDescriptionLabel);
            d.Add(TaskTitleField, TaskEndDateField, TaskPriorityField, TaskDescriptionField, TaskIsDoneLabel, TaskIsDoneCheckBox);

            Application.Run(d);

            if (IsEdit)
            {
                /*selectedtask.Update(TaskTitleField.Text.ToString(), TaskDescriptionField.Text.ToString(),
                    TaskIsDoneCheckBox.Checked, TaskEndDateField.Date, Tasks.prioritiesinv[TaskPriorityField.Text.ToString()]);*/
                DB.EditTask(selectedtask, TaskTitleField.Text.ToString(), TaskDescriptionField.Text.ToString(),
                    TaskIsDoneCheckBox.Checked, TaskEndDateField.Date, Tasks.prioritiesinv[TaskPriorityField.Text.ToString()]);
            }

            return (IsEdit);
        }
        static private bool DeleteTask(int taskindex)
        {
            bool IsDelete = false;

            var ButtonYes = new Button("Yes");
            ButtonYes.MouseClick += delegate (MouseEventArgs args)
            {
                IsDelete = true;
                Application.RequestStop();
            };
            ButtonYes.KeyDown += delegate (KeyEventEventArgs args)
            {
                if (args.KeyEvent.Key == Key.Enter)
                {
                    IsDelete = true;
                    Application.RequestStop();
                }
            };

            var ButtonNo = new Button("No", is_default: true);
            ButtonNo.MouseClick += delegate (MouseEventArgs args)
            {
                Application.RequestStop();
            };

            ButtonNo.KeyDown += delegate (KeyEventEventArgs args)
            {
                if (args.KeyEvent.Key == Key.Enter)
                {
                    Application.RequestStop();
                }
            };

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
                //Tasks.taskslist.RemoveAt(taskindex);
                DB.DeleteTask(taskindex);
            }

            return (IsDelete);
        }
        static private bool AddSubTask(Tasks.Task selectedtask)
        {
            bool IsCreate = false;

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

            var TaskEndDateField = new DateField(DateTime.Today.AddDays(1))
            {
                X = Pos.Right(TaskDescriptionLabel),
                Y = Pos.Top(TaskEndDateLabel),
                Width = Dim.Fill() - 3
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
            ButtonOK.MouseClick += delegate (MouseEventArgs args)
            {
                if (TaskTitleField.Text.Length > 0 && TaskTitleField.Text.Length <= 50)
                {
                    IsCreate = true;
                    Application.RequestStop();
                }
                else
                {
                    TitleControlDialog();
                }
            };

            ButtonOK.KeyDown += delegate (KeyEventEventArgs args)
            {
                if (args.KeyEvent.Key == Key.Enter)
                {
                    if (TaskTitleField.Text.Length > 0 && TaskTitleField.Text.Length <= 50)
                    {
                        IsCreate = true;
                        Application.RequestStop();
                    }
                    else
                    {
                        TitleControlDialog();
                    }
                }
            };

            var ButtonCancel = new Button("Cancel");
            ButtonCancel.MouseClick += delegate (MouseEventArgs args)
            {
                Application.RequestStop();
            };

            ButtonCancel.KeyDown += delegate (KeyEventEventArgs args)
            {
                if (args.KeyEvent.Key == Key.Enter)
                {
                    Application.RequestStop();
                }
            };

            var d = new Dialog("New Subtask", 60, 20, ButtonOK, ButtonCancel);

            d.Add(TaskTitleLabel, TaskEndDateLabel, TaskDescriptionLabel);
            d.Add(TaskTitleField, TaskEndDateField, TaskDescriptionField);

            Application.Run(d);

            if (IsCreate)
            {
                //selectedtask.SubTasks.Add(new Tasks.SubTask(TaskTitleField.Text.ToString(), TaskDescriptionField.Text.ToString(), TaskEndDateField.Date));
                DB.AddSubTask(selectedtask, new Tasks.SubTask(TaskTitleField.Text.ToString(), TaskDescriptionField.Text.ToString(), TaskEndDateField.Date));
            }

            return IsCreate;
        }
        static private bool EditSubTask(Tasks.Task parenttask, Tasks.SubTask selectedtask)
        {
            bool IsEdit = false;

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
                Width = Dim.Fill() - 3,
                Text = selectedtask.title
            };

            var TaskEndDateField = new DateField()
            {
                X = Pos.Right(TaskDescriptionLabel),
                Y = Pos.Top(TaskEndDateLabel),
                Date = selectedtask.enddate
            };
            
            var TaskIsDoneLabel = new Label("Done: ")
            {
                X = Pos.Right(TaskEndDateField) + 1,
                Y = Pos.Top(TaskEndDateLabel)
            };

            var TaskIsDoneCheckBox = new CheckBox()
            {
                X = Pos.Right(TaskIsDoneLabel),
                Y = Pos.Top(TaskEndDateLabel),
                Checked = selectedtask.isdone
            };

            var TaskDescriptionField = new TextView()
            {
                X = Pos.Right(TaskDescriptionLabel),
                Y = Pos.Top(TaskDescriptionLabel),
                Width = Dim.Fill() - 3,
                Height = Dim.Fill() - 2,
                Text = selectedtask.description/*.Replace("\r", "\n")*/,
                ColorScheme = Colors.ColorSchemes["TextViewColor"]
            };

            var ButtonOK = new Button("Ok", is_default: true);
            ButtonOK.MouseClick += delegate (MouseEventArgs args)
            {
                if (TaskTitleField.Text.Length > 0 && TaskTitleField.Text.Length <= 50)
                {
                    IsEdit = true;
                    Application.RequestStop();
                }
                else
                {
                    TitleControlDialog();
                }
            };

            ButtonOK.KeyDown += delegate (KeyEventEventArgs args)
            {
                if (args.KeyEvent.Key == Key.Enter)
                {
                    if (TaskTitleField.Text.Length > 0 && TaskTitleField.Text.Length <= 50)
                    {
                        IsEdit = true;
                        Application.RequestStop();
                    }
                    else
                    {
                        TitleControlDialog();
                    }
                }
            };

            var ButtonCancel = new Button("Cancel");
            ButtonCancel.MouseClick += delegate (MouseEventArgs args)
            {
                Application.RequestStop();
            };

            ButtonCancel.KeyDown += delegate (KeyEventEventArgs args)
            {
                if (args.KeyEvent.Key == Key.Enter)
                {
                    Application.RequestStop();
                }
            };

            var d = new Dialog("Edit Subtask " + Tasks.FormatTitle(selectedtask, 43), 60, 20, ButtonOK, ButtonCancel);

            d.Add(TaskTitleLabel, TaskEndDateLabel, TaskDescriptionLabel);
            d.Add(TaskTitleField, TaskEndDateField, TaskDescriptionField, TaskIsDoneLabel, TaskIsDoneCheckBox);

            Application.Run(d);

            if (IsEdit)
            {
                //selectedtask.Update(TaskTitleField.Text.ToString(), TaskDescriptionField.Text.ToString(), TaskIsDoneCheckBox.Checked, TaskEndDateField.Date);
                DB.EditSubTask(parenttask, selectedtask, TaskTitleField.Text.ToString(), TaskDescriptionField.Text.ToString(), TaskIsDoneCheckBox.Checked, TaskEndDateField.Date);
            }

            return IsEdit;
        }
        static private bool DeleteSubTask(Tasks.Task selectedtask, int taskindex)
        {
            bool IsDelete = false;

            var ButtonYes = new Button("Yes");
            ButtonYes.MouseClick += delegate (MouseEventArgs args)
            {
                IsDelete = true;
                Application.RequestStop();
            };
            ButtonYes.KeyDown += delegate (KeyEventEventArgs args)
            {
                if (args.KeyEvent.Key == Key.Enter)
                {
                    IsDelete = true;
                    Application.RequestStop();
                }
            };

            var ButtonNo = new Button("No", is_default: true);
            ButtonNo.MouseClick += delegate (MouseEventArgs args)
            {
                Application.RequestStop();
            };

            ButtonNo.KeyDown += delegate (KeyEventEventArgs args)
            {
                if (args.KeyEvent.Key == Key.Enter)
                {
                    Application.RequestStop();
                }
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
                //selectedtask.SubTasks.RemoveAt(taskindex);
                DB.DeleteSubTask(selectedtask, taskindex);
            }

            return (IsDelete);
        }
        static private void TaskDetails(Tasks.Task selectedtask)
        {
            int lastsort = 3;
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

            ButtonExit.MouseClick += delegate (MouseEventArgs args)
            {
                Application.RequestStop();
            };

            ButtonExit.KeyDown += delegate (KeyEventEventArgs args)
            {
                if (args.KeyEvent.Key == Key.Enter)
                {
                    Application.RequestStop();
                }
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
                new MenuBarItem ("_Edit", new MenuItem []
                {
                    new MenuItem ("_Copy", "", null),
                    new MenuItem ("C_ut", "", null),
                    new MenuItem ("_Paste", "", null)
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

            ButtonAdd.MouseClick += delegate (MouseEventArgs args)
            {
                if (AddSubTask(selectedtask) == true)
                {
                    selectedtask.SortSubTasks(lastsort);
                    SubTasksListView.SetNeedsDisplay();
                    ButtonEdit.ColorScheme = ButtonEdit.SuperView.ColorScheme;
                    ButtonDelete.ColorScheme = ButtonDelete.SuperView.ColorScheme;
                }
            };

            ButtonAdd.KeyDown += delegate (KeyEventEventArgs args)
            {
                if (args.KeyEvent.Key == Key.Enter)
                {
                    if (AddSubTask(selectedtask) == true)
                    {
                        selectedtask.SortSubTasks(lastsort);
                        SubTasksListView.SetNeedsDisplay();
                        ButtonEdit.ColorScheme = ButtonEdit.SuperView.ColorScheme;
                        ButtonDelete.ColorScheme = ButtonDelete.SuperView.ColorScheme;
                    }
                }
            };

            ButtonEdit.MouseClick += delegate (MouseEventArgs args)
            {
                if (selectedtask.SubTasks.Count > 0)
                {
                    if (EditSubTask(selectedtask, selectedtask.SubTasks[SubTasksListView.SelectedItem]) == true)
                    {
                        SubTasksListView.SetNeedsDisplay();
                    }
                }
            };

            ButtonEdit.KeyDown += delegate (KeyEventEventArgs args)
            {
                if (selectedtask.SubTasks.Count > 0 && args.KeyEvent.Key == Key.Enter)
                {
                    if (EditSubTask(selectedtask, selectedtask.SubTasks[SubTasksListView.SelectedItem]) == true)
                    {
                        SubTasksListView.SetNeedsDisplay();
                    }
                }
            };

            ButtonDelete.MouseClick += delegate (MouseEventArgs args)
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

            ButtonDelete.KeyDown += delegate (KeyEventEventArgs args)
            {
                if (selectedtask.SubTasks.Count > 0 && args.KeyEvent.Key == Key.Enter)
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

        private static void TitleControlDialog()
        {
            var ButtonOK = new Button("Ok", is_default: true);

            ButtonOK.MouseClick += delegate (MouseEventArgs args)
            {
                Application.RequestStop();
            };

            ButtonOK.KeyDown += delegate (KeyEventEventArgs args)
            {
                if (args.KeyEvent.Key == Key.Enter)
                {
                    Application.RequestStop();
                }
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

        private static void SubTaskDetails(Tasks.SubTask subtask)
        {
            var ButtonOK = new Button("Ok", is_default: true);

            ButtonOK.MouseClick += delegate (MouseEventArgs args)
            {
                Application.RequestStop();
            };

            ButtonOK.KeyDown += delegate (KeyEventEventArgs args)
            {
                if (args.KeyEvent.Key == Key.Enter)
                {
                    Application.RequestStop();
                }
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
                new MenuBarItem ("_Edit", new MenuItem []
                {
                    new MenuItem ("_Copy", "", null),
                    new MenuItem ("C_ut", "", null),
                    new MenuItem ("_Paste", "", null)
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

            ButtonAdd.MouseClick += delegate (MouseEventArgs args)
            {
                if (AddTask() == true)
                {
                    Tasks.SortTasks(lastsort);
                    TasksListView.SetNeedsDisplay();
                    ButtonEdit.ColorScheme = ButtonEdit.SuperView.ColorScheme;
                    ButtonDelete.ColorScheme = ButtonDelete.SuperView.ColorScheme;
                }
            };

            ButtonAdd.KeyDown += delegate (KeyEventEventArgs args)
            {
                if (args.KeyEvent.Key == Key.Enter)
                {
                    if (AddTask() == true)
                    {
                        Tasks.SortTasks(lastsort);
                        TasksListView.SetNeedsDisplay();
                        ButtonEdit.ColorScheme = ButtonEdit.SuperView.ColorScheme;
                        ButtonDelete.ColorScheme = ButtonDelete.SuperView.ColorScheme;
                    }
                }
            };

            ButtonEdit.MouseClick += delegate (MouseEventArgs args)
            {
                if (Tasks.taskslist.Count > 0)
                {
                    if(EditTask(Tasks.taskslist[TasksListView.SelectedItem]) == true)
                    {
                        TasksListView.SetNeedsDisplay();
                    }
                }
            };

            ButtonEdit.KeyDown += delegate (KeyEventEventArgs args)
            {
                if (Tasks.taskslist.Count > 0 && args.KeyEvent.Key == Key.Enter)
                {
                    if (EditTask(Tasks.taskslist[TasksListView.SelectedItem]) == true)
                    {
                        TasksListView.SetNeedsDisplay();
                    }
                }
            };

            ButtonDelete.MouseClick += delegate (MouseEventArgs args)
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

            ButtonDelete.KeyDown += delegate (KeyEventEventArgs args)
            {
                if (Tasks.taskslist.Count > 0 && args.KeyEvent.Key == Key.Enter)
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
                if(Tasks.taskslist.Count > 0)
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
            //TasksContext DB = new TasksContext();
            //DB.Tasks.Add(new Tasks.Task("sadasdasda", "adsadas", DateTime.Now, 0));
            //DB.Tasks.Remove(DB.Tasks.Find(2));
            //DB.Tasks.Remove(DB.Tasks.Find(3));
            //DB.SaveChanges();
            //var thing = DB.Tasks.ToList();
            //var thing = DB.Tasks.Find(0);

            TasksWindow();
        }
    }
}
