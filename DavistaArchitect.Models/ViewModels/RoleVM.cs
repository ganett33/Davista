using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DavistaArchitect.Models.ViewModels
{
    public class ControlRolesVM
    {
        public string UserId { get; set; }
        public IList<RolesVM> UserRoles { get; set; }
    }

    public class RolesVM
    {
        public string RoleId { get; set; }
        public string RoleName { get; set; }
        public bool Selected { get; set; }
    }
}
