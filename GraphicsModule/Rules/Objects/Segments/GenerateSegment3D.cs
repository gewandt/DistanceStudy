﻿using System.Drawing;
using GraphicsModule.Configuration;
using GraphicsModule.Geometry.Objects.Segments;
using GraphicsModule.Interfaces;

namespace GraphicsModule.Rules.Objects.Segments
{
    public class GenerateSegment3D : ICreate
    {
        private Segment3D _source;
        public void AddToStorageAndDraw(Point pt, Point frameCenter, Canvas canvas, DrawSettings settings, Storage storage)
        {
            new SelectSegmentOfPlane().Execute(pt, storage, canvas);
            if (storage.SelectedObjects.Count > 1)
            {
                if (ReferenceEquals(storage.SelectedObjects[0].GetType(), storage.SelectedObjects[1].GetType()))
                {
                    storage.SelectedObjects.Remove(storage.SelectedObjects[0]);
                    canvas.Update(storage);
                    return;
                }
                if ((_source = Segment3D.Create(storage.SelectedObjects)) != null)
                {
                    storage.Objects.Remove(storage.SelectedObjects[0]);
                    storage.Objects.Remove(storage.SelectedObjects[1]);
                    storage.SelectedObjects.Clear();
                    canvas.Update(storage);
                    storage.AddToCollection(_source);
                    _source = null;
                    storage.DrawLastAddedToObjects(settings, frameCenter, canvas.Graphics);
                }
                else
                {
                    storage.SelectedObjects.RemoveAt(storage.SelectedObjects.Count - 1);
                    canvas.Update(storage);
                }
            }
            else
            {
                canvas.Update(storage);
            }
        }
    }
}
