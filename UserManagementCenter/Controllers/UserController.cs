using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UserManagementCenter.Models;

namespace UserManagementCenter.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ViewAll()
        {
            return View(GetAllUsers());
        }

        IEnumerable<User> GetAllUsers()
        {
            using(DBModel db = new DBModel())
            {
                return db.Users.ToList<User>();
            }
            // return View();
        }

        public ActionResult AddOrEdit(int id = 0)
        {
            User usr = new User();
            return View(usr);
        }

        [HttpPost]
        public ActionResult AddOrEdit(User usr)
        {
            if (usr.ImageUpload != null)
            {
                string fileName = Path.GetFileNameWithoutExtension(usr.ImageUpload.FileName);
                string extension = Path.GetExtension(usr.ImageUpload.FileName);
                fileName = fileName + DateTime.Now.ToString("yymssfff") + extension;
                usr.ImagePath = "~/AppFiles/Images/" + fileName;
                usr.ImageUpload.SaveAs(Path.Combine(Server.MapPath("~/AppFiles/Images/"), fileName));
            }

            using(DBModel db = new DBModel())
            {
                db.Users.Add(usr);
                db.SaveChanges();
            }
            return RedirectToAction("ViewAll");
        }
    }
}