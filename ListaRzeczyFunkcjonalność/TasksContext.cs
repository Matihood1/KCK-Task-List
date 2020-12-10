using System.Data.Entity;

namespace ListaZadanFunkcjonalnosc
{
    public class TasksContext : DbContext
    {
        public TasksContext() : base("MyDBContext") { }

        public DbSet<Tasks.Task> Tasks { get; set; }
        public DbSet<Tasks.SubTask> SubTasks { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Tasks.Task>().HasMany(i => i.SubTasks).WithMany();
        }
    }
}
