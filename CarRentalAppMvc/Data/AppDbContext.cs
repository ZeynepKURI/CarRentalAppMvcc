﻿using System;
using CarRentalAppMvc.Models;
using Microsoft.EntityFrameworkCore;

namespace CarRentalAppMvc.Data
{
    public class AppDbContext : DbContext
    {


        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<VehicleWorkingTime> vehicleWorkingTimes { get; set; }
         public DbSet<Users> users { get; set; }
    }
}