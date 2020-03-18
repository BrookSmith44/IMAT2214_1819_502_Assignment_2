using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
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

        // Function to split day, month and year
        private void splitDayMonthYear(string rawData)
        {
            // Array to collect the data from the parameter and enter it into local array
            string[] arrayDate = rawData.Split('/');
            // Integer variable to store day
            Int32 day = Convert.ToInt32(arrayDate[0]);
            // Integer variable to store month
            Int32 month = Convert.ToInt32(arrayDate[1]);
            // Integer variable to store year
            Int32 year = Convert.ToInt32(arrayDate[2]);

            // Creates date from three seperate integers
            DateTime myDate = new DateTime(year, month, day);

            // Convert this to a database friendly format
            string dbDate = myDate.ToString("M/dd/yyyy");

            // Call function to split the dates
            splitDates(myDate, dbDate, day, month, year);
        }

        // Function to get the IDs for the dimensions
        private int getIDs(string date)
        {
            // Create a connection to the MDF file
            string connectionStringDestination = Properties.Settings.Default.DestinationDatabaseConnectionString;

            // Create a boundary for the object to be used - Object will be destroyed at the end of te block
            using (SqlConnection myConnection = new SqlConnection(connectionStringDestination))
            {
                // Open the SQL connection
                myConnection.Open();
                // Check if the date already exists in the database - NO DUPLICATES
                SqlCommand command = new SqlCommand("SELECT Time.id FROM Time WHERE Time.date = @date" +
                    "UNION" +
                    "SELECT Customer.id FROM Customer WHERE Customer.id = @id" +
                    "UNION" +
                    "SELECT Product.id FROM Product WHERE Product.id = @id",
                    myConnection);
                // Add a reference to @date
                command.Parameters.Add(new SqlParameter("date", date));

                // Run the command and read the results
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    // If there are results then the date exists so change the boolean to true
                    if (reader.HasRows)
                    {
                        // Store IDs in array
                        int[] id = new int[] { Convert.ToInt32(reader["Time.id"]), Convert.ToInt32(reader["Customer.id"]), Convert.ToInt32(reader["Product.id"]) };
                        // Return the id the reader retrieves from the database
                        return Convert.ToInt32(reader["id"]);
                    }
                }
            }

            // private int requires a return value
            return 0;
        }

        // Function to split the dates
        private void splitDates(DateTime myDate, string dbDate, Int32 day, Int32 month, Int32 year)
        {
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

            // Execute function to insert data into the time dimension
            insertTimeDimension(dbDate, dayOfWeek, day, monthName, month, weekNumber, year, weekend, dayOfYear);
        }

        // Function to split the customer info
        private void splitCustomers(string rawData)
        {
            // Array to collect the data from the parameter and enter it into local array
            string[] arrayCustomer = rawData.Split('/');

            // Customer info
            // string to store the customer id as reference
            string reference = arrayCustomer[0];
            // String to store the customer name
            string customerName = arrayCustomer[1];
            // String to store the country
            string country = arrayCustomer[2];
            // String to store the city
            string city = arrayCustomer[3];
            // String to store the state
            string state = arrayCustomer[4];
            // String to store the postcode
            string postcode = arrayCustomer[5];
            // String to store the region
            string region = arrayCustomer[6];

            // Execute the query to insert the data into the customer dimension
            insertCustomerDimension(reference, customerName,  country, city, state, postcode, region);
        }

        // Function to split the product info
        private void splitProducts(string rawData)
        {
            // Array to collect the data from the products list, split by the slashes between
            string[] arrayProduct = rawData.Split('/');

            // String to store the product id as reference
            string reference = arrayProduct[0];
            // String to store the product name
            string productName = arrayProduct[1];
            // String to store the category
            string category = arrayProduct[2];
            // String to store the subcategory
            string subcategory = arrayProduct[3];

            // Call function to insert data into product dimension
            insertProductDimension(reference, category, subcategory, productName);

        }

        private void insertTimeDimension(string date, string dayName, Int32 dayNumber, string monthName, Int32 monthNumber, Int32 weekNumber, Int32 year, Boolean weekend, Int32 dayOfYear)
        {
            // Create a connection to the MDF file
            string connectionStringDestination = Properties.Settings.Default.DestinationDatabaseConnectionString;

            // Create a boundary for the object to be used - Object will be destroyed at the end of te block
            using (SqlConnection myConnection = new SqlConnection(connectionStringDestination))
            {
                // Open the SQL connection
                myConnection.Open();
                // Check if the date already exists in the database - NO DUPLICATES
                SqlCommand command = new SqlCommand("SELECT id FROM Time WHERE date = @date", myConnection);
                // Add a reference to @date
                command.Parameters.Add(new SqlParameter("date", date));

                // Create a boolean variable and set it to false as default
                Boolean exists = false;

                // Run the command and read the results
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    // If there are results then the date exists so change the boolean to true
                    if (reader.HasRows) exists = true;
                }

                // if there are no rows 
                if (exists == false)
                {
                    // SQL command to inset data into the time dimension table
                    SqlCommand insertCommand = new SqlCommand(
                        "INSERT INTO Time (dayName, dayNumber, monthName, monthNumber, weekNumber, year, weekend, date, dayOfYear)" +
                        "VALUES (@dayName, @dayNumber, @monthName, @monthNumber, @weekNumber, @year, @weekend, @date, @dayOfYear)",
                        myConnection);
                    // Add a reference to @dayName
                    insertCommand.Parameters.Add(new SqlParameter("dayName", dayName));
                    // Add a reference to @dayNumber
                    insertCommand.Parameters.Add(new SqlParameter("dayNumber", dayNumber));
                    // Add a reference to @monthName
                    insertCommand.Parameters.Add(new SqlParameter("monthName", monthName));
                    // Add a reference to @monthNumber
                    insertCommand.Parameters.Add(new SqlParameter("monthNumber", monthNumber));
                    // Add a reference to @weekNumber
                    insertCommand.Parameters.Add(new SqlParameter("weekNumber", weekNumber));
                    // Add a reference to @year
                    insertCommand.Parameters.Add(new SqlParameter("year", year));
                    // Add a reference to @weekend
                    insertCommand.Parameters.Add(new SqlParameter("weekend", weekend));
                    // Add a reference to @date
                    insertCommand.Parameters.Add(new SqlParameter("date", date));
                    // Add a reference to @dayOfYear
                    insertCommand.Parameters.Add(new SqlParameter("dayOfYear", dayOfYear));

                    // Insert the line 
                    int recordAffected = insertCommand.ExecuteNonQuery();
                    Console.WriteLine("Records Affected: " + recordAffected);
                }
            }
        }

        // Function to insert data into the customer dimension
        private void insertCustomerDimension(string reference, string customerName, string country, string city, string state, string postcode, string region)
        {
            // Create a connection to the MDF file
            string connectionStringDestination = Properties.Settings.Default.DestinationDatabaseConnectionString;

            // Create a boundary for the object to be used - Object will be destroyed at the end of te block
            using (SqlConnection myConnection = new SqlConnection(connectionStringDestination))
            {
                // Open the SQL connection
                myConnection.Open();
                // Check if the customer already exists in the database - NO DUPLICATES
                SqlCommand command = new SqlCommand("SELECT id FROM Customer WHERE reference = @reference", myConnection);
                // Add a reference to @reference
                command.Parameters.Add(new SqlParameter("reference", reference));

                // Create a boolean variable and set it to false as default
                Boolean exists = false;

                // Run the command and read the results
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    // If there are results then the customer exists so change the boolean to true
                    if (reader.HasRows) exists = true;
                }

                // if there are no rows 
                if (exists == false)
                {
                    // SQL command to inset data into the customer dimension table
                    SqlCommand insertCommand = new SqlCommand(
                        "INSERT INTO Customer (name, country, city, state, postalCode, region, reference)" +
                        "VALUES (@name, @country, @city, @state, @postalCode, @region, @reference)",
                        myConnection);
                    // Add a reference to @name
                    insertCommand.Parameters.Add(new SqlParameter("name", customerName));
                    // Add a reference to @country
                    insertCommand.Parameters.Add(new SqlParameter("country", country));
                    // Add a reference to @city
                    insertCommand.Parameters.Add(new SqlParameter("city", city));
                    // Add a reference to @state
                    insertCommand.Parameters.Add(new SqlParameter("state", state));
                    // Add a reference to @postalCode
                    insertCommand.Parameters.Add(new SqlParameter("postalCode", postcode));
                    // Add a reference to @region
                    insertCommand.Parameters.Add(new SqlParameter("region", region));
                    // Add a reference to @reference
                    insertCommand.Parameters.Add(new SqlParameter("reference", reference));

                    // Insert the line 
                    int recordAffected = insertCommand.ExecuteNonQuery();
                    Console.WriteLine("Records Affected: " + recordAffected);
                }
            }
        }

        private void insertProductDimension(string reference, string category, string subcategory, string productName)
        {
            // Create a connection to the MDF file
            string connectionStringDestination = Properties.Settings.Default.DestinationDatabaseConnectionString;

            // Create a boundary for the object to be used - Object will be destroyed at the end of te block
            using (SqlConnection myConnection = new SqlConnection(connectionStringDestination))
            {
                // Open the SQL connection
                myConnection.Open();
                // Check if the product already exists in the database - NO DUPLICATES
                SqlCommand command = new SqlCommand("SELECT id FROM Product WHERE reference = @reference", myConnection);
                // Add a reference to @reference
                command.Parameters.Add(new SqlParameter("reference", reference));

                // Create a boolean variable and set it to false as default
                Boolean exists = false;

                // Run the command and read the results
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    // If there are results then the customer exists so change the boolean to true
                    if (reader.HasRows) exists = true;
                }

                // if there are no rows 
                if (exists == false)
                {
                    // SQL command to inset data into the product dimension table
                    SqlCommand insertCommand = new SqlCommand(
                        "INSERT INTO Product (category, subcategory, name, reference)" +
                        "VALUES (@category, @subcategory, @name, @reference)",
                        myConnection);
                    // Add a reference to @category
                    insertCommand.Parameters.Add(new SqlParameter("category", category));
                    // Add a reference to @subcategory
                    insertCommand.Parameters.Add(new SqlParameter("subcategory", subcategory));
                    // Add a reference to @name
                    insertCommand.Parameters.Add(new SqlParameter("name", productName));
                    // Add a reference to @reference
                    insertCommand.Parameters.Add(new SqlParameter("reference", reference));

                    // Insert the line 
                    int recordAffected = insertCommand.ExecuteNonQuery();
                    Console.WriteLine("Records Affected: " + recordAffected);
                }
            }
        }

        // Activates when get data button is clicked
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

                // For each loop that goes through each date in the list
                foreach (string date in Dates)
                {
                    // Split creates array of substrings by splitting original string, split by empty space
                    var dates = date.Split(new char[0], StringSplitOptions.RemoveEmptyEntries);
                    // Add new split date to new DatesFormatted List
                    DatesFormatted.Add(dates[0]);
                }

                // For each loop goes through each date in DatesFormatted list
                foreach (string date in DatesFormatted)
                {
                    // Call function to split the dates with the date as a parameter
                    splitDayMonthYear(date);
                }

                // Splitting the DatesFormatted into day, month and year
                string[] arrayDate = DatesFormatted[0].ToString().Split('/');
                // Display new split day, month and year in console log
                //Console.WriteLine("Day: " + arrayDate[0] + "  Month: " + arrayDate[1] + " Year: " + arrayDate[2]);


                // Customer Dimension - Get Customer info from data set 

                // Query to get all the relevant Customer Info from the data set
                OleDbCommand getCustomerInfo = new OleDbCommand("SELECT [Customer ID], [Customer Name], Country, City, State, [Postal Code], Region FROM Sheet1", connection);

                // Executes the sql query
                reader = getCustomerInfo.ExecuteReader();

                //Read through all results
                while (reader.Read())
                {
                    // Add the customers found in the query by the reader into a string
                    Customers.Add(reader[0].ToString() + "/" +
                        reader[1].ToString() + "/" +
                        reader[2].ToString() + "/" +
                        reader[3].ToString() + "/" +
                        reader[4].ToString() + "/" +
                        reader[5].ToString() + "/" +
                        reader[6].ToString());
                }

                // For each loop goes through each customers info
                foreach (string customer in Customers)
                {
                    // Console to check how to split customer info
                    Console.WriteLine(customer);
                    // Call to split customer function with customer as a parameter
                    splitCustomers(customer);
                }
                // Product Dimension = Get Product info from data set

                // Query to get all the relevatn customer info from the data set
                OleDbCommand getProductInfo = new OleDbCommand("SELECT [Product ID], [Product Name], Category, [Sub-Category] FROM Sheet1", connection);

                // Executes the query
                reader = getProductInfo.ExecuteReader();

                // Read through all results
                while (reader.Read())
                {
                    // Add the products found in the query by the reader into a string
                    Products.Add(reader[0].ToString() + "/" +
                        reader[1].ToString() + "/" +
                        reader[2].ToString() + "/" +
                        reader[3].ToString());
                }

                // For each loop to go through all product info
                foreach (string product in Products) {
                    // Call the function to split the product info 
                    splitProducts(product);
                }

                // Display the populated Dates list in the console window
                //DatesFormatted.ForEach(Console.WriteLine);

                // Display the populated Customer list in the console window
                //Customers.ForEach(Console.WriteLine);

                // Display the populated Products list in the console window
                //Products.ForEach(Console.WriteLine);

            }
        }

        private void btnGetDestinationData_Click(object sender, EventArgs e)
        {
            // Time Dimension
            // Create a list to store the dates data im
            List<string> destinationDates = new List<string>();

            // Product Dimension
            // Create a list to store the product data in
            List<string> DestinationProducts = new List<string>();

            // Set the datasource to null to stop program from crashing
            listBoxProductDestination.DataSource = null;
            // Clear listbox to make sure there is no old data there
            listBoxProductDestination.Items.Clear();

            // Create a connection to MDF File
            string connectionStringDestination = Properties.Settings.Default.DestinationDatabaseConnectionString;

            // Create a boundary for the object to be used - Object will be destroyed at the end of te block
            using (SqlConnection myConnection = new SqlConnection(connectionStringDestination))
            {
                // Open sql connection
                myConnection.Open();

                // Time Dimension
                SqlCommand datesCommand = new SqlCommand("SELECT id, dayName, dayNumber, monthName, monthNumber, weekNumber, year," +
                    "weekend, date, dayOfYear FROM Time", myConnection);

                // Product Dimension
                // Check if the product already exists in the destination database
                SqlCommand ProductCommand = new SqlCommand("SELECT id, category, subcategory, name, reference FROM Product", myConnection);

                // Create a boundary in which the reader can be used to read the data from the sql query
                using (SqlDataReader reader = datesCommand.ExecuteReader())
                {
                    // If there is data 
                    if (reader.HasRows)
                    {
                        // Loop to read through the data
                        while (reader.Read())
                        {
                            // Display data from the Time table in the listbox
                            string id = reader["id"].ToString();
                            string dayName = reader["dayName"].ToString();
                            string dayNumber = reader["dayNumber"].ToString();
                            string monthName = reader["monthName"].ToString();
                            string monthNumber = reader["monthNumber"].ToString();
                            string year = reader["year"].ToString();
                            string weekend = reader["weekend"].ToString();
                            string date = reader["date"].ToString();
                            string dayOfYear = reader["dayOfYear"].ToString();

                            // string to store data as detailed text
                            string text = "ID: " + id + ", Day Name: " + dayName + ", Day Number: " + dayNumber +
                                ", Month Name: " + monthName + ", Month Number: " + monthNumber +
                                ", Year: " + year + ", Weekend: " + weekend + ", Date: " + date +
                                ", Day of the Year: " + dayOfYear;

                            // Add text string to list
                            destinationDates.Add(text);
                        }
                    }
                    // Else if there is no data
                    else
                    {
                        destinationDates.Add("No data present in the product dimension");
                    }
                }

                // Create a boundary in which the reader can be used to read the data from the sql query
                using (SqlDataReader reader = ProductCommand.ExecuteReader())
                {
                    // If there is data 
                    if(reader.HasRows)
                    {
                        // Loop to read through the data
                        while(reader.Read())
                        {
                            // Display data from the Product tabke in the listbox
                            string id = reader["id"].ToString();
                            string category = reader["category"].ToString();
                            string subcategory = reader["subcategory"].ToString();
                            string name = reader["name"].ToString();
                            string reference = reader["reference"].ToString();

                            // string to store data as detailed text
                            string text = "ID: " + id + ", Category: " + category + ", Subcategory: " + 
                                ", Name: " + name + ", Reference: " + reference ;

                            // Add text string to list
                            DestinationProducts.Add(text);
                        }
                    }
                    // Else if there is no data
                    else
                    {
                        DestinationProducts.Add("No data present in the product dimension");
                    }
                }
            }

            // Bind the listbox to list
            listBoxProductDestination.DataSource = DestinationProducts;

            // Display list in console
            foreach (string dates in destinationDates)
            {
                Console.WriteLine(dates);
            }

        }
    }
}
