using System;
using System.IO;
using System.Windows.Forms;
using System.Drawing.Text;


namespace DEEPSCAN {

	class Program {

		public static PrivateFontCollection private_font_collection;


		[STAThread]
		static void Main(string[] args) {

			// 二重起動チェック
			if (System.Diagnostics.Process.GetProcessesByName(
				System.Diagnostics.Process.GetCurrentProcess().ProcessName).Length > 1)
				return;
			
			// ユーザーフォントの登録
			private_font_collection = new PrivateFontCollection();
			private_font_collection.AddFontFile("imgs/8-bit Arcade In.ttf");

			// ビットマップ設定
			BitmapImage BITMAPIMAGE = BitmapImage.GetInstance();

			// 画面表示
            Application.Run(new Settings());
		}
		//
		//
	}
	//
	//
}
