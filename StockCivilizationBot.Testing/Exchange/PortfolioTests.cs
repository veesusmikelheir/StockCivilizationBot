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
    class PortfolioTests
    {
        Portfolio first;
        Portfolio second;
        Security TestSecurity1 = new Security(1);
        Security TestSecurity2 = new Security(2);
        Security TestSecurity3 = new Security(3);
        Security TestSecurity4 = new Security(4);

        [SetUp]
        public void SetUp()
        {
            first = new Portfolio(new Dictionary<Security, BigRational>());
            second = new Portfolio(new Dictionary<Security, BigRational>());
            
        }

        [Test]
        public void PortfolioGetNonExistentTest()
        {
            Assert.IsTrue(first.Get(TestSecurity1).IsZero);
        }

        [Test]
        public void PortfolioGetExistentTest()
        {
            first = new Portfolio(new Dictionary<Security, BigRational>() { { TestSecurity1, new BigRational(10) } });
            Assert.AreEqual(first.Get(TestSecurity1),new BigRational(10));
        }

        [Test]
        public void PortfolioTransactNonExistentTest()
        {
            first.Transact(TestSecurity1, new BigRational(10));
            Assert.AreEqual(first.Get(TestSecurity1), new BigRational(10));
        }
        [Test]
        public void PortfolioTransactExistentTest()
        {
            first = new Portfolio(new Dictionary<Security, BigRational>() { { TestSecurity1, new BigRational(10) } });
            first.Transact(TestSecurity1, new BigRational(-10));
            Assert.AreEqual(first.Get(TestSecurity1), BigRational.Zero);

        }

        [Test]
        public void PortfolioTransactExistentPartialTest()
        {
            first = new Portfolio(new Dictionary<Security, BigRational>() { { TestSecurity1, new BigRational(10) } });
            first.Transact(TestSecurity1, new BigRational(-5));
            Assert.AreEqual(first.Get(TestSecurity1), new BigRational(5));

        }

        [Test]
        public void PortfolioAttemptSuccessfulTransactionTest()
        {
            first.Transact(TestSecurity1, new BigRational(10));
            Assert.IsTrue(first.AttemptTransact(TestSecurity1, new BigRational(-10), out var amount));
            Assert.AreEqual(amount, BigRational.Zero);

        }

        [Test]
        public void PortfolioAttemptSuccessfulPartialTransactionTest()
        {
            first.Transact(TestSecurity1, new BigRational(10));
            Assert.IsTrue(first.AttemptTransact(TestSecurity1, new BigRational(-5), out var amount));
            Assert.AreEqual(amount, new BigRational(5));
        }

        [Test]
        public void PortfolioUnsuccessfulTransactionTest()
        {
            first.Transact(TestSecurity1, new BigRational(10));
            Assert.IsFalse(first.AttemptTransact(TestSecurity1, new BigRational(-11), out var amount));
            Assert.AreEqual(amount,new BigRational(10));
        }

        [Test]
        public void PortfolioSuccessfulTradeTest()
        {
            first.Transact(TestSecurity1, new BigRational(10));
            Assert.IsTrue(Portfolio.TryTrade(TestSecurity1, first, second, new BigRational(10), out var firstAmount, out var secondAmount));
            Assert.AreEqual(firstAmount, BigRational.Zero);
            Assert.AreEqual(secondAmount, new BigRational(10));

        }

        [Test]
        public void PortfolioSuccessfulPartialTradeTest()
        {
            first.Transact(TestSecurity1, new BigRational(10));
            Assert.IsTrue(Portfolio.TryTrade(TestSecurity1, first, second, new BigRational(5), out var firstAmount, out var secondAmount));
            Assert.AreEqual(firstAmount, new BigRational(5));
            Assert.AreEqual(secondAmount, new BigRational(5));

        }

        [Test]
        public void PortfolioUnsuccessfulPartialTradeTest()
        {
            first.Transact(TestSecurity1, new BigRational(10));
            Assert.IsFalse(Portfolio.TryTrade(TestSecurity1, first, second, new BigRational(11), out var firstAmount, out var secondAmount));
            Assert.AreEqual(firstAmount, new BigRational(10));
            Assert.AreEqual(secondAmount, BigRational.Zero);

        }

        [Test]
        public void PortfolioExceptionTradeTest()
        {

            Assert.Throws<ArgumentOutOfRangeException>(()=>Portfolio.TryTrade(TestSecurity1, first, second, new BigRational(-11), out var firstAmount, out var secondAmount));


        }
    }
}
