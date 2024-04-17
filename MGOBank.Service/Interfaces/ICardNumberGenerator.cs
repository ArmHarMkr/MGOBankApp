using MGOBank.Service.Implementations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace MGOBank.Service.Interfaces
{
	public interface ICardNumberGenerator
	{
		public string GenerateCardNumber(string paymentSystem);
	}
}
