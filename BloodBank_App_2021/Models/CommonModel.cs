using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BloodBank_App_2021.Models
{
   public class BloodGroup
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BloodGroupId { get; set; }
        [StringLength(50, ErrorMessage = "Maximum length should be 50")]
        [Display(Name = "Blood Group")]
        [Required(ErrorMessage = "This Field is required")]
        public string BloodGroupName { get; set; }

        public virtual ICollection<BloodDonorRegistration> BloodDonorRegistration { get; set; }
        public virtual ICollection<BloodRequestUser> BloodRequestUser { get; set; }
    }


    public class Country
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CountryId { get; set; }
        [StringLength(50, ErrorMessage = "Maximum length should be 50")]
        [Display(Name = "Country Name")]
        [Required(ErrorMessage = "This Field is required")]
        public string CountryName { get; set; }

        public virtual ICollection<District> District { get; set; }
    }

    public class District
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DistrictId { get; set; }
        [StringLength(50, ErrorMessage = "Maximum length should be 50")]
        [Display(Name = "District Name")]
        [Required(ErrorMessage = "This Field is required")]
        public string DistrictName { get; set; }

        [ForeignKey("CountryId")]
        public int CountryId { get; set; }

        public virtual Country Country { get; set; }
        public virtual ICollection<Thana> Thana { get; set; }
    }

    public class Thana
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ThanaId { get; set; }
        [StringLength(50, ErrorMessage = "Maximum length should be 50")]
        [Display(Name = "Thana Name")]
        [Required(ErrorMessage = "This Field is required")]
        public string ThanaName { get; set; }

        [ForeignKey("DistrictId")]
        public int DistrictId { get; set; }

        public virtual District District { get; set; }
        public virtual ICollection<BloodDonorRegistration> BloodDonorRegistration { get; set; }


    }

    public class Gender
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int GenderId { get; set; }
        [StringLength(50, ErrorMessage = "Maximum length should be 50")]
        [Display(Name = "Gender Name")]
        [Required(ErrorMessage = "This Field is required")]
        public string GenderName { get; set; }

        public virtual ICollection<BloodDonorRegistration> BloodDonorRegistration { get; set; }
    }


    public class Religion
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ReligionId { get; set; }
        [StringLength(50, ErrorMessage = "Maximum length should be 50")]
        [Display(Name = "Religion Name")]
        [Required(ErrorMessage = "This Field is required")]
        public string ReligionName { get; set; }

        public virtual ICollection<BloodDonorRegistration> BloodDonorRegistration { get; set; }
    }



}
