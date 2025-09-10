using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleShootEmUp
{
    internal struct Vector2
    {
        //Max of +-23_170 else Hashcode overflow, actual safe range is larger if values are not close to each other eg 10,60000 is safe, but 10000,60000 is not
        public int X;
        public int Y;
        public Vector2()
        {
            X = 0;
            Y = 0;
        }
        public Vector2(int x, int y)
        {
            X = x;
            Y = y;
        }
        public Vector2(double x, double y)
        {
            X = (int)Math.Round(x);
            Y = (int)Math.Round(y);
        }
        public Vector2(Vector2D vector2D)
        {
            X = (int)Math.Round(vector2D.X);
            Y = (int)Math.Round(vector2D.Y);
        }

        public double Magnitude()
        {
            return Math.Sqrt(X * X + Y * Y);
        }
        public static Vector2 operator +(Vector2 a, Vector2 b)
        {
            return new(a.X + b.X, a.Y + b.Y);
        }
        public static Vector2 operator -(Vector2 a, Vector2 b)
        {
            return new(a.X - b.X, a.Y - b.Y);
        }
        public static bool operator ==(Vector2 a, Vector2 b)
        {
            return a.X == b.X && a.Y == b.Y;
        }
        public static bool operator !=(Vector2 a, Vector2 b)
        {
            return a.X != b.X || a.Y != b.Y;
        }
        public static List<Vector2> GetPath(Vector2 from, Vector2 to, int steps = -1)
        {
            List<Vector2> path = new List<Vector2>();
            path.Add(from);
            double x = from.X;
            double y = from.Y;

            Vector2 delta = to - from;
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
                Vector2 step = new Vector2(x, y);
                if (!path.Contains(step))
                    path.Add(step);
            }
            path.Add(to);
            return path;

        }

        public override bool Equals(object? obj)
        {
            if (obj == null)
                return false;
            if (obj is not Vector2)
                return false;
            Vector2 other = (Vector2)obj;
            return this == other;
        }

        public override int GetHashCode()
        {
            //cantor pairing function
            return (((((X + 23_170) + (Y + 23_170)) * ((X + 23_170) + (Y + 23_170) + 1)) / 2) + (Y + 23_170)) - int.MaxValue;
        }

        public override readonly string ToString()
        {
            return $"X:{X} Y:{Y}";
        }
        public static readonly Vector2 Up = new Vector2(0, -1);
        public static readonly Vector2 Down = new Vector2(0, 1);
        public static readonly Vector2 Left = new Vector2(-1, 0);
        public static readonly Vector2 Right = new Vector2(1, 0);

    }
}
