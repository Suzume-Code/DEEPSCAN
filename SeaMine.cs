using System;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;


namespace DEEPSCAN {

    public class SeaMine {

        BitmapImage BITMAPIMAGE = BitmapImage.GetInstance();

        private int move_range_top = 0;

        private bool mine_visible = false;
        private int mine_state = 0;
        private int mine_left = -100;
        private int mine_top = -100;
        private readonly int mine_width = 24;
        private readonly int mine_height = 24;
        private readonly int padding_top = 60;
        private readonly int step = 4;
        private int life_count = 0;

        private List<Color> sea_mine_colors = new List<Color>();


        public SeaMine(Control owner) {
        
            this.move_range_top = padding_top + 70;

            //
            this.mine_visible = false;
            this.mine_state = 0;

            // 機雷のマーカー
            sea_mine_colors.Add(Color.FromArgb(0xff, 0x7f, 0x50));
            sea_mine_colors.Add(Color.FromArgb(0xff, 0x63, 0x47));
            sea_mine_colors.Add(Color.FromArgb(0xff, 0x45, 0x00));
            sea_mine_colors.Add(Color.FromArgb(0xff, 0x00, 0x00));
            sea_mine_colors.Add(Color.FromArgb(0xff, 0x8c, 0x00));
            sea_mine_colors.Add(Color.FromArgb(0xff, 0xff, 0x00));
        }


        public void CreateMine(int left, int top) {

            if (this.mine_visible)
                return;
            
            // 開始位置設定
            mine_left = left;
            mine_top = top;
            
            this.mine_state = 0;

            mine_visible = true;
        }


        public void Move(Graphics g) {

            //
            if (!this.mine_visible)
                return;
            
            // 機雷の浮上
            if (this.mine_state == 0) {
                mine_top -= step;
                if (this.mine_top < this.move_range_top) {
                    mine_top -= 4;
                    this.mine_state = 1;
                    this.life_count = 0;
                    return;
                }
                // 機雷の描画
                Rectangle drawrect = new Rectangle(mine_left, mine_top, mine_width, mine_height);
                g.DrawImage(BITMAPIMAGE.SeaMine, drawrect);
                // マーカーの描画
                this.life_count++;
                Color color = sea_mine_colors[life_count % 6];
                SolidBrush myBrush = new SolidBrush(color);
                g.FillRectangle(myBrush, new Rectangle(mine_left + 6, mine_top + 6, 4, 4));
            } else {

                this.life_count++;
                if (this.life_count > 8) {
                    this.mine_visible = false;
                    this.mine_state = 0;
                    this.life_count = 0;
                    return;
                }

                // 機雷爆発
                int h = (life_count * 4);
                Rectangle drawrect = new Rectangle(mine_left, mine_top - h, mine_width, mine_height + h);
                g.DrawImage(BITMAPIMAGE.SeaMineExplosion, drawrect);
            }
        }


        public bool Collision(Rectangle rect) {
            
            if (this.mine_state == 0)
                return false;
            if (this.life_count != 8)
                return false;
            Rectangle drawrect = new Rectangle(mine_left, mine_top - 40, mine_width, mine_height + 40);
            return rect.IntersectsWith(drawrect);
        }


        public bool MineState() {

            return this.mine_visible;
        }
    }
}
