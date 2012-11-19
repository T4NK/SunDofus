using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SilverSock;

namespace realm.Network.Editors
{
    class EditorServer : SunDofus.Network.AbstractServer
    {
        public List<EditorClient> myClients;
        
        public EditorServer()
            : base(Utilities.Config.myConfig.GetStringElement("EditorIp"), Utilities.Config.myConfig.GetIntElement("EditorPort"))
        {
            myClients = new List<EditorClient>();

            this.SocketClientAccepted += new AcceptSocketHandler(this.AcceptedClient);
            this.ListeningServer += new ListeningServerHandler(this.Listening);
            this.ListeningServerFailed += new ListeningServerFailedHandler(this.ListenFailed);
        }

        void AcceptedClient(SilverSocket socket)
        {
            if (socket == null) return;

            Utilities.Loggers.InfosLogger.Write("New inputted @editor@ connection !");
            myClients.Add(new EditorClient(socket));
        }

        void Listening(string Remote)
        {
            Utilities.Loggers.StatusLogger.Write(string.Format("@EditorServer@ started on <{0}> !", Remote));
        }

        void ListenFailed(Exception e)
        {
            Utilities.Loggers.ErrorsLogger.Write(string.Format("Cannot start the @EditorServer@ because : {0}", e.ToString()));
        }
    }
}
