using HalloDoc.DAL.DataModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.DAL.ViewModals.Access
{
    public class Rolemenucombine
    {
        [Required]
        public Role? Role {  get; set; }
        public IEnumerable<Menu?>? menu { get; set; }
        public required IEnumerable<AccountType> accounttype {  get; set; }
    }
}
