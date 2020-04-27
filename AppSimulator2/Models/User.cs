using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppSimulator2.Models
{
    public class User
    {
        public enum eGender : int
        {
            Male = 1,
            Female = 2,
            Both = 3
        }
        [JsonIgnore]
        public Guid ID { get; set; }
        public string LoginName { get; set; }
        public string FullName { get; set; }
        public eGender Gender { get; set; }
        public DateTime BirthDay { get; set; }
        public eGender GenderPreference { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public int AgePreferenceMax { get; set; }
        public int AgePreferenceMin { get; set; }
        public int SearchRadiusKM { get; set; }

        [Display(Name = "Created Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime CreatedDate { get; set; }
        public HashSet<User> UserContacts { get; set; } = new HashSet<User>();

    }
}
