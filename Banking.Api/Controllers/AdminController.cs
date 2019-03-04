using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Banking.Api.Dto;
using Banking.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Banking.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : Controller
    {
        #region Injected Dependencies

        private IBankingService _bankingService;

        public AdminController(IBankingService bankingService)
        {
            _bankingService = bankingService;
        }

        #endregion

        [HttpGet("existingAccounts")]
        public ActionResult<ExistingAccountsResultDto> GetExistingAccounts()
        {
            try
            {
                return new ExistingAccountsResultDto { IsSuccess = true, Accounts = _bankingService.GetExistingAccounts().Select(x => x.Number).ToList() };
            }
            catch (Exception e)
            {
                Console.WriteLine("Log full exception here");
                return new ExistingAccountsResultDto { IsSuccess = false, ErrorMessage = e.Message };
            }
        }
    }
}