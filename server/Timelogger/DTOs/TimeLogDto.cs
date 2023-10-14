using System;
using Timelogger.Core.Entities;

namespace Timelogger.Core.DTOs
{
    public class TimeLogDto
    {
        public int Id { get; set; }
        public string Comment { get; set; }
        public int LogTimeInMinutes { get; set; }
        public DateTime LogDate { get; set; }
        public int? ProjectId { get; set; }
    }
}