using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BloodBank_App_2021.Models;
using BloodBank_App_2021.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;

namespace BloodBank_App_2021.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BloodDonorPicture : ControllerBase
    {
        //public readonly BloodAppDbContext db;
        //private readonly IHostingEnvironment _env;
        //public BloodDonorPicture(BloodAppDbContext _db, IHostingEnvironment env)
        //{
        //    this.db = _db;
        //    _env = env;
        //}
    }
}
