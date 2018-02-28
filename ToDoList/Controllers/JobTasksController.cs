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

namespace ToDoList.Controllers
{
    public class JobTasksController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        //ok
        [HttpPost]
        public JsonResult Add(int projectId, string name)
        {
            var tempTask = db.JobTasks.Where(t => t.ProjectId == projectId).OrderByDescending(t => t.Priority).FirstOrDefault();
            int priority;
            if(tempTask == null)
            {
                priority = 1;
            }
            else
            {
                priority = tempTask.Priority + 1;
            }
            JobTask task = new JobTask { Name = name, ProjectId = projectId, Priority = priority };
            db.JobTasks.Add(task);
            db.SaveChanges();
            return Json(task);
        }


        //ok
        [HttpPost]
        public JsonResult Update(int taskId, string name)
        {
            if(taskId == 0 || name == null)
            {
                return Json(new { error = "Error" });
            }
            JobTask task = db.JobTasks.Where(t => t.Id == taskId).FirstOrDefault();
            task.Name = name;
            db.SaveChanges();
            return Json(new { status = "Ok" });
        }


        //ok
        [HttpPost]
        public JsonResult Delete(int taskId)
        {
            if( taskId == 0)
            {
                return Json(new { error = "Error" });
            }
            else
            {
                JobTask task = db.JobTasks.Where(t => t.Id == taskId).FirstOrDefault();
                db.Entry(task).State = EntityState.Deleted;
                db.SaveChanges();
            }
            return Json(new { id = taskId });
        }

        //ok
        [HttpPost]
        public void Check(int taskId, bool done)
        {
            JobTask task = db.JobTasks.Where(t => t.Id == taskId).FirstOrDefault();
            task.Status = done;
            db.Entry(task).State = EntityState.Modified;
            db.SaveChanges();
        }

        //ok
        [HttpPost]
        public void SetDate(int taskId, string date)
        {
            DateTime? deadline = (date == "") ? (DateTime?)null : DateTime.Parse(date);
            JobTask task = db.JobTasks.Where(t => t.Id == taskId).FirstOrDefault();
            task.Deadline = deadline;
            db.Entry(task).State = EntityState.Modified;
            db.SaveChanges();
        }


        public JsonResult Priority(Priority model)
        {
            //create latter
            return null;
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
