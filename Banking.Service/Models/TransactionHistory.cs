using System;
using System.Collections.Generic;
using System.Text;

namespace Banking.Service.Interfaces.Models
{
    public class TransactionHistory : ITransactionHistory
    {
        public ITransaction Transaction { get; set; }

        public decimal NewBalanceAfterTransaction { get; set; }
    }
}
