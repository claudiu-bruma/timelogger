using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Timelogger.Core.Entities;

namespace Timelogger.Infrastructure.DbContext
{
	public class TimeLoggerDbContext :  Microsoft.EntityFrameworkCore.DbContext, ITimeLoggerDbContext
    {
		public TimeLoggerDbContext(DbContextOptions<TimeLoggerDbContext> options)
			: base(options)
		{
		}

		public DbSet<Project> Projects { get; set; }
		public DbSet<TimeLog> TimeLogs { get; set; }
        public new DbSet<TEntity> Set<TEntity>() where TEntity : class
        {
            return base.Set<TEntity>();
        }
}
}
