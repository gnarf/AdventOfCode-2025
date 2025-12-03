using System.Drawing;

namespace AoC2025;

public class Point2D : IEquatable<Point2D>
{
    public long x { get; init; }
    public long y { get; init; }

    public Point2D(long x, long y)
    {
        this.x = x;
        this.y = y;
    }

    public bool Equals(Point2D? other)
    {
        return other is not null && x == other.x && y == other.y;
    }

    public override bool Equals(object? obj)
    {
        if (obj is Point2D other) return Equals(other);
        return base.Equals(obj);
    }

    public override int GetHashCode()
    {
        return Tuple.Create(x, y).GetHashCode();
    }

    public double tan()
    {
        return (double)y / (double)x;
    }

    public static bool operator ==(Point2D a, Point2D b)
    {
        return a?.Equals(b) ?? b is null;
    }
    public static bool operator !=(Point2D? a, Point2D? b)
    {
        return !(a?.Equals(b) ?? b is null);
    }

    public static Point2D operator -(Point2D a, Point2D b)
    {
        return new Point2D(a.x - b.x, a.y - b.y);
    }
    public static Point2D operator +(Point2D a, Point2D b)
    {
        return new Point2D(a.x + b.x, a.y + b.y);
    }

    public static Point2D operator *(Point2D a, long b)
    {
        return new Point2D(a.x * b, a.y * b);
    }
    public static Point2D operator *(long b, Point2D a)
    {
        return new Point2D(a.x * b, a.y * b);
    }

    public static Point2D Min(Point2D a, Point2D b)
    {
        return new Point2D(Math.Min(a.x, b.x), Math.Min(a.y, b.y));
    }

    public static Point2D Max(Point2D a, Point2D b)
    {
        return new Point2D(Math.Max(a.x, b.x), Math.Max(a.y, b.y));
    }
    public static Point2D Sign(Point2D a)
    {
        return new Point2D(Math.Sign(a.x), Math.Sign(a.y));
    }
    public Point2D Sign() => Point2D.Sign(this);

    public long Manhattan(Point2D? other = null)
    {
        other ??= Zero;
        return Math.Abs(x - other.x) + Math.Abs(y - other.y);
    }

    public Point2D CycleMod(Point2D domain)
    {
        return new Point2D( ((x % domain.x) + domain.x) % domain.x, ((y % domain.y) + domain.y) % domain.y);
    }

    public override string ToString()
    {
        return String.Format("Point2D({0},{1})", x, y);
    }


    public static readonly Point2D Zero = new Point2D(0, 0);
    public static readonly Point2D Up = new Point2D(0, -1);
    public static readonly Point2D Down = new Point2D(0, 1);
    public static readonly Point2D Left = new Point2D(-1, 0);
    public static readonly Point2D Right = new Point2D(1, 0);


    public static readonly Dictionary<Facing2D, Point2D> FacingToPointVector = new Dictionary<Facing2D, Point2D>
    {
        { Facing2D.Up, Up },
        { Facing2D.Right, Right },
        { Facing2D.Down, Down },
        { Facing2D.Left, Left },
    };

    public static Point2D Facing(Facing2D F) => FacingToPointVector[F];

    public Point2D Turn(Turn2D turn)
    {
        return turn switch
        {
            Turn2D.Left => new Point2D(this.y, this.x * -1),
            Turn2D.Right => new Point2D(this.y * -1, this.x * 1),
            Turn2D.Around => new Point2D(this.x * -1, this.y * -1),
            _ => this,
        };
    }

    public static void PrintGrid(Point2D min, Point2D max, Func<Point2D, char> charToPrint)
    {
        for (long y=min.y; y<=max.y; y++)
        {
            string linebuffer = "";
            for (long x=min.x; x<=max.x; x++)
            {
                linebuffer += charToPrint(new Point2D(x, y));
            }
            Console.WriteLine(linebuffer);
        }
    }
    public static void PrintGrid(IEnumerable<Point2D> points, Func<Point2D, char> charToPrint)
    {
        PrintGrid(points.Aggregate(Point2D.Min), points.Aggregate(Point2D.Max), charToPrint);
    }

    public static class Point2DCarinals
    {
        public static Point2D North = Point2D.Up;
        public static Point2D South = Point2D.Down;
        public static Point2D West = Point2D.Left;
        public static Point2D East = Point2D.Right;
    }
}

public enum Facing2D
{
    Up, Right, Down, Left
};

public enum Turn2D
{
    Forward = 0,
    None = 0,
    Left = 1,
    CounterClockwise = 1,
    Right = 2,
    Clockwise = 2,
    Around = 3,
    OneEighty = 3
};

