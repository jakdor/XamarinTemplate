using System;
using Android.App;
using App.Android.DI;
using AndroidBase = Android;

namespace App.Android
{
    [Application]
    public class App : Application
    {
        internal static DependencyContainer DiContainer { get; set; } = new DependencyContainer();

        public App(IntPtr javaReference, AndroidBase.Runtime.JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

        public override void OnCreate()
        {
            base.OnCreate();
            DiContainer.Init();
            DiContainer.RegisterAppContext(ApplicationContext);
            DiContainer.Build();
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            DiContainer.Dispose();
        }
    }
}