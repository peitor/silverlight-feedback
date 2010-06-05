using Microsoft.VisualStudio.TestTools.UnitTesting;
using Silverlight_LOB_Demo_Application.FeedbackControl;

namespace Tests.Unit.Silverlight
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class UnitTest1
    {
        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void TestMethod1()
        {

            ConvertAndAssert(1120.ConvertToNiceTime(), "1 sec.");
        }

        [TestMethod]
        public void TestMethod2()
        {
            ConvertAndAssert(3600000.ConvertToNiceTime(), "60 min. 0 sec.");

        }

        [TestMethod]
        public void TestMethod3()
        {
            ConvertAndAssert(1456120.ConvertToNiceTime(), "24 min. 16 sec.");

        }

        [TestMethod]
        public void TestMethod4()
        {
            ConvertAndAssert(3700000.ConvertToNiceTime(), "1 h. 1 min. 40 sec.");

        }

        [TestMethod]
        public void TestMethod5()
        {
            ConvertAndAssert(153700000.ConvertToNiceTime(), "1 d. 18 h. 41 min. 40 sec.");

        }
        

        private static void ConvertAndAssert(string convertToNiceTime, string expected)
        {

            string result = convertToNiceTime;
            Assert.AreEqual(expected, result);
        }
    }
}
