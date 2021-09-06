using System;
using System.Collections.Generic;
using System.Text;

namespace Hilal.DataViewModel.Request.Admin.v1
{
    public class UsersViewModel
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
        public ItemGuidViewModel FK_Role_Id { get; set; }
        public bool? IsPasswordChange { get; set; }
        public Guid CreatedBy { get; set; }
    }

    public class UsersRoleViewModel
    {
        public Guid Id { get; set; }
        public ItemGuidViewModel FK_AdminUser_Id { get; set; }
        public ItemGuidViewModel FK_AdminRole_Id { get; set; }
        public Guid CreatedBy { get; set; }
    }
}
