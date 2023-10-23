using DavistaArchitect.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DavistaArchitect.DataAccess.Repository.IRepository
{
    public interface IUnitOfWork
    {
        ICategoryRepository Category { get; }
        IProjectTypeRepository ProjectType { get; }
        IStaffProfileRepository StaffProfile { get; }
        IHouseRepository House { get; }
        // IUserLevelRepository UserLevel { get; }


        void Save();
    }
}
