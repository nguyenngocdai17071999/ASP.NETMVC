using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Models.Framwork;
using System.Threading.Tasks;

namespace Models.Dao
{
    public class UserGroupDao
    {
        DoAnWEB db = null;
        public UserGroupDao()
        {
            db = new DoAnWEB();
        }

        public List<string> ListAll()
        {
            return db.UserGroups.Select(x => x.ID).ToList();
        }

      
    }
}
