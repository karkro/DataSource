using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataSource
{
    public class StoreObject
    {
        public StoreObject()
        {
            
        }

        public StoreObject(int Id, string Name, string Street, string City, string State, string Zip)
        {
            this.id = Id;
            this.Name = Name;
            this.Street = Street;
            this.City = City;
            this.State = State;
            this.Zip = Zip;
        }

        public int id { get; set; }
        public string Name { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }

        public override string ToString()
        {
            return this.Name + "(" + this.id + ")";
        }
    }

    public class ProductObject
    {
        public ProductObject()
        {

        }

        public ProductObject(int Id, string Manufacturer, string Brand)
        {
            this.id = Id;
            this.Manufacturer = Manufacturer;
            this.Brand = Brand;
        }

        public int id { get; set; }
        public string Manufacturer { get; set; }
        public string Brand { get; set; }

        public override string ToString()
        {
            return this.Manufacturer + " " + this.Brand + " (" + this.id + ")";
        }
    }

    public class InventoryObject
    {
        public InventoryObject()
        {

        }

        public InventoryObject(int Id, int StoreId, int ProductId, string Manufacturer, string Brand)
        {
            this.id = Id;
            this.StoreId = StoreId;
            this.ProductId = ProductId;
            this.Manufacturer = Manufacturer;
            this.Brand = Brand;
        }

        public int id { get; set; }
        public int StoreId { get; set; }
        public int ProductId { get; set; }
        public string Manufacturer { get; set; }
        public string Brand { get; set; }

        public override string ToString()
        {
            return "(" + this.id + ")" + this.Manufacturer + " " + this.Brand;
        }
    }
}
