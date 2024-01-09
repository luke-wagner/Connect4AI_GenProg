partial class C4SmartAI : Connect4 {
    private static readonly SortedDictionary<int, int> _tuning = new SortedDictionary<int, int>() {
        { 1, -198 }, // borders friendly neighbor
        { 2, -23 }, // borders more than one friendly neighbor
        { 3, 414 }, // borders opponent neighbor
        { 4, 393 }, // borders more than one opponent neighbor
        { 5, 343 }, // continues run of two
        { 6, 978 }, // continues run of three or more
        { 7, 373 }, // stops run of three
        { 8, 972 }, // stops run of four or more
    };
}