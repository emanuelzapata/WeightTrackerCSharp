using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Reflection;
using System.IO;


namespace Weight_Tracker_with_DB
{
    public partial class Form1 : Form
    {
        //conn is placed here so it it accessible to other functions;
        MySqlConnection conn = new MySqlConnection();
        MySqlConnection gdConn = new MySqlConnection(); // test connection for MYSQL online database from goDaddy. WORKING!
        bool connectionStatus;
        public Form1()
        {
            InitializeComponent();
          /*  try
            {
                string connection = "Server=198.71.227.96;Database=weightTracker;Uid=emanuelzapata;Pwd=Sk8ordie!6";
                gdConn.ConnectionString = connection;
                gdConn.Open();
                if(gdConn.State == ConnectionState.Open)
                {
                    MessageBox.Show("Connected!");
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }*/
            initializeImages();
            startPopup();
            connectionStatus = connectToDatabase();
            //this.FormClosing used to initialize the closing function on close;
            this.FormClosing += Form1_FormClosing;
            if(connectionStatus == true)
            {
                //change color;
                pictureBox2.ImageLocation = "C:\\Users\\Emanuel\\Desktop\\Weight Program C#\\Weight Tracker with DB\\greenDot.png";
               
            }
            else
            {
                pictureBox2.ImageLocation = "C:\\Users\\Emanuel\\Desktop\\Weight Program C#\\Weight Tracker with DB\\redDot.png";
            }
            displayDatabase();
        }
        //function to check for input value if its greater or less than goal.
        private void goalCheck(string input)
        {
            string[] lines = File.ReadAllLines("settings.txt");
            double i = Convert.ToDouble(input);
            double goalWeight = Convert.ToDouble(lines[1]);
            double startingWeight = Convert.ToDouble(lines[0]);
            if(i>=startingWeight)
            {
                MessageBox.Show("BE CAREFUL! Your current weight is over your starting weight! YOURE GAINING WEIGHT!", "WARNING!");
            }
            else if (i <= goalWeight)
            {
                MessageBox.Show("CONGRATULATIONS! You have lost more/equal than your goal weight! Keep it off this time!", "CONGRATULATIONS!");
            }
            else
            {
                //do nothing
            }

        }

        //function connects to database and displays a popup message to confirm message or error;
        private bool connectToDatabase()
        {
            string connectionString = "Server=127.0.0.1;Port=330;Database=weight_storage;Uid=root;Pwd=skateordie;";
            try
            {
                conn.ConnectionString = connectionString;
                conn.Open();
                if (conn.State == ConnectionState.Open)
                {
                    //MessageBox.Show("Connected!","Weight Tracker");
                    return true;
                }
                else return false;
            }
            catch (MySqlException ex)
            {
                //MessageBox.Show(ex.Message);
                return false;
            }
        }

        //actions that are performed during the SubmitButton click;
        private void submitButton_Click(object sender, EventArgs e)
        {
            insert();
            goalCheck(textBox1.Text);
            listBox1.Items.Clear();
            displayDatabase();
        }

        //Will determin popup notifications when closing program and db disconnect;
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                conn.Close();
                if (conn.State == ConnectionState.Closed)
                {
                    MessageBox.Show("Connection Closed", "Weight Tracker");
                }
            }
            catch(MySqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        //will display database
        private void displayDatabase()
        {
            List<string>[] list = new List<string>[2];
            list[0] = new List<string>();
            list[1] = new List<string>();
            string query = "SELECT * FROM weights_dates";
            if (connectionStatus == true)
            {
                //reads in from SQL to array of lists;
                MySqlCommand cmd = new MySqlCommand(query, conn);
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while(dataReader.Read())
                {
                    list[0].Add(dataReader["weight"] + "");
                    list[1].Add(dataReader["date"] + "");
                }
                dataReader.Close();  
            }
            int size = list[0].Count;
            for(int x = 0; x<size;x++)
            {
                string weightFormat = "Weight: " + list[0][x]+"-";
                string dateFormat = "-Date: " + Convert.ToDateTime(list[1][x]).ToString("MM-dd-yyyy"); 
                listBox1.Items.Add(weightFormat.PadLeft(40) + dateFormat.PadLeft(20));
               
                
            }
        }
        //inserts into database after reading in from input on submit button
        private void insert()
        {
            //SQL INSERT string example -> INSERT INTO weightstorage.weightsdates(weight,date) VALUES('212.0','2016-6-6');
            string date = DateTime.Now.ToString("yyyy-MM-dd");
            string query = "INSERT INTO weight_storage.weights_dates(weight,date) VALUES('" + textBox1.Text + "','" + date+"')";
            try
            {
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.ExecuteNonQuery();
            }
            catch(MySqlException ex)
            {
                MessageBox.Show(ex.Message,"Weight Tracker");
            }
        }
        private void initializeImages()
        {
            /*
            Assembly myAssembly = Assembly.GetExecutingAssembly();
            Stream myStream = myAssembly.GetManifestResourceStream("Title.png");
            Bitmap image = new Bitmap(myStream);
            pictureBox1.Image = image;
             * */
        }
        private void startPopup()
        {
            if (!File.Exists("settings.txt"))
            {
                popUp pop = new popUp();
                pop.ShowDialog();
                using (StreamWriter settings = File.CreateText("settings.txt"))
                {
                    settings.WriteLine(pop.textBox1.Text);
                    settings.WriteLine(pop.textBox2.Text);
                }
                string[] lines = File.ReadAllLines("settings.txt");
                label2.Text = "Starting Weight: " + lines[0];
                label3.Text = "Goal Weight: " + lines[1];
            }
            else
            {
                string[] lines = File.ReadAllLines("settings.txt");
                label2.Text = "Starting Weight: " + lines[0];
                label3.Text = "Goal Weight: " + lines[1];
                //cWeight = Convert.ToDouble(lines[0]);
                //gWeight = Convert.ToDouble(lines[1]);
            }
        }
        //
        private void Form1_Load(object sender, EventArgs e)
        {

        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
        //will sort db by descending;
        private void sortButton_Click(object sender, EventArgs e)
        {
            List<string>[] list = new List<string>[2];
            list[0] = new List<string>();
            list[1] = new List<string>();
            //SELECT * FROM weight_storage.weights_dates order by date desc;
            listBox1.Items.Clear();
            string query = "SELECT * FROM weight_storage.weights_dates order by date desc;";
            try
            {
                MySqlCommand cmd = new MySqlCommand(query, conn);
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    list[0].Add(dataReader["weight"] + "");
                    list[1].Add(dataReader["date"] + "");
                }
                dataReader.Close();  
                //cmd.ExecuteNonQuery();
                int size = list[0].Count;
                for (int x = 0; x < size; x++)
                {
                    string weightFormat = "Weight: " + list[0][x] + "-";
                    string dateFormat = "-Date: " + Convert.ToDateTime(list[1][x]).ToString("MM-dd-yyyy");
                    listBox1.Items.Add(weightFormat.PadLeft(40) + dateFormat.PadLeft(20));
                }
            }
            catch(MySqlException ex)
            {
                MessageBox.Show(ex.Message,"Weight Tracker");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            displayDatabase();
        }
        //graph button click
        private void graphButton_Click(object sender, EventArgs e)
        {
            List<string>[] list = new List<string>[2];
            list[0] = new List<string>();
            list[1] = new List<string>();

            string query = "SELECT * FROM weights_dates";
            if (connectionStatus == true)
            {
                //reads in from SQL to array of lists;
                MySqlCommand cmd = new MySqlCommand(query, conn);
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    list[0].Add(dataReader["weight"] + "");
                    list[1].Add(dataReader["date"] + "");
                }
                dataReader.Close();
            }
            graphForm graph = new graphForm();
            graph.getPoints(list);
            graph.ShowDialog();
        }        
    }
}
