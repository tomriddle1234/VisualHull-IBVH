using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

using System.ComponentModel;
using System.Data;
using System.Drawing;

using System.Text;

//using Emgu.CV;//PS:调用的Emgu dll   
//using Emgu.CV.Structure;
//using Emgu.Util;
using System.Threading; 

namespace WindowsFormsApplication3
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
