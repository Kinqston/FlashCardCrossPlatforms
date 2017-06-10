using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Net;

namespace FlashCardsPort.Droid
{
    [Activity(Label = "Восстановление пароля")]
    public class Forgot_password : Activity
    {
        static BaseData bd = new BaseData();
        EditText txtemail;
        TextView txtaccount, txtsign_up;
        Button Forgot_btn_reset;
        EditText code;
        int code_random;
        EditText password;
        Random random = new Random();
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.forgot_password);
            Forgot_btn_reset = FindViewById<Button>(Resource.Id.forgot_btn_reset);
            txtemail = FindViewById<EditText>(Resource.Id.forgot_email);
            txtaccount = FindViewById<TextView>(Resource.Id.forgot_btn_login);
            txtsign_up = FindViewById<TextView>(Resource.Id.forgot_btn_signup);
            Forgot_btn_reset.Click += Reset;
            txtaccount.Click += Account;
            txtsign_up.Click += Register;
            ActionBar actionBar = ActionBar;
            actionBar.SetDisplayShowHomeEnabled(false);
            // Create your application here
        }

        private void Register(object sender, EventArgs e)
        {
            StartActivity(typeof(Sign_up));
        }

        private void Account(object sender, EventArgs e)
        {
            StartActivity(typeof(MainActivity));
        }

        private void Reset(object sender, EventArgs e)
        {
           if(IsValidEmail(txtemail.Text.ToLower()) == true)
            {
                if (bd.Mail(txtemail.Text.ToLower()) == false)
                {
                    MailMessage mail = new MailMessage();
                    SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
                    mail.From = new MailAddress("FlashCardsCross@gmail.com");
                    mail.To.Add(txtemail.Text.ToLower());
                    code_random = random.Next(1000, 9999);
                    mail.Subject = "Код подтверждения";
                    mail.Body = "Введите этот код подтверждения в приложении для сброса пароля: " + code_random.ToString();
                    SmtpServer.Port = 587;
                    SmtpServer.Credentials = new System.Net.NetworkCredential("FlashCardsCross@gmail.com", "ZxCvBnM12345qwAS");
                    SmtpServer.EnableSsl = true;
                    ServicePointManager.ServerCertificateValidationCallback = delegate (object sender2, System.Security.Cryptography.X509Certificates.X509Certificate certificate, System.Security.Cryptography.X509Certificates.X509Chain chain, System.Net.Security.SslPolicyErrors sslPolicyErrors)
                    {
                        return true;
                    };
                    SmtpServer.Send(mail);
                    Toast.MakeText(Application.Context, "На почту отправлен код подтверждения", ToastLength.Short).Show();

                    LayoutInflater layoutInflater = LayoutInflater.From(this);
                    AlertDialog.Builder alert = new AlertDialog.Builder(this);
                    var view = layoutInflater.Inflate(Resource.Layout.code, null);
                    code = (EditText)view.FindViewById(Resource.Id.code);

                    alert.SetPositiveButton("Ок", Ok);
                    alert.SetNegativeButton("Отмена", HandleNegativeButtonClick);
                    alert.SetView(view);
                    Dialog dialog = alert.Create();
                    dialog.Show();
                }
                else
                {
                    Toast.MakeText(Application.Context, "Такой адрес не зарегистрирован", ToastLength.Short).Show();
                }
            }
        }

        private void HandleNegativeButtonClick(object sender, DialogClickEventArgs e)
        {
      
        }

        private void Ok(object sender, DialogClickEventArgs e)
        {
            if (code_random.ToString() == code.Text)
            {
                LayoutInflater layoutInflater = LayoutInflater.From(this);
                AlertDialog.Builder alert = new AlertDialog.Builder(this);
                var view = layoutInflater.Inflate(Resource.Layout.new_password, null);
                password = (EditText)view.FindViewById(Resource.Id.new_password);
                alert.SetPositiveButton("Ок", Change);
                alert.SetNegativeButton("Отмена", HandleNegativeButtonClick);
                alert.SetView(view);
                Dialog dialog = alert.Create();
                dialog.Show();            
            }
            else
            {
                LayoutInflater layoutInflater = LayoutInflater.From(this);
                AlertDialog.Builder alert = new AlertDialog.Builder(this);
                var view = layoutInflater.Inflate(Resource.Layout.code_false, null);
                code = (EditText)view.FindViewById(Resource.Id.code_false);

                alert.SetPositiveButton("Ок", Ok);
                alert.SetNegativeButton("Отмена", HandleNegativeButtonClick);
                alert.SetView(view);
                Dialog dialog = alert.Create();
                dialog.Show();
            }
        }

        private void Change(object sender, DialogClickEventArgs e)
        {
            if (password.Text.Length > 5)
            {
                bd.Change_password(txtemail.Text.ToLower(), password.Text);
                StartActivity(typeof(Main_menu_admin));
            }
            else
            {
                Toast.MakeText(Application.Context, "Пароль должен содержать больше 5 символов", ToastLength.Long).Show();
                LayoutInflater layoutInflater = LayoutInflater.From(this);
                AlertDialog.Builder alert = new AlertDialog.Builder(this);
                var view = layoutInflater.Inflate(Resource.Layout.new_password, null);
                password = (EditText)view.FindViewById(Resource.Id.new_password);
                alert.SetPositiveButton("Ок", Change);
                alert.SetNegativeButton("Отмена", HandleNegativeButtonClick);
                alert.SetView(view);
                Dialog dialog = alert.Create();
                dialog.Show();
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
    }
}