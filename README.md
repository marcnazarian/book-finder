# book-finder

C# application that retrieve and display book info regarding ISBN:
 * book title and subtitle
 * authors
 * description
 * image cover if available
 

Visual Studio Solution contains 3 projects:
 * **BookFinder**: the WPF application (startup project)
 * **WcfBookServiceLibrary**: a web service that returns a IBookInfo (in json format). For now, it calls GoogleBookApi WS but is designed to call any other web service (just need to write dedicated invoker / wrapper)
 * **WcfBookServiceLibraryTest**: tests for WcfBookLibrary (mainly unit tests, 1 or 2 integration tests do call the Google web service to ensure non regression of wrapper/invoker)

