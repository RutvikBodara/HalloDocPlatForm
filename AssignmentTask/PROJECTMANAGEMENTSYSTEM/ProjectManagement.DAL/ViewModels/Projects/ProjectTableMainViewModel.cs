using Microsoft.Build.Evaluation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagement.DAL.ViewModels.Project
{
    public class ProjectTableMainViewModel
    {
        public IEnumerable<DataModels.Project>? ProjectTables { get; set; }
        public int? PageNumber { get; set; }
        public int? MaxPage { get; set;}
    }
}
