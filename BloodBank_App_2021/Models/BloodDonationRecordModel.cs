using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BloodBank_App_2021.Models
{
    public class BloodDonationRecord
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BloodDonationRecordId { get; set; }
        [StringLength(50, ErrorMessage = "Maximum length should be 50")]
        [Display(Name = "Blood Received By")]
        [Required(ErrorMessage = "This Field is required")]
        public string BloodReceivedBy { get; set; }
        [Required]
        [Display(Name = "Amount Of Blood")]
        public double AmountOfBlood { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Required]
        public DateTime DonateDate { get; set; }
        [StringLength(50, ErrorMessage = "Maximum length should be 50")]
        [Display(Name = "Donate Place")]
        public string DonatePlace { get; set; }
        [StringLength(50, ErrorMessage = "Maximum length should be 50")]
        [Display(Name = "Reason Of Donation")]
        public string ReasonOfDonation { get; set; }

        [ForeignKey("DonorContactNumber")]
        [StringLength(20, ErrorMessage = "Maximum length should be 20")]
        [Display(Name = "Contact Number")]
        [Required(ErrorMessage = "This Field is required")]
        public string DonorContactNumber { get; set; }

        public virtual BloodDonorRegistration BloodDonorRegistration { get; set; }

     

    }
}
