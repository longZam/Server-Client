using FixedMath.NET;

namespace ECS.Physics;


public struct AABB : IComponent
{
    public Vector2 center;
    public Vector2 size;
    
    public Vector2 Extents => size / 2;
    public Vector2 Min => center - Extents;
    public Vector2 Max => center + Extents;


    public AABB(Vector2 center, Vector2 size)
    {
        this.center = center;
        this.size = size;
    }

    /// <summary>
    /// b에 대한 a의 침투 벡터를 계산
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <param name="direction"></param>
    /// <param name="distance"></param>
    /// <returns></returns>
    public static bool ComputePenetration(AABB a, AABB b, out Vector2 direction, out Fix64 distance)
    {
        direction = Vector2.zero;
        distance = Fix64.Zero;

        // 겹침 여부 판정
        if (a.Max.x <= b.Min.x || a.Min.x >= b.Max.x) return false;
        if (a.Max.y <= b.Min.y || a.Min.y >= b.Max.y) return false;

        // x, y축에 각각 투영한 길이 재기
        Fix64 overlapX = Fix64Extension.Max(a.Max.x, b.Max.x) - Fix64Extension.Min(a.Min.x, b.Min.x);
        Fix64 overlapY = Fix64Extension.Max(a.Max.y, b.Max.y) - Fix64Extension.Min(a.Min.y, b.Min.y);
        // 교집합 구하기
        overlapX = a.size.x + b.size.x - overlapX;
        overlapY = a.size.y + b.size.y - overlapY;
        // 방향성 
        overlapX *= a.center.x < b.center.x ? -Fix64.One : Fix64.One;
        overlapY *= a.center.y < b.center.y ? -Fix64.One : Fix64.One;

        // out 인자 채우기
        Vector2 penetration;
        
        if (overlapX < overlapY)
            penetration = new Vector2(overlapX, Fix64.Zero);
        else
            penetration = new Vector2(Fix64.Zero, overlapY);

        if (penetration.Magnitude() == Fix64.Zero)
            return false;

        direction = penetration.Normalize();
        distance = penetration.Magnitude();

        return true;
    }
}