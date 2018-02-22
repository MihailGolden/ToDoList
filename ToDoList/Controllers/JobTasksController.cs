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

        [HttpPost]
        public ActionResult Add(int projectId, string name)
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
            return Json(task);
        }

        [HttpPost]
        public void Update(int taskId, string name)
        {
            JobTask task = db.JobTasks.Where(t => t.Id == taskId).FirstOrDefault();
            task.Name = name;
        }

        [HttpPost]
        public ActionResult Delete(int taskId)
        {

            return View("Index", "ToDo");
        }








        //// GET: JobTasks
        //public async Task<ActionResult> Index()
        //{
        //    var tasks = db.JobTasks.Include(j => j.Project);
        //    return View(await tasks.ToListAsync());
        //}

        // GET: JobTasks/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            JobTask jobTask = await db.JobTasks.FindAsync(id);
            if (jobTask == null)
            {
                return HttpNotFound();
            }
            return View(jobTask);
        }

        // GET: JobTasks/Create
        public ActionResult Create()
        {
            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Name");
            return View();
        }

        // POST: JobTasks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Name,Status,ProjectId")] JobTask jobTask)
        {
            if (ModelState.IsValid)
            {
                db.JobTasks.Add(jobTask);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Name", jobTask.ProjectId);
            return View(jobTask);
        }

        // GET: JobTasks/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            JobTask jobTask = await db.JobTasks.FindAsync(id);
            if (jobTask == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Name", jobTask.ProjectId);
            return View(jobTask);
        }

        // POST: JobTasks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Name,Status,ProjectId")] JobTask jobTask)
        {
            if (ModelState.IsValid)
            {
                db.Entry(jobTask).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Name", jobTask.ProjectId);
            return View(jobTask);
        }

        // GET: JobTasks/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            JobTask jobTask = await db.JobTasks.FindAsync(id);
            if (jobTask == null)
            {
                return HttpNotFound();
            }
            return View(jobTask);
        }

        // POST: JobTasks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            JobTask jobTask = await db.JobTasks.FindAsync(id);
            db.JobTasks.Remove(jobTask);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
