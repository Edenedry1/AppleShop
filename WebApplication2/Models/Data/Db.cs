﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

//connect between pages and database
namespace WebApplication2.Models.Data
{
    public class Db : DbContext
    {
        public DbSet <PagesDTO> Pages { get; set; }
        public DbSet<SidebarDTO> Sidebars { get; set; }

        public DbSet<CategoryDTO> Categories { get; set; }


    }
}