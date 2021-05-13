using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.Framwork;

namespace Models.Dao
{
    public class ContactDao
    {
        DoAnWEB db = null;
        public ContactDao()
        {
            db = new DoAnWEB();
        }

        public Contact GetActiveContact()
        {
            return db.Contacts.Single(x => x.Status == true);
        }

        public long InsertFeedBack(Feedback fb)
        {
            db.Feedbacks.Add(fb);
            db.SaveChanges();
            return fb.ID;
        }
        public void InsertFeedBack_FB(Feedback fb)
        {
            db.Feedbacks.Add(fb);
            db.SaveChanges();
        }
    }
}
