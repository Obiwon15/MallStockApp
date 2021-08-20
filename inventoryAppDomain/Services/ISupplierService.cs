using inventoryAppDomain.Entities;
using inventoryAppDomain.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inventoryAppDomain.Services
{
    public interface ISupplierService
    {
        IEnumerable<Supplier> GetAllSuppliers();
        void AddSupplier(Supplier supplier);
        bool ProcessSupplier(int id, SupplierStatus condition);

        Supplier FindSupplier(int id);

        string GenerateTagNumber();

        int TotalNumberOfSupplier();

        Supplier GetSupplierWithTag(string tag);

        void UpdateSupplier(Supplier supplier);

        IEnumerable<Product> GetAllProductsBySupplier(string tagNumber);
    }
}
