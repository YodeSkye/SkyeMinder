
using SkyeMinder.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkyeMinder.Services
{
    public class DatabaseService
    {
        private readonly SQLiteAsyncConnection _database;

        public DatabaseService(string dbPath)
        {
            _database = new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<Models.BloodSugarEntry>().Wait();
            _database.CreateTableAsync<ReminderTime>().Wait();
        }

        //Blood Sugar Entries
        public Task<List<BloodSugarEntry>> GetEntriesAsync(int maxCount = 20)
        {
            return _database.Table<BloodSugarEntry>()
                            .OrderByDescending(e => e.Timestamp)
                            .Take(maxCount)
                            .ToListAsync();
        }
        public Task<List<BloodSugarEntry>> GetEntriesInRangeAsync(DateTime startDate, DateTime endDate)
        {
            if (startDate > endDate)
                (startDate, endDate) = (endDate, startDate);

            return _database.Table<BloodSugarEntry>()
                            .Where(e => e.Timestamp >= startDate && e.Timestamp <= endDate)
                            .OrderBy(e => e.Timestamp)
                            .ToListAsync();
        }
        public Task<List<BloodSugarEntry>> GetEntriesForLastWeekAsync()
        {
            var start = DateTime.Now.AddDays(-7);
            var end = DateTime.Now;

            return _database.Table<BloodSugarEntry>()
                            .Where(e => e.Timestamp >= start && e.Timestamp <= end)
                            .OrderBy(e => e.Timestamp)
                            .ToListAsync();
        }
        public Task<List<BloodSugarEntry>> GetEntriesForLastMonthAsync()
        {
            var start = DateTime.Now.AddMonths(-1);
            var end = DateTime.Now;

            return _database.Table<BloodSugarEntry>()
                            .Where(e => e.Timestamp >= start && e.Timestamp <= end)
                            .OrderBy(e => e.Timestamp)
                            .ToListAsync();
        }
        public Task<List<BloodSugarEntry>> GetEntriesForLast3MonthsAsync()
        {
            var start = DateTime.Now.AddMonths(-3);
            var end = DateTime.Now;

            return _database.Table<BloodSugarEntry>()
                            .Where(e => e.Timestamp >= start && e.Timestamp <= end)
                            .OrderBy(e => e.Timestamp)
                            .ToListAsync();
        }
        public Task<List<BloodSugarEntry>> GetEntriesForLast6MonthsAsync()
        {
            var start = DateTime.Now.AddMonths(-6);
            var end = DateTime.Now;

            return _database.Table<BloodSugarEntry>()
                            .Where(e => e.Timestamp >= start && e.Timestamp <= end)
                            .OrderBy(e => e.Timestamp)
                            .ToListAsync();
        }
        public Task<List<BloodSugarEntry>> GetEntriesForLastYearAsync()
        {
            var start = DateTime.Now.AddYears(-1);
            var end = DateTime.Now;

            return _database.Table<BloodSugarEntry>()
                            .Where(e => e.Timestamp >= start && e.Timestamp <= end)
                            .OrderBy(e => e.Timestamp)
                            .ToListAsync();
        }
        public Task<List<BloodSugarEntry>> GetAllEntriesAsync()
        {
            return _database.Table<BloodSugarEntry>()
                            .OrderByDescending(e => e.Timestamp)
                            .ToListAsync();
        }
        public Task<int> SaveEntryAsync(Models.BloodSugarEntry entry)
        {
            return _database.InsertAsync(entry);
        }
        public Task<int> UpdateEntryAsync(BloodSugarEntry entry)
        {
            return _database.UpdateAsync(entry);
        }
        public Task<int> DeleteEntryAsync(BloodSugarEntry entry)
        {
            return _database.DeleteAsync(entry);
        }
        public async Task<int> ImportCsvAsync(string filePath, bool purgeFirst)
        {
            if (purgeFirst)
                await _database.DeleteAllAsync<BloodSugarEntry>();

            int importedCount = 0;

            var text = File.ReadAllText(filePath);

            // Split into lines, skip header
            var lines = text.Split('\n')
                            .Skip(1)
                            .Where(l => !string.IsNullOrWhiteSpace(l));

            foreach (var rawLine in lines)
            {
                var line = rawLine.Trim();

                // Split CSV safely
                var parts = line.Split(',');

                if (parts.Length < 2)
                    continue; // malformed row

                // Parse fields
                var timestamp = DateTime.Parse(parts[0]);
                var value = int.Parse(parts[1]);
                var notes = parts.Length >= 3 ? parts[2] : string.Empty;

                var entry = new BloodSugarEntry
                {
                    Timestamp = timestamp,
                    Value = value,
                    Notes = notes
                };

                await SaveEntryAsync(entry);
                importedCount++;   // ⭐ increment count
            }

            return importedCount;  // ⭐ return the number of imported entries
        }

        //Reminder Times
        public Task<List<ReminderTime>> GetReminderTimesAsync() => 
            _database.Table<ReminderTime>()
                .OrderBy(e => e.TimeOfDay)
                .ToListAsync();
        public Task<int> AddReminderTimeAsync(ReminderTime reminder) =>
            _database.InsertAsync(reminder);
        public Task<int> DeleteReminderTimeAsync(int id) =>
            _database.DeleteAsync<ReminderTime>(id);

    }
}