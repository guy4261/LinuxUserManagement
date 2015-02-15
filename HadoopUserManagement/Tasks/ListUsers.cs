using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HadoopUserManagement.Commands;

namespace HadoopUserManagement.Tasks
{
    class ListUsers : Task
    {
        public ListUsers(Connection conn) : base(conn)
        {

        }

        public void listUsers()
        {
            var cmd = new PlinkCommand("-m list_users.sh");
            cmd.execute(this.connection);
        }
    }
}
