using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SunDofus.Tools
{
    static class Program
    {
        private static MainFrame m_mainFrame;
        public static Frames.ItemsModifier m_itemsmodifier;

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            m_mainFrame = new MainFrame();
            m_itemsmodifier = new Frames.ItemsModifier();

            Application.Run(m_mainFrame);
        }
    }
}
