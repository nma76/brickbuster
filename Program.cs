using System;
using System.Linq;

internal static class Program
{
    public static void Main(string[] args)
    {
        bool editorMode = args.Contains("--editor");

        using var game = new brickbuster.Game1();
        game.Run();
    }
}