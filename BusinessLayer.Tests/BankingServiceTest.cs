using Banking.Dao.Interfaces;
using Banking.Service;
using Banking.Service.Interfaces;
using Banking.Service.Interfaces.Models;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessLayer.Tests
{
    public class BankingServiceTest
    {
        private IBankingService _bankingService;

        private Mock<IBankAccountDao> _bankAccountDao;

        [SetUp]
        public void Setup()
        {
            _bankAccountDao = new Mock<IBankAccountDao>();

            _bankingService = new BankingService(_bankAccountDao.Object);
        }

        #region GetExistingAccounts

        [Test]
        public void GetExistingAccounts_Returns_Values_Retrived_From_Dal()
        {
            // Given
            _bankAccountDao.Setup(x => x.GetList()).Returns(new List<IBankAccount>
            {
                Mock.Of<IBankAccount>(x => x.Number == "account1"),
                Mock.Of<IBankAccount>(x => x.Number == "account2"),
            });

            // When
            var result = _bankingService.GetExistingAccounts();

            // Then
            Assert.AreEqual(2, result.Count());
            Assert.AreEqual("account1", result[0].Number);
            Assert.AreEqual("account2", result[1].Number);
        }

        #endregion

        #region IsExistingAccount

        [Test]
        public void IsExistingAccount_Returns_False_When_Account_Doesnt_Exist()
        {
            // Given
            _bankAccountDao.Setup(x => x.GetByNumber("account1")).Returns((IBankAccount)null);

            // When
            var result = _bankingService.IsExistingAccount("account321");

            // Then
            Assert.IsFalse(result);
        }

        [Test]
        public void IsExistingAccount_Returns_True_When_Account_Exists()
        {
            // Given
            _bankAccountDao.Setup(x => x.GetByNumber("account1")).Returns(Mock.Of<IBankAccount>(x => x.Number == "account1"));

            // When
            var result = _bankingService.IsExistingAccount("account1");

            // Then
            Assert.IsTrue(result);
        }

        #endregion

        #region GetBalance

        [Test]
        public void GetBalance_Throws_Exception_When_Account_Doesnt_Exist()
        {
            // Given
            _bankAccountDao.Setup(x => x.GetByNumber("account1")).Returns((IBankAccount) null);

            // Then
            var ex = Assert.Throws<InvalidOperationException>(() => _bankingService.GetBalance("account1"));
            Assert.AreEqual("unknown account: account1", ex.Message);
        }

        [Test]
        public void GetBalance_Returns_Account_Balance_When_Account_Exists()
        {
            // Given
            _bankAccountDao.Setup(x => x.GetByNumber("account1")).Returns(Mock.Of<IBankAccount>(x => x.Number == "account1" && x.Balance == 500));

            // When
            var result = _bankingService.GetBalance("account1");

            // Then
            Assert.AreEqual(500, result);
        }

        #endregion

        #region GetHistory

        [Test]
        public void GetHistory_Throws_Exception_When_Account_Doesnt_Exist()
        {
            // Given
            _bankAccountDao.Setup(x => x.GetByNumber("account1")).Returns((IBankAccount) null);

            // Then
            var ex = Assert.Throws<InvalidOperationException>(() => _bankingService.GetHistory("account1"));
            Assert.AreEqual("unknown account: account1", ex.Message);
        }

        [Test]
        public void GetHistory_Returns_Account_History_When_Account_Exists()
        {
            // Given
            var transactionHistory = new List<ITransactionHistory>();
            _bankAccountDao.Setup(x => x.GetByNumber("account1")).Returns(Mock.Of<IBankAccount>(x => x.Number == "account1" && x.History == transactionHistory));

            // When
            var result = _bankingService.GetHistory("account1");

            // Then
            Assert.AreSame(transactionHistory, result);
        }

        #endregion

        #region Deposit

        [Test]
        public void Deposit_Throws_Exception_When_Account_Doesnt_Exist()
        {
            // Given
            _bankAccountDao.Setup(x => x.GetByNumber("account1")).Returns((IBankAccount)null);

            // Then
            var ex = Assert.Throws<InvalidOperationException>(() => _bankingService.Deposit("account1", It.IsAny<decimal>(), It.IsAny<string>()));
            Assert.AreEqual("unknown account: account1", ex.Message);
        }

        [Test]
        public void Deposit_Returns_TransactionId_When_Account_Exists()
        {
            // Given
            var bankAccount = new Mock<IBankAccount>();
            bankAccount.Setup(x => x.MakeDeposit(It.IsAny<decimal>(), It.IsAny<DateTime>(), It.IsAny<string>())).Returns("transactionId");

            _bankAccountDao.Setup(x => x.GetByNumber("account1")).Returns(bankAccount.Object);

            // When
            var result = _bankingService.Deposit("account1", 50, "comment");

            // Then
            Assert.AreEqual("transactionId", result);
        }

        #endregion

        #region Withdraw

        [Test]
        public void Withdraw_Throws_Exception_When_Account_Doesnt_Exist()
        {
            // Given
            _bankAccountDao.Setup(x => x.GetByNumber("account1")).Returns((IBankAccount)null);

            // Then
            var ex = Assert.Throws<InvalidOperationException>(() => _bankingService.Withdraw("account1", It.IsAny<decimal>(), It.IsAny<string>()));
            Assert.AreEqual("unknown account: account1", ex.Message);
        }

        [Test]
        public void Withdraw_Returns_TransactionId_When_Account_Exists()
        {
            // Given
            var bankAccount = new Mock<IBankAccount>();
            bankAccount.Setup(x => x.MakeWithdrawal(It.IsAny<decimal>(), It.IsAny<DateTime>(), It.IsAny<string>())).Returns("transactionId");

            _bankAccountDao.Setup(x => x.GetByNumber("account1")).Returns(bankAccount.Object);

            // When
            var result = _bankingService.Withdraw("account1", 50, "comment");

            // Then
            Assert.AreEqual("transactionId", result);
        }

        #endregion
    }
}
