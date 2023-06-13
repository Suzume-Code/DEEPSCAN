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


namespace DEEPSCAN {

    public partial class Settings : Form {

		BitmapImage BITMAPIMAGE = BitmapImage.GetInstance();

		private Ocean ocean;

        private Button LoadButton = new Button();
		private Button QuitButton = new Button();
		private PictureBox WelcomePicture = new PictureBox();
		private Label InfoLabel = new Label();
		

        public Settings() {

            //
			this.Text = "DEEPSCAN";
			this.Icon = new Icon(@"app.ico");
			this.MinimumSize = new Size(300, 300);
			this.MaximumSize = new Size(300, 300);
			this.ClientSize = new Size(300, 300);
			this.ShowInTaskbar = true;
			this.Top = 16;
			this.Left = 16;
			this.StartPosition = FormStartPosition.WindowsDefaultLocation;
			this.Opacity = 1.0;
			this.FormBorderStyle = FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.AllowDrop = false;
			this.FormClosing += new FormClosingEventHandler(Form1_FormClosing);

			//
			this.InfoLabel.Size = new Size(260, 20);
			this.InfoLabel.Location = new Point(10, 0);
			this.InfoLabel.Text = "かわいいフリー素材集 いらすとや";
			this.InfoLabel.Font = new Font("Meiryo UI", 9);
			this.InfoLabel.BackColor = Color.Transparent;
			this.Controls.Add(this.InfoLabel);

			//
			this.LoadButton.Size = new Size(100, 40);
			this.LoadButton.Location = new Point(30,220);
			this.LoadButton.Text = "ゲーム開始";
			this.LoadButton.Click += new EventHandler(LoadButton_Click);
			this.Controls.Add(this.LoadButton);

			//
			this.QuitButton.Size = new Size(100, 40);
			this.QuitButton.Location = new Point(160,220);
			this.QuitButton.Text = "ゲーム終了";
			this.QuitButton.Click += new EventHandler(QuitButton_Click);
			this.Controls.Add(this.QuitButton);

			//
			this.WelcomePicture.Size = new Size(260, 260);
			this.WelcomePicture.Location = new Point(10, 0);
			this.WelcomePicture.Image = BITMAPIMAGE.DeepScan;
			this.WelcomePicture.SizeMode = PictureBoxSizeMode.Zoom;
			this.Controls.Add(this.WelcomePicture);

        }
        

        private void LoadButton_Click(object sender, EventArgs e) {

			ocean = new Ocean();
			this.ocean.Show();
			this.ocean.GameStart();
        }


        private void QuitButton_Click(object sender, EventArgs e) {

			this.Close();
        }


        private void Form1_FormClosing(object sender, FormClosingEventArgs e) {
        }


    }
}
