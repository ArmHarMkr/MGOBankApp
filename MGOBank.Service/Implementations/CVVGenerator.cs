using MGOBank.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MGOBank.Service.Implementations
{
    public class CVVGenerator : ICVVGenerator
    {
        private Random rnd = new();
        public string GenerateCVV()
        {
            string firsStr = rnd.Next(1, 10).ToString();
            string secondStr = rnd.Next(0, 10).ToString();
            string thirdStr = rnd.Next(0, 10).ToString();
            while(!(firsStr == secondStr | firsStr == thirdStr | secondStr == thirdStr))
            {
                if (firsStr == secondStr | firsStr == thirdStr)
                {
                    firsStr = rnd.Next(1, 10).ToString();
                }
                else if (secondStr == thirdStr)
                {
                    secondStr = rnd.Next(1, 10).ToString();
                }
                else
                {
                    thirdStr = rnd.Next(1, 10).ToString();
                }
            }

            return firsStr + secondStr + thirdStr;
        }
    }
}
