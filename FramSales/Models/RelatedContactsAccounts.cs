using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FramSales.Models
{
	public class RelatedContactsAccounts
	{
		public List<Contact> Contacts { get; set; }
		public List<Account> Accounts { get; set; }
	}
}