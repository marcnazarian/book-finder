using System.Linq;
using System.Web.Script.Serialization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WcfBookServiceLibrary;
using WcfBookServiceLibrary.Book;
using WcfBookServiceLibrary.GoogleApi;
using WcfBookServiceLibrary.Services;

namespace WcfBookServiceLibraryTest.GoogleApi
{
    [TestClass]
    public class GoogleApiBookSearchIntegrationTest
    {
        private const string ImpactMappingIsbn = "9780955683640";
        private const string PragmaticProgrammerIsbnWithHyphens = "978-0-13211-917-7";

        [TestMethod]
        public void ICanCallGoogleApiToSearchForABookWithIsbn()
        {
            IsbnSearchInvoker googleApiIsbnSearchInvoker = new GoogleApiIsbnSearchInvoker();
            BookSearchService bookSearchService = new BookSearchService(googleApiIsbnSearchInvoker);

            string jsonBookInfo = bookSearchService.GetBookInfo(ImpactMappingIsbn);

            JavaScriptSerializer jsSerializer = new JavaScriptSerializer();
            IBookInfo bookInfo = jsSerializer.Deserialize<BookInfo>(jsonBookInfo);

            Assert.AreEqual("Impact Mapping", bookInfo.Title);
            Assert.AreEqual("Making a Big Impact with Software Products and Projects", bookInfo.SubTitle);
            Assert.AreEqual(1, bookInfo.Authors.Count());
            Assert.AreEqual("Gojko Adzic", bookInfo.Authors.ElementAt(0));
            StringAssert.Contains(bookInfo.Description, "A practical guide to impact mapping, a simple yet incredibly effective method for");
            Assert.AreEqual("http://books.google.com/books/content?id=6tNoMwEACAAJ&printsec=frontcover&img=1&zoom=1&source=gbs_api", bookInfo.Cover);
        }

        [TestMethod]
        public void ICanCallGoogleApiWithIsbnContainingHyphens()
        {
            IsbnSearchInvoker googleApiIsbnSearchInvoker = new GoogleApiIsbnSearchInvoker();
            BookSearchService bookSearchService = new BookSearchService(googleApiIsbnSearchInvoker);

            string jsonBookInfo = bookSearchService.GetBookInfo(PragmaticProgrammerIsbnWithHyphens);

            JavaScriptSerializer jsSerializer = new JavaScriptSerializer();
            IBookInfo bookInfo = jsSerializer.Deserialize<BookInfo>(jsonBookInfo);

            Assert.AreEqual("The Pragmatic Programmer", bookInfo.Title);
            Assert.AreEqual(2, bookInfo.Authors.Count());
            Assert.AreEqual("Andrew Hunt", bookInfo.Authors.ElementAt(0));
            Assert.AreEqual("David Thomas", bookInfo.Authors.ElementAt(1));
        }

    }
}
