
namespace TextEditor;
public partial class Editor
{
    private bool _shouldQuit = false;
    private readonly char _columnChar = '~';
    private TextBuffer _buffer;
    public Editor() 
    {
        Terminal.Initialize();
        Terminal.OnCtrlQPressed += Quit;
        _buffer = new TextBuffer();
        ReadBuffer();
    }

    public Editor(string fileName)
    {
        _buffer = new TextBuffer(fileName);
        Terminal.Initialize();
        Terminal.OnCtrlQPressed += Quit;
        ReadBuffer();
    }

    private void ReadBuffer()
    {
        for (int line = 0; line < _buffer.Text.Count; line++)
        {
            Terminal.MoveCursorTo(2, line);
            Terminal.Print(_buffer.Text[line].ToString());
        }
    }

    public void Run()
    {

        while (!_shouldQuit)
        {
            // Holds all the information of the pressed key
            try
            {
                Refresh();
                ConsoleKeyInfo input = Console.ReadKey(intercept: true);
                Terminal.HandlePressedKey(input);

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                break;
            }
            finally
            {
                Terminal.Terminate();
            }



        }
    }

    private void Refresh()
    {
        Terminal.HideCursor();
        int left = Console.CursorLeft<2? 2:Console.CursorLeft;
        int top = Console.CursorTop;
        DrawRows();
        Terminal.ShowCursor();   
        Terminal.MoveCursorTo(left, top);
    }

    private void DrawRows()
    {
        int windowLines= Terminal.Lines;
        for(int currentLine = 0; currentLine < windowLines; currentLine++)
        { 
           Terminal.MoveCursorTo(0, currentLine);
           Terminal.Print(_columnChar);
            
            if(currentLine == windowLines/3) 
            {
                DrawWelcomeMessage();
            }

        }

    }

    

    private void Quit(object sender, EventArgs e)
    {
        _shouldQuit = true;

        Terminal.ClearScreen();
        Terminal.Print("Goodbye!");
    }
    private void DrawWelcomeMessage()
    {
        const string version = "0.6"; 
        const string message = $"Editor hecto --version {version}";

        int screenWidth = Terminal.GetSize().Width;

        int messageInitialColumn = screenWidth/2 - message.Length/2;

        Terminal.Print($"{new string(' ', messageInitialColumn - 1)}{message}");

    }

}
