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
        public static List<Task> taskslist = new List<Task>();
        public static int lastsorting = 0;

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

        public interface ITask
        {
            string title { get;}
            string description { get; }
            bool isdone { get; }
            DateTime enddate { get; }
            DateTime creationdate { get; }
        }

        /*public enum priorities
        {
            [Description("Very High")] VeryHigh,
            [Description("High")] High,
            [Description("Medium")] Medium,
            [Description("Low")] Low,
            [Description("Very Low")] VeryLow
        }*/

        /*public static List<string> GetTaskTitlesList(List<Task> lista)
        {
            List<string> TaskTitles = new List<string>();
            foreach (var task in lista)
            {
                TaskTitles.Add(task.Title);
            }
            return TaskTitles;
        }

        public static List<string> GetSubTaskTitlesList(List<SubTask> lista)
        {
            List<string> TaskTitles = new List<string>();
            foreach (var task in lista)
            {
                TaskTitles.Add(task.Title);
            }
            return TaskTitles;
        }*/

        public class Task: ITask
        {
            public List<SubTask> SubTasks;

            public string title { get; private set; }
            public string description { get; private set; }
            public bool isdone { get; private set; }
            public DateTime enddate { get; private set; }
            public DateTime creationdate { get; private set; }
            public int priority { get; private set; }

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

            public void SortByTitle(bool ascending)
            {
                if (ascending)
                    SubTasks.Sort((a, b) => a.title.CompareTo(b.title));
                else
                    SubTasks.Sort((a, b) => b.title.CompareTo(a.title));
            }

            public void SortByCreationDate(bool ascending)
            {
                if (ascending)
                    SubTasks.Sort((a, b) => a.creationdate.CompareTo(b.creationdate));
                else
                    SubTasks.Sort((a, b) => b.creationdate.CompareTo(a.creationdate));
            }

            public void SortByEndDate(bool ascending)
            {
                if (ascending)
                    SubTasks.Sort((a, b) => a.enddate == b.enddate ? a.title.CompareTo(b.title) : a.enddate.CompareTo(b.enddate));
                else
                    SubTasks.Sort((a, b) => a.enddate == b.enddate ? b.title.CompareTo(a.title) : b.enddate.CompareTo(a.enddate));
            }

            public void SortByDone(bool ascending)
            {
                if (ascending)
                    SubTasks.Sort((a, b) => a.isdone == b.isdone ? a.title.CompareTo(b.title) : a.isdone.CompareTo(b.isdone));
                else
                    SubTasks.Sort((a, b) => a.isdone == b.isdone ? b.title.CompareTo(a.title) : b.isdone.CompareTo(a.isdone));
            }
        }

        public class SubTask: ITask
        {
            public string title { get; private set; }
            public string description { get; private set; }
            public bool isdone { get; private set; }
            public DateTime enddate { get; private set; }
            public DateTime creationdate { get; private set; }
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

        public static void SortByTitle(bool ascending)
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

        public static void SortByPriority(bool ascending)
        {
            if (ascending)
                taskslist.Sort((a, b) => a.priority == b.priority ? a.title.CompareTo(b.title) : a.priority.CompareTo(b.priority));
            else
                taskslist.Sort((a, b) => a.priority == b.priority ? b.title.CompareTo(a.title) : b.priority.CompareTo(a.priority));
        }

        public static void SortByDone(bool ascending)
        {
            if (ascending)
                taskslist.Sort((a, b) => a.isdone == b.isdone ? a.title.CompareTo(b.title) : a.isdone.CompareTo(b.isdone));
            else
                taskslist.Sort((a, b) => a.isdone == b.isdone ? b.title.CompareTo(a.title) : b.isdone.CompareTo(a.isdone));
        }
    }
}
