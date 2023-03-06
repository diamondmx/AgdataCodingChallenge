﻿using AddressBookModels.DataModels;
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

		public AddressBookCachedRepository(IAddressBookRepository innerRepository) {
			_innerRepository = innerRepository;
		}

		public bool Add(string name, string address)
		{
			var itemWasAdded = _memoryCache.Add(name, address, _policy);
			if (itemWasAdded)
			{
				_innerRepository.Add(name, address);
			}

			return itemWasAdded;
		}

		public bool Delete(string name)
		{
			var itemRemoved = _memoryCache.Remove(name);
			if (itemRemoved!=null)
			{
				_innerRepository.Delete(name);
			}

			return (itemRemoved != null);

		}

		public IEnumerable<AddressBookEntry> GetAll()
		{
			return _memoryCache.Select(cacheItem => new AddressBookEntry(cacheItem.Key, cacheItem.Value as string));
		}

		public bool Update(string name, string newAddress)
		{
			var previousAddress = _memoryCache.Get(name);
			if (previousAddress!=null && previousAddress.ToString() != newAddress)
			{
				_memoryCache.Set(name, newAddress, _policy);
				_innerRepository.Update(name, newAddress);
				return true;
			}
			else
			{
				return false; // May want more specific error here for nonexistant vs no update?
			}
		}
	}
}
