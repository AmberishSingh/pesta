# Background #
Pesta is currently based on [revision 728633](https://code.google.com/p/pesta/source/detail?r=728633) of the Java implementation of Shindig and is license under Apache Software License v2. And supports `OpenSocial 0.8`. Pesta is based on .NET Framework 3.5 and is implemented using C#. It curently uses the ASP .NET MVC framework.

The objective of this project was to quickly provide a .NET implementation that is able to act as a gadget server based on Google's gadget API as well as the comply to the OpenSocial API framework. 'pesta' is Indonesian/Malaysian for festival.

It is currently not 100% native C# and is still dependent on some java libraries via IKVM.
IKVM is used to convert java bytecode to MSIL and this is available in shindig.dll. The main dependency is the use of caja and the HTML parser. Once an equivalent is found for C# then pesta will be 100% native.

For live updates on pesta, join [pestaProject on twitter.](http://twitter.com/pestaProject). You can also follow the author [here](http://twitter.com/seanlinmt).

[raya](http://raya.codeplex.com) is an open source project that uses pesta.

Suggestions, advice, opinions, and constructive criticism are always most welcomed.

Otherwise, a [donation](https://www.paypal.com/cgi-bin/webscr?cmd=_s-xclick&hosted_button_id=7051954) would be nice too.

# The story so far ... #
At the moment, there are no plans to make pesta 100% native C# as porting the caja project over to C# is going to take awhile. IKVM gets the job done and removes the need to maintain different code streams. I just don't have the resources to do this at the moment.

# Current Plans #
  * multiple container support
  * 'OpenSocial 0.9' support
  * Windows Azure support
  * complete deserialization for Atom data format


# Project layout #
pesta actually contains a few projects. The repository is structured as follows
  * http://pesta.googlecode.com/svn/trunk/pesta/pesta
  * http://pesta.googlecode.com/svn/trunk/pesta/pestaServer
  * http://pesta.googlecode.com/svn/trunk/pesta/CustomBuildTasks
  * http://pesta.googlecode.com/svn/trunk/pesta/CloudServicePesta

The following is a summary of the above projects.

### pesta ###
Pesta.NET.dll assembly C# project

### pestaClient ###
C# OpenSocial client library. Ported from the java client library. Currently a work in progress. Porting has completed but it has not been tested and there are no examples.

### pestaServer ###
OpenSocial gadget server and main ASP .NET MVC site.

### CustomBuildTasks ###
Custom MSBuild task using YUI Compressor 2.4.2 for minifying javascript files at compile time.

### CloudServicePesta ###
Windows Azure CloudService project for Windows Azure. This adds pestaServer as a Web Role. Works fine out of the box (July 2009 Windows Azure SDK)

You can find an out of date diagram of the relationships [here](http://my6solutions.com/post/2009/02/05/OpenSocial-relationship-between-pesta-pestaServer-raya-shindig-and-IKVM.aspx).