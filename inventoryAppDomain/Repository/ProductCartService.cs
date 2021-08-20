using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using inventoryAppDomain.Entities;
using inventoryAppDomain.Entities.Enums;
using inventoryAppDomain.IdentityEntities;
using inventoryAppDomain.Services;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace inventoryAppDomain.Repository
{
    public class ProductCartService : IProductCartService
    {

        private readonly ApplicationDbContext _dbContext;
        private readonly ApplicationUserManager userManager;

        public ProductCartService()
        {
            _dbContext = HttpContext.Current.GetOwinContext().Get<ApplicationDbContext>();
            userManager = HttpContext.Current.GetOwinContext().Get<ApplicationUserManager>();
        }

        public ProductCart CreateCart(string userId)
        {
            var user = userManager.Users.FirstOrDefault(applicationUser => applicationUser.Id == userId);
            var cart = new ProductCart()
            {
                ApplicationUser = user,
                ApplicationUserId = userId,
                ProductCartItems = new List<ProductCartItem>(),
                CartStatus = CartStatus.ACTIVE
            };

            _dbContext.ProductCarts.Add(cart);
            _dbContext.SaveChanges();

            return cart;
        }

        public ProductCart GetCart(string userId, CartStatus cartStatus)
        {
            var user = userManager.Users.FirstOrDefault(applicationUser => applicationUser.Id == userId);

            if (user != null)
                return _dbContext.ProductCarts.Include(cart => cart.ProductCartItems)
                    .FirstOrDefault(cart => cart.ApplicationUserId == userId && cart.CartStatus == cartStatus);
            
            return null;
        }


        public void AddToCart(Product Product, string userId, int totalAmountOfPrescribedProducts = 0, int amount = 1)
        {
            var ProductCart = GetCart(userId, CartStatus.ACTIVE);
            var cartItem = _dbContext.ProductCartItems.FirstOrDefault(item => item.ProductId == Product.Id && item.ProductCartId == ProductCart.Id);
            if (cartItem == null)
            {
                cartItem = new ProductCartItem
                {
                    ProductCartId = ProductCart.Id,
                    ProductCart = ProductCart,
                    Product = Product,
                    PrescribedAmount = totalAmountOfPrescribedProducts * Product.PricePerUnit,
                    Amount = amount
                };
                _dbContext.ProductCartItems.Add(cartItem);
            }
            else
            {
                if (cartItem.Amount < cartItem.Product.Quantity)
                    cartItem.Amount++;
            }
            _dbContext.SaveChanges();
        }

        public int RemoveFromCart(Product Product, string userId)
        {
            var ProductCart = GetCart(userId, CartStatus.ACTIVE);
            var cartItem = _dbContext.ProductCartItems.FirstOrDefault(item => item.ProductId == Product.Id);
            if (cartItem != null)
            {
                if (cartItem.Amount > 1)
                {
                    cartItem.Amount--;
                }
                else
                {
                    ProductCart.ProductCartItems.Remove(cartItem);
                    _dbContext.Entry(ProductCart).State = EntityState.Modified;
                    _dbContext.ProductCartItems.Remove(cartItem);
                }
            }
            _dbContext.SaveChanges();
            if (cartItem != null) return cartItem.Amount;
            return 0;
        }

        public Product GetProductById(int Id)
        {
            var Product = _dbContext.Products.FirstOrDefault(x => x.Id == Id);
            return Product;
        }

        public Product GetProductByName(string Name)
        {
            var Product = _dbContext.Products.FirstOrDefault(x => x.ProductName == Name);
            return Product;
        }

        public ProductCartItem GetProductCartItemById(int id)
        {
            var Product = _dbContext.ProductCartItems.Include(item => item.Product).FirstOrDefault(x => x.Id == id);
            return Product;
        }

        public ProductCart RefreshCart(string userId)
        {
            var ProductCart = GetCart(userId, CartStatus.MOST_RECENT);
            var newCart = CreateCart(userId);
            ProductCart.CartStatus = CartStatus.USED;
            _dbContext.Entry(ProductCart).State = EntityState.Modified;
            _dbContext.SaveChanges();
            return newCart;
        }

        public List<ProductCartItem> GetProductCartItems(string userId, CartStatus cartStatus)
        {
            var cart = GetCart(userId,cartStatus);
            return _dbContext.ProductCartItems.Include(item => item.Product).Where(item => item.ProductCartId == cart.Id).ToList();
        }
        public void ClearCart(string userId)
        {
            var cart = GetCart(userId,CartStatus.ACTIVE);
            _dbContext.ProductCartItems.RemoveRange(cart.ProductCartItems);
            cart.ProductCartItems = new List<ProductCartItem>();
            _dbContext.Entry(cart).State = EntityState.Modified;
            _dbContext.SaveChanges();
        }
        public decimal GetProductCartSumTotal(string userId)
        {
            var cart = GetCart(userId,CartStatus.ACTIVE);
            if (cart.ProductCartItems.Count == 0)
            {
                return 0;
            }
            var sum = _dbContext.ProductCartItems.Where(c => c.ProductCartId == cart.Id)
                .Select(c => (c.Product.Price * c.Amount) + c.PrescribedAmount).Sum();
            
            return sum;
        }
        public int GetProductCartTotalCount(string userId)
        {
            var cart = GetCart(userId, CartStatus.ACTIVE);
            var total = _dbContext.ProductCartItems.Count(c => c.ProductCartId == cart.Id); ;

       
            return total;
        }
    }
}