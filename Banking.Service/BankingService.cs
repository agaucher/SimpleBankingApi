using Banking.Dao.Interfaces;
using Banking.Service.Interfaces;
using Banking.Service.Interfaces.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Banking.Service
{
    public class BankingService : IBankingService
    {
        #region Injected Dependencies

        private IBankAccountDao _bankAccountDao;

        public BankingService(IBankAccountDao bankAccountDao)
        {
            _bankAccountDao = bankAccountDao;
        }

        #endregion

        public List<IBankAccount> GetExistingAccounts()
        {
            return _bankAccountDao.GetList();
        }

        public bool IsExistingAccount(string accountNumber)
        {
            return _bankAccountDao.GetByNumber(accountNumber) != null;
        }

        public decimal GetBalance(string accountNumber)
        {
            var account = _bankAccountDao.GetByNumber(accountNumber);

            if (account == null)
            {
                throw new InvalidOperationException($"unknown account: {accountNumber}");
            }

            return account.Balance;
        }

        public List<ITransactionHistory> GetHistory(string accountNumber)
        {
            var account = _bankAccountDao.GetByNumber(accountNumber);

            if (account == null)
            {
                throw new InvalidOperationException($"unknown account: {accountNumber}");
            }

            return account.History;
        }

        public string Deposit(string accountNumber, decimal amount, string comment)
        {
            var account = _bankAccountDao.GetByNumber(accountNumber);

            if (account == null)
            {
                throw new InvalidOperationException($"unknown account: {accountNumber}");
            }

            return account.MakeDeposit(amount, DateTime.Now, comment);
        }

        public string Withdraw(string accountNumber, decimal amount, string comment)
        {
            var account = _bankAccountDao.GetByNumber(accountNumber);

            if (account == null)
            {
                throw new InvalidOperationException($"unknown account: {accountNumber}");
            }

            return account.MakeWithdrawal(amount, DateTime.Now, comment);
        }
    }
}
