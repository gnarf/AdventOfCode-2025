namespace AoC2025;

public class Point3D : IEquatable<Point3D>
{
    public int x { get; init; }
    public int y { get; init; }
    public int z { get; init; }

    public Point3D(int x, int y, int z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }

    public bool Equals(Point3D? other)
    {
        return other is not null && x == other.x && y == other.y && z == other.z;
    }

    public override bool Equals(object? obj)
    {
        if (obj is Point3D other) return Equals(other);
        return base.Equals(obj);
    }

    public override int GetHashCode()
    {
        return Tuple.Create(x, y, z).GetHashCode();
    }

    public static bool operator ==(Point3D a, Point3D b)
    {
        return a?.Equals(b) ?? b is null;
    }
    public static bool operator !=(Point3D? a, Point3D? b)
    {
        return !(a?.Equals(b) ?? b is null);
    }

    public static Point3D operator -(Point3D a, Point3D b)
    {
        return new Point3D(a.x - b.x, a.y - b.y, a.z - b.z);
    }
    public static Point3D operator +(Point3D a, Point3D b)
    {
        return new Point3D(a.x + b.x, a.y + b.y, a.z + b.z);
    }

    public static Point3D Sign(Point3D a)
    {
        return new Point3D(Math.Sign(a.x), Math.Sign(a.y), Math.Sign(a.z));
    }
    public static Point3D Abs(Point3D a)
    {
        return new Point3D(Math.Abs(a.x), Math.Abs(a.y), Math.Abs(a.z));
    }

    public static Point3D Min(Point3D a, Point3D b)
    {
        return new Point3D(Math.Min(a.x, b.x), Math.Min(a.y, b.y), Math.Min(a.z, b.z));
    }

    public static Point3D Max(Point3D a, Point3D b)
    {
        return new Point3D(Math.Max(a.x, b.x), Math.Max(a.y, b.y), Math.Max(a.z, b.z));
    }

    public int Manhattan(Point3D? other = null)
    {
        other ??= Zero;
        return Math.Abs(x - other.x) + Math.Abs(y - other.y) + Math.Abs(z - other.z);
    }

    public override string ToString()
    {
        return $"Point3D({x},{y},{z})";
    }


    public static readonly Point3D Zero = new Point3D(0, 0, 0);
    public static readonly Point3D Up = new Point3D(0, 0, -1);
    public static readonly Point3D Down = new Point3D(0, 0, 1);
    public static readonly Point3D Left = new Point3D(-1, 0, 0);
    public static readonly Point3D Right = new Point3D(1, 0, 0);
    public static readonly Point3D Forward = new Point3D(0, -1, 0);
    public static readonly Point3D Backward = new Point3D(0, 1, 0);


    public static readonly Dictionary<Facing3D, Point3D> FacingToPointVector = new Dictionary<Facing3D, Point3D>
    {
        { Facing3D.Up, Up },
        { Facing3D.Right, Right },
        { Facing3D.Down, Down },
        { Facing3D.Left, Left },
        { Facing3D.Forward, Forward },
        { Facing3D.Backward, Backward },
    };}

public enum Facing3D
{
    Up, Right, Down, Left, Forward, Backward
};
