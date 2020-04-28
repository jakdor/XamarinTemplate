using System;
using System.Collections.Generic;

namespace App.Web.Rest.Model.Product
{
    /// <summary>
    /// IToyaMobileService - single product entry
    /// </summary>
    public class ProductModel
    {
        public int Id { get; set; }
        public string ProductIndex { get; set; }
        public int InternalPos { get; set; }
        public string Name { get; set; }
        public string NameAlternative { get; set; }
        public string Description { get; set; }
        public string BarCode { get; set; }
        public decimal NetPrice { get; set; }
        public decimal NetPriceSap { get; set; } = -1;
        public bool IsStock { get; set; }
        public IList<string> UnitName { get; set; }
        public IList<decimal> UnitQuantity { get; set; }
        public DateTime CreationDate { get; set; }
        public bool LastProduct { get; set; }
        public bool HasReplacements { get; set; }
        public decimal CartAmount { get; set; }
        public decimal CartAmountSap { get; set; } = -1;
        public string Img { get; set; }
        public string ImgThumb { get; set; }
        public int LastUsedUnitNameIndex { get; set; } = -1;

        public int Quantity0100 { get; set; }
        public int Quantity0300 { get; set; }
        public int Quantity0500 { get; set; }
        public int QuantityPort { get; set; }

        public string QuantityZB { get; set; } = string.Empty;

        public int Quantity12H { get; set; }
        public int Quantity24H { get; set; }
        public int Quantity48H { get; set; }
        public int Quantity72H { get; set; }
        public int Quantity96H { get; set; }
    }
}