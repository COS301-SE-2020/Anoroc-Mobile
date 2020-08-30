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
        public class HomeViewTest
        {
            IApp app;
            Platform platform;


            public HomeViewTest(Platform platform)
            {
                this.platform = platform;
            }

            [SetUp]
            public void BeforeEachTest()
            {
                app = AppInitializer.StartApp(platform);
            }

            #region Helpers
            private void ScrollDown(string objectName)
            {
                bool scrool = true;
                if (!string.IsNullOrEmpty(objectName))
                {
                    scrool = !app.Query(e => e.Marked(objectName)).Any();
                }

                if (scrool)
                {
                    if (this.platform == Platform.iOS)
                    {
                        app.ScrollDown();

                    }
                    else
                    {
                        app.ScrollDown(strategy: ScrollStrategy.Gesture);
                    }
                }

            }
            private void ScrollUp(string objectName)
            {
                bool scrool = true;
                if (!string.IsNullOrEmpty(objectName))
                {
                    scrool = !app.Query(e => e.Marked(objectName)).Any();
                }

                if (scrool)
                {
                    if (this.platform == Platform.iOS)
                    {
                        app.ScrollUp();

                    }
                    else
                    {
                        app.ScrollUp(strategy: ScrollStrategy.Gesture);
                    }
                }

            }
            #endregion

            public static bool TestCase1;
            public static bool TestCase2;
            public static bool TestCase3;
            [Test]
            public void ShouldBeAbleToGoToMePage()
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
                app.TapCoordinates(540, 1710);

                AppResult[] results = app.Query(c => c.All());
                bool result = app.Query(c => c.Text("Anoroc Van Looi")).Any();

                Assert.IsTrue(result);

            }

            [Test]
            public void ShouldBeAbleToGoToEncounters()
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
                app.TapCoordinates(270, 1710);

                AppResult[] results = app.Query(c => c.All());
                bool result = app.Query(c => c.Text("MARK ALL AS READ")).Any(); 

                Assert.IsTrue(result);

            }


            [Test]
            public void ShouldBeAbleToGoToSettings()
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

                AppResult[] results = app.Query(c => c.All());

                bool result = app.Query(c => c.Text("Location Tracking")).Any();

                Assert.IsTrue(result);

            }


            [Test]
            public void ShouldBeAbleToGoToMapPage()
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
                app.TapCoordinates(810, 1710);

                bool result = true;

                Assert.IsTrue(result);

            }


            [Test]
            public void ShouldBeAbleToGoToAllPages()
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

                app.TapCoordinates(270, 1710);

                AppResult[] results = app.Query(c => c.All());
                bool result = app.Query(c => c.Text("MARK ALL AS READ")).Any();


                if (result)
                {
                    app.TapCoordinates(540, 1710);

                    results = app.Query(c => c.All());
                    result = app.Query(c => c.Text("Anoroc Van Looi")).Any();

                    if (result)
                    {

                        app.TapCoordinates(810, 1710);

                        result = true;


                        if (result)
                        {

                            app.TapCoordinates(1000, 1710);

                            results = app.Query(c => c.All());

                            result = app.Query(c => c.Text("Location Tracking")).Any();

                            Assert.IsTrue(result);

                        }
                        else
                        {
                            Assert.IsTrue(false);
                        }

                    }
                    else
                    {
                        Assert.IsTrue(false);
                    }

                }
                else
                {
                    Assert.IsTrue(false);
                }


            }



            public void OpenRepl()
            {

                app.Repl();

            }



        }
    

}
