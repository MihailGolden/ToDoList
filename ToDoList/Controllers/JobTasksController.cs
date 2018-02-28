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

        [HttpPost]
        public ActionResult Priority(Priority model)
        {
            switch (model.Change)
            {
                case Models.Change.Up:
                    var target = db.JobTasks
                        .Where(t => t.Id == model.ProjectId && t.Priority < model.CurrentPriority)
                        .OrderByDescending(t => t.Priority)
                        .Select(t => new
                        {
                            targetId = t.Id,
                            targetPriority = t.Priority
                        })
                        .FirstOrDefault();
                    if (target != null)
                    {
                        model.TargetId = target.targetId;
                        model.TargetPriority = target.targetPriority;
                    }
                    else { return Json(new { error = "Error" }); }
                    break;
                case Models.Change.Down:
                    target = db.JobTasks
                        .Where(t => t.Id == model.ProjectId && t.Priority > model.CurrentPriority)
                        .OrderBy(t => t.Priority)
                        .Select(t => new
                        {
                            targetId = t.Id,
                            targetPriority = t.Priority
                        })
                        .FirstOrDefault();
                    if (target != null)
                    {
                        model.TargetId = target.targetId;
                        model.TargetPriority = target.targetPriority;
                    }
                    else { return Json(new { error = "Error" }); }
                    break;
            }
            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    JobTask currentTask = new JobTask { Id = model.CurrentId, Priority = model.TargetPriority };
                    JobTask targetTask = new JobTask { Id = model.TargetId, Priority = model.CurrentPriority };
                    db.JobTasks.Attach(currentTask);
                    db.JobTasks.Attach(targetTask);
                    db.Entry(currentTask).Property(ct => ct.Priority).IsModified = true;
                    db.Entry(targetTask).Property(tt => tt.Priority).IsModified = true;
                    db.SaveChanges();
                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                    return Json(new { error = "Error!" });
                }
            }
            return Json(model);
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
