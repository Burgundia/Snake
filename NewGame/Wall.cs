using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewGame
{
    public class Wall
    {
        public RectPoint[] Blocks;
        public Image img { get; set; }

        public Wall()
        {
            Blocks = new RectPoint[3];
            for (var i = 0; i < Blocks.Length; i++)
                Blocks[i] = new RectPoint();
            img = new Bitmap("block.png");
        }
    }
}
