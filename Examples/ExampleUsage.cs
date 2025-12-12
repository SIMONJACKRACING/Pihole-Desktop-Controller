using System;
using System.Threading.Tasks;
using PiholeController.Public;

namespace ExampleUsage
{
    class Program
    {
        static async Task Main(string[] args)
        {
            string host = "192.168.1.100";
            int port = 22;
            string username = "pi";
            string password = "yourpassword";

            var ssh = new SshConnectionManager();
            ssh.Configure(host, port, username, password);

            try
            {
                ssh.Connect();
                Console.WriteLine("Connected to Pi-hole.");

                string status = ssh.RunCommand("pihole status");
                Console.WriteLine("Pi-hole status:");
                Console.WriteLine(status);

                ssh.Disconnect();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Connection failed: " + ex.Message);
            }

            var cmd = new SshCommands();
            cmd.SetCredentials(host, port, username, password);

            Console.WriteLine("Enabling Pi-hole...");
            bool enabled = await cmd.EnablePiHoleAsync();
            Console.WriteLine("Enable result: " + enabled);

            Console.WriteLine("Disabling Pi-hole for 20 seconds...");
            bool disabled = await cmd.DisablePiHoleForSecondsAsync(20);
            Console.WriteLine("Disable result: " + disabled);

            Console.WriteLine("Custom command:");
            string custom = await cmd.RunCustomCommandAsync("pihole -v");
            Console.WriteLine(custom);
        }
    }
}
