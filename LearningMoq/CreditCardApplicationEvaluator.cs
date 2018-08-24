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
            this._validator = validator;
        }

        public CreditCardApplicationDecision Evaluate(CreditCardApplication creditCardApplication)
        {
            if (creditCardApplication.GrossSalary >= HighIncomeThreshold)
            {
                return CreditCardApplicationDecision.AutoAccepted;
            }

            if (creditCardApplication.GrossSalary < LowIncomeThreshold)
            {
                return CreditCardApplicationDecision.AutoDeclined;
            }

            if (_validator.isValid(creditCardApplication.FrequentFlierNumber))
            {
                return CreditCardApplicationDecision.ReferredToHuman;
            }

            if (creditCardApplication.Age < AutoReferralAge)
            {
                return CreditCardApplicationDecision.ReferredToHuman;
            }

            return CreditCardApplicationDecision.ReferredToHuman;
        }
    }
}
