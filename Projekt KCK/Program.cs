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
                ColorScheme = Colors.ColorSchemes["Test"]
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
                if (TaskTitleField.Text.Length != 0)
                {
                    IsCreate = true;
                    Application.RequestStop();
                }
                else
                {
                    TitleEmptyDialog();
                }
            };

            ButtonOK.KeyDown += delegate (KeyEventEventArgs args)
            {
                if (args.KeyEvent.Key == Key.Enter)
                {
                    if (TaskTitleField.Text.Length != 0)
                    {
                        IsCreate = true;
                        Application.RequestStop();
                    }
                    else
                    {
                        TitleEmptyDialog();
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
                Tasks.taskslist.Add(new Tasks.Task(TaskTitleField.Text.ToString(), TaskDescriptionField.Text.ToString(), TaskEndDateField.Date,
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
                Text = selectedtask.description,
                ColorScheme = Colors.ColorSchemes["Test"]
            };

            var ButtonOK = new Button("Ok", is_default: true);
            ButtonOK.MouseClick += delegate (MouseEventArgs args)
            {
                if (TaskTitleField.Text.Length != 0)
                {
                    IsEdit = true;
                    Application.RequestStop();
                }
                else
                {
                    TitleEmptyDialog();
                }
            };
            ButtonOK.KeyDown += delegate (KeyEventEventArgs args)
            {
                if (args.KeyEvent.Key == Key.Enter)
                {
                    if (TaskTitleField.Text.Length != 0)
                    {
                        IsEdit = true;
                        Application.RequestStop();
                    }
                    else
                    {
                        TitleEmptyDialog();
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
                selectedtask.Update(TaskTitleField.Text.ToString(), TaskDescriptionField.Text.ToString(),
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

            var d = new Dialog("Delete Task " + Tasks.taskslist[taskindex].title, 60, 20, ButtonYes, ButtonNo);

            var ConfirmationLabel = new Label("Are you sure you want to delete this task?")
            {
                X = Pos.Center(),
                Y = Pos.Center()
            };

            d.Add(ConfirmationLabel);

            Application.Run(d);

            if (IsDelete)
            {
                Tasks.taskslist.RemoveAt(taskindex);
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
                ColorScheme = Colors.ColorSchemes["Test"]
            };

            var ButtonOK = new Button("Ok", is_default: true);
            ButtonOK.MouseClick += delegate (MouseEventArgs args)
            {
                if (TaskTitleField.Text.Length != 0)
                {
                    IsCreate = true;
                    Application.RequestStop();
                }
                else
                {
                    TitleEmptyDialog();
                }
            };

            ButtonOK.KeyDown += delegate (KeyEventEventArgs args)
            {
                if (args.KeyEvent.Key == Key.Enter)
                {
                    if (TaskTitleField.Text.Length != 0)
                    {
                        IsCreate = true;
                        Application.RequestStop();
                    }
                    else
                    {
                        TitleEmptyDialog();
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
                selectedtask.SubTasks.Add(new Tasks.SubTask(TaskTitleField.Text.ToString(), TaskDescriptionField.Text.ToString(), TaskEndDateField.Date));
            }

            return IsCreate;
        }
        static private bool EditSubTask(Tasks.SubTask selectedtask)
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
                Text = selectedtask.description,
                ColorScheme = Colors.ColorSchemes["Test"]
            };

            var ButtonOK = new Button("Ok", is_default: true);
            ButtonOK.MouseClick += delegate (MouseEventArgs args)
            {
                if (TaskTitleField.Text.Length != 0)
                {
                    IsEdit = true;
                    Application.RequestStop();
                }
                else
                {
                    TitleEmptyDialog();
                }
            };

            ButtonOK.KeyDown += delegate (KeyEventEventArgs args)
            {
                if (args.KeyEvent.Key == Key.Enter)
                {
                    if (TaskTitleField.Text.Length != 0)
                    {
                        IsEdit = true;
                        Application.RequestStop();
                    }
                    else
                    {
                        TitleEmptyDialog();
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
                selectedtask.Update(TaskTitleField.Text.ToString(), TaskDescriptionField.Text.ToString(), TaskIsDoneCheckBox.Checked, TaskEndDateField.Date);
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

            var d = new Dialog("Delete Subtask " + Tasks.taskslist[taskindex].title, 60, 20, ButtonYes, ButtonNo);

            var ConfirmationLabel = new Label("Are you sure you want to delete this subtask?")
            {
                X = Pos.Center(),
                Y = Pos.Center()
            };

            d.Add(ConfirmationLabel);

            Application.Run(d);

            if (IsDelete)
            {
                selectedtask.SubTasks.RemoveAt(taskindex);
            }

            return (IsDelete);
        }
        static private void TaskDetails(Tasks.Task selectedtask)
        {
            int lastsort = 3;
            var NewTop = new Toplevel(Application.Top.Frame);

            Window win = new Window("Details: " + Tasks.FormatTitle(selectedtask, 43))
            {
                X = 0,
                Y = 1,
                Width = Dim.Fill(),
                Height = Dim.Fill()
            };

            FrameView TaskDetailsFrameView = new FrameView()
            {
                X = 0,
                Y = Pos.Percent(50),
                Width = Dim.Percent(75),
                Height = Dim.Percent(50)
            };

            FrameView SubTasksListFrameView = new FrameView("Subtasks")
            {
                X = 0,
                //Y = 0,
                Y = Pos.Percent(50),
                Width = Dim.Percent(75),
                Height = Dim.Fill()
            };

            var SubTasksListLabel = new Label("Title                     End Date          Done")
            {
                X = 0,
                Y = 0
            };

            //ListView SubTasksListView = new ListView(selectedtask.SubTasks.Select(x => x.title + (x.isdone ? " Done" : " Not")).ToList())
            ListView SubTasksListView = new ListView(new TasksListDisplay(selectedtask.SubTasks))
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

            // Creates a menubar, the item "New" has a help menu.
            var menu = new MenuBar(new MenuBarItem[]
            {
                new MenuBarItem ("_File", new MenuItem []
                {
                    //new MenuItem ("_New", "Creates new file", NewFile),
                    //new MenuItem ("_Close", "", () => Close ()),
                    //new MenuItem ("_Quit", "", () => { if (Quit ()) top.Running = false; })
                    new MenuItem ("_Close", "", () => { NewTop.Running = false; })
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
                //X = 2,
                //Y = Pos.Bottom(TaskListView)
                X = 0,
                Y = 1
            };

            var ButtonEdit = new Button("Edit Subtask")
            {
                //X = Pos.Right(ButtonAdd) + 1,
                //Y = Pos.Bottom(TaskListView),
                X = 0,
                Y = Pos.Bottom(ButtonAdd) + 1
            };

            var ButtonDelete = new Button("Delete Subtask")
            {
                //X = Pos.Right(ButtonEdit) + 1,
                //Y = Pos.Bottom(TaskListView)
                X = 0,
                Y = Pos.Bottom(ButtonEdit) + 1,
            };

            ButtonAdd.MouseClick += delegate (MouseEventArgs args)
            {
                if (AddSubTask(selectedtask) == true)
                {
                    //SubTasksListView.SetSource(selectedtask.SubTasks.Select(x => x.title + (x.isdone ? " Done" : " Not")).ToList());
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
                        //SubTasksListView.SetSource(selectedtask.SubTasks.Select(x => x.title + (x.isdone ? " Done" : " Not")).ToList());
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
                    if (EditSubTask(selectedtask.SubTasks[SubTasksListView.SelectedItem]) == true)
                    {
                        //SubTasksListView.SetSource(selectedtask.SubTasks.Select(x => x.title + (x.isdone ? " Done" : " Not")).ToList());
                        SubTasksListView.SetNeedsDisplay();
                    }
                }
            };

            ButtonEdit.KeyDown += delegate (KeyEventEventArgs args)
            {
                if (selectedtask.SubTasks.Count > 0 && args.KeyEvent.Key == Key.Enter)
                {
                    if (EditSubTask(selectedtask.SubTasks[SubTasksListView.SelectedItem]) == true)
                    {
                        //SubTasksListView.SetSource(selectedtask.SubTasks.Select(x => x.title + (x.isdone ? " Done" : " Not")).ToList());
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
                        //SubTasksListView.SetSource(selectedtask.SubTasks.Select(x => x.title + (x.isdone ? " Done" : " Not")).ToList());
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
                        //SubTasksListView.SetSource(selectedtask.SubTasks.Select(x => x.title + (x.isdone ? " Done" : " Not")).ToList());
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

        private static void TitleEmptyDialog()
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

            var d = new Dialog("Error!", 38, 10, ButtonOK);

            var ErrorLabel = new Label("The Title field must not be empty!")
            {
                X = Pos.Center(),
                Y = Pos.Center()
            };

            d.Add(ErrorLabel);

            Application.Run(d);
        }

        static private void TasksWindow()
        {
            Application.Init();
            Colors.ColorSchemes.Add("Test", new ColorScheme());
            Colors.ColorSchemes.Add("Inactive", new ColorScheme());
            Colors.ColorSchemes["Test"].Normal = Application.Driver.MakeAttribute(Color.Black, Color.Cyan);
            Colors.ColorSchemes["Inactive"].Normal = Application.Driver.MakeAttribute(Color.Gray, Color.Blue);
            Colors.ColorSchemes["Inactive"].Focus = Colors.ColorSchemes["Inactive"].Normal;
            Colors.ColorSchemes["Inactive"].HotNormal = Colors.ColorSchemes["Inactive"].Normal;
            Colors.ColorSchemes["Inactive"].HotFocus = Colors.ColorSchemes["Inactive"].Normal;
            int lastsort = 3;
            //TasksListDisplay tasksListDisplay = new TasksListDisplay(Tasks.taskslist);

            var top = Application.Top;
            ListView TasksListView;

            // Creates the top-level window to show
            var win = new Window("Tasks List")
            {
                X = 0,
                Y = 1, // Leave one row for the toplevel menu

                // By using Dim.Fill(), it will automatically resize without manual intervention
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

            //TasksListView = new ListView(Tasks.taskslist.Select(x => x.title + (x.isdone ? " Done" : " Not")).ToList())
            TasksListView = new ListView(new TasksListDisplay(Tasks.taskslist))
            {
                X = 0,
                Y = 1,
                Width = Dim.Fill(),
                Height = Dim.Fill()
            };

            TasksListFrameView.Add(TasksListView, TasksListLabel);

            // Creates a menubar, the item "New" has a help menu.
            var menu = new MenuBar(new MenuBarItem[]
            {
                new MenuBarItem ("_File", new MenuItem []
                {
                    //new MenuItem ("_New", "Creates new file", NewFile),
                    //new MenuItem ("_Close", "", () => Close ()),
                    //new MenuItem ("_Quit", "", () => { if (Quit ()) top.Running = false; })
                    new MenuItem ("_Quit", "", () => { top.Running = false; })
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
                    //TasksListView.SetSource(Tasks.taskslist.Select(x => x.title + (x.isdone ? " Done" : " Not")).ToList());
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
                        //TasksListView.SetSource(Tasks.taskslist.Select(x => x.title + (x.isdone ? " Done" : " Not")).ToList());
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
                        //TasksListView.SetSource(Tasks.taskslist.Select(x => x.title + (x.isdone ? " Done" : " Not")).ToList());
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
                        //TasksListView.SetSource(Tasks.taskslist.Select(x => x.title + (x.isdone ? " Done" : " Not")).ToList());
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
                        //TasksListView.SetSource(Tasks.taskslist.Select(x => x.title + (x.isdone ? " Done" : " Not")).ToList());
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
                        //TasksListView.SetSource(Tasks.taskslist.Select(x => x.title + (x.isdone ? " Done" : " Not")).ToList());
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

            /*var login = new Label("Login: ") { X = 3, Y = 2 };
            var password = new Label("Password: ")
            {
                X = Pos.Left(login),
                Y = Pos.Top(login) + 1
            };
            var loginText = new TextField("")
            {
                X = Pos.Right(password),
                Y = Pos.Top(login),
                Width = 40
            };
            var passText = new TextField("")
            {
                Secret = true,
                X = Pos.Left(loginText),
                Y = Pos.Top(password),
                Width = Dim.Width(loginText)
            };

            // Add some controls, 
            win.Add(
                // The ones with my favorite layout system, Computed
                login, password, loginText, passText,

                // The ones laid out like an australopithecus, with Absolute positions:
                new CheckBox(3, 6, "Remember me"),
                //new RadioGroup(3, 8, new[] { "_Personal", "_Company" }),
                new Button(3, 14, "Ok"),
                new Button(10, 14, "Cancel"),
                new Label(3, 18, "Press F9 or ESC plus 9 to activate the menubar")
            );*/

            Application.Run();
        }

        static void Main()
        {
            CultureInfo.CurrentCulture = new CultureInfo("pl-PL");

            TasksWindow();
        }
    }
}
