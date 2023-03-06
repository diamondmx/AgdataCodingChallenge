using AddressBookModels.DataModels;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;

namespace AddressBookRepositories.NUnit
{
	public class Tests
	{
		private readonly AddressBookEntry _testEntry1 = new AddressBookEntry("TestName1", "TestAddress1");
		private readonly AddressBookEntry _testEntry2 = new AddressBookEntry("TestName2", "TestAddress2");

		[SetUp]
		public void Setup()
		{
		}

		[Test]
		public void ValidateInitialDataSetIsEmpty()
		{
			var sut = new AddressBookInMemoryRepository();
			var result = sut.GetAll();

			Assert.That(result.Count, Is.EqualTo(0));
		}

		[Test]
		public void ValidateAddOneItemIsListedAfterwards()
		{
			var sut = new AddressBookInMemoryRepository();
			var initialSetResult = sut.GetAll();
			
			Assert.That(initialSetResult.Count, Is.EqualTo(0));

			var returnValue = sut.Add("TestName", "TestLocation");
			Assert.That(returnValue, Is.True);

			var finalSetResult = sut.GetAll();
			Assert.That(finalSetResult.Count, Is.EqualTo(1));
			Assert.That(finalSetResult.Any(entry => entry.Name == "TestName" && entry.Address == "TestLocation"), Is.True);
		}

		[Test]
		public void ValidateAddMultipleItemsAreListedAfterwards()
		{
			var sut = new AddressBookInMemoryRepository();
			var initialSetResult = sut.GetAll();

			Assert.That(initialSetResult.Count, Is.EqualTo(0));

			var returnValue = false;
			returnValue = sut.Add("TestName1", "TestLocation1");
			Assert.That(returnValue, Is.True);
			returnValue = sut.Add("TestName2", "TestLocation2");
			Assert.That(returnValue, Is.True);
			returnValue = sut.Add("TestName3", "TestLocation3");
			Assert.That(returnValue, Is.True);
			returnValue = sut.Add("TestName4", "TestLocation4");
			Assert.That(returnValue, Is.True);

			var finalSetResult = sut.GetAll();
			Assert.That(finalSetResult.Count, Is.EqualTo(4));
			Assert.That(finalSetResult.Any(entry => entry.Name == "TestName1" && entry.Address == "TestLocation1"), Is.True);
			Assert.That(finalSetResult.Any(entry => entry.Name == "TestName2" && entry.Address == "TestLocation2"), Is.True);
			Assert.That(finalSetResult.Any(entry => entry.Name == "TestName3" && entry.Address == "TestLocation3"), Is.True);
			Assert.That(finalSetResult.Any(entry => entry.Name == "TestName4" && entry.Address == "TestLocation4"), Is.True);
		}

		[Test]
		public void ValidateDeleteOneItem()
		{
			var sut = new AddressBookInMemoryRepository();
			sut.Add("TestName1", "TestLocation1");
			sut.Add("TestName2", "TestLocation2");
			var initialSetResult = sut.GetAll();
			Assert.That(initialSetResult.Count, Is.EqualTo(2));

			var returnValue = sut.Delete("TestName1");
			Assert.That(returnValue, Is.True);
			var finalSetResult = sut.GetAll();
			Assert.That(finalSetResult.Count, Is.EqualTo(1));
			Assert.That(finalSetResult.Any(entry => entry.Name == "TestName2" && entry.Address == "TestLocation2"), Is.True);
		}

		[Test]
		public void UpdateOneItem()
		{
			var sut = new AddressBookInMemoryRepository();
			sut.Add(_testEntry1);
			sut.Add(_testEntry2);
			var initialSetResult = sut.GetAll();
			Assert.That(initialSetResult.Count, Is.EqualTo(2));
			var testEntryUpdated = new AddressBookEntry(_testEntry1.Name, "TestAddressUpdated");

			var returnValue = sut.Update(testEntryUpdated);

			Assert.That(returnValue, Is.True);
			var finalSetResult = sut.GetAll();
			Assert.That(finalSetResult.Count, Is.EqualTo(2));
			Assert.That(finalSetResult.Any(entry => entry.Name == _testEntry1.Name && entry.Address == testEntryUpdated.Address), Is.True);
			Assert.That(finalSetResult.Any(entry => entry.Name == _testEntry2.Name && entry.Address == testEntryUpdated.Address), Is.True);
		}

		[Test]
		public void UpdateMissingItem()
		{
			var sut = new AddressBookInMemoryRepository();
			sut.Add(_testEntry1);
			sut.Add(_testEntry2);
			var initialSetResult = sut.GetAll();
			Assert.That(initialSetResult.Count, Is.EqualTo(2));

			var testEntryUpdated = new AddressBookEntry(_testEntry1.Name, "TestAddressUpdated");
			var returnValue = sut.Update(testEntryUpdated);
			Assert.That(returnValue, Is.False);
			var finalSetResult = sut.GetAll();
			
			Assert.That(finalSetResult.Count, Is.EqualTo(2));
			Assert.That(finalSetResult.Any(entry => entry.Name == _testEntry1.Name && entry.Address == testEntryUpdated.Address), Is.True);
			Assert.That(finalSetResult.Any(entry => entry.Name == _testEntry2.Name && entry.Address == _testEntry2.Address), Is.True);
		}
	}
}