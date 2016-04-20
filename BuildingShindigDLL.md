# Introduction #

YOU DO NOT HAVE TO DO THIS NORMALLY. Only necessary if you want to modify shindig.dll.

The following describes the steps necessary to build shindig.dll for pesta. The instructions are based on shindig [r776142](https://code.google.com/p/pesta/source/detail?r=776142). IKVM 0.38 is currently used.


# Details #
  1. create a working directory, eg. c:\ikvm\shindig
  1. Copy shindig-common-1.1-SNAPSHOT.jar, shindig-gadgets-1.1-SNAPSHOT.jar, shindig-social-api-1.1-SNAPSHOT.jar from your shindig target directories into this working directory
  1. Copy the following from your maven repository (mine is located at C:\Users\sean\.m2\repository) into the working directory
    * `caja-r3375`
    * `commons-lang-2.4`
    * `google-collections-0.9`
    * `guice-1.0`
    * `htmlparser-1.0.7`
    * `icu4j-3.8`
    * `jdom-1.0`
    * `json_simple-r1`
    * `nekohtml-1.9.11`
    * `xercesImpl-2.9.1`
    * `juel-api-2.1.1.RC2`
    * `juel-impl-2.1.0`
  1. From the ikvm binaries directory, execute the command **ikvmc -out:shindig.dll -target:library c:\ikvm\shindig\`*`.jar**
  1. There will be a bunch of warnings but this can be safely ignored as we have only included dependencies for the features we will calling from pesta