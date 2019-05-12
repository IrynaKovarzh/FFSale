using FramSales.Context;
using FramSales.Models;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace FramSales.Services
{
	public interface IContactsRepository
	{
		void LogInService(string userName, string password, string crmService);

		RelatedContactsAccounts GetAllRelatedContactsAccounts();
		Contact GetContactByID(string id);
		void PutContact(Contact contact);
	}

	public class ContactsRepository : IContactsRepository
	{
		private SalesContext _context = new SalesContext(); 

		public void LogInService(string userName, string password, string crmService)
		{
			LogInService();

			//_context.LogInService(userName, password, crmService);
		}

		public void LogInService()
		{
			string userName = "mm@company12345678.onmicrosoft.com";
			string password = "Nemely2019";
			string crmService = "crm4";

			_context.LogInService(userName, password, crmService);
		}

		public RelatedContactsAccounts GetAllRelatedContactsAccounts()
		{
			RelatedContactsAccounts relatedContactsAccounts = new RelatedContactsAccounts();

			DataTable contacts = getContacts();

			List<string> emailaddressList = contacts.AsEnumerable()
													.Select(row => row.Field<string>("emailaddress")).ToList();
			DataTable relatedAccounts = getRelatedAccounts(emailaddressList);

			relatedContactsAccounts.Contacts = contacts.AsEnumerable().Select(row => new Contact()
			{
				ContactId = row.Field<string>("contactid"),
				FirstName = row.Field<string>("firstname"),
				LastName = row.Field<string>("lastname"),
				Email = row.Field<string>("emailaddress")
				//Mobile 
			}).ToList();

		    relatedContactsAccounts.Accounts = relatedAccounts.AsEnumerable().Select(row => new Account()
			{
				AccountId = row.Field<string>("accountid"),
				AccountName = row.Field<string>("name"),
				AccountNumber = row.Field<string>("accountnumber")
				//VatNumber 
			}).ToList(); ;

			return relatedContactsAccounts;
		}

		public bool HasContext()
		{
			return _context != null;
		}

		public Contact GetContactByID(string id)
		{
			Entity res = _context.OrganizationService.Retrieve("contact", Guid.Parse(id), new ColumnSet("contactid", "firstname", "lastname", "emailaddress1"));

			return new Contact()
			{
				ContactId = res.GetAttributeValue<Guid>("contactid").ToString(),  
				FirstName = res.GetAttributeValue<string>("firstname"),
				LastName = res.GetAttributeValue<string>("lastname"),
				Email = res.GetAttributeValue<string>("emailaddress1")
			};
		}

		public void PutContact(Contact contact)
		{
			Entity entity = new Entity("contact", Guid.Parse(contact.ContactId)); 
			entity["firstname"] = contact.FirstName;

			_context.OrganizationService.Update(entity);
		}

		private DataTable getRelatedAccounts(List<string> emailaddressList)
		{
			QueryExpression qeAccounts = new QueryExpression();
			qeAccounts.EntityName = "account";
			qeAccounts.ColumnSet = new ColumnSet("primarycontactid", "accountid", "name", "accountnumber", "emailaddress1");

			EntityCollection resultAccounts = _context.OrganizationService.RetrieveMultiple(qeAccounts);

			DataTable relatedAccounts = new DataTable();
			relatedAccounts.Columns.Add("primarycontactid");
			relatedAccounts.Columns.Add("accountid");
			relatedAccounts.Columns.Add("name");
			relatedAccounts.Columns.Add("accountnumber");
			relatedAccounts.Columns.Add("emailaddress");

			foreach (Entity entity in resultAccounts.Entities)
			{
				DataRow drA = relatedAccounts.NewRow();

				drA["accountid"] = Convert.ToString(entity.Id);
				drA["name"] = entity.GetAttributeValue<string>("name");
				drA["accountnumber"] = entity.GetAttributeValue<string>("accountnumber");
				drA["emailaddress"] = entity.GetAttributeValue<string>("emailaddress1");

				if (emailaddressList.Contains(drA["emailaddress"].ToString()))
				{
					relatedAccounts.Rows.Add(drA);
				}
			}

			return relatedAccounts;
		}

		private DataTable getContacts()
		{
			QueryExpression qe = new QueryExpression();
			qe.EntityName = "contact";
			qe.ColumnSet = new ColumnSet("contactid", "firstname", "lastname", "emailaddress1");
			EntityCollection results = _context.OrganizationService.RetrieveMultiple(qe);

			DataTable Contacts = new DataTable();
			Contacts.Columns.Add("contactid");
			Contacts.Columns.Add("firstname");
			Contacts.Columns.Add("lastname");
			Contacts.Columns.Add("emailaddress");

			foreach (Entity entity in results.Entities)
			{
				DataRow dr = Contacts.NewRow();

				dr["contactid"] = Convert.ToString(entity.Id); 
				dr["firstname"] = entity.GetAttributeValue<string>("firstname"); 
				dr["lastname"] = entity.GetAttributeValue<string>("lastname");  
				dr["emailaddress"] = entity.GetAttributeValue<string>("emailaddress1"); 

				Contacts.Rows.Add(dr);
			}

			return Contacts;
		}
	}
}