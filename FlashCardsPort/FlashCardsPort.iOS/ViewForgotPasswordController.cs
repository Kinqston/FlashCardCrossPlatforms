using Foundation;
using System;
using UIKit;
using System.Text.RegularExpressions;
using ToastIOS;
using System.Net.Mail;
using System.Net;

namespace FlashCardsPort.iOS
{

	public partial class ViewForgotPasswordController : UIViewController
	{
		int code_random;
		string id_user;
		Random random = new Random();
		BaseData bd = new BaseData();
		public ViewForgotPasswordController(IntPtr handle) : base(handle)
		{
		}
		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			NavigationItem.HidesBackButton = true;
		}
		partial void UIButton2305_TouchUpInside(UIButton sender)
		{
			var other = Storyboard.InstantiateViewController("ViewRegisterController") as ViewRegisterController;
			NavigationController.PushViewController(other, true);
		}

		partial void UIButton2306_TouchUpInside(UIButton sender)
		{
			var other = Storyboard.InstantiateViewController("ViewLoginController") as ViewLoginController;
			NavigationController.PushViewController(other, true);
		}

		partial void UIButton2300_TouchUpInside(UIButton sender)
		{
			if (IsValidEmail(email.Text.ToLower()) == true)
			{
				if (bd.Mail(email.Text.ToLower()) == false)
				{
					try
					{
						MailMessage mail = new MailMessage();
						SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
						mail.From = new MailAddress("FlashCardsCross@gmail.com");
						mail.To.Add(email.Text.ToLower());
						code_random = random.Next(1000, 9999);
						mail.Subject = "Код подтверждения";
						mail.Body = "Введите этот код подтвержения в приложении: " + code_random.ToString();
						SmtpServer.Port = 587;
						SmtpServer.Credentials = new System.Net.NetworkCredential("FlashCardsCross@gmail.com", "ZxCvBnM12345qwAS");
						SmtpServer.EnableSsl = true;
						ServicePointManager.ServerCertificateValidationCallback = delegate (object sender2, System.Security.Cryptography.X509Certificates.X509Certificate certificate, System.Security.Cryptography.X509Certificates.X509Chain chain, System.Net.Security.SslPolicyErrors sslPolicyErrors)
						{
							return true;
						};
						SmtpServer.Send(mail);

						var textInputAlertController = UIAlertController.Create("Код подтверждения", "Введите код подтверждения который был выслан вам на почту", UIAlertControllerStyle.Alert);
						textInputAlertController.AddTextField(textField => { });

						//Add Actions
						var cancelAction = UIAlertAction.Create("Отмена", UIAlertActionStyle.Cancel, alertAction => Console.WriteLine("Cancel was Pressed"));
						var okayAction = UIAlertAction.Create("Ок", UIAlertActionStyle.Default, alertAction => Code_proverka(textInputAlertController.TextFields[0].Text));
						textInputAlertController.AddAction(cancelAction);
						textInputAlertController.AddAction(okayAction);
						//Present Alert
						PresentViewController(textInputAlertController, true, null);
						Toast.MakeText("На почту был отправлен код подтверждения").Show();
					}
					catch (Exception ex)
					{
						Toast.MakeText("Ошибка при отправлении письма").Show();
					}
				}
				else
				{
					Toast.MakeText("Эта почта не зарегистрирована").Show();
				}
			}
			else
			{
				Toast.MakeText("Некорректно введена почта").Show();
			}
		}

		public bool IsValidEmail(string email)
		{
			//regular expression pattern for valid email
			//addresses, allows for the following domains:
			//com,edu,info,gov,int,mil,net,org,biz,name,museum,coop,aero,pro,tv
			string pattern = @"^[-a-zA-Z0-9][-.a-zA-Z0-9]*@[-.a-zA-Z0-9]+(\.[-.a-zA-Z0-9]+)*\.
				    (com|edu|info|gov|int|mil|net|org|biz|name|museum|coop|aero|pro|tv|[a-zA-Z]{2})$";
			//Regular expression object
			Regex check = new Regex(pattern, RegexOptions.IgnorePatternWhitespace);
			//boolean variable to return to calling method
			bool valid = false;

			//make sure an email address was provided
			if (string.IsNullOrEmpty(email))
			{
				valid = false;
			}
			else
			{
				//use IsMatch to validate the address
				valid = check.IsMatch(email);
			}
			//return the value to the calling method
			return valid;
		}

		void Code_proverka(string text)
		{
			if (text == code_random.ToString())
			{
				var textInputAlertController = UIAlertController.Create("Новый пароль", "Введите новый пароль", UIAlertControllerStyle.Alert);
				//Add Text Input
				textInputAlertController.AddTextField(textField => { });
				//Add Actions
				var cancelAction = UIAlertAction.Create("Отмена", UIAlertActionStyle.Cancel, alertAction => Console.WriteLine("Cancel was Pressed"));
				var okayAction = UIAlertAction.Create("Ок", UIAlertActionStyle.Default, alertAction => ResetPass(textInputAlertController.TextFields[0].Text));
				textInputAlertController.AddAction(cancelAction);
				textInputAlertController.AddAction(okayAction);
				//Present Alert
				PresentViewController(textInputAlertController, true, null);
			}
			else
			{
				var textInputAlertController = UIAlertController.Create("Код подтверждения", "Введите код подтверждения который был выслан вам на почту", UIAlertControllerStyle.Alert);
				//Add Text Input
				textInputAlertController.AddTextField(textField => { });
				//Add Actions
				var cancelAction = UIAlertAction.Create("Отмена", UIAlertActionStyle.Cancel, alertAction => Console.WriteLine("Cancel was Pressed"));
				var okayAction = UIAlertAction.Create("Ок", UIAlertActionStyle.Default, alertAction => Code_proverka(textInputAlertController.TextFields[0].Text));
				textInputAlertController.AddAction(cancelAction);
				textInputAlertController.AddAction(okayAction);
				//Present Alert
				PresentViewController(textInputAlertController, true, null);
				Toast.MakeText("Неверный код").Show();
			}
		}
		void ResetPass(string text)
		{
			if (text.Length < 6)
			{
				var textInputAlertController = UIAlertController.Create("Новый пароль", "Введите новый пароль", UIAlertControllerStyle.Alert);
				//Add Text Input
				textInputAlertController.AddTextField(textField => { });
				//Add Action
				var cancelAction = UIAlertAction.Create("Отмена", UIAlertActionStyle.Cancel, alertAction => Console.WriteLine("Cancel was Pressed"));
				var okayAction = UIAlertAction.Create("Ок", UIAlertActionStyle.Default, alertAction => ResetPass(textInputAlertController.TextFields[0].Text));
				textInputAlertController.AddAction(cancelAction);
				textInputAlertController.AddAction(okayAction);
				//Present Alert
				PresentViewController(textInputAlertController, true, null);
				Toast.MakeText("Пароль должен быть больше 5 символов").Show();
			}
			else
			{
				bd.Change_password(email.ToString().ToLower(), text);
				var other = Storyboard.InstantiateViewController("MainMenu") as MainMenu;
				NavigationController.PushViewController(other, true);
			}
		}
	}
}