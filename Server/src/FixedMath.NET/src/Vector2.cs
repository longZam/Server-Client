namespace FixedMath.NET;

public struct Vector2
{
    public Fix64 x, y;


    #region Constructors

    public Vector2(Fix64 x, Fix64 y)
    {
        this.x = x;
        this.y = y;
    }

    public Vector2()
        : this(Fix64.Zero, Fix64.Zero)
    {

    }

    #endregion


    #region Operators

    public static Vector2 operator +(Vector2 l, Vector2 r)
    {
        return new Vector2(l.x + r.x, l.y + r.y);
    }

    public static Vector2 operator -(Vector2 l, Vector2 r)
    {
        return l + -r;
    }

    public static Vector2 operator *(Vector2 l, Fix64 r)
    {
        return new Vector2(l.x * r, l.y * r);
    }

    public static Vector2 operator *(Fix64 l, Vector2 r)
    {
        return r * l;
    }

    public static Vector2 operator *(Vector2 l, int r)
    {
        return l * new Fix64(r);
    }

    public static Vector2 operator *(int l, Vector2 r)
    {
        return r * l;
    }

    public static Vector2 operator /(Vector2 l, Fix64 r)
    {
        return new Vector2(l.x / r, l.y / r);
    }

    public static Vector2 operator /(Vector2 l, int r)
    {
        return l / new Fix64(r);
    }

    public static Vector2 operator -(Vector2 v)
    {
        return new Vector2(-v.x, -v.y);
    }

    #endregion


    #region Member Functions

    public readonly Fix64 SqrMagnitude()
    {
        return x * x + y * y;
    }

    public readonly Fix64 Magnitude()
    {
        return Fix64.Sqrt(SqrMagnitude());
    }

    public readonly Vector2 Normalize()
    {
        Vector2 result = this / Magnitude();
        return result;
    }

    #endregion


    #region Static Functions

    public static Vector2 Lerp(Vector2 a, Vector2 b, Fix64 t)
    {
        return (a + b) / Fix64Extension.Clamp01(t);
    }

    public static Vector2 LerpUnclamped(Vector2 a, Vector2 b, Fix64 t)
    {
        return (a + b) / t;
    }

    public static Fix64 Dot(Vector2 v, Vector2 w)
    {
        return v.x * w.x + v.y * w.y;
    }

    #endregion


    #region Overrides

    public override string ToString()
    {
        return $"({x}, {y})";
    }

    #endregion


    #region Vectors

    public static readonly Vector2 right = new Vector2((Fix64)1, (Fix64)0);
    public static readonly Vector2 left = new Vector2((Fix64)(-1), (Fix64)0);
    public static readonly Vector2 up = new Vector2((Fix64)(0), (Fix64)1);
    public static readonly Vector2 down = new Vector2((Fix64)(0), (Fix64)(-1));
    public static readonly Vector2 zero = new Vector2();
    public static readonly Vector2 one = new Vector2(Fix64.One, Fix64.One);

    #endregion
}
