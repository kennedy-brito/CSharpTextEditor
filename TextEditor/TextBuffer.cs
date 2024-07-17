using System.Text;

namespace TextEditor;

internal class TextBuffer
{
    /// <summary>
    /// Text is the text the document has
    /// Each position of the list of string builders is a line of the Document
    /// A new line means a new element of the list
    /// </summary>
    public List<StringBuilder> Text;
    public StreamReader TextReader;
    readonly private string _filePath;
    public bool IsEmpty { get => Text.Count < 1; }
    public TextBuffer()
    {
        Text = new List<StringBuilder>();
    } 
    public TextBuffer(string filePath)
    {
        Text = new List<StringBuilder>();
        _filePath = filePath;
        ReadText();

    }

    public void ReadText() 
    {
        try
        {
            foreach(var line in File.ReadLines(_filePath))
            {
                Text.Add(new StringBuilder(line));
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
            throw;
        }
    }

    

}