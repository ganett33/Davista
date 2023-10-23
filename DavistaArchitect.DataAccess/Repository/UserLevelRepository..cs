using DavistaArchitect.DataAccess.Data;
using DavistaArchitect.DataAccess.Repository.IRepository;
using DavistaArchitect.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DavistaArchitect.DataAccess.Repository
{
    public class UserLevelRepository : Repository<UserLevel>, IUserLevelRepository
    {
        private ApplicationDbContext _db;
        public UserLevelRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(UserLevel obj)
        {
            _db.UserLevel.Update(obj);
        }
    }
}
