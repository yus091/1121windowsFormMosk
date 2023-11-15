using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MoskTest
{
    public partial class Form1 : Form
    {

        static Graphics g;	  //繪圖裝置（一個就夠了）
        static int r = 10, r2 = 20;    //半徑，直徑
        
        static int width = 0, height = 0; // 球桌寬高
        public int downtime = 0; // 計算往下跑了多少次
        public bool ballout = false;
        int lab2 = 0; // 擊落球數
        public int mouseDownTime = 0;
        public double a;

        class ball
        {     //球 class(類別)
            int id;                                      //球編號
            public double x = 0, y = 0;    //球心 坐標
            public double left = 240, right = 60;
            public bool moveFlag = true;
            Color c;                                   //球顏色
            SolidBrush br;                        //刷子（畫球用）
            private double ang = 0; // 球 行進角度
            public double cosA, sinA; // coSine 行進角度， Sine 行進角度
            public double speed = 10; // 球行進速度
            public bool IsMoving; // 球是否在前進
            

            public void move()
            {
                if (IsMoving)
                {
                    x -= r * cosA;
                    y -= r * sinA;
                }
                    
                
                //x -= speed * cosA; // x 方向分量
                //y -= speed * sinA; // y 方向分量
                //(x - (12 * r) * cosA - 12), (int)(y - (12 * r) * sinA - 12)
            }
            public void moveRect()
            {
                if (moveFlag == true)
                {
                    left += 10;
                    if (left + 125 >= width)
                    {
                        moveFlag = false;
                    }
                }
                else
                {
                    left -= 10;
                    if (left <= 0)
                    {
                        moveFlag = true;
                    }
                }
                
            }

            public void moveRectDown()
            {
                right += 10;
            }

            public ball(int bx, int by, Color cc, int i)
            {  //建構者
                x = bx;                                 //球心 x 坐標
                y = by;                                 //球心 y 坐標
                c = cc;                                  //球顏色    
                br = new SolidBrush(cc);     //球顏色的刷子
                id = i;                                   //球編號
                IsMoving = false;
            }
            public void redraw(int x, int y)
            {      //畫 球物件 自己
                g.FillEllipse(br, (int)(x - (12 * r) * cosA - 12), (int)(y - (12 * r) * sinA - 12), 30, 30);
                //g.FillEllipse(br, (int)(x - (10 * r) * cosA), (int)(y - (10 * r) * sinA), r2, r2);          //畫橢圓（球刷子，左上角 坐標，直徑寬，直徑高）
            }
            public void draw()
            {      //畫 球物件 自己
                g.FillEllipse(br, (int)(x - (12 * r) * cosA - 12), (int)(y - (12 * r) * sinA - 12), 30, 30);
                //g.FillEllipse(br, (int)(x - (10 * r) * cosA), (int)(y - (10 * r) * sinA), r2, r2);          //畫橢圓（球刷子，左上角 坐標，直徑寬，直徑高）
            }
            public void setAng(double _ang)  // 角度改變
            {
                ang = _ang;  // 存 新角度
                cosA = Math.Cos(ang); // 重算 coSine
                sinA = Math.Sin(ang); // 重算 Sine
            }
            public void drawStick()
            {
                double r12 = 12 * r;
                Pen skyBluePen = new Pen(Brushes.DeepSkyBlue);
                skyBluePen.Width = 10.0F;
                g.DrawLine(skyBluePen,
                    //(float)(x - r12 * cosA), (float)(y - r12 * sinA),
                    //(float)(x - r * cosA), (float)(y - r * sinA)
                    (float)(x - r12 * cosA), (float)(y - r12 * sinA),
                    (float)(x - r * cosA), (float)(y - r * sinA)
                    );
               
            }

            public void drawOrangeStick()
            {
                double r12 = 20 * r;
                Pen OrangePen = new Pen(Color.Orange);
                OrangePen.Width = 60.0F;
                //g.DrawRectangle(OrangePen,
                //(float)(left), (float)(right),
                //(float)(right), (float)(right));
                using (SolidBrush orangrBrush = new SolidBrush(Color.Orange))
                {
                    g.FillRectangle(orangrBrush, (int)(left), (int)(right), 120, 60); //畫方塊（刷子，左上角 坐標，直徑寬，直徑高）
                }


            }

            public void rebound()
            {
                if (x < r || x > width - r)   // 出左右邊
                {
                    //setAng(Math.PI - ang);
                    x = ((12 * r) * cosA - 12);
                    y = ((12 * r) * sinA - 12);
                }
                else if (y < r || y > height - r)  // 出上下邊
                {
                   // setAng(-ang);
                    x = ((12 * r) * cosA - 12);
                    y = ((12 * r) * sinA - 12);
                }
            }

        }	//class ball 結束

        ball[] balls = new ball[4];

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            a = Math.Atan2(balls[0].y - e.Y, balls[0].x - e.X); // e：滑鼠點擊處座標
            for (int i = 1; i <= 3; i++)
            {
                if (!balls[i].IsMoving)
                {
                    balls[i].setAng(a); //存入母球行進角度
                }
                
            }
            mouseDownTime = (mouseDownTime + 1) % 4;
            balls[mouseDownTime].IsMoving = true;

            balls[0].setAng(a);
            panel1.Refresh(); // 重新繪畫轉動過的球桿
                              // g.DrawRectangle(Pens.HotPink, e.X - 2, e.Y - 2, 4, 4); //點擊點畫小方塊
            timer2.Enabled = true;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            panel1.Refresh();
            balls[0].moveRect();
            Hit(balls[0], balls[1]);
            Hit(balls[0], balls[2]);
            Hit(balls[0], balls[3]);

        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            for(int i = 1; i <= 3; i++)
            {
                if (balls[i].x < 30 || balls[i].x > width - r || balls[i].y < 30)  // 出左右上邊
                {
                    //(int)(x - (12 * r) * cosA - 12), (int)(y - (12 * r) * sinA - 12)
                    if (balls[1].IsMoving == false && balls[2].IsMoving == false && balls[3].IsMoving == false)
                    {
                        timer2.Stop();
                    }
                    
                    //double a = Math.Atan2(balls[0].y, balls[0].x); // e：滑鼠點擊處座標
                    balls[i].setAng(a); //存入母球行進角度
                    balls[i].x = 290;
                    balls[i].y = 350;
                    //balls[1].rebound();
                    balls[i].draw();
                    panel1.Refresh();
                    ballout = true;
                    balls[i].IsMoving = false;
                }
                else
                {
                    if(balls[i].IsMoving == true)
                    {
                        balls[i].move();
                    }
                    
                    panel1.Refresh();
                }
            }
            
            
        }

        private void Hit(ball b0, ball b1)
        {
            /*if ((b0.x <= b1.x + r) && (b0.y >= b1.y + r))
            {
                if((b0.x + 60 > b1.x + r) && (b0.y + 30 < b1.y + r))
                {
                    timer1.Stop();
                    timer3.Enabled = true;
                }
                
            }*/
            if ((b0.left <= b1.x - (12 * r) * b1.cosA) && (b0.right <= b1.y - (12 * r) * b1.sinA))
            {
                if ((b0.left + 120 >= b1.x - (12 * r) * b1.cosA - 12) && (b0.right + 60 >= b1.y - (12 * r) * b1.sinA - 12))
                {
                    timer1.Stop();
                    timer3.Enabled = true;
                    lab2 += 1;
                    label2.Text = lab2.ToString();
                }

            }
        }

        private void timer3_Tick(object sender, EventArgs e)
        {
            balls[0].moveRectDown();
            panel1.Refresh();
            downtime += 1;
            if(balls[0].right + 60 > height)
            {
                timer3.Stop();
                balls[0].right = balls[0].right - 10 * downtime;
                downtime = 0;
                timer1.Enabled = true;
            }
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            balls[0].drawOrangeStick();
            balls[0].drawStick();
            for(int i = 1; i <= 3; i++)
            {
                //balls[i].x = balls[i].x - i + 1;
                //balls[i].y = balls[i].y - i + 1;
                balls[i].draw();
            }
            
            
        }

        public Form1()
        {
            InitializeComponent();
            width = panel1.Width;
            height = panel1.Height;
            g = panel1.CreateGraphics();     //繪圖裝置 初始化
            balls[0] = new ball(width/2, 350, Color.DeepSkyBlue, 0);
            balls[0].setAng(Math.PI / 4);
            for(int i = 1; i <= 3; i++)
            {
                balls[i] = new ball(width / 2, 350, Color.DeepSkyBlue, 0);
                balls[i].setAng(Math.PI / 4);
            }
            
            timer1.Enabled = true;
        }
    }
}
