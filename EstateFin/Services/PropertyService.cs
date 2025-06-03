using System.Reflection.Metadata.Ecma335;
using EstateFin.Data;
using EstateFin.Models;
using EstateFin.Repositories;

namespace EstateFin.Services
{
    public class PropertyService : PropertyRepo
    {
        ApplicationDbContext db;
        private readonly IWebHostEnvironment env;

        public PropertyService(ApplicationDbContext db, IWebHostEnvironment env) { this.db = db; this.env = env; }
        public List<Property> GetProperties()
        {
            var list = db.Properties.ToList();
            return list;

        }
        public List<Property_Type> dropdown()
        {
            var list = db.Property_Types.Where(x => x.status.Equals("Active")).ToList();
            return list;
        }
        public List<string> propertyfile(Bind prop)
        {

            var mpath = new List<string>();
            foreach (var item in prop.image.images)
            {
                string path = env.WebRootPath;
                string filepath = "Content/Images/" + item.FileName;
                string fullpath = Path.Combine(path, filepath);
                Fileupload(item, fullpath);
                mpath.Add("/" + filepath);

            }
            return mpath;

        }
        protected void Fileupload(IFormFile file, string path)
        {
            FileStream stream = new FileStream(path, FileMode.Create);
            file.CopyTo(stream);

        }

        public void add_property(Bind prop)
        {
            db.Properties.Add(prop.properties);
            db.SaveChanges();
        }

        public void delete_properties(int id)
        {
            var ids = db.Properties.Find(id);
            var list = db.Properties.Remove(ids);
            db.SaveChanges();
        }

        public Bind edit_property(int id)
        {
            var data = db.Properties.Find(id);
            var bind = new Bind
            {
                properties = data,

            };
            return bind;
        }

        public void edit_property_post(Bind e)

        {
            var update = db.Properties.Update(e.properties);
            db.SaveChanges();
        }

        public void property_typess(Property_Type e)
        {
            db.Property_Types.Add(e);
            db.SaveChanges();

        }
        public void delete_property_typess(int id)
        {
            var ids = db.Property_Types.Find(id);
            db.Property_Types.Remove(ids);
            db.SaveChanges();
        }

        public void edit_property_typess(Property_Type e)
        {
            db.Property_Types.Update(e);
            db.SaveChanges();
        }








    }



}