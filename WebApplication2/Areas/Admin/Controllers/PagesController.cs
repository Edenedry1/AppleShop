using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication2.Models.Data;
using WebApplication2.Models.ViewModels.Pages;

namespace WebApplication2.Areas.Admin.Controllers
{
    public class PagesController : Controller
    {
        // GET: Admin/Pages
        public ActionResult Index()
        {
            //show the database rows from PageVM
            List<PageVM> pageList;

            //update the rows
            using(Db db = new Db())
            {
                pageList = db.Pages.ToArray().OrderBy(x => x.Sorting).Select(x => new PageVM(x)).ToList();
            }

            //return the view
            return View(pageList);
        }

        // GET: Admin/Pages/AddPage
        [HttpGet]
        public ActionResult AddPage()
        {
            return View();
        }

        // POST: Admin/Pages/AddPage
        [HttpPost]
        public ActionResult AddPage(PageVM model)
        {
            //check model validation dont believe to customer
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            
            using(Db db=new Db())
            {

                //this is slug תיאור

                string slug;


                //initialize class PageDTO
                PagesDTO dto = new PagesDTO();

                //mane to the model
                dto.Title = model.Title.ToUpper();

                //check if the slug if no write him
                if (string.IsNullOrWhiteSpace(model.Slug))
                {
                    slug = model.Title.Replace(" ", "-").ToLower();
                }
                else
                {
                    slug=model.Slug.Replace(" ", "-").ToLower();
                }

                //check if the name and slug unique

                if (db.Pages.Any(x=>x.Title == model.Title))
                {
                    ModelState.AddModelError("", "That title already exists.");
                    return View(model);
                }
                else if (db.Pages.Any(x=>x.Slug == model.Slug))
                {
                    ModelState.AddModelError("", "That slug already exists.");
                    return View(model);
                }

                //another things
                dto.Slug = slug;
                dto.Body = model.Body;
                dto.HasSidbar = model.HasSidbar;
                dto.Sorting = 100;//in the end of the table


                //safe in database
                db.Pages.Add(dto);
                db.SaveChanges();
            }

            //message for customer if success or no
            TempData["SM"] = "You have added a new page!";

            //redirection to the index method
            return RedirectToAction("Index");


        }

        // GET: Admin/Pages/EditPage/id
        [HttpGet]
        public ActionResult EditPage(int id)
        {
            //what the model?
            PageVM model;

            using(Db db = new Db())
            {
                //get the page details
                PagesDTO dto = db.Pages.Find(id);//find from id the page we need

                //check if the page exist
                if (dto == null)
                {
                    return Content("The page does not exist.");
                }

                //initialize model with data
                model = new PageVM(dto);
            }

            //return the view from data
            return View(model);
        }

        // POST: Admin/Pages/EditPage/id
        [HttpPost]
        public ActionResult EditPage(PageVM model)
        {
            //check model for validation
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            
            using(Db db = new Db())
            {
                //get id page
                int id = model.Id; //from HiddenForm

                //for slug
                string slug = "home";

                //get the information from id
                PagesDTO dto = db.Pages.Find(id);

                //set the title from customer to the item
                dto.Title = model.Title;

                //check the slug and title if we need
                if (model.Slug !="home")
                {
                    if (string.IsNullOrWhiteSpace(model.Slug))
                    {
                        slug = model.Title.Replace(" ", "-").ToLower();
                    }
                    else
                    {
                        slug = model.Slug.Replace(" ", "-").ToLower();
                    }
                }

                //check the slug and title for unique
                if (db.Pages.Where(x=>x.Id != id).Any(x=>x.Title == model.Title))
                {
                    ModelState.AddModelError("", "That title already exists.");
                    return View(model);
                }
                else if (db.Pages.Where(x => x.Id != id).Any(x => x.Slug == slug))
                {
                    ModelState.AddModelError("", "That slug already exists.");
                    return View(model);
                }

                //write others to the class DTO
                dto.Slug = slug;
                dto.Body = model.Body;
                dto.HasSidbar = model.HasSidbar;

                //save the changes
                db.SaveChanges();
            }
            //text to the client
            TempData["SM"] = "You have edited the page";
            //redirection
            return RedirectToAction("EditPage");
        }

        // GET: Admin/Pages/PageDetails/id
        public ActionResult PageDetails(int id)
        {
            //show the model
            PageVM model;

            using (Db db = new Db())
            {
                
                PagesDTO dto = db.Pages.Find(id);
                //if page exists
                
                if (dto == null)
                {
                    return Content("The page does not exists");
                }
                //from database info to the model
                model = new PageVM(dto);
            }

            //return model from db
            return View(model);
        }


        // GET: Admin/Pages/DeletePages/id
        public ActionResult DeletePage(int id)
        {
            using (Db db = new Db())
            {
                //get the page
                PagesDTO dto = db.Pages.Find(id);

                //delete the page
                db.Pages.Remove(dto);
                //save the changes
                db.SaveChanges();

            }
            //message about deleting
            TempData["SM"] = "You have deleted a page!";
            //redirection
            return RedirectToAction("Index");
        }

        // GET: Admin/Pages/ReorderPages
        [HttpPost]
        public void ReorderPages(int[] id)
        {
            using (Db db = new Db())
            {
                int count = 1;
                PagesDTO dto;

                foreach (var pageId in id)
                {
                    dto = db.Pages.Find(pageId);
                    dto.Sorting = count;
                    db.SaveChanges();

                    count++;
                }
            }
        }




        // GET: Admin/Pages/EditSidebar
        [HttpGet]
        public ActionResult EditSidebar()
        {
            //model
            SidebarVM model;
            
            using (Db db= new Db())
            {
                //get the info from db
                SidebarDTO dto = db.Sidebars.Find(1);//fix 

                //set the info to the db
                model = new SidebarVM(dto);
            }


            //return the view
            return View(model);
        }

        // POST: Admin/Pages/EditSidebar
        [HttpPost]
        public ActionResult EditSidebar(SidebarVM model)
        {
            using (Db db = new Db())
            {
                //get the info from dto
                SidebarDTO dto = db.Sidebars.Find(1);//fix
                //get the info to the body
                dto.Body = model.Body;
                //save changes
                db.SaveChanges();
            }


            //message about success
            TempData["SM"] = "You have edited the sidebar";

            //redirection
            return RedirectToAction("EditSidebar");
        }


    }
}