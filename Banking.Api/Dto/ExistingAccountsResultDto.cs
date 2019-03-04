using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Banking.Api.Dto
{
    public struct ExistingAccountsResultDto
    {
        public bool IsSuccess { get; set; }
        public string ErrorMessage { get; set; }

        public List<string> Accounts { get; set; }
    }
}
