using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ListaZadanFunkcjonalnosc
{
    public abstract class Tasks
    {
        public static List<Task> taskslist;

        public static Dictionary<int, string> priorities = new Dictionary<int, string>()
        {
            { 0, "Very High" },
            { 1, "High" },
            { 2, "Medium" },
            { 3, "Low" },
            { 4, "Very Low" }
        };

        public static Dictionary<string, int> prioritiesinv = new Dictionary<string, int>()
        {
            { "Very High", 0 },
            { "High", 1 },
            { "Medium", 2 },
            { "Low", 3 },
            { "Very Low", 4 }
        };

        public enum sorttype
        {
            TitleAsc = 0,
            TitleDesc = 1,
            CreationDateAsc = 2,
            CreationDateDesc = 3,
            EndDateAsc = 4,
            EndDateDesc = 5,
            IsDoneAsc = 6,
            IsDoneDesc = 7,
            PriorityAsc = 8,
            PriorityDesc = 9
        }

        public interface ITask
        {
            string title { get;}
            string description { get; }
            bool isdone { get; }
            DateTime enddate { get; }
            DateTime creationdate { get; }
        }

        public class Task : ITask
        {
            public virtual List<SubTask> SubTasks { get; set; }

            public int id { get; set; }
            public string title { get; set; }
            public string description { get; set; }
            public bool isdone { get; set; }
            public DateTime enddate { get; set; }
            public DateTime creationdate { get; set; }
            public int priority { get; set; }

            public Task()
            {

            }

            public Task(string title, string description, DateTime enddate, int priority)
            {
                this.title = title;
                this.description = description;
                isdone = false;
                this.enddate = enddate;
                SubTasks = new List<SubTask>();
                this.priority = priority;
                this.creationdate = DateTime.Now;
            }

            public void Update(string title, string description, bool isdone, DateTime enddate, int priority)
            {
                this.title = title;
                this.description = description;
                this.isdone = isdone;
                this.enddate = enddate;
                this.priority = priority;
            }

            public void SortSubTasks(int type)
            {
                switch (Enum.GetName(typeof(sorttype), type))
                {
                    case "TitleAsc":
                        {
                            SubTasks.Sort((a, b) => a.title.CompareTo(b.title));
                            break;
                        }
                    case "TitleDesc":
                        {
                            SubTasks.Sort((a, b) => b.title.CompareTo(a.title));
                            break;
                        }
                    case "CreationDateAsc":
                        {
                            SubTasks.Sort((a, b) => a.creationdate.CompareTo(b.creationdate));
                            break;
                        }
                    case "CreationDateDesc":
                        {
                            SubTasks.Sort((a, b) => b.creationdate.CompareTo(a.creationdate));
                            break;
                        }
                    case "EndDateAsc":
                        {
                            SubTasks.Sort((a, b) => a.enddate == b.enddate ? a.title.CompareTo(b.title) : a.enddate.CompareTo(b.enddate));
                            break;
                        }
                    case "EndDateDesc":
                        {
                            SubTasks.Sort((a, b) => a.enddate == b.enddate ? b.title.CompareTo(a.title) : b.enddate.CompareTo(a.enddate));
                            break;
                        }
                    case "IsDoneAsc":
                        {
                            SubTasks.Sort((a, b) => a.isdone == b.isdone ? a.title.CompareTo(b.title) : a.isdone.CompareTo(b.isdone));
                            break;
                        }
                    case "IsDoneDesc":
                        {
                            SubTasks.Sort((a, b) => a.isdone == b.isdone ? b.title.CompareTo(a.title) : b.isdone.CompareTo(a.isdone));
                            break;
                        }
                }
            }
        }

        public class SubTask: ITask
        {
            public int id { get; set; }
            public string title { get; set; }
            public string description { get; set; }
            public bool isdone { get; set; }
            public DateTime enddate { get; set; }
            public DateTime creationdate { get; set; }

            public SubTask()
            {

            }

            public SubTask(string title, string description, DateTime enddate)
            {
                this.title = title;
                this.description = description;
                isdone = false;
                this.enddate = enddate;
                this.creationdate = DateTime.Now;
            }

            public void Update(string title, string description, bool isdone, DateTime enddate)
            {
                this.title = title;
                this.description = description;
                this.isdone = isdone;
                this.enddate = enddate;
            }
        }

        public static string FormatTitle(ITask task, int length)
        {
            if(length > 3)
            {
                return task.title.Substring(0, task.title.Length < length ? task.title.Length : length-3) + (task.title.Length < length ? "" : "...");
            }
            else
            {
                return task.title;
            }
        }

        private enum taskproperties
        {
            title,
            description,
            isdone,
            enddate,
            creationdate,
            priority
        }

        public static void SortTasks(int type)
        {
            switch (Enum.GetName(typeof(sorttype), type))
            {
                case "TitleAsc":
                    {
                        taskslist.Sort((a, b) => a.title.CompareTo(b.title));
                        break;
                    }
                case "TitleDesc":
                    {
                        taskslist.Sort((a, b) => b.title.CompareTo(a.title));
                        break;
                    }
                case "CreationDateAsc":
                    {
                        taskslist.Sort((a, b) => a.creationdate.CompareTo(b.creationdate));
                        break;
                    }
                case "CreationDateDesc":
                    {
                        taskslist.Sort((a, b) => b.creationdate.CompareTo(a.creationdate));
                        break;
                    }
                case "EndDateAsc":
                    {
                        taskslist.Sort((a, b) => a.enddate == b.enddate ? a.title.CompareTo(b.title) : a.enddate.CompareTo(b.enddate));
                        break;
                    }
                case "EndDateDesc":
                    {
                        taskslist.Sort((a, b) => a.enddate == b.enddate ? b.title.CompareTo(a.title) : b.enddate.CompareTo(a.enddate));
                        break;
                    }
                case "IsDoneAsc":
                    {
                        taskslist.Sort((a, b) => a.isdone == b.isdone ? a.title.CompareTo(b.title) : a.isdone.CompareTo(b.isdone));
                        break;
                    }
                case "IsDoneDesc":
                    {
                        taskslist.Sort((a, b) => a.isdone == b.isdone ? b.title.CompareTo(a.title) : b.isdone.CompareTo(a.isdone));
                        break;
                    }
                case "PriorityAsc":
                    {
                        taskslist.Sort((a, b) => a.priority == b.priority ? a.title.CompareTo(b.title) : a.priority.CompareTo(b.priority));
                        break;
                    }
                case "PriorityDesc":
                    {
                        taskslist.Sort((a, b) => a.priority == b.priority ? b.title.CompareTo(a.title) : b.priority.CompareTo(a.priority));
                        break;
                    }
            }
        }

        /*public static void SortByTitle(bool ascending)
        {
            if(ascending)
                taskslist.Sort((a, b) => a.title.CompareTo(b.title));
            else
                taskslist.Sort((a, b) => b.title.CompareTo(a.title));
        }

        public static void SortByCreationDate(bool ascending)
        {
            if (ascending)
                taskslist.Sort((a, b) => a.creationdate.CompareTo(b.creationdate));
            else
                taskslist.Sort((a, b) => b.creationdate.CompareTo(a.creationdate));
        }

        public static void SortByEndDate(bool ascending)
        {
            if (ascending)
                taskslist.Sort((a, b) => a.enddate == b.enddate ? a.title.CompareTo(b.title) : a.enddate.CompareTo(b.enddate));
            else
                taskslist.Sort((a, b) => a.enddate == b.enddate ? b.title.CompareTo(a.title) : b.enddate.CompareTo(a.enddate));
        }
        public static void SortByDone(bool ascending)
        {
            if (ascending)
                taskslist.Sort((a, b) => a.isdone == b.isdone ? a.title.CompareTo(b.title) : a.isdone.CompareTo(b.isdone));
            else
                taskslist.Sort((a, b) => a.isdone == b.isdone ? b.title.CompareTo(a.title) : b.isdone.CompareTo(a.isdone));
        }
        public static void SortByPriority(bool ascending)
        {
            if (ascending)
                taskslist.Sort((a, b) => a.priority == b.priority ? a.title.CompareTo(b.title) : a.priority.CompareTo(b.priority));
            else
                taskslist.Sort((a, b) => a.priority == b.priority ? b.title.CompareTo(a.title) : b.priority.CompareTo(a.priority));
        }*/
    }
}
