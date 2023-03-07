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

		private readonly AddressBookEntry _testEntry1 = new AddressBookEntry("TestName1", "TestAddress1");
		private readonly AddressBookEntry _testEntry2 = new AddressBookEntry("TestName2", "TestAddress2");

		[SetUp] 
		public void SetUp() {
			_repository = new Mock<IAddressBookRepository>();
			_sut = new AddressBookCachedRepository(_repository.Object);
			_sut.InvalidateCache();

		}

		[Test]
		public void ValidateAddUpdatesCacheAndRepository()
		{
			_repository.Setup(repo => repo.Add(_testEntry1)).Returns(true);
			_repository.Setup(repo => repo.Get(_testEntry1.Name)).Returns(_testEntry1);

			var result = _sut.Add(_testEntry1);
			Assert.That(result, Is.True);

			var resultObject = _sut.Get(_testEntry1.Name);
			Assert.That(resultObject?.Name == _testEntry1.Name && resultObject?.Address == _testEntry1.Address);
			_repository.Verify(repo => repo.Add(_testEntry1));
		}

		[Test]
		public void ValidateUpdateChangesCacheAndRepository()
		{
			var testEntryUpdated = new AddressBookEntry(_testEntry1.Name, "TestAddressUpdated");
			_repository.Setup(repo => repo.Update(testEntryUpdated)).Returns(true);
			_sut.Add(_testEntry1);

			var result = _sut.Update(testEntryUpdated);
			
			Assert.That(result, Is.True);
			var resultObject = _sut.Get(_testEntry1.Name);
			Assert.That(resultObject?.Name == testEntryUpdated.Name && resultObject?.Address == testEntryUpdated.Address);
			_repository.Verify(repo => repo.Update(testEntryUpdated), Times.Once());
		}

		[Test]
		public void ValidateGetAllDoesntRequestFromRepository()
		{
			var repoEntries = new List<AddressBookEntry>() { _testEntry1, _testEntry2	};
			_repository.Setup(repo => repo.GetAll()).Returns(repoEntries);

			var resultList = _sut.GetAll();

			_repository.Verify(repo=>repo.GetAll(), Times.Once());
			Assert.That(resultList.Count, Is.EqualTo(2));
			Assert.That(resultList.Any(entry => entry.Name == _testEntry1.Name && entry.Address == _testEntry1.Address), Is.True);
			Assert.That(resultList.Any(entry => entry.Name == _testEntry2.Name && entry.Address == _testEntry2.Address), Is.True);
		}

		[Test]
		public void ValidateDeleteUpdatesRepository()
		{
			_sut.Add(_testEntry1);
			_sut.Add(_testEntry2);

			_sut.Delete(_testEntry1.Name);

			_repository.Verify(repo => repo.Delete(_testEntry1.Name), Times.Once);
			var resultShouldntExist = _sut.Get(_testEntry1.Name);
			var resultShouldExist = _sut.Get(_testEntry2.Name);

			Assert.That(resultShouldExist.Address, Is.EqualTo(_testEntry2.Address));
			Assert.That(resultShouldntExist, Is.Null);

		}
	}
}
