﻿using CLERP.Database.Entities.Link;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CLERP.Database.Configurations.Link
{
    public class RoleEmployeeConfiguration : IEntityTypeConfiguration<RoleEmployee>
    {
        public void Configure(EntityTypeBuilder<RoleEmployee> builder)
        {
            builder.ToTable("Roley_Employees");

            // Role m-m Employee
            builder.HasKey(x => new { x.RoleGuid, x.EmployeeGuid }).ForSqlServerIsClustered();
            builder.HasOne(x => x.Role)
                .WithMany(y => y.Employees)
                .HasForeignKey(x => x.RoleGuid);
            builder.HasOne(x => x.Employee)
                .WithMany(y => y.Roles)
                .HasForeignKey(x => x.EmployeeGuid);
        }
    }
}
