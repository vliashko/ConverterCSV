using StarterTest.BL.Model;
using System;
using System.Collections.Generic;

namespace StarterTest.BL
{
    public interface IRepository : IDisposable
    {
        List<User> GetNecessaryUserList(User necessaryUser);
        User GetUser(int id);
        void Create(User user);
        void Delete(User user);
        void Save();
    }
}
