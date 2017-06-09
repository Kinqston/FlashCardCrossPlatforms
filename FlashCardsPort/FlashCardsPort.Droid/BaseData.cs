using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using System.IO;
using Android.Graphics;
using System.ComponentModel;
using System.Net;

namespace FlashCardsPort
{   
    class BaseData
    {
        string ftpHost = "ftp.billions-consult.ru";
        string ftpUser = "graversp_fc";
        string ftpPassword = "{*545S7e";
        string filename;
        string ftpfullpath;

        string image_null;

        public string new_id_deck;
        public int i = 0;
        public List<String> items_deck,items_deck_cost,items_deck_id, items_card_title, items_card_id, items_card_translate, items_card_image;
        public byte image_byte;
        public List<Bitmap> bitmap;      
        public string[] decks = new string[10];
         public MySqlConnection con = new MySqlConnection("Server=31.220.20.81;port=3306;database=u688865617_flash;User Id=u688865617_flash;Password = kinkston;charset=utf8");
        //public MySqlConnection con = new MySqlConnection("Server=31.220.20.8;port=3306;database=u688865617_flash;User Id=u688865617_flash;Password = kinkston;charset=utf8");
   //     public MySqlConnectionStringBuilder mysqlbuilder = new MySqlConnectionStringBuilder();
   //     public MySqlConnection con;
        public void connection()
        {
            //mysqlbuilder.Server = "sql11.freemysqlhosting.net";  // IP адоес БД
            //mysqlbuilder.Database = "sql11172481";    // Имя БД
            //mysqlbuilder.UserID = "sql11172481";        // Имя пользователя БД
            //mysqlbuilder.Password = "MpcHhtCag1";   // Пароль пользователя БД
            //mysqlbuilder.CharacterSet = "cp1251"; // Кодировка Базы Данных
            //con = new MySqlConnection(mysqlbuilder.ConnectionString);
            //mysqlbuilder.Server = "31.220.20.81";  
            //mysqlbuilder.Port = 3306;
            //mysqlbuilder.Database = "u688865617_flash";    
            //mysqlbuilder.UserID = "u688865617_flash";        
            //mysqlbuilder.Password = "kinkston";   
            //mysqlbuilder.CharacterSet = "cp1251"; 
            //con = new MySqlConnection(mysqlbuilder.ConnectionString);
        }

        public void Delete_card(String id_deck, String word, String Cards_id)
        {

            try
            {
                if (con.State == System.Data.ConnectionState.Closed)
                {
                    con.Open();
                    MySqlCommand cmd = new MySqlCommand("DELETE FROM cards WHERE (id_deck=@deck && id=@id_cards)", con);
                    cmd.Parameters.AddWithValue("@deck", id_deck);
                    cmd.Parameters.AddWithValue("@id_cards", Cards_id);
                    cmd.ExecuteNonQuery();
                    con.Close();
                    //MySqlCommand cmd = new MySqlCommand();
                }
            }
            catch (MySqlException ex)
            {

            }
            finally
            {
                con.Close();
            }
        }
        public void Change_password(String email, String password)
        {
            try
            {
                if (con.State == System.Data.ConnectionState.Closed)
                {
                    con.Open();
                    MySqlCommand cmd = new MySqlCommand("Update users SET password=@password WHERE email=@email", con);
                    cmd.Parameters.AddWithValue("@email", email);
                    cmd.Parameters.AddWithValue("@password", password);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (MySqlException ex)
            {

            }
            finally
            {
                con.Close();
            }

        }
        public bool Mail(String email)
        {
            try
            {
                if (con.State == System.Data.ConnectionState.Closed)
                {
                    con.Open();
                    MySqlCommand cmd = new MySqlCommand("Select email FROM users WHERE email=@email", con);
                    cmd.Parameters.AddWithValue("@email", email);
                    using (MySqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                return false;                        
                            }
                            dr.NextResult();
                        }
                    }
                    con.Close();
                }
            }
            catch (MySqlException ex)
            {

            }
            finally
            {
                con.Close();
            }
            return true;
        }
        public void User_Registration(String email, String pass)
        {
            try
            {
                if (con.State == System.Data.ConnectionState.Closed)
                {
                    con.Open();
                    MySqlCommand cmd = new MySqlCommand("Insert INTO users(email,password) VALUES (@email,@pass)", con);
                    cmd.Parameters.AddWithValue("@email",email);
                    cmd.Parameters.AddWithValue("@pass", pass);
                    cmd.ExecuteNonQuery();                  
                }
            }
            catch (MySqlException ex)
            {
                
            }
            finally
            {
                con.Close();
            }
        }
        public string Login(String email, String pass)
        {
            try
            {
                if (con.State == System.Data.ConnectionState.Closed)
                {
                    con.Open();
                    MySqlCommand cmd = new MySqlCommand("Select id FROM users WHERE email=@email and password=@password", con);
                    cmd.Parameters.AddWithValue("@email", email);
                    cmd.Parameters.AddWithValue("@password", pass);
                    using (MySqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                return dr.GetString(0);                               
                            }
                            dr.NextResult();
                        }
                    }
                    con.Close();
                }
                
            }
            catch (MySqlException ex)
            {
 
            }
            finally
            {
                con.Close();
            }
            return "false";
        }
        public void Add_deck(String title, String cost)
        {
            try
            {
                if (con.State == System.Data.ConnectionState.Closed)
                {
                    con.Open();
                    MySqlCommand cmd = new MySqlCommand("Insert INTO decks(title,Cost) VALUES (@title,@cost)", con);
                    cmd.Parameters.AddWithValue("@title", title);
                    cmd.Parameters.AddWithValue("@cost", cost);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (MySqlException ex)
            {

            }
            finally
            {
                con.Close();
            }
        }
        public void New_deck_id(String title, String cost)
        {
            try
            {
                if (con.State == System.Data.ConnectionState.Closed)
                {
                    con.Open();
                    MySqlCommand cmd = new MySqlCommand("Select id FROM decks WHERE title=@deck AND cost=@cost", con);
                    cmd.Parameters.AddWithValue("@deck", title);
                    cmd.Parameters.AddWithValue("@cost", cost);
                    using (MySqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                new_id_deck = dr.GetString(0);
                            }
                            dr.NextResult();
                        }
                    }
                    con.Close();
                }
            }
            catch (MySqlException ex)
            {

            }
            finally
            {
                con.Close();
            }
        }
        public void Add_deck_cards(String id_deck, List<FlashCardsPort.Droid.Card> list)
        {
            try
            {
                if (con.State == System.Data.ConnectionState.Closed)
                {
                    con.Open();
                    for (int i = 0; i < list.Count; i++)
                    {
                        MySqlCommand cmd = new MySqlCommand("Insert INTO cards(id_deck,word,translate,image) VALUES (@deck,@word,@translate,@image)", con);
                        cmd.Parameters.AddWithValue("@deck", id_deck);
                        cmd.Parameters.AddWithValue("@word", list[i].Word);
                        cmd.Parameters.AddWithValue("@translate", list[i].Translate);

                        //filename = "file_" + Guid.NewGuid().ToString() + ".jpg";
                        //ftpfullpath = "ftp://graversp.beget.tech/public_html/" + list[i].Image;
                        //FtpWebRequest ftp = (FtpWebRequest)FtpWebRequest.Create(ftpfullpath);
                        //ftp.Credentials = new NetworkCredential(ftpUser, ftpPassword);
                        //ftp.KeepAlive = true;
                        //ftp.UseBinary = true;
                        //ftp.Method = WebRequestMethods.Ftp.UploadFile;
                        ////Android.Net.Uri url = Android.Net.Uri.Parse(ImagePath);

                        //System.IO.FileStream fs = System.IO.File.OpenRead(list[i].Image);
                        //byte[] buffer = new byte[fs.Length];
                        //fs.Read(buffer, 0, buffer.Length);
                        //fs.Close();
                        //System.IO.Stream ftpstream = ftp.GetRequestStream();
                        //ftpstream.Write(buffer, 0, buffer.Length);
                        //ftpstream.Close();
                        //ftpstream.Flush();
                        if (list[i].Image == null)
                        {
                            cmd.Parameters.AddWithValue("@image", " ");
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@image", list[i].Image);
                        }
                        //MemoryStream stream = new MemoryStream();
                        //list[i].image.Compress(Bitmap.CompressFormat.Png, 0, stream);
                        //byte[] bitmapData = stream.ToArray();
                        //byte[] bitmapData;
                        //using (var stream = new MemoryStream())
                        //{
                        //    list[i].image.Compress(Bitmap.CompressFormat.Png, 0, stream);
                        //    bitmapData = stream.ToArray();
                        //}
                        // cmd.Parameters.AddWithValue("@image", bitmapData);
                        // cmd.Parameters.AddWithValue("@image", list[i].Image);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (MySqlException ex)
            {

            }
            finally
            {
                con.Close();
            }
        }

        public void Decks_list()
        {
            items_deck = new List<String>();
            items_deck_cost = new List<String>();
            items_deck_id = new List<String>();
            try
            {
                if (con.State == System.Data.ConnectionState.Closed)
                {
                    con.Open();
                    MySqlCommand cmd = new MySqlCommand("Select id, title, cost FROM decks", con);
                    using (MySqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                items_deck_id.Add(dr.GetString(0));
                                items_deck.Add(dr.GetString(1));
                                items_deck_cost.Add(dr.GetString(2));
                            }
                            dr.NextResult();
                        }
                    }
                    con.Close();
                }
            }
            catch (MySqlException ex)
            {

            }
            finally
            {
                con.Close();
            }
        }
        public void Update_deck(String id_deck, String title, String cost)
        {
            try
            {
                if (con.State == System.Data.ConnectionState.Closed)
                {
                    con.Open();
                    MySqlCommand cmd = new MySqlCommand("Update decks SET title=@title, cost=@cost WHERE id=@deck", con);
                    cmd.Parameters.AddWithValue("@title", title);
                    cmd.Parameters.AddWithValue("@cost", cost);
                    cmd.Parameters.AddWithValue("@deck", id_deck);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (MySqlException ex)
            {

            }
            finally
            {
                con.Close();
            }
        }
        public void Delete_all_card(String id_deck)
        {
            try
            {
                if (con.State == System.Data.ConnectionState.Closed)
                {
                    con.Open();
                    MySqlCommand cmd = new MySqlCommand("DELETE FROM cards WHERE id_deck=@deck", con);
                    cmd.Parameters.AddWithValue("@deck", id_deck);
                    cmd.ExecuteNonQuery();
                    con.Close();
                    //MySqlCommand cmd = new MySqlCommand();
                }
            }
            catch (MySqlException ex)
            {

            }
            finally
            {
                con.Close();
            }
        }
        public void delete_item_list(String id_deck)
        {
            try
            {
                if (con.State == System.Data.ConnectionState.Closed)
                {
                    con.Open();
                    MySqlCommand cmd = new MySqlCommand("DELETE FROM cards WHERE id_deck=@deck", con);
                    cmd.Parameters.AddWithValue("@deck", id_deck);
                    cmd.ExecuteNonQuery();
                    MySqlCommand cmd1 = new MySqlCommand("DELETE FROM `user-deck` WHERE id_deck=@deck", con);
                    cmd1.Parameters.AddWithValue("@deck", id_deck);
                    cmd1.ExecuteNonQuery();
                    MySqlCommand cmd2 = new MySqlCommand("DELETE FROM decks WHERE id=@deck", con);
                    cmd2.Parameters.AddWithValue("@deck", id_deck);
                    cmd2.ExecuteNonQuery();
                    con.Close();
                    //MySqlCommand cmd = new MySqlCommand();
                }
            }
            catch (MySqlException ex)
            {

            }
            finally
            {
                con.Close();
            }
        }
        public void Cards_list(String id_deck)
        {
            items_card_title= new List<String>();
            items_card_translate= new List<String>();
            items_card_image = new List<String>();
            items_card_id = new List<String>();
            bitmap = new List<Bitmap>();
            try
            {
                if (con.State == System.Data.ConnectionState.Closed)
                {
                    con.Open();
                    MySqlCommand cmd = new MySqlCommand("Select id,word,translate,image FROM cards WHERE id_deck=@deck", con);
                    cmd.Parameters.AddWithValue("@deck", id_deck);
                    using (MySqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                items_card_id.Add(dr.GetString(0));
                                items_card_title.Add(dr.GetString(1));
                                items_card_translate.Add(dr.GetString(2));
                                image_null = dr.GetString(3);
                                if (image_null == " ")
                                {
                                    items_card_image.Add(null);
                                    bitmap.Add(null);
                                }
                                else
                                {
                                    items_card_image.Add(image_null);
                                    bitmap.Add(GetImageBitmapFromUrl("http://graversp.beget.tech/" + image_null));
                                }
                             
                                //  Console.WriteLine(dr.GetString(2)+"   o    ");
                                //ordinal = dr.GetOrdinal("Image");

                                //Console.WriteLine(byte_image + "    byte    ");
                                //bitmap_image = bytesToBitmap(byte_image);
                                //Console.WriteLine(ms+"    bitmap    ");
                                //byte_image = (byte[])dr.Exe
                                //bitmap_image = bytesToBitmap(byte_image);
                                //Console.WriteLine(bitmap_image);
                                // bitmap.Add(bitmap_image);
                                // byte_image = (byte[])dr.GetValue(2);

                                //bitmap_image = bytesToBitmap(byte_image);
                                // bitmap.Add();
                                //TypeConverter tc = TypeDescriptor.GetConverter(typeof(Bitmap));
                                //Bitmap bitmap1 = (Bitmap)tc.ConvertFrom(dr.GetByte(2));
                                //bitmap.Add(bitmap1);
                            }
                            dr.NextResult();
                        }
                    }                 
                    con.Close();
                }
            }
            catch (MySqlException ex)
            {

            }
            finally
            {
                con.Close();
            }
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
        public static Bitmap bytesToBitmap(byte[] imageBytes)
        {

            Bitmap bitmap = BitmapFactory.DecodeByteArray(imageBytes, 0, imageBytes.Length);

            return bitmap;
        }
        public void Update_cards(String id_card, String id_deck, String old_word, String new_word, String old_translate, String new_translate, String image)
        {
            try
            {
                if (con.State == System.Data.ConnectionState.Closed)
                {
                    con.Open();
                    MySqlCommand cmd = new MySqlCommand("Update cards SET word=@new_word, translate=@new_translate, image=@image WHERE id_deck=@deck AND id=@id_card", con);
                    cmd.Parameters.AddWithValue("@new_word", new_word);
                    cmd.Parameters.AddWithValue("@new_translate", new_translate);
                    cmd.Parameters.AddWithValue("@deck", id_deck);
                    if (image == null)
                    {
                        cmd.Parameters.AddWithValue("@image", " ");
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@image", image);
                    }
                    cmd.Parameters.AddWithValue("@id_card", id_card);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (MySqlException ex)
            {

            }
            finally
            {
                con.Close();
            }
        }
    }   
}