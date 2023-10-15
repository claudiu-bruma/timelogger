using System;

namespace Timelogger.Core.DTOs
{
    public class AddProjectDto
    {
        public string Name { get; set; }
        public DateTime? Deadline { get; set; } 
        public bool IsCompleted { get; set; }
    }
}