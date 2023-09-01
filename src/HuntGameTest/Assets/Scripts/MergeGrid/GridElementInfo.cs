namespace MergeGrid
{
    public readonly struct GridElementInfo
    {
        public GridElementInfo(int x, int y, int level)
        {
            X = x;
            Y = y;
            Level = level;
        }

        public int X { get; }
        public int Y { get; }
        public int Level { get; }
    }
}