using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SilverSock;
using System.Timers;

namespace realm.Network.Authentication
{
    class AuthenticationsLinks
    {
        List<AuthenticationClient> m_clients;

        public AuthenticationsLinks()
        {
            m_clients = new List<AuthenticationClient>();
        }

        public void Send(string _message)
        {
            foreach (var client in m_clients)
                client.Send(_message);
        }

        public void Start()
        {
            foreach (var client in Database.Cache.AuthsCache.m_auths)
                m_clients.Add(new AuthenticationClient(client));

            foreach (var client in m_clients)
                client.Start();
        }

        public void Update(List<Database.Models.Clients.AuthClientModel> _modelList)
        {
            foreach (var model in _modelList)
            {
                if (!m_clients.Any(x => x.m_model.m_id == model.m_id))
                {
                    var client = new AuthenticationClient(model);
                    m_clients.Add(client);
                    client.Start();
                }
            }

            //Si l'ID n'existe plus ? Supprimer un realm de m_clients ?
        }
    }
}
