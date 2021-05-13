using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.Framwork;

namespace Models.Dao
{
    public class OrderDetailDao_client
    {
        DoAnWEB db = null;
        public OrderDetailDao_client()
        {
            db = new DoAnWEB();
        }
        public bool Insert(OrderDetail detail)
        {
            try
            {
                db.OrderDetails.Add(detail);
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;

            }
        }
    }
}
