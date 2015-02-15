
namespace HadoopUserManagement.Commands
{
    /*
    * Execution of a plink (Putty Link) command.
    */
    class PlinkCommand : Command
    {
        private string cmd;
        public PlinkCommand(string cmd)
            : base("plink.exe")
        {
            this.cmd = cmd;
        }

        protected override string _prepareArguments(Connection conn)
        {
            var arguments = string.Format("-pw {0} {1}@{2} {3}",
                conn.Password,
                conn.Username,
                conn.Server,
                this.cmd);
            return arguments;
        }
    }
}
