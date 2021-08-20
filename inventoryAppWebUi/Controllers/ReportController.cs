using System.Web.Mvc;
using AutoMapper;
using inventoryAppDomain.Entities.Enums;
using inventoryAppDomain.Services;
using inventoryAppWebUi.Models;

namespace inventoryAppWebUi.Controllers
{
    [Authorize]
    public class ReportController : Controller
    {
        private readonly IReportService _reportService;

        public ReportController(IReportService reportService)
        {
            _reportService = reportService;
        }
        
        // GET
        public ActionResult Index(TimeFrame timeFrame)
        {
            ViewBag.CurrentPage = timeFrame + " Report";
            var report = Mapper.Map<ReportViewModel>(_reportService.CreateReport(timeFrame));
            return View(report);
        }
    }
}