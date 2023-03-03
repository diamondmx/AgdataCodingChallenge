﻿namespace AddressBookRepositories
{ 
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

		public bool Delete(string name)
		{
			return AddressBook.Remove(name);
		}

		public bool Update(string name, string newAddress)
		{
			if (AddressBook.GetValueOrDefault(name)!= null)
			{
				AddressBook[name] = newAddress;
				return true;
			}
			else
			{
				return false;
			}
			
			
		}

	}
}