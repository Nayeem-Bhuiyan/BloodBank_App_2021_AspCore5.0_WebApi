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
    public class BloodUsers : ControllerBase
    {
        public readonly BloodAppDbContext db;

        public BloodUsers(BloodAppDbContext _db)
        {
            this.db = _db;
        }

        //GET:api/BloodUsers/GetBloodRequestUserList

        [HttpGet]
        [ActionName("GetBloodRequestUserList")]

        public async Task<ActionResult<IEnumerable<VmModelData>>> GetBloodRequestUserList()
        {
            List<VmModelData> vmList = new List<VmModelData>();

            var dataList = await db.BloodRequestUser.Include(bg=>bg.BloodGroup).ToListAsync();

            foreach (var data in dataList)
            {
                VmModelData vm = new VmModelData()
                {
                    BloodRequestUserId = data.BloodRequestUserId,
                    UserName = data.UserName,
                    UserMobile = data.UserMobile,
                    UserAddress = data.UserAddress,
                    ReasonOfBlood = data.ReasonOfBlood,
                    AmountOfBloodRequested = data.AmountOfBloodRequested,
                    Date_BloodRequest = data.Date_BloodRequest,
                    Date_BloodNeed = data.Date_BloodNeed,
                    BloodGroupName = data.BloodGroup.BloodGroupName
                };
                vmList.Add(vm);
            }
            return vmList;
        }

        //GET:api/BloodUsers/GetBloodRequestUserById/{id}
        [HttpGet("{id}")]
        [ActionName("GetBloodRequestUserById")]
        public async Task<ActionResult<BloodRequestUser>> GetBloodRequestUserById(int id)
        {

            BloodRequestUser bloodUser = new BloodRequestUser();
          

            var data = await db.BloodRequestUser.Where(r => r.BloodRequestUserId == id).FirstOrDefaultAsync();

            if (data != null)
            {
                BloodRequestUser user = new BloodRequestUser()
                {
                    BloodRequestUserId = data.BloodRequestUserId,
                    UserName = data.UserName,
                    UserMobile = data.UserMobile,
                    UserAddress = data.UserAddress,
                    ReasonOfBlood = data.ReasonOfBlood,
                    AmountOfBloodRequested = data.AmountOfBloodRequested,
                    Date_BloodRequest = data.Date_BloodRequest,
                    Date_BloodNeed = data.Date_BloodNeed,
                    BloodGroupId = data.BloodGroupId
                };

                bloodUser = user;

            }

            return bloodUser;

        }


        //POST:api/BloodUsers/InsertOrUpdateBloodUser
        [HttpPost]
        [ActionName("InsertOrUpdateBloodUser")]

        public async Task<ActionResult<VmResponseMessage>> InsertOrUpdateBloodUser([FromBody] BloodRequestUser bloodUser)
        {
            VmResponseMessage msg = new VmResponseMessage();
            int response;

            if (bloodUser.BloodRequestUserId==0)
            {
                db.BloodRequestUser.Add(bloodUser);
                response = await db.SaveChangesAsync();


                if (response > 0)
                {
                    msg.SuccessMessage = "Inserted Data Successfully!!";
                }
                else
                {
                    msg.ErrorMessage = "Sorry Data can not Inserted";
                }

            }
            else if(bloodUser.BloodRequestUserId>0)
            {
                db.Entry(bloodUser).State = EntityState.Modified;
                response = await db.SaveChangesAsync();

                if (response > 0)
                {
                    msg.SuccessMessage = "Updated Data Successfully!!";
                }
                else
                {
                    msg.ErrorMessage = "Sorry Data can not Updated";
                }
            }
            else
            {
                msg.ErrorMessage = "Sorry Data can not Post";
            }

            
            return msg;
        }


        //POST:api/BloodUsers/DeleteBloodUserRecordById/{id}
        [HttpPost("{id}")]
        [ActionName("DeleteBloodUserRecordById")]

        public async Task<ActionResult<VmResponseMessage>> DeleteBloodUserRecordById(int id)
        {
            VmResponseMessage msg = new VmResponseMessage();
            int response = 0;

            BloodRequestUser user=  await db.BloodRequestUser.Where(r=>r.BloodRequestUserId==id).FirstOrDefaultAsync();
            if (user!=null)
            {
                db.BloodRequestUser.Remove(user);
                response= await db.SaveChangesAsync();

                if (response > 0)
                {
                    msg.SuccessMessage = "Deletd Data Successfully!!";
                }
                else
                {
                    msg.ErrorMessage = "Sorry Data can not Deletd";
                }
            }
            else
            {
                msg.ErrorMessage = "Sorry Data can not Deletd";
            }
            
            return msg;
        }


    }
}
