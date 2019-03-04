using Banking.Dao.Interfaces.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Banking.Service.Interfaces.Models
{
    public class BankAccount : IBankAccount
    {
        public string Number { get; }

        private List<ITransaction> _transactions = new List<ITransaction>();

        public BankAccount() { }

        public BankAccount(BankAccountDto dto)
        {
            Number = dto.Number;
        }

        public decimal Balance
            => _transactions.Sum(x => x.Amount);

        public List<ITransactionHistory> History
        {
            get
            {
                var orderedTransactions = _transactions.OrderBy(x => x.Date);
                var result = new List<ITransactionHistory>();

                decimal balance = 0;
                foreach (var transaction in orderedTransactions)
                {
                    balance += transaction.Amount;
                    result.Add(new TransactionHistory { Transaction = transaction, NewBalanceAfterTransaction = balance });
                }

                return result;
            }
        }

        public string MakeDeposit(decimal amount, DateTime date, string comment)
        {
            if (amount < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(amount), "Amount of deposit must be positive");
            }
            var deposit = new Transaction(amount, date, comment);
            _transactions.Add(deposit);

            return deposit.Id;
        }

        public string MakeWithdrawal(decimal amount, DateTime date, string comment)
        {
            if (amount <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(amount), "Amount of withdrawal must be positive");
            }
            if (Balance - amount < 0)
            {
                throw new InvalidOperationException("Not sufficient funds for this withdrawal");
            }
            var withdrawal = new Transaction(-amount, date, comment);
            _transactions.Add(withdrawal);

            return withdrawal.Id;
        }
    }
}
