using CommandLine;
using CommandLine.Text;

namespace HadoopUserManagement
{
    /*
     * https://commandline.codeplex.com/
     */
    class CommandLineOptions
    {

        #region Connection credentials on remote machines.

        [Option('a', "addr",// Required = true,
           HelpText = "Address of managed server.")]
        public string Address { get; set; }

        [Option('u', "username",// Required = true,
            HelpText = "Username that can modify managed server.")]
        public string Username { get; set; }

        [Option('p', "password",// Required = true,
            HelpText = "Password for the username.")]
        public string Password { get; set; }

        [Option('d', "directory",
            HelpText = "Writable directory on remote system.")]
        public string Directory { get; set; }

        #endregion

        #region Flag
        [Option('f', "flag", Required = true,
            HelpText = "{test/generate/add/remove} .")]
        public string Flag { get; set; }

        #endregion

        #region CLI arguments for generating a list of users.
        [Option('n', "number",
    HelpText = "Number of users to generate.")]
        public int NumberOfUsers { get; set; }

        [Option('x', "prefix",
    HelpText = "prefix for newly generated users.")]
        public string Prefix { get; set; }

        [Option('o', "output",
            HelpText = "output file for generated list of users.")]
        public string Output { get; set; }
        #endregion

        #region CLI arguments for adding new user.

        [Option('l', "login",
            HelpText = "login to be removed.")]
        public string Login { get; set; }

        [Option('c', "credential",
          HelpText = "login:password for new user.")]
        public string Credential { get; set; }

        [Option('g', "group", DefaultValue = "",
          HelpText = "Group for the added/removed login.")]
        public string Group { get; set; }

        #endregion


        #region Batch command

        [Option('b', "batch",
            HelpText = "Filename with list of user,password,group triplets")]
        public string Batch { get; set; }

        #endregion


        [ParserState]
        public IParserState LastParserState { get; set; }

        private string example_script = @"

::Take the 1st, 2nd and 3rd command line arguments as the
::server address, username and password for the managed remote machine.
SET addr=%1
SET username=%2
SET password=%3

::Test
HadoopUserManagement.exe -f test -a %addr% -u %username% -p %password%

::Add and remove single user
HadoopUserManagement.exe -f add -a %addr% -u %username% -p %password% -c fayak:fazal -g toyota
HadoopUserManagement.exe -f remove -a %addr% -u %username% -p %password% -f remove -l fayak

::Generate
HadoopUserManagement.exe -f generate -n 5 -x team -g subaru -o example_users.txt

::Add and remove batch of users
HadoopUserManagement.exe -f add -a %addr% -u %username% -p %password% -b example_users.txt
HadoopUserManagement.exe -f remove -a %addr% -u %username% -p %password% -b example_users.txt
";

        [HelpOption]
        public string GetUsage()
        {
            string usage = HelpText.AutoBuild(this,
              (HelpText current) => HelpText.DefaultParsingErrorsHandler(this, current));

            usage += "\nExample script:\n";
            usage +=  this.example_script;
            return usage;
        }

    }
}
