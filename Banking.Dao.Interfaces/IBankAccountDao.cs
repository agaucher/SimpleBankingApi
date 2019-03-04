using Banking.Service.Interfaces.Models;
using System;
using System.Collections.Generic;

namespace Banking.Dao.Interfaces
{
    public interface IBankAccountDao
    {
        List<IBankAccount> GetList();

        IBankAccount GetByNumber(string number);
    }
}
