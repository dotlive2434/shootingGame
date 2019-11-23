using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Drawing;
using NAudio.Wave;
using System.IO;

namespace shootinggame
{
    class shooter
    {
        public Image img;
        public Point Point;

        public shooter()
        {
            img = Image.FromFile(@"..\..\Asset\chieri0.png");
        }
    }
    partial class mainProgram : Form
    {
        private KeyManager km;
        private shooter chieri;
        private int vx, vy;

        public void game()
        {
            Point p = new Point(0, 0);
            chieri = new shooter();
            chieri.Point = p;

            km = new KeyManager();

            vx = 10;
            vy = 10;

            /*Timer tm = new Timer();
            tm.Interval = 33;
            tm.Start();*/

            panel[gameNumber].Paint += new PaintEventHandler(DrawShooter);
            (panel[gameNumber] as Control).KeyDown += new KeyEventHandler(keyMove);
            (panel[gameNumber] as Control).KeyUp += new KeyEventHandler(keyReset);
            //tm.Tick += new EventHandler(tm_Tick);
        }

        public void DrawShooter(Object sender, PaintEventArgs e)
        {
            int x, y;
            Graphics g = e.Graphics;
            x = chieri.Point.X;
            y = chieri.Point.Y;
            g.DrawImage(chieri.img, x, y, 50, 50);
        }

        public void keyMove(Object sender, KeyEventArgs e)
        {
            Point p = chieri.Point;
            double sqrt2 = Math.Sqrt(2);

            km.Add(e.KeyCode);

            if (km.Contains(new Keys[] { Keys.Left, Keys.Up }))
            {
                p.X -= (int)((float)vx / sqrt2);
                p.Y -= (int)((float)vy / sqrt2);
            }
            if (km.Contains(new Keys[] { Keys.Left, Keys.Down }))
            {
                p.X -= (int)((float)vx / sqrt2);
                p.Y += (int)((float)vy / sqrt2);
            }
            if (km.Contains(new Keys[] { Keys.Right, Keys.Up }))
            {
                p.X += (int)((float)vx / sqrt2);
                p.Y -= (int)((float)vy / sqrt2);
            }
            if (km.Contains(new Keys[] { Keys.Right, Keys.Down }))
            {
                p.X += (int)((float)vx / sqrt2);
                p.Y += (int)((float)vy / sqrt2);
            }
            if (km.Contains(new Keys[] { Keys.Right }))
            {
                p.X += vx;
            }
            if (km.Contains(new Keys[] { Keys.Up }))
            {
                p.Y -= vy;
            }
            if (km.Contains(new Keys[] { Keys.Down }))
            {
                p.Y += vy;
            }
            if (km.Contains(new Keys[] { Keys.Left }))
            {
                p.X -= vx;
            }

            chieri.Point = p;
            panel[gameNumber].Invalidate();
        }

        public void keyReset(Object sender, KeyEventArgs e)
        {
            km.Remove(e.KeyCode);
        }


        /*public void tm_Tick(Object sender, EventArgs e)
        {
            Point p = chieri.Point;

            if (p.X < 0 || p.X > this.ClientSize.Width - 10) vx = -vx;
            if (p.Y < 0 || p.Y > this.ClientSize.Height - 10) vy = -vy;
            p.X += vx;
            p.Y += vy;

            chieri.Point = p;
            panel[gameNumber].Invalidate();
        }*/
    }

    public class KeyManager
    {
        private List<Keys> list = new List<Keys>();

        public void Add(Keys key)
        {
            if (!list.Contains(key))
            {
                list.Add(key);
            }
        }

        public void Remove(Keys k)
        {
            list.Remove(k);
        }

        public bool Contains(Keys[] keyList)
        {
            foreach (Keys key in keyList)
            {
                if (!list.Contains(key)) return false;
            }
            return true;
        }
    }

}