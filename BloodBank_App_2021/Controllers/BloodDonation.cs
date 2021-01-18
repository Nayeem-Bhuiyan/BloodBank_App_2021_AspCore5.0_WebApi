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
    public class BloodDonation : ControllerBase
    {
        public readonly BloodAppDbContext db;

        public BloodDonation(BloodAppDbContext _db)
        {
            this.db = _db;
        }

        //GET:api/BloodDonation/DonationRecordList
        [HttpGet]
        [ActionName("DonationRecordList")]
        public async Task<ActionResult<IEnumerable<VmModelData>>> DonationRecordList()
        {
            List<VmModelData> DonationRecordList = new List<VmModelData>();
            var dataList = await db.BloodDonationRecord.Include(dr=>dr.BloodDonorRegistration).ToListAsync();
            foreach (var m in dataList)
            {
                VmModelData donationRecord = new VmModelData()
                {
                    BloodDonationRecordId = m.BloodDonationRecordId,
                    BloodReceivedBy = m.BloodReceivedBy,
                    AmountOfBlood = m.AmountOfBlood,
                    DonateDate = m.DonateDate,
                    DonatePlace = m.DonatePlace,
                    ReasonOfDonation = m.ReasonOfDonation,
                    DonorContactNumber = m.DonorContactNumber,
                    DonorName=m.BloodDonorRegistration.DonorName,
                    Address = m.BloodDonorRegistration.Address,
                    Age = m.BloodDonorRegistration.Age,
                    Profession = m.BloodDonorRegistration.Profession
                    
                };
                DonationRecordList.Add(donationRecord);
            }

            return DonationRecordList;
        }

        //POST:api/BloodDonation/InsertorUpdatedDonationRecord
        [HttpPost]
        [Consumes("application/json")]
        [ActionName("InsertorUpdatedDonationRecord")]
        public async Task<ActionResult<VmResponseMessage>> InsertorUpdatedDonationRecord([FromBody] BloodDonationRecord donation)
        {
            VmResponseMessage msg = new VmResponseMessage();
            int responseNumber=0;
            //Insert Data
            BloodDonorRegistration DonarConactCheck = await db.BloodDonorRegistration.FindAsync(donation.DonorContactNumber);

            if (donation.BloodDonationRecordId<=0 && DonarConactCheck.DonorContactNumber!=null)
            {
                BloodDonationRecord donationInsert = new BloodDonationRecord()
                {
                    BloodDonationRecordId = donation.BloodDonationRecordId,
                    BloodReceivedBy = donation.BloodReceivedBy,
                    AmountOfBlood = donation.AmountOfBlood,
                    DonateDate = donation.DonateDate,
                    DonatePlace = donation.DonatePlace,
                    ReasonOfDonation = donation.ReasonOfDonation,
                    DonorContactNumber = DonarConactCheck.DonorContactNumber
                };

                db.BloodDonationRecord.Add(donation);
                responseNumber =  await db.SaveChangesAsync();
                if (responseNumber > 0)
                {
                    msg.SuccessMessage = "Inserted Data Successfully";
                }
                else
                {
                    msg.ErrorMessage = "Sorry Data Can not inserted";
                }
            }
            else if(donation.BloodDonationRecordId >0 && DonarConactCheck.DonorContactNumber != null)
            {
                var donationRecord =await db.BloodDonationRecord.FindAsync(donation.BloodDonationRecordId);
                if (donationRecord==null)
                {
                    return NotFound();
                }
                
                
                    BloodDonationRecord donationEditData = new BloodDonationRecord()
                    {
                        BloodDonationRecordId=donation.BloodDonationRecordId ,
                        BloodReceivedBy=donation.BloodReceivedBy ,
                        AmountOfBlood=donation.AmountOfBlood ,
                        DonateDate=donation.DonateDate ,
                        DonatePlace=donation.DonatePlace ,
                        ReasonOfDonation=donation.ReasonOfDonation ,
                        DonorContactNumber= DonarConactCheck.DonorContactNumber
                    };
                    db.Entry(donationEditData).State = EntityState.Modified;
                    responseNumber= await db.SaveChangesAsync();

                    if (responseNumber > 0)
                    {
                        msg.SuccessMessage = "Updated Data Successfully";
                    }
                    else
                    {
                        msg.ErrorMessage = "Sorry Data Can not Updated";
                    }


            }
            else
            {
                msg.ErrorMessage = "Invalid Data";
            }

            return msg;
        }

        //POST:api/BloodDonation/DeleteDonationRecord/{id}
        [HttpPost("{id}")]
        [ActionName("DeleteDonationRecord")]
        public async Task<ActionResult<VmResponseMessage>> DeleteDonationRecord(int id)
        {
            VmResponseMessage msg = new VmResponseMessage();
            int responseNumber = 0;

            if (id<=0)
            {
                msg.ErrorMessage = "invalid Id";
            }
           var m= await db.BloodDonationRecord.Where(d => d.BloodDonationRecordId == id).FirstOrDefaultAsync();
            
            if (m!=null)
            {
               db.BloodDonationRecord.Remove(m);
               responseNumber =await db.SaveChangesAsync();

                if (responseNumber > 0)
                {
                    msg.SuccessMessage = "Deleted Data Successfully";
                }
                else
                {
                    msg.ErrorMessage = "Sorry Data Can not Delete";
                }
            }

            return msg;
        }


        //GET:api/BloodDonation/GetDonationRecordById/{id}
        [HttpGet("{id}")]
        [ActionName("GetDonationRecordById")]
        public async Task<ActionResult<BloodDonationRecord>> GetDonationRecordById(int id)
        {

            BloodDonationRecord dRecord = new BloodDonationRecord();

            var m = await db.BloodDonationRecord.Where(d => d.BloodDonationRecordId == id).FirstOrDefaultAsync();
            if (m!=null)
            {
                BloodDonationRecord donationRecord = new BloodDonationRecord()
                {
                    BloodDonationRecordId = m.BloodDonationRecordId,
                    BloodReceivedBy = m.BloodReceivedBy,
                    AmountOfBlood = m.AmountOfBlood,
                    DonateDate = m.DonateDate,
                    DonatePlace = m.DonatePlace,
                    ReasonOfDonation = m.ReasonOfDonation,
                    DonorContactNumber = m.DonorContactNumber
                };
                dRecord=donationRecord;
            }
            return dRecord;
        }




        //GET:api/BloodDonation/TotalAmountOfBloodDonateUpToToday
        [HttpGet]
        [ActionName("TotalAmountOfBloodDonateUpToToday")]
        public async Task<ActionResult<double>> TotalAmountOfBloodDonateUpToToday()
        {
            double TotalAmountOfBlood = 0;
           
            var dataList = await db.BloodDonationRecord.Include(dr => dr.BloodDonorRegistration).ToListAsync();
            foreach (var m in dataList)
            {
                TotalAmountOfBlood += m.AmountOfBlood;
            }

            return TotalAmountOfBlood;
        }



        //GET:api/BloodDonation/DonationRecordList
        [HttpGet]
        [ActionName("DonationRecordList")]
        public async Task<ActionResult<IEnumerable<VmModelData>>> DonationRecordListByDonor(VmParams prm)
        {
            List<VmModelData> DonationRecordList = new List<VmModelData>();
            var dataList = await db.BloodDonationRecord.Include(dr => dr.BloodDonorRegistration).Where(dr=>dr.DonorContactNumber.ToLower()==prm.DonorContactNumber.ToLower()).ToListAsync();
            foreach (var m in dataList)
            {
                VmModelData donationRecord = new VmModelData()
                {
                    BloodDonationRecordId = m.BloodDonationRecordId,
                    BloodReceivedBy = m.BloodReceivedBy,
                    AmountOfBlood = m.AmountOfBlood,
                    DonateDate = m.DonateDate,
                    DonatePlace = m.DonatePlace,
                    ReasonOfDonation = m.ReasonOfDonation,
                    DonorContactNumber = m.DonorContactNumber,
                    DonorName = m.BloodDonorRegistration.DonorName,
                    Address = m.BloodDonorRegistration.Address,
                    Age = m.BloodDonorRegistration.Age,
                    Profession = m.BloodDonorRegistration.Profession

                };
                DonationRecordList.Add(donationRecord);
            }

            return DonationRecordList;
        }


        //GET:api/BloodDonation/CalculateTotalBloodAmountByEachDonor
        [HttpGet]
        [ActionName("CalculateTotalBloodAmountByEachDonor")]
        public async Task<ActionResult<IEnumerable<VmModelData>>> CalculateTotalBloodAmountByEachDonor()
        {
            double TotalBloodCount = 0;
            var DonorList = await db.BloodDonorRegistration.ToListAsync();

            List<VmModelData> VmList = new List<VmModelData>();

            foreach (var donor in DonorList)
            {
                var donationRecordByThisDonor = await db.BloodDonationRecord.Where(dr => dr.DonorContactNumber == donor.DonorContactNumber).ToListAsync();
                foreach (var donation in donationRecordByThisDonor)
                {
                    TotalBloodCount += donation.AmountOfBlood;
                }

                

                VmModelData vm = new VmModelData()
                {
                    DonorName=donor.DonorName,
                    DonorContactNumber=donor.DonorContactNumber,
                    BloodGroupName=donor.BloodGroup.BloodGroupName,
                    AmountOfBlood=TotalBloodCount
                };

                VmList.Add(vm);
            }

            return VmList;
        }



        //GET:api/BloodDonation/CalculateTotalBloodAmountByThisDonor
        [HttpGet]
        [ActionName("CalculateTotalBloodAmountByThisDonor")]
        public async Task<ActionResult<double>> CalculateTotalBloodAmountByThisDonor(VmParams prm)
        {
            double TotalBloodCount = 0;
            var donationRecordByThisDonor = await db.BloodDonationRecord.Where(dr => dr.DonorContactNumber == prm.DonorContactNumber).ToListAsync();
            foreach (var donation in donationRecordByThisDonor)
            {
                TotalBloodCount += donation.AmountOfBlood;
            }

            return TotalBloodCount;
        }

      

    }
}
