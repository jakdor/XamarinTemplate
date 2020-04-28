namespace App.Web.Rest.Model.Product
{
    /// <summary>
    /// IToyaMobileService - product categories/manufacturers model
    /// </summary>
    public class ProductCategoryModel
    {
        public int Id { get; set; }
        public int IdProductGroup { get; set; }
        public string Name { get; set; }
    }
}