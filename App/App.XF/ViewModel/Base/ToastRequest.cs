namespace App.XF.ViewModel.Base
{
    /// <summary>
    /// Toast request bundle
    /// </summary>
    public class ToastRequest
    {
        public string Msg { get; set; }
        public ToastLength Length { get; set; }

        public enum ToastLength
        {
            Short, Long
        }
    }
}
