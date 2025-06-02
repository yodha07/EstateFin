using EstateFin.Data;
using EstateFin.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EstateFin.Controllers
{
    public class PropertiesController : Controller
    {
        ApplicationDbContext db;
        private readonly IWebHostEnvironment env;

        public PropertiesController(ApplicationDbContext db, IWebHostEnvironment env) { this.db = db; this.env = env; }
        public IActionResult Index()
        {
            var list = db.Properties.ToList();
            return View(list);
        }


        [HttpPost]
        public IActionResult Index(string txt)
        {
            if (txt != null)
            {
                var list = db.Properties.Where(x => x.Title.Contains(txt) || x.Description.Contains(txt) || x.Price.ToString().Contains(txt) || x.Address.Contains(txt) || x.City.Contains(txt) || x.State.Contains(txt) || x.ZipCode.Contains(txt) || x.PropertyType.Contains(txt) || x.Status.Contains(txt)).ToList();

                return View(list);
            }
            else
            {
                var list = db.Properties.ToList();
                return View(list);
            }
        }



        public IActionResult add_properties()
        {
            //var my_propertiess = db.Property_Types.Where(x => x.status.Equals("Active")).Select(x => new SelectListItem
            //{
            //    Value = x.PropertyType.ToString(),
            //    Text = x.PropertyType.ToString()

            //}).ToList();
            //var dropdown = new dropdowns() 
            //{ 
            //    myproperties = my_propertiess
            //};
            //var propertiess = new properties() { };


            //var bind = new Bind
            //{
            //    properties = propertiess,

            //    dropdowns = dropdown
            //};
            ViewBag.myproperties = new SelectList(db.Property_Types.Where(x => x.status.Equals("Active")).ToList(), "MyPropertyId", "PropertyType");
            return View();
        }

        [HttpPost]
        public IActionResult add_properties(Bind prop)
        {
            prop.properties.userid = int.Parse(HttpContext.Session.GetString("Login"));

            var mpath = new List<string>();
            foreach (var item in prop.image.images)
            {
                string path = env.WebRootPath;
                string filepath = "Content/Images/" + item.FileName;
                string fullpath = Path.Combine(path, filepath);
                Fileupload(item, fullpath);
                mpath.Add("/" + filepath);

            }
            prop.properties.images = string.Join(",", mpath);
            prop.properties.CreatedAt = DateTime.Now;
            HttpContext.Session.SetInt32("PropertyID", prop.properties.PropertyId);
            
            //foreach (var modelError in ModelState)
            //{
            //    foreach (var error in modelError.Value.Errors)
            //    {
            //        Console.WriteLine($"Field: {modelError.Key} — Error: {error.ErrorMessage}");
            //    }
            //}

            if (ModelState.IsValid)
            {




                db.Properties.Add(prop.properties);
                db.SaveChanges();
                TempData["msg"] = "data added";
                return RedirectToAction("Index");


            }
            else
            {
                return View();
            }


        }
        protected void Fileupload(IFormFile file, string path)
        {
            FileStream stream = new FileStream(path, FileMode.Create);
            file.CopyTo(stream);

        }




        public IActionResult Delete_Properties(int id)
        {
            if (id != null)
            {
                var ids = db.Properties.Find(id);
                var list = db.Properties.Remove(ids);
                db.SaveChanges();
                TempData["del"] = "deleted";
                return RedirectToAction("Index");

            }
            else
            {
                return RedirectToAction("Index");
            }

        }
        public IActionResult Edit_Properties(int id)
        {
            var data = db.Properties.Find(id);
            ViewBag.editproperties = new SelectList(db.Property_Types.Where(x => x.status.Equals("Active")).ToList(), "MyPropertyId", "PropertyType");

            //var my_propertiess = db.Property_Types.Where(x => x.status.Equals("Active")).Select(x => new SelectListItem
            //{
            //    Text = x.PropertyType,
            //    Value = x.PropertyType
            //}).ToList();
            //var dropdown = new dropdowns()
            //{
            //    myproperties = my_propertiess ,

            //};
            //var propertiess = new properties() { } ;
            var bind = new Bind
            {
                properties = data,

            };

            return View(bind);
        }

        [HttpPost]
        public IActionResult Edit_Properties(Bind e)
        {
            var mpath = new List<string>();
            foreach (var item in e.image.images)
            {
                string path = env.WebRootPath;
                string filepath = "Content/Images/" + item.FileName;
                string fullpath = Path.Combine(path, filepath);
                Fileupload(item, fullpath);
                mpath.Add(filepath);

            }
            e.properties.images = string.Join(",", mpath);
            e.properties.CreatedAt = DateTime.Now;
            //foreach (var modelError in ModelState)
            //{
            //    foreach (var error in modelError.Value.Errors)
            //    {
            //        Console.WriteLine($"Field: {modelError.Key} — Error: {error.ErrorMessage}");
            //    }
            //}

            if (ModelState.IsValid)
            {




                var update = db.Properties.Update(e.properties);
                db.SaveChanges();
                TempData["updt"] = "updated successfully";
                return RedirectToAction("Index");



            }
            else
            {
                //var my_propertiess = db.Property_Types.Where(x => x.status.Equals("Active")).Select(x => new SelectListItem { Text = x.PropertyType, Value = x.PropertyType }).ToList();
                //e.dropdowns = new dropdowns() { myproperties = my_propertiess };
                return View();
            }



        }


        public IActionResult property_type()
        {
            return View();

        }

        [HttpPost]
        public IActionResult property_type(Property_Type e)
        {
            e.createdat = DateTime.Now;
            e.createdby = "session";

            if (ModelState.IsValid)
            {
                db.Property_Types.Add(e);
                db.SaveChanges();
                return RedirectToAction("property_type_list");
            }
            else
            {
                return View();
            }
        }
        public IActionResult property_type_list()
        {
            var e = db.Property_Types.ToList();

            return View(e);

        }

        public IActionResult delete_property_type(int id)
        {
            var ids = db.Property_Types.Find(id);
            db.Property_Types.Remove(ids);
            db.SaveChanges();

            return RedirectToAction("property_type_list");

        }

        public IActionResult edit_property_type(int id)
        {
            var ids = db.Property_Types.Find(id);
            return View(ids);
        }

        [HttpPost]
        public IActionResult edit_property_type(Property_Type e)
        {
            e.createdat = DateTime.Now;
            e.createdby = "Session";
            if (ModelState.IsValid)
            {
                db.Property_Types.Update(e);
                db.SaveChanges();
                return RedirectToAction("property_type_list");

            }
            else
            {
                return View();
            }



        }

        public IActionResult property_user()
        {
            var data = db.Properties.Where(x => x.Status.Equals("Available")).ToList();
            return View(data);



        }
    }

}
