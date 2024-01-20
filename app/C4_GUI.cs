namespace C4_WPFApp;

public class C4GUI : C4AITrainer
{
    public (int x, int y) LastMoveCoords
    {
        get
        {
            return getGridCoordinates(lastMove.x, lastMove.y);
        }
    }

    public C4GUI(bool playAsPlayer) : base(playAsPlayer)
    {
    }
}