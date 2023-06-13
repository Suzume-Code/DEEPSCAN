using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using System.Timers;
using System.Windows.Input;
using System.Threading.Tasks;
using System.Collections;
using System.Drawing.Drawing2D;


namespace DEEPSCAN {

    public partial class Ocean : Form {

        private readonly int screen_width = 600;
        private readonly int screen_height = 700;
        
        private PictureBox background = new PictureBox();
        private Bitmap background_bitmap;

        private BattleShip battleship;
        private ArrayList submarines = new ArrayList();
        private readonly int enemies_count = 6;
        
        private int game_score = 0;

        private System.Timers.Timer projection_timer;
        private Label temp = new Label();
        private Label gameover = new Label();

        private readonly int fps = 1000 / 60;


        public Ocean() {
            
            //
			this.Text = "DEEPSCAN";
			this.Icon = new Icon(@"app.ico");
			this.MinimumSize = new Size(screen_width, screen_height);
			this.MaximumSize = new Size(screen_width, screen_height);
			this.ClientSize = new Size(screen_width, screen_height);
			this.ShowInTaskbar = true;
			this.Top = 16;
			this.Left = 16;
			this.StartPosition = FormStartPosition.WindowsDefaultLocation;  //FormStartPosition.Manual;
			this.Opacity = 1.0;
			this.FormBorderStyle = FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.AllowDrop = false;
			this.FormClosing += new FormClosingEventHandler(Form1_FormClosing);
            this.BackColor = Color.White;

            //
            background_bitmap = new Bitmap(screen_width, screen_height);

            //
            background.Location = new Point(0, 0);
            background.Size = new Size(screen_width, screen_height);
            this.Controls.Add(this.background);

            //
            this.temp.Location = new Point(20, 0);
            this.temp.Size = new Size(210, 32);
            this.temp.Font = new Font(Program.private_font_collection.Families[0], 24);
            this.temp.BackColor = Color.Transparent;
            this.temp.Text = UpdateScore(0);
            background.Controls.Add(this.temp);

            this.gameover.Location = new Point(0, (this.Height / 2) - 40);
            this.gameover.Size = new Size(this.Width, 40);
            this.gameover.Font = new Font(Program.private_font_collection.Families[0], 48);
            this.gameover.BackColor = Color.Transparent;
            this.gameover.TextAlign = ContentAlignment.MiddleCenter;
            this.gameover.Text = "";
            background.Controls.Add(this.gameover);

            //
            this.battleship = new BattleShip(this);

            //
            Random _rnd = new Random();
            for (int i = 0; i < enemies_count; i++) {
                submarines.Add(new Submarine(this, _rnd, i));
                Application.DoEvents();
            }

            // タイマー設定
            this.projection_timer = new System.Timers.Timer();
            this.projection_timer.Interval = fps;
            this.projection_timer.Elapsed += this.Projection_Tick;  
            this.projection_timer.Enabled = false;

            //GameStart();
        }


        public void GameStart() {

            this.projection_timer.Enabled = true;
        }


        private void Form1_FormClosing(object sender, FormClosingEventArgs e) {

            this.projection_timer.Enabled = false;
        }


        private void Projection_Tick(object sender, EventArgs e) {

            bool isRightKeyDown = IsKeyDown(Keys.Right);
            bool isLeftKeyDown = IsKeyDown(Keys.Left);        
            bool isZKeyDown = IsKeyDown(Keys.Z);
            bool isXKeyDown = IsKeyDown(Keys.X);

            using (Graphics g = Graphics.FromImage(background_bitmap)) {
                
                g.FillRectangle(Brushes.LightGray, new Rectangle(0, 0, screen_width, 141));

                Rectangle bg_rect = new Rectangle(0, 141, screen_width, screen_height - 141);
                LinearGradientBrush gb = new LinearGradientBrush(
                        bg_rect, 
                        Color.DodgerBlue, 
                        Color.DarkBlue, 
                        LinearGradientMode.Vertical);
                g.FillRectangle(gb, bg_rect);

                int pos = battleship.Move(g, 
                        isRightKeyDown, 
                        isLeftKeyDown);
                battleship.CreateMine(isZKeyDown, isXKeyDown);
                
                Application.DoEvents();

                foreach (Submarine submarine in submarines) {
                    submarine.Move(g);

                    Application.DoEvents();
                }
            }
            
            this.background.Image = background_bitmap;

            Rectangle rect = battleship.GetRect();
            foreach (Submarine submarine in submarines) {
                if (submarine.Collision(rect)) {
                    this.projection_timer.Enabled = false;
                    DrawGameOverString();
                }
            }

            // 爆雷と潜水艦の衝突判定
            game_score += battleship.Collision(submarines);
            
            // スコアの更新
            temp.Text = UpdateScore(game_score);
        }


        private void DrawGameOverString() {

            // ゲームオーバーメッセージ
            this.gameover.ForeColor = Color.White;
            this.gameover.Text = "GAME OVER";
        }


		private static KeyStates GetKeyState(Keys key) {

			KeyStates state = KeyStates.None;

			short retVal = User32.GetKeyState((int)key);
			if ((retVal & 0x8000) == 0x8000)
				state |= KeyStates.Down;

			if ((retVal & 1) == 1)
				state |= KeyStates.Toggled;

			return state;
		}


		private bool IsKeyDown(Keys key) {

			return KeyStates.Down == (GetKeyState(key) & KeyStates.Down);
		}


        private string UpdateScore(int score) {

            // 表示用スコアの表示
            return "SCORE " + score.ToString("00000") + "0";
        }
    }
}
