using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.Framwork;

namespace Models.Dao
{
    public class OrderDao_client
    {
        DoAnWEB db = null;
        public OrderDao_client()
        {
            db = new DoAnWEB();
        }
        public long Insert(Order order)
        {
            db.Orders.Add(order);
            db.SaveChanges();
            return order.ID;
        }
    }
}
