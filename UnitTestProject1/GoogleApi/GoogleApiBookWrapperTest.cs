using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web.Script.Serialization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WcfBookServiceLibrary.Book;
using WcfBookServiceLibrary.GoogleApi;

namespace WcfBookServiceLibraryTest.GoogleApi
{
    [TestClass]
    public class GoogleApiBookWrapperTest
    {
        private const string FixturesPath = ".GoogleApi.fixtures.";

        [TestMethod]
        public void DeserializeJsonResponseAndBuildBookInfo()
        {
            string jsonBookFixture = "google-api-impact-mapping.json";
            CopyResource(jsonBookFixture);
            String jsonBookContent = File.ReadAllText(jsonBookFixture);

            JavaScriptSerializer jsSerializer = new JavaScriptSerializer();
            GoogleApiBookWrapper googleApiBookWrapper = jsSerializer.Deserialize<GoogleApiBookWrapper>(jsonBookContent);
            IBookInfo bookInfo = googleApiBookWrapper.GetBookInfo;

            Assert.AreEqual("Impact Mapping", bookInfo.Title);
            Assert.AreEqual("Making a Big Impact with Software Products and Projects", bookInfo.SubTitle);
            Assert.AreEqual(1, bookInfo.Authors.Count());
            Assert.AreEqual("Gojko Adzic", bookInfo.Authors.ElementAt(0));
            StringAssert.Contains(bookInfo.Description, "A practical guide to impact mapping, a simple yet incredibly effective method for");
            Assert.AreEqual("http://books.google.com/books/content?id=6tNoMwEACAAJ&printsec=frontcover&img=1&zoom=1&source=gbs_api", bookInfo.Cover);
        }

        [TestMethod]
        public void DeserializeJsonResponseAndReturnNullWhenNotFound()
        {
            string jsonBookFixture = "google-api-not-found.json";
            CopyResource(jsonBookFixture);
            String jsonBookNotFoundContent = File.ReadAllText(jsonBookFixture);

            JavaScriptSerializer jsSerializer = new JavaScriptSerializer();
            GoogleApiBookWrapper googleApiBookWrapper = jsSerializer.Deserialize<GoogleApiBookWrapper>(jsonBookNotFoundContent);
            IBookInfo bookInfo = googleApiBookWrapper.GetBookInfo;

            Assert.IsNull(bookInfo);
        }

        private static void CopyResource(string file)
        {
            var assembly = Assembly.GetExecutingAssembly();
            string resourceName = Assembly.GetExecutingAssembly().GetName().Name + FixturesPath + file;
            using (Stream resource = assembly.GetManifestResourceStream(resourceName))
            {
                if (resource == null)
                {
                    throw new ArgumentException("No such resource", "resourceName");
                }
                using (Stream output = File.OpenWrite(file))
                {
                    resource.CopyTo(output);
                }
            }
        }
    }
}
