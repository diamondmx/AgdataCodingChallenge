using AddressBookModels.DataModels;

namespace AddressBookRepositories
{ 
	public class AddressBookInMemoryRepository : IAddressBookRepository
	{
		public Dictionary<string, string> AddressBook { get; set; } = new Dictionary<string, string>()
		{
			//{"Mark", "North" },
			//{"Steve", "South" },
			//{"Erin", "East" },
			//{"Wesley", "West" }
		};

		public IEnumerable<AddressBookEntry> GetAll()
		{
			return AddressBook.Select(entry=>new AddressBookEntry(entry.Key, entry.Value));
		}

		public bool Add(AddressBookEntry newEntry)
		{
			return AddressBook.TryAdd(newEntry.Name, newEntry.Address);
		}

		public bool Delete(string name)
		{ 			
			return AddressBook.Remove(name);
		}

		public bool Update(AddressBookEntry newEntry)
		{
			if (AddressBook.GetValueOrDefault(newEntry.Name)!= null)
			{
				AddressBook[newEntry.Name] = newEntry.Address;
				return true;
			}
			else
			{
				return false;
			}	
		}

		public AddressBookEntry Get(string name)
		{
				var foundEntry = AddressBook.First(entry => entry.Key == name);
				return new AddressBookEntry(foundEntry.Key, foundEntry.Value);	
		}
	}
}