using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Banking.Api.Dto
{
    public struct WithdrawResultDto
    {
        public bool IsSuccess { get; set; }
        public ErrorTypeEnum ErrorType { get; set; }
        public string ErrorMessage { get; set; }

        public string TransactionIdentifier { get; set; }

        public enum ErrorTypeEnum
        {
            None,
            Err_Account_Not_Found,
            Err_Amount_Should_Be_A_Positive_Value,
            Err_Insufficient_Funds,
        }
    }
}
