using System.Diagnostics;
using ECS.Physics;
using FixedMath.NET;

namespace ECS;

public class TransformUpSystem : System
{
    public override void Tick(World world, Fix64 deltaTime)
    {
        world.ForEach((int index, ref Transform transform, ref AABB aabb) =>
        {
            aabb.center += Vector2.left * deltaTime;
        });
    }
}