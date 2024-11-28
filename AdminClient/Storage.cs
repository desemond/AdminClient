using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.WebSockets;

namespace AdminClient
{
    static internal class Storage
    {
        static public ClientWebSocket currentWebSocket { get; set; }
        static public List<ClientLevel> AllClients { get; set; }

    }
}
