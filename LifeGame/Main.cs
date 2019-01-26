using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LifeGame
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }

        //横向
        public const int gameX = 150;

        //纵向
        public const int gameY = 80;

        //实时状态
        public static bool[,] state = new bool[gameX, gameY];

        /// <summary>
        /// 画背景
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Main_Paint(object sender, PaintEventArgs e)
        {
            Pen pen = new Pen(Color.Black);
            Graphics g = e.Graphics;
            g.Clear(Color.White);
            for (int i = 0; i <= gameX * 10; i += 10)
            {
                Point a = new Point(i, 0);
                Point b = new Point(i, gameY * 10);
                g.DrawLine(pen, a, b);
            }
            for (int i = 0; i <= gameY * 10; i += 10)
            {
                Point a = new Point(0, i);
                Point b = new Point(gameX * 10, i);
                g.DrawLine(pen, a, b);
            }
        }

        /// <summary>
        /// 点击变色
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Main_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
            {
                return;
            }
            Postion pos = new Postion(e.X / 10, e.Y / 10);
            if (pos.x >= gameX || pos.y >= gameY)
            {
                return;
            }
            state[pos.x, pos.y] = !state[pos.x, pos.y];
            Graphics g = this.CreateGraphics();
            SolidBrush sb = new SolidBrush(state[pos.x, pos.y] ? Color.Black : Color.White);
            g.FillRectangle(sb, pos.rectangle);
            g.Dispose();
        }

        /// <summary>
        /// 演化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Main_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != 13)
            {
                return;
            }
            bool[,] s = new bool[gameX, gameY];
            for (int x = 0; x < gameX; x++)
            {
                for (int y = 0; y < gameY; y++)
                {
                    Postion p = new Postion(x, y);
                    s[x, y] = p.setNewState();

                    Graphics g = this.CreateGraphics();
                    SolidBrush sb = new SolidBrush(s[x, y] ? Color.Black : Color.White);
                    g.FillRectangle(sb, p.rectangle);
                    g.Dispose();
                }
            }

            state = s;
        }
    }

    public class Postion
    {
        public Postion(int x, int y)
        {
            this.x = x;
            this.y = y;
            this.rectangle = new Rectangle(this.x * 10 + 1, this.y * 10 + 1, 9, 9);
        }

        //横向
        public int x { get; set; }

        //纵向
        public int y { get; set; }

        //方块
        public Rectangle rectangle { get; set; }

        //演化
        public bool setNewState()
        {
            int count = 0;
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    if (i == 0 && j == 0)
                        continue;
                    if (this.x + i < 0 || this.x + i >= Main.gameX || this.y + j < 0 || this.y + j >= Main.gameY)
                        continue;
                    if (Main.state[this.x + i, this.y + j])
                        count++;
                }
            }

            if (count == 3)
                return true;
            else if (count == 2)
                return Main.state[this.x, this.y];
            else
                return false;
        }
    }
}
