using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ListaZadanFunkcjonalnosc
{
    public class TasksContext : DbContext
    {
        public TasksContext(): base("MyDBContext") { }
        //{
            // Turn off the Migrations, (NOT a code first Db)
            //Database.SetInitializer<TasksContext>(null);
        //}

        public DbSet<Tasks.Task> Tasks { get; set; }
        public DbSet<Tasks.SubTask> SubTasks { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Tasks.Task>().HasMany(i => i.SubTasks).WithMany();
        }
        /*protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer<TasksContext>(null);
            base.OnModelCreating(modelBuilder);
        }*/
    }
}
