using Microsoft.AspNetCore.Mvc;

namespace hellodocsrsmvc.Controllers
{
    public class AssignmentController : Controller
    {
        public IActionResult FirstFile()
        {
            return View();
        }
    }
}
