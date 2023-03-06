using AddressBookModels.DataModels;
using AddressBookRepositories;
using AgDataCodingChallengeApi.Controllers;
using Castle.Core.Logging;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Linq;

namespace AgDataCodingChallengeApi.NUnit
{
	public class Tests
	{
		private Mock<Microsoft.Extensions.Logging.ILogger<AgDataCodingChallengeApi.Controllers.AddressBookController>> _mockLogger = new Mock<Microsoft.Extensions.Logging.ILogger<AgDataCodingChallengeApi.Controllers.AddressBookController>>();
		private Mock<IAddressBookCachedRepository> _mockAddressBookRepository = new Mock<IAddressBookCachedRepository>();
		private List<AddressBookEntry> _testEntries = new List<AddressBookEntry>() { 
			new AddressBookEntry("TestName1", "TestAddress1"),
			new AddressBookEntry("TestName2", "TestAddress2")
		};

		[SetUp]
		public void Setup()
		{
			
		}

		[Test]
		public void VerifyGetAllReturnsAll()
		{
			_mockAddressBookRepository.Setup(abr => abr.GetAll()).Returns(_testEntries);
			var sut = new AddressBookController(_mockLogger.Object, _mockAddressBookRepository.Object);

			OkObjectResult actionResult = sut.GetAll() as OkObjectResult;

			IEnumerable<AddressBookEntry> result = actionResult.Value as IEnumerable<AddressBookEntry>;
			
			_mockAddressBookRepository.Verify(abr=>abr.GetAll(), Times.Once());
			Assert.That(result.Count(), Is.EqualTo(_testEntries.Count()));
			_testEntries.ForEach(repoEntry =>
			{
				Assert.That(result.ToList().Contains(repoEntry), Is.True);
			});
		}

		[Test]
		public void VerifyGetReturnsCachedItem()
		{
			var testEntry = new AddressBookEntry("TestName1", "TestAddress1");
			_mockAddressBookRepository.Setup(abr => abr.Get("TestName1")).Returns(testEntry);
			var sut = new AddressBookController(_mockLogger.Object, _mockAddressBookRepository.Object);

			var actionResult = sut.Get("TestName1") as OkObjectResult;
			var result = actionResult.Value as AddressBookEntry;

			Assert.That(result, Is.EqualTo(testEntry));
			_mockAddressBookRepository.Verify(abr => abr.Get("TestName1"), Times.Once());
		}

		[Test]
		public void VerifyUpdateCallsRepository()
		{
			var testEntryUpdated = new AddressBookEntry("TestName1", "TestAddressUpdated");
			_mockAddressBookRepository.Setup(abr => abr.Update(testEntryUpdated)).Returns(true);
			var sut = new AddressBookController(_mockLogger.Object, _mockAddressBookRepository.Object);

			var result = sut.UpdateAddress(testEntryUpdated.Name, testEntryUpdated.Address);
			_mockAddressBookRepository.Verify(abr => abr.Update(testEntryUpdated), Times.Once);
			Assert.That(result.GetType(), Is.EqualTo(typeof(OkObjectResult)));
			var resultAsOk = result as OkObjectResult;
			Assert.That(resultAsOk?.StatusCode, Is.EqualTo(200));
		}

		[Test]
		public void VerifyDeleteCallsRepository()
		{
			_mockAddressBookRepository.Setup(abr => abr.Delete("TestName1")).Returns(true);
			var sut = new AddressBookController(_mockLogger.Object, _mockAddressBookRepository.Object);

			var result = sut.DeleteAddress("TestName1");
			_mockAddressBookRepository.Verify(abr => abr.Delete("TestName1"), Times.Once);
			Assert.That(result.GetType(), Is.EqualTo(typeof(OkObjectResult)));
			var resultAsOk = result as OkObjectResult;
			Assert.That(resultAsOk?.StatusCode, Is.EqualTo(200));
		}

		[Test]
		public void VerifyAddCallsRepository()
		{
			_mockAddressBookRepository.Setup(abr => abr.Add("TestName3", "TestAddress3")).Returns(true);
			var sut = new AddressBookController(_mockLogger.Object, _mockAddressBookRepository.Object);

			var result = sut.AddAddress("TestName3", "TestAddress3");
			_mockAddressBookRepository.Verify(abr => abr.Add("TestName3", "TestAddress3"), Times.Once);
			Assert.That(result.GetType(), Is.EqualTo(typeof(CreatedResult)));
			var resultAsCreated = result as CreatedResult;
			Assert.That(resultAsCreated?.StatusCode, Is.EqualTo(201));
		}
	}
}