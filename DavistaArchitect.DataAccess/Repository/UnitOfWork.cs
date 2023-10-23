using DavistaArchitect.DataAccess.Data;
using DavistaArchitect.DataAccess.Repository.IRepository;
using DavistaArchitect.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DavistaArchitect.DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private ApplicationDbContext _db;

        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            Category = new CategoryRepository(_db);
            ProjectType = new ProjectTypeRepository(_db);
            StaffProfile = new StaffProfileRepository(_db);
            House = new HouseRepository(_db);


        }

        public ICategoryRepository Category { get; private set; }
        public IProjectTypeRepository ProjectType { get; private set; }
        public IStaffProfileRepository StaffProfile { get; private set; }
        public IHouseRepository House { get; private set; }



        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
