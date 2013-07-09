using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CustomWebServer.Data;
using Newtonsoft.Json.Linq;

namespace CustomWebServer.Services
{
    public class ToDoService
    {
        public IEnumerable<ToDo> Get()
        {
            using (var db = new ToDoContext())
            {
                return db.ToDos.ToList();
            }
        }

        public void Put(IEnumerable<ToDo> todos)
        {
            using(var db = new ToDoContext())
            {
                foreach (var todo in todos ?? Enumerable.Empty<ToDo>())
                {
                    var existing = db.ToDos.FirstOrDefault(t => t.ToDoId == todo.ToDoId);

                    if (existing == null)
                    {
                        db.ToDos.Add(todo);
                    }
                    else
                    {
                        existing.Title = todo.Title;
                        existing.Completed = todo.Completed;
                    }
                }

                db.SaveChanges();
            }
        }

        public void Delete(IEnumerable<ToDo> todos)
        {
            using(var db = new ToDoContext())
            {
                foreach (var todo in todos)
                {
                    var existing = db.ToDos.FirstOrDefault(t => t.ToDoId == todo.ToDoId);

                    if (existing != null)
                    {
                        db.ToDos.Remove(existing);
                    }
                }

                db.SaveChanges();
            }
        }
    }
}
