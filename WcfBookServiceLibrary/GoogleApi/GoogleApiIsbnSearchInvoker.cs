using System.IO;
using System.Net;
using System.Web.Script.Serialization;
using WcfBookServiceLibrary.Book;

namespace WcfBookServiceLibrary.GoogleApi
{
    public class GoogleApiIsbnSearchInvoker : IsbnSearchInvoker
    {
        private const string GoogleApiBookV1IsbnSearchBaseUrl = "https://www.googleapis.com/books/v1/volumes?q=isbn:{0}";

        public IBookInfo GetBookInfo(string isbn)
        {

            string googleApiBookIsbnSearchUrl = string.Format(GoogleApiBookV1IsbnSearchBaseUrl, isbn);
            
            HttpWebRequest request = WebRequest.Create(googleApiBookIsbnSearchUrl) as HttpWebRequest;

            using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
            {
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    StreamReader reader = new StreamReader(response.GetResponseStream());
                    string jsonResponse = reader.ReadToEnd();

                    JavaScriptSerializer jsSerializer = new JavaScriptSerializer();
                    GoogleApiBookWrapper googleApiBookWrapper = jsSerializer.Deserialize<GoogleApiBookWrapper>(jsonResponse);
                    
                    return googleApiBookWrapper.GetBookInfo;
                }

                return null;
            }
            
        }
    }
}
