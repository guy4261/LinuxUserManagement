using System;
using HadoopUserManagement.Commands;

namespace HadoopUserManagement
{
    class Connection
    {

        public string Server;
        public string Username;
        public string Password;
        public string Directory;

        public Connection(string server, string username, string password, string directory = null)
        {
            this.Server = server;
            this.Username = username;
            this.Password = password;

            if ((directory == null) || (directory.Length == 0))
            {
                if (username.Equals("root"))
                {
                    directory = "/root/";
                }
                else
                {
                    directory = string.Format("/home/{0}/", username);
                }
            }
            this.Directory = directory;
        }

        public bool TestConnection()
        {
            var cmd = new PlinkCommand(
                string.Format("touch {0}dolphins",
                this.Directory)
                );

            int exitCode = cmd.execute(this);
            return (exitCode == 0) ? true : false;
        }


    }
}
