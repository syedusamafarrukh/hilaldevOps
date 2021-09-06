using Hilal.DataViewModel.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Hilal.DataViewModel.Request.Admin.v1
{
    public class CreateUserRequest
    {
        [JsonProperty(PropertyName = "id")]
        public Guid? Id { get; set; }

        [Required]
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [Required]
        [JsonProperty(PropertyName = "email")]
        public string Email { get; set; }

        [Required]
        [JsonProperty(PropertyName = "password")]
        public string Password { get; set; }

        [Required]
        [JsonProperty(PropertyName = "countryCode")]
        public string CountryCode { get; set; }

        [Required]
        [JsonProperty(PropertyName = "phoneNumber")]
        public string PhoneNumber { get; set; }

        [Required]
        [JsonProperty(PropertyName = "genderId")]
        public int GenderId { get; set; }

        [JsonProperty(PropertyName = "genderName")]
        public string GenderName { get; set; }

        [Required]
        [JsonProperty(PropertyName = "designation")]
        public string Designation { get; set; }

        [JsonProperty(PropertyName = "dateOfBirth")]
        public DateTime? DateOfBirth { get; set; }

        [Required]
        [JsonProperty(PropertyName = "roleId")]
        public Guid RoleId { get; set; }

        [JsonProperty(PropertyName = "roleName")]
        public string RoleName { get; set; }

        [JsonProperty(PropertyName = "profileImage")]
        public FileUrlResponce ProfileImage { get; set; } = new FileUrlResponce();
    }
}
