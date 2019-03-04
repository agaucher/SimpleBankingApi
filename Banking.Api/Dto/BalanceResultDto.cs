using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Banking.Api.Dto
{
    public struct BalanceResultDto
    {
        public bool IsSuccess { get; set; }
        public ErrorTypeEnum ErrorType { get; set; }
        public string ErrorMessage { get; set; }

        public decimal Balance { get; set; }

        public enum ErrorTypeEnum
        {
            None,
            Err_Account_Not_Found,
        }
    }
}
