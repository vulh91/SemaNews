using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SemaNews.Test
{
    [TestClass]
    public class HtmlHandler_Test
    {
        [TestMethod]
        public void Test_NormalizeURL()
        {
            var url = "www.vietnamworks.com/my-career-center/my-resume/step-by-step/software-developer-2525001";
            var testURL1 = SemaNews.Utilities.HtmlHandler.NormalizeUrl(url + "#alskjdlj");
            Console.WriteLine(testURL1);
            Assert.AreEqual("http://" + url, testURL1);
        }
    }
}
