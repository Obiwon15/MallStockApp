using inventoryAppDomain.Entities;
using inventoryAppDomain.Entities.Enums;
using inventoryAppDomain.IdentityEntities;
using inventoryAppDomain.Services;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace inventoryAppDomain.Repository
{
    public class SupplierService : ISupplierService
    {
        private readonly ApplicationDbContext _dbContext;

        public SupplierService() => _dbContext = HttpContext.Current.GetOwinContext()
            .Get<ApplicationDbContext>();

        public void AddSupplier(Supplier supplier)
        {
            var newSupplier = _dbContext.Suppliers.Add(supplier);
            _dbContext.Entry(newSupplier).State = EntityState.Added;
            _dbContext.SaveChanges();
        }

        public void UpdateSupplier(Supplier supplier)
        {
            var updateSupplier = _dbContext.Suppliers.Add(supplier);
            _dbContext.Entry(updateSupplier).State = EntityState.Modified;

            _dbContext.SaveChanges();
        }

        public IEnumerable<Supplier> GetAllSuppliers() =>
            _dbContext.Suppliers
            .OrderBy(s => s.Status)
            .ToList();

        public bool ProcessSupplier(int id, SupplierStatus condition)
        {
            var supplier = FindSupplier(id);

            if (supplier == null)
                return false;
            else
            {
                supplier.Status = condition;
                _dbContext.SaveChanges();
            }
            return true;
        }


        public Supplier FindSupplier(int id) {
            var supplier = _dbContext.Suppliers.SingleOrDefault(s => s.Id == id);
            if (supplier == null)
                return null;

            return supplier;
        }
        public IEnumerable<Product> GetAllProductsBySupplier(string supplierTagNumber){
           var allProductsBySupplier = _dbContext.Products.Where(d => d.SupplierTag == supplierTagNumber).ToList();

            if (allProductsBySupplier == null)
                return null;

            return allProductsBySupplier;
        }

        private static Random random = new Random();
        public string GenerateTagNumber()
        {
            const int tagLength = 7;

            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

            var uniqueTag = "TAG-" + new string(Enumerable.Repeat(chars, tagLength)
              .Select(s => s[random.Next(s.Length)]).ToArray());

            var tagInDb = _dbContext.Suppliers.SingleOrDefault(t => t.TagNumber == uniqueTag);

            if (tagInDb != null)
                GenerateTagNumber();

            return uniqueTag;
        }

        public int TotalNumberOfSupplier() => _dbContext.Suppliers.Count();

        public Supplier GetSupplierWithTag(string tag) => _dbContext.Suppliers.SingleOrDefault(s => s.TagNumber == tag);

    }
}
