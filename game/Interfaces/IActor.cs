namespace test_roguelike.Interfaces
{
    public interface IActor
    {
        int Munition { get; set; }
        int Awareness { get; set; }
        int Degat { get; set; }
        double Accuracy { get; set; }
        int Health { get; set; }
        int MaxHealth { get; set; }
        string Name { get; set; }
        int Speed { get; set; }
    }
}