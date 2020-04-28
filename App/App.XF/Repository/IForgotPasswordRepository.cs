using System.Reactive.Subjects;

namespace App.XF.Repository
{
    public interface IForgotPasswordRepository
    {
        void RequestPasswordReset(string login, string email);
        BehaviorSubject<ForgotPasswordRequestStatus> ForgotPasswordRequestStatusSubject { get; }
    }

    public enum ForgotPasswordRequestStatus
    {
        Idle, Pending, Success, BadData, NoNetwork, Error
    }
}
