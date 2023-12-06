using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace DataSource
{
    public class StoreInventoryDB : DatabaseHandler
    {

        public StoreInventoryDB(string connection_string) : base(connection_string) 
        {
            
        }

        public List<StoreObject> getStoreData()
        {
            List<StoreObject> result = new List <StoreObject>();
            string query = "SELECT Id, Name, Street, City, State, Zip FROM Store ORDER BY Name ASC";
            SqlDataReader reader = this.ExecuteQuery(query);
            
            while (reader.Read())
            {
                int storeId = reader.GetInt32(0);
                string storeName = reader.GetString(1);
                string storeStreet = reader.GetString(2);
                string storeCity = reader.GetString(3);
                string storeState = reader.GetString(4);
                string storeZip = reader.GetString(5);
                result.Add(new StoreObject(storeId, storeName, storeStreet, storeCity, storeState, storeZip));
            }

            reader.Close();

            return result;
        }

        public List<ProductObject> getProductData()
        {
            List <ProductObject> result = new List<ProductObject>();

            string query = "SELECT Id, Manufacturer, Brand FROM Product ORDER BY Manufacturer ASC, Brand ASC";
            SqlDataReader reader = this.ExecuteQuery(query);
            
            while (reader.Read())
            {
                int productId = reader.GetInt32(0);
                string manufacturer = reader.GetString(1);
                string brand = reader.GetString(2);
                
                result.Add(new ProductObject(productId, manufacturer, brand));
            }

            reader.Close();

            return result;
        }

        public List<InventoryObject> getInventoryData(int storeId)
        {
            List<InventoryObject> result = new List<InventoryObject>();

            string query = "SELECT si.Id AS InventoryId, si.StoreId, p.Id as ProductId, p.Manufacturer, p.Brand FROM Product p INNER JOIN StoreInventory si ON p.Id = si.ProductId WHERE si.StoreId = @StoreId ORDER BY p.Manufacturer ASC, p.Brand ASC";
            //string query = "SELECT si.Id AS InventoryId, p.Id as ProductId, p.Manufacturer, p.Brand FROM Product p INNER JOIN StoreInventory si ON p.Id = si.ProductId WHERE si.StoreId = @StoreId ORDER BY p.Manufacturer ASC, p.Brand ASC";
            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@StoreId", storeId)
            };
            
            SqlDataReader reader = this.ExecuteQuery(query, parameters);

            while (reader.Read())
            {
                int id = reader.GetInt32(0);
                //int storeIdFromDB = reader.GetInt32(1);
                int productId = reader.GetInt32(2);
                string manufacturer = reader.GetString(3);
                string brand = reader.GetString(4);

                result.Add(new InventoryObject(id, storeId, productId, manufacturer, brand));
            }

            reader.Close();

            return result;
        }

        public string Add(StoreObject item)
        {
            try
            {
                string query = "INSERT INTO Store (Name, Street, City, State, Zip) VALUES (@Name, @Street, @City, @State, @Zip)";

                List<SqlParameter> parameters = new List<SqlParameter>
                {
                    new SqlParameter("@Name", item.Name),
                    new SqlParameter("@Street", item.Street),
                    new SqlParameter("@City", item.City),
                    new SqlParameter("@State", item.State),
                    new SqlParameter("@Zip", item.Zip)
                };

                this.ExecuteNonQuery(query, parameters);
                return null;
            }

            catch (Exception ex)
            {
                return $"Error adding store: {ex.Message}";
            }
        }

        public string Add(ProductObject item)
        {
            try
            {
                string query = "INSERT INTO Product (Manufacturer, Brand) VALUES (@Manufacturer, @Brand)";
                List<SqlParameter> parameters = new List<SqlParameter>
                {
                    new SqlParameter("@Manufacturer", item.Manufacturer),
                    new SqlParameter("@Brand", item.Brand)
                };

                this.ExecuteNonQuery(query, parameters);
                return null;
            }
            catch (Exception ex)
            {
                return $"Error adding product: {ex.Message}";
            }
        }

        public string Add(InventoryObject item)
        {
            try
            {
                // Sprawdzanie czy produkt istnieje w asortymencie sklepu
                string checkQuery = "SELECT COUNT(*) FROM StoreInventory WHERE StoreId = @StoreId AND ProductId = @ProductId";
                List<SqlParameter> checkParameters = new List<SqlParameter>
                {
                    new SqlParameter("@StoreId", item.StoreId),
                    new SqlParameter("@ProductId", item.ProductId)
                };

                int productCount = this.ExecuteScalar<int>(checkQuery, checkParameters);

                if (productCount == 0)
                {
                    // Dodawanie produktu do asortymentu sklepu
                    string addQuery = "INSERT INTO StoreInventory (StoreId, ProductId) VALUES (@StoreId, @ProductId)";
                    List<SqlParameter> addParameters = new List<SqlParameter>
                    {
                        new SqlParameter("@StoreId", item.StoreId),
                        new SqlParameter("@ProductId", item.ProductId)
                    };

                    this.ExecuteNonQuery(addQuery, addParameters);

                    return null;
                }
                else
                {
                    return "Ten produkt już istnieje w asortymencie sklepu.";
                }
            }
            catch (Exception ex)
            {
                return $"Error adding inventory: {ex.Message}";
            }
        }

        public string Delete(StoreObject item)
        {
            try
            {
                string query = "DELETE FROM Store WHERE Id = @Id";
                List<SqlParameter> parameters = new List<SqlParameter>
                {
                    new SqlParameter("@Id", item.id)
                };

                this.ExecuteNonQuery(query, parameters);
                return null;
            }
            catch (Exception ex)
            {
                return $"Error deleting store: {ex.Message}";
            }
        }

        public string Delete(ProductObject item)
        {
            try
            {
                string query = "DELETE FROM Product WHERE Id = @Id";
                List<SqlParameter> parameters = new List<SqlParameter>
                {
                    new SqlParameter("@Id", item.id)
                };

                this.ExecuteNonQuery(query, parameters);
                return null;
            }
            catch (Exception ex)
            {
                return $"Error deleting product: {ex.Message}";
            }
        }

        public string Delete(InventoryObject item)
        {
            try
            {
                string query = "DELETE FROM StoreInventory WHERE Id = @Id";
                List<SqlParameter> parameters = new List<SqlParameter> 
                { 
                    new SqlParameter("@id", item.id) 
                };

                this.ExecuteNonQuery(query, parameters);
                return null;
                
            }
            catch (Exception ex)
            {
                return $"Error deleting product from store: {ex.Message}";
            }
        }

    }
}
