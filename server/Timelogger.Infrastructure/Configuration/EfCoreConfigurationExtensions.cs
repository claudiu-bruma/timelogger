using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Timelogger.Core.Entities;
using Timelogger.Infrastructure.DbContext;

namespace Timelogger.Infrastructure.Configuration
{
    public static class EfCoreConfigurationExtensions
    {
        public static void InitializeInMemoryData(this IApplicationBuilder app)
        {
            var serviceScopeFactory = app.ApplicationServices.GetService<IServiceScopeFactory>();

            using (var scope = serviceScopeFactory.CreateScope())
            {
                SeedDatabase(scope);
            }
        }


        private static void SeedDatabase(IServiceScope scope)
        {
            var context = scope.ServiceProvider.GetService<TimeLoggerDbContext>();
            var testProject1 = new Project
            {
                Id = 1,
                Name = "e-conomic Interview",
                Deadline = new DateTime(2023, 10, 18, 23, 59, 59),
                TimeLogs = new List<TimeLog>()
                {
                    new TimeLog()
                    {
                        Comment = "Dummy comment1",

                        LogDate = new System.DateTime(2020, 1, 1),
                        LogTimeInMinutes = 60
                    },
                    new TimeLog()
                    {
                        Comment = "Dummy comment2",
                        LogDate = new System.DateTime(2023, 2, 1),
                        LogTimeInMinutes = 120
                    }
                }
            };
            var testProject2 = new Project
            {
                Id = 2,
                Name = "some other project",
                Deadline = new DateTime(2023, 12, 18, 23, 59, 59),
                TimeLogs = new List<TimeLog>()
                {
                    new TimeLog()
                    {
                        Comment = "Dummy comment3",

                        LogDate = new System.DateTime(2021, 2, 3),
                        LogTimeInMinutes = 80
                    },
                    new TimeLog()
                    {
                        Comment = "Dummy comment4",
                        LogDate = new System.DateTime(2023, 5, 6),
                        LogTimeInMinutes = 30
                    }
                }
            };
            var testProject3 = new Project
            {
                Id = 3,
                Name = "some past closed project",
                Deadline = new DateTime(2023, 08, 18, 23, 59, 59),
                TimeLogs = new List<TimeLog>()
                {
                    new TimeLog()
                    {
                        Comment = "Dummy comment5",

                        LogDate = new System.DateTime(2022, 7, 8),
                        LogTimeInMinutes = 80
                    },
                    new TimeLog()
                    {
                        Comment = "Dummy comment6",
                        LogDate = new System.DateTime(2023, 8, 8),
                        LogTimeInMinutes = 30
                    }
                }
            };
            context.Projects.Add(testProject1);

            context.Projects.Add(testProject2);

            context.Projects.Add(testProject3);

            context.SaveChanges();
        }
    }
}