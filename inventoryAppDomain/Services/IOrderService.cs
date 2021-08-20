using inventoryAppDomain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inventoryAppDomain.Services
{
    public interface IOrderService
    {
        Order CreateOrder(Order order, string userId, string clearedBy);

        Order GetOrderById(int orderId);

        List<Order> GetOrdersForTheDay();

        List<Order> GetOrdersForTheWeek();

        List<Order> GetOrdersForTheMonth();

        decimal GetTotalRevenue();
        int GetTotalSales();
        
    }
}
