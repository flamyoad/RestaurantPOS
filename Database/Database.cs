using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Diagnostics;
using FlamyPOS.Models;
using System.Collections.ObjectModel;
using System.Windows;
using System.Data;
using FlamyPOS.ViewModels;
using FlamyPOS.Utilities;

namespace FlamyPOS.Data
{
    public static class Database
    {
        private static MySqlConnection connection;

        private static void CreateTables()
        {
            #region CREATE TABLE . . .
            string statement =
                @"CREATE TABLE IF NOT EXISTS Access
                (
                AccessLevel          INT NOT NULL AUTO_INCREMENT,
                Access               VARCHAR(20) NOT NULL,
                PRIMARY KEY(AccessLevel)
                );
                
                CREATE TABLE IF NOT EXISTS Staffs
                (
                Staff_ID                INT          NOT NULL   AUTO_INCREMENT,
                Staff_Name              VARCHAR(50)  NOT NULL,
                Staff_Passcode          VARCHAR(64)  NOT NULL   UNIQUE,
                JoinedDateTime          DATETIME     NOT NULL,
                LeaveDateTime           DATETIME,
                AccessLevel             INT          NOT NULL,
                PRIMARY KEY(Staff_ID),
                FOREIGN KEY (AccessLevel) REFERENCES Access(AccessLevel)
                );
                
                CREATE TABLE IF NOT EXISTS Product_Category
                (
                Product_CategoryID      INT NOT NULL AUTO_INCREMENT,
                Product_CategoryName    VARCHAR(50) NOT NULL,
                PRIMARY KEY(Product_CategoryID)
                );
                
                CREATE TABLE IF NOT EXISTS Products
                (
                Product_ID			    INT          NOT NULL AUTO_INCREMENT,
                Product_Name		    VARCHAR(50)  NOT NULL,
                Product_Price		    NUMERIC(8,2) NOT NULL,
                Product_CategoryID	    INT 		 NOT NULL,
                IsVisible			    BIT(1)       NOT NULL,
                PRIMARY KEY(Product_ID),
                FOREIGN KEY(Product_CategoryID) REFERENCES Product_Category(Product_CategoryID)
                );
                
                CREATE TABLE IF NOT EXISTS Tables
                (
                Table_ID          INT NOT NULL AUTO_INCREMENT,
                TableName         VARCHAR(50)  NOT NULL,
                CoordsX           DOUBLE       NOT NULL,
                CoordsY           DOUBLE       NOT NULL,
                IsTaken           BIT(1)       NOT NULL,
                IsVisible         BIT(1)       NOT NULL,
                PRIMARY KEY(Table_ID)
                );
                
                CREATE TABLE IF NOT EXISTS Orders
                (
                Order_ID                INT             NOT NULL    AUTO_INCREMENT,
                Staff_ID                INT             NOT NULL,
                Table_ID                INT             NOT NULL,
                Receipt_Datetime        DATETIME        NOT NULL,
                BillTotal               NUMERIC(8,2)    NOT NULL,
                IsSettled               BIT(1)          NOT NULL,
                PRIMARY KEY(Order_ID),
                FOREIGN KEY(Staff_ID) REFERENCES Staffs(Staff_ID),
                FOREIGN KEY(Table_ID) REFERENCES Tables(Table_ID)
                );
                
                CREATE TABLE IF NOT EXISTS OrderLine
                (
                OrderLine_ID                INT          NOT NULL AUTO_INCREMENT,
                Order_ID                    INT          NOT NULL,
                Product_ID                  INT          NOT NULL,
                TotalPrice                  NUMERIC(8,2) NOT NULL,
                Quantity                    INT          NOT NULL,
                PRIMARY KEY(OrderLine_ID),
                FOREIGN KEY(Order_ID) REFERENCES Orders(Order_ID),
                FOREIGN KEY(Product_ID) REFERENCES Products(Product_ID)
                );               
             ";
            #endregion 
            ExecuteNonQuery(statement);
        }


        public static void Initialize()
        {
            string server = "localhost";
            string port = "3306";
            string database = "FlamyPOS";
            string uid = "root";
            string password = "123456";
            connection = new MySqlConnection(
                $"SERVER={server};PORT={port};DATABASE={database};UID={uid};PASSWORD={password};SSLMODE=NONE;"); // POOLING=FALSE

            CreateTables();
        }

        private static void OpenConnection()
        {
            try
            {
                if (connection.State.HasFlag(ConnectionState.Open))
                { return; }
                else
                { connection.Open(); }
            }
            catch (MySqlException e)
            {
                Debug.WriteLine(e.Number + " " + e.Message);
                Debug.WriteLine(e.StackTrace);
            }
        }

        private static void CloseConnection()
        {
            try
            {
                connection.Close();
            }
            catch (MySqlException e)
            {
                Debug.WriteLine(e.Number + " " + e.Message);
                Debug.WriteLine(e.StackTrace);
            }
        }

        public static bool ExecuteNonQuery(string query)
        {
            OpenConnection();
            try
            {
                using (var cmd = new MySqlCommand(query, connection))
                {
                    cmd.ExecuteNonQuery();
                    return true;
                }
            }
            catch (MySqlException e)
            {
                Debug.WriteLine(e.Number + " " + e.Message);
                Debug.WriteLine(e.StackTrace);
                Debug.WriteLine(e.TargetSite);
                return false;
            }
            finally
            {
                CloseConnection();
            }
        }

        public static bool ExecuteNonQuery(string query, out int lastInsertedId)
        {
            OpenConnection();
            try
            {
                using (var cmd = new MySqlCommand(query, connection))
                {
                    cmd.ExecuteNonQuery();
                    lastInsertedId = (int)cmd.LastInsertedId;
                    return true;
                }
            }
            catch (MySqlException e)
            {
                Debug.WriteLine(e.Number + " " + e.Message);
                Debug.WriteLine(e.StackTrace);
                lastInsertedId = -1;
                return false;
            }
            finally
            {
                CloseConnection();
            }
        }

        public static MySqlDataReader ExecuteReader(string query)
        {
            OpenConnection();
            MySqlDataReader reader = null;
            try
            {
                var cmd = new MySqlCommand(query, connection);
                reader = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
            }
            catch (MySqlException e)
            {
                CloseConnection();
                Debug.WriteLine(e.Number + " " + e.Message);
                Debug.WriteLine(e.StackTrace);
            }
            return reader;
        }

        //public static bool Insert(string tablename, string values)
        //{
        //    string query = $"INSERT INTO {tablename} VALUES({values})";
        //    return ExecuteNonQuery(query);
        //}

        public static int Insert(string tablename, string values)
        {
            string query = $"INSERT INTO {tablename} VALUES({values})";
            var status = ExecuteNonQuery(query, out int lastUsedId);
            return lastUsedId;
        }

        public static bool Insert(string tablename, string values, out int lastUsedId)
        {
            string query = $"INSERT INTO {tablename} VALUES({values})";
            var status = ExecuteNonQuery(query, out lastUsedId);
            return status;
        }

        public static int InsertOrder(string values)
        {
            string query = $@"INSERT INTO Orders (Order_ID, Staff_ID, Table_ID, Receipt_Datetime, BillTotal, IsSettled)
                              VALUES ({values});";
            var status = ExecuteNonQuery(query, out int lastUsedId);
            return lastUsedId;
        }

        public static int InsertOrder(string values, double newBillTotal)
        {
            string query = $@"INSERT INTO Orders (Order_ID, Staff_ID, Table_ID, Receipt_Datetime, BillTotal, IsSettled)
                              VALUES ({values})
                              ON DUPLICATE KEY UPDATE BillTotal = {newBillTotal};";
            var status = ExecuteNonQuery(query, out int lastUsedId);
            return lastUsedId;
        }

        public static int Replace(string tableName, string values)
        {
            string query = $@"REPLACE INTO {tableName} VALUES({values})";
            var status = ExecuteNonQuery(query, out int lastUsedId);
            return lastUsedId;
        }

        public static ObservableCollection<Product> GetProducts(ProductCategory category)
        {
            string categoryName = Enum.GetName(typeof(ProductCategory), category);
            string query = $@"SELECT * 
                              FROM Products
                              WHERE Product_CategoryID = (SELECT Product_CategoryID 
                                                          FROM Product_Category
                                                          WHERE Product_CategoryName = '{categoryName}')
                                    AND IsVisible = 1;";
            var reader = ExecuteReader(query);
            var list = new ObservableCollection<Product>();
            if (reader != null)
            {
                using (reader)
                {
                    while (reader.Read())
                    {
                        list.Add(new Product()
                        {
                            ID = reader.GetInt32("Product_ID"),
                            Name = reader.GetString("Product_Name"),
                            Price = reader.GetDouble("Product_Price"),
                            CategoryID = reader.GetInt32("Product_CategoryID"),
                        });
                    }
                }
            }
            reader.Close();
            CloseConnection();
            return list;
        }

        public static ObservableCollection<Order> GetAllOrders()
        {
            string query = $@"SELECT * FROM {Columns.ORDERS}
                              WHERE IsSettled = 0";
            var reader = ExecuteReader(query);

            var list = new ObservableCollection<Order>();
            if (reader != null)
            {
                while (reader.Read())
                {
                    list.Add(new Order()
                    {
                        Id = reader.GetInt32("Order_ID"),
                        StaffID = reader.GetInt32("Staff_ID"),
                        TableID = reader.GetInt32("Table_ID"),
                        OrderDateTime = reader.GetDateTime("Receipt_Datetime"),
                        BillTotal = reader.GetDouble("BillTotal")
                    });
                }
                reader.Close();
            }
            CloseConnection();
            return list;
        }

        public static Dictionary<string, int> GetOrderIdByTable()
        {
            string query = $@"SELECT Orders.Order_ID, Tables.TableName
                              FROM Orders
                              INNER JOIN Tables ON Orders.Table_ID = Tables.Table_ID
                              WHERE Orders.IsSettled = 0";
            var map = new Dictionary<string, int>();
            var reader = ExecuteReader(query);
            if (reader != null)
            {
                while (reader.Read())
                {
                    map.Add(reader.GetString("TableName"),
                            reader.GetInt32("Order_ID"));
                }
            }
            reader.Close();
            CloseConnection();
            return map;
        }

        public static Dictionary<string, int> GetTableIdByName()
        {
            string query = $@"SELECT * FROM {Columns.TABLES}
                              WHERE IsVisible = 1;";
            var map = new Dictionary<string, int>();
            var reader = ExecuteReader(query);
            if(reader != null)
            {
                while (reader.Read())
                {
                    map.Add(reader.GetString("TableName"),
                            reader.GetInt32("Table_ID"));
                }
            }
            reader.Close();
            CloseConnection();
            return map;
        }


        public static Dictionary<int, Product> GetProductById()
        {
            string query = $@"SELECT * FROM {Columns.PRODUCTS}
                              WHERE IsVisible = 1;";

            var map = new Dictionary<int, Product>();
            var reader = ExecuteReader(query);
            if (reader != null)
            {
                while (reader.Read())
                {
                    int Id = reader.GetInt32("Product_ID");
                    Product item = new Product
                    {
                        ID = reader.GetInt32("Product_ID"),
                        Name = reader.GetString("Product_Name"),
                        Price = reader.GetDouble("Product_Price"),
                        CategoryID = reader.GetInt32("Product_CategoryID")
                    };
                    map.Add(Id, item);
                }
            }
            reader.Close();
            CloseConnection();
            return map;
        }

        public static Dictionary<string, DateTime> GetOrderDateByTable()
        {
            string query = $@"SELECT Orders.Receipt_Datetime, Tables.TableName
                              FROM Orders
                              INNER JOIN Tables ON Orders.Table_ID = Tables.Table_ID
                              WHERE Orders.IsSettled = 0";
            var map = new Dictionary<string, DateTime>();
            var reader = ExecuteReader(query);
            if (reader != null)
            {
                while (reader.Read())
                {
                    map.Add(reader.GetString("TableName"),
                            reader.GetDateTime("Receipt_Datetime"));
                }
            }
            reader.Close();
            CloseConnection();
            return map;
        }

        public static List<Staff> GetAllStaffs()
        {
            string query = $@"SELECT * FROM {Columns.STAFFS}
                              WHERE AccessLevel != 3";
            var reader = ExecuteReader(query);

            var list = new List<Staff>();
            if (reader != null)
            {
                while (reader.Read())
                {
                    list.Add(new Staff()
                    {
                        Id = reader.GetInt32("Staff_ID"),
                        Name = reader.GetString("Staff_Name"),
                        JoinedDate = reader.GetDateTime("JoinedDateTime"),
                        LeaveDate = reader.GetDateTime("LeaveDateTime"),
                        Position = Enum.GetName(typeof(Access), (reader.GetInt32("AccessLevel")))
                    });
                }
                reader.Close();
            }
            CloseConnection();
            return list;
        }

        public static ObservableCollection<OrderLine> GetOrderLine(int orderId)
        {
            string query = $@"SELECT * FROM OrderLine
                              WHERE Order_ID = {orderId}";
            var orderlines = new ObservableCollection<OrderLine>();
            var reader = ExecuteReader(query);
            if (reader != null)
            {
                while (reader.Read())
                {
                    var prodId = reader.GetInt32("Product_ID");
                    orderlines.Add(new OrderLine()
                    {
                        Id = reader.GetInt32("OrderLine_ID"),
                        SelectedProduct = Mains.ProductById[prodId],
                        Quantity = reader.GetInt32("Quantity"),
                        TotalPrice = reader.GetDouble("TotalPrice")
                    });
                }
            }
            reader.Close();
            CloseConnection();
            return orderlines;
        }

        public static Dictionary<string, int> GetPasscodes()
        {
            var staffDictionary = new Dictionary<string, int>();
            string query = $@"SELECT * FROM {Columns.STAFFS}
                              WHERE AccessLevel != 3";
            OpenConnection();
            using (var cmd = new MySqlCommand(query, connection))
            {
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        staffDictionary.Add(reader.GetString("Staff_Passcode"),
                                            reader.GetInt32("Staff_ID"));
                    }
                }
            }
            CloseConnection();
            return staffDictionary;
        }

        /// <summary>
        /// Soft delete Product columns
        /// </summary>
        /// <param name="prod"></param>
        /// <returns></returns>
        public static bool DeleteProduct(Product prod)
        {
            string statement = $@"UPDATE {Columns.PRODUCTS}
                                  SET    IsVisible = 0
                                  WHERE  Product_ID = {prod.ID}";
            bool status = ExecuteNonQuery(statement);
            return status;
        }

        /// <summary>
        /// Promote/Derank/Remove Staffs
        /// </summary>
        /// <param name="staffName"></param>
        /// <returns></returns>
        public static bool UpdateStaffPosition(string name, Access access)
        {
            int staffId = Mains.AdminVM.StaffList.Where(x => x.Name == name)
                                                 .Select(x => x.Id)
                                                 .FirstOrDefault();
            string statement = $@"UPDATE  {Columns.STAFFS}
                                  SET     AccessLevel = {(int)access}
                                  WHERE   Staff_ID = {staffId};
                                 ";
            bool status = ExecuteNonQuery(statement);

            if (access == Access.Staff || access == Access.Supervisor)
                return status;

            if (access == Access.Disabled)
            {
                var randomGenerator = new Random();
                int randomNumber = randomGenerator.Next();
                string hashedRandomNumber = Security.GetHash(name + randomNumber.ToString());
                statement = $@"UPDATE  {Columns.STAFFS}
                               SET     Staff_Passcode = '{hashedRandomNumber}'
                               WHERE   Staff_ID = {staffId};
                                 ";
            }
            status = ExecuteNonQuery(statement);

            string staffPasscode =
                (from kvp in Mains.LoginVM.StaffDictionary
                 where kvp.Value == staffId
                 select kvp.Key).FirstOrDefault();
            if (staffPasscode != null)
            {
                Mains.LoginVM.StaffDictionary.Remove(staffPasscode);
            }
            return status;
        }

        public static int GetStaffId(string name)
        {
            OpenConnection();
            try
            {
                string unescapedName = Helpers.GetUnescapedString(name);
                string statement = $@"SELECT Staff_ID FROM {Columns.STAFFS}
                                      WHERE Staff_Name = '{unescapedName} AND AccessLevel != 3'
                                     ";
                using (var cmd = new MySqlCommand(statement, connection))
                {
                    int Id = Convert.ToInt32(cmd.ExecuteScalar());
                    return Id;
                }
            }
            catch(MySqlException e)
            {
                Debug.WriteLine(e.Number + " " + e.Message);
                Debug.WriteLine(e.StackTrace);
                return -1;
            }
            finally
            {
                CloseConnection();
            }
        }

        public static Staff GetStaff(int id)
        {
            OpenConnection();
            try
            {
                string statement = $@"SELECT * FROM {Columns.STAFFS}
                                      WHERE Staff_ID = '{id}'
                                     ";
                var reader = ExecuteReader(statement);
                var staff = new Staff();
                if(reader != null)
                {
                    while(reader.Read())
                    {
                        staff = new Staff()
                        {
                            Id = reader.GetInt32("Staff_ID"),
                            Name = reader.GetString("Staff_Name"),
                            JoinedDate = reader.GetDateTime("JoinedDateTime"),
                            LeaveDate = reader.GetDateTime("LeaveDateTime"),
                            Position = Enum.GetName(typeof(Access), reader.GetInt32("AccessLevel"))
                        };
                    }
                }
                return staff;
            }
            catch (MySqlException e)
            {
                Debug.WriteLine(e.Number + " " + e.Message);
                Debug.WriteLine(e.StackTrace);
                return null;
            }
            finally
            {
                CloseConnection();
            }
        }

        public static void CommencePayment(int orderId)
        {
            string statement = $@"UPDATE Orders
                                  SET IsSettled = 1
                                  WHERE Order_ID = {orderId}";
            ExecuteNonQuery(statement);
        }

        public static ObservableCollection<Item> GetTopProductsAllTime()
        {
            ObservableCollection<Item> list = new ObservableCollection<Item>();
            string query = $@"SELECT Product_ID, SUM(TotalPrice) AS Total, COUNT(*) AS QuantitySold
                              FROM {Columns.ORDERLINE}
                              GROUP BY Product_ID
                              ORDER BY Total DESC 
                              LIMIT 10";
            var reader = ExecuteReader(query);
            if (reader != null)
            {
                while (reader.Read())
                {
                    var product = Mains.ProductById[reader.GetInt32("Product_ID")];
                    list.Add(new Item()
                    {
                        Name = product.Name,
                        ProductCategory = product.Category,
                        TotalSale = reader.GetDouble("Total"),
                        QuantitySold = reader.GetInt32("QuantitySold")
                    });
                }
                reader.Close();
            }
            CloseConnection();
            return list;
        }

        public static ObservableCollection<Item> GetAllProductsOnMonth(DateTime dt)
        {
            ObservableCollection<Item> list = new ObservableCollection<Item>();
            DateTime startDate = new DateTime(dt.Year, dt.Month, 1);
            DateTime endDate = startDate.AddMonths(1).AddDays(-1);
            string startDateString = startDate.ToString("yyyy-MM-dd HH:mm:ss");
            string endDateString = endDate.ToString("yyyy-MM-dd HH:mm:ss");
            string query = $@"SELECT Product_Name, SUM(TotalPrice) AS Total, SUM(Quantity) AS QuantitySold, Product_CategoryName
                              FROM OrderLine
                              INNER JOIN Orders ON Orders.Order_Id = OrderLine.Order_Id
                              INNER JOIN Products ON OrderLine.Product_ID = Products.Product_ID  
                              INNER JOIN Product_Category ON Products.Product_CategoryID = Product_Category.Product_CategoryID
                              WHERE Orders.Receipt_Datetime BETWEEN '{startDateString}' AND '{endDateString}'
                              GROUP BY OrderLine.Product_ID
                              ORDER BY Total DESC;";
            var reader = ExecuteReader(query);
            if (reader != null)
            {
                while (reader.Read())
                {
                    list.Add(new Item()
                    {
                        Name = reader.GetString("Product_Name"),
                        TotalSale = reader.GetDouble("Total"),
                        QuantitySold = reader.GetInt32("QuantitySold"),
                        ProductCategory = reader.GetString("Product_CategoryName")
                    });
                }
                reader.Close();
            }
            CloseConnection();
            return list;
        }

        public static ObservableCollection<Item> GetTopProductsOnMonth(DateTime dt)
        {
            ObservableCollection<Item> list = new ObservableCollection<Item>();
            DateTime startDate = new DateTime(dt.Year, dt.Month, 1);
            DateTime endDate = startDate.AddMonths(1).AddDays(-1);
            string startDateString = startDate.ToString("yyyy-MM-dd HH:mm:ss");
            string endDateString = endDate.ToString("yyyy-MM-dd HH:mm:ss");
            string query = $@"SELECT Product_Name, SUM(TotalPrice) AS Total, SUM(Quantity) AS QuantitySold
                              FROM OrderLine
                              INNER JOIN Orders ON Orders.Order_Id = OrderLine.Order_Id
                              INNER JOIN Products ON OrderLine.Product_ID = Products.Product_ID            
                              WHERE Orders.Receipt_Datetime BETWEEN '{startDateString}' AND '{endDateString}'
                              GROUP BY OrderLine.Product_ID
                              ORDER BY Total DESC 
                              LIMIT 10;";
            var reader = ExecuteReader(query);
            if (reader != null)
            {
                while (reader.Read())
                {
                    list.Add(new Item()
                    {
                        Name = reader.GetString("Product_Name"),
                        TotalSale = reader.GetDouble("Total"),
                        QuantitySold = reader.GetInt32("QuantitySold")
                    });
                }
                reader.Close();
            }
            CloseConnection();
            return list;
        }

        public static void OverwriteTables(List<Table> tableList)
        {
            string reset = $@"UPDATE {Columns.TABLES}
                              SET IsVisible = 0";
            ExecuteNonQuery(reset);
            foreach (Table t in tableList)
            {
                string name = Helpers.GetUnescapedString(t.Name);
                string query = $@"INSERT INTO Tables (Table_ID, TableName, CoordsX, CoordsY, IsVisible, IsTaken)
                              VALUES ({t.Id}, '{name}', {t.CoordsX}, {t.CoordsY}, 1, {t.IsTaken})
                              ON DUPLICATE KEY UPDATE TableName = VALUES(TableName),
                              CoordsX = VALUES(CoordsX),
                              CoordsY = VALUES(CoordsY),
                              IsVisible = VALUES(IsVisible),
                              IsTaken = VALUES(IsTaken);
                             ";
                ExecuteNonQuery(query, out int lastInsertedId);
                if (!Mains.OrderVM.TableIdByName.ContainsKey(t.Name))
                {
                    Mains.OrderVM.TableIdByName.Add(name, lastInsertedId);
                    t.Id = lastInsertedId;
                }
            }
        }

        public static List<Table> GetTables()
        {
            List<Table> tableList = new List<Table>();
            string query = $@"SELECT * FROM {Columns.TABLES}
                              WHERE IsVisible = 1";
            var reader = ExecuteReader(query);
            if (reader != null)
            {
                while (reader.Read())
                {
                    tableList.Add(new Table()
                    {
                        Id = reader.GetInt32("Table_ID"),
                        Name = reader.GetString("TableName"),
                        CoordsX = reader.GetDouble("CoordsX"),
                        CoordsY = reader.GetDouble("CoordsY"),
                        IsTaken = reader.GetBoolean("IsTaken")
                    });
                }
                reader.Close();
            }
            CloseConnection();
            return tableList;
        }

        public static void SetTableTaken(string tableName, bool tableStatus)
        {
            int tableId = Mains.TableButtonList
                 .Where(t => t.Name == tableName)
                 .Select(t => t.Id)
                 .FirstOrDefault();

            Table table = Mains.TableButtonList.Find(x => x.Id == tableId);
            table.IsTaken = tableStatus;

            string query = $@"UPDATE Tables
                              SET IsTaken = {tableStatus}
                              WHERE TableName = '{tableName}';
                            ";
            ExecuteNonQuery(query);
        }

        public static void ChangePrice(Product product)
        {
            string query = $@"UPDATE Products
                              SET Product_Price = {product.Price}
                              WHERE Product_ID = {product.ID};
                            ";
            ExecuteNonQuery(query);                            
        }

        public static void ChangeTable(ObservableCollection<OrderLine> orderlines,
                                       Staff staff,
                                       string newTableName,
                                       string prevTableName,
                                       int previousOrderId,
                                       int newOrderId)
        {
            if (previousOrderId != default(int)) { CommencePayment(previousOrderId); }
            if (newOrderId != default(int)) { CommencePayment(newOrderId); }

            int orderId = InsertOrder($@"NULL, 
                                        {staff.Id},
                                        {Mains.TableIdByName[newTableName]},
                                        '{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}',
                                        0,
                                        0");

            Mains.TableOverviewVM.OrderIdByTable[newTableName] = orderId;
            foreach (var line in orderlines)
            {
                int id = Database.Insert(Columns.ORDERLINE, $@"NULL,
                                                              {orderId},
                                                              {line.SelectedProduct.ID},
                                                              {line.TotalPrice},
                                                              {line.Quantity}");
                line.Id = id;
            }
            SetTableTaken(newTableName, true);
            SetTableTaken(prevTableName, false);
            Mains.TableOverviewVM.OrderlinesByTable[prevTableName].Clear();
            Mains.TableOverviewVM.OrderlinesByTable[newTableName] = orderlines;
        }

        public static bool UpdatePasscode(string name, string newHashedPasscode)
        {
            string query = $@"UPDATE {Columns.STAFFS}
                              SET Staff_Passcode = '{newHashedPasscode}'
                              WHERE Staff_Name = '{name}'";
            bool status = ExecuteNonQuery(query);
            return status;                                               
        }
    }

    public static class Columns
    {
        public const string ACCESS = "Access";
        public const string STAFFS = "Staffs";
        public const string PRODUCT_CATEGORY = "Product_Category";
        public const string PRODUCTS = "Products";
        public const string TABLES = "Tables";
        public const string ORDERS = "Orders";
        public const string ORDERLINE = "OrderLine";
    }
}
