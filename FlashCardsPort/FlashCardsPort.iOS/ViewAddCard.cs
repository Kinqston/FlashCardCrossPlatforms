using Foundation;
using System;
using UIKit;
using System.Collections.Generic;
using Media.Plugin;
using Media.Plugin.Abstractions;
using System.Net;

namespace FlashCardsPort.iOS
{
    public partial class ViewAddCard : UIViewController
    {
		public string word, translate, title_deck, flag, image_edit;
		public List<Cards_item> cards;
		string ftpHost = "ftp.billions-consult.ru";
		string ftpUser = "graversp_fc";
		string ftpPassword = "{*545S7e";
		public string filename;
		string ftpfullpath;
		public string photoPath;
        public ViewAddCard (IntPtr handle) : base (handle)
        {
        }
		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			Title = title_deck;
			Word.Text = word;
			Translate.Text = translate;
			image.Image = FromUrl("http://graversp.beget.tech/" + image_edit);
			Gallery.TouchUpInside += async(sender, e) =>
		   {
			   if (CrossMedia.Current.IsPickPhotoSupported)
			   {
				    MediaFile photo = await CrossMedia.Current.PickPhotoAsync();
					UIImage imagesoursce = UIImage.FromFile(photo.Path);
				    photoPath = photo.Path;
					image.Image = imagesoursce;
					filename = "file_" + Guid.NewGuid().ToString() + ".jpg";
					ftpfullpath = "ftp://graversp.beget.tech/public_html/" + filename;
					Console.WriteLine(filename);
		            FtpWebRequest ftp = (FtpWebRequest)FtpWebRequest.Create(ftpfullpath);
					ftp.Credentials = new NetworkCredential(ftpUser, ftpPassword);
					ftp.KeepAlive = true;
		            ftp.UseBinary = true;
		            ftp.Method = WebRequestMethods.Ftp.UploadFile;
		            //Android.Net.Uri url = Android.Net.Uri.Parse(ImagePath);
					System.IO.FileStream fs = System.IO.File.OpenRead(photo.Path);
					byte[] buffer = new byte[fs.Length];
					fs.Read(buffer, 0, buffer.Length);
		            fs.Close();
		            System.IO.Stream ftpstream = ftp.GetRequestStream();
					ftpstream.Write(buffer, 0, buffer.Length);
		            ftpstream.Close();
		            ftpstream.Flush();
                }

            };

			Camera.TouchUpInside += async(sender, e) => {
                if (CrossMedia.Current.IsCameraAvailable && CrossMedia.Current.IsTakePhotoSupported)
                {
                    MediaFile file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
					{

						Directory = "Sample",
						Name = $"{DateTime.Now.ToString("dd.MM.yyyy_hh.mm.ss")}.jpg"
					});
 
                    if (file == null)
                        return;
 
                    //img.Source = ImageSource.FromFile(file.Path);
                }
            };
		}

		partial void UIButton1780_TouchUpInside(UIButton sender)
		{
			word = Word.Text;
			translate = Translate.Text;

		}
		UIImage FromUrl(string uri)
		{
			using (var url = new NSUrl(uri))
			using (var data = NSData.FromUrl(url))
				return UIImage.LoadFromData(data);
		}
	}
}