using System.Buffers;
using FixedMath.NET;
using System.Collections.Concurrent;

namespace ECS.Physics;


public class AABBSeparateSystem : System
{
    private readonly static ChunkArray<AABB> buffer = new ChunkArray<AABB>(1024);


    public override void Tick(World world, Fix64 deltaTime)
    {
        int count = 0;

        world.ForEach((int index, ref AABB aabb) =>
        {
            lock (buffer)
            {
                count += 1;
                buffer[index] = aabb;
            }
        });

        world.ForEach((int index, ref AABB aabb) =>
        {
            for (int i = 0; i < count; i++)
            {
                if (i == index)
                    continue;

                if (AABB.ComputePenetration(aabb, buffer[i], out Vector2 direction, out Fix64 distance))
                {
                    aabb.center += direction * distance / 2;
                }
            }
        });
    }
}