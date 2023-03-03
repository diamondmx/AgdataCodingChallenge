using System.Net;
using System.Net.Http.Headers;

namespace AddressBookRepositories.NUnit
{
	public class Tests
	{
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
			Assert.That(finalSetResult["TestName"], Is.EqualTo("TestLocation"));
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
			Assert.That(finalSetResult["TestName1"], Is.EqualTo("TestLocation1"));
			Assert.That(finalSetResult["TestName2"], Is.EqualTo("TestLocation2"));
			Assert.That(finalSetResult["TestName3"], Is.EqualTo("TestLocation3"));
			Assert.That(finalSetResult["TestName4"], Is.EqualTo("TestLocation4"));
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
			Assert.That(finalSetResult["TestName2"], Is.EqualTo("TestLocation2"));
		}

		[Test]
		public void UpdateOneItem()
		{
			var sut = new AddressBookInMemoryRepository();
			sut.Add("TestName1", "TestLocation1");
			sut.Add("TestName2", "TestLocation2");
			var initialSetResult = sut.GetAll();
			Assert.That(initialSetResult.Count, Is.EqualTo(2));

			var returnValue = sut.Update("TestName1", "TestLocationUpdated");
			Assert.That(returnValue, Is.True);
			var finalSetResult = sut.GetAll();
			Assert.That(finalSetResult.Count, Is.EqualTo(2));
			Assert.That(finalSetResult["TestName1"], Is.EqualTo("TestLocationUpdated"));
			Assert.That(finalSetResult["TestName2"], Is.EqualTo("TestLocation2"));
		}

		[Test]
		public void UpdateMissingItem()
		{
			var sut = new AddressBookInMemoryRepository();
			sut.Add("TestName1", "TestLocation1");
			sut.Add("TestName2", "TestLocation2");
			var initialSetResult = sut.GetAll();
			Assert.That(initialSetResult.Count, Is.EqualTo(2));

			var returnValue = sut.Update("TestNameMissing", "TestLocationUpdated");
			Assert.That(returnValue, Is.False);
			var finalSetResult = sut.GetAll();
			
			Assert.That(finalSetResult.Count, Is.EqualTo(2));
			Assert.That(finalSetResult["TestName1"], Is.EqualTo("TestLocation1"));
			Assert.That(finalSetResult["TestName2"], Is.EqualTo("TestLocation2"));
		}
	}
}