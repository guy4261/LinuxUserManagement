using System;
using System.Collections.Generic;

namespace HadoopUserManagement
{
    class GenerateList : Task
    {
        private string allowedChars = "abcdefghijkmnopqrstuvwxyzABCDEFGHJKLMNOPQRSTUVWXYZ0123456789";

        private string generate_password()
        {
            string password = "";
            var rnd = new Random();
            for (int i = 0; i < PASSWORD_LENGTH; i++)
            {
                password += allowedChars[rnd.Next(0, PASSWORD_LENGTH)];
            }
            return password;
        }

        public GenerateList()
            : base(null)
        {

        }

        private const int PASSWORD_LENGTH = 8;
        public void generate_list(int n,
            string prefix,
            string group,
            string output)
        {
            int zfill = n.ToString().Length;
            var lines = new List<string>();
            for (int i = 1; i <= n; i++)
            {
                string username = prefix + i.ToString().PadLeft(zfill, '0');
                /*
                string password = System.Web.Security.Membership
                    .GeneratePassword(PASSWORD_LENGTH,
                    0).Replace(':','_');
                */
                string password = this.generate_password();
                var cells = new string[] { username, password, group };
                string line = string.Join(",", cells);
                lines.Add(line);
            }

            System.IO.File.WriteAllLines(output, lines);

        }

    }
}
