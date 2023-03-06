using AddressBookModels.DataModels;

namespace AddressBookRepositories
{
	public interface IAddressBookCachedRepository
	{
		bool Add(string name, string address);
		bool Delete(string name);
		IEnumerable<AddressBookEntry> GetAll();
		AddressBookEntry Get(string name);
		bool Update(string name, string newAddress);
	}
}