using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Banking.Api.Dto;
using Banking.Service;
using Banking.Service.Interfaces;
using Banking.Service.Interfaces.Models;
using Microsoft.AspNetCore.Mvc;

namespace Banking.Api.Controllers
{
    [Route("")]
    [ApiController]
    public class BankController : ControllerBase
    {
        #region Injected Dependencies

        private IBankingService _bankingService;

        public BankController(IBankingService bankingService)
        {
            _bankingService = bankingService;
        }

        #endregion

        private const string Err_Account_Not_Found = "Account doesn't exist.";
        private const string Err_Amount_Should_Be_A_Positive_Value = "Amount should be a positive value";
        private const string Err_Unexpected_Error = "An unexpected error occurs. Please retry later or contact your support team.";
        private const string Err_Insufficient_Funds = "Insufficient funds";

        [HttpGet("accounts", Name=nameof(GetExistingAccounts))]
        public ActionResult<ExistingAccountsResultDto> GetExistingAccounts()
        {
            try
            {
                return new ExistingAccountsResultDto
                {
                    IsSuccess = true,
                    Accounts = _bankingService.GetExistingAccounts().Select(x => x.Number).ToList(),
                };
            }
            catch (Exception e)
            {
                Console.WriteLine("Log full exception here");
                return new ExistingAccountsResultDto { IsSuccess = false, ErrorMessage = e.Message };
            }
        }

        [HttpGet("accounts/{uuid}", Name = nameof(GetAccountInfos))]
        public ActionResult<AccountInfoDto> GetAccountInfos(string uuid)
        {
            try
            {
                if (!_bankingService.IsExistingAccount(uuid))
                {
                    return new AccountInfoDto { IsSuccess = false, ErrorType = AccountInfoDto.ErrorTypeEnum.Err_Account_Not_Found, ErrorMessage = Err_Account_Not_Found };
                }

                return new AccountInfoDto
                {
                    IsSuccess = true,
                    Links = new List<LinkDto> {
                                new LinkDto { Href = this.Url.Link(nameof(GetBalance), new { uuid = uuid }), Rel = "Get current balance", Method = "Get" },
                                new LinkDto { Href = this.Url.Link(nameof(GetTransactions), new { uuid = uuid }), Rel = "Get transactions history", Method = "Get" },
                                new LinkDto { Href = this.Url.Link(nameof(Deposit), new { uuid = uuid }), Rel = "Make Deposit", Method = "Post" },
                                new LinkDto { Href = this.Url.Link(nameof(Withdraw), new { uuid = uuid }), Rel = "Make Withdraw", Method = "Post" },
                            },
                };
            }
            catch (Exception e)
            {
                Console.WriteLine("Log full exception here");
                return new AccountInfoDto { IsSuccess = false, ErrorMessage = Err_Unexpected_Error };
            }
        }

        [HttpGet("accounts/{uuid}/balance", Name = nameof(GetBalance))]
        public ActionResult<BalanceResultDto> GetBalance(string uuid)
        {
            try
            {
                if (!_bankingService.IsExistingAccount(uuid))
                {
                    return new BalanceResultDto { IsSuccess = false, ErrorType = BalanceResultDto.ErrorTypeEnum.Err_Account_Not_Found, ErrorMessage = Err_Account_Not_Found };
                }

                return new BalanceResultDto { IsSuccess = true, Balance = _bankingService.GetBalance(uuid) };
            }
            catch (Exception)
            {
                Console.WriteLine("Log full exception here");
                return new BalanceResultDto { IsSuccess = false, ErrorMessage = Err_Unexpected_Error };
            }
        }

        [HttpGet("accounts/{uuid}/transactions", Name = nameof(GetTransactions))]
        public ActionResult<TransactionResultDto> GetTransactions(string uuid)
        {
            try
            {
                if (!_bankingService.IsExistingAccount(uuid))
                {
                    return new TransactionResultDto { IsSuccess = false, ErrorType = TransactionResultDto.ErrorTypeEnum.Err_Account_Not_Found, ErrorMessage = Err_Account_Not_Found };
                }

                return new TransactionResultDto
                {
                    IsSuccess = true,
                    History = _bankingService.GetHistory(uuid)
                                             .Select(x => new TransactionResultDto.TransactionDto
                                             {
                                                 Operation = x.Transaction.Amount >= 0 ? "Deposit" : "Withdraw",
                                                 Date = x.Transaction.Date,
                                                 Amount = Math.Abs(x.Transaction.Amount),
                                                 Comment = x.Transaction.Comment,
                                                 Balance = x.NewBalanceAfterTransaction,
                                             }).ToList()
                };
            }
            catch (Exception)
            {
                Console.WriteLine("Log full exception here");
                return new TransactionResultDto { IsSuccess = false, ErrorMessage = Err_Unexpected_Error };
            }
        }
        
        [HttpPost("accounts/{uuid}/deposit", Name = nameof(Deposit))]
        public ActionResult<DepositResultDto> Deposit([FromRoute] string uuid, decimal amount, string comment)
        {
            try
            {
                if (!_bankingService.IsExistingAccount(uuid))
                {
                    return new DepositResultDto { IsSuccess = false, ErrorType = DepositResultDto.ErrorTypeEnum.Err_Account_Not_Found, ErrorMessage = Err_Account_Not_Found };
                }

                if (amount <= 0)
                {
                    return new DepositResultDto { IsSuccess = false, ErrorType = DepositResultDto.ErrorTypeEnum.Err_Amount_Should_Be_A_Positive_Value, ErrorMessage = Err_Amount_Should_Be_A_Positive_Value };
                }

                return new DepositResultDto { IsSuccess = true, TransactionIdentifier = _bankingService.Deposit(uuid, amount, comment) };
            }
            catch (Exception)
            {
                Console.WriteLine("Log full exception here");
                return new DepositResultDto { IsSuccess = false, ErrorMessage = Err_Unexpected_Error };
            }
        }

        [HttpPost("accounts/{uuid}/withdraw", Name = nameof(Withdraw))]
        public ActionResult<WithdrawResultDto> Withdraw([FromQuery] string uuid, decimal amount, string comment)
        {
            try
            {
                if (!_bankingService.IsExistingAccount(uuid))
                {
                    return new WithdrawResultDto { IsSuccess = false, ErrorType = WithdrawResultDto.ErrorTypeEnum.Err_Account_Not_Found, ErrorMessage = Err_Account_Not_Found };
                }

                if (amount <= 0)
                {
                    return new WithdrawResultDto { IsSuccess = false, ErrorType = WithdrawResultDto.ErrorTypeEnum.Err_Amount_Should_Be_A_Positive_Value, ErrorMessage = Err_Amount_Should_Be_A_Positive_Value };
                }

                if (amount > _bankingService.GetBalance(uuid))
                {
                    return new WithdrawResultDto { IsSuccess = false, ErrorType = WithdrawResultDto.ErrorTypeEnum.Err_Insufficient_Funds, ErrorMessage = Err_Insufficient_Funds };
                }

                return new WithdrawResultDto { IsSuccess = true, TransactionIdentifier = _bankingService.Withdraw(uuid, amount, comment) };
            }
            catch (Exception)
            {
                Console.WriteLine("Log full exception here");
                return new WithdrawResultDto { IsSuccess = false, ErrorMessage = Err_Unexpected_Error };
            }
        }
    }
}
