using System.ComponentModel.DataAnnotations;

namespace FramSales.Models
{
	public class LoginModel
	{
		//public int UserId { get; set; }

		[Required(ErrorMessage = "Required.")]
		public string UserName { get; set; }

		[Required(ErrorMessage = "Required.")]
		public string Password { get; set; }

		[Required(ErrorMessage = "Required.")]
		public string CRMService { get; set; }
	}
}