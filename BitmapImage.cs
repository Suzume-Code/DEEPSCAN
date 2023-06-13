using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;


namespace DEEPSCAN {

    public class BitmapImage {

        // インスタンス
        private static BitmapImage singleton = null;

        public Bitmap DeepScan;
        public Bitmap BattleShip;
        public Bitmap Submarine;
        public Bitmap Submarine_Reverse;
        public Bitmap Explosion;
        public Bitmap DropMine;
        public Bitmap SeaMine;
        public Bitmap SeaMineExplosion;

        
        public static BitmapImage GetInstance() {

            if (singleton == null) {
                singleton = new BitmapImage();
            }
            return singleton;
        }

        private BitmapImage() {

            try {
                DeepScan = new Bitmap("imgs/deepscan.png");
                
                BattleShip = new Bitmap("imgs/battleship.png");
                BattleShip.MakeTransparent(Color.White);
                
                Submarine = new Bitmap("imgs/submarine.png");
                Submarine.MakeTransparent(Color.White);
                
                // 反転処理（左向き→右向き）
                Submarine_Reverse = (Bitmap) Submarine.Clone();
                Submarine_Reverse.RotateFlip(RotateFlipType.RotateNoneFlipX);

                Explosion = new Bitmap("imgs/explosion.png");
                Explosion.MakeTransparent(Color.White);

                DropMine = new Bitmap("imgs/drop_mine.png");
                DropMine.MakeTransparent(Color.White);

                SeaMine = new Bitmap("imgs/sea_mine.png");
                SeaMine.MakeTransparent(Color.White);

                SeaMineExplosion = new Bitmap("imgs/sea_mine_explosion.png");
                SeaMineExplosion.MakeTransparent(Color.White);
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message);
                Environment.Exit(0x8020);
            }
        }


    }
}
