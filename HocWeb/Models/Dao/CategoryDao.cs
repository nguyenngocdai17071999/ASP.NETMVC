using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Models.Framwork;
using System.Threading.Tasks;
using PagedList;

namespace Models.Dao
{
    public class CategoryDao
    {
        DoAnWEB db = null;
        public CategoryDao()
        {
            db = new DoAnWEB();
        }
        public bool update(Category entity)
        {
            try
            {
                var category = db.Categories.Find(entity.ID);
                category.Name = entity.Name;
                category.MetaTitle = entity.MetaTitle;
                category.DisplayOrder = entity.DisplayOrder;              
                category.ModifiedDate = entity.ModifiedDate;
                category.ModifiedBy = entity.ModifiedBy;              
                category.MetaKeywords = entity.MetaKeywords;
                category.MetaDescriptions = entity.MetaDescriptions;
                category.Status = entity.Status;
                category.ShowOnHome = entity.ShowOnHome;
                category.Language = entity.Language;
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public List<Category> ListAll()
        {
            return db.Categories.Where(x => x.Status == true).ToList();
        }
        public IEnumerable<Category> ListAllPaging(int page, int pageSize, string searchString)
        {
            IQueryable<Category> model = db.Categories.OrderByDescending(x => x.CreatedDate);
            if (!string.IsNullOrEmpty(searchString))
            {
                model = model.Where(x => x.Name.Contains(searchString));
            }
            return model.OrderByDescending(x => x.CreatedDate).ToPagedList(page, pageSize);
        }
        public bool ChangeStatus(long id)
        {
            var category = db.Categories.Find(id);
            category.Status = !category.Status;
            db.SaveChanges();
            return category.Status;
        }
        public bool ChangeShowOnHome(long id)
        {
            var category = db.Categories.Find(id);
            category.ShowOnHome = !category.ShowOnHome;
            db.SaveChanges();
            return category.ShowOnHome;
        }
        public bool Delete(long ID)
        {
            try
            {
                var category = db.Categories.Find(ID);
                db.Categories.Remove(category);
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public long Insert(Category entity)
        {
            db.Categories.Add(entity);
            db.SaveChanges();
            return entity.ID;
        }
        public Category ViewDetail(long id)
        {
            return db.Categories.Find(id);
        }
    }
}
