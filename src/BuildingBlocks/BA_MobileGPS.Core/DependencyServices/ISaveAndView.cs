using System.IO;

public interface ISaveAndView
{
    //Method to save document as a file and view the saved document
    void SaveAndView(string filename, string contentType, MemoryStream stream);
}