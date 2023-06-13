using System;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;


namespace DEEPSCAN {

    public class InfluenceMine {

		
        private struct SPRITE_SHIP {
			public PictureBox picture;		// イメージビットマップ
			public int Left;				// X軸
			public int Top;					// Y軸
			public int Width;
			public int Height;
			public int Step;				// 一回の移動数
			public int AxisX;
			public int AxisY;
			public bool Jumping;
			public bool Working;
		};

        public InfluenceMine() {

        }

    }
}
