using System.Linq;
using System.Web.Script.Serialization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NMock;
using WcfBookServiceLibrary;
using WcfBookServiceLibrary.Book;
using WcfBookServiceLibrary.Services;

namespace WcfBookServiceLibraryTest.Services
{
    [TestClass]
    public class BookSearchTest
    {
        private const string GoodIsbn = "0955683645";
        private const string WrongIsbn = "666";

        [TestMethod]
        public void ReturnsJsonResponseWithBookInfoWhenIsbnIsGood()
        {
            string bookTitle = "Impact mapping";
            string bookSubTitle = "Making a big impact with software products and projects";
            string bookAuthor = "Gojko Adzik";
            string bookDescription = "long description for Impact mapping book";
            string bookCover = "http://books.google.com/books/content?id=6tNoMwEACAAJ&printsec=frontcover&img=1&zoom=1&source=gbs_api";

            IBookInfo impactMappingBookInfo = BookBuilder.GetInstance()
                .WithTitle(bookTitle)
                .WithSubTitle(bookSubTitle)
                .WithAuthors(bookAuthor)
                .WithDescription(bookDescription)
                .WithCover(bookCover)
                .Build();

            MockFactory mockFactory = new MockFactory();
            Mock<IsbnSearchInvoker> isbnSearchInvokerMocked = mockFactory.CreateMock<IsbnSearchInvoker>();
            isbnSearchInvokerMocked.Expects.One.Method(invoker => invoker.GetBookInfo(null))
                .With(GoodIsbn)
                .WillReturn(impactMappingBookInfo);

            BookSearchService bookSearchService = new BookSearchService(isbnSearchInvokerMocked.MockObject);

            string jsonBookInfo = bookSearchService.GetBookInfo(GoodIsbn);

            JavaScriptSerializer jsSerializer = new JavaScriptSerializer();
            IBookInfo bookInfo = jsSerializer.Deserialize<BookInfo>(jsonBookInfo);

            Assert.AreEqual(bookTitle, bookInfo.Title);
            Assert.AreEqual(bookSubTitle, bookInfo.SubTitle);
            Assert.AreEqual(1, bookInfo.Authors.Count());
            Assert.AreEqual(bookAuthor, bookInfo.Authors.ElementAt(0));
            Assert.AreEqual(bookDescription, bookInfo.Description);
            Assert.AreEqual(bookCover, bookInfo.Cover);
        }

        [TestMethod]
        public void ReturnsNullWhenIsbnIsNotFound()
        {
            IBookInfo bookInfo = null;

            MockFactory mockFactory = new MockFactory();
            Mock<IsbnSearchInvoker> isbnSearchInvokerMocked = mockFactory.CreateMock<IsbnSearchInvoker>();
            isbnSearchInvokerMocked.Expects.One.Method(invoker => invoker.GetBookInfo(null))
                .With(WrongIsbn)
                .WillReturn(bookInfo);

            BookSearchService bookSearchService = new BookSearchService(isbnSearchInvokerMocked.MockObject);

            string jsonBookInfo = bookSearchService.GetBookInfo(WrongIsbn);

            Assert.IsNull(jsonBookInfo);
        }
    }
}
