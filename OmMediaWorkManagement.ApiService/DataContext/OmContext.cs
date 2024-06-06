﻿using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using OmMediaWorkManagement.ApiService.Models;

namespace OmMediaWorkManagement.ApiService.DataContext
{
    public class OmContext:DbContext
    {
        public OmContext()
        {
        }

        public OmContext(DbContextOptions<OmContext> options)
            : base(options)
        {
        }
        public DbSet<OmClient> OmClient { get; set; }
        public DbSet<OmClientWork> OmClientWork { get; set; }
        public DbSet<OmEmployee> OmEmployee { get; set; }
        public DbSet<OmEmployeeWork> OmEmployeeWork { get; set; }
    }
}
