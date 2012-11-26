using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SilverSock;

namespace realm.Network.Editors
{
    class EditorServer : SunDofus.Network.TCPServer
    {
        public List<EditorClient> m_clients;
        
        public EditorServer()
            : base(Utilities.Config.m_config.GetStringElement("EditorIp"), Utilities.Config.m_config.GetIntElement("EditorPort"))
        {
            m_clients = new List<EditorClient>();

            this.SocketClientAccepted += new AcceptSocketHandler(this.OnAcceptedClient);
            this.ListeningServer += new ListeningServerHandler(this.OnListeningServer);
            this.ListeningServerFailed += new ListeningServerFailedHandler(this.OnListeningFailedServer);
        }

        void OnAcceptedClient(SilverSocket _socket)
        {
            if (_socket == null) return;

            Utilities.Loggers.m_infosLogger.Write("New inputted @editor@ connection !");
            m_clients.Add(new EditorClient(_socket));
        }

        void OnListeningServer(string _remote)
        {
            Utilities.Loggers.m_statusLogger.Write(string.Format("@EditorServer@ started on <{0}> !", _remote));
        }

        void OnListeningFailedServer(Exception _exception)
        {
            Utilities.Loggers.m_errorsLogger.Write(string.Format("Cannot start the @EditorServer@ because : {0}", _exception.ToString()));
        }
    }
}
