using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Banking.Api.Dto
{
    public struct TransactionResultDto
    {
        public bool IsSuccess { get; set; }
        public ErrorTypeEnum ErrorType { get; set; }
        public string ErrorMessage { get; set; }

        public List<TransactionDto> History { get; set; }

        public enum ErrorTypeEnum
        {
            None,
            Err_Account_Not_Found,
        }

        public struct TransactionDto
        {
            public string Operation { get; set; }
            public DateTime Date { get; set; }
            public decimal Amount { get; set; }
            public string Comment { get; set; }
            public decimal Balance { get; set; }
        }
    }
}
