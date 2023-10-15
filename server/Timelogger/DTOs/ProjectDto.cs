using System.Collections.Generic;
using System;

namespace Timelogger.Core.DTOs
{
    public class ProjectDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime? Deadline { get; set; }
        public ICollection<TimeLogDto> TimeLogs { get; set; }
        public bool IsCompleted { get; set; }
    }
}