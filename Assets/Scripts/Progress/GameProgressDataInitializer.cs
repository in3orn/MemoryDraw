namespace Dev.Krk.MemoryDraw.Progress
{
    public class GameProgressDataInitializer : BinaryDataLoader<GameProgressData>
    {
        protected override GameProgressData CreateEmpty()
        {
            return new GameProgressData();
        }
    }
}
