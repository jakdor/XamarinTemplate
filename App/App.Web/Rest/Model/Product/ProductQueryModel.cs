namespace App.Web.Rest.Model.Product
{
    /// <summary>
    /// IToyaMobileService - post model with query parameters
    /// </summary>
    public class ProductQueryModel
    {
        public CurrencyEnum Currency { get; set; } = CurrencyEnum.Pln;
        public int Limit { get; set; }
        public int Offset { get; set; }
        public string LikeName { get; set; }
        public int Category { get; set; }
        public int Manufacturer { get; set; }
        public double FromNetPrice { get; set; }
        public double ToNetPrice { get; set; }
        public string OrderByColumn { get; set; }
        public bool IsAscending { get; set; }
        public bool SkipPriceValidation { get; set; } = false;
        public bool SkipIsStockValidation { get; set; } = false;
    }
}