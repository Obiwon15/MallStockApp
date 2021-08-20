using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using AutoMapper;
using inventoryAppDomain.Entities;
using inventoryAppDomain.Entities.Enums;
using inventoryAppDomain.Services;
using inventoryAppWebUi.Models;

namespace inventoryAppWebUi.Controllers
{
	[Authorize]
	public class ProductController : Controller
	{
		private readonly IProductService _ProductService;
		private readonly ISupplierService _supplierService;

		public ProductController(IProductService ProductService, ISupplierService supplierService)
		{
			_ProductService = ProductService;
			_supplierService = supplierService;

		}
		// GET: Product
		public ActionResult AllProducts()
		{
			return View(_ProductService.GetAllProducts());
		}
		public async Task<ActionResult> AvailableProductsAsync()
		{
			await _ProductService.NotifyProductExpirationAsync();
			var Products = _ProductService.GetAvailableProducts();

			return View(Products);
		}

        public ActionResult GetExpiredProducts()
        {
			var expiredProducts = _ProductService.GetAllExpiredProducts();

			return View(expiredProducts);
        }

        public ActionResult GetOutOfStockProducts()
        {
			var outOfStockProducts = _ProductService.GetOutOfStockProducts();

			return View(outOfStockProducts);
        }

		public ActionResult FilteredProductsList(string searchString)
		{
			var Products = _ProductService.GetAvailableProducts();
			var ProductFilter = _ProductService.GetAvailableProductFilter(searchString);
			if (string.IsNullOrWhiteSpace(searchString) || string.IsNullOrEmpty(searchString))
			{
				var ProductsVM = new ProductSearchViewModel
				{
					Products = Products,
					SearchString = searchString
				};
				return View(ProductsVM);
			}
			var ProductsearchVM = new ProductSearchViewModel
			{
				Products = ProductFilter,
				SearchString = searchString
			};
			return View(ProductsearchVM);
		}


		public ActionResult AddProductForm()
		{
			var ProductCategory = new ProductViewModel
			{
				ProductCategory = _ProductService.AllCategories()
			};

			return PartialView("_ProductPartial", ProductCategory);
		}

		public ActionResult UpdateProduct(int id)
		{

			var ProductInDb = Mapper.Map<Product, ProductViewModel>(_ProductService.EditProduct(id));

			if (ProductInDb == null) return HttpNotFound("No Product found");

			ProductInDb.ProductCategory = _ProductService.AllCategories();

			return PartialView("_ProductPartial", ProductInDb);
		}

		public ActionResult SaveProduct(ProductViewModel product)
		{
			if (!ModelState.IsValid)
			{
				product.ProductCategory = _ProductService.AllCategories();
				TempData["failed"] = "failed";
				return PartialView("_ProductPartial", product);
			}

			try
			{
				var supplierInDb = _supplierService.GetSupplierWithTag(product.SupplierTag);

				if (supplierInDb == null)
				{
					 SetModelStateError(product, "SupplierTag", "Supplier Tag isn't registered yet");
					return PartialView("_ProductPartial", product);
				}

				//ADD A NEW PRODUCT
				if (product.Id == 0)
				{
					bool result = ValidateProduct(product, supplierInDb);

					if(!result) return PartialView("_ProductPartial", product);
					
					var newProduct = Mapper.Map<ProductViewModel, Product>(product);
					_ProductService.AddProduct(newProduct);

				}
				else
				{
					// UPDATE EXISTING PRODUCT

					var getProductInDb = _ProductService.EditProduct(product.Id);

					if (getProductInDb == null)
						return HttpNotFound("Product not found!");

					bool result = ValidateProduct(product, supplierInDb);

					if (!result)
						return PartialView("_ProductPartial", product);

					_ProductService.UpdateProduct(Mapper.Map(product, getProductInDb));

				}
				TempData["added"] = "added";
			}
			catch (Exception ex)
			{

				Console.WriteLine(ex.Message);
				return Json(new { response = ex.Message }, JsonRequestBehavior.AllowGet);

			}
			return Json(new { response = "success" }, JsonRequestBehavior.AllowGet);

		}

		private void SetModelStateError(ProductViewModel product, string key, string errorMessage)
		{
			ModelState.AddModelError(key, errorMessage);
			TempData["failed"] = "failed";
			product.ProductCategory = _ProductService.AllCategories();
		}


		private bool ValidateProduct(ProductViewModel product, Supplier supplier)
        {
			var expiryDate = _ProductService.DateComparison(DateTime.Today, product.ExpiryDate);

			if (expiryDate >= 0)
            {
				SetModelStateError(product, "ExpiryDate", "Must be later than today");
				return false;
			}
			else if(supplier.Status == SupplierStatus.InActive)
			{
				SetModelStateError(product, "SupplierTag", "Supplier has been deactivated");
				return false;
			}
			else if(product.Quantity <= 0)
            {
				SetModelStateError(product, "Quantity", "Quantity should be greater than zero");
				return false;
			}
			else if (product.Price <= 0)
            {
				SetModelStateError(product, "Price", "Price should be greater than zero");
				return false;
			}
			return true;
		}	

		[HttpGet]
		public ActionResult AddProductCategory()
		{
			return PartialView("_CategoryPartial");
		}

		[HttpPost]
		public ActionResult SaveProductCategory(ProductCategoryViewModel category)
		{
			if (ModelState.IsValid)
			{

				if (string.IsNullOrWhiteSpace(category.CategoryName))
				{
					ModelState.AddModelError("Category Name", @"Please input category");
					//return Json(new { response = "failure", cat = category }, JsonRequestBehavior.AllowGet);

					return PartialView("_CategoryPartial", category);

				}

				var cate = Mapper.Map<ProductCategoryViewModel, ProductCategory>(category);
				_ProductService.AddProductCategory(cate);

				TempData["categoryAdded"] = "added";
				return Json(new { response = "success" }, JsonRequestBehavior.AllowGet);

			}
			TempData["failedToAddCategory"] = "failed";
			return PartialView("_CategoryPartial", category);
		}

		public ActionResult RemoveProduct(int id)
		{
			try
			{
				var ProductInDb = _ProductService.GetProductById(id);

				// if the Product is not found
				if (ProductInDb == null)
				{
					return HttpNotFound("Product does not exist");
				}
				_ProductService.RemoveProduct(id);

			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);

			}
			return RedirectToAction("AllProducts");

		}

		public ActionResult ListProductCategories()
		{
			return View(_ProductService.AllCategories());
		}

		public ActionResult RemoveProductCategory(int id)
		{
			var removeCategory = _ProductService.RemoveProductCategory(id);
			if (!removeCategory)
				return HttpNotFound("Category does not exist");

			_ProductService.RemoveProductCategory(id);
			return RedirectToAction("ListProductCategories");
		}

		[HttpGet]
		public ActionResult UpdateProductCategory(int id)
		{
			var categoryInDb = Mapper.Map<EditCategoryViewModel>(_ProductService.EditProductCategory(id));

			if (categoryInDb == null) return HttpNotFound("No category found");

			return PartialView("_EditCategoryPartial", categoryInDb);
		}

		[HttpPost]
		public ActionResult UpdateProductCategory(EditCategoryViewModel category)
		{

			_ProductService.UpdateProductCategory(Mapper.Map<ProductCategory>(category));
			return Json(new { response = "success" }, JsonRequestBehavior.AllowGet);

		}

		public ActionResult ViewProduct(int id)
		{
			var ProductInDb = Mapper.Map<Product, ProductViewModel>(_ProductService.EditProduct(id));

			if (ProductInDb == null) return HttpNotFound("No Product found");

			return PartialView("_ViewProductPartial", ProductInDb);

		}
	}
}