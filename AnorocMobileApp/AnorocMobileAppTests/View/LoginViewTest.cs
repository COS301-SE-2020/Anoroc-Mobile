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
    public class Tests
    {
        IApp app;
        Platform platform;

        public Tests(Platform platform)
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
            //Arrange

            app.Tap("UserNameBox");
            app.EnterText("kevin");
            app.DismissKeyboard();
            app.Tap("PasswordBox");
            app.EnterText("huang");
            app.DismissKeyboard();

            //Act
            app.Tap("LoginButton");
            app.WaitForElement("WelcomeBox");

            //Assert
            bool result = true;

            Assert.IsTrue(result);


        }
        
        public void OpenRepl()
        {

            app.Repl();

        }



    }
}
