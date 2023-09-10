using FixedMath.NET;

namespace ECS;


[Serializable]
public struct Transform : IComponent
{
    public Vector2 position;


    public Transform(Fix64 x, Fix64 y)
    {
        position = new Vector2(x, y);
    }

    public Transform() : this(Fix64.Zero, Fix64.Zero) { }
}