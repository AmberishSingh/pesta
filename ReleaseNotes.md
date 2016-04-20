# Introduction #
Notes on initial release of pesta and differences between Shindig


# Details #
Pesta is based on [revision 698050](https://code.google.com/p/pesta/source/detail?r=698050) of the Java implementation of Shindig. Pesta is based on .NET Framework 3.5 and is implemented using C#. Support for .NET Framework 2.0 should be possible with some minor changes to the code.

The initial release includes class libraries from the IKVM project (www.ikvm.net). This is due to external jar files that were used by Shindig especially those from com.google that have/could not be ported due to time constraints or lack of source code. Due to these dependencies, shindig.dll contains some necessary java classes to support these external libraries. In the future, this should disappear.

There are two folders :

  * pesta - this is the .NET class library project that should compile into Pesta.NET.dll

  * PestaSample - this is the .NET sample website which shows a sample application of using Pesta.NET.dll

I have tried to keep the directory structure as similar as possible to the Shindig project. Future changes and improvements to the Shindig implementation can then be easily diff'ed and Pesta can updated appropriately

_Differences to the java implementation_
  * Code injection via guice is not supported
  * Logging was removed
  * web.config httphandlers added to map the various 'servlets' via the IHttpHanderFactory in Pesta.NET.dll

_What works_
  * Gadget server
  * metadata support
  * samples included in PestaSample
  * social samplecontainer
  * REST?, RPC?

_What does not work (if it hasn't been tested, it's broken)_
  * Oauth (untested)
  * BeanXMLConverter (untested)
  * Cache time in HTTP Responses (untested)

# Future work #
  * Custom web.config configuration section for Pesta
  * Code injection support (possible candidate, http://ninject.org/)
  * Remove use of IKVM and all java libraries
  * Remove use of Jayrock.JSON to use the code from JSON.org. Why? There are slight differences between the two implementations.
  * Ensure pesta adheres to standard by running compliance tests at http://code.google.com/p/opensocial-resources/wiki/ComplianceTests




