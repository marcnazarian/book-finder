using System;
using System.Web.Script.Serialization;
using System.Windows.Forms;
using WcfBookServiceLibrary.Book;
using WcfBookServiceLibrary.GoogleApi;
using WcfBookServiceLibrary.Services;

namespace BookFinder
{
    public partial class BookFinderForm : Form
    {
        public BookFinderForm()
        {
            InitializeComponent();
        }

        private void searchIsbnButton_Click(object sender, EventArgs e)
        {
            BookSearchService bookSearchService = new BookSearchService(new GoogleApiIsbnSearchInvoker());
            string jsonBookInfo = bookSearchService.GetBookInfo(textBoxIsbn.Text);

            if (jsonBookInfo == null)
            {
                string errorMessage = String.Format("No book found with ISBN '{0}'. Please try another ISBN.", textBoxIsbn.Text);
                MessageBox.Show(errorMessage);
            }
            else
            {
                JavaScriptSerializer jsSerializer = new JavaScriptSerializer();
                IBookInfo bookInfo = jsSerializer.Deserialize<BookInfo>(jsonBookInfo);

                labelTitle.Text = bookInfo.Title;
                labelSubTitle.Text = bookInfo.SubTitle;
                labelDescription.Text = bookInfo.Description;
                
                labelAuthors.Text = String.Join(", ", bookInfo.Authors);
            
                if (! string.IsNullOrWhiteSpace(bookInfo.Cover))
                {
                    pictureBoxCover.LoadAsync(bookInfo.Cover);
                }

            }
        }
    }
}
