xrm-cli
=======

Command Line Interface for CRM 2011 & 2013.  A productive dev is a happy dev.

I feel that the developer workflow in CRM is a little too slow.  The Developer Toolkit for Visual Studio has improved things but it still needs work.

As someone that spends a lot of time at the command line anyway with git, I figured this would be the ideal way to try and bring some increased productivity.

This tool can also be used by a system administrator for deploying solutions across environments.

Some example usages:

#####Export a solution managed and increment the version number also save the server details to config
`xrm.exe export solutionname "c:\exports\solution.zip" -i -m o:orgname s:servername --save`

#####Import a solution.
`xrm.exe import "c:\exports\solution.zip`

#####Publish all customizations.
`xrm.exe publish`

#####Deploy new_file web resource from disk.
`xrm.exe deploy "c:\webresources\new_file.js`

#####When xrm.exe is on your path the current directory is assumed allowing.
`xrm.exe deploy new_file.js`

#####Deploy all changed web resources from folder.
`xrm.exe deploy "c:\webresources`

#####Pull the javascript web resource new_file from the server to disk into the current dirctory or specify a path.
`xrm.exe pull new_file`
