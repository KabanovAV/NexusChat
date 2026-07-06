using Server;

ServerObject tcpServer = new();
await tcpServer.Listener();
