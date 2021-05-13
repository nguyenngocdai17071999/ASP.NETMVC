using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Models.Framwork;
using System.Threading.Tasks;
using PagedList;

namespace Models.Dao
{
    public class ContentDao
    {
        DoAnWEB db = null;
        public ContentDao()
        {
            db = new DoAnWEB();
        }
        public IEnumerable<Content> ListAllPaging(int page, int pageSize, string searchString)
        {
            IQueryable<Content> model = db.Contents.OrderByDescending(x => x.CreatedDate);
            if (!string.IsNullOrEmpty(searchString))
            {
                model = model.Where(x => x.Name.Contains(searchString));
            }
            return model.OrderByDescending(x => x.CreatedDate).ToPagedList(page, pageSize);
        }
        public Content ViewDetail(long id)
        {
            return db.Contents.Find(id);
        }
        public bool update(Content entity)
        {
            try
            {
                var content = db.Contents.Find(entity.ID);
                content.Name = entity.Name;
                content.MetaTitle = entity.MetaTitle;
                content.Description = entity.Description;
                content.Image = entity.Image;         
                content.Language = entity.Language;
                content.CategoryID = entity.CategoryID;
                content.Detail = entity.Detail;
                content.Warranty = entity.Warranty;
                content.ModifiedDate = entity.ModifiedDate;
                content.ModifiedBy = entity.ModifiedBy;
                content.MetaKeywords = entity.MetaKeywords;
                content.MetaDescriptions = entity.MetaDescriptions;
                content.Status = entity.Status;
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
                var content = db.Contents.Find(ID);
                db.Contents.Remove(content);
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public bool ChangeStatus(long id)
        {
            var pro = db.Contents.Find(id);
            pro.Status = !pro.Status;
            db.SaveChanges();
            return pro.Status;
        }
        public long Insert(Content entity)
        {
            db.Contents.Add(entity);
            db.SaveChanges();
            return entity.ID;
        }
        public bool CheckContent(string name)
        {
            return db.Contents.Count(x => x.Name == name) > 0;
        }
    }
}
