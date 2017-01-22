﻿using System.Collections.Generic;
using System.Drawing;
using GraphicsModule.Geometry.Interfaces;
using GraphicsModule.Geometry.Objects.Points;
using GraphicsModule.Settings;

namespace GraphicsModule.Geometry.Objects.Lines
{
    /// <summary>Класс для расчета параметров проекции 3D линии на X0Y плоскость проекций</summary>
    /// <remarks>Copyright © Polozkov V. Yury, 2015</remarks>
    public class LineOfPlane1X0Y : IObject, ILineOfPlane
    {
        public PointOfPlane1X0Y Point0 { get; set; }
        public PointOfPlane1X0Y Point1 { get; set; }
        public double kx { get; set; }
        public double ky { get; set; }
        private LineDrawCalc _calc;
        public List<PointF> pts { get; set; }
        public LineOfPlane1X0Y()
        {
            Point0 = new PointOfPlane1X0Y();
            Point1 = new PointOfPlane1X0Y();
        }
        public LineOfPlane1X0Y(PointOfPlane1X0Y pt0, PointOfPlane1X0Y pt1)
        {
            Point0 = pt0;
            Point1 = pt1;
            kx = pt1.X - pt0.X;
            ky = pt1.Y - pt0.Y;
        }
        public LineOfPlane1X0Y(PointOfPlane1X0Y pt0, PointOfPlane1X0Y pt1, Point frameCenter, RectangleF rc)
        {
            Point0 = pt0;
            Point1 = pt1;
            kx = pt1.X - pt0.X;
            ky = pt1.Y - pt0.Y;
            _calc = new LineDrawCalc(frameCenter, rc);
            pts = _calc.CalculatePointsForDraw(this);
        }
        public LineOfPlane1X0Y(Line2D line)
        {
            Point0.X = line.Point0.X;
            Point0.Y = line.Point0.Y;
            Point1.X = line.Point1.X;
            Point1.Y = line.Point1.Y;
            kx = Point1.X - Point0.X;
            ky = Point1.Y - Point0.Y;
        }
        public LineOfPlane1X0Y(Line3D line)
        {
            Point0.X = line.Point0.X;
            Point0.Y = line.Point0.Y;
            Point1.X = line.Point1.X;
            Point1.Y = line.Point1.Y;
        }
        public void Draw(DrawS st, Point framecenter, Graphics g)
        {
            
            Point0.Draw(st, framecenter, g);
            Point1.Draw(st, framecenter, g);
            g.DrawLine(st.PenLineOfPlane1X0Y, pts[0], pts[1]);
        }
        public void DrawLineOnly(DrawS st, Point framecenter, Graphics g)
        {
            Point0.DrawPointsOnly(st, framecenter, g);
            Point1.DrawPointsOnly(st, framecenter, g);
            g.DrawLine(st.PenLineOfPlane1X0Y, pts[0], pts[1]);
        }
        public void CalculatePointsForDraw()
        {
            pts = _calc.CalculatePointsForDraw(this);
        }
        public void CalculatePointsForDraw(Point frameCenter, RectangleF rc)
        {
            _calc = new LineDrawCalc(frameCenter, rc);
            pts = _calc.CalculatePointsForDraw(this);
        }
        public bool IsSelected(Point mscoords, float ptR, Point frameCenter, double distance)
        {
            var ln = DeterminePosition.ForLineProjection(this, frameCenter);
            return Analyze.Analyze.LinesPos.IncidenceOfPoint(mscoords, ln, 35 * distance);
        }

    }
}