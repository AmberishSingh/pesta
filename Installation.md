# Installing pesta and raya #


## Prerequisites ##
- ASP .NET 3.5,
- ASP .NET MVC, http://go.microsoft.com/fwlink/?LinkID=140768&clcid=0x409
- Your favourite versioning tool, eg. Tortoise SVN, http://tortoisesvn.tigris.org/
- Access to MS SQL database or SQL database express

create 3 folders on your drive somewhere called
- pesta
- pestaserver
- raya

Then, check-out the code for each folder at the locations specified below
- pesta, https://pesta.googlecode.com/svn/trunk/pesta/pesta/
- pestaserver , https://pesta.googlecode.com/svn/trunk/pesta/pestaServer/
- raya, https://raya.svn.codeplex.com/svn/raya

While waiting for your code to finish downloading, you will need to downloading the IKVM libraries.
You can do that here, http://pesta.googlecode.com/files/IKVM.Shindig.dll.0.1.zip. The use of this file
will be explained later.

Once you have checked out the above projects, open the .csproj files under each project. Basically, both pestaserver and raya
is dependent on pesta (which builds into a DLL). What I normally do is that I open 2 copies of Visual Studio. pesta and pestaserver
will be loaded on the first VS; pesta and raya will be loaded on the second VS.

## pestaServer ##
Now back to IKVM.Shindig.dll.0.1.zip. Extract the contents of this file somewhere. Next, go to your pestaserver project
and delete the existing references for the following files (as they refer to the incorrect path)
- IKVM.OpenJDK.ClassLibrary
- IKVM.Runtime
- IKVM.Runtime>JNI
- shindig

Now, Add References.. to the 4 files that you have extracted somewhere above.

You may additionally need to remove the existing reference for pesta and add the pesta project again as a reference
for pestaServer. You should be able to build the solution correctly now.


## Installing the SQL database ##
You either use SQL Server Management Studio or from the command line via sqlcmd to create the database.
Under the raya project, there is a file called raya.sql. This is the database script.

You will first need to create a database called raya. Once this is done, simply execute the script (raya.sql)
from raya.


## pestaServer web.config ##
There are only 3 things you need to do here
1. Point the connection string to the database you've created
2. Specify the address for containerUrlPrefix. This is the address of the "raya project" when it starts up.
You can update the dynamic port number later.
3. Either specify the tokenMasterKey so that communication between the site and gadget server includes a security token
OR
set allowUnauthenticated to true to bare it all

FYI: There will be another article on how to setup OAuth.


## raya ##
With the raya project, you will just need to delete the current reference to pesta and add the reference
to pesta again.

You will only need to configure the web.config file. In this file, you will need to
1. Point the connectionString to the database you created above
2. If you had specify the tokenMasterKey for pesta, you will need to specify the same value here
3. specify the address of the gadget\_server, ie. the address that pesta is running on.


And that should be it. If you have any questions, I can be contacted at seanlinmt at my6solutions dot com
