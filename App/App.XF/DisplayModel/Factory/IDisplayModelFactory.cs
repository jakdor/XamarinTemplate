namespace App.XF.DisplayModel.Factory
{
    internal interface IDisplayModelFactory<in T1, out T2>
    {
        T2 Create(T1 model);
    }
}
