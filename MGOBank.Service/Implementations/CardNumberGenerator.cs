using MGOBank.Service.Interfaces;
using MGOBankApp.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MGOBank.Service.Implementations
{
	public class CardNumberGenerator : ICardNumberGenerator 
	{
		private LunaCounter _counter = new();
		public string GenerateCardNumber(string paymentSystem)
		{
			paymentSystem = paymentSystem.ToLower();
			string generatedCard = "";
			switch (paymentSystem)
			{
				case "visa":
					generatedCard += "4";
					break;
				case "arca":
					generatedCard += "9";
					break;
				case "mastercard":
					generatedCard += "5";
					break;
				case "americanexpress":
					generatedCard += "3";
					break;
			}
			//our bank number
			generatedCard += "077";
			Random randomDigit = new Random();
			for (int i = 0; i < 11; i++)
			{
				generatedCard += randomDigit.Next(10).ToString();
			}

			int lunaSum = _counter.LunaCounting(generatedCard);
			if (lunaSum % 10 != 0)
				generatedCard += (10 - lunaSum % 10).ToString();
			else
				generatedCard += "0";
			return generatedCard;
		}
	}
}
