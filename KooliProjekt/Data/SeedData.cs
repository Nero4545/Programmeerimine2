using KooliProjekt.Data;
using KooliProjekt.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace KooliProjekt.Data
{
    public static class SeedData
    {
        public static void Generate(ApplicationDbContext context)
        {
         
            if (context.TodoLists.Any())
            {
                return; 
            }

      
            var todoLists = new TodoList[]
            {

                new TodoList { Name = "f0)@!#!@#ing SLAYEEEEER" },

            };

            context.TodoLists.AddRange(todoLists);
            context.SaveChanges(); 
        }
    }
}
