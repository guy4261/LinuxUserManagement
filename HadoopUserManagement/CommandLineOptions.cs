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
            HelpText = "add/delete.")]
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
          HelpText = "Password for new login..")]
        public string Group { get; set; }

        #endregion


        #region Batch command

        [Option('b', "batch",
            HelpText = "Filename with list of user,password,group triplets")]
        public string Batch { get; set; }

        #endregion


        [ParserState]
        public IParserState LastParserState { get; set; }

        [HelpOption]
        public string GetUsage()
        {
            string usage = HelpText.AutoBuild(this,
              (HelpText current) => HelpText.DefaultParsingErrorsHandler(this, current));
            return usage;
        }

    }
}
