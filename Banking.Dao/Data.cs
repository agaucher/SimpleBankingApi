using Banking.Dao.Interfaces;
using Banking.Dao.Interfaces.Dtos;
using Banking.Service.Interfaces.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Banking.Dao
{
    public class Data<T> : IData<T>
    {
        private static List<BankAccountDto> _existingAccounts = new List<BankAccountDto>
        {
            new BankAccountDto { Number = Guid.NewGuid().ToString(), },
            new BankAccountDto { Number = Guid.NewGuid().ToString(), },
            new BankAccountDto { Number = Guid.NewGuid().ToString(), },
            new BankAccountDto { Number = Guid.NewGuid().ToString(), },
            new BankAccountDto { Number = Guid.NewGuid().ToString(), },
        };

        private List<T> _bankAccounts = _existingAccounts.Select(x =>
        {
            var constructor = typeof(T).GetConstructor(new[] { typeof(BankAccountDto) });
            return constructor.Invoke(new[] { x });
        }).Cast<T>().ToList();

        public List<T> BankAccounts
            => _bankAccounts;
    }
}
