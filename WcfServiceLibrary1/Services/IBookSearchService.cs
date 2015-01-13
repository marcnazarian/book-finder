using System.ServiceModel;

namespace WcfBookServiceLibrary
{
    [ServiceContract]
    public interface IBookSearchService
    {
        [OperationContract]
        string GetBookInfo(string isbn);

    }
    
}
