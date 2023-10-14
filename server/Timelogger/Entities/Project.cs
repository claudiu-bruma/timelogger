using System;
using System.Collections.Generic;

namespace Timelogger.Core.Entities
{
	public class Project
	{
		public int Id { get; set; }
		public string Name { get; set; }
        public DateTime? Deadline { get; set; }

		public ICollection<TimeLog> TimeLogs { get; set; }
		public bool IsCompleted { get; set; }

	}
}
