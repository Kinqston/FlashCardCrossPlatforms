using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;

namespace FlashCardsPort
{   
    class BaseData
    {
        public string new_id_deck;
        public int i = 0;
        public List<String> items_deck,items_deck_cost,items_deck_id, items_card_title, items_card_translate;       
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

        public void Delete_card(String id_deck, String word)
        {

            try
            {
                if (con.State == System.Data.ConnectionState.Closed)
                {
                    con.Open();
                    MySqlCommand cmd = new MySqlCommand("DELETE FROM cards WHERE (id_deck=@deck && word=@word)", con);
                    cmd.Parameters.AddWithValue("@deck", id_deck);
                    cmd.Parameters.AddWithValue("@word", word);
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
                        MySqlCommand cmd = new MySqlCommand("Insert INTO cards(id_deck,word,translate) VALUES (@deck,@word,@translate)", con);
                        cmd.Parameters.AddWithValue("@deck", id_deck);
                        cmd.Parameters.AddWithValue("@word", list[i].Word);
                        cmd.Parameters.AddWithValue("@translate", list[i].Translate);
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
            try
            {
                if (con.State == System.Data.ConnectionState.Closed)
                {
                    con.Open();
                    MySqlCommand cmd = new MySqlCommand("Select word,translate FROM cards WHERE id_deck=@deck", con);
                    cmd.Parameters.AddWithValue("@deck", id_deck);
                    using (MySqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                items_card_title.Add(dr.GetString(0));
                                items_card_translate.Add(dr.GetString(1));
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
        public void Update_cards(String id_deck, String old_word, String new_word, String old_translate, String new_translate)
        {
            try
            {
                if (con.State == System.Data.ConnectionState.Closed)
                {
                    con.Open();
                    MySqlCommand cmd = new MySqlCommand("Update cards SET word=@new_word, translate=@new_translate WHERE id_deck=@deck AND word=@old_word AND translate=@old_translate", con);
                    cmd.Parameters.AddWithValue("@new_word", new_word);
                    cmd.Parameters.AddWithValue("@new_translate", new_translate);
                    cmd.Parameters.AddWithValue("@old_word", old_word);
                    cmd.Parameters.AddWithValue("@deck", id_deck);                              
                    cmd.Parameters.AddWithValue("@old_translate", old_translate);
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