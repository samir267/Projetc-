using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SQLite;

namespace Projectc_
{
    public partial class Form1 : Form
    {
        string path = "person_table.db";
        string cs = @"Data Source=" + Application.StartupPath + "\\person_table.db;Version=3;";

        SQLiteConnection con;
        SQLiteCommand cmd;
        SQLiteDataReader reader;


        public Form1()
        {
            InitializeComponent();
        }

        private void get_Data()
        {
            var con = new SQLiteConnection(cs);
            con.Open();
            string getdata = "select * from person";
            var cmd = new SQLiteCommand(getdata, con);
            reader = cmd.ExecuteReader();

            while (reader.Read())
            {

                String[] row = new string[] { reader.GetInt64(0).ToString(), reader.GetString(1), reader.GetString(2), reader.GetValue(3).ToString(), reader.GetString(4), reader.GetValue(5).ToString() };
                dataGridView1.Rows.Add(row);
            }

        }

        private void create_db()
        {
            if (!System.IO.File.Exists(path))
            {
                SQLiteConnection.CreateFile(path);
                using (var sqlite = new SQLiteConnection(@"Data Source=" + path))
                {
                    sqlite.Open();
                    string sql = "create table person(id integer PRIMARY KEY AUTOINCREMENT,first_name varchar(20),last_name varchar(20),phone_number integer,address varchar(20),age integer)";
                    SQLiteCommand command = new SQLiteCommand(sql, sqlite);
                    command.ExecuteNonQuery();

                }
            }
            else
            {
                Console.WriteLine("problem database");
                return;
            }
        }


        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            create_db();
            get_Data();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var con = new SQLiteConnection(cs);
            con.Open();

            var cmd = new SQLiteCommand(con);
            cmd.CommandText = "insert into person(first_name,last_name,phone_number,address,age)values(@first,@last,@phone,@adr,@age)";

            String txt1 = textBox1.Text;
            String txt2 = textBox2.Text;
            String txt3 = textBox3.Text;
            String txt4 = textBox4.Text;
            String txt5 = textBox5.Text;

            cmd.Parameters.AddWithValue("@first", txt1);
            cmd.Parameters.AddWithValue("@last", txt2);
            cmd.Parameters.AddWithValue("@phone", txt3);
            cmd.Parameters.AddWithValue("@adr", txt4);
            cmd.Parameters.AddWithValue("@age", txt5);
            cmd.ExecuteNonQuery();
            long lastInsertedId = con.LastInsertRowId;


            dataGridView1.ColumnCount = 10;


            String[] row = new string[] { lastInsertedId.ToString(), txt1, txt2, txt3, txt4, txt5 };
            dataGridView1.Rows.Add(row);

            con.Close();


        }






        private void Update_Click(object sender, EventArgs e)
        {
            using (var con = new SQLiteConnection(cs))
            {
                con.Open();

                string idToUpdate = textBox6.Text;

                String txt1 = textBox1.Text;
                String txt2 = textBox2.Text;
                String txt3 = textBox3.Text;
                String txt4 = textBox4.Text;
                String txt5 = textBox5.Text;

                var updateCmd = new SQLiteCommand(con);
                updateCmd.CommandText = "UPDATE person SET first_name=@first, last_name=@last, phone_number=@phone, address=@adr, age=@age WHERE id=@id";
                updateCmd.Parameters.AddWithValue("@first", txt1);
                updateCmd.Parameters.AddWithValue("@last", txt2);
                updateCmd.Parameters.AddWithValue("@phone", txt3);
                updateCmd.Parameters.AddWithValue("@adr", txt4);
                updateCmd.Parameters.AddWithValue("@age", txt5);
                updateCmd.Parameters.AddWithValue("@id", idToUpdate);
                updateCmd.ExecuteNonQuery();
                dataGridView1.Rows.Clear();
                get_Data();
            }
        }



        private void button2_Click(object sender, EventArgs e)
        {
            using (var con = new SQLiteConnection(cs))
            {
                con.Open();

                string idToUpdate = textBox6.Text;
                var cmd = new SQLiteCommand("SELECT * FROM person WHERE id = @id", con);
                cmd.Parameters.AddWithValue("@id", idToUpdate);

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        textBox1.Text = reader.GetString(1);
                        textBox2.Text = reader.GetString(2);
                        textBox3.Text = reader.GetValue(3).ToString();
                        textBox4.Text = reader.GetString(4);
                        textBox5.Text = reader.GetValue(5).ToString(); 
                    }

                }
            }
        }

        private void Delete_Click(object sender, EventArgs e)
        {
            using (var con = new SQLiteConnection(cs))
            {
                con.Open();

                string idToUpdate = textBox6.Text;
                var cmd = new SQLiteCommand("delete FROM person WHERE id = @id", con);
                cmd.Parameters.AddWithValue("@id", idToUpdate);
                cmd.ExecuteNonQuery();
                dataGridView1.Rows.Clear();
                get_Data();
            }
        }
    }
}

       
