using System;
using System.Collections.Generic;
using System.Text;

namespace Hilal.DataViewModel.Request.Admin.v1
{
    public class RolesViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid CreatedBy { get; set; }
    }
    public class RightsViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public long? Grade { get; set; }
        public Guid? Fk_Self { get; set; }
        public string Logo { get; set; }
        public Guid CreatedBy { get; set; }
    }
    public class AssignRolesRightsViewModel
    {
        public Guid Id { get; set; }
        public ItemGuidViewModel FK_AdminRights_Id { get; set; }
        public ItemGuidViewModel FK_AdminRoles_Id { get; set; }
        public Guid CreatedBy { get; set; }
    }
}
