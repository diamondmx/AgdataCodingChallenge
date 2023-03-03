using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AddressBookRepositories
{
	public interface IAddressBookRepository
	{
		Dictionary<string, string> GetAll();
		bool Add(string name, string address);
		bool Update(string name, string newAddress);
		bool Delete(string name);
	}
}
