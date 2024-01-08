public interface IPlayable {
    // the entry point for all game code
    void Play();

    // should print out a message relating to the end of the game
    // (e.g. "Thanks for playing!")
    void Quit();
}