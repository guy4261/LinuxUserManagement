
namespace HadoopUserManagement.Commands
{
    /*
     * Execution of a pscp (Putty SCP) command.
     */
    class PscpCommand : Command
    {
        private string file_to_copy;
        public PscpCommand(string file_to_copy)
            : base("pscp.exe")
        {
            this.file_to_copy = file_to_copy;
        }

        protected override string _prepareArguments(Connection conn)
        {
            var arguments = string.Format("-pw {0} {1} {2}@{3}:{4}",
                conn.Password,
                this.file_to_copy,
                conn.Username,
                conn.Server,
                conn.Directory
                );
            if (!arguments.EndsWith("/"))
            {
                arguments += "/";
            }
            return arguments;
        }
    }
}
