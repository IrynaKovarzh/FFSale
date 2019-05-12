using FramSales.Models;
using FramSales.Services;
using System.Web.Mvc;

namespace FramSales.Controllers
{
	public class ContactsController : Controller
    {
		ContactsRepository contactsRepository = new ContactsRepository();

		[HttpGet]
		public ActionResult ContactDashBoard()
		{
			contactsRepository.LogInService(); 

			var crelatedContactsAccounts = contactsRepository.GetAllRelatedContactsAccounts();

			return View(crelatedContactsAccounts);
		}

		public ActionResult Login(LoginModel loginModel)
		{
			//contactsRepository.LogInService(loginModel.UserName, loginModel.Password, loginModel.CRMService);

			contactsRepository.LogInService();

			if (!contactsRepository.HasContext())
			{
				return RedirectToAction("LoginError", "Home");
			}

			return RedirectToAction("ContactDashBoard"); 
		}

		public ActionResult ContactDetailsGet(string id)
		{
			contactsRepository.LogInService();

			var contactDetail = contactsRepository.GetContactByID(id);

			return View("ContactDetails", contactDetail);
		}

		[HttpPost]
		public ActionResult ContactDetails(Contact contact)
		{
			contactsRepository.LogInService();

			contactsRepository.PutContact(contact);

			return RedirectToAction("ContactDashBoard");
		}
	}
}
 