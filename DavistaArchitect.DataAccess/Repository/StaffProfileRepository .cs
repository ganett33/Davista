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
    public class StaffProfileRepository : Repository<StaffProfile>, IStaffProfileRepository
    {
        private ApplicationDbContext _db;
        public StaffProfileRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(StaffProfile obj)
        {
            var objFromDb = _db.StaffProfile.FirstOrDefault(u => u.Id == obj.Id);
            if (objFromDb != null)
            {
                objFromDb.FirstName = obj.FirstName;
                objFromDb.LastName = obj.LastName;
                objFromDb.Description = obj.Description;
                objFromDb.Email = obj.Email;
                objFromDb.StartedDate = obj.StartedDate;
                objFromDb.CategoryId = obj.CategoryId;
                if (obj.ImageUrl != null)
                {
                    objFromDb.ImageUrl = obj.ImageUrl;

                }
            }
        }
    }
}
