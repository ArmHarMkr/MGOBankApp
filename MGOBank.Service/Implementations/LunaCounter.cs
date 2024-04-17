using MGOBank.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MGOBank.Service.Implementations
{
    public class LunaCounter : ILunaCounter
    {

        public int LunaCounting(string cardNumber)
        {
            int lunaSum = 0;

            int currentDigit;
            for (int i = 0; i < cardNumber.Length; i += 2)
            {
                currentDigit = int.Parse(cardNumber[i].ToString());
                if (currentDigit > 4)
                    lunaSum += currentDigit * 2 - 9;
                else
                    lunaSum += currentDigit * 2;
            }

            for (int i = 1; i < cardNumber.Length; i += 2)
                lunaSum += int.Parse(cardNumber[i].ToString());

            return lunaSum;
        }
    }
}
