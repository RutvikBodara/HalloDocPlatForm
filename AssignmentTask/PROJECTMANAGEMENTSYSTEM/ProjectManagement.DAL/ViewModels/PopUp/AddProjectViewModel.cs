
using System.ComponentModel.DataAnnotations;


namespace ProjectManagement.DAL.ViewModels.PopUp
{
    public class AddProjectViewModel
    {
        [Required]
        public required string TaskName { get; set; }
        [Required]
        public required string Assignee { get; set;}
        [Required]
        public required string Description { get; set;}
        [Required]
        public required DateOnly Duedate { get; set;}
        [Required]
        public required string city { get; set;}
        [Required]
        public required string domain { get; set;}
        public int? Id { get; set;} 
    }
}
