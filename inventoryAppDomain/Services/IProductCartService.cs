using inventoryAppDomain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using inventoryAppDomain.Entities.Enums;

namespace inventoryAppDomain.Services
{
    public interface IProductCartService
    {
        ProductCart CreateCart(string userId);
        ProductCart GetCart(string userId, CartStatus cartStatus);
        void AddToCart(Product Product, string userId, int totalAmountOfPrescribedProducts = 0, int amount = 1);
        void ClearCart(string cartId);
        List<ProductCartItem> GetProductCartItems(string userId, CartStatus cartStatus);
        int RemoveFromCart(Product Product, string userId);
   
        decimal GetProductCartSumTotal(string userId);
        int GetProductCartTotalCount(string userId);
        Product GetProductById(int id);
        Product GetProductByName(string Name);
        ProductCartItem GetProductCartItemById(int id);
        ProductCart RefreshCart(string userId);
    }
}
