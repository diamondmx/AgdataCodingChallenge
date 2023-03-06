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

			var result = sut.Get();
			_mockAddressBookRepository.Verify(abr=>abr.GetAll(), Times.Once());
			Assert.That(result.Count, Is.EqualTo(_testEntries.Count));
			_testEntries.ForEach(repoEntry =>
			{
				Assert.That(result.ToList().Contains(repoEntry), Is.True);
			});
		}

		[Test]
		public void VerifyUpdateCallsRepository()
		{
			_mockAddressBookRepository.Setup(abr => abr.Update("TestName1", "TestAddressUpdated")).Returns(true);
			var sut = new AddressBookController(_mockLogger.Object, _mockAddressBookRepository.Object);

			var result = sut.UpdateAddress("TestName1", "TestAddressUpdated");
			_mockAddressBookRepository.Verify(abr => abr.Update("TestName1", "TestAddressUpdated"), Times.Once);
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