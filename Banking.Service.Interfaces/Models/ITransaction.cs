using System;
using System.Collections.Generic;
using System.Text;

namespace Banking.Service.Interfaces.Models
{
    public interface ITransaction
    {
        string Id { get; }

        decimal Amount { get; }

        DateTime Date { get; }

        string Comment { get; }
    }
}
