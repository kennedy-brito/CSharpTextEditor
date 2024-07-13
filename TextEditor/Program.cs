
// Constantes para os modos de entrada e saída do console
using System.Runtime.InteropServices;
using TextEditor;

partial class Program
{
    
    static void Main(string[] args)
    {

        string fileName = args[0] ?? "";
        Console.WriteLine(args);
        Console.ReadLine();
        Editor editor;
        if(fileName.Length < 1)
        {
            editor = new Editor();
        }
        else
        {
            editor = new Editor(fileName);
        }

        editor.Run();
    }
}
