using FixedMath.NET;

namespace ECS;



public class Systems
{
    private readonly List<System> systems;

    public Systems()
    {
        systems = new List<System>();
    }

    public void Tick(World world, Fix64 deltaTime)
    {
        systems.ForEach(system => system.Tick(world, deltaTime));
    }

    public void RegisterSystem<T>(T system) where T : System
    {
        this.systems.Add(system);
    }
}