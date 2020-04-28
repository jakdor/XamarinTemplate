using System;

namespace App.XF.ViewModel.Base
{
    /// <summary>
    /// Alert request bundle
    /// </summary>
    public class AlertRequest
    {
        public string Title { get; set; }
        public string Msg { get; set; }
        public AlertType Type { get; set; }
        public Action OkAction { get; set; }
        public Action CancelAction { get; set; }
        public string OkActionText { get; set; }
        public string CancelActionText { get; set; }

        public static AlertRequest OkAlertRequest(
            string title, string msg = null, Action okAction = null, string okActionText = null)
        {
            return new AlertRequest
            {
                Type = AlertType.Ok,
                Title = title,
                Msg = msg,
                OkAction = okAction ?? (() => { }),
                OkActionText = okActionText
            };
        }

        public static AlertRequest OkCancelAlertRequest(string title, Action okAction, string msg = null,
            Action cancelAction = null, string okActionText = null, string cancelActionText = null)
        {
            return new AlertRequest
            {
                Type = AlertType.OkCancel,
                Title = title,
                Msg = msg,
                OkAction = okAction,
                OkActionText = okActionText,
                CancelAction = cancelAction ?? (() => { }),
                CancelActionText = cancelActionText
            };
        }

        public enum AlertType
        {
            Ok, OkCancel
        }
    }
}
