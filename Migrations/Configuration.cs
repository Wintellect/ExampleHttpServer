using CustomWebServer.Data;

namespace CustomWebServer.Migrations
{
    using System.Data.Entity.Migrations;

    internal sealed class Configuration : DbMigrationsConfiguration<ToDoContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(ToDoContext context)
        {
                context.ToDos.AddOrUpdate(
                  t => t.Title,
                  new ToDo{Title = "Finish Codestock Presentation", Completed = false},
                  new ToDo { Title = "Implement RestHandler", Completed = true },
                  new ToDo{Title = "Be Awesome!", Completed = false}
                );
            
        }
    }
}
