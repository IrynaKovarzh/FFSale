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
	public class RelatedContactsAccounts
	{
		public RelatedContactsAccounts()
		{
			Contacts = new DataTable();
			RelatedAccounts = new DataTable();
		}

		public DataTable Contacts { get; set; }
		public DataTable RelatedAccounts { get; set; }
	}

	public interface IProductRepository
	{
		void LogInService(string userName, string password, string crmService);

		RelatedContactsAccounts GetAllRelatedContactsAccounts();
		Contact GetContactByID(int id);
		DataTable GetAllContacts();
	}

	public class ContactsRepository : IProductRepository
	{
		private SalesContext _context = new SalesContext(); //???

		public void LogInService(string userName, string password, string crmService)
		{
			userName = "mm@company12345678.onmicrosoft.com";  //"iryna@kovarzh.onmicrosoft.com"; 
			password = "Nemely2019";
			crmService = "crm4";

			_context.LogInService(userName, password, crmService);
		}

		public RelatedContactsAccounts GetAllRelatedContactsAccounts()
		{
			// if (OrganizationService != null)
			RelatedContactsAccounts relatedContactsAccounts = new RelatedContactsAccounts();

			DataTable contacts = getContacts();

			List<string> emailaddressList = contacts.AsEnumerable()
													.Select(row => row.Field<string>("emailaddress")).ToList();
			DataTable relatedAccounts = getRelatedAccounts(emailaddressList);

			relatedContactsAccounts.Contacts = contacts;
			relatedContactsAccounts.RelatedAccounts = relatedAccounts;

			return relatedContactsAccounts;
		}

		public Contact GetContactByID(int id)
		{
			return new Contact(); 
		}

		public DataTable GetAllContacts()
		{
			// if (OrganizationService != null)

			return getContacts();
		}

		private DataTable getRelatedAccounts(List<string> emailaddressList)
		{
			int kk = 6;

			QueryExpression qeAccounts = new QueryExpression();
			qeAccounts.EntityName = "account";
			//qeAccounts.ColumnSet.AllColumns = true;

			qeAccounts.ColumnSet = new ColumnSet("primarycontactid", "accountid", "name", "accountnumber", "emailaddress1");
			//qe.ColumnSet.Columns.Add("name"); //AddRange();
			//qeAccounts.Criteria.AddCondition("primarycontactid", ConditionOperator.NotEqual, null);
			//qeAccounts.Criteria.AddCondition("primarycontactid", ConditionOperator.NotNull);
			//qeAccounts.Criteria.AddCondition("primarycontactid", ConditionOperator.In, dt.Columns["contactid"]); //["contactid"]);
			EntityCollection resultAccounts = _context.OrganizationService.RetrieveMultiple(qeAccounts);

			int kkk = 6;

			DataTable relatedAccounts = new DataTable();
			relatedAccounts.Columns.Add("primarycontactid");
			relatedAccounts.Columns.Add("accountid");
			relatedAccounts.Columns.Add("name");
			relatedAccounts.Columns.Add("accountnumber");
			relatedAccounts.Columns.Add("emailaddress");

			/*dt.Columns.Add("contactid");
			dt.Columns.Add("firstname");
			dt.Columns.Add("lastname");
			dt.Columns.Add("emailaddress"); */

			int ffff = 1;

			foreach (Entity entity in resultAccounts.Entities)
			{
				DataRow drA = relatedAccounts.NewRow();

				// Console.WriteLine(entity.Attributes["contactid"]); // + " " + entity.Attributes["emailaddress1"]);

				drA["accountid"] = Convert.ToString(entity.Id); // entity.Attributes["contactid"] ?? entity.Attributes["contactid"].ToString();
				drA["name"] = entity.GetAttributeValue<string>("name"); // entity.Attributes["firstname"] ?? entity.Attributes["firstname"].ToString();
				drA["accountnumber"] = entity.GetAttributeValue<string>("accountnumber");  //entity.Attributes["lastname"] ?? entity.Attributes["lastname"].ToString();
																						   //drA["primarycontactid"] = Convert.ToString(entity.Attributes["primarycontactid"]); //?? entity.Attributes["emailaddress1"].ToString();
				drA["emailaddress"] = entity.GetAttributeValue<string>("emailaddress1");

				//new List<string> ( dt.Columns["contactid"]
				//DataTable d = new DataTable();
				//d.Columns[0] = new DataColumn[] { dt.Columns["contactid"] };
				string vvv = drA["emailaddress"].ToString();
				if (emailaddressList.Contains(vvv))
				{
					//Console.WriteLine(ffff);
					relatedAccounts.Rows.Add(drA);
					ffff++;
					// Console.WriteLine(drA["accountid"] + " " + drA["name"] + " " + drA["accountnumber"] + " " + drA["emailaddress"]);
				}
				// + " " + drA["primarycontactid"]); // + " " + drA["primarycontactid"]); // + " " + drA["name"] + " " + drA["accountnumber"] + " " + drA["primarycontactid"]);
			}
			int fffff = 1;

			return relatedAccounts;
		}

		private DataTable getContacts()
		{
			QueryExpression qe = new QueryExpression();
			qe.EntityName = "contact";
			qe.ColumnSet = new ColumnSet("contactid", "firstname", "lastname", "emailaddress1");
			//qe.ColumnSet.Columns.Add("name"); //AddRange();
			EntityCollection results = _context.OrganizationService.RetrieveMultiple(qe);

			int k = 6;

			DataTable Contacts = new DataTable();
			Contacts.Columns.Add("contactid");
			Contacts.Columns.Add("firstname");
			Contacts.Columns.Add("lastname");
			Contacts.Columns.Add("emailaddress");

			int f = 1;

			foreach (Entity entity in results.Entities)
			{
				DataRow dr = Contacts.NewRow();

				// Console.WriteLine(entity.Attributes["contactid"]); // + " " + entity.Attributes["emailaddress1"]);

				dr["contactid"] = Convert.ToString(entity.Id); // entity.Attributes["contactid"] ?? entity.Attributes["contactid"].ToString();
				dr["firstname"] = entity.GetAttributeValue<string>("firstname"); // entity.Attributes["firstname"] ?? entity.Attributes["firstname"].ToString();
				dr["lastname"] = entity.GetAttributeValue<string>("lastname");  //entity.Attributes["lastname"] ?? entity.Attributes["lastname"].ToString();
				dr["emailaddress"] = entity.GetAttributeValue<string>("emailaddress1"); // entity.Attributes["emailaddress1"] ?? entity.Attributes["emailaddress1"].ToString();

				Console.WriteLine(f);
				Console.WriteLine(dr["contactid"] + " " + dr["emailaddress"] + " " + dr["firstname"] + " " + dr["lastname"]);
				f++;

				Contacts.Rows.Add(dr);
			}

			List<string> emailaddressList = Contacts.AsEnumerable()
							   .Select(row => row.Field<string>("emailaddress")).ToList();

			//List<string> contactid = dt.Columns["contactid"];

			int kk = 6;

			QueryExpression qeAccounts = new QueryExpression();
			qeAccounts.EntityName = "account";
			//qeAccounts.ColumnSet.AllColumns = true;

			qeAccounts.ColumnSet = new ColumnSet("primarycontactid", "accountid", "name", "accountnumber", "emailaddress1");
			//qe.ColumnSet.Columns.Add("name"); //AddRange();
			//qeAccounts.Criteria.AddCondition("primarycontactid", ConditionOperator.NotEqual, null);
			//qeAccounts.Criteria.AddCondition("primarycontactid", ConditionOperator.NotNull);
			//qeAccounts.Criteria.AddCondition("primarycontactid", ConditionOperator.In, dt.Columns["contactid"]); //["contactid"]);
			EntityCollection resultAccounts = _context.OrganizationService.RetrieveMultiple(qeAccounts);

			int kkk = 6;

			DataTable dtA = new DataTable();
			dtA.Columns.Add("primarycontactid");
			dtA.Columns.Add("accountid");
			dtA.Columns.Add("name");
			dtA.Columns.Add("accountnumber");
			dtA.Columns.Add("emailaddress");

			/*dt.Columns.Add("contactid");
			dt.Columns.Add("firstname");
			dt.Columns.Add("lastname");
			dt.Columns.Add("emailaddress"); */

			int ffff = 1;

			foreach (Entity entity in resultAccounts.Entities)
			{
				DataRow drA = dtA.NewRow();

				// Console.WriteLine(entity.Attributes["contactid"]); // + " " + entity.Attributes["emailaddress1"]);

				drA["accountid"] = Convert.ToString(entity.Id); // entity.Attributes["contactid"] ?? entity.Attributes["contactid"].ToString();
				drA["name"] = entity.GetAttributeValue<string>("name"); // entity.Attributes["firstname"] ?? entity.Attributes["firstname"].ToString();
				drA["accountnumber"] = entity.GetAttributeValue<string>("accountnumber");  //entity.Attributes["lastname"] ?? entity.Attributes["lastname"].ToString();
																						   //drA["primarycontactid"] = Convert.ToString(entity.Attributes["primarycontactid"]); //?? entity.Attributes["emailaddress1"].ToString();
				drA["emailaddress"] = entity.GetAttributeValue<string>("emailaddress1");

				//new List<string> ( dt.Columns["contactid"]
				//DataTable d = new DataTable();
				//d.Columns[0] = new DataColumn[] { dt.Columns["contactid"] };
				string vvv = drA["emailaddress"].ToString();
				if (emailaddressList.Contains(vvv))
				{
					Console.WriteLine(ffff);
					dtA.Rows.Add(drA);
					ffff++;
					Console.WriteLine(drA["accountid"] + " " + drA["name"] + " " + drA["accountnumber"] + " " + drA["emailaddress"]);
				}
				// + " " + drA["primarycontactid"]); // + " " + drA["primarycontactid"]); // + " " + drA["name"] + " " + drA["accountnumber"] + " " + drA["primarycontactid"]);
			}
			int fffff = 1;

			return Contacts;
		}
	}
}