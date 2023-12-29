using AMBRD.Models;
using AMBRD.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace AMBRD.API
{
    public class WalletApiController : ApiController
    {
        abdul_amurdEntities11 ent = new abdul_amurdEntities11();

        [HttpPost]
        [Route("api/WalletApi/AddWalletAmount")]
        public IHttpActionResult AddWalletAmount(UserWallet model)
        {
            try
            {
                if (model.UserId != null)
                {
                    var emp = ent.Patients.FirstOrDefault(a => a.Id == model.UserId);
                    if (emp.walletAmount == null)
                    {
                        emp.walletAmount = 0;
                    }
                    emp.walletAmount = model.walletAmount + emp.walletAmount;
                    ent.SaveChanges();
                    return Ok("Add Money SuccessFully");
                }
                else
                {
                    return Ok("Please enter the amount");
                }
            }
            catch
            {
                return BadRequest("Server Error");
            }
        }

        [HttpPost, Route("api/WalletApi/UpdateWalletAmount")]
        public IHttpActionResult UpdateWalletAmount(UserWallet model)
        {
            try
            {
                if (model.walletAmount != null)
                {
                    var emp = ent.Patients.FirstOrDefault(a => a.Id == model.UserId);
                    if (emp.walletAmount >= model.walletAmount)
                    {
                        emp.walletAmount = emp.walletAmount - model.walletAmount;
                        ent.SaveChanges();
                        return Ok("Wallet Amount Update SuccessFully");
                    }
                    else
                    {
                        return BadRequest("Please check wallet amount");
                    }
                }
                else
                {
                    return BadRequest("Please enter the amount");
                }
            }
            catch
            {
                return BadRequest("Server Error");
            }
        }

        [HttpGet, Route("api/WalletApi/GetWalletAmount")]

        public IHttpActionResult GetWalletAmount(int id)
        {
            string qry= @"select Id as UserId,walletAmount from Patient where Id=" + id + "";
            var WalletAmount = ent.Database.SqlQuery<UserWallet>(qry).FirstOrDefault();
            return Ok(WalletAmount);
        }
    }
}
