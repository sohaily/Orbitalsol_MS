using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace IdentityServer.Api.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTimeOffset? BirthDay { get; set; }
        public DateTimeOffset CreatedAt { get; set; }   

        public string Gender { get; set; }
        public string MaritalStatus { get; set; }
        public string Website { get; set; }
        public string AboutMe { get; set; }
                   
        
        public string Currency { get; set; }        
        
       
            
        public string UserAgent { get; set; }    
        
        public string LanguageCode { get; set; }
        

        public string Country { get; set; }    
        public string City { get; set; }
        public double Latitude { get; set; }
        public string LocationName { get; set; }
        public double Longitude { get; set; }
        public string TwoLetterCountryCode { get; set; }

        public string Pin { get; set; }
        public bool IsSubscribed { get; set; } = false;
        
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ChatId { get; set; }

        public string PhoneCode { get; set; }

        public bool IsHideboxSectionHidden { get; set; } = true;
        public int _status { get; set; } = 1;
    
    }
}
