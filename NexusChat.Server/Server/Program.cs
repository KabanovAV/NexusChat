using Server;

ChatServer tcpServer = new();
await tcpServer.Listener();
