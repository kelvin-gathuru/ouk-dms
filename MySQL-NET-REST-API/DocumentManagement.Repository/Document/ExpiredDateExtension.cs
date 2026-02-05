using DocumentManagement.Data.Entities;
using System;
using System.Linq;

namespace DocumentManagement.Repository
{
    public static class ExpiredDateExtension
    {
        public static DateTime? GetDoucmentExpiredDate(
           this Document document)
        {
            if (document.DocumentUserPermissions != null && document.DocumentRolePermissions != null && document.DocumentUserPermissions.Count > 0 && document.DocumentRolePermissions.Count > 0)
            {
                var roleMaxEndDate = document.DocumentRolePermissions.Max(c => c.EndDate);
                var userMaxEndDate = document.DocumentUserPermissions.Max(c => c.EndDate);
                return roleMaxEndDate > userMaxEndDate ? roleMaxEndDate : userMaxEndDate;
            }
            else if (document.DocumentUserPermissions != null && document.DocumentUserPermissions.Count > 0)
            {
                return document.DocumentUserPermissions.Max(c => c.EndDate);
            }
            else if (document.DocumentRolePermissions != null && document.DocumentRolePermissions.Count > 0)
            {
                return document.DocumentRolePermissions.Max(c => c.EndDate);
            }
            else
            {
                return null;
            }
        }
    }
}
