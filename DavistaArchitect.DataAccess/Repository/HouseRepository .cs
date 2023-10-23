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
    public class HouseRepository : Repository<House>, IHouseRepository
    {
        private ApplicationDbContext _db;
        public HouseRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }


        public void Update(House obj)
        {
            var objFromDb = _db.House.FirstOrDefault(u => u.Id == obj.Id);
            if (objFromDb != null)
            {
                objFromDb.HouseName = obj.HouseName;
                objFromDb.HouseDescription = obj.HouseDescription;
                objFromDb.CategoryId = obj.CategoryId;
                objFromDb.ProjectTypeId = obj.ProjectTypeId;

                if (obj.HouseImage != null)
                {
                    objFromDb.HouseImage = obj.HouseImage;
                }
            }
        }


    }
}
