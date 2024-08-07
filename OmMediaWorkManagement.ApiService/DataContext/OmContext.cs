﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OmMediaWorkManagement.ApiService.Models;

namespace OmMediaWorkManagement.ApiService.DataContext
{
    public class OmContext : IdentityDbContext<UserRegistration>
	{
		public OmContext(DbContextOptions<OmContext> options)
			: base(options)
		{
		}

		public DbSet<OmClient> OmClient { get; set; }
		public DbSet<OmClientWork> OmClientWork { get; set; }
		public DbSet<OmEmployee> OmEmployee { get; set; }
		public DbSet<OmEmployeeDocuments> OmEmployeeDocuments { get; set; }
		public DbSet<OmEmployeeSalaryManagement> OmEmployeeSalaryManagement { get; set; }
		public DbSet<OmEmployeeShift> OmEmployeeShift { get; set; }
		public DbSet<OmMachines> OmMachines { get; set; }
		public DbSet<JobToDo> JobToDo { get; set; }
		public DbSet<JobImages> JobImages { get; set; }
		public DbSet<JobTypeStatus> JobTypeStatus { get; set; }
	}
}
