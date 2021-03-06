using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using admin_db;
using System.Text.RegularExpressions;
using MySql.Data.MySqlClient;

namespace admin
{
    public partial class Dashboard : Form
    {
        public Dashboard()
        {
            InitializeComponent();
        }

        public void button1_Click(object sender, EventArgs e, MySqlConnection conn, int sessionId)
        {
            // I need to add a categorie field to class the product in it
            if (this.name.Text != "" && this.productDescription.Text != "" && this.productPrice.Text != "" && this.productCategorie.Text != "")
            {
                try
                {
                    Console.WriteLine("Trying to create a new product ...");
                    MyProduct table = new MyProduct();
                    table.userId = sessionId; // Will change later with the user auth
                    table.cat = new MyCategorie();
                    table.cat.name = this.productCategorie.Text; // User has to choose a categorie
                    table.cat.ReadId(conn);
                    table.price = Double.Parse(this.productPrice.Text); // That field needs to be written with a ',' not a '.' !!!
                    table.name = this.name.Text;
                    table.desc = this.productDescription.Text;
                    table.Add(conn);
                    Console.WriteLine("Ended creating the product !");
                    // Show a success panel ...
                    // Refresh the displayed list
                }

                catch (Exception ex)
                {
                    // Show an error panel ...
                    Console.WriteLine("Error while adding the product : " + ex);
                }
            }

            else
            {
                Console.WriteLine("Form error : missing some fields !");
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void Products_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        // Events handling
        public void products_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            int index = this.Products.IndexFromPoint(e.Location);
            if (index != System.Windows.Forms.ListBox.NoMatches)
            {
                MessageBox.Show(index.ToString());
            }
        }

        public void categories_DoubleClick(object sender, EventArgs e, MySqlConnection conn, int sessionId)
        {
            if (this.Categories.SelectedItem != null)
            {
                string cat_name = this.Categories.SelectedItem.ToString();
                MyCategorie table = new MyCategorie();
                table.name = cat_name;
                table.Read(conn);
                // Display table.name and table.id values
            }
        }

        public int BoolToInt(bool boolean)
        {
            if (boolean) return 1;
            return 0;
        }

        public string GetAgeStr(string date)
        {
            if (date != "")
            {
                try
                {
                    Console.WriteLine("Getting age for " + date + "...");
                    DateTime d = DateTime.Parse(date);
                    DateTime now = DateTime.Now;
                    int age = now.Year - d.Year + BoolToInt(now.Month >= d.Month && now.Day >= d.Day);
                    Console.WriteLine("Got age !");
                    return age.ToString();
                }

                catch(Exception ex)
                {
                    Console.WriteLine(ex);
                    return "";
                }
            }

            else
            {
                Console.WriteLine("Error : age str empty");
                return "";
            }
        }

        public void users_DoubleClick(object sender, EventArgs e, MySqlConnection conn, int sessionId)
        {
            if (this.Users.SelectedItem != null)
            {
                string user_username = this.Users.SelectedItem.ToString();
                Console.WriteLine("Selected {0} !", user_username);
                MyUser table = new MyUser();
                table.username = user_username;
                table.Read(conn);
                table.ReadId(conn);
                UserItem item = new UserItem();
                item.firstname.Text = table.firstname;
                item.lastname.Text = table.lastname;
                Console.WriteLine("Getting age for date " + table.bornAt);
                item.age.Text = GetAgeStr(table.bornAt); // Get age from date of birth
                Console.WriteLine("Got age !");
                item.username.Text = table.username;
                item.email.Text = table.email;
                table.CountCategories(conn);
                table.CountProducts(conn);
                item.nbProducts.Text = table.nbProducts.ToString();
                item.nbCategories.Text = table.nbCategories.ToString();
                // Must count other tables for the rest
                item.nbSells.Text = "10";
                item.nbPurchases.Text = "5";
                item.ShowDialog();
            }
        }

        public void products_DoubleClick(object sender, EventArgs e, MySqlConnection conn, int sessionId)
        {
            if (this.Products.SelectedItem != null)
            {
                string product_name = this.Products.SelectedItem.ToString();
                Console.WriteLine("Selected {0} !", product_name);
                MyProduct table = new MyProduct();
                table.cat = new MyCategorie();
                table.cat.id = -1; // To reset the id
                table.name = product_name;
                table.Read(conn);
                ProductItem item = new ProductItem();
                item.productName.Text = table.name;
                item.productAddedAt.Text = table.addedAt; // Get age from date of birth
                item.productDescription.Text = table.desc;
                item.productPrice.Text = table.price.ToString();
                // table.CountSells(connection);
                table.GetCategorie(conn);
                item.productSells.Text = table.nbSells.ToString();
                item.productCategorie.Text = table.cat.name.ToString();
                // Must count other tables for the rest
                item.ShowDialog();
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }
    }
}
