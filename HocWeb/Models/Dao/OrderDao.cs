using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Models.Framwork;

using PagedList;

namespace Models.Dao
{
    public class OrderDao
    {
        DoAnWEB db = null;
        public OrderDao()
        {
            db = new DoAnWEB();
        }
        public IEnumerable<Order> ListAllPaging(int page, int pageSize, string searchString)
        {
            IQueryable<Order> model = db.Orders.OrderByDescending(x => x.CreatedDate);
            if (!string.IsNullOrEmpty(searchString))
            {
                model = model.Where(x => x.ShipName.Contains(searchString));
            }
            return model.OrderByDescending(x => x.CreatedDate).ToPagedList(page, pageSize);
        }
        public List<Order> GetListOrder()
        {
            return db.Orders.ToList();
        }
        public List<OrderDetail> GetListDetail()
        {
            return db.OrderDetails.ToList();
        }
        public Order ViewDetail(long id)
        {
            return db.Orders.Find(id);
        }     

        public bool hasOrder(long id)
        {
            var result = db.Orders.Where(x => x.CustomerID == id);
            if (result == null)
            {
                return false;
            }
            else
            {
                return true;
            }                   
        }
        public bool hasOrderDetail(long id)
        {
            var result = db.OrderDetails.Where(x => x.ProductID == id);
            if (result == null)
                return false;
            return true;
        }
        public List<OrderDetail> GetAll(long id)
        {
            return db.OrderDetails.Where(x => x.OrderID == id).ToList();
        }
        public bool update(Order entity)
        {
            try
            {
                var order = db.Orders.Find(entity.ID);
                order.Status = entity.Status;
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
