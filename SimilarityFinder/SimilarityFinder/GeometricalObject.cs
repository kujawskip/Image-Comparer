using AForge;
using AForge.Imaging;
using AForge.Math.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimilarityFinder
{
    public abstract class GeometricalObject
    {
        public UnmanagedImage Image;
        public List<IntPoint> Points;
        public int Area;

        public abstract bool RoughlyEquals(GeometricalObject other);
    }

    public class Circle : GeometricalObject
    {
        public AForge.Point Center;
        public float Radius;

        public override bool RoughlyEquals(GeometricalObject other)
        {
            return other is Circle;
        }
    }

    public class Polygon : GeometricalObject
    {
        public PolygonSubType Type;
        public List<IntPoint> Corners;
        public double Fullness;
        private List<double> angles = null;
        public List<double> Angles
        {
            get
            {
                if (angles != null)
                {
                    return angles;
                }
                else
                {
                    angles = new List<double>();
                    for (int i = 0; i < Corners.Count; i++)
                    {
                        Line l1 =
                            Line.FromPoints(Corners[(i - 1 + Corners.Count) % Corners.Count], Corners[i]);
                        Line l2 =
                            Line.FromPoints(Corners[i], Corners[(i + 1) % Corners.Count]);
                        float angle = l1.GetAngleBetweenLines(l2);
                        angles.Add(angle);
                    }
                }
                return angles;
            }
        }

        public override bool RoughlyEquals(GeometricalObject other)
        {
            if(!(other is Polygon))
            {
                return false;
            }
            Polygon otherPolygon = other as Polygon;
            bool sameType = otherPolygon.Type == Type &&
                otherPolygon.Corners.Count == Corners.Count;
            if (sameType)
            {
                if(Math.Abs(Fullness - otherPolygon.Fullness) < 0.1)
                {
                    foreach (var angle in Angles)
                    {
                        bool found = false;
                        foreach (var otherAngle in otherPolygon.Angles)
                        {
                            if (Math.Abs(angle - otherAngle) < 5)
                            {
                                found = true;
                                break;
                            }
                        }
                        if (!found)
                        {
                            return false;
                        }
                    }
                    return true;
                }
            }
            return false;
        }
    }
}
