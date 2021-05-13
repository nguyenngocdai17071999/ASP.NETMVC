using System;
using System.Collections.Generic;
using System.Linq;
using Models.Framwork;
using System.Text;
using System.Threading.Tasks;

namespace Models.Dao
{
    public class LanguageDao
    {
        DoAnWEB db = null;
         public LanguageDao()
        {
            db = new DoAnWEB();
        }
        public List<Language> ListAll()
        {
            return db.Languages.Where(x => x.IsDefault == true).ToList();
        }
        public List<string> GetListLang()
        {        
            var data = (from a in db.Languages                                             
                        select new
                        {
                            name = a.Name
                        });
            return data.Select(x => x.name).ToList();

        }
    }
}
