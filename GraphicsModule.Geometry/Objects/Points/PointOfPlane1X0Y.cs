﻿using System;
using System.Drawing;
using GraphicsModule.Configuration;
using GraphicsModule.Geometry.Interfaces;

namespace GraphicsModule.Geometry.Objects.Points
{
    /// <summary>Класс для расчета параметров проекции 3D точки на X0Y плоскость проекций</summary>
    /// <remarks>Copyright © Polozkov V. Yury, 2015</remarks>
    public class PointOfPlane1X0Y : IPointOfPlane, IObjectOfPlane1X0Y
    {
        public double X { get; set; }
        /// <summary>Получает или задает координату Y двумерной проекции точки</summary>
        /// <remarks></remarks>
        public double Y { get; set; }
        private Name _name;
        public Name Name {
            get { return _name; }
            set
            {
                _name = new Name(value);
                _name.Value += "'";
            }
        }
        public string Id => $"P{X}{Y}";
        /// <summary>Инициализирует новый экземпляр двумерной проекции точки с указанными координатами</summary>
        /// <remarks></remarks>
        public PointOfPlane1X0Y(double x, double y) { X = x; Y = y; }
        public PointOfPlane1X0Y(Point pt, Point center)
        {
            X = -(pt.X - center.X);
            Y = pt.Y - center.Y;
        }
        public static bool IsCreatable(Point pt, Point frameCenter)
        {
            return (pt.X - frameCenter.X) <= 0 && (pt.Y - frameCenter.Y) >= 0;
        }
        /// <summary>Передвигает ранее заданную двумерную проекцию точку
        /// (изменяет коодинаты на указанные величины по осям в 2D)</summary>
        /// <remarks>PointOfPlan1_X0Y.X += dx; PointOfPlan1_X0Y.Y += dy</remarks>
        public void PointMove(double dx, double dy) { X += dx; Y += dy; }
        public void Draw(Pen pen, float poitRaduis, Point frameCenter, Graphics graphics)
        {
            var ptForDraw = DeterminePosition.ForPointProjection(this, poitRaduis, frameCenter);
            graphics.DrawPie(pen, ptForDraw.X, ptForDraw.Y, poitRaduis * 2, poitRaduis * 2, 0, 360);
        }
        public void Draw(DrawSettings settings, Point frameCenter, Graphics g)
        {
            Draw(settings.PenPoints, settings.RadiusPoints, frameCenter, g);
            if (settings.LinkLinesSettings.IsDraw)
            {
                DrawLinkLine(settings.LinkLinesSettings.PenLinkLineX0YtoX, settings.LinkLinesSettings.PenLinkLineX0YtoY, true, true, true, true, true, frameCenter, g);
            }
            if (Name.IsDraw)
                DrawName(settings, settings.RadiusPoints, frameCenter, g);
        }
        public void DrawName(DrawSettings settings, float poitRaduis, Point frameCenter, Graphics graphics)
        {
            var ptForDraw = DeterminePosition.ForPointProjection(this, poitRaduis, frameCenter);
            graphics.DrawString(Name.Value, settings.TextFont, settings.TextBrush, ptForDraw.X + Name.Dx, ptForDraw.Y + Name.Dy);
        }
        public void DrawPointsOnly(DrawSettings st, Point frameCenter, Graphics g)
        {
            Draw(st.PenPoints, st.RadiusPoints, frameCenter, g);
            DrawName(st, st.RadiusPoints, frameCenter, g);
        }
        public void DrawLinkLine(Pen penLinkLineToX, Pen penLinkLinetoY, bool linkPointToX, bool linkPointToY, bool linkXToBorderPi2, bool linkYToBorderPi3, bool linkCurveY1ToY3, Point frameCenter, Graphics graphics)
        {
            //Отрисовка линий связи Горизонтальной проекции
            //Контроль нулевого значения координаты X Горизонтальной проекции точки
            //Проекция точки инцидентна оси X, следовательно отрисовка линий связи не требуется (обрабатывается ошибка существования нулевой ширины и высоты прямоугольника, в который вписывается дуга окружности)
            const double tolerance = 0.0001;
            if (Math.Abs(Y) < tolerance) return;
            if (linkPointToX) //Контроль включения линии связи от проекции точки до оси X
            {
                //Горизонтальная (от Pi1 к Pi3) - Часть 1: отрезок от заданной точки до оси X
                graphics.DrawLine(penLinkLineToX, Convert.ToInt32(frameCenter.X - X), Convert.ToInt32(frameCenter.Y + Y), Convert.ToInt32(frameCenter.X - X), Convert.ToInt32(frameCenter.Y));
            }
            if (linkXToBorderPi2)//Контроль включения линии связи от оси X до верхней границы плоскости проекций Pi2
            {
                //Горизонтальная (от Pi1 к Pi3) - Часть 2: отрезок от оси X до верхней границы области рисования (верхней границы плоскости проекций Pi2)
                graphics.DrawLine(penLinkLineToX, Convert.ToInt32(frameCenter.X - X), Convert.ToInt32(frameCenter.Y), Convert.ToInt32(frameCenter.X - X), -Convert.ToInt32(2 * frameCenter.Y + 20));
            }
            if (linkPointToY)//Контроль включения линии связи от проекции точки до оси Y
            {
                //Горизонтальная (от Pi1 к Pi3) - Часть 3: отрезок от заданной точки до вертикальной оси Y (оси Y плоскости проекций Pi1)
                graphics.DrawLine(penLinkLinetoY, Convert.ToInt32(frameCenter.X - X), Convert.ToInt32(frameCenter.Y + Y), Convert.ToInt32(frameCenter.X), Convert.ToInt32(frameCenter.Y + Y));
            }
            if (linkCurveY1ToY3)//Контроль включения линии связи (дуги) от вертикальной оси Y плоскости Pi1 до горизонтальной оси Y плоскости Pi3
            {
                //Горизонтальная (от Pi1 к Pi3) - Часть 4: дуга от вертикальной оси Y до горизонтальной оси Y
                graphics.DrawArc(penLinkLinetoY, Convert.ToInt32(frameCenter.X) - Convert.ToInt32(Y), Convert.ToInt32(frameCenter.Y) - Convert.ToInt32(Y), Convert.ToInt32(2 * Y), Convert.ToInt32(2 * Y), 0, 90);
            }
            if (linkYToBorderPi3) //Контроль включения линии связи от горизонтальной оси Y до верхней границы плоскости проекций Pi3
            {
                //Вертикальная (от Pi1 к Pi2) - Часть 5: отрезок от горизонтальной оси Y до границы области рисования (верхней границы плоскости проекций Pi3)
                graphics.DrawLine(penLinkLinetoY, Convert.ToInt32(frameCenter.X + Y), frameCenter.Y, Convert.ToInt32(frameCenter.X + Y), 0);
            }
        }
        public bool IsSelected(Point mscoords, float ptR, Point frameCenter, double distance)
        {
            return Calculate.Distance(mscoords, ptR, frameCenter, this) < distance;
        }
        public Name GetName()
        {
            return _name;
        }

        public void SetName(Name name)
        {
            _name = name;
        }
    }
}
