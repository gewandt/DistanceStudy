﻿using System.Collections.ObjectModel;
using System.Drawing;
using GraphicsModule.Geometry.Interfaces;
using GraphicsModule.Geometry.Objects.Points;
using GraphicsModule.Settings;

namespace GraphicsModule.Geometry.Objects.Lines
{
    /// <summary>Класс для задания и расчета параметров 3D прямой</summary>
    /// <remarks>Copyright © Polozkov V. Yury, 2015</remarks>
    public class Line3D : IObject
    {
        public Point3D Point0 { get; set; }
        public Point3D Point1 { get; set; }
        public Name Name { get; set; }
        public LineOfPlane1X0Y LineOfPlane1X0Y { get; set; }
        public LineOfPlane2X0Z LineOfPlane2X0Z { get; set; }
        public LineOfPlane3Y0Z LineOfPlane3Y0Z { get; set; }
        public double Kx { get; set; }
        public double Ky { get; set; }
        public double Kz { get; set; }
        public static Line3D Create(Collection<IObject> lst)
        {
            if (lst[0].GetType() == typeof(LineOfPlane1X0Y))
            {
                return lst[1].GetType() == typeof(LineOfPlane2X0Z) ? 
                    new Line3D((LineOfPlane1X0Y)lst[0], (LineOfPlane2X0Z)lst[1]) :
                    new Line3D((LineOfPlane1X0Y)lst[0], (LineOfPlane3Y0Z)lst[1]);
            }
            if (lst[0].GetType() == typeof(LineOfPlane2X0Z))
            {
                return lst[1].GetType() == typeof(LineOfPlane1X0Y) ? 
                    new Line3D((LineOfPlane1X0Y)lst[1], (LineOfPlane2X0Z)lst[0]) : 
                    new Line3D((LineOfPlane2X0Z)lst[0], (LineOfPlane3Y0Z)lst[1]);
            }
            return lst[1].GetType() == typeof(LineOfPlane1X0Y) ? 
                new Line3D((LineOfPlane1X0Y)lst[1], (LineOfPlane3Y0Z)lst[0]) : 
                new Line3D((LineOfPlane2X0Z)lst[1], (LineOfPlane3Y0Z)lst[0]);
        }
        public Line3D(LineOfPlane1X0Y linePi1, LineOfPlane2X0Z linePi2)
        {
            Point0 = new Point3D();
            Point1 = new Point3D();
            if (linePi1.Point0.X == linePi2.Point0.X &&
                linePi1.Point1.X == linePi2.Point1.X)
            {
                Point0.X = linePi1.Point0.X;
                Point0.Y = linePi1.Point0.Y;
                Point0.Z = linePi2.Point0.Z;
                Point1.X = linePi1.Point1.X;
                Point1.Y = linePi1.Point1.Y;
                Point1.Z = linePi2.Point1.Z;
                LineOfPlane1X0Y = linePi1;
                LineOfPlane2X0Z = linePi2;
                LineOfPlane3Y0Z = new LineOfPlane3Y0Z(new PointOfPlane3Y0Z(linePi1.Point0.Y, linePi2.Point0.Z), new PointOfPlane3Y0Z(linePi1.Point1.Y, linePi2.Point1.Z));
                Point0.InitializePointsOfPlane();
                Point1.InitializePointsOfPlane();
            }
            else if (linePi1.Point0.X == linePi2.Point1.X &&
                     linePi1.Point1.X == linePi2.Point0.X)
            {
                Point0.X = linePi1.Point1.X;
                Point0.Y = linePi1.Point1.Y;
                Point0.Z = linePi2.Point0.Z;
                Point1.X = linePi1.Point0.X;
                Point1.Y = linePi1.Point0.Y;
                Point1.Z = linePi2.Point1.Z;
                Point0.InitializePointsOfPlane();
                Point1.InitializePointsOfPlane();
                LineOfPlane1X0Y = linePi1;
                LineOfPlane2X0Z = linePi2;
                LineOfPlane3Y0Z = new LineOfPlane3Y0Z(new PointOfPlane3Y0Z(linePi1.Point1.Y, linePi2.Point0.Z), new PointOfPlane3Y0Z(linePi1.Point0.Y, linePi2.Point1.Z));
            }
        }
        public Line3D(LineOfPlane1X0Y linePi1, LineOfPlane3Y0Z linePi3)
        {
            Point0 = new Point3D();
            Point1 = new Point3D();
            if (linePi1.Point0.Y == linePi3.Point0.Y &&
                linePi1.Point1.Y == linePi3.Point1.Y)
            {
                Point0.X = linePi1.Point0.X;
                Point0.Y = linePi1.Point0.Y;
                Point0.Z = linePi3.Point0.Z;
                Point1.X = linePi1.Point1.X;
                Point1.Y = linePi1.Point1.Y;
                Point1.Z = linePi3.Point1.Z;
                Point0.InitializePointsOfPlane();
                Point1.InitializePointsOfPlane();
                LineOfPlane1X0Y = linePi1;
                LineOfPlane2X0Z = new LineOfPlane2X0Z(new PointOfPlane2X0Z(linePi1.Point0.X, linePi3.Point0.Z), new PointOfPlane2X0Z(linePi1.Point1.X, linePi3.Point1.Z));
                LineOfPlane3Y0Z = linePi3;
            }
            else if (linePi1.Point0.Y == linePi3.Point1.Y &&
                     linePi1.Point1.Y == linePi3.Point0.Y)
            {
                Point0.X = linePi1.Point0.X;
                Point0.Y = linePi1.Point0.Y;
                Point0.Z = linePi3.Point1.Z;
                Point1.X = linePi1.Point1.X;
                Point1.Y = linePi1.Point1.Y;
                Point1.Z = linePi3.Point0.Z;
                Point0.InitializePointsOfPlane();
                Point1.InitializePointsOfPlane();
                LineOfPlane1X0Y = linePi1;
                LineOfPlane2X0Z = new LineOfPlane2X0Z(new PointOfPlane2X0Z(linePi1.Point0.X, linePi3.Point1.Z), new PointOfPlane2X0Z(linePi1.Point1.X, linePi3.Point0.Z));
                LineOfPlane3Y0Z = linePi3;
            }
        }
        public Line3D(LineOfPlane2X0Z linePi2, LineOfPlane3Y0Z linePi3)
        {
            Point0 = new Point3D();
            Point1 = new Point3D();
            if (linePi2.Point0.Z == linePi3.Point0.Z &&
               linePi2.Point1.Z == linePi3.Point1.Z)
            {
                Point0.X = linePi2.Point0.X;
                Point0.Y = linePi3.Point0.Y;
                Point0.Z = linePi2.Point0.Z;
                Point1.X = linePi2.Point1.X;
                Point1.Y = linePi3.Point1.Y;
                Point1.Z = linePi2.Point1.Z;
                Point0.InitializePointsOfPlane();
                Point1.InitializePointsOfPlane();
                LineOfPlane1X0Y = new LineOfPlane1X0Y(new PointOfPlane1X0Y(linePi2.Point0.X, linePi3.Point0.Y), new PointOfPlane1X0Y(linePi2.Point1.X, linePi3.Point1.Y));
                LineOfPlane2X0Z = linePi2;
                LineOfPlane3Y0Z = linePi3;
            }
            else if (linePi2.Point1.Z == linePi3.Point0.Z &&
                    linePi2.Point0.Z == linePi3.Point1.Z)
            {
                Point0.X = linePi2.Point0.X;
                Point0.Y = linePi3.Point1.Y;
                Point0.Z = linePi2.Point0.Z;
                Point1.X = linePi2.Point1.X;
                Point1.Y = linePi3.Point0.Y;
                Point1.Z = linePi2.Point1.Z;
                Point0.InitializePointsOfPlane();
                Point1.InitializePointsOfPlane();
                LineOfPlane1X0Y = new LineOfPlane1X0Y(new PointOfPlane1X0Y(linePi2.Point0.X, linePi3.Point1.Y), new PointOfPlane1X0Y(linePi2.Point1.X, linePi3.Point0.Y));
                LineOfPlane2X0Z = linePi2;
                LineOfPlane3Y0Z = linePi3;
            }
        }
        public void Draw(DrawS st, Point frameCenter, Graphics g)
        {
            LineOfPlane1X0Y.DrawLineOnly(st, frameCenter, g);
            LineOfPlane2X0Z.DrawLineOnly(st, frameCenter, g);
            LineOfPlane3Y0Z.DrawLineOnly(st, frameCenter, g);
            if (st.LinkLineSettings.IsDraw)
            {
                DrawLinkLine(st.LinkLineSettings.PenLinkLineX0YtoX, st.LinkLineSettings.PenLinkLineX0YtoY, st.LinkLineSettings.PenLinkLineX0ZtoX, st.LinkLineSettings.PenLinkLineX0ZtoZ,
                             st.LinkLineSettings.PenLinkLineY0ZtoZ, st.LinkLineSettings.PenLinkLineY0ZtoY, frameCenter, ref g);
            }

        }
        public void DrawLinkLine(Pen penLinkLineX0YtoX, Pen penLinkLineX0YtoY, Pen penLinkLineX0ZtoX, Pen penLinkLineX0ZtoZ, Pen penLinkLineY0ZtoZ, Pen penLinkLineY0ZtoY, Point frameCenter, ref Graphics graphics)
        {
            Point0.DrawLinkLine(penLinkLineX0YtoX, penLinkLineX0YtoY, penLinkLineX0ZtoX, penLinkLineX0ZtoZ,
                         penLinkLineY0ZtoZ, penLinkLineY0ZtoY, frameCenter, ref graphics);
            Point1.DrawLinkLine(penLinkLineX0YtoX, penLinkLineX0YtoY, penLinkLineX0ZtoX, penLinkLineX0ZtoZ,
                          penLinkLineY0ZtoZ, penLinkLineY0ZtoY, frameCenter, ref graphics);
        }
        public bool IsSelected(Point mscoords, float ptR, Point frameCenter, double distance)
        {
            return LineOfPlane1X0Y.IsSelected(mscoords, ptR, frameCenter, distance) ||
                   LineOfPlane2X0Z.IsSelected(mscoords, ptR, frameCenter, distance) ||
                   LineOfPlane3Y0Z.IsSelected(mscoords, ptR, frameCenter, distance);
        }
        private void CalculatePointsForDraw(Point frameCenter, RectangleF rc1, RectangleF rc2, RectangleF rc3)
        {
            LineOfPlane1X0Y.CalculatePointsForDraw(frameCenter, rc1);
            LineOfPlane2X0Z.CalculatePointsForDraw(frameCenter, rc2);
            LineOfPlane3Y0Z.CalculatePointsForDraw(frameCenter, rc3);
        }
        public void SpecifyBoundaryPoints(Point frameCenter, RectangleF rc1, RectangleF rc2, RectangleF rc3)
        {
            CalculatePointsForDraw(frameCenter, rc1, rc2, rc3);
            CutLineX0YtoX0Z(frameCenter, rc1);
            CutLineX0ZtoX0Y(frameCenter, rc1);
            CutLineX0ZtoY0Z(frameCenter, rc1);
            CutLineY0ZtoX0Z(frameCenter, rc1);
        }
        private void CutLineX0YtoX0Z(Point frameCenter, RectangleF rc)
        {
            if((LineOfPlane1X0Y.DrawPoints[0].X > LineOfPlane2X0Z.DrawPoints[0].X) && (LineOfPlane1X0Y.DrawPoints[0].Y == rc.Top))
            {
                var ln = new Line2D(new Point2D(LineOfPlane1X0Y.DrawPoints[0].X, LineOfPlane1X0Y.DrawPoints[0].Y),
                                    new Point2D(LineOfPlane1X0Y.DrawPoints[0].X, LineOfPlane1X0Y.DrawPoints[0].Y - 10));
                LineOfPlane2X0Z.DrawPoints[0] = Calculate.CrossingPoint(ln, LineOfPlane2X0Z, frameCenter);
            }
            if(LineOfPlane1X0Y.DrawPoints[1].X < LineOfPlane2X0Z.DrawPoints[1].X && (LineOfPlane1X0Y.DrawPoints[1].Y == rc.Top))
            {
                var ln = new Line2D(new Point2D(LineOfPlane1X0Y.DrawPoints[1].X, LineOfPlane1X0Y.DrawPoints[1].Y),
                                    new Point2D(LineOfPlane1X0Y.DrawPoints[1].X, LineOfPlane1X0Y.DrawPoints[1].Y - 10));
                LineOfPlane2X0Z.DrawPoints[1] = Calculate.CrossingPoint(ln, LineOfPlane2X0Z, frameCenter);
            }
        }
        private void CutLineX0ZtoX0Y(Point frameCenter, RectangleF rc)
        {
            if ((LineOfPlane2X0Z.DrawPoints[0].X > LineOfPlane1X0Y.DrawPoints[0].X) && (LineOfPlane2X0Z.DrawPoints[0].Y == rc.Top))
            {
                var ln = new Line2D(new Point2D(LineOfPlane2X0Z.DrawPoints[0].X, LineOfPlane2X0Z.DrawPoints[0].Y),
                                    new Point2D(LineOfPlane2X0Z.DrawPoints[0].X, LineOfPlane2X0Z.DrawPoints[0].Y - 10));
                LineOfPlane1X0Y.DrawPoints[0] = Calculate.CrossingPoint(ln, LineOfPlane1X0Y, frameCenter);
            }
            if ((LineOfPlane2X0Z.DrawPoints[1].X < LineOfPlane1X0Y.DrawPoints[1].X) && (LineOfPlane2X0Z.DrawPoints[1].Y == rc.Top))
            {
                var ln = new Line2D(new Point2D(LineOfPlane2X0Z.DrawPoints[1].X, LineOfPlane2X0Z.DrawPoints[1].Y),
                                    new Point2D(LineOfPlane2X0Z.DrawPoints[1].X, LineOfPlane2X0Z.DrawPoints[1].Y - 10));
                LineOfPlane1X0Y.DrawPoints[1] = Calculate.CrossingPoint(ln, LineOfPlane1X0Y, frameCenter);
            }
        }
        private void CutLineX0ZtoY0Z(Point frameCenter, RectangleF rc)
        {
            if ((LineOfPlane2X0Z.DrawPoints[0].Y < LineOfPlane3Y0Z.DrawPoints[0].Y) && (LineOfPlane2X0Z.DrawPoints[0].X == rc.Right))
            {
                var ln = new Line2D(new Point2D(LineOfPlane2X0Z.DrawPoints[0].X, LineOfPlane2X0Z.DrawPoints[0].Y),
                                    new Point2D(LineOfPlane2X0Z.DrawPoints[0].X - 10, LineOfPlane2X0Z.DrawPoints[0].Y));
                LineOfPlane3Y0Z.DrawPoints[0] = Calculate.CrossingPoint(ln, LineOfPlane3Y0Z, frameCenter);
            }
            if((LineOfPlane2X0Z.DrawPoints[1].Y > LineOfPlane3Y0Z.DrawPoints[1].Y) && (LineOfPlane2X0Z.DrawPoints[1].X == rc.Right))
            {
                var ln = new Line2D(new Point2D(LineOfPlane2X0Z.DrawPoints[1].X, LineOfPlane2X0Z.DrawPoints[1].Y),
                                    new Point2D(LineOfPlane2X0Z.DrawPoints[1].X - 10, LineOfPlane2X0Z.DrawPoints[1].Y));
                LineOfPlane3Y0Z.DrawPoints[1] = Calculate.CrossingPoint(ln, LineOfPlane3Y0Z, frameCenter);
            }
        }
        private void CutLineY0ZtoX0Z(Point frameCenter, RectangleF rc)
        {
            if ((LineOfPlane3Y0Z.DrawPoints[0].Y > LineOfPlane2X0Z.DrawPoints[0].Y) && (LineOfPlane3Y0Z.DrawPoints[0].X == rc.Right))
            {
                var ln = new Line2D(new Point2D(LineOfPlane3Y0Z.DrawPoints[0].X, LineOfPlane3Y0Z.DrawPoints[0].Y),
                                    new Point2D(LineOfPlane3Y0Z.DrawPoints[0].X - 10, LineOfPlane3Y0Z.DrawPoints[0].Y));
                LineOfPlane2X0Z.DrawPoints[0] = Calculate.CrossingPoint(ln, LineOfPlane2X0Z, frameCenter);
            }
            if ((LineOfPlane3Y0Z.DrawPoints[1].Y > LineOfPlane2X0Z.DrawPoints[1].Y) && (LineOfPlane3Y0Z.DrawPoints[1].X == rc.Right))
            {
                var ln = new Line2D(new Point2D(LineOfPlane3Y0Z.DrawPoints[1].X, LineOfPlane3Y0Z.DrawPoints[1].Y),
                                    new Point2D(LineOfPlane3Y0Z.DrawPoints[1].X - 10, LineOfPlane3Y0Z.DrawPoints[1].Y));
                LineOfPlane2X0Z.DrawPoints[1] = Calculate.CrossingPoint(ln, LineOfPlane2X0Z, frameCenter);
            }
        }
        public Name GetName()
        {
            return Name;
        }
        public void SetName(Name name)
        {
            Name = new Name(name);
            Point0.SetName(Name);
            Point1.SetName(Name);
            LineOfPlane1X0Y.SetName(Name);
            LineOfPlane2X0Z.SetName(Name);
            LineOfPlane3Y0Z.SetName(Name);
        }
    }
}
