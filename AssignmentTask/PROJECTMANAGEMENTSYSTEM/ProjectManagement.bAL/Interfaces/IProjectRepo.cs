
using ProjectManagement.DAL.DataModels;
using ProjectManagement.DAL.ViewModels.PopUp;
using ProjectManagement.DAL.ViewModels.Project;

namespace ProjectManagement.BAL.Interfaces
{
    public  interface IProjectRepo
    {
        Task<ProjectTableMainViewModel> ProjectTableMainViewModel(string? Search, int? PageNumber);
        Task<bool> AddProjectPost(AddProjectViewModel ProjectData);
        Task<AddProjectViewModel?> GetProjectData(int ProjectID);
        Task<bool> EditProject(AddProjectViewModel ProjectData);
        Task<IEnumerable<Domain>?> LoadDomain();
        Task<bool> DeleteProject(int ProjectID);
    }
}
