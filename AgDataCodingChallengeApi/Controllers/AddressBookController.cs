using AddressBookModels.DataModels;
using AddressBookRepositories;
using Microsoft.AspNetCore.Mvc;
using System.Buffers;

namespace AgDataCodingChallengeApi.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class AddressBookController : ControllerBase
	{
		private readonly ILogger<AddressBookController> _logger;
		private readonly IAddressBookCachedRepository _addressBookRepository;

		public AddressBookController(ILogger<AddressBookController> logger, IAddressBookCachedRepository addressBookRepository)
		{
			_logger = logger;
			_addressBookRepository = addressBookRepository;
		}

		[HttpGet("GetAll", Name = "GetAll")]
		[ProducesResponseType(200, Type=typeof(IEnumerable<AddressBookEntry>))]
		public IActionResult GetAll()
		{
			return Ok(_addressBookRepository.GetAll());
		}

		[HttpGet("Get", Name = "Get")]
		[ProducesResponseType(200, Type = typeof(AddressBookEntry))]
		public IActionResult Get(string name)
		{
			var foundResult = _addressBookRepository.Get(name);
			if(foundResult != null)
			{
				return Ok(foundResult);
			}
			else
			{

				return NotFound($"Name \"{name}\" was not found in the address book");
			}
		}


		[HttpPut("Add", Name = "Add")]
		public IActionResult AddAddress(string name, string address)
		{
			if(_addressBookRepository.Add(name, address))
			{
				return Created("uri not available", $"{name}:{address}");
			}
			else
			{
				return BadRequest($"Name \"{name}\" already exists or is invalid");
			}
			
		}

		[HttpPost("Update", Name = "Update")]
		public IActionResult UpdateAddress(string name, string newAddress)
		{
			if(_addressBookRepository.Update(name, newAddress))
			{
				return Ok($"{name}:{newAddress}");
			}
			else
			{
				return NotFound($"Name \"{name}\" was not found in the address book");
			}


		}

		[HttpDelete("Delete", Name = "Delete")]
		public IActionResult DeleteAddress(string name)
		{
			if(_addressBookRepository.Delete(name))
			{
				return Ok($"Name \"{name}\" was deleted");
			}
			else
			{
				return BadRequest($"Name \"{name}\" was not found in the address book");
			}
		}
	}
}