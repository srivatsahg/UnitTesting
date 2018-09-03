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

            var application = new CreditCardApplication
            {
                GrossSalary = 10000,
                Age = 42,
                FrequentFlierNumber = "x"
            };

            CreditCardApplicationDecision decision = sut.Evaluate(application);
            Assert.AreEqual(CreditCardApplicationDecision.AutoDeclined, decision);
        }

        [TestMethod]
        public void AutoDeclineLowIncomeApplicationUsingCustomizedArgumentMatchingMethod()
        {
            //Create Mock object and introduce that as a DI to the CreditCardApplicationEvaluator class
            Mock<IFrequentFlierNumberService> mockValidator
                = new Mock<IFrequentFlierNumberService>();

            //Using the It class

            //Setting up values for the FrequentFlierNumber Property
            //mockValidator.Setup(x => x.isValid(It.IsNotNull<string>())).Returns(true);

            //Setting the frequent flier number within the specified range
            mockValidator.Setup(x => x.isValid(It.IsInRange<string>("a","f",Range.Inclusive))).Returns(true);


            //var sut = new CreditCardApplicationEvaluator(null); //Instead of sending null to the constructor, we send the mock objects
            var sut = new CreditCardApplicationEvaluator(mockValidator.Object); //Instead of sending null to the constructor, we send the mock objects

            var application = new CreditCardApplication
            {
                GrossSalary = 10000,
                Age = 42,
                FrequentFlierNumber = "b"
            };

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

        /// <summary>
        /// Strict Mock
        /// </summary>
        [TestMethod]
        public void ReferInvalidFrequentFlierApplication()
        {
            Mock<IFrequentFlierNumberService> mockValidator 
                = new Mock<IFrequentFlierNumberService>(MockBehavior.Strict);

            mockValidator.Setup(x => x.isValid(It.IsAny<string>())).Returns(false);//if this is commented, Strict Mock will raise an exception

            var sut = new CreditCardApplicationEvaluator(mockValidator.Object);

            var application = new CreditCardApplication();

            CreditCardApplicationDecision decision = sut.Evaluate(application);

            Assert.AreEqual(CreditCardApplicationDecision.ReferredToHuman, decision);
        }

        [TestMethod]
        public void ReferInvalidFrequentFlierApplicationUsingOut()
        {
            //Step 1. Creation of Strict Mocks
            Mock<IFrequentFlierNumberService> mockValidator
                = new Mock<IFrequentFlierNumberService>(MockBehavior.Strict);

            //Step 2. Setting up mocks arguments, using out keyword
            bool isPremiumCustomer = false;
            mockValidator.Setup(x => x.isValid(It.IsAny<string>(),out isPremiumCustomer));

            //Step 3. Call overloaded methods in the mock
            var sut = new CreditCardApplicationEvaluator(mockValidator.Object);
            var application = new CreditCardApplication() { Age = 19 };
            CreditCardApplicationDecision decision = sut.EvaluateUsingOut(application);

            //Step 4. Validate the test
            Assert.AreEqual(CreditCardApplicationDecision.ReferredToHuman, decision);
        }

        [TestMethod]
        public void ReferHumanWhenLicenseExpired()
        {
            //Step 1. Creation of Strict Mocks
            Mock<IFrequentFlierNumberService> mockValidator
                = new Mock<IFrequentFlierNumberService>(MockBehavior.Strict);

            //Step 2. Setting up mocks arguments, using using mock properties
            bool isPremiumCustomer = true;
            mockValidator.Setup(x => x.isValid(It.IsAny<string>(), out isPremiumCustomer));
            mockValidator.Setup(x => x.LicenseKey).Returns("EXPIRED");

            //Step 3. Call overloaded methods in the mock
            var sut = new CreditCardApplicationEvaluator(mockValidator.Object);

            var application = new CreditCardApplication() { Age = 42 };

            CreditCardApplicationDecision decision = sut.EvaluateUsingOut(application);

            //Step 4. Validate the test
            Assert.AreEqual(CreditCardApplicationDecision.ReferredToHuman, decision);
        }
    }
}
