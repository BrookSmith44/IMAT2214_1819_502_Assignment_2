using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IMAT2214_1819_502_Assignment_2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        // Function to split the dates
        private void splitDates (string rawData)
        {
            // Array to collect the data from the parameter and enter it into local array
            string[] arrayDate = rawData.Split('/');
            // Integer variable to store day
            Int32 day = Convert.ToInt32(arrayDate[0]);
            // Integer variable to store month
            Int32 month = Convert.ToInt32(arrayDate[2]);
            // Integer variable to store year
            Int32 year = Convert.ToInt32(arrayDate[3]);

            // Creates date from three seperate integers
            DateTime myDate = new DateTime(year, month, day);

            // String Variable to store the day of the week
            string dayOfWeek = myDate.DayOfWeek.ToString();
            // Integer variable to store the day of the year
            Int32 dayOfYear = myDate.DayOfYear;
            // String variable to store the name of the month
            string monthName = myDate.ToString("MMMM");
            // Integer variable to store the week number
            Int32 weekNumber = myDate.DayOfYear / 7;
            // Boolean variable to store whether it is currently the weekend or not
            Boolean weekend = false;
            // If statement to check if it is the weekend
            if (dayOfWeek == "Saturday" || dayOfWeek == "Sunday") weekend = true;
        }

        private void btnGetData_Click(object sender, EventArgs e)
        {

            // Create list for the dates to be collected in
            List<string> Dates = new List<string>();

            // Create list for the customer info to be collected in
            List<string> Customers = new List<string>();

            // Create list for the customer info to be collected in
            List<string> Products = new List<string>();

            // Create the database string
            string connectionString = Properties.Settings.Default.Data_set_1ConnectionString;

            // Create a boundary for the object to be used - Object will be destroyed at the end of te block
            using (OleDbConnection connection = new OleDbConnection(connectionString))
            {
                // Opens a filestream to the specified path
                connection.Open();
                // Reader provides a way of reading forward only stream of data rows from a data source
                OleDbDataReader reader = null;

                // Dates Dimension - Get Dates from dataset 

                // getDates allows us to write a query in order to get the data from the rows we want
                OleDbCommand getDates = new OleDbCommand("SELECT [Order Date], [Ship Date] FROM Sheet1", connection);

                // Sends the command query to the reader
                reader = getDates.ExecuteReader();

                // Call read before accessing data - while will keep reading until there are no results left
                while (reader.Read())
                {
                    // Add date to the list from the first column
                    Dates.Add(reader[0].ToString());
                    // Add date to the list from the second colun
                    Dates.Add(reader[1].ToString());
                }

                // Create new list for dates without timestamp
                List<string> DatesFormatted = new List<string>();

                // For loop that goes through each date in the list
                foreach(string date in Dates)
                {
                    // Split creates array of substrings by splitting original string, split by empty space
                    var dates = date.Split(new char[0], StringSplitOptions.RemoveEmptyEntries);
                    // Add new split date to new DatesFormatted List
                    DatesFormatted.Add(dates[0]);
                }

                // Splitting the DatesFormatted into day, month and year
                string[] arrayDate = DatesFormatted[0].ToString().Split('/');
                // Display new split day, month and year in console log
                Console.WriteLine("Day: " + arrayDate[0] + "  Month: " + arrayDate[1] + " Year: " + arrayDate[2]);


                // Customer Dimension - Get Customer info from data set 

                // Query to get all the relevant Customer Info from the data set
                OleDbCommand getCustomerInfo = new OleDbCommand("SELECT [Customer ID], [Customer Name], Country, City, State, [Postal Code], Region FROM Sheet1", connection);

                // Executes the sql query
                reader = getCustomerInfo.ExecuteReader();

                //Read through all results
                while (reader.Read())
                {
                    // Add first column to the product list
                    Customers.Add(reader[0].ToString());
                    // Add second column to the product list
                    Customers.Add(reader[1].ToString());
                    // Add third column to the product list
                    Customers.Add(reader[2].ToString());
                    // Add fourth column to the product list
                    Customers.Add(reader[3].ToString());
                    // Add fifth column to the product list
                    Customers.Add(reader[4].ToString());
                    // Add sixth column to the product list
                    Customers.Add(reader[5].ToString());
                    // Add seventh column to the product list
                    Customers.Add(reader[6].ToString());
                }

                // Product Dimension = Get Product info from data set

                // Query to get all the relevatn customer info from the data set
                OleDbCommand getProductInfo = new OleDbCommand("SELECT [Product ID], [Product Name], Category, [Sub-Category] FROM Sheet1", connection);

                // Executes the query
                reader = getProductInfo.ExecuteReader();

                // Read through all results
                while (reader.Read())
                {
                    // Add first column to the product list
                    Products.Add(reader[0].ToString());
                    // Add second column to the product list
                    Products.Add(reader[1].ToString());
                    // Add third column to the product list
                    Products.Add(reader[2].ToString());
                    // Add fourth column to the product list
                    Products.Add(reader[3].ToString());
                }

                // Display the populated Dates list in the console window
                //DatesFormatted.ForEach(Console.WriteLine);

                // Display the populated Customer list in the console window
                //Customers.ForEach(Console.WriteLine);

                // Display the populated Products list in the console window
                //Products.ForEach(Console.WriteLine);

            }
        }
    }
}
