using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using LearningMoq;
using Moq;

namespace UnitTests
{
    [TestClass]
    public class CreditCardApplicationEvaluatorShould
    {
        [TestMethod]
        public void AutoAcceptHighIncomeApplication()
        {
            //Create Mock object and introduce that as a DI to the CreditCardApplicationEvaluator class
            Mock<IFrequentFlierNumberService> mockValidator 
                = new Mock<IFrequentFlierNumberService>();

            //var sut = new CreditCardApplicationEvaluator(null); //Instead of sending null to the constructor, we send the mock objects
            var sut = new CreditCardApplicationEvaluator(mockValidator.Object); //Instead of sending null to the constructor, we send the mock objects

            var application = new CreditCardApplication { GrossSalary = 100000 };
            CreditCardApplicationDecision decision = sut.Evaluate(application);
            Assert.AreEqual(CreditCardApplicationDecision.AutoAccepted, decision);
        }


        [TestMethod]
        public void AutoDeclineLowIncomeApplication()
        {
            //Create Mock object and introduce that as a DI to the CreditCardApplicationEvaluator class
            Mock<IFrequentFlierNumberService> mockValidator
                = new Mock<IFrequentFlierNumberService>();

            //Setting up values for the FrequentFlierNumber Property
            mockValidator.Setup(x => x.isValid("x")).Returns(true);  

            //var sut = new CreditCardApplicationEvaluator(null); //Instead of sending null to the constructor, we send the mock objects
            var sut = new CreditCardApplicationEvaluator(mockValidator.Object); //Instead of sending null to the constructor, we send the mock objects

            var application = new CreditCardApplication { GrossSalary = 10000, Age = 42, FrequentFlierNumber = "x"};

            CreditCardApplicationDecision decision = sut.Evaluate(application);
            Assert.AreEqual(CreditCardApplicationDecision.AutoDeclined, decision);
        }

        [TestMethod]
        public void ReferHumanFrequentFlier()
        {
            //Create Mock object and introduce that as a DI to the CreditCardApplicationEvaluator class
            Mock<IFrequentFlierNumberService> mockValidator
                = new Mock<IFrequentFlierNumberService>();

            mockValidator.Setup(invalidfreqFlier => invalidfreqFlier.isValid("invalid")).Returns(false);

            //var sut = new CreditCardApplicationEvaluator(null); //Instead of sending null to the constructor, we send the mock objects
            var sut = new CreditCardApplicationEvaluator(mockValidator.Object); //Instead of sending null to the constructor, we send the mock objects

            var application = new CreditCardApplication { GrossSalary = 10000, FrequentFlierNumber = "invalid" };
            CreditCardApplicationDecision decision = sut.Evaluate(application);
            Assert.AreEqual(CreditCardApplicationDecision.ReferredToHuman, decision);
        }
    }
}
