using System;
using System.IO;
using System.Linq;
using AnorocMobileApp;
using NUnit.Framework;
using Xamarin.UITest;
using Xamarin.UITest.Queries;

namespace AnorocMobileAppTests.View
{
    [TestFixture(Platform.Android)]
    //[TestFixture(Platform.iOS)]
    public class SettingsViewTest
    {
        IApp app;
        Platform platform;

        public SettingsViewTest(Platform platform)
        {
            this.platform = platform;
        }

        [SetUp]
        public void BeforeEachTest()
        {
            app = AppInitializer.StartApp(platform);
        }
/*
        [Test]
        public void ShouldBeAbleToToggleLocation()
        {
            //Arrange

            //app.Tap("MeButton");

            app.Tap("UserNameBox");
            app.EnterText("kevin@anoroc.co.za");
            app.DismissKeyboard();
            app.Tap("PasswordBox");
            app.EnterText("AnorocUITest");
            app.DismissKeyboard();

            //Act
            app.Tap("LoginButton");

            Console.WriteLine(App.ScreenWidth);

            app.WaitForElement("HomeLabel");
            //MePage Coordinates
            app.TapCoordinates(1000, 1710);

            var switchValue = app.Query(c => c.Marked("Location_Tracking_Switch").Invoke("isChecked").Value<bool>()).First();

        *//*    AppResult[] results = app.Query(c => c.All());
            for(int i=0;i<results.Length;i++)
            {
                Console.WriteLine(results[i].Text);
            }
*//*
            Assert.IsTrue(switchValue);


        }*/



        public void OpenRepl()
        {

            app.Repl();

        }



    }
}
