using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using System.Net;
using FlashCardsPort;
using Android.Views.InputMethods;
using System.IO;

namespace FlashCardsPort.Droid
{
	[Activity (Label = "Авторизация", MainLauncher = true, Icon = "@drawable/Icon")]
	public class MainActivity : Activity
	{
        int count = 1;
        static BaseData bd = new BaseData();
        String Id_user;
        TextView txtlog, sign_up, forgot_password;
        EditText txtpass, txtemail;
        Button login;
        String picture = "123";
        public byte[] fileContents;
        protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
            // Set our view from the "main" layout resource
            SetContentView (Resource.Layout.Main);
            // Get our button from the layout resource,
            // and attach an event to it
            bd.connection();
            login = FindViewById<Button> (Resource.Id.login_btn_login);
            txtemail = FindViewById<EditText>(Resource.Id.login_email);
            txtpass = FindViewById<EditText>(Resource.Id.login_password);
            sign_up = FindViewById<TextView>(Resource.Id.login_btn_signup);
            forgot_password = FindViewById<TextView>(Resource.Id.login_btn_forgot_password);

            //string ftpfullpath = "ftp://ftp.billions-consult.ru/flashcards/" + filename;
            //FtpWebRequest ftp = (FtpWebRequest)FtpWebRequest.Create(ftpfullpath);
            //ftp.Credentials = new NetworkCredential(ftpUser, ftpPassword);
            //ftp.KeepAlive = true;
            //ftp.UseBinary = true;
            //ftp.Method = WebRequestMethods.Ftp.UploadFile;
            //System.IO.FileStream fs = System.IO.File.OpenRead(file.ToString());
            //byte[] buffer = new byte[fs.Length];
            //fs.Read(buffer, 0, buffer.Length);
            //fs.Close();
            //System.IO.Stream ftpstream = ftp.GetRequestStream();
            //ftpstream.Write(buffer, 0, buffer.Length);
            //ftpstream.Close();
            //ftpstream.Flush();

            forgot_password.Click += Forgot_pass;
            sign_up.Click += Sign_up_user;
            login.Click += Register_user;        
        }

        private void Forgot_pass(object sender, EventArgs e)
        {
            StartActivity(typeof(Forgot_password));
        }

        private void Sign_up_user(object sender, EventArgs e)
        {
            StartActivity(typeof(Sign_up));
        }

        private void Register_user(object sender, EventArgs e)
        {
            //bd.User_Registration(txtemail.Text,txtpass.Text);
           //nputMethodManager inputManager = (InputMethodManager)this.GetSystemService(Context.InputMethodService);
          //inputManager.HideSoftInputFromWindow(this.CurrentFocus.WindowToken, HideSoftInputFlags.NotAlways);
            //Id_user = bd.Login(txtemail.Text, txtpass.Text);
            //if (Id_user == "false")
            //{
            //    Toast.MakeText(this, "Не правильный логин или пароль", ToastLength.Long).Show();
            //}
            //else
            //{
                StartActivity(typeof(Main_menu_admin));
            //}
        }
    }
}


