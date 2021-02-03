using ExtendedNumerics;
using NUnit.Framework;
using StockCivilizationBot.Exchange;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockCivilizationBot.Testing.Exchange
{
    [TestFixture]
    class TransactionProcessorTests
    {
        Account source;
        Account target;

        Security security1 = new Security(1);
        Security security2 = new Security(2);

        TransactionProcessor processor;

        Transaction TestTransaction;

        [SetUp]
        public void SetUp()
        {
            processor = new TransactionProcessor();
            source = new Account(1);
            source.Portfolio.ForceTransact(security1, new BigRational(10));
            target = new Account(2);
            target.Portfolio.ForceTransact(security2, new BigRational(10));
            TestTransaction = new Transaction(processor.Transactions.GetNextIdentifier(), source, target, security1, new BigRational(10), security2, new BigRational(10));

        }

        [Test]
        public void TransactionProcessorValidTransactionTest()
        {
            
            Assert.IsTrue(processor.TryProcessTransaction(TestTransaction));
            Assert.AreEqual(source.Portfolio.Get(security1), BigRational.Zero);
            Assert.AreEqual(target.Portfolio.Get(security2), BigRational.Zero);
            Assert.AreEqual(source.Portfolio.Get(security2), new BigRational(10));
            Assert.AreEqual(target.Portfolio.Get(security1), new BigRational(10));
        }


        [Test]
        public void TransactionProcessorInvalidBackingTransactionTest()
        {
            var testTransaction = new Transaction(processor.Transactions.GetNextIdentifier(), source, target, security1, new BigRational(10), security2, new BigRational(20));
            Assert.IsFalse(processor.TryProcessTransaction(testTransaction));
            Assert.AreEqual(source.Portfolio.Get(security2), BigRational.Zero);
            Assert.AreEqual(target.Portfolio.Get(security1), BigRational.Zero);
            Assert.AreEqual(source.Portfolio.Get(security1), new BigRational(10));
            Assert.AreEqual(target.Portfolio.Get(security2), new BigRational(10));
        }
        [Test]
        public void TransactionProcessorInvalidTradedTransactionTest()
        {
            var testTransaction = new Transaction(processor.Transactions.GetNextIdentifier(), source, target, security1, new BigRational(20), security2, new BigRational(10));
            Assert.IsFalse(processor.TryProcessTransaction(testTransaction));
            Assert.AreEqual(source.Portfolio.Get(security2), BigRational.Zero);
            Assert.AreEqual(target.Portfolio.Get(security1), BigRational.Zero);
            Assert.AreEqual(source.Portfolio.Get(security1), new BigRational(10));
            Assert.AreEqual(target.Portfolio.Get(security2), new BigRational(10));
        }


        [Test]
        public void TransactionProcessorIsUniqueTest()
        {
            Assert.IsTrue(processor.IsTransactionUnique(TestTransaction));
        }

        [Test]
        public void TransactionProcessorIsAuthorizedTest()
        {
            Assert.IsTrue(processor.IsTransactionAuthorized(TestTransaction));
        }


        [Test]
        public void TransactionProcessorIsPossibleTest()
        {
            Assert.IsTrue(processor.IsTransactionPossible(TestTransaction));
        }
    }
}
