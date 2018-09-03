using System;
using System.Collections.Generic;
using System.Text;

namespace LearningMoq
{
    public class FrequentFlierNumberService : IFrequentFlierNumberService
    {
        public string LicenseKey => throw new NotImplementedException();

        public bool isValid(string frequentFlierNumber)
        {
            throw new NotImplementedException();
        }

        public void isValid(string frequentFlierNumber, out bool isValid)
        {
            throw new NotImplementedException();
        }

    }
}
