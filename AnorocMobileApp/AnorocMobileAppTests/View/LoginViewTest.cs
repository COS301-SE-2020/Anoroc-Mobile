using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.UITest;

namespace AnorocMobileAppTests.View
{
    [TestFixture(Platform.Android)]
    //[TestFixture(Platform.iOS)]
    public class LoginViewTest
    {
        IApp app;
        Platform platform;

        public LoginViewTest(Platform platform)
        {
            this.platform = platform;
        }

        [SetUp]
        public void BeforeEachTest()
        {
            app = AppInitializer.StartApp(platform);
        }

        [Test]
        public void ShouldBeAbleTologin()
        {
            app.Tap("LoginButton");
        }

        public void OpenRepl()
        {

            app.Repl();

        }



    }
}
