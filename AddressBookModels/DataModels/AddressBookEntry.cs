using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AddressBookModels.DataModels
{
	[DebuggerDisplay("Entry({Name}:{Address})")]
	public class AddressBookEntry
	{
		public string Name;
		public string Address;

		public AddressBookEntry(string name, string address) => (Name, Address) = (name, address);

		public bool Equals(AddressBookEntry obj)
		{
			return Name == obj?.Name && Address == obj?.Address;
		}

		public static bool operator == (AddressBookEntry left, AddressBookEntry right)
		{
			return left.Equals(right);
		}

		public static bool operator != (AddressBookEntry left, AddressBookEntry right)
		{
			return !left.Equals(right);
		}

		public override int GetHashCode()
		{
			return Name.GetHashCode() ^ Address.GetHashCode();
		}
	}
}
