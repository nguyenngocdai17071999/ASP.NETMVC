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
    public class UserDao
    {
        DoAnWEB db = null;
        public UserDao()
        {   
            db = new DoAnWEB();
        }
        public long Insert(User entity)
        {          
                db.Users.Add(entity);
                db.SaveChanges();
                return entity.UserID;                      
        }
        public long InsertForFacebook(User entity)
        {
            var user = db.Users.SingleOrDefault(x => x.UserName == entity.UserName);
            if(user==null)
            {
                db.Users.Add(entity);
                db.SaveChanges();
                return entity.UserID;
            }
            else
            {
                return user.UserID;
            }
        }
        public IEnumerable<User> GetAll()
        {
            return db.Users.Where(x => x.Status == true);
        }
        public IEnumerable<User> GetUser(string gmail)
        {          
            return db.Users.Where(x => x.Email == gmail);
        }
        public IEnumerable<User> ListAllPaging(int page,int pageSize,string searchString)
        {
            IQueryable<User> model = db.Users.OrderByDescending(x => x.CreatedDate);
            if(!string.IsNullOrEmpty(searchString))
            {
                model = model.Where(x => x.FirstName.Contains(searchString) || x.LastName.Contains(searchString));
            }
            return model.OrderByDescending(x => x.CreatedDate).ToPagedList(page, pageSize);
        }
        public User GetByID(string userName)
        {
            return db.Users.SingleOrDefault(x=>x.UserName==userName);
        }
        public User GetByEmail(string Email)
        {
            return db.Users.SingleOrDefault(x => x.Email == Email);
        }
        public User ViewDetail(long id)
        {
            return db.Users.Find(id);
        }
        public int Login(string username,string password)
        {
            var result = db.Users.SingleOrDefault(x => x.UserName == username);
           
            if(result ==null)
            {
                return 0;
            }
            else
            {
                if(result.Status ==false)
                {
                    return -1;
                }
                else
                {
                    if (result.Passwords == password)
                    {
                        if(result.Position=="2" ||result.Position=="3")
                        {
                            return 2;
                        }
                        else
                        {
                            return 1;
                        }
                    }                      
                    else
                        return -2;
                }
            }
            
        }
        
        public bool CheckUserName(string username)
        {
            return db.Users.Count(x => x.UserName == username) > 0;
        }
        public bool CheckUserEmail(string email)
        {
            return db.Users.Count(x => x.Email == email) > 0;
        }

        public int ChangePass(long id, string newpass)
        {
            var result = db.Users.SingleOrDefault(x => x.UserID == id);

            if (result.Passwords == newpass)
            {
                return 1;//pass trùng 
            }
            else
            {
                result.Passwords = newpass;// doi pass thanh cong
                db.SaveChanges();
                return 2;
            }
        
            
        }
        public int doimatkhau(long id, string newpass, string code)
        {
            var result = db.Users.SingleOrDefault(x => x.UserID == id);
            if (result.CodeChangePass == code)
            {
                if (result.Passwords == newpass)
                {
                    return 1;//pass trùng 
                }
                else
                {
                    result.Passwords = newpass;// doi pass thanh cong
                    db.SaveChanges();
                    return 2;
                }
            }
            else
            {
                return 3;
            }
        }

        public bool Update(User entity)
        {
            try
            {
                var user = db.Users.Find(entity.UserID);
                user.FirstName = entity.FirstName;
                user.LastName = entity.LastName;
                if (!string.IsNullOrEmpty(entity.Passwords))
                {
                    user.Passwords = entity.Passwords;
                }
                user.Avatar = entity.Avatar;
                user.Email = entity.Email;
                user.Phone = entity.Phone;
                user.Address = entity.Address;
                user.CMND = entity.CMND;
                user.Position = entity.Position;
                user.ModifiedBy = entity.ModifiedBy;
                user.ModifiedDate = DateTime.Now;
                user.CodeChangePass = entity.CodeChangePass;
                
                db.SaveChanges();
                return true;
            }
            catch(Exception)
            {
                return false;
            }

        }
        public List<string> GetListCredential(string userName)
        {
            var user = db.Users.Single(x => x.UserName == userName);
            var data = (from a in db.Credentials
                        join b in db.UserGroups on a.UserGroupID equals b.ID
                        join c in db.Roles on a.RoleID equals c.ID
                        where b.ID == user.Position
                        select new
                        {
                            RoleID = a.RoleID,
                            UserGroupID = a.UserGroupID
                        }).AsEnumerable().Select(x => new Credential()
                        {
                            RoleID = x.RoleID,
                            UserGroupID = x.UserGroupID
                        });
            return data.Select(x => x.RoleID).ToList();

        }
        public bool ChangeStatus(long id)
        {
            var user = db.Users.Find(id);
            user.Status = !user.Status;
            db.SaveChanges();
            return user.Status;
        }
        public bool Delete(int id)
        {
            try
            {
              
                var user = db.Users.Find(id);
                db.Users.Remove(user);
                db.SaveChanges();
                return true;
            }catch(Exception)
            {
                return false;
            }
            
        }
        public bool UpdateDetail(User entity)
        {
            try
            {
                var user = db.Users.Find(entity.UserID);
                user.FirstName = entity.FirstName;
                user.LastName = entity.LastName;               
                user.Avatar = entity.Avatar;
                user.Email = entity.Email;
                user.Phone = entity.Phone;
                user.Address = entity.Address;
                user.CMND = entity.CMND;           
                user.ModifiedBy = entity.ModifiedBy;
                user.ModifiedDate = DateTime.Now;

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
