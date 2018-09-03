using System;
using System.Collections.Generic;
using System.Text;

namespace LearningMoq
{
    public class CreditCardApplicationEvaluator
    {
        private const int AutoReferralAge = 20;
        private const int HighIncomeThreshold = 100000;
        private const int LowIncomeThreshold = 20000;

        IFrequentFlierNumberService _validator;

        public CreditCardApplicationEvaluator(IFrequentFlierNumberService validator)
        {
            this._validator = validator ?? throw new ArgumentNullException(nameof(validator));
        }

        public CreditCardApplicationDecision Evaluate(CreditCardApplication creditCardApplication)
        {
            if (creditCardApplication.GrossSalary >= HighIncomeThreshold)
            {
                return CreditCardApplicationDecision.AutoAccepted;
            }
            
            //This results in NRE
            if (!_validator.isValid(creditCardApplication.FrequentFlierNumber))
            {
                return CreditCardApplicationDecision.ReferredToHuman;
            }

            if(_validator.LicenseKey == "EXPIRED")
            {
                return CreditCardApplicationDecision.ReferredToHuman;
            }

            if (creditCardApplication.Age < AutoReferralAge)
            {
                return CreditCardApplicationDecision.ReferredToHuman;
            }

            if (creditCardApplication.GrossSalary < LowIncomeThreshold)
            {
                return CreditCardApplicationDecision.AutoDeclined;
            }

            return CreditCardApplicationDecision.ReferredToHuman;
        }

        public CreditCardApplicationDecision EvaluateUsingOut(CreditCardApplication creditCardApplication)
        {
            if (creditCardApplication.GrossSalary >= HighIncomeThreshold)
            {
                return CreditCardApplicationDecision.AutoAccepted;
            }

            //using the overloaded method (out) keyword
            _validator.isValid(creditCardApplication.FrequentFlierNumber, out var isValidFrequentFlier);
            if (!isValidFrequentFlier)
            {
                return CreditCardApplicationDecision.ReferredToHuman;
            }

            if (_validator.LicenseKey == "EXPIRED")
            {
                return CreditCardApplicationDecision.ReferredToHuman;
            }

            if (creditCardApplication.Age < AutoReferralAge)
            {
                return CreditCardApplicationDecision.ReferredToHuman;
            }

            if (creditCardApplication.GrossSalary < LowIncomeThreshold)
            {
                return CreditCardApplicationDecision.AutoDeclined;
            }

            return CreditCardApplicationDecision.ReferredToHuman;
        }
    }
}
