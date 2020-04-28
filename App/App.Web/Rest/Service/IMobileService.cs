using System;
using System.Collections.Generic;
using App.Web.Rest.Model.Auth;
using App.Web.Rest.Model.Product;
using Refit;

namespace App.Web.Rest.Service
{
    /// <summary>
    /// ToyaMobileService - api endpoints declaration
    /// </summary>
    internal interface IMobileService
    {
        [Post("/api/Account/Login")]
        [Headers("Content-Type: application/json")]
        IObservable<LoginResult> PostLogin(
            [Body(BodySerializationMethod.Serialized)] LoginModel loginModel);

        [Post("/api/Account/ForgotPassword")]
        [Headers("Content-Type: application/json")]
        IObservable<ForgotPasswordResult> PostForgotPassword(
            [Body(BodySerializationMethod.Serialized)] ForgotPasswordModel forgotPasswordModel);

        [Post("/api/Account/Refresh")]
        [Headers("Content-Type: application/json")]
        IObservable<TokenModel> PostRefresh(
            [Body(BodySerializationMethod.Serialized)] TokenModel tokenModel);

        [Post("/api/Products/list")]
        [Headers("Content-Type: application/json")]
        IObservable<IList<ProductModel>> PostProductList(
            [Body(BodySerializationMethod.Serialized)] ProductQueryModel productQueryModel,
            [Header("Authorization")] string authorization);
    }
}
