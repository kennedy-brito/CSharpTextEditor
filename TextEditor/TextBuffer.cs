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
    public TextBuffer()
    {
        Text = new List<StringBuilder>();
        Text.Add(new StringBuilder("Hello, World!"));
    }

    

}