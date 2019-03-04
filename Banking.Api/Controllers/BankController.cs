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
    [Route("api/[controller]")]
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

        [HttpGet("getBalance/{accountNumber}")]
        public ActionResult<BalanceResultDto> GetBalance(string accountNumber)
        {
            try
            {
                if (!_bankingService.IsExistingAccount(accountNumber))
                {
                    return new BalanceResultDto { IsSuccess = false, ErrorType = BalanceResultDto.ErrorTypeEnum.Err_Account_Not_Found, ErrorMessage = Err_Account_Not_Found };
                }

                return new BalanceResultDto { IsSuccess = true, Balance = _bankingService.GetBalance(accountNumber) };
            }
            catch (Exception)
            {
                Console.WriteLine("Log full exception here");
                return new BalanceResultDto { IsSuccess = false, ErrorMessage = Err_Unexpected_Error };
            }
        }

        [HttpGet("getHistory/{accountNumber}")]
        public ActionResult<HistoryResultDto> GetHistory(string accountNumber)
        {
            try
            {
                if (!_bankingService.IsExistingAccount(accountNumber))
                {
                    return new HistoryResultDto { IsSuccess = false, ErrorType = HistoryResultDto.ErrorTypeEnum.Err_Account_Not_Found, ErrorMessage = Err_Account_Not_Found };
                }

                return new HistoryResultDto
                {
                    IsSuccess = true,
                    History = _bankingService.GetHistory(accountNumber)
                                             .Select(x => new HistoryResultDto.TransactionDto
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
                return new HistoryResultDto { IsSuccess = false, ErrorMessage = Err_Unexpected_Error };
            }
        }
        
        [HttpPost("deposit")]
        public ActionResult<DepositResultDto> Deposit(string accountNumber, decimal amount, string comment)
        {
            try
            {
                if (!_bankingService.IsExistingAccount(accountNumber))
                {
                    return new DepositResultDto { IsSuccess = false, ErrorType = DepositResultDto.ErrorTypeEnum.Err_Account_Not_Found, ErrorMessage = Err_Account_Not_Found };
                }

                if (amount <= 0)
                {
                    return new DepositResultDto { IsSuccess = false, ErrorType = DepositResultDto.ErrorTypeEnum.Err_Amount_Should_Be_A_Positive_Value, ErrorMessage = Err_Amount_Should_Be_A_Positive_Value };
                }

                return new DepositResultDto { IsSuccess = true, TransactionIdentifier = _bankingService.Deposit(accountNumber, amount, comment) };
            }
            catch (Exception)
            {
                Console.WriteLine("Log full exception here");
                return new DepositResultDto { IsSuccess = false, ErrorMessage = Err_Unexpected_Error };
            }
        }

        [HttpPost("withdraw")]
        public ActionResult<WithdrawResultDto> Withdraw(string accountNumber, decimal amount, string comment)
        {
            try
            {
                if (!_bankingService.IsExistingAccount(accountNumber))
                {
                    return new WithdrawResultDto { IsSuccess = false, ErrorType = WithdrawResultDto.ErrorTypeEnum.Err_Account_Not_Found, ErrorMessage = Err_Account_Not_Found };
                }

                if (amount <= 0)
                {
                    return new WithdrawResultDto { IsSuccess = false, ErrorType = WithdrawResultDto.ErrorTypeEnum.Err_Amount_Should_Be_A_Positive_Value, ErrorMessage = Err_Amount_Should_Be_A_Positive_Value };
                }

                if (amount > _bankingService.GetBalance(accountNumber))
                {
                    return new WithdrawResultDto { IsSuccess = false, ErrorType = WithdrawResultDto.ErrorTypeEnum.Err_Insufficient_Funds, ErrorMessage = Err_Insufficient_Funds };
                }

                return new WithdrawResultDto { IsSuccess = true, TransactionIdentifier = _bankingService.Withdraw(accountNumber, amount, comment) };
            }
            catch (Exception)
            {
                Console.WriteLine("Log full exception here");
                return new WithdrawResultDto { IsSuccess = false, ErrorMessage = Err_Unexpected_Error };
            }
        }
    }
}
