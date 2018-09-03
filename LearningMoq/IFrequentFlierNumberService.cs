using System;
using System.Collections.Generic;
using System.Text;

namespace LearningMoq
{
    public interface IFrequentFlierNumberService
    {
        bool isValid(string frequentFlierNumber);
        void isValid(string frequentFlierNumber,out bool isValid);
        string LicenseKey { get; }
    }
}
