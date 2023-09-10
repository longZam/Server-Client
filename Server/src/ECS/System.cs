using FixedMath.NET;

namespace ECS;

public abstract class System
{
    public abstract void Tick(World world, Fix64 deltaTime);
}