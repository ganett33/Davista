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
    public class ProjectTypeRepository : Repository<ProjectType>, IProjectTypeRepository
    {
        private ApplicationDbContext _db;
        public ProjectTypeRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(ProjectType obj)
        {
            var objFromDb = _db.ProjectType.FirstOrDefault(u => u.Id == obj.Id);
            if (objFromDb != null)
            {
                objFromDb.ProjectTypeName = obj.ProjectTypeName;
                objFromDb.ProjectTypeDescription = obj.ProjectTypeDescription;
                objFromDb.CreatedDateTime = obj.CreatedDateTime;

                if (obj.ProjectTypeImage != null)
                {
                    objFromDb.ProjectTypeImage = obj.ProjectTypeImage;
                }
            }
        }
    }
}
