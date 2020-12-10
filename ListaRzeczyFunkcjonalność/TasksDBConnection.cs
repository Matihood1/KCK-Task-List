using System;
using System.Linq;

namespace ListaZadanFunkcjonalnosc
{
    public class TasksDBConnection
    {
        private TasksContext DB;
        public TasksDBConnection()
        {
            DB = new TasksContext();
            Tasks.taskslist = DB.Tasks.ToList();
            Tasks.SortTasks(3);
        }

        public void AddTask(Tasks.Task task)
        {
            Tasks.taskslist.Add(task);
            DB.Tasks.Add(task);
            DB.SaveChanges();
        }

        public void EditTask(Tasks.Task task, string title, string description, bool isdone, DateTime enddate, int priority)
        {
            task.Update(title, description, isdone, enddate, priority);
            DB.Tasks.Find(task.id).Update(title, description, isdone, enddate, priority);
            DB.SaveChanges();
        }

        public void DeleteTask(int index)
        {
            int id = Tasks.taskslist[index].id;
            Tasks.taskslist.RemoveAt(index);
            DB.Tasks.Remove(DB.Tasks.Find(id));
            DB.SaveChanges();
        }

        public void AddSubTask(Tasks.Task task, Tasks.SubTask subtask)
        {
            //subtask.id = BitConverter.ToInt32(Guid.NewGuid().ToByteArray(), 0);
            task.SubTasks.Add(subtask);
            DB.SubTasks.Add(subtask);
            //DB.Tasks.Find(task.id).SubTasks.Add(subtask);
            DB.SaveChanges();
        }

        public void EditSubTask(Tasks.Task task, Tasks.SubTask subtask, string title, string description, bool isdone, DateTime enddate)
        {
            subtask.Update(title, description, isdone, enddate);
            DB.SubTasks.First(x => x.id == subtask.id).Update(title, description, isdone, enddate);
            //DB.Tasks.Find(task.id).SubTasks.First(x => x.id == subtask.id).Update(title, description, isdone, enddate);
            DB.SaveChanges();
        }

        public void DeleteSubTask(Tasks.Task task, int index)
        {
            int id = task.SubTasks[index].id;
            task.SubTasks.RemoveAt(index);
            DB.SubTasks.Remove(DB.SubTasks.First(x => x.id == id));
            //DB.Tasks.Find(task.id).SubTasks.Remove(DB.Tasks.Find(task.id).SubTasks.First(x => x.id == id));
            DB.SaveChanges();
        }
    }
}
