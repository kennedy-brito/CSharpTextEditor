
using System.Globalization;

namespace TextEditor;
public partial class Editor
{
    private bool _shouldQuit = false;
    private readonly char _columnChar = '~';
    public Editor() 
    {
        Terminal.Initialize();
    }


    public void Run()
    {

        while (!_shouldQuit)
        {
            // Holds all the information of the pressed key
            try
            {
                ConsoleKeyInfo input = Console.ReadKey(intercept: true);
                Refresh();
                
                HandlePressedKey(input);

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

    private void HandlePressedKey(ConsoleKeyInfo input)
    {
        ConsoleKey key = input.Key;

        ConsoleModifiers modifiers = input.Modifiers;

        switch (modifiers, key)
        {
            case (ConsoleModifiers.Control, ConsoleKey.Q):
                Quit();
                break;
            case (_, ConsoleKey.LeftArrow):
                MoveCaretToLeft();
                break;
            case (_, ConsoleKey.RightArrow):
                MoveCaretToRight();
                break;
            case (_, ConsoleKey.UpArrow):
                MoveCaretToPreviousLine();
                break;
            case (_, ConsoleKey.DownArrow):
                MoveCaretToNextLine();
                break;
            case (_, ConsoleKey.Home):
                MoveCaretToStart();
                break;
            case (_, ConsoleKey.End):
                MoveCaretToEnd();
                break;
            case (_, ConsoleKey.PageUp):
                MoveToPreviousPage();
                break;
            case (_, ConsoleKey.PageDown):
                MoveToNextPage();
                break;
            default:
                Terminal.Print(input.KeyChar);
                break;

        }
    }

    private  void MoveCaretToEnd()
    {
        Terminal.MoveCursorTo(Terminal.Columns - 1, Terminal.Lines - 1);
    }

    private void MoveCaretToStart()
    {
        Terminal.MoveCursorTo(2, 0);
    }

    private void MoveToNextPage()
    {
        int nextPagePosition = Console.CursorTop + Terminal.Lines - 1;

        if(nextPagePosition > Terminal.Lines - 1) { nextPagePosition = Terminal.Lines - 1; }

        MoveCaretToLine(nextPagePosition);
    }

    private void MoveToPreviousPage()
    {
        int previousPageLine = Console.CursorLeft - Terminal.Lines - 1;

        if(previousPageLine < 0) { previousPageLine = 0; }

        MoveCaretToLine(previousPageLine);
    }

    private void MoveCaretToNextLine()
    {
    
        int futureLinePosition = Console.CursorTop + 1;

        MoveCaretToLine(futureLinePosition);
    }

    private void MoveCaretToPreviousLine()
    {
        int futureLinePosition = Console.CursorTop - 1;

        MoveCaretToLine(futureLinePosition);
    }


    private void MoveCaretToRight()
    {
        int futureRightPosition = Console.CursorLeft + 1;
        if (futureRightPosition >= Terminal.Columns) return;

        Terminal.MoveCursorTo(futureRightPosition, Console.CursorTop);
    }
    private void MoveCaretToLine(int futureLinePosition)
    {
        if(futureLinePosition < 0) 
        {
            return;
        }
        Terminal.MoveCursorTo(Console.CursorLeft, futureLinePosition);
    }

    private void MoveCaretToLeft()
    {
        int futureLeftPosition = Console.CursorLeft - 1;
        if (futureLeftPosition < 2) return;

        Terminal.MoveCursorTo(futureLeftPosition, Console.CursorTop);
    }

    private void Quit()
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
