using System;
using System.Runtime.InteropServices;

namespace DEEPSCAN {

    internal class User32 {
    
    [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern short GetKeyState(int keyCode);
    
    }
}
