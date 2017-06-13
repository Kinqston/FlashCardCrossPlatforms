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
	[Activity (Label = "FlashCard", MainLauncher = true)]
	public class MainActivity : Activity
	{
        int count = 1;
        static BaseData bd = new BaseData();
        String Id_user;
        TextView txtlog, sign_up, forgot_password;
        EditText txtpass, txtemail;
        Button login;
        String picture = "123";
        Button offline_button;
        public byte[] fileContents;
        protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
            // Set our view from the "main" layout resource
            SetContentView (Resource.Layout.Main);
            // Get our button from the layout resource,
            // and attach an event to it
            Title = "Авторизация";
            ActionBar actionBar = ActionBar;
            actionBar.SetDisplayShowHomeEnabled(false);
            var prefs = Application.Context.GetSharedPreferences("FC", FileCreationMode.Private);
            var somePref = prefs.GetString("Id", null);
            Id_user = somePref;
            if (Id_user == "1")
            {
                StartActivity(typeof(Main_menu_admin));
            }
            else
            {
                if (Id_user != null)
                {
                    StartActivity(typeof(Main_user));
                }
            }
            bd.connection();
            login = FindViewById<Button> (Resource.Id.login_btn_login);
            txtemail = FindViewById<EditText>(Resource.Id.login_email);
            txtpass = FindViewById<EditText>(Resource.Id.login_password);
            sign_up = FindViewById<TextView>(Resource.Id.login_btn_signup);
            forgot_password = FindViewById<TextView>(Resource.Id.login_btn_forgot_password);

            offline_button = FindViewById<Button>(Resource.Id.offline_button);

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
            offline_button.Click += Offline_user;
        }

        private void Offline_user(object sender, EventArgs e)
        {
            StartActivity(typeof(Main_user));
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
            InputMethodManager inputManager = (InputMethodManager)this.GetSystemService(Context.InputMethodService);
            inputManager.HideSoftInputFromWindow(this.CurrentFocus.WindowToken, HideSoftInputFlags.NotAlways);
            Id_user = bd.Login(txtemail.Text.ToLower(), txtpass.Text);
            if (Id_user == "false")
            {
                Toast.MakeText(this, "Не правильный логин или пароль", ToastLength.Long).Show();
            }
            else
            {
                if (Id_user == "1")
                {
                    var prefs = Application.Context.GetSharedPreferences("FC", FileCreationMode.Private);
                    var prefEditor = prefs.Edit();
                    prefEditor.PutString("Id", Id_user);
                    prefEditor.Commit();
                    StartActivity(typeof(Main_menu_admin));
                }
                else
                {
                    var prefs = Application.Context.GetSharedPreferences("FC", FileCreationMode.Private);
                    var prefEditor = prefs.Edit();
                    prefEditor.PutString("Id", Id_user);
                    prefEditor.Commit();
                    Intent intent = new Intent(this, typeof(Main_user));
                    intent.PutExtra("Id", Id_user);
                    StartActivity(intent);
                }
            }
        }

    }
}


