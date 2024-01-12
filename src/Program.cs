internal class Program
{
    private static void Main(string[] args)
    {
        Connect4 baseGame = new Connect4();
        C4SmartAI gameWithSmartAI = new C4SmartAI();

        //baseGame.Play();
        gameWithSmartAI.Play();
    }
}