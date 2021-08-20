using System.Web.Mvc;
using inventoryAppDomain.Services;

namespace inventoryAppWebUi.Controllers
{
    public class RoleController : Controller
    {
        private readonly IRoleService _roleService;

        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }
        
        // GET
        public ActionResult Index()
        {
            var roles = _roleService.GetAllRoles();
            return View(roles);
        }

        public ActionResult Create()
        {
            return PartialView("_RolePartial");
        }

        [HttpPost]
        public ActionResult Create(string roleName)
        {
            if (!ModelState.IsValid)
            {
                TempData["failed"] = "failed";
                
            }
            else
            {
                var role = _roleService.Create(roleName);
                TempData["roleAdded"] = "added";
                

            }
            return Json(new { response = "success" }, JsonRequestBehavior.AllowGet);
        }
    }
}