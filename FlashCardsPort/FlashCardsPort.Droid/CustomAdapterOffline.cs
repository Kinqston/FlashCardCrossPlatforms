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
using Java.Util;
using Android.Graphics;
using System.Net;
using System.IO;

namespace FlashCardsPort.Droid
{
    class CustomAdapterOffline : ArrayAdapter
    {
        private Context c;
        public Android.Net.Uri uri;
        public List<Card> cards;
        private int resourse;
        private LayoutInflater inflater;
        string ftpHost = "ftp.billions-consult.ru";
        string ftpUser = "u688865617.kinkston";
        string ftpPassword = "kinkston";
        string fileName;
        Bitmap image_bitmap;
        FtpWebResponse ftpResponse;
        FtpWebRequest ftpRequest;
        public CustomAdapterOffline(Context context,int resourse, List<Card> objects):base(context, resourse, objects)
        {
            this.c = context;
            this.resourse = resourse;
            this.cards = objects;
        }
        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            if (inflater == null)
            {
                inflater = (LayoutInflater)c.GetSystemService(Context.LayoutInflaterService);
            }
            if (convertView == null)
            {
                convertView = inflater.Inflate(resourse, parent, false);
            }
            MyHolder holder = new MyHolder(convertView);
            holder.word.Text = cards[position].Word;
            holder.translate.Text = cards[position].Translate;
            if (cards[position].Bitmap_image != null)
            {
                holder.image.SetImageBitmap(cards[position].Bitmap_image);
            }
            else
            {
                if (cards[position].Image == null)
                {
                    holder.image.SetImageBitmap(null);
                }
                else{
                    Android.Net.Uri uri = Android.Net.Uri.FromFile(new Java.IO.File(cards[position].Image));
                    holder.image.SetImageURI(uri);
                }
            }         
            // holder.image.SetImageBitmap(cards[position].Image);
            //FtpWebRequest ftp = (FtpWebRequest)FtpWebRequest.Create(cards[position].Image);
            //ftp.Credentials = new NetworkCredential(ftpUser, ftpPassword);
            ////команда фтп RETR
            //ftp.Method = WebRequestMethods.Ftp.DownloadFile;
            ////Файлы будут копироваться в кталог программы
            //FileStream downloadedFile = new FileStream(fileName, FileMode.Create, FileAccess.ReadWrite);

            //ftpResponse = (FtpWebResponse)ftpRequest.GetResponse();
            ////Получаем входящий поток
            //Stream responseStream = ftpResponse.GetResponseStream();

            ////Буфер для считываемых данных
            //byte[] buffer = new byte[1024];
            //int size = 0;

            //while ((size = responseStream.Read(buffer, 0, 1024)) > 0)
            //{
            //    downloadedFile.Write(buffer, 0, size);

            //}
            //ftpResponse.Close();
            //downloadedFile.Close();
            //responseStream.Close();
            //Console.WriteLine(downloadedFile);
            ////  holder.image.SetImageBitmap(imageBitmap);
            return convertView;
        }
        private Bitmap GetImageBitmapFromUrl(string url)
        {
            Bitmap imageBitmap = null;

            using (var webClient = new WebClient())
            {
                var imageBytes = webClient.DownloadData(url);
                if (imageBytes != null && imageBytes.Length > 0)
                {
                    imageBitmap = BitmapFactory.DecodeByteArray(imageBytes, 0, imageBytes.Length);
                }
            }

            return imageBitmap;
        }
        //public byte[] GetImgByte(string ftpFilePath)
        //{
        //    WebClient ftpClient = new WebClient();
        //    ftpClient.Credentials = new NetworkCredential(ftpUser, ftpPassword);

        //    byte[] imageByte = ftpClient.DownloadData(ftpFilePath);
        //    return imageByte;
        //}
        // public static Bitmap ByteToImage(byte[] blob)
        //{
        //    System.IO.MemoryStream mStream = new System.IO.MemoryStream();
        //    byte[] pData = blob;
        //    mStream.Write(pData, 0, Convert.ToInt32(pData.Length));
        //    Bitmap bm = new Bitmap(mStream);
        //    mStream.Dispose();
        //    return bm;

        //}
    }
}