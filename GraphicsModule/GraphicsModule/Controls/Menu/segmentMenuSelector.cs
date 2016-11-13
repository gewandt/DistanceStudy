﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GraphicsModule.Controls.Menu
{
    public partial class SegmentMenuSelector : UserControl
    {
        private PictureBox pb;
        public SegmentMenuSelector(PictureBox pb)
        {
            InitializeComponent();
            this.pb = pb;
        }

        private void mainStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            Visible = false;
            GraphicsControl.Operations = null;
        }

        private void buttonSegment2D_Click(object sender, EventArgs e)
        {
            pb.Cursor = Cursors.Cross;
            GraphicsControl.SetObject = new CreateSegment2D();
        }

        private void buttonSegmentOfPlaneX0Y_Click(object sender, EventArgs e)
        {
            pb.Cursor = Cursors.Cross;
            GraphicsControl.SetObject = new CreateSegmentOfPlane1X0Y();
        }

        private void buttonSegmentOfPlane2X0Z_Click(object sender, EventArgs e)
        {
            pb.Cursor = Cursors.Cross;
            GraphicsControl.SetObject = new CreateSegmentOfPlane2X0Z();
        }

        private void buttonSegmentOfPlane3Y0Z_Click(object sender, EventArgs e)
        {
            pb.Cursor = Cursors.Cross;
            GraphicsControl.SetObject = new CreateSegmentOfPlane3Y0Z();
        }

        private void buttonSegment3D_Click(object sender, EventArgs e)
        {
            pb.Cursor = Cursors.Cross;
            GraphicsControl.SetObject = new CreateSegment3D();
        }

        private void GenerateSegment3D_Click(object sender, EventArgs e)
        {
            pb.Cursor = Cursors.Hand;
            GraphicsControl.SetObject = new GenerateSegment3D();
        }
    }
}
