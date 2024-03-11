using System;

namespace meuJogo
{
    class Program
    {
    
        static void Main(string[] args)
        {
            using (Game game = new Game(800, 600, "LearningOpenTK"))
            {
            game.Run();
            }
        }
    }
}
