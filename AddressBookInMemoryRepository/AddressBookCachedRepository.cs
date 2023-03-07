using AddressBookModels.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

// Using https://learn.microsoft.com/en-us/dotnet/api/system.runtime.caching.memorycache?view=dotnet-plat-ext-7.0

namespace AddressBookRepositories
{
	public class AddressBookCachedRepository : IAddressBookCachedRepository
	{
		private readonly IAddressBookRepository _innerRepository;
		private MemoryCache _memoryCache = MemoryCache.Default;
		private CacheItemPolicy _policy = new CacheItemPolicy();
		private DateTime cacheRetrievedTime = DateTime.MinValue;

		public AddressBookCachedRepository(IAddressBookRepository innerRepository)
		{
			_innerRepository = innerRepository;
		}

		public bool Add(AddressBookEntry newEntry)
		{
			var itemWasAdded = _memoryCache.Add(newEntry.Name, newEntry.Address, _policy);
			if (itemWasAdded)
			{
				_innerRepository.Add(newEntry);
			}

			return itemWasAdded;
		}

		public bool Delete(string name)
		{
			var itemRemoved = _memoryCache.Remove(name);
			if (itemRemoved != null)
			{
				_innerRepository.Delete(name);
			}

			return (itemRemoved != null);

		}

		public IEnumerable<AddressBookEntry> GetAll()
		{
			return _innerRepository.GetAll();
		}

		public AddressBookEntry? Get(string name)
		{
			try
			{
				var cachedEntry = _memoryCache.Get(name);
				
				if(cachedEntry != null)
				{
					return new AddressBookEntry(name, cachedEntry as string);
				}
				else
				{
					var storedEntry = _innerRepository.Get(name);
					return storedEntry;
				}
			}
			catch (InvalidOperationException) 
			{
				return null;
			}
		}

		public bool Update(AddressBookEntry newEntry)
		{
			var previousAddress = _memoryCache.Get(newEntry.Name);
			if (previousAddress != null && previousAddress.ToString() != newEntry.Address)
			{
				_memoryCache.Set(newEntry.Name, newEntry.Address, _policy);
				_innerRepository.Update(newEntry);
				return true;
			}
			else
			{
				return false; // May want more specific error here for nonexistant vs no update?
			}
		}

		public void InvalidateCache()
		{
			_memoryCache.Trim(100);
		}
	}
}
