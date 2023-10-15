using System;
using System.Collections.Generic;
using System.Text;

namespace TellDontAskKata.Main.Helpers
{
    public static class DecimalHelper
    {
        public static decimal RoundToPositiveInfinity(this decimal amount) => decimal.Round(amount, 2, MidpointRounding.ToPositiveInfinity);
    }
}
