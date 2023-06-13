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

    public class BattleShip {

        // インスタンス
        //private static BitmapImage singleton = null;

        BitmapImage BITMAPIMAGE = BitmapImage.GetInstance();

        public int picture_left = 0;
        public int picture_top = 60;
        private int picture_width = 0;
        private int picture_height = 0;

        private int move_range_left = 0;
        private int move_range_right = 0;
        private readonly int padding = 20;
        //private readonly int padding_top = 60;
        private readonly int step = 4;

        private ArrayList drop_mines = new ArrayList();
        private readonly int number_of_drop_mine = 5;

        private int wait_count = 0;


        public BattleShip(Control owner) {

            this.picture_width = BITMAPIMAGE.BattleShip.Width;
            this.picture_height = BITMAPIMAGE.BattleShip.Height;

            move_range_left = padding;
            move_range_right = owner.Width - picture_width - padding;

            picture_left = (owner.Width - picture_width) / 2;

            // 爆雷設定
            for (int i = 0; i < number_of_drop_mine; i++) {
                drop_mines.Add(new DropMine(owner));
                Application.DoEvents();
            }
        }


        public void CreateMine(bool left_side, bool right_side) {

            wait_count++;
            if (wait_count < 8)
                return;

            if (left_side) {
                int left = picture_left - 24;    // + 8;
                int top = picture_top + 40;
                foreach (DropMine drop_mine in drop_mines) {
                    if (drop_mine.GetReady()) {
                        drop_mine.CreateMine(left, top);
                        wait_count = 0;
                        break;
                    }
                }
            }
            if (right_side) {
                int left = picture_left + picture_width + 24;    // - 8;
                int top = picture_top + 40;
                foreach (DropMine drop_mine in drop_mines) {
                    if (drop_mine.GetReady()) {
                        drop_mine.CreateMine(left, top);
                        wait_count = 0;
                        break;
                    }
                }
            }
        }


        public int Move(Graphics g, bool isRightKeyDown, bool isLeftKeyDown) {

            if (isRightKeyDown)
                picture_left += step;
            if (isLeftKeyDown)
                picture_left -= step;
            if (picture_left < move_range_left)
                picture_left = move_range_left;
            if (picture_left > move_range_right)
                picture_left = move_range_right;

            g.DrawImage(BITMAPIMAGE.BattleShip, picture_left, picture_top, picture_width, picture_height);

            foreach (DropMine drop_mine in drop_mines) {
                drop_mine.Move(g);
            }

            return picture_left;
        }


        public Rectangle GetRect() {

            // 当たり判定用
            int inset = 4;
            return new Rectangle(picture_left + inset, picture_top, picture_width - inset - inset, picture_height);
        }


        public int Collision(ArrayList submarines) {

            int add_score = 0;
            int depth = 0;

            // 潜水艦のリストから当たり判定を行う
            // 潜水艦ｎ発の機雷と戦艦ｍ発の機雷を判断する
            foreach (Submarine submarine in submarines) {
                depth++;
                if (submarine.Visibled()) {
                    Rectangle sub_rect = submarine.GetRect();
                    foreach (DropMine drop_mine in drop_mines) {
                        if (!drop_mine.GetReady()) {
                            Rectangle mine_rect = drop_mine.GetRect();
                            if (sub_rect.IntersectsWith(mine_rect)) {
                                submarine.Explosion();
                                drop_mine.Explosion();
                                add_score += depth;
                                break;
                            }
                        }
                    }
                }
            }
            
            return add_score;
        }


    }
}
