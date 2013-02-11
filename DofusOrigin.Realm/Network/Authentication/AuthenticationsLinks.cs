using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SilverSock;
using System.Timers;

namespace DofusOrigin.Network.Authentication
{
    class AuthenticationsLinks
    {
        private List<AuthenticationClient> Clients;

        public AuthenticationsLinks()
        {
            Clients = new List<AuthenticationClient>();
        }

        public void Send(string message)
        {
            foreach (var client in Clients)
                client.Send(message);
        }

        public void Start()
        {
            foreach (var client in Database.Cache.AuthsCache.AuthsList)
                Clients.Add(new AuthenticationClient(client));

            foreach (var client in Clients)
                client.Start();
        }

        public void Update(List<Database.Models.Clients.AuthClientModel> modelList)
        {
            foreach (var model in modelList)
            {
                if (!Clients.Any(x => x.Model.ID == model.ID))
                {
                    var client = new AuthenticationClient(model);

                    lock(Clients)
                        Clients.Add(client);

                    client.Start();
                }
            }

            //Si l'ID n'existe plus ? Supprimer un realm de m_clients ?
        }
    }
}
