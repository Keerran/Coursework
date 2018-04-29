[System.Serializable]
public class Game
{

    public static Game current;
    public Particle[] particles;
    public PlaneObj[] planes;
    
    public Game()
    {
        
    }

    public Game(Particle[] particles, PlaneObj[] planes)
    {
        this.particles = particles;
        this.planes = planes;
    }
}