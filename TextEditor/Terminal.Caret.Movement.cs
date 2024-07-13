using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextEditor;
public partial class Terminal
{
    public static event EventHandler OnCtrlQPressed;
    public static void HandlePressedKey(ConsoleKeyInfo input)
    {
        ConsoleKey key = input.Key;

        ConsoleModifiers modifiers = input.Modifiers;

        switch (modifiers, key)
        {
            case (ConsoleModifiers.Control, ConsoleKey.Q):
                RaiseOnCtrlQPressed();
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


    private static void MoveCaretToEnd()
    {
        Terminal.MoveCursorTo(Terminal.Columns - 1, Terminal.Lines - 1);
    }

    private static void MoveCaretToStart()
    {
        Terminal.MoveCursorTo(2, 0);
    }

    private static void MoveToNextPage()
    {
        int nextPagePosition = Console.CursorTop + Terminal.Lines - 1;

        if (nextPagePosition > Terminal.Lines - 1) { nextPagePosition = Terminal.Lines - 1; }

        MoveCaretToLine(nextPagePosition);
    }

    private static void MoveToPreviousPage()
    {
        int previousPageLine = Console.CursorLeft - Terminal.Lines - 1;

        if (previousPageLine < 0) { previousPageLine = 0; }

        MoveCaretToLine(previousPageLine);
    }

    private static void MoveCaretToNextLine()
    {

        int futureLinePosition = Console.CursorTop + 1;

        MoveCaretToLine(futureLinePosition);
    }

    private static void MoveCaretToPreviousLine()
    {
        int futureLinePosition = Console.CursorTop - 1;

        MoveCaretToLine(futureLinePosition);
    }


    private static void MoveCaretToRight()
    {
        int futureRightPosition = Console.CursorLeft + 1;
        if (futureRightPosition >= Terminal.Columns) return;

        Terminal.MoveCursorTo(futureRightPosition, Console.CursorTop);
    }
    private static void MoveCaretToLine(int futureLinePosition)
    {
        if (futureLinePosition < 0)
        {
            return;
        }
        Terminal.MoveCursorTo(Console.CursorLeft, futureLinePosition);
    }

    private static void MoveCaretToLeft()
    {
        int futureLeftPosition = Console.CursorLeft - 1;
        if (futureLeftPosition < 2) return;

        Terminal.MoveCursorTo(futureLeftPosition, Console.CursorTop);
    }
    private static void RaiseOnCtrlQPressed()
    {
        OnCtrlQPressed?.Invoke(new object(), System.EventArgs.Empty);
    }
}
