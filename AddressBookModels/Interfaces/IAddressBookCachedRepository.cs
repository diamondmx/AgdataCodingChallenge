using AddressBookModels.DataModels;

namespace AddressBookRepositories
{
	public interface IAddressBookCachedRepository
	{
		bool Add(AddressBookEntry newEntry);
		bool Delete(string name);
		IEnumerable<AddressBookEntry> GetAll();
		AddressBookEntry? Get(string name);
		bool Update(AddressBookEntry newEntry);
	}
}