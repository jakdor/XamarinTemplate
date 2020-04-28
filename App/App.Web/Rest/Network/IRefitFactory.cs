namespace App.Web.Rest.Network
{
    public interface IRefitFactory
    {
        /// <summary>
        /// Build new Refit instance with provided base URL and Service interface
        /// </summary>
        /// <typeparam name="T">Service interface</typeparam>
        /// <param name="url">base URL</param>
        /// <returns>Refit instance</returns>
        T BuildService<T>(string url);
    }
}