using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Models.Framwork;
using System.Threading.Tasks;
using PagedList;

namespace Models.Dao
{
    public class CateProductDao
    {
        DoAnWEB db = null;
        public CateProductDao()
        {
            db = new DoAnWEB();
        }

        public List<ProductCategory> ListAll()
        {
            return db.ProductCategories.Where(x => x.Status == true).ToList();
        }
        public IEnumerable<ProductCategory> ListAllPaging(int page, int pageSize, string searchString)
        {
            IQueryable<ProductCategory> model = db.ProductCategories.OrderByDescending(x => x.CreatedDate);
            if (!string.IsNullOrEmpty(searchString))
            {
                model = model.Where(x => x.Name.Contains(searchString));
            }
            return model.OrderByDescending(x => x.CreatedDate).ToPagedList(page, pageSize);
        }
        public bool ChangeStatus(long id)
        {
            var category = db.ProductCategories.Find(id);
            category.Status = !category.Status;
            db.SaveChanges();
            return category.Status;
        }
        public bool ChangeShowOnHome(long id)
        {
            var category = db.ProductCategories.Find(id);
            category.ShowOnHome = !category.ShowOnHome;
            db.SaveChanges();
            return category.ShowOnHome;
        }
        public long Insert(ProductCategory entity)
        {
            db.ProductCategories.Add(entity);
            db.SaveChanges();
            return entity.ID;
        }
        public ProductCategory ViewDetail(long id)
        {
            return db.ProductCategories.Find(id);
        }
        public bool update(ProductCategory entity)
        {
            try
            {
                var category = db.ProductCategories.Find(entity.ID);
                category.Name = entity.Name;
                category.MetaTitle = entity.MetaTitle;               
                category.DisplayOrder = entity.DisplayOrder;
                category.ModifiedDate = entity.ModifiedDate;
                category.ModifiedBy = entity.ModifiedBy;
                category.MetaKeywords = entity.MetaKeywords;
                category.MetaDescriptions = entity.MetaDescriptions;
                category.Status = entity.Status;
                category.ShowOnHome = entity.ShowOnHome;
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public bool Delete(long ID)
        {
            try
            {
                var category = db.ProductCategories.Find(ID);
                db.ProductCategories.Remove(category);
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public bool CheckProduct(string name)
        {
            return db.ProductCategories.Count(x => x.Name == name) > 0;
        }
    }
}
