using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BloodBank_App_2021.ViewModels
{
    public class VmModelData
    {

        //donor registration record
        public string DonorContactNumber { get; set; }
        public string DonorName { get; set; }
        public string FatherName { get; set; }
        public string Address { get; set; }
        public string Age { get; set; }
        public double Height { get; set; }
        public double Weight { get; set; }
        public string NIDorBirthId { get; set; }
        public DateTime BirthDate { get; set; }
        public string Email { get; set; }
        public string Profession { get; set; }
        public string LastWorkPlace { get; set; }
        public bool MajorPhysicalProblem { get; set; }
        public bool Smocker { get; set; }
        public bool DrugAddicted { get; set; }
        public bool BloodDonateExperience { get; set; }
        public bool MajorOperation { get; set; }

        //donation record
        public int BloodDonationRecordId { get; set; }
        public string BloodReceivedBy { get; set; }
        public double AmountOfBlood { get; set; }
        public DateTime DonateDate { get; set; }
        public string DonatePlace { get; set; }
        public string ReasonOfDonation { get; set; }
       // public string DonorContactNumber { get; set; }



        //Blood Request User Record
        public int BloodRequestUserId { get; set; }
        public string UserName { get; set; }
        public string UserMobile { get; set; }
        public string UserAddress { get; set; }
        public string ReasonOfBlood { get; set; }
        public string AmountOfBloodRequested { get; set; }
        public DateTime Date_BloodRequest { get; set; }
        public DateTime Date_BloodNeed { get; set; }


        //Common Table 
        public int BloodGroupId { get; set; }
        public string BloodGroupName { get; set; }
        public int GenderId { get; set; }
        public string GenderName { get; set; }
        public int ReligionId { get; set; }
        public string ReligionName { get; set; }
        public int CountryId { get; set; }
        public string CountryName { get; set; }
        public int DistrictId { get; set; }
        public string DistrictName { get; set; }
        public int ThanaId { get; set; }
        public string ThanaName { get; set; }

        //other
        public int TotalDays { get; set; }



    }
}
