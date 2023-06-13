using System;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Collections;


namespace DEEPSCAN {

    public partial class Submarine {

        BitmapImage BITMAPIMAGE = BitmapImage.GetInstance();

        private int move_range_left = 0;
        private int move_range_right = 0;
        private int move_range_top = 0;
        private int move_range_bottom = 0;
        private readonly int padding = 20;
        private readonly int padding_top = 200;
        private int step = 2;
        private readonly int start_step = 2; 
        
        public int picture_left = 100;
        public int picture_top = 60;
        private int picture_width = 60;
        private int picture_height = 48;

        private int picture_depth = 0;
        private int picture_arrow = 1;

        private Random rnd;
        private bool picture_enabled = false;
        private bool picture_visibled = false;

        private ArrayList sea_mines = new ArrayList();
        private readonly int number_of_sea_mine = 2;


        public Submarine(Control owner, Random rnd, int depth) {

            move_range_left = 0 - (picture_width + padding);
            move_range_right = owner.Width + padding;
            move_range_top = padding_top;
            move_range_bottom = owner.Height - picture_height;

            this.picture_width = BITMAPIMAGE.Submarine.Width;
            this.picture_height = BITMAPIMAGE.Submarine.Height;

			this.rnd = rnd;

            this.picture_depth = depth;
            
            // 潜水艦の移動方向を決定
            picture_arrow = CalculateOrientation(rnd);

            // 機雷の初期設定
            for (int i = 0; i < number_of_sea_mine; i++) {
                sea_mines.Add(new SeaMine(owner));
                Application.DoEvents();
            }
        }

        
        public void Move(Graphics g) {

            // 機雷移動
            // 潜水艦が使用可能状態であれば機雷を移動（作成済みなら）
            if (this.picture_enabled) {
                foreach (SeaMine sea_mine in sea_mines) {
                    sea_mine.Move(g);
                    int rr = rnd.Next(100);
                    if (rr > 98 && this.picture_visibled)
                        sea_mine.CreateMine(picture_left + (picture_width / 2), picture_top + (picture_height / 2));
                    Application.DoEvents();
                }
            }

            // 潜水艦作成
            if (!this.picture_enabled) {

                // 潜水艦の出現を乱数で決定
                int r = rnd.Next(100);
                if (r < 99)
                    return;

                // 潜水艦の移動方向を決定
                picture_arrow = CalculateOrientation(rnd);

                picture_top = (this.picture_depth * picture_height) + padding_top;
                if (picture_arrow == 1) {
                    picture_left = move_range_left;    
                } else {
                    picture_left = move_range_right;
                }
                this.step = this.start_step + (rnd.Next(2) * 2);

                picture_enabled = true;
                picture_visibled = true;
                return;
            }
            
            Application.DoEvents();

            if (this.picture_visibled) {
                if (picture_arrow == 1) {
                    picture_left += step;
                    if (picture_left > move_range_right) {
                        if (NumberOfRemainingMine() == 0) {
                            picture_enabled = false;
                            picture_visibled = false;
                        }
                    }
                    if (picture_visibled)
                        g.DrawImage(BITMAPIMAGE.Submarine_Reverse, picture_left, picture_top, picture_width, picture_height);
                } else {
                    picture_left -= step;
                    if (picture_left < move_range_left) {
                        if (NumberOfRemainingMine() == 0) {
                            picture_enabled = false;
                            picture_visibled = false;
                        }
                    }
                    if (picture_visibled)
                        g.DrawImage(BITMAPIMAGE.Submarine, picture_left, picture_top, picture_width, picture_height);
                }
            } else {
                if (NumberOfRemainingMine() == 0) {
                }

                this.picture_enabled = false;
            }
        }


        public bool Enabled() {

            return this.picture_enabled;
        }


        public bool Visibled() {

            return this.picture_visibled;
        }


        public bool Collision(Rectangle rect) {
            
            foreach (SeaMine sea_mine in sea_mines) {
                if (sea_mine.Collision(rect))
                    return true;
            }
            return false;
        }


        private int NumberOfRemainingMine() {

            int result = 0;
            foreach (SeaMine sea_mine in sea_mines) {
                if (sea_mine.MineState())
                    result++;
            }
            return result;
        }


        public Rectangle GetRect() {

            // 当たり判定用矩形を返却
            // inset値を大きくすると当たりやすくなる
            int inset = 10;
            return new Rectangle(picture_left + inset, picture_top + inset, picture_width - inset, picture_height - inset);
        }


        public void Explosion() {

            picture_visibled = false;
        }


        private int CalculateOrientation(Random rnd) {

            // 潜水艦の移動方向を決定
            // 1:右向き -1:左向き
            int num = rnd.Next(10);
            if (num % 2 == 0)
                return 1;
            else
                return -1;
        }


    }
}
