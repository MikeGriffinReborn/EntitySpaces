using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#if (!WindowsCE)
using System.Security;
#endif

[assembly: CLSCompliant(true)]

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
#if (!WindowsCE)
[assembly: AssemblyTitle("EntitySpaces.SandboxLoader")]
[assembly: AllowPartiallyTrustedCallers]
#else
[assembly: AssemblyTitle("EntitySpaces.Interfaces.CF")]
#endif
[assembly: AssemblyDescription("The EntitySpaces Query Sandbox Loader Tool")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("EntitySpaces, LLC")]
[assembly: AssemblyProduct("EntitySpacesArchitecture")]
[assembly: AssemblyCopyright("Copyright © EntitySpaces, LLC. 2005 - 2012")]
[assembly: AssemblyTrademark("EntitySpaces(TM) is a legal trademark of EntitySpaces, LLC.")]
[assembly: AssemblyCulture("")]

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM componenets.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("7C6ACE0D-68BA-4D91-952C-6883B0571841")]

// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version 
//      Build Number
//      Revision
//
// You can specify all the values or you can default the Revision and Build Numbers 
// by using the '*' as shown below:
[assembly: AssemblyVersion("2012.1.0319.0")]
#if (!WindowsCE)
[assembly: AssemblyFileVersion("2012.1.0319.0")]
#endif