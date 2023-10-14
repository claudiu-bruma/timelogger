using System;

namespace Timelogger.Core.DTOs
{
    public class AddTimeLogDto
    {
        public string Comment { get; set; }
        public int LogTimeInMinutes { get; set; }
        public DateTime LogDate { get; set; }
        public int? ProjectId { get; set; }
    }
}