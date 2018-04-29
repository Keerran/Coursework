[System.Serializable]
public class Game
{
    // Store the current scene.
    public static Game current;
    // Serialize all the particles in the scene.
    public Particle[] particles;
    // Serialize all the planes in the scene.
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