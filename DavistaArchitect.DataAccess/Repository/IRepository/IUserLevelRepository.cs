﻿using DavistaArchitect.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DavistaArchitect.DataAccess.Repository.IRepository
{
    public interface IUserLevelRepository : IRepository<UserLevel>
    {
        void Update(UserLevel obj);

    }
}
