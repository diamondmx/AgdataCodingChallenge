using Microsoft.AspNetCore.Mvc;

namespace AgDataCodingChallengeApi.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class AddressBookController : ControllerBase
	{
		public Dictionary<string, string> AddressBook { get; set; } = new Dictionary<string, string>()
		{
			//{"Mark", "North" },
			//{"Steve", "South" },
			//{"Erin", "East" },
			//{"Wesley", "West" }
		};

		private readonly ILogger<AddressBookController> _logger;

		public AddressBookController(ILogger<AddressBookController> logger)
		{
			_logger = logger;
		}

		[HttpGet(Name = "GetAll")]
		public IEnumerable<Tuple<string, string>> Get()
		{
			return AddressBook.Select(entry=>new Tuple<string, string>(entry.Key, entry.Value)).ToArray();
		}

		[HttpPost(Name = "Add")]
		public bool AddAddress(string name, string address)
		{
			return AddressBook.TryAdd(name, address);
		}
	}
}