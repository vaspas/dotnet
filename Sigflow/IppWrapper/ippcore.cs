/*
//
//                  INTEL CORPORATION PROPRIETARY INFORMATION
//     This software is supplied under the terms of a license agreement or
//     nondisclosure agreement with Intel Corporation and may not be copied
//     or disclosed except in accordance with the terms of that agreement.
//        Copyright(c) 2003-2007 Intel Corporation. All Rights Reserved.
//
//     Intel® Integrated Performance Primitives Using Intel® IPP 
//     in Microsoft* C# .NET for Windows* Sample
//
//  By downloading and installing this sample, you hereby agree that the
//  accompanying Materials are being provided to you under the terms and
//  conditions of the End User License Agreement for the Intel® Integrated
//  Performance Primitives product previously accepted by you. Please refer
//  to the file ippEULA.rtf located in the root directory of your Intel® IPP
//  product installation for more information.
//
*/

// generated automatically on Mon Sep 10 15:08:49 2007

using System;
using System.Security;
using System.Runtime.InteropServices;

namespace ipp {

//
// enums
//
//
// hidden or own structures
//

unsafe public class core {

   internal const string libname = "ippcore-7.1.dll";


[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.core.libname)] public static extern
void ippAlignPtr (  char *ptr, int alignBytes );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.core.libname)] public static extern
void ippFree (  char *ptr );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.core.libname)] public static extern
ulong ippGetCpuClocks (    );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.core.libname)] public static extern
IppStatus ippGetCpuFreqMhz (  int *pMhz );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.core.libname)] public static extern
IppCpuType ippGetCpuType (    );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.core.libname)] public static extern
IppLibraryVersion ippGetLibVersion (    );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.core.libname)] public static extern
IppStatus ippGetMaxCacheSizeB (  int *pSizeByte );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.core.libname)] public static extern
int ippGetNumCoresOnDie (    );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.core.libname)] public static extern
IppStatus ippGetNumThreads (  int *pNumThr );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.core.libname)] public static extern
string ippGetStatusString (  IppStatus StsCode );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.core.libname)] public static extern
void ippMalloc (  int length );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.core.libname)] public static extern
IppStatus ippSetDenormAreZeros (  int value );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.core.libname)] public static extern
IppStatus ippSetFlushToZero (  int value, uint *pUMask );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.core.libname)] public static extern
IppStatus ippSetNumThreads (  int numThr );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.core.libname)] public static extern
IppStatus ippStaticInit (    );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.core.libname)] public static extern
IppStatus ippStaticInitCpu (  IppCpuType cpu );
};
};
