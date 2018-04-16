using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Case.Study.Api.Models
{
    public interface IUserRepository : IDisposable
    {
        IEnumerable<User> GetUsers();
        User GetUserByID(int UserID_);
        User GetUserByEmailOrUserName(string Email_,string UserName_);
        void InsertUser(User User_);
        void DeleteUser(int userID_);
        void UpdateUser(User User_);
        void Save();
    }
}