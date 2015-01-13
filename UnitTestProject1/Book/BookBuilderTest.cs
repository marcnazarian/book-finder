using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WcfBookServiceLibrary.Book;

namespace WcfBookServiceLibraryTest.Book
{
    [TestClass]
    public class BookBuilderTest
    {
        [TestMethod]
        public void ICanBuildAnEmptyBook()
        {
            IBookInfo bookInfo = BookBuilder.GetInstance().Build();

            Assert.IsNotNull(bookInfo);

            Assert.IsNull(bookInfo.Title);
            Assert.IsNull(bookInfo.SubTitle);
            Assert.IsNull(bookInfo.Authors);
            Assert.IsNull(bookInfo.Description);
            Assert.IsNull(bookInfo.Cover);
        }

        [TestMethod]
        public void ICanBuildABookWithAllFields()
        {
            IBookInfo bookInfo = BookBuilder.GetInstance()
                .WithTitle("my book title")
                .WithSubTitle("my book subtitle")
                .WithAuthors("author 1", "author 2", "author 3")
                .WithDescription("my book description")
                .WithCover("my book cover")
                .Build();

            Assert.IsNotNull(bookInfo);

            Assert.AreEqual("my book title", bookInfo.Title);
            Assert.AreEqual("my book subtitle", bookInfo.SubTitle);
            Assert.AreEqual("my book description", bookInfo.Description);
            Assert.AreEqual("my book cover", bookInfo.Cover);

            Assert.AreEqual(3, bookInfo.Authors.Count());
            string author1 = bookInfo.Authors.ElementAt(0);
            string author2 = bookInfo.Authors.ElementAt(1);
            string author3 = bookInfo.Authors.ElementAt(2);

            Assert.AreEqual("author 1", author1);
            Assert.AreEqual("author 2", author2);
            Assert.AreEqual("author 3", author3);
        }
    }
}
