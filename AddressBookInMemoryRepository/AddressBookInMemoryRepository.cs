namespace AddressBookRepositories
{
	public interface IAddressBookRepository
	{
		Dictionary<string, string> GetAll();
		bool Add(string name, string address);
	}
	public class AddressBookInMemoryRepository : IAddressBookRepository
	{
		public Dictionary<string, string> AddressBook { get; set; } = new Dictionary<string, string>()
		{
			{"Mark", "North" },
			{"Steve", "South" },
			{"Erin", "East" },
			{"Wesley", "West" }
		};

		public Dictionary<string, string> GetAll()
		{
			return AddressBook;
		}

		public bool Add(string name, string address)
		{
			return AddressBook.TryAdd(name, address);
		}
	}
}