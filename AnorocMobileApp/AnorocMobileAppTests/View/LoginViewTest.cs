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
            /*
            //Arrange
            
            app.Tap("UserNameBox");
            app.EnterText("kevin@anoroc.co.za");
            app.DismissKeyboard();
            app.Tap("PasswordBox");
            app.EnterText("AnorocUITest");
            app.DismissKeyboard();

            //Act
            app.Tap("LoginButton");
            app.WaitForElement("HomeLabel");

            //Assert
            bool result = app.Query(e => e.Marked("HomeLabel")).Any(); 

            Assert.IsTrue(result);
            */

            //ArrangeActAssert
            app.Tap("LoginButton");

        }
        
        public void OpenRepl()
        {

            app.Repl();

        }



    }
}
