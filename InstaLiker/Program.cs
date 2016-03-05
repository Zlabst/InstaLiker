using System;
using System.Threading;
using System.Windows.Forms;

namespace InstaLiker
{
    internal static class Program
    {
        public static FrmMain FrmMain;

        /// <summary>
        ///     Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            FrmMain = new FrmMain();

            using (var myLock = new Mutex(false, "InstaLiker"))
            {
                if (myLock.WaitOne(3000, false))
                {
                    Application.Run(FrmMain);
                }
                else
                {
                    MessageBox.Show("Programm is open", Application.ProductName, MessageBoxButtons.OK,
                        MessageBoxIcon.Exclamation);
                }
            }
        }
    }
}