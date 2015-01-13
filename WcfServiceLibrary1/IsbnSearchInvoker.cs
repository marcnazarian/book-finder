namespace WcfBookServiceLibrary
{

    public interface IsbnSearchInvoker
    {
        IBookInfo GetBookInfo(string isbn);
    }

}
