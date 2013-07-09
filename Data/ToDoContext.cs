using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomWebServer.Data
{
    public class ToDoContext : DbContext
    {
        public DbSet<ToDo> ToDos { get; set; }
    }
}
