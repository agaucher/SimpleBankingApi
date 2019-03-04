using Banking.Dao.Interfaces;
using Banking.Service.Interfaces.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Banking.Dao
{
    public class BankAccountDao<T> : IBankAccountDao
        where T: IBankAccount
    {
        #region Injected Dependencies

        private IData<T> _data;

        public BankAccountDao(IData<T> data)
        {
            _data = data;
        }

        #endregion

        public List<IBankAccount> GetList()
        {
            return _data.BankAccounts.Cast<IBankAccount>().ToList();
        }

        public IBankAccount GetByNumber(string number)
        {
            var account = GetList().Where(x => x.Number == number);

            if (account.Count() > 1)
            {
                throw new InvalidOperationException("duplicated values in database (two accounts have the same number). This case should never occur !");
            }

            return account.FirstOrDefault();
        }
    }
}
