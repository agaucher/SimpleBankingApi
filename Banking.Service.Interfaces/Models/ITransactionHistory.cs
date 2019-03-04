using System;
using System.Collections.Generic;
using System.Text;

namespace Banking.Service.Interfaces.Models
{
    public interface ITransactionHistory
    {
        ITransaction Transaction { get; set; }

        decimal NewBalanceAfterTransaction { get; set; }
    }
}
