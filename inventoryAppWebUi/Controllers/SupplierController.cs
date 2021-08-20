using AutoMapper;
using inventoryAppDomain.Entities;
using inventoryAppDomain.Entities.Enums;
using inventoryAppDomain.IdentityEntities;
using inventoryAppDomain.Services;
using inventoryAppWebUi.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace inventoryAppWebUi.Controllers
{
    [Authorize(Roles = "Admin")]
    public class SupplierController : Controller
    {
        private readonly ISupplierService _supplierService;

        public SupplierController(ISupplierService supplierService)
        {
            _supplierService = supplierService;
        }
        public ActionResult AllSuppliers()
        {
            var suppliers = Mapper.Map<IEnumerable<SupplierViewModel>>(_supplierService.GetAllSuppliers());
            
            return View(suppliers);
        }
        // GET
        public ActionResult AddSupplier()
        {
            ViewBag.TagNumber = _supplierService.GenerateTagNumber();

            return PartialView("_SupplierPartial", new SupplierViewModel());
        }

        [HttpPost]
        public ActionResult Save(SupplierViewModel supplier)
        {
            if (!ModelState.IsValid)
            {
                TempData["failed"] = "failed";
                Response.StatusCode = 201;
                return PartialView("_SupplierPartial", supplier);
            }

            //Add new supplier
            try
            {
                if (supplier.Id == 0)
                {
                    var allSuppliers = _supplierService.GetAllSuppliers();
                    var tagAlreadyExists = allSuppliers.Any(s => s.TagNumber == supplier.TagNumber);
                    if (tagAlreadyExists)
                    {
                        ModelState.AddModelError("Supplier Tag", "Supplier with this tag already exists");
                        return View("AddSupplier", supplier);
                    }
                    else
                    {
                        var newSupplier = Mapper.Map<SupplierViewModel, Supplier>(supplier);
                        _supplierService.AddSupplier(Mapper.Map<SupplierViewModel, Supplier>(supplier));
                        TempData["supplierAdded"] = "added";
                    }
                }
                else
                {
                    //Update the existing supplier in DB
                    var supplierInDb = _supplierService.FindSupplier(supplier.Id);

                    if (supplierInDb == null)
                        return HttpNotFound(SupplierErrorsMessages.supplierNotFound);

                    _supplierService.UpdateSupplier(Mapper.Map(supplier, supplierInDb));
                    TempData["supplierAdded"] = "added";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            // return RedirectToAction("AllSuppliers");
            return Json(new { response = "success" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ProcessSupplier(int id)
        {
            var supplier = _supplierService.FindSupplier(id);

            if (supplier == null)
                return HttpNotFound(SupplierErrorsMessages.supplierNotFound);
                //return View("EPage", SupplierErrorsMessages.supplierNotFound);
            
            if(supplier.Status == SupplierStatus.Active)
                supplier.Status = SupplierStatus.InActive;
            else
                supplier.Status = SupplierStatus.Active;

            bool processSupplier = _supplierService.ProcessSupplier(id, supplier.Status);

            if (processSupplier == true)
                return RedirectToAction("AllSuppliers");

            return HttpNotFound(SupplierErrorsMessages.supplierNotFound);
        }

        public ActionResult EditSupplier(int id)
        {
            var supplier = Mapper.Map<SupplierViewModel>(_supplierService.FindSupplier(id));

            if (supplier == null)
                return HttpNotFound(SupplierErrorsMessages.supplierNotFound);
            //    return View("EPage", SupplierErrorsMessages.supplierNotFound);

            return PartialView("_SupplierPartial", supplier);
        }

        public ActionResult SupplierAndProductDetails(int id)
        {
            var supplier = Mapper.Map<SupplierViewModel>(_supplierService.FindSupplier(id));

            if (supplier == null)
                return HttpNotFound(SupplierErrorsMessages.supplierNotFound);

            var ProductsBySupplier = Mapper.Map<IEnumerable<ProductViewModel>>(_supplierService.GetAllProductsBySupplier(supplier.TagNumber));

            if (ProductsBySupplier == null)
                return HttpNotFound("Not Products found by supplier!");

            var supplierAndProducts = new SupplierAndProductsViewModel
            {
                SupplierViewModel = supplier,
                ProductViewModel = ProductsBySupplier
            };

            return View("SupplierDetails", supplierAndProducts);
        }

        public ActionResult GetProductsBySupplier(string supplierTag)
        {
            var ProductsBySupplier = Mapper.Map<IEnumerable<ProductViewModel>>(_supplierService.GetAllProductsBySupplier(supplierTag));

            if (ProductsBySupplier == null)
                return HttpNotFound("Not Products found by supplier!");

            return View(ProductsBySupplier);
        }

    }
}