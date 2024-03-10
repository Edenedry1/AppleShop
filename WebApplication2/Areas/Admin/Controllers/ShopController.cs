using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication2.Models.Data;
using WebApplication2.Models.ViewModels.Shop;

namespace WebApplication2.Areas.Admin.Controllers
{
    public class ShopController : Controller
    {
        // GET: Admin/Shop
        public ActionResult Categories()
        {
            //list model
            List<CategoryVM> categoryVMList;

            using(Db db =new Db())
            {
                //initialize model with data
                categoryVMList = db.Categories.ToArray().OrderBy(x => x.Sorting).Select(x => new CategoryVM(x)).ToList();
            }



            //return list with data
            return View(categoryVMList);
        }


        // POST: Admin/Shop/AddNewCategory
        [HttpPost]
        public string AddNewCategory(string catName)
        {
            //get Id
            string id;
            using (Db db = new Db())
            {
                //check the title unique
                if (db.Categories.Any(x => x.Name == catName))
                    return "titletaken";
                //initialize model DTO
                CategoryDTO dto = new CategoryDTO();
                //add data to model
                dto.Name = catName;
                dto.Slug = catName.Replace(" ", "-").ToLower();
                dto.Sorting = 100;
                //save changes
                db.Categories.Add(dto);
                db.SaveChanges();
                //get id for return to the view
                id = dto.Id.ToString();
            }
            //return id

            return id;

            
        }

        // POST: Admin/Shop/reordercategories
        [HttpPost]
        public void ReorderCategories(int[] id)
        {
            using (Db db = new Db())
            {
                int count = 1;
                CategoryDTO dto;

                foreach (var catId in id)
                {
                    dto = db.Categories.Find(catId);
                    dto.Sorting = count;
                    db.SaveChanges();

                    count++;
                }
            }
        }


        //GET: Admin/Shop/DeleteCategory/id
        public ActionResult DeleteCategory(int id)
        {
            using (Db db = new Db())
            {
                //get the pcategory
                CategoryDTO dto = db.Categories.Find(id);

                //delete the category
                db.Categories.Remove(dto);
                //save the changes
                db.SaveChanges();

            }
            //message about deleting
            TempData["SM"] = "You have deleted a category!";
            //redirection
            return RedirectToAction("Categories");
        }
    }
}