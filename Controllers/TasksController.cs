using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using CapStone_TaskList_MVC.Models;
using Microsoft.AspNetCore.Mvc;

namespace CapStone_TaskList_MVC.Controllers
{
    public class TasksController : Controller
    {
        private readonly IdentityTasksListsDbContext _context;

        public TasksController(IdentityTasksListsDbContext context)
        {
            _context = context;
        }

        //Index controller.  Lists all tasks in the db
        public IActionResult Index()
        {
            string id = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            List<Tasks> thisUsersTasks = _context.Tasks.Where(x => x.TaskOwnerId == id).ToList();
            return View("Index", thisUsersTasks);
        }

        //Controller to add new tasks.  Gets the form from the AddTask view first
        [HttpGet]
        public IActionResult AddTask()
        {
            //returns the AddTask view
            return View();
        }

        //Controller to add the new task to the db
        [HttpPost]
        public IActionResult AddTask(Tasks newTask)
        {
            newTask.TaskOwnerId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            if (ModelState.IsValid)
            {
                _context.Tasks.Add(newTask);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("AddTask");
            }
        }

        //Controller to update a task.  Gets the form from the UpdateTasks view first
        [HttpGet]
        public IActionResult UpdateTasks(int id)
        {
            Tasks foundTask = _context.Tasks.Find(id);
            return View(foundTask);
        }

        //Controller to update the current user task to the db
        [HttpPost]
        public IActionResult UpdateTasks(Tasks updatedTasks)
        {
            //Finds the task to be updated by Id
            Tasks dbTasks = _context.Tasks.Find(updatedTasks.Id);

            //checks to make sure Model is valid
            if (ModelState.IsValid)
            {
                dbTasks.TaskDescription = updatedTasks.TaskDescription;
                dbTasks.DueDate = updatedTasks.DueDate;
                dbTasks.Completion = updatedTasks.Completion;

                //when using _context.Update, you need to change the Entry state.
                _context.Entry(dbTasks).State = Microsoft.EntityFrameworkCore.EntityState.Modified;

                _context.Update(dbTasks);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        //Controller to delete a task from the db
        public IActionResult DeleteTask(int Id)
        {
            Tasks found = _context.Tasks.Find(Id);
            if (found != null)
            {
                _context.Remove(found);
                _context.SaveChanges();

            }

            List<Tasks> thisUsersTasks = _context.Tasks.Where(x => x.Id == Id).ToList();
            return RedirectToAction("Index", "Tasks");
        }
    }
}