using System;
using System.Collections.Generic;
using App.Web.Rest.Model.Auth;
using App.Web.Rest.Model.Product;

namespace App.Web
{
    public interface IRestServices
    {
        void SetUrl(string url);
        string FormatAuthHeader(string login, string token);
        IObservable<T> CallWithAuthToken<T>( Func<IObservable<T>> apiCallMethodFunc, Action unauthorizedFunc,
            Func<bool> refreshTokenFunc, Action noNetwork, bool hasInternetService);

        IObservable<LoginResult> PostLogin(string login, string password);
        IObservable<ForgotPasswordResult> PostForgotPassword(string login, string email);
        IObservable<TokenModel> PostRefresh(string login, string refreshToken);
        IObservable<IList<ProductModel>> PostProductList(ProductQueryModel productQuery, string login, string token);

        Func<IObservable<IList<ProductModel>>> PostProductListFunc(
            ProductQueryModel productQuery, string login, Func<string> getTokenFunc);
    }
}
