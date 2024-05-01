using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.DAL.ViewModals
{
    public  class RegisterViewModel
    {
        public required string Email {  get; set; }

        public required string Password { get; set; }

        public required string  ConfirmPassword { get; set; }   

    }
}
