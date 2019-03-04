using Banking.Service.Interfaces.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Banking.Dao.Interfaces
{
    public interface IData<T>
    {
        List<T> BankAccounts { get; }
    }
}
