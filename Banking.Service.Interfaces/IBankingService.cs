using Banking.Service.Interfaces.Models;
using System;
using System.Collections.Generic;

namespace Banking.Service.Interfaces
{
    public interface IBankingService
    {
        List<IBankAccount> GetExistingAccounts();
        bool IsExistingAccount(string accountNumber);

        decimal GetBalance(string accountNumber);

        List<ITransactionHistory> GetHistory(string accountNumber);

        string Deposit(string accountNumber, decimal amount, string note);
        string Withdraw(string accountNumber, decimal amount, string note);
    }
}
