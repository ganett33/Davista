using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DavistaArchitect.Models.Constants
{
    public static class Permissions
    {
        public static List<string> GeneratePermissionsForModule(string module)
        {
            return new List<string>()
            {
              $"Permissions.{module}.Index",
              $"Permissions.{module}.Create",
              $"Permissions.{module}.Edit",
              $"Permissions.{module}.Upsert",
              $"Permissions.{module}.Delete",
              $"Permissions.{module}.Manage",
            };
        }

        public static class Category
        {
            public const string Index = "Permissions.Category.Index";
            public const string Create = "Permissions.Category.Create";
            public const string Edit = "Permissions.Category.Edit";
            public const string Delete = "Permissions.Category.Delete";
        }        
        public static class StaffProfile
        {
            public const string Index = "Permissions.StaffProfile.Index";
            public const string Upsert = "Permissions.StaffProfile.Upsert";
            public const string Delete = "Permissions.StaffProfile.Delete";
        }

        public static class ProjectType 
        {
            public const string Index = "Permissions.ProjectType.Index";
            public const string Create = "Permissions.ProjectType.Create";
            public const string Edit = "Permissions.ProjectType.Edit";
            public const string Delete = "Permissions.ProjectType.Delete";
        }

        public static class House
        {
            public const string Index = "Permissions.House.Index";
            public const string Upsert = "Permissions.House.Upsert";
            public const string Delete = "Permissions.House.Delete";
        }


        public static class UserRoles
        {
            public const string Index = "Permissions.UserRoles.Index";
            public const string Manage = "Permissions.UserRoles.Manage";

        }
        public static class Roles
        {
            public const string Index = "Permissions.Roles.Index";
            public const string Edit = "Permissions.Roles.Edit";
            public const string Delete = "Permissions.Roles.Delete";

        }

    }
}
