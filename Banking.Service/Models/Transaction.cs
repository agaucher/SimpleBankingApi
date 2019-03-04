using System;
using System.Collections.Generic;
using System.Text;

namespace Banking.Service.Interfaces.Models
{
    public class Transaction : ITransaction
    {
        public string Id { get; }
        public decimal Amount { get; }
        public DateTime Date { get; }
        public string Comment { get; }

        public Transaction(decimal amount, DateTime date, string comment)
        {
            this.Id = Guid.NewGuid().ToString();
            this.Amount = amount;
            this.Date = date;
            this.Comment = comment ?? string.Empty;
        }
    }
}
