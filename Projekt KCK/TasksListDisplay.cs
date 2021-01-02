using System;
using System.Collections.Generic;
using NStack;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terminal.Gui;
using ListaZadanFunkcjonalnosc;
using System.Collections;

namespace ListaRzeczyTUI
{
    public class TasksListDisplay : IListDataSource
    {
        //Application.Driver.SetAttribute(Application.Driver.MakeAttribute(Color.Black, Color.Cyan));

        private enum priorities
        {
            VeryHigh = 0,
            High = 1,
            Medium = 2,
            Low = 3,
            VeryLow = 4
        }

        private enum styles
        {
            normal = 0,
            dark = 1,
            late = 2
        }

        private List<Tasks.Task> TasksList;
        private List<Tasks.SubTask> SubTasksList;
        public int Count => TasksList != null ? TasksList.Count : SubTasksList.Count;

        public TasksListDisplay(List<Tasks.Task> TasksList)
        {
            this.TasksList = TasksList;
        }
        public TasksListDisplay(List<Tasks.SubTask> SubTasksList)
        {
            this.SubTasksList = SubTasksList;
        }

        public void Render(ListView container, ConsoleDriver driver, bool selected, int item, int col, int line, int width)
        {
            container.Move(col, line);

            string s;
            if(TasksList != null)
            {
                s = string.Format("{0,-25} {1}      {2,-9}     {3}",
                    Tasks.FormatTitle(TasksList[item], 25),
                    TasksList[item].enddate.ToString("dd.MM.yy"), Tasks.priorities[TasksList[item].priority], (TasksList[item].isdone ? "Done" : ""));
                RenderUstr(container, driver, s, col, line, width, TasksList[item].priority, TasksList[item].enddate, selected, TasksList[item].isdone);
            }
            else
            {
                s = string.Format("{0,-25} {1}          {2}",
                    Tasks.FormatTitle(SubTasksList[item], 25),
                    SubTasksList[item].enddate.ToString("dd.MM.yy"), (SubTasksList[item].isdone ? "Done" : ""));
                RenderUstr(container, driver, s, col, line, width, -1, SubTasksList[item].enddate, selected, SubTasksList[item].isdone);
            }
        }

        private void RenderUstr(ListView container, ConsoleDriver driver, ustring ustr, int col, int line, int width, int priority, DateTime enddate, bool selected, bool isdone)
        {
            int used = 0;
            int index = 0;
            if(isdone == true)
            {
                SetColor(container, driver, (int)styles.normal, selected, selected ? Color.Green : Color.BrightGreen);
            }
            else if(enddate <= DateTime.Now)
            {
                SetColor(container, driver, (int)styles.late, selected, Color.BrightRed);
            }
            else
            {
                switch(Enum.GetName(typeof(priorities), priority))
                {
                    case "VeryHigh":
                        {
                            SetColor(container, driver, (int)styles.normal, selected, selected ? Color.Magenta : Color.BrightMagenta);
                            break;
                        }
                    case "High":
                        {
                            SetColor(container, driver, (int)styles.dark, selected, Color.BrightYellow);
                            break;
                        }
                    case "Medium":
                        {
                            SetColor(container, driver, (int)styles.dark, selected, Color.BrighCyan);
                            break;
                        }
                    case "Low":
                        {
                            SetColor(container, driver, (int)styles.normal, selected, Color.Cyan);
                            break;
                        }
                    case "VeryLow":
                        {
                            SetColor(container, driver, (int)styles.normal, selected, selected ? Color.Black : Color.DarkGray);
                            break;
                        }
                    default:
                        {
                            SetColor(container, driver, (int)styles.normal, selected, selected ? Color.Black : Color.White);
                            break;
                        }
                }
            }
            while (index < ustr.Length)
            {
                (var rune, var size) = Utf8.DecodeRune(ustr, index, index - ustr.Length);
                var count = Rune.ColumnWidth(rune);
                if (used + count >= width) break;
                driver.AddRune(rune);
                used += count;
                index += size;
            }

            while (used < width)
            {
                driver.AddRune(' ');
                used++;
            }

            driver.SetAttribute(Application.Driver.MakeAttribute(Color.Black, Color.Blue));
        }

        private void SetColor (ListView container, ConsoleDriver driver, int style, bool selected, Color color)
        {
            switch(Enum.GetName(typeof(styles), style))
            {
                case "normal":
                    {
                        driver.SetAttribute(driver.MakeAttribute(color, container.HasFocus ? (selected ? Color.Gray : Color.Blue) : Color.Blue));
                        break;
                    }
                case "dark":
                    {
                        driver.SetAttribute(driver.MakeAttribute(color, container.HasFocus ? (selected ? Color.DarkGray : Color.Blue) : Color.Blue));
                        break;
                    }
                case "late":
                    {
                        driver.SetAttribute(driver.MakeAttribute(color, container.HasFocus ? (selected ? Color.BrightYellow : Color.Red) : Color.Red));
                        break;
                    }
            }
        }

        public bool IsMarked(int item)
        {
            return false;
        }

        public void SetMark(int item, bool value)
        {
        }

        public IList ToList()
        {
            if(TasksList != null)
            {
                return TasksList;
            }
            else
            {
                return SubTasksList;
            }
        }
    }
}
