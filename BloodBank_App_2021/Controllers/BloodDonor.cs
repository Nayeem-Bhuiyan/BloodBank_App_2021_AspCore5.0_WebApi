using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BloodBank_App_2021.Models;
using BloodBank_App_2021.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace BloodBank_App_2021.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class BloodDonor : ControllerBase
    {
        public readonly BloodAppDbContext db;

        public BloodDonor(BloodAppDbContext _db)
        {
            this.db = _db;
        }


        //GET:api/BloodDonor/GetDonorList
        [HttpGet]
        [ActionName("GetDonorList")]
        public async Task<ActionResult<IEnumerable<VmModelData>>> GetDonorList(){

            List<VmModelData> VmList = new List<VmModelData>();

            var responseDataList = await db.BloodDonorRegistration.Include(th=>th.Thana).Include(g=>g.Gender).Include(r=>r.Religion).Include(b=>b.BloodGroup).ToListAsync();

            foreach (var data in responseDataList)
            {
                
                var thana = await db.Thana.Where(d => d.ThanaId == data.ThanaId).Select(s=>new {s.ThanaId,s.ThanaName,s.DistrictId}).FirstOrDefaultAsync();
                var district = await db.District.Where(d => d.DistrictId == thana.DistrictId).Select(s=>new {s.DistrictId,s.DistrictName,s.CountryId}).FirstOrDefaultAsync();
                var country = await db.Country.Where(d => d.CountryId == district.CountryId).Select(s=>new {s.CountryId,s.CountryName }).FirstOrDefaultAsync();

                VmModelData vm = new VmModelData()
                {
                     DonorContactNumber =data.DonorContactNumber,
                     DonorName =data.DonorName,
                     FatherName =data.FatherName,
                     Address =data.Address,
                     Age =data.Age,
                     Height =data.Height,
                     Weight =data.Weight,
                     NIDorBirthId =data.NIDorBirthId,
                     BirthDate =data.BirthDate,
                     Email =data.Email,
                     Profession =data.Profession,
                     LastWorkPlace =data.LastWorkPlace,
                     MajorPhysicalProblem =data.MajorPhysicalProblem,
                     Smocker =data.Smocker,
                     DrugAddicted =data.DrugAddicted,
                     BloodDonateExperience =data.BloodDonateExperience,
                     MajorOperation =data.MajorOperation,
                     ThanaName=data.Thana.ThanaName,
                     DistrictName=district.DistrictName,
                     CountryName=country.CountryName,
                     GenderName=data.Gender.GenderName,
                     ReligionName=data.Religion.ReligionName,
                     BloodGroupName=data.BloodGroup.BloodGroupName

                };
                VmList.Add(vm);
            };

            return VmList;
        }

        //GET:api/BloodDonor/GetDonorByContactNumber
        [HttpGet]
        [ActionName("GetDonorByContactNumber")]

        public async Task<ActionResult<BloodDonorRegistration>> GetDonorByContactNumber(VmParams vm)
        {
            BloodDonorRegistration donor = new BloodDonorRegistration();
            
            var data= await db.BloodDonorRegistration.Where(d => d.DonorContactNumber == vm.DonorContactNumber).FirstOrDefaultAsync();

            if (data!=null)
            {
                BloodDonorRegistration bloodDonor = new BloodDonorRegistration()
                {
                    DonorContactNumber = data.DonorContactNumber,
                    DonorName = data.DonorName,
                    FatherName = data.FatherName,
                    Address = data.Address,
                    Age = data.Age,
                    Height = data.Height,
                    Weight = data.Weight,
                    NIDorBirthId = data.NIDorBirthId,
                    BirthDate = data.BirthDate,
                    Email = data.Email,
                    Profession = data.Profession,
                    LastWorkPlace = data.LastWorkPlace,
                    MajorPhysicalProblem = data.MajorPhysicalProblem,
                    Smocker = data.Smocker,
                    DrugAddicted = data.DrugAddicted,
                    BloodDonateExperience = data.BloodDonateExperience,
                    MajorOperation = data.MajorOperation,

                    //All ForeignKey, Selected In DropDown

                    ThanaId = data.ThanaId,
                    GenderId = data.GenderId,
                    ReligionId = data.ReligionId,
                    BloodGroupId = data.BloodGroupId
                };

                donor = bloodDonor;
            }

            return donor;
        }





        //POST:api/BloodDonor/InsertOrUpdateDonor
        [HttpPost]
        [ActionName("InsertOrUpdateDonor")]
        public async Task<ActionResult<VmResponseMessage>> InsertOrUpdateDonor([FromBody] BloodDonorRegistration donor)
        {
            VmResponseMessage msg = new VmResponseMessage();
           
            int response = 0;
            var CheckRecord= await db.BloodDonorRegistration.FindAsync(donor.DonorContactNumber);

            donor.Age = CalculateAge(donor.BirthDate);

            if (CheckRecord == null)
            {
                //Insert Data

                db.BloodDonorRegistration.Add(donor);
                response= await db.SaveChangesAsync();

                if (response>0)
                {
                    msg.SuccessMessage = "Inserted Data Successfully!!";
                }
                else
                {
                    msg.ErrorMessage = "Sorry Data can not Inserted";
                }

            }
            else if(CheckRecord==null)
            {
                //Update Data
                db.Entry(donor).State = EntityState.Modified;
                response= await db.SaveChangesAsync();

                if (response > 0)
                {
                    msg.SuccessMessage = "Updated Data Successfully!!";
                }
                else
                {
                    msg.ErrorMessage = "Sorry Data can not Updated";
                }
            }

            return msg;
        }


        private string CalculateAge(DateTime dateOfBirth)
        {
            DateTime Now = DateTime.Now;
            int Years = new DateTime(DateTime.Now.Subtract(dateOfBirth).Ticks).Year - 1;
            DateTime PastYearDate = dateOfBirth.AddYears(Years);
            int Months = 0;
            for (int i = 1; i <= 12; i++)
            {
                if (PastYearDate.AddMonths(i) == Now)
                {
                    Months = i;
                    break;
                }
                else if (PastYearDate.AddMonths(i) >= Now)
                {
                    Months = i - 1;
                    break;
                }
            }
            int Days = Now.Subtract(PastYearDate.AddMonths(Months)).Days;
            int Hours = Now.Subtract(PastYearDate).Hours;
            int Minutes = Now.Subtract(PastYearDate).Minutes;
            int Seconds = Now.Subtract(PastYearDate).Seconds;
            return String.Format("Age: {0} Year(s) {1} Month(s) {2} Day(s) {3} Hour(s) {4} Minute(s) {5}  Second(s)",Years, Months, Days, Hours,Minutes, Seconds);
        }



        //POST:api/BloodDonor/DeleteDonorRecord
        [HttpPost]
        [ActionName("DeleteDonorRecord")]

        public async Task<ActionResult<VmResponseMessage>> DeleteDonorRecord(VmParams vm)
        {
            VmResponseMessage msg = new VmResponseMessage();
            int response = 0;


            BloodDonorRegistration donor = new BloodDonorRegistration();

            var data = await db.BloodDonorRegistration.Where(d => d.DonorContactNumber == vm.DonorContactNumber).FirstOrDefaultAsync();

            if (data != null)
            {
                BloodDonorRegistration bloodDonor = new BloodDonorRegistration()
                {
                    DonorContactNumber = data.DonorContactNumber,
                    DonorName = data.DonorName,
                    FatherName = data.FatherName,
                    Address = data.Address,
                    Age = data.Age,
                    Height = data.Height,
                    Weight = data.Weight,
                    NIDorBirthId = data.NIDorBirthId,
                    BirthDate = data.BirthDate,
                    Email = data.Email,
                    Profession = data.Profession,
                    LastWorkPlace = data.LastWorkPlace,
                    MajorPhysicalProblem = data.MajorPhysicalProblem,
                    Smocker = data.Smocker,
                    DrugAddicted = data.DrugAddicted,
                    BloodDonateExperience = data.BloodDonateExperience,
                    MajorOperation = data.MajorOperation,

                    //All ForeignKey, Selected In DropDown

                    ThanaId = data.ThanaId,
                    GenderId = data.GenderId,
                    ReligionId = data.ReligionId,
                    BloodGroupId = data.BloodGroupId
                };

                donor = bloodDonor;
            }

            db.BloodDonorRegistration.Remove(donor);
            response= await db.SaveChangesAsync();

            if (response > 0)
            {
                msg.SuccessMessage = "Deleted Data Successfully!!";
            }
            else
            {
                msg.ErrorMessage = "Sorry Data can not Deletd";
            }
            return msg;
        }

        //GET:api/BloodDonor/ReadyExperiencedDonorList
        [HttpGet]
        [ActionName("ReadyExperiencedDonorList")]
        public async Task<ActionResult<IEnumerable<VmModelData>>> ReadyExperiencedDonorList()
        {
            List<VmModelData> ReadyDonorList = new List<VmModelData>();
            int totalDays = 0;
            var AllDonor = await db.BloodDonorRegistration.ToListAsync();
            
            
            foreach (var donor in AllDonor)
            {
                
                var result = await db.BloodDonationRecord.Where(dr => dr.DonorContactNumber == donor.DonorContactNumber).ToListAsync();
                if (result!=null)
                {
                    var lstRecord = result.LastOrDefault();
                     totalDays = (DateTime.Now.Date - lstRecord.DonateDate.Date).Days;
                }
                
                if (totalDays>120)
                {
                    VmModelData readyDonor = new VmModelData()
                    {
                        DonorName = donor.DonorName,
                        DonorContactNumber = donor.DonorContactNumber,
                        Address = donor.Address,
                        BloodGroupName = donor.BloodGroup.BloodGroupName,
                        TotalDays = totalDays
                    };


                    ReadyDonorList.Add(readyDonor);
                    TotalDays = 0;
                };
            }
            
            return ReadyDonorList;
        }



        //GET:api/BloodDonor/GetDonorListByBloodGroup
        [HttpGet]
        [ActionName("GetDonorListByBloodGroup")]
        public async Task<ActionResult<IEnumerable<VmModelData>>> GetDonorListByBloodGroup(VmParams perams)
        {

            List<VmModelData> VmList = new List<VmModelData>();

            var responseDataList = await db.BloodDonorRegistration.Include(th => th.Thana).Include(g => g.Gender).Include(r => r.Religion).Include(b => b.BloodGroup).Where(d=>d.BloodGroup.BloodGroupName.ToLower()== perams.BloodGroupName.ToLower() || d.BloodGroupId==perams.BloodGroupId).ToListAsync();

            foreach (var data in responseDataList)
            {

                var thana = await db.Thana.Where(d => d.ThanaId == data.ThanaId).Select(s => new { s.ThanaId, s.ThanaName, s.DistrictId }).FirstOrDefaultAsync();
                var district = await db.District.Where(d => d.DistrictId == thana.DistrictId).Select(s => new { s.DistrictId, s.DistrictName, s.CountryId }).FirstOrDefaultAsync();
                var country = await db.Country.Where(d => d.CountryId == district.CountryId).Select(s => new { s.CountryId, s.CountryName }).FirstOrDefaultAsync();

                VmModelData vm = new VmModelData()
                {
                    DonorContactNumber = data.DonorContactNumber,
                    DonorName = data.DonorName,
                    FatherName = data.FatherName,
                    Address = data.Address,
                    Age = data.Age,
                    Height = data.Height,
                    Weight = data.Weight,
                    NIDorBirthId = data.NIDorBirthId,
                    BirthDate = data.BirthDate,
                    Email = data.Email,
                    Profession = data.Profession,
                    LastWorkPlace = data.LastWorkPlace,
                    MajorPhysicalProblem = data.MajorPhysicalProblem,
                    Smocker = data.Smocker,
                    DrugAddicted = data.DrugAddicted,
                    BloodDonateExperience = data.BloodDonateExperience,
                    MajorOperation = data.MajorOperation,
                    ThanaName = data.Thana.ThanaName,
                    DistrictName = district.DistrictName,
                    CountryName = country.CountryName,
                    GenderName = data.Gender.GenderName,
                    ReligionName = data.Religion.ReligionName,
                    BloodGroupName = data.BloodGroup.BloodGroupName

                };
                VmList.Add(vm);
            };

            return VmList;
        }

       

        //GET:api/BloodDonor/GetDonorListByThana
        [HttpGet]
        [ActionName("GetDonorListByThana")]
        public async Task<ActionResult<IEnumerable<VmModelData>>> GetDonorListByThana(VmParams perams)
        {

            List<VmModelData> VmList = new List<VmModelData>();

            var responseDataList = await db.BloodDonorRegistration.Include(th => th.Thana).Include(g => g.Gender).Include(r => r.Religion).Include(b => b.BloodGroup).Where(d => d.Thana.ThanaName.ToLower() == perams.ThanaName.ToLower() || d.ThanaId == perams.ThanaId).ToListAsync();

            foreach (var data in responseDataList)
            {

                var thana = await db.Thana.Where(d => d.ThanaId == data.ThanaId).Select(s => new { s.ThanaId, s.ThanaName, s.DistrictId }).FirstOrDefaultAsync();
                var district = await db.District.Where(d => d.DistrictId == thana.DistrictId).Select(s => new { s.DistrictId, s.DistrictName, s.CountryId }).FirstOrDefaultAsync();
                var country = await db.Country.Where(d => d.CountryId == district.CountryId).Select(s => new { s.CountryId, s.CountryName }).FirstOrDefaultAsync();

                VmModelData vm = new VmModelData()
                {
                    DonorContactNumber = data.DonorContactNumber,
                    DonorName = data.DonorName,
                    FatherName = data.FatherName,
                    Address = data.Address,
                    Age = data.Age,
                    Height = data.Height,
                    Weight = data.Weight,
                    NIDorBirthId = data.NIDorBirthId,
                    BirthDate = data.BirthDate,
                    Email = data.Email,
                    Profession = data.Profession,
                    LastWorkPlace = data.LastWorkPlace,
                    MajorPhysicalProblem = data.MajorPhysicalProblem,
                    Smocker = data.Smocker,
                    DrugAddicted = data.DrugAddicted,
                    BloodDonateExperience = data.BloodDonateExperience,
                    MajorOperation = data.MajorOperation,
                    ThanaName = data.Thana.ThanaName,
                    DistrictName = district.DistrictName,
                    CountryName = country.CountryName,
                    GenderName = data.Gender.GenderName,
                    ReligionName = data.Religion.ReligionName,
                    BloodGroupName = data.BloodGroup.BloodGroupName

                };
                VmList.Add(vm);
            };

            return VmList;
        }

        //GET:api/BloodDonor/GetDonorListByGender
        [HttpGet]
        [ActionName("GetDonorListByGender")]
        public async Task<ActionResult<IEnumerable<VmModelData>>> GetDonorListByGender(VmParams perams)
        {

            List<VmModelData> VmList = new List<VmModelData>();

            var responseDataList = await db.BloodDonorRegistration.Include(th => th.Thana).Include(g => g.Gender).Include(r => r.Religion).Include(b => b.BloodGroup).Where(d => d.Gender.GenderName.ToLower() == perams.GenderName.ToLower() || d.GenderId == perams.GenderId).ToListAsync();

            foreach (var data in responseDataList)
            {

                var thana = await db.Thana.Where(d => d.ThanaId == data.ThanaId).Select(s => new { s.ThanaId, s.ThanaName, s.DistrictId }).FirstOrDefaultAsync();
                var district = await db.District.Where(d => d.DistrictId == thana.DistrictId).Select(s => new { s.DistrictId, s.DistrictName, s.CountryId }).FirstOrDefaultAsync();
                var country = await db.Country.Where(d => d.CountryId == district.CountryId).Select(s => new { s.CountryId, s.CountryName }).FirstOrDefaultAsync();

                VmModelData vm = new VmModelData()
                {
                    DonorContactNumber = data.DonorContactNumber,
                    DonorName = data.DonorName,
                    FatherName = data.FatherName,
                    Address = data.Address,
                    Age = data.Age,
                    Height = data.Height,
                    Weight = data.Weight,
                    NIDorBirthId = data.NIDorBirthId,
                    BirthDate = data.BirthDate,
                    Email = data.Email,
                    Profession = data.Profession,
                    LastWorkPlace = data.LastWorkPlace,
                    MajorPhysicalProblem = data.MajorPhysicalProblem,
                    Smocker = data.Smocker,
                    DrugAddicted = data.DrugAddicted,
                    BloodDonateExperience = data.BloodDonateExperience,
                    MajorOperation = data.MajorOperation,
                    ThanaName = data.Thana.ThanaName,
                    DistrictName = district.DistrictName,
                    CountryName = country.CountryName,
                    GenderName = data.Gender.GenderName,
                    ReligionName = data.Religion.ReligionName,
                    BloodGroupName = data.BloodGroup.BloodGroupName

                };
                VmList.Add(vm);
            };

            return VmList;
        }

        //GET:api/BloodDonor/GetDonorListByReligion
        [HttpGet]
        [ActionName("GetDonorListByReligion")]
        public async Task<ActionResult<IEnumerable<VmModelData>>> GetDonorListByReligion(VmParams perams)
        {

            List<VmModelData> VmList = new List<VmModelData>();

            var responseDataList = await db.BloodDonorRegistration.Include(th => th.Thana).Include(g => g.Gender).Include(r => r.Religion).Include(b => b.BloodGroup).Where(d => d.Religion.ReligionName.ToLower() == perams.ReligionName.ToLower() || d.ReligionId == perams.ReligionId).ToListAsync();

            foreach (var data in responseDataList)
            {

                var thana = await db.Thana.Where(d => d.ThanaId == data.ThanaId).Select(s => new { s.ThanaId, s.ThanaName, s.DistrictId }).FirstOrDefaultAsync();
                var district = await db.District.Where(d => d.DistrictId == thana.DistrictId).Select(s => new { s.DistrictId, s.DistrictName, s.CountryId }).FirstOrDefaultAsync();
                var country = await db.Country.Where(d => d.CountryId == district.CountryId).Select(s => new { s.CountryId, s.CountryName }).FirstOrDefaultAsync();

                VmModelData vm = new VmModelData()
                {
                    DonorContactNumber = data.DonorContactNumber,
                    DonorName = data.DonorName,
                    FatherName = data.FatherName,
                    Address = data.Address,
                    Age = data.Age,
                    Height = data.Height,
                    Weight = data.Weight,
                    NIDorBirthId = data.NIDorBirthId,
                    BirthDate = data.BirthDate,
                    Email = data.Email,
                    Profession = data.Profession,
                    LastWorkPlace = data.LastWorkPlace,
                    MajorPhysicalProblem = data.MajorPhysicalProblem,
                    Smocker = data.Smocker,
                    DrugAddicted = data.DrugAddicted,
                    BloodDonateExperience = data.BloodDonateExperience,
                    MajorOperation = data.MajorOperation,
                    ThanaName = data.Thana.ThanaName,
                    DistrictName = district.DistrictName,
                    CountryName = country.CountryName,
                    GenderName = data.Gender.GenderName,
                    ReligionName = data.Religion.ReligionName,
                    BloodGroupName = data.BloodGroup.BloodGroupName

                };
                VmList.Add(vm);
            };

            return VmList;
        }

        //GET:api/BloodDonor/GetDonorListByAddress
        [HttpGet]
        [ActionName("GetDonorListByAddress")]
        public async Task<ActionResult<IEnumerable<VmModelData>>> GetDonorListByAddress(VmParams perams)
        {

            List<VmModelData> VmList = new List<VmModelData>();

            var responseDataList = await db.BloodDonorRegistration.Include(th => th.Thana).Include(g => g.Gender).Include(r => r.Religion).Include(b => b.BloodGroup).Where(d => d.Address.ToLower() == perams.Address.ToLower()).ToListAsync();

            foreach (var data in responseDataList)
            {

                var thana = await db.Thana.Where(d => d.ThanaId == data.ThanaId).Select(s => new { s.ThanaId, s.ThanaName, s.DistrictId }).FirstOrDefaultAsync();
                var district = await db.District.Where(d => d.DistrictId == thana.DistrictId).Select(s => new { s.DistrictId, s.DistrictName, s.CountryId }).FirstOrDefaultAsync();
                var country = await db.Country.Where(d => d.CountryId == district.CountryId).Select(s => new { s.CountryId, s.CountryName }).FirstOrDefaultAsync();

                VmModelData vm = new VmModelData()
                {
                    DonorContactNumber = data.DonorContactNumber,
                    DonorName = data.DonorName,
                    FatherName = data.FatherName,
                    Address = data.Address,
                    Age = data.Age,
                    Height = data.Height,
                    Weight = data.Weight,
                    NIDorBirthId = data.NIDorBirthId,
                    BirthDate = data.BirthDate,
                    Email = data.Email,
                    Profession = data.Profession,
                    LastWorkPlace = data.LastWorkPlace,
                    MajorPhysicalProblem = data.MajorPhysicalProblem,
                    Smocker = data.Smocker,
                    DrugAddicted = data.DrugAddicted,
                    BloodDonateExperience = data.BloodDonateExperience,
                    MajorOperation = data.MajorOperation,
                    ThanaName = data.Thana.ThanaName,
                    DistrictName = district.DistrictName,
                    CountryName = country.CountryName,
                    GenderName = data.Gender.GenderName,
                    ReligionName = data.Religion.ReligionName,
                    BloodGroupName = data.BloodGroup.BloodGroupName

                };
                VmList.Add(vm);
            };

            return VmList;
        }

        //GET:api/BloodDonor/GetDonorByNID_BirthId
        [HttpGet]
        [ActionName("GetDonorByNID_BirthId")]
        public async Task<ActionResult<VmModelData>> GetDonorByNID_BirthId(VmParams perams)
        {

           

            var data = await db.BloodDonorRegistration.Include(th => th.Thana).Include(g => g.Gender).Include(r => r.Religion).Include(b => b.BloodGroup).Where(d => d.NIDorBirthId.ToLower() == perams.NIDorBirthId.ToLower()).FirstOrDefaultAsync();

       
                var thana = await db.Thana.Where(d => d.ThanaId == data.ThanaId).Select(s => new { s.ThanaId, s.ThanaName, s.DistrictId }).FirstOrDefaultAsync();
                var district = await db.District.Where(d => d.DistrictId == thana.DistrictId).Select(s => new { s.DistrictId, s.DistrictName, s.CountryId }).FirstOrDefaultAsync();
                var country = await db.Country.Where(d => d.CountryId == district.CountryId).Select(s => new { s.CountryId, s.CountryName }).FirstOrDefaultAsync();

                VmModelData vm = new VmModelData()
                {
                    DonorContactNumber = data.DonorContactNumber,
                    DonorName = data.DonorName,
                    FatherName = data.FatherName,
                    Address = data.Address,
                    Age = data.Age,
                    Height = data.Height,
                    Weight = data.Weight,
                    NIDorBirthId = data.NIDorBirthId,
                    BirthDate = data.BirthDate,
                    Email = data.Email,
                    Profession = data.Profession,
                    LastWorkPlace = data.LastWorkPlace,
                    MajorPhysicalProblem = data.MajorPhysicalProblem,
                    Smocker = data.Smocker,
                    DrugAddicted = data.DrugAddicted,
                    BloodDonateExperience = data.BloodDonateExperience,
                    MajorOperation = data.MajorOperation,
                    ThanaName = data.Thana.ThanaName,
                    DistrictName = district.DistrictName,
                    CountryName = country.CountryName,
                    GenderName = data.Gender.GenderName,
                    ReligionName = data.Religion.ReligionName,
                    BloodGroupName = data.BloodGroup.BloodGroupName

                };
              
           

            return vm;
        }



        //GET:api/BloodDonor/GetDonorByName
        [HttpGet]
        [ActionName("GetDonorByName")]
        public async Task<ActionResult<VmModelData>> GetDonorByName(VmParams perams)
        {



            var data = await db.BloodDonorRegistration.Include(th => th.Thana).Include(g => g.Gender).Include(r => r.Religion).Include(b => b.BloodGroup).Where(d => d.DonorName.ToLower() == perams.DonorName.ToLower()).FirstOrDefaultAsync();


            var thana = await db.Thana.Where(d => d.ThanaId == data.ThanaId).Select(s => new { s.ThanaId, s.ThanaName, s.DistrictId }).FirstOrDefaultAsync();
            var district = await db.District.Where(d => d.DistrictId == thana.DistrictId).Select(s => new { s.DistrictId, s.DistrictName, s.CountryId }).FirstOrDefaultAsync();
            var country = await db.Country.Where(d => d.CountryId == district.CountryId).Select(s => new { s.CountryId, s.CountryName }).FirstOrDefaultAsync();

            VmModelData vm = new VmModelData()
            {
                DonorContactNumber = data.DonorContactNumber,
                DonorName = data.DonorName,
                FatherName = data.FatherName,
                Address = data.Address,
                Age = data.Age,
                Height = data.Height,
                Weight = data.Weight,
                NIDorBirthId = data.NIDorBirthId,
                BirthDate = data.BirthDate,
                Email = data.Email,
                Profession = data.Profession,
                LastWorkPlace = data.LastWorkPlace,
                MajorPhysicalProblem = data.MajorPhysicalProblem,
                Smocker = data.Smocker,
                DrugAddicted = data.DrugAddicted,
                BloodDonateExperience = data.BloodDonateExperience,
                MajorOperation = data.MajorOperation,
                ThanaName = data.Thana.ThanaName,
                DistrictName = district.DistrictName,
                CountryName = country.CountryName,
                GenderName = data.Gender.GenderName,
                ReligionName = data.Religion.ReligionName,
                BloodGroupName = data.BloodGroup.BloodGroupName

            };



            return vm;
        }







    }
}
