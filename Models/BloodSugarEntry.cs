using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace SkyeMinder.Models
{
    public class BloodSugarEntry
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public DateTime Timestamp { get; set; } = DateTime.Now;
        public int Value { get; set; }
        public string? Notes { get; set; }
    }
}
