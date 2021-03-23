using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using AspNetCoreWithOutEF.Models;

namespace AspNetCoreWithOutEF.Data
{
    public class AspNetCoreWithOutEFContext : DbContext
    {
        public AspNetCoreWithOutEFContext (DbContextOptions<AspNetCoreWithOutEFContext> options)
            : base(options)
        {
        }

        public DbSet<AspNetCoreWithOutEF.Models.BookModelView> BookModelView { get; set; }
    }
}
