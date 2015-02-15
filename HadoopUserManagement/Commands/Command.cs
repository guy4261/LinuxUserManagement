using System.Diagnostics;

namespace HadoopUserManagement
{
    /*
     * Command to be executed on the local machine.
     */
    abstract class Command
    {
        string executable;
        string arguments;

        public Command(string executable)
        {
            this.executable = executable;
            this.arguments = null;
        }

        protected abstract string _prepareArguments(Connection conn);

        public string PrepareArguments(Connection conn)
        {
            if (this.arguments == null)
            {
                this.arguments = this._prepareArguments(conn);
            }
            return this.arguments;
        }

        public int execute(Connection conn)
        {
            ProcessStartInfo processInfo = new ProcessStartInfo();
            processInfo.FileName = this.executable;
            processInfo.Arguments = PrepareArguments(conn);

            System.Console.WriteLine("$ " + processInfo.FileName + " " +processInfo.Arguments);

            processInfo.UseShellExecute = false;
            var process = Process.Start(processInfo);
            process.WaitForExit();
            return process.ExitCode;
        }

    }

}
