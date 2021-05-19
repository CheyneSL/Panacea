using Korzh.EasyQuery.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Project.Models;
using System;
using System.Linq;
using System.IO;
using System.Web;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Project.Controllers
{
    public class ApplicationController : Controller
    {
        private UserManager<User> UserMgr { get; }
        private SignInManager<User> SignInMgr { get; }
        private readonly IBugRepository BugRepository;
        private readonly IProjectRepository ProjectRepository;
        private readonly ImageDbContext _imageDbContext;
        private readonly IWebHostEnvironment _hostEnvironment;

        public ApplicationController(UserManager<User> userManager, SignInManager<User> signInManager, IBugRepository bugRepository, IProjectRepository projectRepository, 
            ImageDbContext imageDbContext, IWebHostEnvironment hostEnvironment)
        {
            UserMgr = userManager;
            SignInMgr = signInManager;
            BugRepository = bugRepository;
            ProjectRepository = projectRepository;
            _imageDbContext = imageDbContext;
            _hostEnvironment = hostEnvironment;
        }

        /*----------DASHBOARD----------*/
        [Authorize]
        [HttpGet]
        public ActionResult Dashboard()
        {
            ViewBag.Page = "Dashboard";

            var model = new SingleBugViewModel
            {
                Bugs = BugRepository.GetAllBugs().Where(b => b.UserID == UserMgr.GetUserId(User) && b.Open == true).AsQueryable().OrderByDescending(b => b.BugID)
            };
            return View(model);
        }

        [Authorize]
        [HttpPost]
        public ActionResult Dashboard(SingleBugViewModel model)
        {
            if (!string.IsNullOrEmpty(model.Query))
                model.Bugs = BugRepository.GetAllBugs().Where(
                    b => b.UserID == UserMgr.GetUserId(User) && b.Open == true).AsQueryable().FullTextSearchQuery(model.Query).OrderByDescending(b => b.BugID);
            else
                model.Bugs = BugRepository.GetAllBugs().Where(b => b.UserID == UserMgr.GetUserId(User) && b.Open == true).AsQueryable().OrderByDescending(b => b.BugID);

            return View(model);
        }

        /*----------PROJECT REGISTRATION----------*/
        [Authorize]
        [HttpGet]
        public ActionResult ProjectRegistration()
        {
            ViewBag.Page = "Project Registration";
            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult ProjectRegistration(ProjectModel project)
        {
            project.UserID = UserMgr.GetUserId(User);
            project.CreatedOn = DateTime.Now;
            ProjectRepository.Create(project);
            return Redirect("Dashboard");

        }

        /*----------BUG SUBMISSION----------*/
        [Authorize]
        [HttpGet]
        public ActionResult BugSubmission()
        {
            ViewBag.Page = "Bug Submission";
            if (ProjectRepository.GetAllProjects().Where(p => p.UserID == UserMgr.GetUserId(User)).Count() < 1)
                return Redirect("ProjectRegistration");
            else
                return View(
            new ProjectBugVM
            {
                Bug = new Bug(),
                Projects = ProjectRepository.GetAllProjects().Where(p => p.UserID == UserMgr.GetUserId(User))
            });
        }

        
       [Authorize]
        [HttpPost]
        public ActionResult BugSubmission(Bug bug)
        {
            bug.UserID = UserMgr.GetUserId(User);
            bug.CreatedOn = DateTime.Now;
            bug.LastModified = DateTime.Now;
            bug.Open = true;
            bug.StepsToReproduce = null;
            bug.ProjectName = ProjectRepository.GetProject(bug.ProjectID).Name;

            BugRepository.Create(bug);
            return Redirect("Dashboard");
        }

       /* [Authorize]
        [HttpPost("UploadFile")]
        public async Task<IActionResult> BugSubmission(Bug bug, List<IFormFile> images)
        {
            long size = images.Sum(f => f.Length);
            var filePaths = new List<string>();

            foreach (var image in images)
            {
                if (image.Length > 0)
                {
                    var filePath = Path.GetTempFileName();
                    filePaths.Add(filePath);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await image.CopyToAsync(stream);
                    }
                }
            }

            return Ok(new { count = images.Count, size, filePaths });
        }
        */

        /*[Authorize]
        [HttpPost]
        public async Task<IActionResult> BugSubmission(Bug bug, [Bind("imageID, imageTitle, ImageFile")] Image image)
        {
            if (ModelState.IsValid)
            {
                bug.UserID = UserMgr.GetUserId(User);
                bug.CreatedOn = DateTime.Now;
                bug.LastModified = DateTime.Now;
                bug.Open = true;
                bug.Solution = null;
                bug.ProjectName = ProjectRepository.GetProject(bug.ProjectID).Name;
                BugRepository.Create(bug);


                string wwwRootPath = _hostEnvironment.WebRootPath;
                string fileName = Path.GetFileNameWithoutExtension(image.ImageFile.FileName);
                string extension = Path.GetExtension(image.ImageFile.FileName);
                image.imageName = fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                string path = Path.Combine(wwwRootPath + "/userImages/", fileName);
                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    await image.ImageFile.CopyToAsync(fileStream);
                }

                _imageDbContext.Add(image);
                await _imageDbContext.SaveChangesAsync();

                return RedirectToAction("Dashboard");
            }

            return RedirectToAction("Dashboard");
        }*/

        /*----------ARCHIVE----------*/
        [Authorize]
        [HttpGet]
        public ActionResult Archive()
        {
            ViewBag.Page = "Archive";
            var model = new SingleBugViewModel
            {
                Bugs = BugRepository.GetAllBugs().Where(b => b.UserID == UserMgr.GetUserId(User) && b.Open == false).AsQueryable().OrderByDescending(b => b.BugID)
            };
            return View(model);
        }

        /*----------ACTIVE BUG VIEW----------*/
        [Authorize]
        [ActionName("BugView"), HttpGet]
        public ActionResult BugViewGet(Bug bug)
        {
            ViewBag.Page = "Bug Details";
            return View(bug);
        }

        [Authorize]
        [ActionName("BugView"), HttpPost]
        public ActionResult BugViewPost(Bug bug, string submit)
        {
            switch (submit)
            {
                case "Update":
                    BugRepository.Update(bug);
                    break;
                case "Close":
                    bug.Open = false;
                    BugRepository.Update(bug);
                    break;
                case "Delete":
                    BugRepository.Delete(bug.BugID);
                    break;
            }
        
            return RedirectToAction("Dashboard");
        }

        /*----------ARCHIVE BUG VIEW----------*/
        [Authorize]
        [ActionName("ArchiveBugView"), HttpGet]
        public ActionResult ArchiveBugViewGet(Bug bug)
        {
            ViewBag.Page = "Bug Details";
            return View(bug);
        }

        [Authorize]
        [ActionName("ArchiveBugView"), HttpPost]
        public ActionResult ArchiveBugView(Bug bug)
        {
            BugRepository.Delete(bug.BugID);
            return RedirectToAction("Archive");
        }
    }
}


