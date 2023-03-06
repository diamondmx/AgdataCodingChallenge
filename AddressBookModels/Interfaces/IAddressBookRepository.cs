using AddressBookModels.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AddressBookRepositories
{
	public interface IAddressBookRepository
	{
		IEnumerable<AddressBookEntry> GetAll();
		bool Add(AddressBookEntry newEntry);
		bool Update(AddressBookEntry newEntry);
		bool Delete(string name);
		AddressBookEntry Get(string name);
	}
}
