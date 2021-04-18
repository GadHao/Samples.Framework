﻿using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Samples.LittleAspNetCoreBook.Data;
using Samples.LittleAspNetCoreBook.Models;

namespace Samples.LittleAspNetCoreBook.Services
{
    public class TodoItemService : ITodoItemService
    {
        private readonly ApplicationDbContext _context;

        public TodoItemService(ApplicationDbContext context)
        {
            this._context = context;
        }

        public async Task<TodoItem[]> GetIncompleteItemsAsync()
        {
            var items = await _context.Items
                                      .Where(x => x.IsDone == false)
                                      .ToArrayAsync();

            return items;
        }


        public async Task<bool> AddItemAsync(TodoItem newItem)
        {
            newItem.Id = new Guid();
            newItem.IsDone = false;
            newItem.DueAt = DateTimeOffset.Now.AddDays(3);

            _context.Items.Add(newItem);

            var saveResult = await _context.SaveChangesAsync();
            return saveResult == 1;
        }

        public async Task<bool> MarkDoneAsync(Guid id)
        {
            var item = await _context.Items
                                     .Where(x => x.Id == id)
                                     .SingleOrDefaultAsync();

            if (item == null)
            {
                return false;
            }

            item.IsDone = true;
            
            return await _context.SaveChangesAsync() == 1;
        }
    }
}
