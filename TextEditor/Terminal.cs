using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace TextEditor;
public class Terminal
{
    // Constantes para os modos de entrada e saída do console
    const uint ENABLE_PROCESSED_INPUT = 0x0001;         //00001
    const uint ENABLE_LINE_INPUT = 0x0002;         //00010
    const uint ENABLE_ECHO_INPUT = 0x0004;         //00100
    const uint ENABLE_WINDOW_INPUT = 0x0008;         //01000
    const uint ENABLE_MOUSE_INPUT = 0x0010;         //10000

    // Importa a função GetConsoleMode da API do Windows
    [DllImport("kernel32.dll", SetLastError = true)]
    static extern bool GetConsoleMode(IntPtr hConsoleHandle, out uint lpMode);

    // Importa a função SetConsoleMode da API do Windows
    [DllImport("kernel32.dll", SetLastError = true)]
    static extern bool SetConsoleMode(IntPtr hConsoleHandle, uint dwMode);

    // Importa a função GetStdHandle da API do Windows
    [DllImport("kernel32.dll", SetLastError = true)]
    static extern IntPtr GetStdHandle(int nStdHandle);

    // Constantes para os identificadores de entrada e saída padrão do console
    const int STD_INPUT_HANDLE = -10;
    const int STD_OUTPUT_HANDLE = -11;

    public static Size Size
    {
        get => new Size(Console.WindowWidth, Console.WindowHeight);
    }

    public static int Lines
    {
        get => Size.Height;
    }
    public static int Columns
    {
        get => Size.Width;
    }
    public static void Initialize()
    {
        EnableRawMode();
        ClearScreen();
        MoveCursorTo(0, 0);
    }

    public static void Terminate() => DisableRawMode();

    public static void MoveCursorTo(int left, int top)
    {
        if(left < 0 || top> Console.WindowHeight)
        {
            throw new ArgumentException("Invalid position of the cursor");
        }
        Console.SetCursorPosition(left, top);
    }

    public static void ClearLine()
    {
        Console.SetCursorPosition(0, Console.CursorTop);
        Console.Write(new string(' ', Console.WindowWidth));
        Console.SetCursorPosition(0, Console.CursorTop);
    }
    public static void HideCursor() => Console.CursorVisible = false;

    public static void ShowCursor() => Console.CursorVisible = true;


    public static void Print(string text) => Console.Write(text);
    public static void Print(char text) => Console.Write(text);

    public static Size GetSize() => new Size(
        width: (ushort)Console.WindowWidth, 
        height: (ushort)Console.WindowHeight);
    public static void ClearScreen() => Console.Clear();
    

    private static void EnableRawMode()
    {
        // Obtém o handle de entrada padrão do console
        IntPtr consoleHandle = GetStdHandle(STD_INPUT_HANDLE);
        if (consoleHandle == IntPtr.Zero)
        {
            throw new Exception("A error ocurred while getting the standard input");
        }

        // Obtém o modo do console
        if (!GetConsoleMode(consoleHandle, out uint mode))
        {
            throw new Exception("A error ocurred while getting the console mode");
        }
        mode &= ~(ENABLE_PROCESSED_INPUT | ENABLE_LINE_INPUT | ENABLE_ECHO_INPUT);
        /*
            * basicamente teremos que:
            * A: (ENABLE_PROCESSED_INPUT | ENABLE_LINE_INPUT | ENABLE_ECHO_INPUT) = 000000111
            * B: mode                                                             = 111100111   
            * 
            * Ao negar A teremos então
            * ~A = 111111000
            * 
            * Fazendo bitwise AND de B com ~A, ou seja, B & ~A
            * TEREMOS> (111100111) & (111111000)  
            * O que significa que as únicas casas de bytes 'ligadas'(com 1) serão as que estiverem
            * 'ligadas'em B e desligadas em A
            *  B & ~A = 111100000
            *
            * Isso significa que desativamos os valores que ativam o Console no modo Cooked
            */

        mode |= (ENABLE_PROCESSED_INPUT | ENABLE_LINE_INPUT | ENABLE_ECHO_INPUT);

        // Define o novo modo do console
        if (!SetConsoleMode(consoleHandle, mode))
        {
            throw new Exception("A error ocurred while setting the console mode");
        }
    }

    private static void DisableRawMode()
    {
        // Obtém o handle de entrada padrão do console
        IntPtr consoleHandle = GetStdHandle(STD_INPUT_HANDLE);
        if (consoleHandle == IntPtr.Zero)
        {
            throw new Exception("A error ocurred while getting the standard input");
        }

        // Obtém o modo do console
        if (!GetConsoleMode(consoleHandle, out uint mode))
        {
            throw new Exception("A error ocurred while getting the console mode");
        }

        mode |= (ENABLE_PROCESSED_INPUT | ENABLE_LINE_INPUT | ENABLE_ECHO_INPUT);

        // Define o novo modo do console
        if (!SetConsoleMode(consoleHandle, mode))
        {
            throw new Exception("A error ocurred while setting the console mode");
        }
    }
}

public record struct  Position
{
    public ushort X { get; set; }
    public ushort  Y { get; set; }

    public Position(ushort x, ushort y)
    {
        X = x; 
        Y = y;
    }

    
}