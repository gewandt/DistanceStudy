﻿using System.Drawing;
using System.Linq;
using GraphicsModule.Configuration;
using GraphicsModule.Controls;
using GraphicsModule.Geometry.Analyze;
using GraphicsModule.Geometry.Objects.Lines;
using GraphicsModule.Geometry.Objects.Points;
using GraphicsModule.Interfaces;

namespace GraphicsModule.Rules.Objects.Lines
{
    /// <summary>
    /// Создание проекции линии на плоскость X0Y
    /// </summary>
    public class CreateLineOfPlane1X0Y : ICreate
    {
        public void AddToStorageAndDraw(Point pt, Point frameCenter, Canvas canvas, DrawSettings settings, Storage storage)
        {
            var obj = Create(pt, frameCenter, canvas, settings, storage);
            if (obj == null) return;
            storage.AddToCollection(obj);
            canvas.Update(storage);
        }
        public LineOfPlane1X0Y Create(Point pt, Point frameCenter, Canvas can, DrawSettings setting, Storage strg)
        {
            if (!PointOfPlane1X0Y.IsCreatable(pt, frameCenter)) return null;
            var ptOfPlane = new PointOfPlane1X0Y(pt, frameCenter);
            if (strg.TempObjects.Count == 0)
            {
                ptOfPlane.Name = GraphicsControl.NamesGenerator.Generate();
                strg.TempObjects.Add(ptOfPlane);
                strg.DrawLastAddedToTempObjects(setting, frameCenter, can.Graphics);
                return null;
            }
            if (Analyze.PointsPosition.Coincidence((PointOfPlane1X0Y)strg.TempObjects.First(), new PointOfPlane1X0Y(pt, frameCenter))) return null;
            var source = new LineOfPlane1X0Y((PointOfPlane1X0Y)strg.TempObjects.First(), new PointOfPlane1X0Y(pt, frameCenter), frameCenter, can.PlaneX0Y);
            source.Name = strg.TempObjects.First().Name;
            strg.TempObjects.Clear();
            return source;
        }
    }
}
