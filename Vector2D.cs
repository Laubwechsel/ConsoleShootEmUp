using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleShootEmUp
{
    internal struct Vector2D
    {
        public double X;
        public double Y;
        public Vector2D()
        {
            X = 0;
            Y = 0;
        }
        public Vector2D(int x, int y)
        {
            X = x;
            Y = y;
        }
        public Vector2D(double x, double y)
        {
            X = x;
            Y = y;
        }
        public Vector2D(Vector2D other)
        {
            X = other.X;
            Y = other.Y;
        }
        public Vector2D(Vector2 vector2)
        {
            X = vector2.X;
            Y = vector2.Y;
        }
        public double Magnitude()
        {
            return Math.Sqrt(X * X + Y * Y);
        }
        public Vector2D Normalized()
        {
            return this / Magnitude();
        }
        public static List<Vector2D> GetPath(Vector2D from, Vector2D to, int steps = -1)
        {
            List<Vector2D> path = new List<Vector2D>();
            path.Add(from);
            double x = from.X;
            double y = from.Y;

            Vector2D delta = to - from;
            double stepsD;
            if (steps < 0)
            {
                stepsD = (int)(delta.Magnitude() * 4d);
            }
            else
            {
                stepsD = steps;
            }

            for (int i = 0; i < steps; i++)
            {
                x += delta.X * stepsD;
                y += delta.X * stepsD;
                Vector2D step = new Vector2D(x, y);
                if (!path.Contains(step))
                    path.Add(step);
            }
            path.Add(to);
            return path;

        }

        public static Vector2D operator +(Vector2D a, Vector2D b)
        {
            return new(a.X + b.X, a.Y + b.Y);
        }
        public static Vector2D operator +(Vector2D a, Vector2 b)
        {
            return new(a.X + b.X, a.Y + b.Y);
        }
        public static Vector2D operator -(Vector2D a, Vector2D b)
        {
            return new(a.X - b.X, a.Y - b.Y);
        }
        public static Vector2D operator *(Vector2D a, double b)
        {
            return new(a.X * b, a.Y * b);
        }
        public static Vector2D operator *(double a, Vector2D b)
        {
            return new(a * b.X, a * b.Y);
        }
        public static Vector2D operator /(Vector2D a, double b)
        {
            return new(a.X / b, a.Y / b);
        }

        public override readonly string ToString()
        {
            return $"X:{X} Y:{Y}";
        }

        public static readonly Vector2D Up = new Vector2D(0, -1);
        public static readonly Vector2D Down = new Vector2D(0, 1);
        public static readonly Vector2D Left = new Vector2D(-1, 0);
        public static readonly Vector2D Right = new Vector2D(1, 0);
    }
}
