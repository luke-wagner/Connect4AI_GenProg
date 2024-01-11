internal class Program
{
    private static void Main(string[] args)
    {
        C4AITrainer AI_1 = new C4AITrainer(true);
        C4AITrainer AI_2 = new C4AITrainer(false);

        AI_2.Play();

        //C4AITrainer? winner = AIDueler.PlayMatch(ref AI_1, ref AI_2, true);
        //if (winner != null){
        //    Console.WriteLine(winner.ToString());
        //}

        /*
        Connect4 baseGame = new Connect4();
        C4SmartAI gameWithSmartAI = new C4SmartAI();

        //baseGame.Play();
        gameWithSmartAI.Play();
        */
    }
}