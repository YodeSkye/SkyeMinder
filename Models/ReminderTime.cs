using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkyeMinder.Models
{
    public class ReminderTime
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public TimeSpan TimeOfDay { get; set; }
    }
}
