using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using LearningMoq;

namespace UnitTests
{
    [TestClass]
    public class CreditCardApplicationEvaluatorShould
    {
        [TestMethod]
        public void AutoAcceptHighIncomeApplication()
        {
            var sut = new CreditCardApplicationEvaluator(); //Instead of sending null to the constructor, we send the mock objects
            var application = new CreditCardApplication { GrossSalary = 100000 };
            CreditCardApplicationDecision decision = sut.Evaluate(application);
            Assert.AreEqual(CreditCardApplicationDecision.AutoAccepted, decision);
        }


        [TestMethod]
        public void AutoDeclineLowIncomeApplication()
        {
            var sut = new CreditCardApplicationEvaluator(); //Instead of sending null to the constructor, we send the mock objects
            var application = new CreditCardApplication { GrossSalary = 10000 };
            CreditCardApplicationDecision decision = sut.Evaluate(application);
            Assert.AreEqual(CreditCardApplicationDecision.AutoDeclined, decision);
        }

        [TestMethod]
        public void AutoReferHumanFrequentFlier()
        {
            var sut = new CreditCardApplicationEvaluator(); //Instead of sending null to the constructor, we send the mock objects
            var application = new CreditCardApplication { GrossSalary = 10000 };
            CreditCardApplicationDecision decision = sut.Evaluate(application);
            Assert.AreEqual(CreditCardApplicationDecision.AutoDeclined, decision);
        }
    }
}
