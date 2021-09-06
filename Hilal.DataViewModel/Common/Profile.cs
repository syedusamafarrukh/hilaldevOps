using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Hilal.DataViewModel.Common
{
    public class Profile
    {
        [Required]
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; } = "";

        [Required]
        [JsonProperty(PropertyName = "email")]
        public string Email { get; set; } = "";

        [Required]
        [JsonProperty(PropertyName = "nationality")]
        public string Nationality { get; set; } = "";

        [JsonProperty(PropertyName = "tierName")]
        public string TierName { get; set; } = "";

        [JsonProperty(PropertyName = "gender")]
        public string Gender { get; set; } = "";

        [Required]
        [JsonProperty(PropertyName = "genderId")]
        public int GenderId { get; set; } = 0;

        [JsonProperty(PropertyName = "imageUrl")]
        public string ImageUrl { get; set; } = "";

        [JsonProperty(PropertyName = "imageThumbnailUrl")]
        public string ImageThumbnailUrl { get; set; } = "";

        [JsonProperty(PropertyName = "expiryDate")]
        public DateTime? ExpiryDate { get; set; }

        [Required]
        [JsonProperty(PropertyName = "dateOfBirth")]
        public DateTime? DateOfBirth { get; set; }

        [Required]
        [JsonProperty(PropertyName = "phoneNumber")]
        public Phone PhoneNumber { get; set; } = new Phone();

    }
}
