using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BloodBank_App_2021.Models
{
    public class BloodDonorRegistration
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [StringLength(20, ErrorMessage = "Maximum length should be 20")]
        [Display(Name = "Contact Number")]
        [Required(ErrorMessage = "This Field is required")]
        public string DonorContactNumber { get; set; }
        [StringLength(50, ErrorMessage = "Maximum length should be 50")]
        [Display(Name = "Donor Name")]
        [Required(ErrorMessage = "This Field is required")]
        public string DonorName { get; set; }
        [StringLength(50, ErrorMessage = "Maximum length should be 50")]
        [Display(Name = "Father Name")]
        [Required(ErrorMessage = "This Field is required")]
        public string FatherName { get; set; }
        [StringLength(50, ErrorMessage = "Maximum length should be 50")]
        public string Address { get; set; }
        [StringLength(100, ErrorMessage = "Maximum length should be 100")]
        public string Age { get; set; }
        public double Height { get; set; }
        public double Weight { get; set; }
        [StringLength(50, ErrorMessage = "Maximum length should be 50")]
        [Display(Name = "National Id")]
        public string NIDorBirthId { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime BirthDate { get; set; }
        [StringLength(50, ErrorMessage = "Maximum length should be 50")]
        [Display(Name = "Email")]
        public string Email { get; set; }
        [StringLength(50, ErrorMessage = "Maximum length should be 50")]
        public string Profession { get; set; }
        [StringLength(50, ErrorMessage = "Maximum length should be 50")]
        [Display(Name = "Last Work Place")]
        public string LastWorkPlace { get; set; }
        public bool MajorPhysicalProblem { get; set; }
        public bool Smocker { get; set; }
        public bool DrugAddicted { get; set; }
        public bool BloodDonateExperience { get; set; }
        public bool MajorOperation { get; set; }

        [ForeignKey("ThanaId")]
        public int ThanaId { get; set; }
        [ForeignKey("GenderId")]
        public int GenderId { get; set; }
        [ForeignKey("ReligionId")]
        public int ReligionId { get; set; }
        [ForeignKey("BloodGroupId")]
        public int BloodGroupId { get; set; }

        public virtual  Thana Thana { get; set; }
        public virtual  Religion Religion { get; set; }
        public virtual  Gender Gender { get; set; }
        public virtual  BloodGroup BloodGroup { get; set; } 
        public virtual DonorPicture DonorPicture { get; set; } 
        public virtual ICollection<BloodDonationRecord> BloodDonationRecord { get; set; }

    }



    public class DonorPicture
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DonorPictureId { get; set; }
        public string ImageUrl { get; set; }
        public DateTime? UploadDate { get; set; }
        [ForeignKey("DonorContactNumber")]
        [StringLength(20, ErrorMessage = "Maximum length should be 20")]
        [Display(Name = "Contact Number")]
        [Required(ErrorMessage = "This Field is required")]
        public string DonorContactNumber { get; set; }  //Donor Id

        public virtual BloodDonorRegistration BloodDonorRegistration { get; set; }


    }
}
