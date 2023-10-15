using System;

namespace Timelogger.Core.Entities
{
    public class TimeLog
    {
        public int Id { get; set; }
        public string Comment { get; set; }
        public int LogTimeInMinutes { get; set; }
        public DateTime LogDate { get; set; }
        public Project Project { get; set; }

        public int UserId { get; set; }
    }
}
