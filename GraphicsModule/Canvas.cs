﻿using System.Drawing;
using System.Windows.Forms;
using GraphicsModule.Configuration;

namespace GraphicsModule
{
    public class Canvas
    {
        public PictureBox PicBox { get; set; }
        public Settings Setting { get; set; }
        public Bitmap Mainbmp { get; set; }
        public Background Bckground { get; set; }
        public Graphics Graphics { get; set; }
        public RectangleF PlaneX0Y { get; set; }
        public RectangleF PlaneX0Z { get; set; }
        public RectangleF PlaneY0Z { get; set; }
        public Point CenterSystemPoint { get; set; }
        public Canvas(Settings setting, PictureBox picBox)
        {
            PicBox = picBox;
            Setting = setting;
            CalculateBackground();
            Mainbmp = new Bitmap(Bckground.BackBitmap);
            Mainbmp.MakeTransparent();
            Graphics = Graphics.FromImage(Mainbmp);
            CalculatePlanes(Bckground.Axis.Center);
            picBox.Image = (Image)Mainbmp.Clone();
            picBox.Refresh();
        }

        public void CalculateBackground()
        {
            CenterSystemPoint = new Point(PicBox.ClientSize.Width / 2, PicBox.ClientSize.Height / 2);
            Bckground = new Background(CenterSystemPoint, Setting, PicBox);
        }
        public void Refresh()
        {
            PicBox.Image = (Image)Mainbmp.Clone();
            PicBox.Refresh();
        }
        public void Update(Storage strg)
        {
            Mainbmp = new Bitmap(Bckground.BackBitmap);
            Mainbmp.MakeTransparent();
            Graphics = Graphics.FromImage(Mainbmp);
            strg.DrawObjects(Setting, Bckground.Axis.Center, Graphics);
            CalculatePlanes(Bckground.Axis.Center);
            PicBox.Image = (Image)Mainbmp.Clone();
            PicBox.Refresh();
        }
        private void CalculatePlanes(Point centerPoint)
        {
            PlaneX0Y = new RectangleF(0, centerPoint.Y, centerPoint.X, centerPoint.Y);
            PlaneX0Z = new RectangleF(0, 0, centerPoint.X, centerPoint.Y);
            PlaneY0Z = new RectangleF(centerPoint.X, 0, centerPoint.X, centerPoint.Y);
        }
    }
}
