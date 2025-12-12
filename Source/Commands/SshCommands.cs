using System.Threading.Tasks;
using Renci.SshNet;

namespace PiholeController.Public
{
    public class SshCommands
    {
        private string _host;
        private int _port;
        private string _username;
        private string _password;

        public void SetCredentials(string host, int port, string username, string password)
        {
            _host = host;
            _port = port;
            _username = username;
            _password = password;
        }

        private SshClient CreateClient()
        {
            return new SshClient(_host, _port, _username, _password);
        }

        public async Task<bool> EnablePiHoleAsync()
        {
            return await Task.Run(() =>
            {
                try
                {
                    using var ssh = CreateClient();
                    ssh.Connect();
                    var cmd = ssh.RunCommand("sudo pihole enable");
                    ssh.Disconnect();
                    return cmd.ExitStatus == 0;
                }
                catch { return false; }
            });
        }

        public async Task<bool> DisablePiHoleForSecondsAsync(int seconds)
        {
            return await Task.Run(() =>
            {
                try
                {
                    using var ssh = CreateClient();
                    ssh.Connect();
                    var cmd = ssh.RunCommand($"sudo pihole disable {seconds}s");
                    ssh.Disconnect();
                    return cmd.ExitStatus == 0;
                }
                catch { return false; }
            });
        }

        public async Task<string> RunCustomCommandAsync(string command)
        {
            return await Task.Run(() =>
            {
                try
                {
                    using var ssh = CreateClient();
                    ssh.Connect();
                    var cmd = ssh.RunCommand(command);
                    ssh.Disconnect();
                    return cmd.Result;
                }
                catch
                {
                    return string.Empty;
                }
            });
        }
    }
}
