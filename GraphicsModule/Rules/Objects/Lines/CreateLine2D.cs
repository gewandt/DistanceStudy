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
    /// Создание 2Д линии
    /// </summary>
    public class CreateLine2D : ICreate
    {
        public void AddToStorageAndDraw(Point pt, Point frameCenter, Canvas can, DrawS settings, Storage strg)
        {
            var obj = Create(pt, frameCenter, can, settings, strg);
            if (obj == null) return;
            strg.AddToCollection(obj);
            can.Update(strg);
        }
        public Line2D Create(Point pt, Point frameCenter, Canvas can, DrawS settings, Storage strg)
        {
            var ptOfPlane = new Point2D(pt);
            if (strg.TempObjects.Count == 0)
            {
                ptOfPlane.SetName(GraphicsControl.NmGenerator.Generate());
                strg.TempObjects.Add(ptOfPlane);
                strg.DrawLastAddedToTempObjects(settings, frameCenter, can.Graphics);
            }
            else
            {
                if (Analyze.PointPos.Coincidence((Point2D)strg.TempObjects.First(), new Point2D(pt))) return null;
                var source = new Line2D((Point2D)strg.TempObjects.First(), new Point2D(pt), can.PicBox);
                source.SetName(strg.TempObjects.First().GetName());
                strg.TempObjects.Clear();
                return source;
            }
            return null;
        }
    }
}