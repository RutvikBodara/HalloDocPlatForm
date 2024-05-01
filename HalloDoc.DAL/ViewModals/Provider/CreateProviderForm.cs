using HalloDoc.DAL.DataModels;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.DAL.ViewModals.Provider
{
    public class CreateProviderForm
    {
        public  Aspnetuser? aspnetuser {  get; set; }    
        public  required Physician physician {  get; set; } 
       
        public IFormFile? ProviderPhoto { get; set; }
        public IFormFile? file1 { get; set; }
        public IFormFile? file2 { get; set; }  
        public IFormFile? file3 { get; set; }
        public IFormFile? file4 { get; set; }
        public IFormFile? file5 { get; set; }
    }
}
