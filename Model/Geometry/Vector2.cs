using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Geometry
{
    public class Vector2<T>
    {
        public T X {  get; set; } 
        public T Y { get; set; }

        public Vector2()
        {
            randomise();
        }

        public Vector2(T x, T y)
        {
            X = x;
            Y = y;
        }

        public void randomise()
        {
            Random r = new Random();

            if (typeof(T) == typeof(float))
            {
                float sign = (r.NextSingle() - 0.5f) * 2;
                X = (T)(object)(r.NextSingle() * sign);
                Y = (T)(object)(r.NextSingle() * sign);
            }
            if (typeof(T) == typeof(double))
            {
                double sign = (r.NextDouble() - 0.5) * 2;
                X = (T)(object)(r.NextDouble() * sign);
                Y = (T)(object)(r.NextDouble() * sign);
            }
            if (typeof(T) == typeof(int))
            {
                int sign = r.Next() % 2 == 0? 1 : -1;
                X = (T)(object)(r.Next() * sign);
                Y = (T)(object)(r.Next() * sign);
            }
        }
        public void normalise()
        {
            if (typeof(T) == typeof(float))
            {
                // Cast X and Y to float
                float x = (float)(object)X;
                float y = (float)(object)Y;

                // Calculate the magnitude of the vector
                float magnitude = (float)Math.Sqrt(x * x + y * y);

                // Avoid division by zero
                if (magnitude > 0)
                {
                    // Normalize the vector components
                    X = (T)(object)(x / magnitude);
                    Y = (T)(object)(y / magnitude);
                }
            }
            else if (typeof(T) == typeof(double))
            {
                // Cast X and Y to float
                double x = (double)(object)X;
                double y = (double)(object)Y;

                // Calculate the magnitude of the vector
                double magnitude = (double)Math.Sqrt(x * x + y * y);

                // Avoid division by zero
                if (magnitude > 0)
                {
                    // Normalize the vector components
                    X = (T)(object)(x / magnitude);
                    Y = (T)(object)(y / magnitude);
                }
            }
            else
            {
                throw new InvalidOperationException("Normalization is only supported for float types");
            }
        }

        public static T toAngle(Vector2<T> target)
        {
            if (typeof(T) == typeof(float))
            {
                float x = (float)(object)target.X;
                float y = (float)(object)target.Y;

                return (T)(Object)float.Parse(Math.Atan2(y, x).ToString());
            }
            else if (typeof(T) == typeof(double))
            {
                double x = (double)(object)target.X;
                double y = (double)(object)target.Y;

                return (T)(Object)Math.Atan2(y, x);
            }
            else
            {
                throw new InvalidOperationException("ToAngle is only supported for float and double types");
            }
        }

        public static Vector2<T> fromAngle(T angle)
        {
            if (typeof(T) == typeof(float))
            {
                float a = (float)(object)angle;

                float x = (float)Math.Cos(a);
                float y = (float)Math.Sin(a);

                return new Vector2<T>((T)(object)x, (T)(object)y);
            }
            else if (typeof(T) == typeof(double))
            {
                double a = (double)(object)angle;

                double x = Math.Cos(a);
                double y = Math.Sin(a);

                return new Vector2<T>((T)(object)x, (T)(object)y);
            }
            else
            {
                throw new InvalidOperationException("fromAngle is only supported for float and double types");
            }
        }

        public T fromAngle(float angle)
        {
            if (typeof(T) == typeof(float))
            {
                float x = (float)(object)X;
                float y = (float)(object)Y;

                return (T)(Object)float.Parse(Math.Atan2(y, x).ToString());
            }
            else if (typeof(T) == typeof(double))
            {
                double x = (double)(object)X;
                double y = (double)(object)Y;

                return (T)(Object)Math.Atan2(y, x);
            }
            else
            {
                throw new InvalidOperationException("FromAngle is only supported for float types");
            }
        }

        public Vector2<T> add(Vector2<T> a, Vector2<T> b)
        {
            throw new NotImplementedException();
        }
    }
}
