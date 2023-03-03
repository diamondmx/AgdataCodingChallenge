using AddressBookRepositories;
using Microsoft.AspNetCore.Mvc;

namespace AgDataCodingChallengeApi.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class AddressBookController : ControllerBase
	{
		private readonly ILogger<AddressBookController> _logger;
		private readonly IAddressBookRepository _addressBookRepository;

		public AddressBookController(ILogger<AddressBookController> logger, IAddressBookRepository addressBookRepository)
		{
			_logger = logger;
			_addressBookRepository = addressBookRepository;
		}

		[HttpGet("GetAll", Name = "GetAll")]
		public IEnumerable<Tuple<string, string>> Get()
		{
			return _addressBookRepository.GetAll().Select(entry=>new Tuple<string, string>(entry.Key, entry.Value)).ToArray();
		}

		[HttpPut("Add", Name = "Add")]
		public bool AddAddress(string name, string address)
		{ 
			return _addressBookRepository.Add(name, address);
		}

		[HttpPost("Update", Name = "Update")]
		public bool UpdateAddress(string name, string newAddress)
		{
			return _addressBookRepository.Update(name, newAddress);
		}

		[HttpDelete("Delete", Name = "Delete")]
		public bool DeleteAddress(string name)
		{
			return _addressBookRepository.Delete(name);
		}
	}
}