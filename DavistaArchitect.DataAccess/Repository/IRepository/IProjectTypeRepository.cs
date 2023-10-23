using DavistaArchitect.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DavistaArchitect.DataAccess.Repository.IRepository
{
    public interface IProjectTypeRepository : IRepository<ProjectType>
    {
        void Update(ProjectType obj);

    }
}
