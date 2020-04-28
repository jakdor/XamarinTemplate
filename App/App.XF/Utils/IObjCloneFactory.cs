namespace App.XF.Utils
{
    /// <summary>
    /// Factory for creating object clones
    /// </summary>
    public interface IObjCloneFactory
    {
        T DeepClone<T>(T obj);
    }
}
