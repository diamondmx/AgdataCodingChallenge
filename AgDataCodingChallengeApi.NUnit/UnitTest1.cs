using AgDataCodingChallengeApi.Controllers;

namespace AgDataCodingChallengeApi.NUnit
{
	public class Tests
	{
		[SetUp]
		public void Setup()
		{
		}

		[Test]
		public void VerifyGetAllReturnsAll()
		{
			
			var sut = new AddressBookController(mockLogger, mockAddressBookRepository);
		}
	}
}