using System;
using Renci.SshNet;

namespace PiholeController.Public
{
    public class SshConnectionManager : IDisposable
    {
        private SshClient _client;
        private ConnectionInfo _connectionInfo;

        public bool IsConnected => _client?.IsConnected ?? false;

        public void Configure(string host, int port, string username, string password)
        {
            var auth = new PasswordAuthenticationMethod(username, password);
            _connectionInfo = new ConnectionInfo(host, port, username, auth);
        }

        public void Connect()
        {
            if (_connectionInfo == null)
                throw new InvalidOperationException("Connection not configured.");

            _client = new SshClient(_connectionInfo);
            _client.Connect();
        }

        public string RunCommand(string command)
        {
            if (!IsConnected)
                throw new InvalidOperationException("SSH is not connected.");

            var result = _client.RunCommand(command);
            return result.Result;
        }

        public void Disconnect()
        {
            if (_client != null && _client.IsConnected)
                _client.Disconnect();
        }

        public void Dispose()
        {
            try { _client?.Dispose(); } catch { }
        }
    }
}
