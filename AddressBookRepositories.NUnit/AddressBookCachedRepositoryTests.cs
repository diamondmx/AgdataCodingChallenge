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

		[SetUp] 
		public void SetUp() {
		}

		[Test]
		public void ValidateAddUpdatesCacheAndRepository()
		{
			_repository = new Mock<IAddressBookRepository>();
			_repository.Setup(repo => repo.Add("TestName1", "TestAddress1")).Returns(true);
			var _sut = new AddressBookCachedRepository(_repository.Object);

			var result = _sut.Add("TestName1", "TestAddress1");
			Assert.That(result, Is.True);

			var finalResultSet = _sut.GetAll();
			Assert.That(finalResultSet.Count(), Is.EqualTo(1));
			Assert.That(finalResultSet.Any(entry => entry.Name == "TestName1" && entry.Address == "TestAddress1"));

		}

		[Test]
		public void ValidateUpdateChangesCacheAndRepository()
		{
			_repository = new Mock<IAddressBookRepository>();
			_repository.Setup(repo => repo.Update("TestName1", "TestAddress1")).Returns(true);
			var _sut = new AddressBookCachedRepository(_repository.Object);
			_sut.Add("TestName1", "TestAddress1");
			var initialResultSet = _sut.GetAll();
			Assert.That(initialResultSet.Count(), Is.EqualTo(1));
			Assert.That(initialResultSet.Any(entry => entry.Name == "TestName1" && entry.Address == "TestAddress1"));

			var result = _sut.Update("TestName1", "TestAddressUpdated");
			
			Assert.That(result, Is.True);
			var finalResultSet = _sut.GetAll();
			Assert.That(finalResultSet.Count(), Is.EqualTo(1));
			Assert.That(finalResultSet.Any(entry => entry.Name == "TestName1" && entry.Address == "TestAddressUpdated"));
			_repository.Verify(repo => repo.Update("TestName1", "TestAddressUpdated"), Times.Once());
		}

		[Test]
		public void ValidateGetAllDoesntRequestFromRepository()
		{
			_repository = new Mock<IAddressBookRepository>();
			var _sut = new AddressBookCachedRepository(_repository.Object);

			_sut.GetAll();

			_repository.VerifyNoOtherCalls();
		}

		[Test]
		public void ValidateDeleteUpdatesRepository()
		{
			_repository = new Mock<IAddressBookRepository>();
			var _sut = new AddressBookCachedRepository(_repository.Object);
			_sut.Add("TestName1", "TestAddress1");
			_sut.Add("TestName2", "TestAddress2");
			var initialResultSet = _sut.GetAll();
			Assert.That(initialResultSet.Count(), Is.EqualTo(2));
			Assert.That(initialResultSet.Any(entry => entry.Name == "TestName1" && entry.Address == "TestAddress1"));
			Assert.That(initialResultSet.Any(entry => entry.Name == "TestName2" && entry.Address == "TestAddress2"));

			_sut.Delete("TestName1");

			_repository.Verify(repo => repo.Delete("TestName1"), Times.Once);
			var finalResultSet = _sut.GetAll();
			Assert.That(finalResultSet.Count(), Is.EqualTo(1));
			Assert.That(initialResultSet.Any(entry => entry.Name == "TestName2" && entry.Address == "TestAddress2"));

		}
	}
}
