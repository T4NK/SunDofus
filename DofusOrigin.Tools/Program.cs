using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DofusOrigin
{
    static class Program
    {
        public static int willOpen = -1;

        private static MainFrame m_mainFrame;
        public static Frames.ItemsModifier m_itemsModifier;
        public static Frames.Database m_databaseHandler;
        public static Frames.DeleteItem m_itemDeleter;
        public static Frames.OpenItem m_itemOpener;

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            m_mainFrame = new MainFrame();
            m_itemsModifier = new Frames.ItemsModifier();
            m_databaseHandler = new Frames.Database();
            m_itemDeleter = new Frames.DeleteItem();
            m_itemOpener = new Frames.OpenItem();

            Application.Run(m_mainFrame);
        }
    }
}
