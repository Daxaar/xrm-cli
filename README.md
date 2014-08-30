xrm-cli
=======

Command Line Interface for CRM 2011.  A productive dev is a happy dev.

I feel that the developer workflow in CRM is a little too slow.  The Developer Toolkit for Visual Studio has improved things but it still needs work.

As someone that spends a lot of time at the command line anyway with git, I figured this would be the ideal way to try and bring some increased productivity.

This tool can also be used by a system administrator for deploying solutions across environments.

Initially only supports Import (with publish) and Export of solutions.

TODO: Add MEF support to prevent the need for the XrmTaskFactory.

Extensibility:

Add a new IXrmTask taking a CommandLine instance on it's ctor.  This should provide the IXrmTask implementation with all of the information it needs off the command line.

Bucket List:

- Add new task to set custom import path
