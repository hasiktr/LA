using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Case.Study.Api.Models
{
    public class UserRepository : IUserRepository, IDisposable
    {
        protected MyTestDBEntities _DBContext = null;

        public UserRepository(MyTestDBEntities Context_)
        {
            this._DBContext = Context_;
        }

        public void DeleteUser(int userID_)
        {
            var user_ = _DBContext.Users.Find(userID_);
            _DBContext.Users.Remove(user_);
        }

        public User GetUserByID(int UserID_)
        {
            return _DBContext.Users.Find(UserID_);
        }

        public IEnumerable<User> GetUsers()
        {
            return _DBContext.Users.ToList();
        }

        public void InsertUser(User User_)
        {
            _DBContext.Users.Add(User_);
        }

        public void Save()
        {
            _DBContext.SaveChanges();
        }

        public void UpdateUser(User User_)
        {
            _DBContext.Entry(User_).State = EntityState.Modified;
        }

        #region Dispose

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _DBContext.Dispose();
                }
            }
            this.disposed = true;
        }
         
        public User GetUserByEmailOrUserName(string Email_, string UserName_)
        {
            return _DBContext.Users.Where(p => p.User_Email == Email_ || p.User_UserName == UserName_).FirstOrDefault();
        }

        #endregion


    }
}