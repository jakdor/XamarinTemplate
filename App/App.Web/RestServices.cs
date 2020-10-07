using System;
using System.Collections.Generic;
using System.Net;
using System.Reactive.Linq;
using System.Security.Authentication;
using App.Web.Rest.Model.Auth;
using App.Web.Rest.Model.Product;
using App.Web.Rest.Network;
using App.Web.Rest.Service;
using Newtonsoft.Json;
using Refit;

namespace App.Web
{
    /// <summary>
    /// Provides Observable REST Service responses, and auth wrappers
    /// </summary>
    public class RestServices : IRestServices
    {

#if DEBUG
        private const string MobileServiceUrl = "http://91.237.118.41:7122/B2B/";
#else
        private const string MobileServiceUrl = "https://91.237.118.39:8080/b2b";
#endif

        private IMobileService _mobileService;
        private readonly IRefitFactory _refitFactory;

        /// <summary>
        /// Build IMobileService with default URL
        /// </summary>
        public RestServices(IRefitFactory refitFactory)
        {
            _refitFactory = refitFactory;
            SetUrl(MobileServiceUrl);
        }

        /// <summary>
        /// Builds new IMobileService instance with provided URL
        /// </summary>
        /// <param name="url">overrides default URL</param>
        public void SetUrl(string url)
        {
            _mobileService = _refitFactory.BuildService<IMobileService>(url);
        }

        /// <summary>
        /// Serializes token and formats authorization header
        /// </summary>
        /// <param name="login">user login</param>
        /// <param name="token">token</param>
        /// <returns>Authorization header str</returns>
        public string FormatAuthHeader(string login, string token)
        {
            var authHeader = "Bearer ";

            var tokenModel = new TokenModel(login, token);
            var tokenJson = JsonConvert.SerializeObject(tokenModel);
            var tokenJsonBytes = System.Text.Encoding.UTF8.GetBytes(tokenJson);
            var tokenJsonBase64 = Convert.ToBase64String(tokenJsonBytes);

            authHeader += tokenJsonBase64;

            return authHeader;
        }

        /// <summary>
        /// Generic method for wrapping api call requiring token auth that can return 401 status code.
        /// </summary>
        /// <typeparam name="T">Api response model type</typeparam>
        /// <param name="apiCallMethodFunc">Service call method delegate</param>
        /// <param name="unauthorizedFunc">Called on 401 status code</param>
        /// <param name="refreshTokenFunc">Try refresh token action</param>
        /// <param name="noNetwork">Called on network error</param>
        /// <param name="hasInternetService">false if no Internet</param>
        /// <returns></returns>
        public IObservable<T> CallWithAuthToken<T>(Func<IObservable<T>> apiCallMethodFunc, Action unauthorizedFunc,
            Func<bool> refreshTokenFunc, Action noNetwork, bool hasInternetService)
        {
            return Observable.Create<T>(observer =>
            {
                if (!hasInternetService)
                {
                    observer.OnError(new Exception("No Internet service"));
                    observer.OnCompleted();
                    return () => { };
                }

                try
                {
                    var result = apiCallMethodFunc().Wait();
                    observer.OnNext(result);
                }
                catch (Exception e)
                {
                    if (e.GetType() == typeof(ApiException) &&
                        ((ApiException) e).StatusCode == HttpStatusCode.Unauthorized)
                    {
                        if (refreshTokenFunc())
                        {
                            try
                            {
                                var result = apiCallMethodFunc().Wait();
                                observer.OnNext(result);
                            }
                            catch (Exception e2)
                            {
                                if (e2.GetType() == typeof(ApiException) &&
                                    ((ApiException) e2).StatusCode == HttpStatusCode.Unauthorized)
                                {
                                    observer.OnError(new AuthenticationException("Invalid bearer token"));
                                    unauthorizedFunc();
                                }
                                else
                                {
                                    observer.OnError(e);
                                }
                            }
                        }
                        else
                        {
                            observer.OnError(new AuthenticationException("Invalid refresh token"));
                            unauthorizedFunc();
                        }
                    }
                    else if (e.InnerException != null &&
                             e.InnerException.Message.Contains("ConnectFailure (Network is unreachable)"))
                    {
                        observer.OnError(new Exception("No Internet service"));
                        noNetwork();
                    }
                    else
                    {
                        observer.OnError(e);
                    }
                }

                observer.OnCompleted();
                return () => { };
            });
        }

        /// <summary>
        /// Login endpoint
        /// Makes a POST call to IMobileService - /api/Account/Login
        /// </summary>
        /// <param name="login">user login</param>
        /// <param name="password">user password</param>
        /// <returns>IObservable&lt;LoginResult&gt;</returns>
        public IObservable<LoginResult> PostLogin(string login, string password)
        {
            return _mobileService.PostLogin(new LoginModel(login, password));
        }

        /// <summary>
        /// Forgot password endpoint
        /// Makes a POST call to IMobileService - /api/Account/ForgotPassword
        /// </summary>
        /// <param name="login">user login</param>
        /// <param name="email">account registration email</param>
        /// <returns>IObservable&lt;ForgotPasswordResult&gt;</returns>
        public IObservable<ForgotPasswordResult> PostForgotPassword(string login, string email)
        {
            return _mobileService.PostForgotPassword(new ForgotPasswordModel(login, email));
        }

        /// <summary>
        /// Bearer token refresh endpoint
        /// Makes a POST call to IMobileService - /api/Account/Refresh
        /// </summary>
        /// <param name="login">user login</param>
        /// <param name="refreshToken">refresh token provided from login response</param>
        /// <returns>IObservable&lt;TokenModel&gt;</returns>
        public IObservable<TokenModel> PostRefresh(string login, string refreshToken)
        {
            return _mobileService.PostRefresh(new TokenModel(login, refreshToken));
        }

        /// <summary>
        /// Product list with query endpoint
        /// Makes a POST call to IMobileService - /api/Products/list
        /// </summary>
        /// <param name="productQuery">ProductQueryModel - defines product query parameters</param>
        /// <param name="login">user login</param>
        /// <param name="token">bearer token</param>
        /// <returns>IObservable&lt;IList&lt;ProductModel&gt;&gt;</returns>
        public IObservable<IList<ProductModel>> PostProductList(ProductQueryModel productQuery, string login,
            string token)
        {
            return _mobileService.PostProductList(productQuery, FormatAuthHeader(login, token));
        }

        /// <summary>
        /// Creates delegate of PostProductList call
        /// </summary>
        /// <param name="productQuery">ProductQueryModel - defines product query parameters</param>
        /// <param name="login">user login</param>
        /// <param name="getTokenFunc">get bearer token delegate</param>
        /// <returns>PostProductList delegate</returns>
        public Func<IObservable<IList<ProductModel>>> PostProductListFunc(
            ProductQueryModel productQuery, string login, Func<string> getTokenFunc)
        {
            IObservable<IList<ProductModel>> PostProductListDelegate() =>
                PostProductList(productQuery, login, getTokenFunc());

            return PostProductListDelegate;
        }
    }
}
