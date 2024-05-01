using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.DAL.ViewModals
{
    public class Loginviewmodel
    {
        [Column("username")]
        [StringLength(256)]
        public string Username { get; set; } = null!;

        [Column("passwordhash")]
        public string? Passwordhash { get; set; }
    }
}
