using System;
using System.Collections.Generic;
using System.Text;

namespace Banking.Service.Interfaces.Models
{
    public interface IBankAccount
    {
        string Number { get; }

        decimal Balance { get; }

        List<ITransactionHistory> History { get; }

        string MakeDeposit(decimal amount, DateTime date, string comment);

        string MakeWithdrawal(decimal amount, DateTime date, string comment);
    }
}
