using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BloodBank_App_2021.ViewModels
{
    public class VmParams
    {
        public string DonorContactNumber { get; set; }
        public int BloodGroupId { get; set; }
        public string BloodGroupName { get; set; }
        public int DistrictId { get; set; }
        public string DistrictName { get; set; }
        public int ThanaId { get; set; }
        public string ThanaName { get; set; }
        public int GenderId { get; set; }
        public string GenderName { get; set; }
        public int ReligionId { get; set; }
        public string ReligionName { get; set; }
        public string NIDorBirthId { get; set; }
        public string DonorName { get; set; }
        public string Address { get; set; }

    }
}
