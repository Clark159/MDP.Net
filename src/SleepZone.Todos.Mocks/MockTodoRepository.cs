﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CLK.Mocks;

namespace SleepZone.Todos.Mocks
{
    public class MockTodoRepository : MockRepository<Todo, string>, TodoRepository
    {
        // Constructors
        public MockTodoRepository() : base(todo => Tuple.Create(todo.TodoId))
        {
            // Default

        }


        // Methods
        public TodoCounts CountAll()
        {
            // TodoCounts
            var todoCounts = new TodoCounts()
            {
                TotalCount = this.EntityList.Count,
                CompleteCount = this.EntityList.Where(todo => todo.IsComplete == true).Count()
            };

            // Return
            return todoCounts;
        }
    }
}
