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
using System.Runtime.InteropServices;
using System.Windows.Forms.DataVisualization.Charting;
using System.Text.RegularExpressions;

namespace IMAT2214_1819_502_Assignment_2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        // Create regex to check if string is numeric
        //https://regex101.com/r/sL6zP7/2/codegen?language=csharp
        private static readonly Regex regexInt = new Regex(@"^[0-9]{0,9}$");
        private static readonly Regex regexDecimal = new Regex(@"^\d{0,9}\[.]\ \d{0,9}");
        // Customer ID
        //https://stackoverflow.com/questions/18898700/regex-for-combination-of-letters-numbers-w-special-characters/18899449
        private static readonly Regex regexCustomerID = new Regex(@"[A-Z]{2}[-]\d{0,6}");
        // Regex for product ID
        private static readonly Regex regexProductID = new Regex(@"[A-Z]{3}[-][A-Z]{2}[-]\d{0,9}");


        // Appearance buttons

        // Move form by panel
        //https://stackoverflow.com/questions/11379209/how-do-i-make-mousedrag-inside-panel-move-form-window
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();

        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }
        // Button to close the program
        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }
        // Button to maximise application
        private void btnMaximise_Click(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Normal)
            {
                WindowState = FormWindowState.Maximized;
            }
            else
            {
                WindowState = FormWindowState.Normal;
            }
        }
        // Button to minimize application
        private void btnMinimize_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Combo boxes
            // Time Dimension
            // Set combo box items for time sales selection
            comboBoxTimeSales.Items.AddRange(new Object[] { "Weeks", "Months", "Years" });
            comboBoxTimeSales.SelectedItem = "Weeks";

            // Set combo box items for time profit selection
            comboBoxTimeProfit.Items.AddRange(new Object[] { "Days", "Weeks", "Months", "Years" });
            comboBoxTimeProfit.SelectedItem = "Days";

            // Set combo box items for time quantity selection
            comboBoxTimeQuantity.Items.AddRange(new Object[] { "Days", "Weeks", "Months", "Years" });
            comboBoxTimeQuantity.SelectedItem = "Days";

            // Set combo box items for time value selection
            comboBoxTimeValue.Items.AddRange(new Object[] { "Days", "Weeks", "Months", "Years" });
            comboBoxTimeValue.SelectedItem = "Days";

            // Set combo box items for time discount selection
            comboBoxTimeDiscount.Items.AddRange(new Object[] { "Days", "Weeks", "Months", "Years" });
            comboBoxTimeDiscount.SelectedItem = "Days";

            // Customer Dimension
            // Set combo box items for customer sales selection
            comboBoxCustomerSales.Items.AddRange(new Object[] { "Cities", "States", "Regions", "Countries", "Postcodes" });
            comboBoxCustomerSales.SelectedItem = "Cities";

            // Set combo box items for customer profit selection
            comboBoxCustomerProfit.Items.AddRange(new Object[] { "Cities", "States", "Regions", "Countries", "Postcodes" });
            comboBoxCustomerProfit.SelectedItem = "Cities";

            // Set combo box items for customer quantity selection
            comboBoxCustomerQuantity.Items.AddRange(new Object[] { "Cities", "States", "Regions", "Countries", "Postcodes" });
            comboBoxCustomerQuantity.SelectedItem = "Cities";

            // Set combo box items for customer value selection
            comboBoxCustomerValue.Items.AddRange(new Object[] { "Cities", "States", "Regions", "Countries", "Postcodes" });
            comboBoxCustomerValue.SelectedItem = "Cities";

            // Set combo box items for customer discount selection
            comboBoxCustomerDiscount.Items.AddRange(new Object[] { "Cities", "States", "Regions", "Countries", "Postcodes" });
            comboBoxCustomerDiscount.SelectedItem = "Cities";

            // Product Dimension
            // Set combo box items for product sales selection
            comboBoxProductSales.Items.AddRange(new Object[] { "Names", "Categories", "Sub-Categories" });
            comboBoxProductSales.SelectedItem = "Names";

            // Set combo box items for product profit selection
            comboBoxProductProfit.Items.AddRange(new Object[] { "Names", "Categories", "Sub-Categories" });
            comboBoxProductProfit.SelectedItem = "Names";

            // Set combo box items for product quantity selection
            comboBoxProductQuantity.Items.AddRange(new Object[] { "Names", "Categories", "Sub-Categories" });
            comboBoxProductQuantity.SelectedItem = "Names";

            // Set combo box items for product value selection
            comboBoxProductValue.Items.AddRange(new Object[] { "Names", "Categories", "Sub-Categories" });
            comboBoxProductValue.SelectedItem = "Names";

            // Set combo box items for product discount selection
            comboBoxProductDiscount.Items.AddRange(new Object[] { "Names", "Categories", "Sub-Categories" });
            comboBoxProductDiscount.SelectedItem = "Names";
        }

        // Function to split day, month and year
        static Tuple<Int32, Int32, Int32, DateTime, string> ReturnDayMonthYearData(String rawData)
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

            return Tuple.Create<Int32, Int32, Int32, DateTime, string>(day, month, year, myDate, dbDate);
        }

        private void DisplayGraphs(string connectionString, string SQLstring, List<string> list, Dictionary<string, dynamic> fact, Chart chart, string value)
        {
                // loop through dates list
                foreach (string listItem in list)
                {
                    //  Create space to access the database
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        // Open the connection
                        connection.Open();

                        SqlCommand command = new SqlCommand(SQLstring, connection);
                        // Set parameter for @date
                        command.Parameters.Add(new SqlParameter("@selection", listItem));

                        // Create reader to read the database data
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            // Check the query returns data
                            if (reader.HasRows)
                            {
                                // Get the data
                                while (reader.Read())
                                {
                                    if (value == "sales" || value =="quantity")
                                    {
                                        // Add the sales count to the date
                                        fact.Add(listItem, Int32.Parse(reader[0].ToString()));
                                    }
                                    if (value == "profit" || value == "value" || value == "discount")
                                    {
                                        fact.Add(listItem, Decimal.Parse(reader[0].ToString()));
                                        Console.WriteLine(reader[0].ToString());
                                    }
                                }
                            }
                            // If no data is returned
                            else
                            {
                                // Add 0 sales to date
                                fact.Add(listItem, 0);
                            }
                        }
                    }
                }

                // Building the chart
                chart.DataSource = fact;
                chart.Series[0].XValueMember = "Key";
                chart.Series[0].YValueMembers = "Value";
                chart.DataBind();
        }

        private string CreateNewID(string initials, string[] SQLString)
        {
            // Create string for reference 
            String reference = "";
            // Get number for reference
            // Switch statement so code can loop to generate if reference already exists
            switch (1)
            {
                case 1:
                    // Create random number generator
                    Random rndGenerator = new Random();

                    // Create string with random number in it
                    string referenceNo = rndGenerator.Next(10000, 9999999).ToString();
                    Console.WriteLine(referenceNo);

                    // Create reference as a whole
                    reference = initials + referenceNo;

                    // Go to next case
                    goto case 2;
                case 2:
                    // Create Connection string for data set 1
                    String connectionString = Properties.Settings.Default.Data_set_1ConnectionString;

                    using (OleDbConnection connection = new OleDbConnection(connectionString))
                    {
                        // Open the connection
                        connection.Open();

                        // Create reader 
                        OleDbDataReader reader = null;

                        // Create command to check if reference already exists
                        OleDbCommand command = new OleDbCommand(SQLString[0], connection);
                        // Create parameter for reference
                        command.Parameters.Add(new OleDbParameter("@reference", reference));

                        // link reader and command 
                        reader = command.ExecuteReader();

                        if (reader.HasRows)
                        {
                            goto case 1;
                        }
                        else
                        {
                            goto case 3;
                        }
                    }
                case 3:
                    // Create Connection string for data set 2
                    String connectionString2 = Properties.Settings.Default.DataSet2_1_ConnectionString;

                    using (OleDbConnection connection = new OleDbConnection(connectionString2))
                    {
                        // Open the connection
                        connection.Open();

                        // Create reader 
                        OleDbDataReader reader = null;

                        // Create command to check if reference already exists
                        OleDbCommand command = new OleDbCommand(SQLString[1], connection);
                        // Create parameter for reference
                        command.Parameters.Add(new OleDbParameter("@reference", reference));

                        // link reader and command 
                        reader = command.ExecuteReader();

                        if (reader.HasRows)
                        {
                            goto case 2;
                        }
                        else
                        {
                            break;
                        }
                    }
            }

            // Return value
            return reference;
        }

        // Function to get the IDs for the dimensions
        private int getIDs(string date, string customerID, string productID, string dimension, bool corrupt)
        {
            // Remove the timestamp from the date
            var dateSplit = date.Split(new char[0], StringSplitOptions.RemoveEmptyEntries);
            // Set new date
            date = dateSplit[0];
            // Crate dimension string
            Int32 dimensionID = 0;
            // Store tuple method as result
            Tuple<Int32, Int32, Int32, DateTime, String> result = ReturnDayMonthYearData(date);
            // Store db compatible date as string
            string dbDate = result.Item5;
            // Create empty string for sql
            string SQLString = "";
            // Choose SQL String based on if ID is corrupt
            if (corrupt == false)
            {
                SQLString = "SELECT DISTINCT c.id AS id_customer, p.id AS id_product, t.id AS id_time, t.date, c.reference, p.reference FROM Customer c, Product p, Time t " +
                    "WHERE t.date = @date AND c.reference = @customerid AND p.reference = @productid";
            } else
            {
                SQLString = "SELECT DISTINCT c.id AS id_customer, p.id AS id_product, t.id AS id_time, t.date, c.reference, p.reference FROM Customer c, Product p, Time t " +
                    "WHERE t.date = @date AND c.name = @customerid AND p.reference = @productid";
            }
            // Create a connection to the MDF file
            string connectionStringDestination = Properties.Settings.Default.DestinationDatabaseConnectionString;

            // Create a boundary for the object to be used - Object will be destroyed at the end of te block
            using (SqlConnection myConnection = new SqlConnection(connectionStringDestination))
            {
                // Open the SQL connection
                myConnection.Open();
                // Check if the date already exists in the database - NO DUPLICATES
                
                SqlCommand command = new SqlCommand(SQLString, myConnection);

                // Add a reference to @date
                command.Parameters.Add(new SqlParameter("date", dbDate));
                // Add a reference to @customerid
                command.Parameters.Add(new SqlParameter("customerid", customerID));
                // Add a reference to @productid
                command.Parameters.Add(new SqlParameter("productid", productID));

                // Run the command and read the results
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    // If there are results then the date exists so change the boolean to true
                    if (reader.HasRows)
                    {
                        // Loop while the reads data
                        while (reader.Read())
                        {
                            // If statement to if the time dimension has been selected
                            if (dimension == "Time")
                            {
                                // Return time id
                                dimensionID = Convert.ToInt32(reader["id_time"]);
                            }
                            else if (dimension == "Product")
                            {
                                // Return product id
                                dimensionID = Convert.ToInt32(reader["id_product"]);
                            }
                            else if (dimension == "Customer")
                            {
                                // Return customer product
                                dimensionID = Convert.ToInt32(reader["id_customer"]);
                            }
                        }
                    }
                }
            }

            // private int requires a return value
            return dimensionID;
        } 
 
        // Function to split the dates
        private void splitDates(string rawData)
        {

            Tuple<Int32, Int32, Int32, DateTime, string> result = ReturnDayMonthYearData(rawData);
            // int to store day
            Int32 day = result.Item1;
            // int to store month
            Int32 month = result.Item2;
            // int to store year
            Int32 year = result.Item3;
            // DateTime to store date
            DateTime myDate = result.Item4;
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

            // string to store database compatable date
            string dbDate = result.Item5;

            // Execute function to insert data into the time dimension
            insertTimeDimension(dbDate, dayOfWeek, day, monthName, month, weekNumber, year, weekend, dayOfYear);
        }

        // Function to split the customer info
        private void splitCustomers(string rawData)
        {
            // Array to collect the data from the parameter and enter it into local array
            string[] arrayCustomer = rawData.Split('/');

            // Customer info
            // String to store the customer name
            string customerName = arrayCustomer[1];
            // Create empty string for reference
            string reference = "";

            // Check the customer id is in the correct format
            if (regexCustomerID.IsMatch(arrayCustomer[0]))
            {
                // string to store the customer id as reference
                reference = arrayCustomer[0];
            } else
            {
                // Split Customer name into two string
                String[] arrayCustomerName = customerName.Split(new Char[0], StringSplitOptions.RemoveEmptyEntries);
                // Get first letter from each string and add to reference with hyphen
                String initials = arrayCustomerName[0].Substring(0, 1) + arrayCustomerName[1].Substring(0, 1) + "-";

                // Create array for two sql strings
                String[] SQLString = { "SELECT [Customer ID] FROM Sheet1 WHERE [Customer ID] = @reference",
                        "SELECT [Customer ID] FROM [Student Sample 2 - Sheet1] WHERE [Customer ID] = @reference"};

                // all refernce
                reference = CreateNewID(initials, SQLString);
            }
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
            string[] arrayProduct = rawData.Split('~');


            // String to store the product id as reference
            string reference = "";
            // String to store the category
            string category = arrayProduct[2];
            // String to store the subcategory
            string subcategory = arrayProduct[3];

            // Check the customer id is in the correct format
            if (regexProductID.IsMatch(arrayProduct[0]))
            {
                // string to store the customer id as reference
                reference = arrayProduct[0];

            }
            else
            {
                // Get first letter from each string and add to reference with hyphen
                String initials = category.Substring(0, 3) + "-" + subcategory.Substring(0, 2) + "-";
                // Capitilize string
                string initialsCap = initials.ToUpper();
                Console.WriteLine(initialsCap);

                // Create array for two sql strings
                String[] SQLString = { "SELECT [Customer ID] FROM Sheet1 WHERE [Customer ID] = @reference",
                        "SELECT [Customer ID] FROM [Student Sample 2 - Sheet1] WHERE [Customer ID] = @reference"};

                // all refernce
                reference = CreateNewID(initialsCap, SQLString);
            }
            // String to store the product name
            string productName = arrayProduct[1];

            // Call function to insert data into product dimension
            insertProductDimension(reference, category, subcategory, productName);

        }

        // Function to split the data for the fact table
        private void splitFactTable(String rawData)
        {
            // Create array to split the info into 
            String[] arrayFacts = rawData.Split(':');

            Console.WriteLine(arrayFacts[0] + " : " + arrayFacts[1] + " : " + arrayFacts[2] + " : " + arrayFacts[3] + " : " + arrayFacts[4] + " : " + arrayFacts[5] + " : " + arrayFacts[6] + " : " + arrayFacts[7] + " : " + arrayFacts[8] );

            // Create empty strings for facts
            Decimal sales = 0;
            Int32 quantity = 0;
            Decimal profit = 0;
            Decimal discount = 0;
            Decimal d = 0;

            // Check if first array item can be converted
            bool tryConvert = decimal.TryParse(arrayFacts[0], out d);
            // Check that facts are in the correct format
            // Check Sales
            if (tryConvert == true)
            {
                //Console.WriteLine("This is in the correct format: " + arrayFacts[0]);
                // Set array string to sales decimal
                sales = Convert.ToDecimal(arrayFacts[0]);
                // Set array string to quantity int
                quantity = Convert.ToInt32(arrayFacts[1]);
                // Set array string to discount decimal
                discount = Convert.ToDecimal(arrayFacts[2]);
                // Set array string to profit decimal
                profit = Convert.ToDecimal(arrayFacts[3]);
            } else
            {
                //Console.WriteLine("This is in the wrong format: " + arrayFacts[0]);
                // Check next array
                tryConvert = decimal.TryParse(arrayFacts[1], out d);
                // If true
                if (tryConvert == true)
                {
                    // Set array string to sales decimal
                    sales = Convert.ToDecimal(arrayFacts[1]);
                    // Set array string to quantity int
                    quantity = Convert.ToInt32(arrayFacts[2]);
                    // Set array string to discount decimal
                    discount = Convert.ToDecimal(arrayFacts[3]);
                    // Set array string to profit decimal
                    profit = Convert.ToDecimal(arrayFacts[4]);
                } else
                {
                    bool isNull = String.IsNullOrEmpty(arrayFacts[2]);
                    if (isNull == true)
                    {
                        //Console.WriteLine("String is null");
                        // Set array string to sales decimal
                        sales = Convert.ToDecimal(arrayFacts[3]);
                        // Set array string to quantity int
                        quantity = Convert.ToInt32(arrayFacts[4]);
                        // Set array string to discount decimal
                        discount = Convert.ToDecimal(arrayFacts[5]);
                        // Set array string to profit decimal
                        profit = 0;
                    }
                    else
                    {
                        //Console.WriteLine("This is in the wrong format: " + arrayFacts[2] + "   3");
                        // Set array string to sales decimal
                        sales = Convert.ToDecimal(arrayFacts[2]);
                        // Set array string to quantity int
                        quantity = Convert.ToInt32(arrayFacts[3]);
                        // Set array string to discount decimal
                        discount = Convert.ToDecimal(arrayFacts[4]);
                        // Set array string to profit decimal
                        profit = Convert.ToDecimal(arrayFacts[5]);
                    }
                }
            }
            

            

            // Set ID strings
            // Time ID
            Int32 timeID = Convert.ToInt32(arrayFacts[6]);
            // Product ID
            Int32 productID = Convert.ToInt32(arrayFacts[7]);
            // Time ID
            Int32 customerID = Convert.ToInt32(arrayFacts[8]);

            // Insert it into the database
            // Console.WriteLine(sales + " : " + quantity + " : " + discount + " : " + profit + " : " + timeID + " : " + productID + " : " + customerID);
            insertFactTable(timeID, productID, customerID, sales, quantity, profit, discount);
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
                    // Add a reference to @dates
                    insertCommand.Parameters.Add(new SqlParameter("date", date));
                    // Add a reference to @dayOfYear
                    insertCommand.Parameters.Add(new SqlParameter("dayOfYear", dayOfYear));

                    // Insert the line 
                    int recordAffected = insertCommand.ExecuteNonQuery();
                    //console.WriteLine("Records Affected: " + recordAffected);
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
                    //console.WriteLine("Records Affected: " + recordAffected);
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
                    //console.WriteLine("Records Affected: " + recordAffected);
                }
            }
        }

        // Insert into fact table
        private void insertFactTable(Int32 timeID, Int32 productID, Int32 customerID, Decimal sales, Int32 quantity, Decimal profit, Decimal discount)
        {
            // Create a connection to the MDF file
            string connectionStringDestination = Properties.Settings.Default.DestinationDatabaseConnectionString;

            // Create a boundary for the object to be used - Object will be destroyed at the end of te block
            using (SqlConnection myConnection = new SqlConnection(connectionStringDestination))
            {
                // Open the SQL connection
                myConnection.Open();
                // Check if the product already exists in the database - NO DUPLICATES
                SqlCommand command = new SqlCommand("SELECT timeId FROM FactTableAssignment WHERE timeId = @timeId", myConnection);
                // Add a reference to @reference
                command.Parameters.Add(new SqlParameter("timeId", timeID));

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
                        "INSERT INTO FactTableAssignment (productid, timeid, customerid, value, discount, profit, quantity)" +
                        "VALUES (@productid, @timeid, @customerid, @value, @discount, @profit, @quantity)",
                        myConnection);
                    // Add a reference to @productid
                    insertCommand.Parameters.Add(new SqlParameter("productid", productID));
                    // Add a reference to @timeid
                    insertCommand.Parameters.Add(new SqlParameter("timeid", timeID));
                    // Add a reference to @customerid
                    insertCommand.Parameters.Add(new SqlParameter("customerid", customerID));
                    // Add a reference to @value
                    insertCommand.Parameters.Add(new SqlParameter("value", sales));
                    // Add a reference to @discount
                    insertCommand.Parameters.Add(new SqlParameter("discount", discount));
                    // Add a reference to @profit
                    insertCommand.Parameters.Add(new SqlParameter("profit", profit));
                    // Add a reference to @quantity
                    insertCommand.Parameters.Add(new SqlParameter("quantity", quantity));
                    
                    // Insert the line 
                    int recordAffected = insertCommand.ExecuteNonQuery();
                    //Console.WriteLine("Fact table Records Affected: " + recordAffected);
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

            // Set the datasource to null to stop program from crashing
            listBoxTimeDimension.DataSource = null;
            // Clear listbox to make sure there is no old data there
            listBoxTimeDimension.Items.Clear();

            // Set the datasource to null to stop program from crashing
            listBoxProductDestination.DataSource = null;
            // Clear listbox to make sure there is no old data there
            listBoxProductDestination.Items.Clear();

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
                OleDbCommand getDates = new OleDbCommand("SELECT DISTINCT([Order Date]), [Ship Date] FROM Sheet1", connection);

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

                // Customer Dimension - Get Customer info from data set 

                // Query to get all the relevant Customer Info from the data set
                OleDbCommand getCustomerInfo = new OleDbCommand("SELECT DISTINCT([Customer ID]), [Customer Name], Country, City, State, [Postal Code], Region FROM Sheet1", connection);

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

                // Product Dimension = Get Product info from data set

                // Query to get all the relevatn customer info from the data set
                OleDbCommand getProductInfo = new OleDbCommand("SELECT DISTINCT([Product ID]), [Product Name], Category, [Sub-Category] FROM Sheet1", connection);

                // Executes the query
                reader = getProductInfo.ExecuteReader();

                // Read through all results
                while (reader.Read())
                {
                    // Add the products found in the query by the reader into a string
                    Products.Add(reader[0].ToString() + "~" +
                        reader[1].ToString() + "~" +
                        reader[2].ToString() + "~" +
                        reader[3].ToString());
                }
            }

            // Second dataset
            // Create the database string
            string connectionString2 = Properties.Settings.Default.DataSet2_1_ConnectionString;

            // Create a boundary for the object to be used - Object will be destroyed at the end of te block
            using (OleDbConnection connection = new OleDbConnection(connectionString2))
            {
                // Opens a filestream to the specified path
                connection.Open();
                // Reader provides a way of reading forward only stream of data rows from a data source
                OleDbDataReader reader = null;

                // Dates Dimension - Get Dates from dataset 2

                // getDates allows us to write a query in order to get the data from the rows we want
                OleDbCommand getDates = new OleDbCommand("SELECT DISTINCT([Order Date]), [Ship Date] FROM [Student Sample 2 - Sheet1]", connection);

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

                // Bind list to listbox
                listBoxTimeDimension.DataSource = DatesFormatted;

                // For each loop goes through each date in DatesFormatted list
                foreach (string date in DatesFormatted)
                {
                    // Call function to split the dates with the date as a parameter
                    splitDates(date);
                }

                // Splitting the DatesFormatted into day, month and year
                string[] arrayDate = DatesFormatted[0].ToString().Split('/');


                // Customer Dimension - Get Customer info from data set 2

                // Query to get all the relevant Customer Info from the data set
                OleDbCommand getCustomerInfo = new OleDbCommand("SELECT DISTINCT([Customer ID]), [Customer Name], Country, City, State, [Postal Code], Region FROM [Student Sample 2 - Sheet1]", connection);

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
                    // Call to split customer function with customer as a parameter
                    splitCustomers(customer);
                }

                // Bind list to customer listbox
                listBoxCustomerDimension.DataSource = Customers;

                // Product Dimension = Get Product info from data set

                // Query to get all the relevatn customer info from the data set
                OleDbCommand getProductInfo = new OleDbCommand("SELECT DISTINCT([Product ID]), [Product Name], Category, [Sub-Category] FROM [Student Sample 2 - Sheet1]", connection);

                // Executes the query
                reader = getProductInfo.ExecuteReader();

                // Read through all results
                while (reader.Read())
                {
                    // Add the products found in the query by the reader into a string
                    Products.Add(reader[0].ToString() + "~" +
                        reader[1].ToString() + "~" +
                        reader[2].ToString() + "~" +
                        reader[3].ToString());
                }

                // For each loop to go through all product info
                foreach (string product in Products)
                {
                    // Call the function to split the product info 
                    splitProducts(product);
                }

                // Bind the listbox to list
                listBoxProductDestination.DataSource = Products;
            }
        }

        private void btnGetDestinationData_Click(object sender, EventArgs e)
        {
            // Time Dimension
            // Create a list to store the dates data in
            List<string> destinationDates = new List<string>();

            // Customer Dimension
            // Create a list to store the customer data in
            List<string> destinationCustomer = new List<string>();

            // Product Dimension
            // Create a list to store the product data in
            List<string> DestinationProducts = new List<string>();

            // Create a connection to MDF File
            string connectionStringDestination = Properties.Settings.Default.DestinationDatabaseConnectionString;

            // Create a boundary for the object to be used - Object will be destroyed at the end of te block
            using (SqlConnection myConnection = new SqlConnection(connectionStringDestination))
            {
                // Open sql connection
                myConnection.Open();

                // Time Dimension
                // Query to get the data from the destination time table
                SqlCommand datesCommand = new SqlCommand("SELECT id, dayName, dayNumber, monthName, monthNumber, weekNumber, year," +
                    "weekend, date, dayOfYear FROM Time", myConnection);

                // Create a boundary in which the reader can be used to read the data from the sql query
                using (SqlDataReader reader = datesCommand.ExecuteReader())
                {
                    // If there is data 
                    if (reader.HasRows)
                    {
                        // Loop to read through the data
                        while (reader.Read())
                        {
                            // Display data from the Time table in the datatable
                            string id = reader["id"].ToString();
                            string dayName = reader["dayName"].ToString();
                            string dayNumber = reader["dayNumber"].ToString();
                            string monthName = reader["monthName"].ToString();
                            string monthNumber = reader["monthNumber"].ToString();
                            string weekNumber = reader["weekNumber"].ToString();
                            string year = reader["year"].ToString();
                            string weekend = reader["weekend"].ToString();
                            string date = Convert.ToDateTime(reader["date"]).ToString("M/dd/yyyy");
                            string dayOfYear = reader["dayOfYear"].ToString();

                            // string to store data as detailed text
                            string text = id + " : " + dayName + " : " + dayNumber +
                                " : " + monthName + " : " + monthNumber + " : " + weekNumber +
                                " : " + year + " : " + weekend + " : " + date +
                                " : " + dayOfYear;

                            // Add text string to list
                            destinationDates.Add(text);
                        }
                    }
                    // Else if there is no data
                    else
                    {
                        destinationDates.Add("No Data available");
                    }
                }

                // Insert list into datatable 
                // Create array for column headers
                string[] timeColumnHeaders = { "ID", "Day Name", "Day Number", "Month Name", "Month Number", 
                                                "Week Number", "Year", "Weekend", "Date", "Day of the Year" };

                // Call convert list to datatable function
                DataTable timeTable = ConvertListToDataTable(destinationDates, timeColumnHeaders);
                // Bind datatable to datagrid
                dataGridViewTime.DataSource = timeTable;

                // Create array for text split
                string[] timeArray = { };

                // Add rows to the datatable
                foreach (string row in destinationDates)
                {
                    timeArray = row.Split(':');
                    timeTable.Rows.Add(new Object[] { timeArray[0], timeArray[1], timeArray[2], timeArray[3], timeArray[4],
                    timeArray[5], timeArray[6], timeArray[7], timeArray[8], timeArray[9]});
                }

                // Customer Dimension
                // Query to get the data from the destination customer table
                SqlCommand customerCommand = new SqlCommand("SELECT id, name, country, city, state, postalCode, region, reference FROM Customer", myConnection);

                // Create a boundary in which the reader can be used to read the data from the sql query
                using (SqlDataReader reader = customerCommand.ExecuteReader())
                {
                    // If there is data 
                    if (reader.HasRows)
                    {
                        // Loop to read through the data
                        while (reader.Read())
                        {
                            // Display data from the Customer table in the datatable
                            string id = reader["id"].ToString();
                            string name = reader["name"].ToString();
                            string country = reader["country"].ToString();
                            string city = reader["city"].ToString();
                            string state = reader["state"].ToString();
                            string postalCode = reader["postalCode"].ToString();
                            string region = reader["region"].ToString();
                            string reference = reader["reference"].ToString();

                            // string to store data as detailed text
                            string text =  id + " : " + name + " : " + country +
                                " : " + city + " : " + state +
                                " : " + postalCode + " : " + region + " : " + reference;

                            // Add text string to list
                            destinationCustomer.Add(text);
                        }
                    }
                    // Else if there is no data
                    else
                    {
                        destinationCustomer.Add("No Data available");
                    }
                }

                // Insert List into DataTable
                string[] customerColumnNames = { "ID", "Name", "Country", "City", "State", "Post Code", "Region", "Referemce" };
                DataTable customerTable = ConvertListToDataTable(destinationCustomer, customerColumnNames);
                dataGridCustomer.DataSource = customerTable;

                // For each loop to go through all customer info
                string[] customerArray = { };
                foreach (string customer in destinationCustomer)
                {
                    customerArray = customer.Split(':');
                    customerTable.Rows.Add(new Object[] { customerArray[0], customerArray[1], customerArray[2], customerArray[3], customerArray[4], customerArray[5], customerArray[6], customerArray[7] });
                }

                // Product Dimension
                // Query to get the data from the destination customer table
                SqlCommand ProductCommand = new SqlCommand("SELECT id, category, subcategory, name, reference FROM Product", myConnection);

                // Create a boundary in which the reader can be used to read the data from the sql query
                using (SqlDataReader reader = ProductCommand.ExecuteReader())
                {
                    // If there is data 
                    if(reader.HasRows)
                    {
                        // Loop to read through the data
                        while(reader.Read())
                        {
                            // Display data from the Product tabke in the datatable
                            string id = reader["id"].ToString();
                            string category = reader["category"].ToString();
                            string subcategory = reader["subcategory"].ToString();
                            string name = reader["name"].ToString();
                            string reference = reader["reference"].ToString();

                            // string to store data as detailed text
                            string text = id + " : " + category + " : " + subcategory +
                                " : " + name + " : " + reference ;

                            // Add text string to list
                            DestinationProducts.Add(text);
                        }
                    }
                    // Else if there is no data
                    else
                    {
                        DestinationProducts.Add("No Data available");
                    }
                }
            }

            // Insert List into DataTable
            string[] productColumnNames = { "ID", "Category", "Subcategory", "Product Name", "Reference" };
            DataTable productTable = ConvertListToDataTable(DestinationProducts, productColumnNames);
            dataGridProduct.DataSource = productTable;
            
            // For each loop to go through all product info
            string[] productArray = { };
            foreach (string product in DestinationProducts)
            {
                productArray = product.Split(':');
                productTable.Rows.Add(new Object[] { productArray[0], productArray[1], productArray[2], productArray[3], productArray[4] });
            }
        }

        private void btnFactTable_Click(object sender, EventArgs e)
        {
            // Create database connection string
            string connectionString = Properties.Settings.Default.Data_set_1ConnectionString;

            using (OleDbConnection connection = new OleDbConnection(connectionString))
            {
                // Open the connection
                connection.Open();
                // Set reader to null
                OleDbDataReader reader = null;
                // Query to get all columns from sheet1 
                OleDbCommand getData = new OleDbCommand("SELECT ID, [Row ID], [Order ID], [Ship Date], [Order Date], " +
                    "[Ship Mode], [Customer ID], [Customer Name], Segment, Country, City, State, [Postal Code], " +
                    "[Product ID], Region, Category, [Sub-Category], [Product Name], Sales, Quantity, Profit, " +
                    "Discount FROM Sheet1", connection);

                // Use reader to execute query
                reader = getData.ExecuteReader();
                // While the reader goes through the data
                while (reader.Read())
                {
                    // Get a line of data from the source

                    // Get the numeric values
                    Decimal sales = Convert.ToDecimal(reader["Sales"]);
                    Int32 quantity = Convert.ToInt32(reader["quantity"]);
                    Decimal profit = Convert.ToDecimal(reader["profit"]);
                    Decimal discount = Convert.ToDecimal(reader["discount"]);

                    // Get the dimension IDs
                    Int32 timeID = getIDs(reader["Order Date"].ToString(), reader["Customer ID"].ToString(), reader["Product ID"].ToString(), "Time", false);
                    Int32 productID = getIDs(reader["Order Date"].ToString(), reader["Customer ID"].ToString(), reader["Product ID"].ToString(), "Product", false);
                    Int32 customerID = getIDs(reader["Order Date"].ToString(), reader["Customer ID"].ToString(), reader["Product ID"].ToString(), "Customer", false);

                    // Insert it into the database
                    insertFactTable(timeID, productID, customerID, sales, quantity, profit, discount);
                }
            }

            // Create database connection string for second dataset
            string connectionString2 = Properties.Settings.Default.DataSet2_1_ConnectionString;

            using (OleDbConnection connection = new OleDbConnection(connectionString2))
            {
                // Open the connection
                connection.Open();
                // Set reader to null
                OleDbDataReader reader = null;
                // Query to get all columns from sheet1 
                OleDbCommand getData = new OleDbCommand("SELECT [Row ID], [Order ID], [Ship Date], [Order Date], " +
                    "[Ship Mode], [Customer ID], [Customer Name], Segment, Country, City, State, [Postal Code], " +
                    "[Product ID], Region, Category, [Sub-Category], [Product Name], Sales, Quantity, Profit, " +
                    "Discount, Field22, Field23 FROM [Student Sample 2 - Sheet1];", connection);

                // Use reader to execute query
                reader = getData.ExecuteReader();
                // While the reader goes through the data
                while (reader.Read())
                {

                    
                    
                    // Get a line of data from the source
                    // Get the numeric values
                    String sales = reader["Sales"].ToString();
                    String quantity = reader["Quantity"].ToString();
                    String profit = reader["Profit"].ToString();
                    String discount = reader["Discount"].ToString();
                    String field22 = reader["Field22"].ToString();
                    String field23 = reader["Field23"].ToString();



                    // Get the dimension IDs
                    Int32 timeID = 0;
                    Int32 productID = 0;
                    Int32 customerID = 0;
                    if (regexCustomerID.IsMatch(reader["Customer ID"].ToString()))
                    {
                        customerID = getIDs(reader["Order Date"].ToString(), reader["Customer ID"].ToString(), reader["Product ID"].ToString(), "Customer", false);
                        timeID = getIDs(reader["Order Date"].ToString(), reader["Customer ID"].ToString(), reader["Product ID"].ToString(), "Time", false);
                        productID = getIDs(reader["Order Date"].ToString(), reader["Customer ID"].ToString(), reader["Product ID"].ToString(), "Product", false);
                    } else
                    {
                        Console.WriteLine(reader["Customer ID"] + " Is in the wrong format");
                        customerID = getIDs(reader["Order Date"].ToString(), reader["Customer Name"].ToString(), reader["Product ID"].ToString(), "Customer", true);
                        timeID = getIDs(reader["Order Date"].ToString(), reader["Customer Name"].ToString(), reader["Product ID"].ToString(), "Time", true);
                        productID = getIDs(reader["Order Date"].ToString(), reader["Customer Name"].ToString(), reader["Product ID"].ToString(), "Product", true);
                    }


                    string text = sales + ":" + quantity + ":" + discount + ":" + profit + ":" + field22 + ":" + field23 + ":" + timeID + ":" + productID + ":" + customerID;


                    splitFactTable(text);
                }
            }

            // Build table
            // Create List to store the data in
            List<String> FactTableList = new List<string>();

            // Create connection to the MDF file
            string connectionStringDestination = Properties.Settings.Default.DestinationDatabaseConnectionString;

            using (SqlConnection myConnection = new SqlConnection(connectionStringDestination))
            {
                // Open the connnection
                myConnection.Open();
                // Check if the date already exists in the database
                SqlCommand command = new SqlCommand("SELECT productId, timeId, customerId, value, discount, profit, quantity FROM FactTableAssignment", myConnection);



                using (SqlDataReader reader = command.ExecuteReader())
                {
                    // Check if there is data in the table
                    if (reader.HasRows)
                    {
                        // Retrieve the data
                        while (reader.Read())
                        {
                            string productid = reader["productId"].ToString();
                            string timeid = reader["timeId"].ToString();
                            string customerid = reader["customerId"].ToString();
                            string value = reader["value"].ToString();
                            string discount = reader["discount"].ToString();
                            string profit = reader["profit"].ToString();
                            string quantity = reader["quantity"].ToString();

                            string text;

                            text =  productid + " : " + customerid + " : " + timeid + " : " + value + " : " + discount + " : " + profit + " : " + quantity; ;

                            FactTableList.Add(text);
                        }
                    }
                    else // If there is no data available
                    {
                        FactTableList.Add("No Data available");
                    }


                    // Insert List into DataTable
                    string[] factColumnNames = { "Product ID", "Customer ID", "Time ID", "Value", "Discount", "Profit", "Quantity" };
                    DataTable factTable = ConvertListToDataTable(FactTableList, factColumnNames);
                    dataGridFactTable.DataSource = factTable;

                    // For each loop to go through all product info
                    string[] factArray = { };
                    foreach (string fact in FactTableList)
                    {
                        factArray = fact.Split(':');
                        if (!factArray.Contains("No Data available"))
                        {
                            factTable.Rows.Add(new Object[] { factArray[0], factArray[1], factArray[2], factArray[3], factArray[4], factArray[5], factArray[6] });
                        }
                        else
                        {
                            factTable.Rows.Add(new Object[] { factArray[0] });
                        }
                    }
                }
            }
        }
        static DataTable ConvertListToDataTable(List<string> list, string[] columnNames)
        {

            string[] array = { };
            // For each loop to go through all product info
            foreach (string product in list)
            {
                array = product.Split(':');
            }

            // Create new table
            DataTable table = new DataTable();

            // Get MAX columns
            int columns = array.Length;

            // Add columns
            // Loop through the amount of columns
            for (int i = 0; i < columns; i++)
            {
                table.Columns.Add(columnNames[i]);
            }

            // Return variable
            return table;
        }

        // Time Dimension
        private void btnLoadTimeData_Click(object sender, EventArgs e)
        {
            // Create dictionaries
            // Create a empty dictionary to store the destination data for sales
            Dictionary<String, dynamic> salesCount = new Dictionary<String, dynamic>();

            // Create an empty dictionary to store the destination data for profit
            Dictionary<String, dynamic> profit = new Dictionary<String, dynamic>();

            // Create an empty dictionary to store destination data for quantity
            Dictionary<String, dynamic> quantity = new Dictionary<String, dynamic>();

            // Create an empty dictionary to store the destination data for value
            Dictionary<String, dynamic> value = new Dictionary<String, dynamic>();

            // Create an empty dictionary to store the destination data for discount
            Dictionary<String, dynamic> discount = new Dictionary<String, dynamic>();

            // Create list for dates
            List<string> datesList = new List<string>();

            // Create a new list for the week numbers
            List<string> weekList = new List<String>();

            // For loop to add all the weeks in a year
            for (int i = 0; i < 53; i++)
            {
                weekList.Add(i.ToString());
            }

            // Create list for months in a yea
            List<String> monthList = new List<string> { "January", "Febuary", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };

            // Create list for years
            List<String> yearList = new List<string>();


            // Create a connection to the MDF file 
            String connectionStringDestination = Properties.Settings.Default.DestinationDatabaseConnectionString;

            // Populate year list
            using (SqlConnection connection = new SqlConnection(connectionStringDestination))
            {
                // Open the connection
                connection.Open();

                // Sql query to get the latest year from time table
                SqlCommand command = new SqlCommand("SELECT MAX(year) FROM Time", connection);

                // Create reader
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    // Check the query returns results
                    if (reader.HasRows)
                    {
                        // While the reader gets the data
                        while (reader.Read())
                        {
                            // Get last 10 years from latest year
                            Int32 maxYear = Convert.ToInt32(reader[0]);
                            for (int i = maxYear - 10; i <= maxYear; i++)
                            {
                                // Add year to list
                                yearList.Add(Convert.ToString(i));
                            }
                        }
                    }
                }
                // Get dates from time dimension
                // Sql query to get the latest year from time table
                SqlCommand command2 = new SqlCommand("SELECT date FROM Time", connection);

                // Create reader
                using (SqlDataReader reader2 = command2.ExecuteReader())
                {
                    // Check the query returns results
                    if (reader2.HasRows)
                    {
                        // While the reader gets the data
                        while (reader2.Read())
                        {
                            datesList.Add(reader2[0].ToString());
                        }
                    }
                }
            }

            // Create list for formatted dates
            List<string> formattedDates = new List<string>();

            foreach (string date in datesList)
            {
                var dates = date.Split(new Char[0], StringSplitOptions.RemoveEmptyEntries);
                formattedDates.Add(dates[0]);
            }

            // Create list for formatted dates
            List<string> dbDates = new List<string>();

            foreach (string  date in formattedDates)
            {
                // Store tuple method as result
                Tuple<Int32, Int32, Int32, DateTime, String> result = ReturnDayMonthYearData(date);
                // Store db compatible date as string
                string dbDate = result.Item5;

                dbDates.Add(dbDate);
            }

            // Sales

            // If user selects weeks for sales
            if (comboBoxTimeSales.Text == "Weeks")
            {
                // Sql string to pass to display grapth method
                string SQLstring = "SELECT COUNT(*) as SalesCount FROM FactTableAssignment " +
                                                    "JOIN Time ON FactTableAssignment.timeId = Time.id WHERE Time.WeekNumber = @selection";

                // Call method
                DisplayGraphs(connectionStringDestination, SQLstring, weekList, salesCount, chartTimeSales, "sales");
            }

            // If the user has selected months for sales
            else if (comboBoxTimeSales.Text == "Months")
            {
                // Sql string to pass to display grapth method
                string SQLstring = "SELECT COUNT(*) as SalesCount FROM FactTableAssignment " +
                                                    "JOIN Time ON FactTableAssignment.timeId = Time.id WHERE Time.monthName = @selection";

                // Call method
                DisplayGraphs(connectionStringDestination, SQLstring, monthList, salesCount, chartTimeSales, "sales");
            }

            // If the user has selected years for sales
            else if (comboBoxTimeSales.Text == "Years")
            {
               // Sql string to pass to display grapth method
               string SQLstring = "SELECT COUNT(*) as SalesCount FROM FactTableAssignment " +
                                                        "JOIN Time ON FactTableAssignment.timeId = Time.id WHERE Time.year = @selection";

                // Call method
                DisplayGraphs(connectionStringDestination, SQLstring, yearList, salesCount, chartTimeSales, "sales");
            }

            // Profit

            // If the user has selected Days for profit
            if (comboBoxTimeProfit.Text == "Days")
            {
                // Sql string to pass to display grapth method
                string SQLstring = "SELECT profit FROM FactTableAssignment " +
                                                         "JOIN Time ON FactTableAssignment.timeId = Time.id WHERE Time.Date = @selection";
                
                // Call method
                DisplayGraphs(connectionStringDestination, SQLstring, dbDates, profit, chartTimeProfit, "profit");
            }

            // If the user has selected weeks for profit
            else if (comboBoxTimeProfit.Text == "Weeks")
            {
                // Sql string to pass to display grapth method
                string SQLstring = "SELECT COALESCE(SUM(profit), 0) FROM FactTableAssignment " +
                                                         "JOIN Time ON FactTableAssignment.timeId = Time.id WHERE Time.weekNumber = @selection";

                // Call method
                DisplayGraphs(connectionStringDestination, SQLstring, weekList, profit, chartTimeProfit, "profit");
            }

            // If the user has selected months for profit
            else if (comboBoxTimeProfit.Text == "Months")
            {
                // Sql string to pass to display grapth method
                string SQLstring = "SELECT COALESCE(SUM(profit), 0) FROM FactTableAssignment " +
                                                         "JOIN Time ON FactTableAssignment.timeId = Time.id WHERE Time.monthName = @selection";

                // Call method
                DisplayGraphs(connectionStringDestination, SQLstring, monthList , profit, chartTimeProfit, "profit");
            }

            // If the user has selected years for profit
            else if (comboBoxTimeProfit.Text == "Years")
            {
                // Sql string to pass to display grapth method
                string SQLstring = "SELECT COALESCE(SUM(profit), 0) FROM FactTableAssignment " +
                                                         "JOIN Time ON FactTableAssignment.timeId = Time.id WHERE Time.year = @selection";

                // Call method
                DisplayGraphs(connectionStringDestination, SQLstring, yearList, profit, chartTimeProfit, "profit");
            }

            // Quantity

            // If the user has selected Days for Quantity
            if (comboBoxTimeQuantity.Text == "Days")
            {
                // Sql string to pass to display grapth method
                string SQLstring = "SELECT quantity FROM FactTableAssignment " +
                                                         "JOIN Time ON FactTableAssignment.timeId = Time.id WHERE Time.Date = @selection";

                // Call method
                DisplayGraphs(connectionStringDestination, SQLstring, dbDates, quantity, chartTimeQuantity, "quantity");
            }

            // If the user has selected weeks for Quantity
            else if (comboBoxTimeQuantity.Text == "Weeks")
            {
                // Sql string to pass to display grapth method
                string SQLstring = "SELECT COALESCE(SUM(quantity), 0) FROM FactTableAssignment " +
                                                         "JOIN Time ON FactTableAssignment.timeId = Time.id WHERE Time.weekNumber = @selection";

                // Call method
                DisplayGraphs(connectionStringDestination, SQLstring, weekList, quantity, chartTimeQuantity, "quantity");
            }

            // If the user has selected months for Quantity
            else if (comboBoxTimeQuantity.Text == "Months")
            {
                // Sql string to pass to display grapth method
                string SQLstring = "SELECT COALESCE(SUM(quantity), 0) FROM FactTableAssignment " +
                                                         "JOIN Time ON FactTableAssignment.timeId = Time.id WHERE Time.monthName = @selection";

                // Call method
                DisplayGraphs(connectionStringDestination, SQLstring, monthList, quantity, chartTimeQuantity, "quantity");
            }

            // If the user has selected years for Quantity
            else if (comboBoxTimeQuantity.Text == "Years")
            {
                // Sql string to pass to display grapth method
                string SQLstring = "SELECT COALESCE(SUM(quantity), 0) FROM FactTableAssignment " +
                                                         "JOIN Time ON FactTableAssignment.timeId = Time.id WHERE Time.year = @selection";

                // Call method
                DisplayGraphs(connectionStringDestination, SQLstring, yearList, quantity, chartTimeQuantity, "quantity");
            }

            // Value

            // If the user has selected Days for Value
            if (comboBoxTimeValue.Text == "Days")
            {
                // Sql string to pass to display grapth method
                string SQLstring = "SELECT value FROM FactTableAssignment " +
                                                         "JOIN Time ON FactTableAssignment.timeId = Time.id WHERE Time.Date = @selection";

                // Call method
                DisplayGraphs(connectionStringDestination, SQLstring, dbDates, value, chartTimeValue, "value");
            }

            // If the user has selected weeks for Value
            else if (comboBoxTimeValue.Text == "Weeks")
            {
                // Sql string to pass to display grapth method
                string SQLstring = "SELECT COALESCE(SUM(value), 0) FROM FactTableAssignment " +
                                                         "JOIN Time ON FactTableAssignment.timeId = Time.id WHERE Time.weekNumber = @selection";

                // Call method
                DisplayGraphs(connectionStringDestination, SQLstring, weekList, value, chartTimeValue, "value");
            }

            // If the user has selected months for Value
            else if (comboBoxTimeValue.Text == "Months")
            {
                // Sql string to pass to display grapth method
                string SQLstring = "SELECT COALESCE(SUM(value), 0) FROM FactTableAssignment " +
                                                         "JOIN Time ON FactTableAssignment.timeId = Time.id WHERE Time.monthName = @selection";

                // Call method
                DisplayGraphs(connectionStringDestination, SQLstring, monthList, value, chartTimeValue, "value");
            }

            // If the user has selected years for Value
            else if (comboBoxTimeValue.Text == "Years")
            {
                // Sql string to pass to display grapth method
                string SQLstring = "SELECT COALESCE(SUM(value), 0) FROM FactTableAssignment " +
                                                         "JOIN Time ON FactTableAssignment.timeId = Time.id WHERE Time.year = @selection";

                // Call method
                DisplayGraphs(connectionStringDestination, SQLstring, yearList, value, chartTimeValue, "value");
            }

            // Discount

            // If the user has selected Days for Discount
            if (comboBoxTimeDiscount.Text == "Days")
            {
                // Sql string to pass to display grapth method
                string SQLstring = "SELECT discount FROM FactTableAssignment " +
                                                         "JOIN Time ON FactTableAssignment.timeId = Time.id WHERE Time.Date = @selection";

                // Call method
                DisplayGraphs(connectionStringDestination, SQLstring, dbDates, discount, chartTimeDiscount, "discount");
            }

            // If the user has selected weeks for Discount
            else if (comboBoxTimeDiscount.Text == "Weeks")
            {
                // Sql string to pass to display grapth method
                string SQLstring = "SELECT COALESCE(SUM(discount), 0) FROM FactTableAssignment " +
                                                         "JOIN Time ON FactTableAssignment.timeId = Time.id WHERE Time.weekNumber = @selection";

                // Call method
                DisplayGraphs(connectionStringDestination, SQLstring, weekList, discount, chartTimeDiscount, "discount");
            }

            // If the user has selected months for Discount
            else if (comboBoxTimeDiscount.Text == "Months")
            {
                // Sql string to pass to display grapth method
                string SQLstring = "SELECT COALESCE(SUM(discount), 0) FROM FactTableAssignment " +
                                                         "JOIN Time ON FactTableAssignment.timeId = Time.id WHERE Time.monthName = @selection";

                // Call method
                DisplayGraphs(connectionStringDestination, SQLstring, monthList, discount, chartTimeDiscount, "discount");
            }

            // If the user has selected years for Discount
            else if (comboBoxTimeDiscount.Text == "Years")
            {
                // Sql string to pass to display grapth method
                string SQLstring = "SELECT COALESCE(SUM(discount), 0) FROM FactTableAssignment " +
                                                         "JOIN Time ON FactTableAssignment.timeId = Time.id WHERE Time.year = @selection";

                // Call method
                DisplayGraphs(connectionStringDestination, SQLstring, yearList, discount, chartTimeDiscount, "discount");
            }
        }

        private void btnLoadCustomerData_Click(object sender, EventArgs e)
        {
            // Create dictionaries
            // Create a empty dictionary to store the destination data for sales
            Dictionary<String, dynamic> salesCount = new Dictionary<String, dynamic>();

            // Create an empty dictionary to store the destination data for profit
            Dictionary<String, dynamic> profit = new Dictionary<String, dynamic>();

            // Create an empty dictionary to store destination data for quantity
            Dictionary<String, dynamic> quantity = new Dictionary<String, dynamic>();

            // Create an empty dictionary to store the destination data for value
            Dictionary<String, dynamic> value = new Dictionary<String, dynamic>();

            // Create an empty dictionary to store the destination data for discount
            Dictionary<String, dynamic> discount = new Dictionary<String, dynamic>();

            // Create Lists 
            // Create empty list for Cities
            List<String> cities = new List<String>();

            // Create empty list for States
            List<String> states = new List<String>();

            // Create empty list for Regions
            List<String> regions = new List<String>();

            // Create empty list for Countries 
            List<String> countries = new List<String>();

            // Create empty list for postcodes
            List<String> postcodes = new List<String>();

            // Create a connection to the MDF file 
            String connectionStringDestination = Properties.Settings.Default.DestinationDatabaseConnectionString;

            // Populate year list
            using (SqlConnection connection = new SqlConnection(connectionStringDestination))
            {
                // Open the connection
                connection.Open();

                // Query for reader to execute to get cities
                SqlCommand command = new SqlCommand("SELECT DISTINCT city FROM Customer", connection);

                // Create data reader
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    // Check the query returns data
                    if (reader.HasRows)
                    {
                        // While the reader reads the data
                        while (reader.Read())
                        {
                            // Add each city to list
                            cities.Add(reader[0].ToString());
                        }
                    }
                }

                // Query for reader to execute to get states
                SqlCommand command2 = new SqlCommand("SELECT DISTINCT state FROM Customer", connection);

                // Create data reader
                using (SqlDataReader reader = command2.ExecuteReader())
                {
                    // Check the query returns data
                    if (reader.HasRows)
                    {
                        // While the reader reads the data
                        while (reader.Read())
                        {
                            // Add each state to list
                            states.Add(reader[0].ToString());
                        }
                    }
                }

                // Query for reader to execute to get region
                SqlCommand command3 = new SqlCommand("SELECT DISTINCT region FROM Customer", connection);

                // Create data reader
                using (SqlDataReader reader = command3.ExecuteReader())
                {
                    // Check the query returns data
                    if (reader.HasRows)
                    {
                        // While the reader reads the data
                        while (reader.Read())
                        {
                            // Add each region to list
                            regions.Add(reader[0].ToString());
                        }
                    }
                }

                // Query for reader to execute to get countries
                SqlCommand command4 = new SqlCommand("SELECT DISTINCT country FROM Customer", connection);

                // Create data reader
                using (SqlDataReader reader = command4.ExecuteReader())
                {
                    // Check the query returns data
                    if (reader.HasRows)
                    {
                        // While the reader reads the data
                        while (reader.Read())
                        {
                            // Add each country to list
                            countries.Add(reader[0].ToString());
                        }
                    }
                }

                // Query for reader to execute to get postcodes
                SqlCommand command5 = new SqlCommand("SELECT DISTINCT postalCode FROM Customer", connection);

                // Create data reader
                using (SqlDataReader reader = command5.ExecuteReader())
                {
                    // Check the query returns data
                    if (reader.HasRows)
                    {
                        // While the reader reads the data
                        while (reader.Read())
                        {
                            // Add each postcode to list
                            postcodes.Add(reader[0].ToString());
                        }
                    }
                }
            }

            // Sales

            // If user selects cities for sales
            if (comboBoxCustomerSales.Text == "Cities")
            {
                // Sql string to pass to display grapth method
                string SQLstring = "SELECT COUNT(*) as SalesCount FROM FactTableAssignment " +
                                                    "JOIN Customer ON FactTableAssignment.customerId = Customer.id WHERE Customer.city = @selection";

                // Call method
                DisplayGraphs(connectionStringDestination, SQLstring, cities, salesCount, chartCustomerSales, "sales");
            }

            // If user selects cities for states
            else if (comboBoxCustomerSales.Text == "States")
            {
                // Sql string to pass to display grapth method
                string SQLstring = "SELECT COUNT(*) as SalesCount FROM FactTableAssignment " +
                                                    "JOIN Customer ON FactTableAssignment.customerId = Customer.id WHERE Customer.state = @selection";

                // Call method
                DisplayGraphs(connectionStringDestination, SQLstring, states, salesCount, chartCustomerSales, "sales");
            }

            // If user selects regions for sales
            else if (comboBoxCustomerSales.Text == "Regions")
            {
                // Sql string to pass to display grapth method
                string SQLstring = "SELECT COUNT(*) as SalesCount FROM FactTableAssignment " +
                                                    "JOIN Customer ON FactTableAssignment.customerId = Customer.id WHERE Customer.region = @selection";

                // Call method
                DisplayGraphs(connectionStringDestination, SQLstring, regions, salesCount, chartCustomerSales, "sales");
            }

            // If user selects countries for states
            else if (comboBoxCustomerSales.Text == "Countries")
            {
                // Sql string to pass to display grapth method
                string SQLstring = "SELECT COUNT(*) as SalesCount FROM FactTableAssignment " +
                                                    "JOIN Customer ON FactTableAssignment.customerId = Customer.id WHERE Customer.country = @selection";

                // Call method
                DisplayGraphs(connectionStringDestination, SQLstring, countries, salesCount, chartCustomerSales, "sales");
            }

            // If user selects postcodes for sales
            else if (comboBoxCustomerSales.Text == "Postcodes")
            {
                // Sql string to pass to display grapth method
                string SQLstring = "SELECT COUNT(*) as SalesCount FROM FactTableAssignment " +
                                                    "JOIN Customer ON FactTableAssignment.customerId = Customer.id WHERE Customer.postalCode = @selection";

                // Call method
                DisplayGraphs(connectionStringDestination, SQLstring, postcodes, salesCount, chartCustomerSales, "sales");
            }

            // Profit
            // If user selects cities for profit
            if (comboBoxCustomerProfit.Text == "Cities")
            {
                // Sql string to pass to display grapth method
                string SQLstring = "SELECT COALESCE(SUM(profit), 0) FROM FactTableAssignment " +
                                                    "JOIN Customer ON FactTableAssignment.customerId = Customer.id WHERE Customer.city = @selection";

                // Call method
                DisplayGraphs(connectionStringDestination, SQLstring, cities, profit, chartCustomerProfit, "profit");
            }

            // If user selects states for profit
            else if (comboBoxCustomerProfit.Text == "States")
            {
                // Sql string to pass to display grapth method
                string SQLstring = "SELECT COALESCE(SUM(profit), 0) FROM FactTableAssignment " +
                                                    "JOIN Customer ON FactTableAssignment.customerId = Customer.id WHERE Customer.state = @selection";

                // Call method
                DisplayGraphs(connectionStringDestination, SQLstring, states, profit, chartCustomerProfit, "profit");
            }

            // If user selects regions for profit
            else if (comboBoxCustomerProfit.Text == "Regions")
            {
                // Sql string to pass to display grapth method
                string SQLstring = "SELECT COALESCE(SUM(profit), 0) FROM FactTableAssignment " +
                                                    "JOIN Customer ON FactTableAssignment.customerId = Customer.id WHERE Customer.region = @selection";

                // Call method
                DisplayGraphs(connectionStringDestination, SQLstring, regions, profit, chartCustomerProfit, "profit");
            }

            // If user selects countries for profit
            else if (comboBoxCustomerProfit.Text == "Countries")
            {
                // Sql string to pass to display grapth method
                string SQLstring = "SELECT COALESCE(SUM(profit), 0) FROM FactTableAssignment " +
                                                    "JOIN Customer ON FactTableAssignment.customerId = Customer.id WHERE Customer.country = @selection";

                // Call method
                DisplayGraphs(connectionStringDestination, SQLstring, countries, profit, chartCustomerProfit, "profit");
            }

            // If user selects postcodes for profit
            else if (comboBoxCustomerProfit.Text == "Postcodes")
            {
                // Sql string to pass to display grapth method
                string SQLstring = "SELECT COALESCE(SUM(profit), 0) FROM FactTableAssignment " +
                                                    "JOIN Customer ON FactTableAssignment.customerId = Customer.id WHERE Customer.postalCode = @selection";

                // Call method
                DisplayGraphs(connectionStringDestination, SQLstring, postcodes, profit, chartCustomerProfit, "profit");
            }

            // Quantity
            // If user selects cities for Quantity
            if (comboBoxCustomerQuantity.Text == "Cities")
            {
                // Sql string to pass to display grapth method
                string SQLstring = "SELECT COALESCE(SUM(quantity), 0) FROM FactTableAssignment " +
                                                    "JOIN Customer ON FactTableAssignment.customerId = Customer.id WHERE Customer.city = @selection";

                // Call method
                DisplayGraphs(connectionStringDestination, SQLstring, cities, quantity, chartCustomerQuantity, "quantity");
            }

            // If user selects states for Quantity
            else if (comboBoxCustomerQuantity.Text == "States")
            {
                // Sql string to pass to display grapth method
                string SQLstring = "SELECT COALESCE(SUM(quantity), 0) FROM FactTableAssignment " +
                                                    "JOIN Customer ON FactTableAssignment.customerId = Customer.id WHERE Customer.state = @selection";

                // Call method
                DisplayGraphs(connectionStringDestination, SQLstring, states, quantity, chartCustomerQuantity, "quantity");
            }

            // If user selects regions for Quantity
            else if (comboBoxCustomerQuantity.Text == "Regions")
            {
                // Sql string to pass to display grapth method
                string SQLstring = "SELECT COALESCE(SUM(quantity), 0) FROM FactTableAssignment " +
                                                    "JOIN Customer ON FactTableAssignment.customerId = Customer.id WHERE Customer.region = @selection";

                // Call method
                DisplayGraphs(connectionStringDestination, SQLstring, regions, quantity, chartCustomerQuantity, "quantity");
            }

            // If user selects countries for Quantity
            else if (comboBoxCustomerQuantity.Text == "Countries")
            {
                // Sql string to pass to display grapth method
                string SQLstring = "SELECT COALESCE(SUM(quantity), 0) FROM FactTableAssignment " +
                                                    "JOIN Customer ON FactTableAssignment.customerId = Customer.id WHERE Customer.country = @selection";

                // Call method
                DisplayGraphs(connectionStringDestination, SQLstring, countries, quantity, chartCustomerQuantity, "quantity");
            }

            // If user selects postcodes for Quantity
            else if (comboBoxCustomerQuantity.Text == "Postcodes")
            {
                // Sql string to pass to display grapth method
                string SQLstring = "SELECT COALESCE(SUM(quantity), 0) FROM FactTableAssignment " +
                                                    "JOIN Customer ON FactTableAssignment.customerId = Customer.id WHERE Customer.postalCode = @selection";

                // Call method
                DisplayGraphs(connectionStringDestination, SQLstring, postcodes, quantity, chartCustomerQuantity, "quantity");
            }

            // Value
            // If user selects cities for Value
            if (comboBoxCustomerValue.Text == "Cities")
            {
                // Sql string to pass to display grapth method
                string SQLstring = "SELECT COALESCE(SUM(value), 0) FROM FactTableAssignment " +
                                                    "JOIN Customer ON FactTableAssignment.customerId = Customer.id WHERE Customer.city = @selection";

                // Call method
                DisplayGraphs(connectionStringDestination, SQLstring, cities, value, chartCustomerValue, "value");
            }

            // If user selects states for Value
            else if (comboBoxCustomerValue.Text == "States")
            {
                // Sql string to pass to display grapth method
                string SQLstring = "SELECT COALESCE(SUM(value), 0) FROM FactTableAssignment " +
                                                    "JOIN Customer ON FactTableAssignment.customerId = Customer.id WHERE Customer.state = @selection";

                // Call method
                DisplayGraphs(connectionStringDestination, SQLstring, states, value, chartCustomerValue, "value");
            }

            // If user selects regions for Value
            else if (comboBoxCustomerValue.Text == "Regions")
            {
                // Sql string to pass to display grapth method
                string SQLstring = "SELECT COALESCE(SUM(value), 0) FROM FactTableAssignment " +
                                                    "JOIN Customer ON FactTableAssignment.customerId = Customer.id WHERE Customer.region = @selection";

                // Call method
                DisplayGraphs(connectionStringDestination, SQLstring, regions, value, chartCustomerValue, "value");
            }

            // If user selects countries for Value
            else if (comboBoxCustomerValue.Text == "Countries")
            {
                // Sql string to pass to display grapth method
                string SQLstring = "SELECT COALESCE(SUM(value), 0) FROM FactTableAssignment " +
                                                    "JOIN Customer ON FactTableAssignment.customerId = Customer.id WHERE Customer.country = @selection";

                // Call method
                DisplayGraphs(connectionStringDestination, SQLstring, countries, value, chartCustomerValue, "value");
            }

            // If user selects postcodes for Value
            else if (comboBoxCustomerValue.Text == "Postcodes")
            {
                // Sql string to pass to display grapth method
                string SQLstring = "SELECT COALESCE(SUM(value), 0) FROM FactTableAssignment " +
                                                    "JOIN Customer ON FactTableAssignment.customerId = Customer.id WHERE Customer.postalCode = @selection";

                // Call method
                DisplayGraphs(connectionStringDestination, SQLstring, postcodes, value, chartCustomerValue, "value");
            }

            // Discount
            // If user selects cities for Discount
            if (comboBoxCustomerDiscount.Text == "Cities")
            {
                // Sql string to pass to display grapth method
                string SQLstring = "SELECT COALESCE(SUM(discount), 0) FROM FactTableAssignment " +
                                                    "JOIN Customer ON FactTableAssignment.customerId = Customer.id WHERE Customer.city = @selection";

                // Call method
                DisplayGraphs(connectionStringDestination, SQLstring, cities, discount, chartCustomerDiscount, "discount");
            }

            // If user selects states for Discount
            else if (comboBoxCustomerDiscount.Text == "States")
            {
                // Sql string to pass to display grapth method
                string SQLstring = "SELECT COALESCE(SUM(discount), 0) FROM FactTableAssignment " +
                                                    "JOIN Customer ON FactTableAssignment.customerId = Customer.id WHERE Customer.state = @selection";

                // Call method
                DisplayGraphs(connectionStringDestination, SQLstring, states, discount, chartCustomerDiscount, "discount");
            }

            // If user selects regions for Discount
            else if (comboBoxCustomerDiscount.Text == "Regions")
            {
                // Sql string to pass to display grapth method
                string SQLstring = "SELECT COALESCE(SUM(discount), 0) FROM FactTableAssignment " +
                                                    "JOIN Customer ON FactTableAssignment.customerId = Customer.id WHERE Customer.region = @selection";

                // Call method
                DisplayGraphs(connectionStringDestination, SQLstring, regions, discount, chartCustomerDiscount, "discount");
            }

            // If user selects countries for Discount
            else if (comboBoxCustomerDiscount.Text == "Countries")
            {
                // Sql string to pass to display grapth method
                string SQLstring = "SELECT COALESCE(SUM(discount), 0) FROM FactTableAssignment " +
                                                    "JOIN Customer ON FactTableAssignment.customerId = Customer.id WHERE Customer.country = @selection";

                // Call method
                DisplayGraphs(connectionStringDestination, SQLstring, countries, discount, chartCustomerDiscount, "discount");
            }

            // If user selects postcodes for Discount
            else if (comboBoxCustomerDiscount.Text == "Postcodes")
            {
                // Sql string to pass to display grapth method
                string SQLstring = "SELECT COALESCE(SUM(discount), 0) FROM FactTableAssignment " +
                                                    "JOIN Customer ON FactTableAssignment.customerId = Customer.id WHERE Customer.postalCode = @selection";

                // Call method
                DisplayGraphs(connectionStringDestination, SQLstring, postcodes, discount, chartCustomerDiscount, "discount");
            }
        }

        private void btnLoadProductData_Click(object sender, EventArgs e)
        {
            // Create dictionaries
            // Create a empty dictionary to store the destination data for sales
            Dictionary<String, dynamic> salesCount = new Dictionary<String, dynamic>();

            // Create an empty dictionary to store the destination data for profit
            Dictionary<String, dynamic> profit = new Dictionary<String, dynamic>();

            // Create an empty dictionary to store destination data for quantity
            Dictionary<String, dynamic> quantity = new Dictionary<String, dynamic>();

            // Create an empty dictionary to store the destination data for value
            Dictionary<String, dynamic> value = new Dictionary<String, dynamic>();

            // Create an empty dictionary to store the destination data for discount
            Dictionary<String, dynamic> discount = new Dictionary<String, dynamic>();

            // Create Lists 
            // Create empty list for Name
            List<String> names = new List<String>();

            // Create empty list for Category
            List<String> categories = new List<String>();

            // Create empty list for Sub-Category
            List<String> subcategories = new List<String>();

            // Create a connection to the MDF file 
            String connectionStringDestination = Properties.Settings.Default.DestinationDatabaseConnectionString;

            // Populate year list
            using (SqlConnection connection = new SqlConnection(connectionStringDestination))
            {
                // Open the connection
                connection.Open();

                // Query for reader to execute to get names
                SqlCommand command = new SqlCommand("SELECT DISTINCT name FROM Product", connection);

                // Create data reader
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    // Check the query returns data
                    if (reader.HasRows)
                    {
                        // While the reader reads the data
                        while (reader.Read())
                        {
                            // Add each name to list
                            names.Add(reader[0].ToString());
                        }
                    }
                }

                // Query for reader to execute to get categories
                SqlCommand command2 = new SqlCommand("SELECT DISTINCT category FROM Product", connection);

                // Create data reader
                using (SqlDataReader reader = command2.ExecuteReader())
                {
                    // Check the query returns data
                    if (reader.HasRows)
                    {
                        // While the reader reads the data
                        while (reader.Read())
                        {
                            // Add each category to list
                            categories.Add(reader[0].ToString());
                        }
                    }
                }

                // Query for reader to execute to get sub categories
                SqlCommand command3 = new SqlCommand("SELECT DISTINCT subcategory FROM Product", connection);

                // Create data reader
                using (SqlDataReader reader = command3.ExecuteReader())
                {
                    // Check the query returns data
                    if (reader.HasRows)
                    {
                        // While the reader reads the data
                        while (reader.Read())
                        {
                            // Add each sub category to list
                            subcategories.Add(reader[0].ToString());
                        }
                    }
                }
            }

            // Sales

            // If user selects names for sales
            if (comboBoxProductSales.Text == "Names")
            {
                // Sql string to pass to display grapth method
                string SQLstring = "SELECT COUNT(*) as SalesCount FROM FactTableAssignment " +
                                                    "JOIN Product ON FactTableAssignment.productId = Product.id WHERE Product.name = @selection";

                // Call method
                DisplayGraphs(connectionStringDestination, SQLstring, names, salesCount, chartProductSales, "sales");
            }
            // If user selects categories for sales
            else if (comboBoxProductSales.Text == "Categories")
            {
                // Sql string to pass to display grapth method
                string SQLstring = "SELECT COUNT(*) as SalesCount FROM FactTableAssignment " +
                                                    "JOIN Product ON FactTableAssignment.productId = Product.id WHERE Product.category = @selection";

                // Call method
                DisplayGraphs(connectionStringDestination, SQLstring, categories, salesCount, chartProductSales, "sales");
            }
            // If user selects sub categories for sales
            else if (comboBoxProductSales.Text == "Sub-Categories")
            {
                // Sql string to pass to display grapth method
                string SQLstring = "SELECT COUNT(*) as SalesCount FROM FactTableAssignment " +
                                                    "JOIN Product ON FactTableAssignment.productId = Product.id WHERE Product.subcategory = @selection";

                // Call method
                DisplayGraphs(connectionStringDestination, SQLstring, subcategories, salesCount, chartProductSales, "sales");
            }

            // Profit
            // If user selects names for profit
            if (comboBoxProductProfit.Text == "Names")
            {
                // Sql string to pass to display grapth method
                string SQLstring = "SELECT COALESCE(SUM(profit), 0) FROM FactTableAssignment " +
                                                    "JOIN Product ON FactTableAssignment.productId = Product.id WHERE Product.name = @selection";

                // Call method
                DisplayGraphs(connectionStringDestination, SQLstring, names, profit, chartProductProfit, "profit");
            }
            // If user selects categories for profit
            else if (comboBoxProductProfit.Text == "Categories")
            {
                // Sql string to pass to display grapth method
                string SQLstring = "SELECT COALESCE(SUM(profit), 0) FROM FactTableAssignment " +
                                                    "JOIN Product ON FactTableAssignment.productId = Product.id WHERE Product.category = @selection";

                // Call method
                DisplayGraphs(connectionStringDestination, SQLstring, categories, profit, chartProductProfit, "profit");
            }
            // If user selects sub categories for profit
            else if (comboBoxProductProfit.Text == "Sub-Categories")
            {
                // Sql string to pass to display grapth method
                string SQLstring = "SELECT COALESCE(SUM(profit), 0) FROM FactTableAssignment " +
                                                    "JOIN Product ON FactTableAssignment.productId = Product.id WHERE Product.subcategory = @selection";

                // Call method
                DisplayGraphs(connectionStringDestination, SQLstring, subcategories, profit, chartProductProfit, "profit");
            }

            // Quantity
            // If user selects names for quantity
            if (comboBoxProductQuantity.Text == "Names")
            {
                // Sql string to pass to display grapth method
                string SQLstring = "SELECT COALESCE(SUM(quantity), 0) FROM FactTableAssignment " +
                                                    "JOIN Product ON FactTableAssignment.productId = Product.id WHERE Product.name = @selection";

                // Call method
                DisplayGraphs(connectionStringDestination, SQLstring, names, quantity, chartProductQuantity, "quantity");
            }
            // If user selects categories for quantity
            else if (comboBoxProductQuantity.Text == "Categories")
            {
                // Sql string to pass to display grapth method
                string SQLstring = "SELECT COALESCE(SUM(quantity), 0) FROM FactTableAssignment " +
                                                    "JOIN Product ON FactTableAssignment.productId = Product.id WHERE Product.category = @selection";

                // Call method
                DisplayGraphs(connectionStringDestination, SQLstring, categories, quantity, chartProductQuantity, "quantity");
            }
            // If user selects sub categories for quantity
            else if (comboBoxProductQuantity.Text == "Sub-Categories")
            {
                // Sql string to pass to display grapth method
                string SQLstring = "SELECT COALESCE(SUM(quantity), 0) FROM FactTableAssignment " +
                                                    "JOIN Product ON FactTableAssignment.productId = Product.id WHERE Product.subcategory = @selection";

                // Call method
                DisplayGraphs(connectionStringDestination, SQLstring, subcategories, quantity, chartProductQuantity, "quantity");
            }

            // Value
            // If user selects names for value
            if (comboBoxProductValue.Text == "Names")
            {
                // Sql string to pass to display grapth method
                string SQLstring = "SELECT COALESCE(SUM(value), 0) FROM FactTableAssignment " +
                                                    "JOIN Product ON FactTableAssignment.productId = Product.id WHERE Product.name = @selection";

                // Call method
                DisplayGraphs(connectionStringDestination, SQLstring, names, value, chartProductValue, "value");
            }
            // If user selects categories for value
            else if (comboBoxProductValue.Text == "Categories")
            {
                // Sql string to pass to display grapth method
                string SQLstring = "SELECT COALESCE(SUM(value), 0) FROM FactTableAssignment " +
                                                    "JOIN Product ON FactTableAssignment.productId = Product.id WHERE Product.category = @selection";

                // Call method
                DisplayGraphs(connectionStringDestination, SQLstring, categories, value, chartProductValue, "value");
            }
            // If user selects sub categories for value
            else if (comboBoxProductValue.Text == "Sub-Categories")
            {
                // Sql string to pass to display grapth method
                string SQLstring = "SELECT COALESCE(SUM(value), 0) FROM FactTableAssignment " +
                                                    "JOIN Product ON FactTableAssignment.productId = Product.id WHERE Product.subcategory = @selection";

                // Call method
                DisplayGraphs(connectionStringDestination, SQLstring, subcategories, value, chartProductValue, "value");
            }

            // Discount
            // If user selects names for discount
            if (comboBoxProductDiscount.Text == "Names")
            {
                // Sql string to pass to display grapth method
                string SQLstring = "SELECT COALESCE(SUM(discount), 0) FROM FactTableAssignment " +
                                                    "JOIN Product ON FactTableAssignment.productId = Product.id WHERE Product.name = @selection";

                // Call method
                DisplayGraphs(connectionStringDestination, SQLstring, names, discount, chartProductDiscount, "discount");
            }
            // If user selects categories for discount
            else if (comboBoxProductDiscount.Text == "Categories")
            {
                // Sql string to pass to display grapth method
                string SQLstring = "SELECT COALESCE(SUM(discount), 0) FROM FactTableAssignment " +
                                                    "JOIN Product ON FactTableAssignment.productId = Product.id WHERE Product.category = @selection";

                // Call method
                DisplayGraphs(connectionStringDestination, SQLstring, categories, discount, chartProductDiscount, "discount");
            }
            // If user selects sub categories for discount
            else if (comboBoxProductDiscount.Text == "Sub-Categories")
            {
                // Sql string to pass to display grapth method
                string SQLstring = "SELECT COALESCE(SUM(discount), 0) FROM FactTableAssignment " +
                                                    "JOIN Product ON FactTableAssignment.productId = Product.id WHERE Product.subcategory = @selection";

                // Call method
                DisplayGraphs(connectionStringDestination, SQLstring, subcategories, discount, chartProductDiscount, "discount");
            }
        }
    }
}

