using Microsoft.EntityFrameworkCore;
using ProjectManagement.BAL.Interfaces;
using ProjectManagement.DAL.Data;
using ProjectManagement.DAL.DataModels;
using ProjectManagement.DAL.ViewModels.PopUp;
using ProjectManagement.DAL.ViewModels.Project;
using System.Text.RegularExpressions;


namespace ProjectManagement.BAL.Repository
{
    public class ProjectRepo : IProjectRepo
    {
        private readonly PMSDBContext _db;

        public ProjectRepo(PMSDBContext db)
        {
            _db = db;
        }
        public async Task<ProjectTableMainViewModel> ProjectTableMainViewModel(string? Search, int? PageNumber)
        {
            IEnumerable<Project>? ProjectDetails = _db.Projects.Where(x => (Search == null) || x.ProjectName.ToLower().Contains(Search.ToLower())).OrderBy(x=>x.Id);
            int count = ProjectDetails.Count();
            int page = 1;
            int maxPage = count / 2;
            if (count % 2 != 0)
            {
                maxPage += 1;
            }
            if (PageNumber != null)
            {
                if (PageNumber == 999999)
                {
                    if (page != maxPage)
                        PageNumber = count / 2 + 1;
                    else
                    {
                        PageNumber = 1;
                    }
                }
                page = (int)PageNumber;
            }
            var Paginateddata = ProjectDetails.Skip((page - 1) * 2).Take(2);
            ProjectTableMainViewModel model = new();
            model.ProjectTables = Paginateddata;
            model.PageNumber = page;
            model.MaxPage = maxPage;
            return await Task.Run(() => model);
        }
        public async Task<bool> AddProjectPost(AddProjectViewModel ProjectData)
        {
            //add project in table

            try
            {

          
                Domain DomainData = new Domain()
                {

                    Name = ProjectData.domain,
                };
                _db.Domains.Add(DomainData);
                await _db.SaveChangesAsync();

                var count = 0;

                if(_db.Projects.Count() != 0)
                {
                   count=_db.Projects.Max(x => x.Id);
                }

                //update project table
                Project NewProject = new()
                {
                    Id = ++count,
                    ProjectName = ProjectData.TaskName,
                    Description = ProjectData.Description,
                    Assignee = ProjectData.Assignee,
                    Domain = ProjectData.domain,
                    DomainId = DomainData.Id,
                    DueDate = ProjectData.Duedate,
                    City = ProjectData.city,
                };
                _db.Projects.Add(NewProject);
                _db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }

        public async Task<AddProjectViewModel?> GetProjectData(int ProjectID)
        {
            Project? project = _db.Projects.FirstOrDefault(x => x.Id == ProjectID);
            AddProjectViewModel model = null;
            if (project == null)
            {
                return model;
            }
            model = new()
            {
                city = project.City,
                domain = project.Domain,
                Assignee = project.Assignee,
                Description = project.Description,
                Duedate = project.DueDate,
                TaskName = project.ProjectName,
                Id=project.Id
            };
            return model;

        }
        public async Task<IEnumerable<Domain>?> LoadDomain()
        {
            return await Task.Run(()=> _db.Domains);
        }
        public async Task<bool> EditProject(AddProjectViewModel ProjectData)
        {
            Project? project = _db.Projects.FirstOrDefault(x=>x.Id == ProjectData.Id);

            if(project == null)
            {
                return false;
            }
            project.ProjectName=ProjectData.TaskName;
            project.Description=ProjectData.Description;
            project.City = ProjectData.city;
            project.Domain=ProjectData.domain;
            project.Assignee=ProjectData.Assignee;
            project.DueDate=ProjectData.Duedate;
            _db.Projects.Update(project);
            _db.SaveChanges();

            Domain? data =await _db.Domains.FirstOrDefaultAsync(x => x.Id == project.DomainId);
            if(data == null) { return false; }
            data.Name=ProjectData.domain;

            _db.Projects.Update(project);
            _db.SaveChanges();

            return await Task.Run(()=> true);
        }
        public async Task<bool> DeleteProject(int ProjectID)
        {
            
            Project? dataPRoject =await _db.Projects.FirstOrDefaultAsync(x => x.Id == ProjectID);
            if(dataPRoject == null)
            {
                return false;
            }

            Domain? dataDomain =await _db.Domains.FirstOrDefaultAsync(y => y.Id == dataPRoject.DomainId);
            if(dataDomain == null)
            {
                return false;
            }
            _db.Projects.Remove(dataPRoject);
            await _db.SaveChangesAsync();
            _db.Domains.Remove(dataDomain);

            await _db.SaveChangesAsync();
            return true;
        }
    }
}
