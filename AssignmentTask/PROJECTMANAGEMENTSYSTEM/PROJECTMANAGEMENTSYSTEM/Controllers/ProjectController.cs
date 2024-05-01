
using Microsoft.AspNetCore.Mvc;

using ProjectManagement.BAL.Interfaces;
using ProjectManagement.DAL.DataModels;
using ProjectManagement.DAL.ViewModels.PopUp;
using ProjectManagement.DAL.ViewModels.Project;

namespace PROJECTMANAGEMENTSYSTEM.Controllers
{
    public class ProjectController : Controller
    {
        private readonly IProjectRepo _projectRepo;

        public ProjectController(IProjectRepo projectRepo)
        {
            _projectRepo = projectRepo;
        }

        #region HomePage
        public async Task<IActionResult> _ProjectDetailsPartial(string? Search , int? PageNumber)
        {

            try
            {
                ProjectTableMainViewModel ProjectData = await _projectRepo.ProjectTableMainViewModel(Search, PageNumber);
                 return PartialView("/Views/Project/_ProjectDetailsPartial.cshtml", ProjectData);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return RedirectToAction("ErrorPage");
            }
        }
        public async Task<IActionResult> _ProjectTablePartial()
        {
            return await Task.Run(()=> View());
        }
        public async Task<IActionResult> LoadAddProjectPartial()
        {
            try
            {
            return await Task.Run(()=> PartialView("/Views/PopUp/_AddProjectPartial.cshtml"));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return RedirectToAction("ErrorPage");
            }
        }
        public async Task<IActionResult> LoadEditProjectPartial(int ProjectID)
        {
            try
            {
                AddProjectViewModel? data = await _projectRepo.GetProjectData(ProjectID);
                return await Task.Run(() => PartialView("/Views/PopUp/_EditProjectPartial.cshtml", data));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return RedirectToAction("ErrorPage");
            }
        }
        public async Task<IActionResult> AddProjectPost(AddProjectViewModel ProjectData)
        {
            try
            {
                bool status = await _projectRepo.AddProjectPost(ProjectData);
                return Json(status);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return RedirectToAction("ErrorPage");

            }
        }
        //public async Task<IActionResult> LoadDomain()
        //{
        //    try
        //    {
        //    IEnumerable<Domain> domains = await _projectRepo.LoadDomain();
        //    return Json(domains);
        //    }
        //    catch(Exception e) {
        //        Console.WriteLine(e);
        //        return RedirectToAction("ErrorPage");
        //    }
        //}

        public async Task<IActionResult> EditProjectPost(AddProjectViewModel ProjectData)
        {
            bool status = await _projectRepo.EditProject(ProjectData);
            return Json(status);    
        }
        public async Task<IActionResult> DeleteProjectPost(int  ProjectID)
        {
            bool status = await _projectRepo.DeleteProject(ProjectID);
            return Json(status);
        }

        #endregion
        public IActionResult ErrorPage()
        {
            return View();  
        }
    }
}
