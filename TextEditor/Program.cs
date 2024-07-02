
// Constantes para os modos de entrada e saída do console
using System.Runtime.InteropServices;

internal class Program
{
    // Constantes para os modos de entrada e saída do console
    const uint ENABLE_PROCESSED_INPUT = 0x0001;         //00001
    const uint ENABLE_LINE_INPUT =      0x0002;         //00010
    const uint ENABLE_ECHO_INPUT =      0x0004;         //00100
    const uint ENABLE_WINDOW_INPUT =    0x0008;         //01000
    const uint ENABLE_MOUSE_INPUT =     0x0010;         //10000

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

    static void Main()
    {
        // Obtém o handle de entrada padrão do console
        IntPtr consoleHandle = GetStdHandle(STD_INPUT_HANDLE);
        if (consoleHandle == IntPtr.Zero)
        {
            Console.WriteLine("Erro ao obter o handle do console.");
            return;
        }

        // Obtém o modo do console
        if (!GetConsoleMode(consoleHandle, out uint mode))
        {
            Console.WriteLine("Erro ao obter o modo do console.");
            return;
        }
        // Remove as flags de entrada processada, entrada de linha e eco de entrada para ativar o modo raw
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

        // Define o novo modo do console
        if (!SetConsoleMode(consoleHandle, mode))
        {
            Console.WriteLine("Erro ao definir o modo do console.");
            return;
        }

        // Lê a entrada do usuário para demonstrar o modo raw
        while (true)
        {
            int input = Console.Read();
            char c = (char)input;

            Console.Write($"Binary: {input:B} ASCII: {input} ");

            if( !Char.IsControl(c))
            {
                Console.Write($"Character {c}");
            }

            Console.WriteLine("");
            if (c =='q') break; 
        }
    }
}
