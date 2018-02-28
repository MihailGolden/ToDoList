using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ToDoList.Models;
using Microsoft.AspNet.Identity;

namespace ToDoList.Controllers
{
    [Authorize]
    public class ToDoController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        //ok
        // GET: ToDo
        public ActionResult  Index()
        {
            return View();
        }

        //ok
        public JsonResult Add()
        {
            var userId = User.Identity.GetUserId();
            ApplicationUser user = db.Users.FirstOrDefault(x => x.Id == userId);
            int projectCount = db.Projects.Where(p => p.User.Id == userId).Count();
            string projectName = $"New project {Convert.ToString(projectCount + 1)}";

            Project project = new Project() { Name = projectName, User = user };
            db.Projects.Add(project);
            db.SaveChanges();
            return Json(project);
           // return View("Index");
        }

        //ok
        [HttpPost]
        public JsonResult Delete(int id)
        {
            if(id == 0)
            {
                return Json(new { error = "Error" });
            }
            else
            {
                Project project = db.Projects.Where(p => p.Id == id).FirstOrDefault();
                    if(project == null)
                    {
                        return Json(new { error = "Error" });
                    }
                //db.Projects.Remove(project);
                db.Entry(project).State = EntityState.Deleted;
                db.SaveChanges();
                return Json(new { id = project.Id });
            }
        }

        //ok
        [HttpPost]
        public JsonResult Update(int id, string name)
        {
                if (name == null || id == 0)
                {
                    return Json(new { error = "Error" });
                }
            Project project = db.Projects.Where(p => p.Id == id).FirstOrDefault();
                if(project == null)
                {
                    return Json(new { error = "Error" });
                }
            project.Name = name;
            db.Entry(project).State = EntityState.Modified;
            db.SaveChanges();
            return Json(new { status = "Ok" });
        }

        //ok
        [HttpGet]
        public JsonResult ShowAll()
        {
            var userId = User.Identity.GetUserId();
            ApplicationUser user = db.Users.FirstOrDefault(x => x.Id == userId);
            IEnumerable<Project> projects = db.Projects.Include(p => p.JobTasks).ToList().Where(p => p.User == user);
            foreach(Project proj in projects)
            {
                
                proj.JobTasks.OrderBy(j => j.Priority).ToList();
            }
            return Json(projects, JsonRequestBehavior.AllowGet);
        }

        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        db.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}
    }
}
