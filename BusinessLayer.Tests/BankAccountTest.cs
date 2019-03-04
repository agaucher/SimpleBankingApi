using Banking.Service.Interfaces.Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessLayer.Tests
{
    public class BankAccountTest
    {
        [SetUp]
        public void Setup()
        {
        }

        #region Balance computation

        [Test]
        public void Balance_Is_Zero_For_New_Accounts()
        {
            // Given
            var account = new BankAccount();
            
            // Then
            Assert.AreEqual(0, account.Balance);
        }

        [Test]
        public void Balance_Is_50_When_Only_One_Deposit_Of_50()
        {
            // Given
            var account = new BankAccount();

            // When
            account.MakeDeposit(50, DateTime.Now, string.Empty);

            // Then
            Assert.AreEqual(50, account.Balance);
        }

        [Test]
        public void Balance_Is_The_Sum_Of_Deposit_Minus_Withdrawals()
        {
            // Given
            var account = new BankAccount();

            // When
            account.MakeDeposit(50, DateTime.Now, string.Empty);
            account.MakeWithdrawal(20, DateTime.Now, string.Empty);
            account.MakeDeposit(50, DateTime.Now, string.Empty);

            // Then
            Assert.AreEqual(80, account.Balance);
        }

        #endregion

        #region Make Deposits

        [Test]
        public void MakeDeposit_Throws_When_Amount_Value_Is_A_Negative_Value()
        {
            // Given
            var account = new BankAccount();

            // Then
            var ex = Assert.Throws<ArgumentOutOfRangeException>(() => account.MakeDeposit(-50, DateTime.Now, string.Empty));
            StringAssert.Contains("Amount of deposit must be positive", ex.Message);
        }

        [Test]
        public void MakeDeposit_Returns_Transaction_Id()
        {
            // Given
            var account = new BankAccount();

            // When
            var id = account.MakeDeposit(50, DateTime.Now, string.Empty);

            // Then
            Assert.IsNotEmpty(id);
        }

        [Test]
        public void MakeDeposit_Create_A_New_Transaction_In_History()
        {
            // Given
            var account = new BankAccount();

            // When
            var id = account.MakeDeposit(50, DateTime.Now, string.Empty);

            // Then
            CollectionAssert.Contains(account.History.Select(x => x.Transaction.Id), id);
        }

        #endregion

        #region Make Withdrawal

        [Test]
        public void MakeWithdrawal_Throws_When_Amount_Value_Is_A_Negative_Value()
        {
            // Given
            var account = new BankAccount();

            // Then
            var ex = Assert.Throws<ArgumentOutOfRangeException>(() => account.MakeWithdrawal(-50, DateTime.Now, string.Empty));
            StringAssert.Contains("Amount of withdrawal must be positive", ex.Message);
        }

        [Test]
        public void MakeWithdrawal_Are_Not_Allowed_When_No_Enough_Funds()
        {
            // Given
            var account = new BankAccount();

            // Then
            var ex = Assert.Throws<InvalidOperationException>(() => account.MakeWithdrawal(50, DateTime.Now, string.Empty));
            Assert.AreEqual("Not sufficient funds for this withdrawal", ex.Message);
        }

        [Test]
        public void MakeWithdrawal_Returns_Transaction_Id()
        {
            // Given
            var account = new BankAccount();
            account.MakeDeposit(50, DateTime.Now, string.Empty);

            // When
            var id = account.MakeWithdrawal(50, DateTime.Now, string.Empty);

            // Then
            Assert.IsNotEmpty(id);
        }

        [Test]
        public void MakeWithdrawal_Create_A_New_Transaction_In_History()
        {
            // Given
            var account = new BankAccount();
            account.MakeDeposit(50, DateTime.Now, string.Empty);

            // When
            var id = account.MakeWithdrawal(50, DateTime.Now, string.Empty);

            // Then
            CollectionAssert.Contains(account.History.Select(x => x.Transaction.Id), id);
        }

        #endregion

        #region History

        [Test]
        public void History_Contains_All_Deposits_And_Withdrawal()
        {
            // Given
            var account = new BankAccount();

            // When
            var id1 = account.MakeDeposit(50, DateTime.Today.AddDays(-2), string.Empty);
            var id2 = account.MakeWithdrawal(20, DateTime.Today.AddDays(-1), string.Empty);
            var id3 = account.MakeDeposit(150, DateTime.Now, string.Empty);

            // Then
            Assert.AreEqual(3, account.History.Count());
            CollectionAssert.Contains(account.History.Select(x => x.Transaction.Id), id1);
            CollectionAssert.Contains(account.History.Select(x => x.Transaction.Id), id2);
            CollectionAssert.Contains(account.History.Select(x => x.Transaction.Id), id3);
        }

        [Test]
        public void History_Doesnt_Contain_Invalid_Deposits()
        {
            // Given
            var account = new BankAccount();

            // When
            Assert.Throws<ArgumentOutOfRangeException>(() => account.MakeDeposit(-50, DateTime.Today.AddDays(-2), string.Empty));

            // Then
            Assert.AreEqual(0, account.History.Count());
        }

        [Test]
        public void History_Doesnt_Contain_Invalid_Withdrawal()
        {
            // Given
            var account = new BankAccount();

            // When
            Assert.Throws<ArgumentOutOfRangeException>(() => account.MakeWithdrawal(-50, DateTime.Today.AddDays(-2), string.Empty));

            // Then
            Assert.AreEqual(0, account.History.Count());
        }

        [Test]
        public void History_Contains_Transactions_Ordered_By_Date()
        {
            // Given
            var account = new BankAccount();

            // When
            var id1 = account.MakeDeposit(50, DateTime.Today.AddDays(-3), string.Empty);
            var id2 = account.MakeWithdrawal(20, DateTime.Today.AddDays(-1), string.Empty);
            var id3 = account.MakeDeposit(150, DateTime.Today.AddDays(-2), string.Empty);

            // Then
            Assert.AreEqual(3, account.History.Count());
            Assert.AreEqual(account.History[0].Transaction.Id, id1);
            Assert.AreEqual(account.History[1].Transaction.Id, id3);
            Assert.AreEqual(account.History[2].Transaction.Id, id2);
        }

        [Test]
        public void History_Contains_Balance_For_Each_Transaction()
        {
            // Given
            var account = new BankAccount();

            // When
            var id1 = account.MakeDeposit(50, DateTime.Today.AddDays(-3), string.Empty);
            var id2 = account.MakeWithdrawal(20, DateTime.Today.AddDays(-1), string.Empty);
            var id3 = account.MakeDeposit(150, DateTime.Today.AddDays(-2), string.Empty);

            // Then
            Assert.AreEqual(3, account.History.Count());
            Assert.AreEqual(account.History[0].NewBalanceAfterTransaction, 50);
            Assert.AreEqual(account.History[1].NewBalanceAfterTransaction, 200);
            Assert.AreEqual(account.History[2].NewBalanceAfterTransaction, 180);
        }

        #endregion
    }
}
