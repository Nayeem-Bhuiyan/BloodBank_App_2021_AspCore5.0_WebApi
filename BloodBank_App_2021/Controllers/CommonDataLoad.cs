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
    public class CommonDataLoad : ControllerBase
    {
        public readonly BloodAppDbContext db;
        
        public CommonDataLoad(BloodAppDbContext _db)
        {
            this.db = _db;
        }

        [HttpGet]
        [ActionName("LoadGenderList")]
        public async Task<ActionResult<IEnumerable<Gender>>> LoadGenderList()
        {
            return await db.Gender.ToListAsync();
        }


        [HttpGet]
        [ActionName("LoadReligionList")]
        public async Task<ActionResult<IEnumerable<Religion>>> LoadReligionList()
        {
            return await db.Religion.ToListAsync();
        }


        [HttpGet]
        [ActionName("LoadBloodGroupList")]
        public async Task<ActionResult<IEnumerable<BloodGroup>>> LoadBloodGroupList()
        {
            return await db.BloodGroup.ToListAsync();
        }

        [HttpGet]
        [ActionName("LoadCountryList")]
        public async Task<ActionResult<IEnumerable<Country>>> LoadCountryList()
        {
            return await db.Country.ToListAsync();
        }


        [HttpGet("{id}")]
        [ActionName("LoadDistrictListByCountryId")]
        public async Task<ActionResult<IEnumerable<District>>> LoadDistrictListByCountryId(int id)  //id means CountryId
        {
            List<District> districtList = new List<District>();

          var dList = await db.District.Where(d => d.CountryId == id).Select(d => new { d.DistrictId, d.DistrictName, d.CountryId }).ToListAsync();
            foreach (var district in dList)
            {
                District d = new District()
                {
                    DistrictId=district.DistrictId,
                    DistrictName=district.DistrictName,
                    CountryId=district.CountryId
                };

                districtList.Add(d);
            }
            return districtList;
        }

        [HttpGet("{id}")]
        [ActionName("LoadThanaListByDistrictId")]
        public async Task<ActionResult<IEnumerable<Thana>>> LoadThanaListByDistrictId(int id)  //id means DistrictId
        {
            List<Thana> thanaList = new List<Thana>();
            var dataList= await db.Thana.Where(t => t.DistrictId == id).Select(t => new {t.ThanaId,t.ThanaName,t.DistrictId }).ToListAsync();
            foreach (var th in dataList)
            {
                Thana thana = new Thana()
                {
                   ThanaId=th.ThanaId,
                    ThanaName=th.ThanaName,
                    DistrictId=th.DistrictId
                };
                thanaList.Add(thana);
            }

            return thanaList;
        }



    }
}
