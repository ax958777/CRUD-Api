using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Model
{
    public class Product
    {
        public int Id {get;set;}
        public string Name {get;set;}
        public string Description {get;set;}
        public decimal Price {get;set;}
        public int StockQuantity {get;set;}
        public string Category {get;set;}
        public DateTime CreatedAt {get;set;}
        public DateTime UpdatedAt {get;set;}
    }
}