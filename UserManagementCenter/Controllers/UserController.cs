using System;
using System.Collections.Generic;
using System.Data.Entity;
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

            if(id != 0)
            {
                using(DBModel db = new DBModel())
                {
                    usr = db.Users.Where(i => i.UserID == id).FirstOrDefault<User>();
                }
            }
            return View(usr);
        }

        [HttpPost]
        public ActionResult AddOrEdit(User usr)
        {
            try
            {
                if (usr.ImageUpload != null)
                {
                    string fileName = Path.GetFileNameWithoutExtension(usr.ImageUpload.FileName);
                    string extension = Path.GetExtension(usr.ImageUpload.FileName);
                    fileName = fileName + DateTime.Now.ToString("yymssfff") + extension;
                    usr.ImagePath = "~/AppFiles/Images/" + fileName;
                    usr.ImageUpload.SaveAs(Path.Combine(Server.MapPath("~/AppFiles/Images/"), fileName));
                }

                using (DBModel db = new DBModel())
                {
                    if(usr.UserID == 0)
                    {
                        db.Users.Add(usr);
                        db.SaveChanges();
                    }
                    else
                    {
                        db.Entry(usr).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                    
                }
                return Json(new { success = true, html = GlobalClass.RenderRazorViewToString(this, "ViewAll", GetAllUsers()), message = "Submitted Successfully" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new {success = false, message = ex.Message}, JsonRequestBehavior.AllowGet);
            }
        }


        public ActionResult Delete(int id)
        {
            try
            {
                using(DBModel db = new DBModel())
                {
                    User usr = db.Users.Where(x => x.UserID == id).FirstOrDefault<User>();

                    db.Users.Remove(usr);
                    db.SaveChanges();
                }
                return Json(new { success = true, html = GlobalClass.RenderRazorViewToString(this, "ViewAll", GetAllUsers()), message = "Record Successfully Removed" }, JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
            {
                return Json(new { success = false, message = ex.Message}, JsonRequestBehavior.AllowGet);
            }
        }
    }
}