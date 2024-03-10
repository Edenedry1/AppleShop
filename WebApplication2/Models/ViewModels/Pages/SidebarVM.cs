using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication2.Models.Data;

namespace WebApplication2.Models.ViewModels.Pages
{
    public class SidebarVM
    {
        public SidebarVM()//BUILDER
        {

        }

        public SidebarVM(SidebarDTO row)
        {
            Id = row.Id;
            Body = row.Body;
        }

        public int Id { get; set; }
        [AllowHtml]
        public string Body { get; set; }
    }
}