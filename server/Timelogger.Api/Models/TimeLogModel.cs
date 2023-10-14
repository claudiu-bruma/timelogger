using System;

namespace Timelogger.Api.Models
{
    public class TimeLogModel
    {
        public string Comment { get; set; }
        public int ProjectId { get; set; }
        public int TimeSpentInMinutes { get; set; }
        public DateTime TimeLogDate { get; set; }
    }
}