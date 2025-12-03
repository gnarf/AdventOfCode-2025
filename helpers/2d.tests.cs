using NUnit;
using System.Collections;
using AoC2025;
using NUnit.Framework;

public static class Point2DTests
{
    public static object[] TestTurnValues => new object[]
    {
        new object[] { Point2D.Up, Turn2D.Left, Point2D.Left },
        new object[] { Point2D.Up, Turn2D.Right, Point2D.Right },
        new object[] { Point2D.Up, Turn2D.Around, Point2D.Down },
        new object[] { Point2D.Up, Turn2D.None, Point2D.Up },
        new object[] { Point2D.Right, Turn2D.Left, Point2D.Up },
        new object[] { Point2D.Right, Turn2D.Right, Point2D.Down },
        new object[] { Point2D.Right, Turn2D.Around, Point2D.Left },
        new object[] { new Point2D(10,20), Turn2D.Right, new Point2D(-20, 10) },
    };

    [TestCaseSource(nameof(TestTurnValues))]
    public static void TestTurn(Point2D point, Turn2D turn, Point2D expected) => Assert.AreEqual( expected, point.Turn(turn) );
}