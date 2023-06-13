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

    public class DropMine {

        BitmapImage BITMAPIMAGE = BitmapImage.GetInstance();

        private int move_range_bototm = 0;

        private int picture_left = 0;
        private int picture_top = 0;
        private int picture_width = 24;
        private int picture_height = 24;
        private int life_count = 0;
        private bool picture_enable = false;
        private bool picture_visible = false;

        private readonly int step = 4;

        private readonly int[] explosion_pattern = {4, 8, 16, 32, 48, 64, 80, 64, 54, 48, 36, 28, 20, 16, 12, 4};


        public DropMine(Control owner) {

            this.move_range_bototm = owner.Height + 40;

            this.picture_left = -100;
            this.picture_top = -100;
            this.picture_width = BITMAPIMAGE.DropMine.Width;
            this.picture_height = BITMAPIMAGE.DropMine.Height;
        }


        public void CreateMine(int left, int top) {

            if (this.picture_enable)
                return;
            
            // 開始位置設定
            picture_left = left;
            picture_top = top;

            picture_enable = true;
            picture_visible = true;

            life_count = 0;
        }


        public void Move(Graphics g) {

            if (!picture_enable)
                return;
            
            if (move_range_bototm < picture_top) {
                picture_enable = false;
                picture_visible = false;
                return;
            }

            Rectangle drawrect;

            if (!picture_visible) {
                this.life_count++;
                if (life_count >= 16) {
                    picture_enable = false;
                    return;
                }
                int explosion_picture_left = picture_left - (explosion_pattern[this.life_count - 1] / 2);
                int explosion_picture_top = picture_top - (explosion_pattern[this.life_count - 1] / 2);
                int explosion_picture_width = explosion_pattern[this.life_count - 1];
                int explosion_picture_height = explosion_pattern[this.life_count - 1];
                drawrect = new Rectangle(explosion_picture_left, explosion_picture_top, explosion_picture_width, explosion_picture_height);
                g.DrawImage(BITMAPIMAGE.Explosion, drawrect);
                return;
            }


            picture_top += step;

            // 機雷の描画
            drawrect = new Rectangle(picture_left, picture_top, picture_width, picture_height);
            g.DrawImage(BITMAPIMAGE.DropMine, drawrect);
        }


        public bool GetReady() {

            return !picture_visible;
        }

    
        public Rectangle GetRect() {

            return new Rectangle(picture_left, picture_top, picture_width, picture_height);
        }


        public void Explosion() {

            picture_visible = false;
            this.life_count = 0;
        }        
    }
}
