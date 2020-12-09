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
            // Equivalent to an interpolated string like $"{Scenarios[item].Name, -widtestname}"; if such a thing were possible
            //var s = String.Format(String.Format("{{0,{0}}}", -_nameColumnWidth), Scenario.ScenarioMetadata.GetName(Scenarios[item]));
            //RenderUstr(driver, $"{s}  {Scenario.ScenarioMetadata.GetDescription(Scenarios[item])}", col, line, width);
            string s;
            if(TasksList != null)
            {
                s = string.Format("{0,-25} {1}       {2,-9}    {3}",
                    Tasks.FormatTitle(TasksList[item], 25) /*TasksList[item].title.Substring(0, TasksList[item].title.Length < 25 ? TasksList[item].title.Length : 22)
                    + (TasksList[item].title.Length < 25 ? "" : "...")*/,
                    TasksList[item].enddate.ToString("dd.MM.yy"), Tasks.priorities[TasksList[item].priority], (TasksList[item].isdone ? "Done" : ""));
                RenderUstr(container, driver, s, col, line, width, TasksList[item].priority, TasksList[item].enddate, selected, TasksList[item].isdone);
            }
            else
            {
                s = string.Format("{0,-25} {1}       {2}",
                    Tasks.FormatTitle(SubTasksList[item], 25) /*SubTasksList[item].title.Substring(0, SubTasksList[item].title.Length < 25 ? SubTasksList[item].title.Length : 22)
                    + (SubTasksList[item].title.Length < 25 ? "" : "...")*/,
                    SubTasksList[item].enddate.ToString("dd.MM.yy"), (SubTasksList[item].isdone ? "Done" : ""));
                RenderUstr(container, driver, s, col, line, width, -1, SubTasksList[item].enddate, selected, SubTasksList[item].isdone);
            }
        }

        private void RenderUstr(ListView container, ConsoleDriver driver, ustring ustr, int col, int line, int width, int priority, DateTime date, bool selected, bool isdone)
        {
            int used = 0;
            int index = 0;
            if(isdone == true)
            {
                SetColor(container, driver, (int)styles.normal, selected, selected ? Color.Green : Color.BrightGreen);
                /*driver.SetAttribute(driver.MakeAttribute(selected ? Color.Green : Color.BrightGreen, 
                    container.HasFocus ? (selected ? Color.Gray : Color.Blue) : Color.Blue));*/
            }
            else if(date <= DateTime.Now)
            {
                SetColor(container, driver, (int)styles.late, selected, Color.BrightRed);
                /*driver.SetAttribute(driver.MakeAttribute(Color.BrightRed, 
                    container.HasFocus ? (selected ? Color.BrightYellow : Color.Red) : Color.Red));*/
            }
            else
            {
                switch(Enum.GetName(typeof(priorities), priority))
                {
                    case "VeryHigh":
                        {
                            SetColor(container, driver, (int)styles.normal, selected, selected ? Color.Magenta : Color.BrightMagenta);
                            /*driver.SetAttribute(driver.MakeAttribute(selected ? Color.Magenta : Color.BrightMagenta, 
                                container.HasFocus ? (selected ? Color.Gray : Color.Blue) : Color.Blue));*/
                            break;
                        }
                    case "High":
                        {
                            SetColor(container, driver, (int)styles.dark, selected, Color.BrightYellow);
                            /*driver.SetAttribute(driver.MakeAttribute(Color.BrightYellow, 
                                container.HasFocus ? (selected ? Color.Gray : Color.Blue) : Color.Blue));*/
                            break;
                        }
                    case "Medium":
                        {
                            SetColor(container, driver, (int)styles.dark, selected, Color.BrighCyan);
                            /*driver.SetAttribute(driver.MakeAttribute(Color.BrighCyan, 
                                container.HasFocus ? (selected ? Color.Gray : Color.Blue) : Color.Blue));*/
                            break;
                        }
                    case "Low":
                        {
                            SetColor(container, driver, (int)styles.normal, selected, Color.Cyan);
                            /*driver.SetAttribute(driver.MakeAttribute(Color.Cyan, 
                                container.HasFocus ? (selected ? Color.Gray : Color.Blue) : Color.Blue));*/
                            break;
                        }
                    case "VeryLow":
                        {
                            SetColor(container, driver, (int)styles.normal, selected, selected ? Color.Black : Color.DarkGray);
                            /*driver.SetAttribute(driver.MakeAttribute(selected ? Color.Black : Color.DarkGray, 
                                container.HasFocus ? (selected ? Color.Gray : Color.Blue) : Color.Blue));*/
                            break;
                        }
                    default:
                        {
                            SetColor(container, driver, (int)styles.normal, selected, selected ? Color.Black : Color.White);
                            /*driver.SetAttribute(driver.MakeAttribute(selected ? Color.Black : Color.White, 
                                container.HasFocus ? (selected ? Color.Gray : Color.Blue) : Color.Blue));*/
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
