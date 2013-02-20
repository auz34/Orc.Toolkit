Orc.Toolkit
===========

WPF and Silverlight controls

This Visual Studio 2012 solution contains the following controls and classes
in WPF and Silverlight versions:

FolderPicker
PinnableTooltip
StatusBar
CanvasToPrint
ColorPicker

# License

All code is released under the MS-PL License unless stated otherwise.

# Notes

To run the Demo Silverlight project you will need elevated trust:

1) Check "Require elevated trust when running in-browser" setting in your Silverlight project that uses the control
2) The XAP file of the application must be signed with a certificate. I have used a self-signed certificate for FolderPicker
3) Install the certificate on the client machine. The certificate will need to be installed to the Trusted Publishers certificate store. If it is a self-signed certificate, it will also need to be installed to the Trusted Root Certification Authorities certificate store.
4) Set a registry key on the client machine. You need to add a value named AllowElevatedTrustAppsInBrowser of type
DWORD under the following keys:
o HKEY_LOCAL_MACHINE\Software\Microsoft\Silverlight\ for 32-bit Windows
o HKEY_LOCAL_MACHINE\Software\Wow6432Node\Microsoft\Silverlight\ for
64-bit Windows
The value must be set to 1.
