//using AdminHalloDoc.Controllers.Login;
//using HalloDoc.DAL.Data;
//using HalloDoc.DAL.ViewModals;
//using HalloDoc.DAL.ViewModals.AdminDashBoardViewModels;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;

//namespace hellodocsrsmvc.Controllers
//{
//    [AdminAuth("admin")]
//    public class AdminDashBoardPartialsController : Controller
//    {
//        private readonly HalloDocDBContext _db;

//        public AdminDashBoardPartialsController(HalloDocDBContext db)
//        {
//            _db = db;
//        }
//        public IActionResult NewTablePartial()
//        {
//            return PartialView("/Views/AdminPartials/AdminDashBoardPartials/_NewTablePartial.cshtml");
//        }
//        public IActionResult PendingTablePartial()
//        {
//            return PartialView("/Views/AdminPartials/AdminDashBoardPartials/_PendingTablePartial.cshtml");
//        }
//    }
//}
