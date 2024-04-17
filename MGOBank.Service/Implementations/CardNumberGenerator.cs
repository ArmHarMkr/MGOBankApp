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
		private LunaCounter _counter;
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
			for (int i = 0; i<12; i++)
			{
				generatedCard += randomDigit.Next(10).ToString();
			}

			int lunaSum = _counter.LunaCounting(generatedCard);
			return generatedCard;
		}
	}
}
