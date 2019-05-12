using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using System;
using System.Net;
using System.ServiceModel.Description;

namespace FramSales.Context
{
	public class SalesContext
	{
		public IOrganizationService OrganizationService = null;

     	public void LogInService(string userName, string password, string crmService)
		{
			if (OrganizationService == null)
			{
				try
			{
				ClientCredentials clientCredentials = new ClientCredentials();
				clientCredentials.UserName.UserName = userName; 
				clientCredentials.UserName.Password = password;

				// For Dynamics 365 Customer Engagement V9.X, set Security Protocol as TLS12
				ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
				// Get the URL from CRM, Navigate to Settings -> Customizations -> Developer Resources
				// Copy and Paste Organization Service Endpoint Address URL
				Uri sss = new Uri("https://company12345678.api.crm4.dynamics.com/XRMServices/2011/Organization.svc");
				OrganizationService = (IOrganizationService)new OrganizationServiceProxy(sss, null, clientCredentials, null);

				if (OrganizationService != null)
				{
					Guid userid = ((WhoAmIResponse)OrganizationService.Execute(new WhoAmIRequest())).UserId;

					if (userid != Guid.Empty)
					{
					 //Console.WriteLine("Connection Established Successfully...");
					}
					//OrganizationService = null;
				}
				else
				{
					//Console.WriteLine("Failed to Established Connection!!!");
				}
			}
			catch (Exception ex)
			{
				//Console.WriteLine("Exception caught - " + ex.Message);
			}
			}
			return;
		}
	}
}