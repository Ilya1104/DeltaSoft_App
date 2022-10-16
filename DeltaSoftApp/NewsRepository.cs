using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace DeltaSoftApp
{
    public class NewsRepository
    {
        SQLiteAsyncConnection database;
        public NewsRepository(string databasePath)
        {
            database = new SQLiteAsyncConnection(databasePath);
        }
       public async Task CreateTable()
        {
            await database.CreateTableAsync<News>();
        }
        public async Task<List<News>> GetItemsAsync()
        {
            return await database.Table<News>().ToListAsync();
        }
        public async Task<News> GetItemAsync(int id)
        {
            return await database.GetAsync<News>(id);
        }
    }
}