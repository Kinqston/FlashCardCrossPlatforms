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
using Android.Views.InputMethods;
using Java.Net;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Net.Mail;
using System.Net;
using Java.Security.Cert;
using System.Security.Cryptography.X509Certificates;

namespace FlashCardsPort.Droid
{
    [Activity(Label = "Регистрация")]
    public class Sign_up : Activity
    {
        static BaseData bd = new BaseData();
        String Id_user;
        TextView txtlog, login, forgot_password;
        EditText txtpass, txtemail;
        EditText code;
        Button Register;
        Random random = new Random();
        HttpURLConnection conn;
        string password;
        int code_random;
        string server_name = "http://kinkston.esy.es";
        string res;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.sign_up);
            // Create your application here
            Register = FindViewById<Button>(Resource.Id.signup_btn_register);
            txtemail = FindViewById<EditText>(Resource.Id.signup_email);
            txtpass = FindViewById<EditText>(Resource.Id.signup_password);
            login = FindViewById<TextView>(Resource.Id.signup_btn_login);
            forgot_password = FindViewById<TextView>(Resource.Id.signup_btn_forgot_password);

            ActionBar actionBar = ActionBar;
            actionBar.SetDisplayShowHomeEnabled(false);

            login.Click += Login;
            Register.Click += Register_user;
            forgot_password.Click += Forgot_Password;

        }

        private void Forgot_Password(object sender, EventArgs e)
        {
            StartActivity(typeof(Forgot_password));
        }

        private void Register_user(object sender, EventArgs e)
        {
            InputMethodManager inputManager = (InputMethodManager)this.GetSystemService(Context.InputMethodService);
            inputManager.HideSoftInputFromWindow(this.CurrentFocus.WindowToken, HideSoftInputFlags.NotAlways);
            // bd.User_Registration(txtemail.Text, txtpass.Text);
            //Id_user = bd.Login(txtemail.Text, txtpass.Text);
            // соберем линк для передачи новой строки
            //string post_url = server_name
            //        + "/register.php?action=insert&email="
            //        + txtemail.Text
            //        + "&password="
            //        + txtpass.Text;

            //URL url = new URL(post_url);
            //conn = (HttpURLConnection)url.OpenConnection();
            //conn.Connect();
            bool re = IsValidEmail(txtemail.Text.ToLower());
            if (IsValidEmail(txtemail.Text.ToLower()) == true)
            {
                if (bd.Mail(txtemail.Text.ToLower()) == true)
                {
                    if(txtpass.Text.Length > 5)
                    {
                        try
                        {
                            MailMessage mail = new MailMessage();
                            SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
                            mail.From = new MailAddress("FlashCardsCross@gmail.com");
                            mail.To.Add(txtemail.Text.ToLower());
                            code_random = random.Next(1000, 9999);
                            mail.Subject = "Код подтверждения";
                            mail.Body = "Введите этот код подтверждения в приложении: " + code_random.ToString();
                            SmtpServer.Port = 587;
                            SmtpServer.Credentials = new System.Net.NetworkCredential("FlashCardsCross@gmail.com", "ZxCvBnM12345qwAS");
                            SmtpServer.EnableSsl = true;
                            ServicePointManager.ServerCertificateValidationCallback = delegate (object sender2, System.Security.Cryptography.X509Certificates.X509Certificate certificate, X509Chain chain, System.Net.Security.SslPolicyErrors sslPolicyErrors)
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
                        catch (Exception ex)
                        {
                            Toast.MakeText(Application.Context, "Ошибка при отправке кода подтверждения на почту", ToastLength.Long).Show();
                        }
                    }
                    else
                    {
                        Toast.MakeText(Application.Context, "Пароль должен быть больше 5 символов", ToastLength.Long).Show();
                    }
                }
                else
                {
                    Toast.MakeText(Application.Context, "Эта почта уже зарегистрирована", ToastLength.Short).Show();
                }
            }
            else
            {
                Toast.MakeText(Application.Context, "Неверно введена почта", ToastLength.Long).Show();
            }
        }

        private void HandleNegativeButtonClick(object sender, DialogClickEventArgs e)
        {
           
        }

        private void Ok(object sender, DialogClickEventArgs e)
        {
            if (code_random.ToString() == code.Text)
            {
                bd.User_Registration(txtemail.Text.ToLower(), txtpass.Text);
                Id_user = bd.Login(txtemail.Text.ToLower(), txtpass.Text);
                StartActivity(typeof(Main_menu_admin));
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
    
            private void Login(object sender, EventArgs e)
            {
                StartActivity(typeof(MainActivity));
            }

        }

    }