using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


namespace BloodBank_App_2021.Models
{
    public class BloodRequestUser
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BloodRequestUserId { get; set; }
        [StringLength(50, ErrorMessage = "Maximum length should be 50")]
        [Display(Name = "User Name")]
        [Required(ErrorMessage = "This Field is required")]
        public string UserName { get; set; }
        [StringLength(50, ErrorMessage = "Maximum length should be 50")]
        [Display(Name = "User Mobile")]
       
        public string UserMobile { get; set; }
        [StringLength(50, ErrorMessage = "Maximum length should be 50")]
        [Display(Name = "User Address")]
        public string UserAddress { get; set; }
        [StringLength(50, ErrorMessage = "Maximum length should be 50")]
        [Display(Name = "Reason Of Blood")]
        [Required(ErrorMessage = "This Field is required")]
        public string ReasonOfBlood { get; set; }
        [StringLength(50, ErrorMessage = "Maximum length should be 50")]
        [Display(Name = "Donate Place")]
        [Required(ErrorMessage = "This Field is required")]
        public string DonatePlace { get; set; }
        [StringLength(50, ErrorMessage = "Maximum length should be 50")]
        [Display(Name = "Amount Of Blood Requested")]
        public string AmountOfBloodRequested { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Required]
        public DateTime Date_BloodRequest { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Date_BloodNeed { get; set; }

        [ForeignKey("BloodGroupId")]
        public int BloodGroupId { get; set; }  //Patient Blood Group

        public virtual BloodGroup BloodGroup { get; set; }
    }
}
