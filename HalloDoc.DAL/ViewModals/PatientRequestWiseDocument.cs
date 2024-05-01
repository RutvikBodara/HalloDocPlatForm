using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.DAL.ViewModals
{
    public class PatientRequestWiseDocument
    {

        public String document{  get; set; }

        public string uploader { get; set; }    

        public DateTime? uploaddate { get; set; }

        public int ReqId {  get; set; }
        
        public string? Email {  get; set; }

        public IFormFile? uploads {  get; set; }
    }
}
