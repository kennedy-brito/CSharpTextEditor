
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
                Refresh();
                ConsoleKeyInfo input = Console.ReadKey(intercept: true);
                
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
        DrawRows();
        Terminal.ShowCursor();   
    }

    private void DrawRows()
    {
        int windowLines= Terminal.Lines;
        for(int currentLine = 0; currentLine < windowLines; currentLine++)
        { 
           Terminal.MoveCursorTo(new Position(0, (ushort)currentLine));

           Terminal.Print(_columnChar);
        }

        Terminal.MoveCursorTo(new Position(2, 0));

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
            default:
                Terminal.Print(input.KeyChar);
                break;

        }
    }

    private void Quit()
    {
        _shouldQuit = true;

        Terminal.ClearScreen();
        Terminal.Print("Goodbye!");
    }

}
