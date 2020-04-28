using System;
using System.Reactive.Subjects;

namespace App.XF.Repository
{
    public interface IAuthRepository
    {
        bool IsLoggedIn();
        void Login(string username, string password, bool saveToken);
        void LogOut(bool force = false);
        BehaviorSubject<AuthStatusEnum> AuthStatus { get; }
        BehaviorSubject<LoginRequestStatus> LoginRequestStatusSubject { get; }
        IObservable<T> CallWithAuthToken<T>(Func<IObservable<T>> apiCallMethodFunc);
        string GetBearerToken();
        bool RefreshToken();
        string GetUserName();
    }

    public enum AuthStatusEnum
    {
        Idle, Logged, LoggedOut, LoggedOutUserAction, NoInternet
    }

    public enum LoginRequestStatus
    {
        Idle, Pending, Authorized, Unauthorized, BadLogin, NoNetwork, Error
    }
}
