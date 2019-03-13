using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Banking.Api.Dto
{
    public class AccountInfoDto
    {
        public bool IsSuccess { get; set; }
        public ErrorTypeEnum ErrorType { get; set; }
        public string ErrorMessage { get; set; }

        public List<LinkDto> Links { get; set; }

        public enum ErrorTypeEnum
        {
            None,
            Err_Account_Not_Found,
        }
    }
}
