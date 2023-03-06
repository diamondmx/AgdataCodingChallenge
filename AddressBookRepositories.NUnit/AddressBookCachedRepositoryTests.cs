using AddressBookModels.DataModels;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AddressBookRepositories.NUnit
{
	public class AddressBookCachedRepositoryTests
	{
		private Mock<IAddressBookRepository> _repository;
		private AddressBookCachedRepository _sut;

		[SetUp] 
		public void SetUp() {
			_repository = new Mock<IAddressBookRepository>();
			_sut = new AddressBookCachedRepository(_repository.Object);
			_sut.InvalidateCache();

		}

		[Test]
		public void ValidateAddUpdatesCacheAndRepository()
		{
			_repository.Setup(repo => repo.Add("TestName1", "TestAddress1")).Returns(true);

			var result = _sut.Add("TestName1", "TestAddress1");
			Assert.That(result, Is.True);

			var resultObject = _sut.Get("TestName1");
			Assert.That(resultObject.Name == "TestName1" && resultObject.Address == "TestAddress1");
			_repository.Verify(repo => repo.Add("TestName1", "TestAddress1"));
		}

		[Test]
		public void ValidateUpdateChangesCacheAndRepository()
		{
			_repository.Setup(repo => repo.Update("TestName1", "TestAddress1")).Returns(true);
			_sut.Add("TestName1", "TestAddress1");

			var result = _sut.Update("TestName1", "TestAddressUpdated");
			
			Assert.That(result, Is.True);
			var resultObject = _sut.Get("TestName1");
			Assert.That(resultObject.Name == "TestName1" && resultObject.Address == "TestAddressUpdated");
			_repository.Verify(repo => repo.Update("TestName1", "TestAddressUpdated"), Times.Once());
		}

		[Test]
		public void ValidateGetAllDoesntRequestFromRepository()
		{
			var repoEntries = new List<AddressBookEntry>() {
				new AddressBookEntry("TestName1", "TestAddress1"),
				new AddressBookEntry("TestName2", "TestAddress2")
			};
			_repository.Setup(repo => repo.GetAll()).Returns(repoEntries);

			var resultList = _sut.GetAll();

			_repository.Verify(repo=>repo.GetAll(), Times.Once());
			Assert.That(resultList.Count, Is.EqualTo(2));
			Assert.That(resultList.Any(entry => entry.Name == "TestName1" && entry.Address == "TestAddress1"), Is.True);
			Assert.That(resultList.Any(entry => entry.Name == "TestName2" && entry.Address == "TestAddress2"), Is.True);
		}

		[Test]
		public void ValidateDeleteUpdatesRepository()
		{
			_sut.Add("TestName1", "TestAddress1");
			_sut.Add("TestName2", "TestAddress2");		

			_sut.Delete("TestName1");

			_repository.Verify(repo => repo.Delete("TestName1"), Times.Once);
			var resultShouldntExist = _sut.Get("TestName1");
			var resultShouldExist = _sut.Get("TestName2");

			Assert.That(resultShouldExist.Address, Is.EqualTo("TestAddress2"));
			Assert.That(resultShouldntExist, Is.Null);

		}
	}
}
