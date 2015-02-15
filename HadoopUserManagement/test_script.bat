
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
