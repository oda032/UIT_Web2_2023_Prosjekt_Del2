using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blog.DataAccess.Data;
using Microsoft.EntityFrameworkCore;

namespace Blog.Test.Helpers
{
    public class InMemoryHelper
    {
        public static ApplicationDbContext GetTestDbContext(string dbName)
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: dbName)
                .Options;

            return new ApplicationDbContext(options);
        }
    }
}
