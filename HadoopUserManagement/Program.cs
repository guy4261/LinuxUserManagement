using System;
using System.Linq;
using System.Collections.Generic;
using HadoopUserManagement.Tasks;

namespace HadoopUserManagement
{

    class Program
    {

        static void CLI(string[] args)
        {
            var options = new CommandLineOptions();
            if (CommandLine.Parser.Default.ParseArguments(args, options))
            {

                /***************
                 * Validations *
                 ***************/
                var available_flags = new HashSet<string>(
                    new string[] { "add", "remove", "list", "test", "generate" } //"deploy"
                );

                if (!available_flags.Contains(options.Flag.ToLower()))
                {
                    Console.Error.WriteLine(
                        string.Format(
                        "Error: flag must be one of: {1}. Accepted unknown value: {0}",
                        options.Flag, string.Join(", ", available_flags)));
                    Environment.Exit(2);
                }

                /***************************************
                 * Switch-casing for flags and options *
                 ***************************************/

                string flag = options.Flag.ToLower();

                if (flag.Equals("generate"))
                {
                    if ((options.NumberOfUsers > 0) &&
                    (options.Prefix != null) &&
                    (options.Group != null) &&
                    (options.Output != null))
                    {
                        var task = new GenerateList();
                        int n = options.NumberOfUsers;
                        string prefix = options.Prefix;
                        string group = options.Group;
                        string output = options.Output;
                        task.generate_list(n, prefix, group, output);
                        return;
                    }
                    else
                    {
                        Console.Error.WriteLine("Missing arugments.");
                        Environment.Exit(5);
                    }

                }

                var connection = new Connection(options.Address,
                    options.Username,
                    options.Password,
                    options.Directory);

                if (flag.Equals("test"))
                {
                    var test_results = connection.TestConnection();
                    if (test_results)
                    {
                        Console.WriteLine("Successfully connected to remote machine!");
                        Environment.Exit(0);
                    }
                    else
                    {
                        Console.Error.WriteLine("Connection test failed! Exiting...");
                        Environment.Exit(1);
                    }
                }

                if (flag.Equals("list"))
                {
                    var listTask = new ListUsers(connection);
                    listTask.listUsers();
                    return;
                }


                if ((options.Username.Equals(options.Login)) ||
                    ((options.Credential != null) && (options.Credential.StartsWith(options.Username + ":"))))
                {
                    Console.Error.WriteLine("Username and Login must be different!");
                    Environment.Exit(3);
                }

                string batch = options.Batch;
                bool batchOperation = (batch != null && batch.Length > 0);
                if (flag.Equals("deploy"))
                {
                    var task = new Deploy(connection);
                    task.deploy();
                }

                if (flag.Equals("add"))
                {
                    if (batchOperation)
                    {
                        //add multiple users
                        var task = new AddBatchUsers(connection);
                        task.addUsersBatch(options.Batch);
                    }
                    else
                    {
                        //add single user

                        if ((options.Credential == null) ||
                        (options.Group == null))
                        {
                            Console.Error.Write("Missing arguments to add single user!");
                            Environment.Exit(8);
                        }

                        var credential = options.Credential;
                        if (credential.Count(c => c.Equals(':')) != 1)
                        {
                            Console.Error.WriteLine("Credential (-c) must be of the form login:password .");
                            Environment.Exit(6);
                        }

                        string[] login_and_password = credential.Split(':');
                        var group = options.Group;
                        var cmd = new AddSingleUser(connection);
                        cmd.addSingleUser(
                            login_and_password[0],
                            login_and_password[1],
                            group);
                    }
                }

                else if (flag.Equals("remove"))
                {
                    if (batchOperation)
                    {
                        //del multiple users
                        var cmd = new RemoveBatchUsers(connection);
                        cmd.RemoveUsersBatch(batch);
                    }
                    else
                    {
                        //del single user
                        if (options.Login == null)
                        {
                            Console.Error.Write("Missing arguments to remove single user!");
                            Environment.Exit(9);
                        }
                        var login = options.Login;
                        var cmd = new RemoveSingleUser(connection);
                        cmd.remove_single_user(login);
                    }
                }
            }
        }

        static void Main(string[] args)
        {
            CLI(args);
        }
    }
}
