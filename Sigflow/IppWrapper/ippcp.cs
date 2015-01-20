/*
//
//                  INTEL CORPORATION PROPRIETARY INFORMATION
//     This software is supplied under the terms of a license agreement or
//     nondisclosure agreement with Intel Corporation and may not be copied
//     or disclosed except in accordance with the terms of that agreement.
//        Copyright(c) 2007 Intel Corporation. All Rights Reserved.
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

// generated automatically on Mon Sep 10 15:33:05 2007

using System;
using System.Security;
using System.Runtime.InteropServices;

namespace ipp {

//
// enums
//
   public enum IppRSAKeyTag {
      IppRSAkeyN = 1,
      IppRSAkeyE = 2,
      IppRSAkeyD = 4,
      IppRSAkeyP = 8,
      IppRSAkeyQ = 16,
   };
   public enum IppDLResult {
      ippDLValid = 0,
      ippDLBaseIsEven = 1,
      ippDLOrderIsEven = 2,
      ippDLInvalidBaseRange = 3,
      ippDLInvalidOrderRange = 4,
      ippDLCompositeBase = 5,
      ippDLCompositeOrder = 6,
      ippDLInvalidCofactor = 7,
      ippDLInvalidGenerator = 8,
      ippDLInvalidPrivateKey = 9,
      ippDLInvalidPublicKey = 10,
      ippDLInvalidKeyPair = 11,
      ippDLInvalidSignature = 12,
   };
   public enum IppDLPKeyTag {
      IppDLPkeyP = 1,
      IppDLPkeyR = 2,
      IppDLPkeyG = 4,
   };
   public enum IppsRijndaelKeyLength {
      IppsRijndaelKey128 = 128,
      IppsRijndaelKey192 = 192,
      IppsRijndaelKey256 = 256,
   };
   public enum IppECCType {
      IppECCArbitrary = 0,
      IppECCPStd = 65536,
      IppECCPStd112r1 = 65536,
      IppECCPStd112r2 = 65537,
      IppECCPStd128r1 = 65538,
      IppECCPStd128r2 = 65539,
      IppECCPStd160r1 = 65540,
      IppECCPStd160r2 = 65541,
      IppECCPStd192r1 = 65542,
      IppECCPStd224r1 = 65543,
      IppECCPStd256r1 = 65544,
      IppECCPStd384r1 = 65545,
      IppECCPStd521r1 = 65546,
      IppECCBStd = 131072,
      IppECCBStd113r1 = 131072,
      IppECCBStd113r2 = 131073,
      IppECCBStd131r1 = 131074,
      IppECCBStd131r2 = 131075,
      IppECCBStd163r1 = 131076,
      IppECCBStd163r2 = 131077,
      IppECCBStd193r1 = 131078,
      IppECCBStd193r2 = 131079,
      IppECCBStd233r1 = 131080,
      IppECCBStd283r1 = 131081,
      IppECCBStd409r1 = 131082,
      IppECCBStd571r1 = 131083,
      IppECCKStd = 262144,
      IppECCBStd163k1 = 262144,
      IppECCBStd233k1 = 262145,
      IppECCBStd239k1 = 262146,
      IppECCBStd283k1 = 262147,
      IppECCBStd409k1 = 262148,
      IppECCBStd571k1 = 262149,
   };
   public enum IppsBigNumSGN {
      IppsBigNumNEG = 0,
      IppsBigNumPOS = 1,
   };
   public enum IppsExpMethod {
      IppsBinaryMethod = 0,
      IppsSlidingWindows = 1,
   };
   public enum IppRSAKeyType {
      IppRSApublic = 1073741824,
      IppRSAprivate = -2147483648,
   };
   public enum IppECResult {
      ippECValid = 0,
      ippECCompositeBase = 1,
      ippECComplicatedBase = 2,
      ippECIsNotAG = 3,
      ippECCompositeOrder = 4,
      ippECInvalidOrder = 5,
      ippECIsWeakMOV = 6,
      ippECIsWeakSSSA = 7,
      ippECIsSupersingular = 8,
      ippECInvalidPrivateKey = 9,
      ippECInvalidPublicKey = 10,
      ippECInvalidKeyPair = 11,
      ippECPointIsAtInfinite = 12,
      ippECPointIsNotValid = 13,
      ippECPointIsEqual = 14,
      ippECPointIsNotEqual = 15,
      ippECInvalidSignature = 16,
   };
   public enum IppsCPPadding {
      NONE = 0,
      IppsCPPaddingNONE = 0,
      PKCS7 = 1,
      IppsCPPaddingPKCS7 = 1,
      ZEROS = 2,
      IppsCPPaddingZEROS = 2,
   };
//
// hidden or own structures
//
   [StructLayout(LayoutKind.Sequential)] public struct IppsARCFive128Spec {};
   [StructLayout(LayoutKind.Sequential)] public struct IppsARCFive64Spec {};
   [StructLayout(LayoutKind.Sequential)] public struct IppsARCFourState {};
   [StructLayout(LayoutKind.Sequential)] public struct IppsBigNumState {};
   [StructLayout(LayoutKind.Sequential)] public struct IppsBlowfishSpec {};
   [StructLayout(LayoutKind.Sequential)] public struct IppsCMACRijndael128State {};
   [StructLayout(LayoutKind.Sequential)] public struct IppsDAABlowfishState {};
   [StructLayout(LayoutKind.Sequential)] public struct IppsDAADESState {};
   [StructLayout(LayoutKind.Sequential)] public struct IppsDAARijndael128State {};
   [StructLayout(LayoutKind.Sequential)] public struct IppsDAARijndael192State {};
   [StructLayout(LayoutKind.Sequential)] public struct IppsDAARijndael256State {};
   [StructLayout(LayoutKind.Sequential)] public struct IppsDAATDESState {};
   [StructLayout(LayoutKind.Sequential)] public struct IppsDAATwofishState {};
   [StructLayout(LayoutKind.Sequential)] public struct IppsDESSpec {};
   [StructLayout(LayoutKind.Sequential)] public struct IppsDLPState {};
   [StructLayout(LayoutKind.Sequential)] public struct IppsECCBPointState {};
   [StructLayout(LayoutKind.Sequential)] public struct IppsECCBState {};
   [StructLayout(LayoutKind.Sequential)] public struct IppsECCPPointState {};
   [StructLayout(LayoutKind.Sequential)] public struct IppsECCPState {};
   [StructLayout(LayoutKind.Sequential)] public struct IppsHMACMD5State {};
   [StructLayout(LayoutKind.Sequential)] public struct IppsHMACSHA1State {};
   [StructLayout(LayoutKind.Sequential)] public struct IppsHMACSHA224State {};
   [StructLayout(LayoutKind.Sequential)] public struct IppsHMACSHA256State {};
   [StructLayout(LayoutKind.Sequential)] public struct IppsHMACSHA384State {};
   [StructLayout(LayoutKind.Sequential)] public struct IppsHMACSHA512State {};
   [StructLayout(LayoutKind.Sequential)] public struct IppsMD5State {};
   [StructLayout(LayoutKind.Sequential)] public struct IppsMontState {};
   [StructLayout(LayoutKind.Sequential)] public struct IppsPRNGState {};
   [StructLayout(LayoutKind.Sequential)] public struct IppsPrimeState {};
   [StructLayout(LayoutKind.Sequential)] public struct IppsRSAState {};
   [StructLayout(LayoutKind.Sequential)] public struct IppsRijndael128Spec {};
   [StructLayout(LayoutKind.Sequential)] public struct IppsRijndael192Spec {};
   [StructLayout(LayoutKind.Sequential)] public struct IppsRijndael256Spec {};
   [StructLayout(LayoutKind.Sequential)] public struct IppsSHA1State {};
   [StructLayout(LayoutKind.Sequential)] public struct IppsSHA224State {};
   [StructLayout(LayoutKind.Sequential)] public struct IppsSHA256State {};
   [StructLayout(LayoutKind.Sequential)] public struct IppsSHA384State {};
   [StructLayout(LayoutKind.Sequential)] public struct IppsSHA512State {};
   [StructLayout(LayoutKind.Sequential)] public struct IppsTwofishSpec {};

unsafe public class cp {

   internal const string libname = "ippcp-7.1.dll";


public delegate IppStatus IppMGF( byte *pSeed, int seedLen, byte *pMask, int maskLen );
public delegate IppStatus IppHASH( byte *pMsg, int len, byte *pMD );
public delegate IppStatus IppBitSupplier( uint *pRand, int nBits, void *pEbsParams );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppLibraryVersion ippcpGetLibVersion (    );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsARCFive128DecryptCBC (  byte *pSrc, byte *pDst, int length, IppsARCFive128Spec *pCtx, byte *pIV, IppsCPPadding padding );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsARCFive128DecryptCFB (  byte *pSrc, byte *pDst, int length, int cfbBlkSize, IppsARCFive128Spec *pCtx, byte *pIV, IppsCPPadding padding );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsARCFive128DecryptCTR (  byte *pSrc, byte *pDst, int len, IppsARCFive128Spec *pCtx, byte *pCtrValue, int ctrNumBitSize );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsARCFive128DecryptECB (  byte *pSrc, byte *pDst, int length, IppsARCFive128Spec *pCtx, IppsCPPadding padding );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsARCFive128DecryptOFB (  byte *pSrc, byte *pDst, int length, int cfbBlkSize, IppsARCFive128Spec *pCtx, byte *pIV );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsARCFive128EncryptCBC (  byte *pSrc, byte *pDst, int length, IppsARCFive128Spec *pCtx, byte *pIV, IppsCPPadding padding );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsARCFive128EncryptCFB (  byte *pSrc, byte *pDst, int length, int cfbBlkSize, IppsARCFive128Spec *pCtx, byte *pIV, IppsCPPadding padding );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsARCFive128EncryptCTR (  byte *pSrc, byte *pDst, int len, IppsARCFive128Spec *pCtx, byte *pCtrValue, int ctrNumBitSize );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsARCFive128EncryptECB (  byte *pSrc, byte *pDst, int length, IppsARCFive128Spec *pCtx, IppsCPPadding padding );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsARCFive128EncryptOFB (  byte *pSrc, byte *pDst, int length, int cfbBlkSize, IppsARCFive128Spec *pCtx, byte *pIV );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsARCFive128GetSize (  int rounds, int *size );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsARCFive128Init (  byte *pKey, int keylen, int rounds, IppsARCFive128Spec *pCtx );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsARCFive64DecryptCBC (  byte *pSrc, byte *pDst, int length, IppsARCFive64Spec *pCtx, byte *pIV, IppsCPPadding padding );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsARCFive64DecryptCFB (  byte *pSrc, byte *pDst, int length, int cfbBlkSize, IppsARCFive64Spec *pCtx, byte *pIV, IppsCPPadding padding );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsARCFive64DecryptCTR (  byte *pSrc, byte *pDst, int len, IppsARCFive64Spec *pCtx, byte *pCtrValue, int ctrNumBitSize );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsARCFive64DecryptECB (  byte *pSrc, byte *pDst, int length, IppsARCFive64Spec *pCtx, IppsCPPadding padding );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsARCFive64DecryptOFB (  byte *pSrc, byte *pDst, int length, int cfbBlkSize, IppsARCFive64Spec *pCtx, byte *pIV );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsARCFive64EncryptCBC (  byte *pSrc, byte *pDst, int length, IppsARCFive64Spec *pCtx, byte *pIV, IppsCPPadding padding );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsARCFive64EncryptCFB (  byte *pSrc, byte *pDst, int length, int cfbBlkSize, IppsARCFive64Spec *pCtx, byte *pIV, IppsCPPadding padding );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsARCFive64EncryptCTR (  byte *pSrc, byte *pDst, int len, IppsARCFive64Spec *pCtx, byte *pCtrValue, int ctrNumBitSize );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsARCFive64EncryptECB (  byte *pSrc, byte *pDst, int length, IppsARCFive64Spec *pCtx, IppsCPPadding padding );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsARCFive64EncryptOFB (  byte *pSrc, byte *pDst, int length, int cfbBlkSize, IppsARCFive64Spec *pCtx, byte *pIV );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsARCFive64GetSize (  int rounds, int *pSize );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsARCFive64Init (  byte *pKey, int keylen, int rounds, IppsARCFive64Spec *pCtx );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsARCFourCheckKey (  byte *pKey, int keyLen, IppBool *pIsWeak );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsARCFourDecrypt (  byte *pSrc, byte *pDst, int length, IppsARCFourState *pCtx );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsARCFourEncrypt (  byte *pSrc, byte *pDst, int length, IppsARCFourState *pCtx );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsARCFourGetSize (  int *pSize );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsARCFourInit (  byte *pKey, int keyLen, IppsARCFourState *pCtx );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsARCFourReset (  IppsARCFourState *pCtx );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsAdd_BN (  IppsBigNumState *pA, IppsBigNumState *pB, IppsBigNumState *pR );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsAdd_BNU (  uint *pA, uint *pB, uint *pR, int n, uint *carry );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsBigNumGetSize (  int length, int *pSize );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsBigNumInit (  int length, IppsBigNumState *pBN );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsBlowfishDecryptCBC (  byte *pSrc, byte *pDst, int len, IppsBlowfishSpec *pCtx, byte *pIV, IppsCPPadding padding );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsBlowfishDecryptCFB (  byte *pSrc, byte *pDst, int len, int cfbBlkSize, IppsBlowfishSpec *pCtx, byte *pIV, IppsCPPadding padding );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsBlowfishDecryptCTR (  byte *pSrc, byte *pDst, int len, IppsBlowfishSpec *pCtx, byte *pCtrValue, int ctrNumBitSize );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsBlowfishDecryptECB (  byte *pSrc, byte *pDst, int len, IppsBlowfishSpec *pCtx, IppsCPPadding padding );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsBlowfishDecryptOFB (  byte *pSrc, byte *pDst, int len, int ofbBlkSize, IppsBlowfishSpec *pCtx, byte *pIV );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsBlowfishEncryptCBC (  byte *pSrc, byte *pDst, int len, IppsBlowfishSpec *pCtx, byte *pIV, IppsCPPadding padding );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsBlowfishEncryptCFB (  byte *pSrc, byte *pDst, int len, int cfbBlkSize, IppsBlowfishSpec *pCtx, byte *pIV, IppsCPPadding padding );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsBlowfishEncryptCTR (  byte *pSrc, byte *pDst, int len, IppsBlowfishSpec *pCtx, byte *pCtrValue, int ctrNumBitSize );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsBlowfishEncryptECB (  byte *pSrc, byte *pDst, int len, IppsBlowfishSpec *pCtx, IppsCPPadding padding );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsBlowfishEncryptOFB (  byte *pSrc, byte *pDst, int len, int ofbBlkSize, IppsBlowfishSpec *pCtx, byte *pIV );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsBlowfishGetSize (  int *pSize );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsBlowfishInit (  byte *pKey, int keyLen, IppsBlowfishSpec *pCtx );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsCMACRijndael128Final (  byte *pMD, int mdLen, IppsCMACRijndael128State *pState );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsCMACRijndael128GetSize (  int *pSize );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsCMACRijndael128Init (  byte *pKey, IppsRijndaelKeyLength keyLen, IppsCMACRijndael128State *pState );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsCMACRijndael128MessageDigest (  byte *pMsg, int msgLen, byte *pKey, IppsRijndaelKeyLength keyLen, byte *pMD, int mdLen );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsCMACRijndael128Update (  byte *pSrc, int len, IppsCMACRijndael128State *pState );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsCmpZero_BN (  IppsBigNumState *pBN, uint *pResult );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsCmp_BN (  IppsBigNumState *pA, IppsBigNumState *pB, uint *pResult );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsDAABlowfishFinal (  byte *pMD, int mdLen, IppsDAABlowfishState *pCtx );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsDAABlowfishGetSize (  int *pSize );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsDAABlowfishInit (  byte *pKey, int keyLen, IppsDAABlowfishState *pCtx );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsDAABlowfishMessageDigest (  byte *pMsg, int msgLen, byte *pKey, int keyLen, byte *pMD, int mdLen );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsDAABlowfishUpdate (  byte *pSrc, int len, IppsDAABlowfishState *pCtx );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsDAADESFinal (  byte *pMD, int mdLen, IppsDAADESState *pCtx );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsDAADESGetSize (  int *pSize );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsDAADESInit (  byte *pKey, IppsDAADESState *pCtx );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsDAADESMessageDigest (  byte *pMsg, int msgLen, byte *pKey, byte *pMD, int mdLen );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsDAADESUpdate (  byte *pSrc, int len, IppsDAADESState *pCtx );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsDAARijndael128Final (  byte *pMD, int mdLen, IppsDAARijndael128State *pCtx );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsDAARijndael128GetSize (  int *pSize );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsDAARijndael128Init (  byte *pKey, IppsRijndaelKeyLength keyLen, IppsDAARijndael128State *pCtx );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsDAARijndael128MessageDigest (  byte *pMsg, int msgLen, byte *pKey, IppsRijndaelKeyLength keyLen, byte *pMD, int mdLen );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsDAARijndael128Update (  byte *pSrc, int len, IppsDAARijndael128State *pCtx );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsDAARijndael192Final (  byte *pMD, int mdLen, IppsDAARijndael192State *pCtx );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsDAARijndael192GetSize (  int *pSize );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsDAARijndael192Init (  byte *pKey, IppsRijndaelKeyLength keyLen, IppsDAARijndael192State *pCtx );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsDAARijndael192MessageDigest (  byte *pMsg, int msgLen, byte *pKey, IppsRijndaelKeyLength keyLen, byte *pMD, int mdLen );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsDAARijndael192Update (  byte *pSrc, int len, IppsDAARijndael192State *pCtx );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsDAARijndael256Final (  byte *pMD, int mdLen, IppsDAARijndael256State *pCtx );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsDAARijndael256GetSize (  int *pSize );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsDAARijndael256Init (  byte *pKey, IppsRijndaelKeyLength keyLen, IppsDAARijndael256State *pCtx );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsDAARijndael256MessageDigest (  byte *pMsg, int msgLen, byte *pKey, IppsRijndaelKeyLength keyLen, byte *pMD, int mdLen );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsDAARijndael256Update (  byte *pSrc, int len, IppsDAARijndael256State *pCtx );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsDAATDESFinal (  byte *pMD, int mdLen, IppsDAATDESState *pCtx );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsDAATDESGetSize (  int *pSize );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsDAATDESInit (  byte *pKey1, byte *pKey2, byte *pKey3, IppsDAATDESState *pCtx );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsDAATDESMessageDigest (  byte *pMsg, int msgLen, byte *pKey1, byte *pKey2, byte *pKey3, byte *pMD, int mdLen );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsDAATDESUpdate (  byte *pSrc, int len, IppsDAATDESState *pCtx );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsDAATwofishFinal (  byte *pMD, int mdLen, IppsDAATwofishState *pCtx );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsDAATwofishGetSize (  int *pSize );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsDAATwofishInit (  byte *pKey, int keyLen, IppsDAATwofishState *pCtx );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsDAATwofishMessageDigest (  byte *pMsg, int msgLen, byte *pKey, int keyLen, byte *pMD, int mdLen );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsDAATwofishUpdate (  byte *pSrc, int len, IppsDAATwofishState *pCtx );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsDESDecryptCBC (  byte *pSrc, byte *pDst, int length, IppsDESSpec *pCtx, byte *pIV, IppsCPPadding padding );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsDESDecryptCFB (  byte *pSrc, byte *pDst, int length, int cfbBlkSize, IppsDESSpec *pCtx, byte *pIV, IppsCPPadding padding );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsDESDecryptCTR (  byte *pSrc, byte *pDst, int len, IppsDESSpec *pCtx, byte *pCtrValue, int ctrNumBitSize );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsDESDecryptECB (  byte *pSrc, byte *pDst, int length, IppsDESSpec *pCtx, IppsCPPadding padding );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsDESDecryptOFB (  byte *pSrc, byte *pDst, int len, int ofbBlkSize, IppsDESSpec *pCtx, byte *pIV );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsDESEncryptCBC (  byte *pSrc, byte *pDst, int length, IppsDESSpec *pCtx, byte *pIV, IppsCPPadding padding );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsDESEncryptCFB (  byte *pSrc, byte *pDst, int length, int cfbBlkSize, IppsDESSpec *pCtx, byte *pIV, IppsCPPadding padding );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsDESEncryptCTR (  byte *pSrc, byte *pDst, int len, IppsDESSpec *pCtx, byte *pCtrValue, int ctrNumBitSize );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsDESEncryptECB (  byte *pSrc, byte *pDst, int length, IppsDESSpec *pCtx, IppsCPPadding padding );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsDESEncryptOFB (  byte *pSrc, byte *pDst, int len, int ofbBlkSize, IppsDESSpec *pCtx, byte *pIV );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsDESGetSize (  int *size );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsDESInit (  byte *pKey, IppsDESSpec *pCtx );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
string ippsDLGetResultString (  IppDLResult code );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsDLPGenKeyPair (  IppsBigNumState *pPrvKey, IppsBigNumState *pPubKey, IppsDLPState *pCtx, IppBitSupplier rndFunc, void *pRndParam );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsDLPGenerateDH (  IppsBigNumState *pSeedIn, int nTrials, IppsDLPState *pCtx, IppsBigNumState *pSeedOut, int *pCounter, IppBitSupplier rndFunc, void *pRndParam );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsDLPGenerateDSA (  IppsBigNumState *pSeedIn, int nTrials, IppsDLPState *pCtx, IppsBigNumState *pSeedOut, int *pCounter, IppBitSupplier rndFunc, void *pRndParam );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsDLPGet (  IppsBigNumState *pP, IppsBigNumState *pR, IppsBigNumState *pG, IppsDLPState *pCtx );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsDLPGetDP (  IppsBigNumState *pDP, IppDLPKeyTag tag, IppsDLPState *pCtx );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsDLPGetSize (  int bitSizeP, int bitSizeR, int *pSize );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsDLPInit (  int bitSizeP, int bitSizeR, IppsDLPState *pCtx );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsDLPPublicKey (  IppsBigNumState *pPrvKey, IppsBigNumState *pPubKey, IppsDLPState *pCtx );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsDLPSet (  IppsBigNumState *pP, IppsBigNumState *pR, IppsBigNumState *pG, IppsDLPState *pCtx );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsDLPSetDP (  IppsBigNumState *pDP, IppDLPKeyTag tag, IppsDLPState *pCtx );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsDLPSetKeyPair (  IppsBigNumState *pPrvKey, IppsBigNumState *pPubKey, IppsDLPState *pCtx );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsDLPSharedSecretDH (  IppsBigNumState *pPrvKeyA, IppsBigNumState *pPubKeyB, IppsBigNumState *pShare, IppsDLPState *pCtx );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsDLPSignDSA (  IppsBigNumState *pMsgDigest, IppsBigNumState *pPrvKey, IppsBigNumState *pSignR, IppsBigNumState *pSignS, IppsDLPState *pCtx );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsDLPValidateDH (  int nTrials, IppDLResult *pResult, IppsDLPState *pCtx, IppBitSupplier rndFunc, void *pRndParam );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsDLPValidateDSA (  int nTrials, IppDLResult *pResult, IppsDLPState *pCtx, IppBitSupplier rndFunc, void *pRndParam );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsDLPValidateKeyPair (  IppsBigNumState *pPrvKey, IppsBigNumState *pPubKey, IppDLResult *pResult, IppsDLPState *pCtx );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsDLPVerifyDSA (  IppsBigNumState *pMsgDigest, IppsBigNumState *pSignR, IppsBigNumState *pSignS, IppDLResult *pResult, IppsDLPState *pCtx );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsDiv_64u32u (  ulong a, uint b, uint *pQ, uint *pR );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsDiv_BN (  IppsBigNumState *pA, IppsBigNumState *pB, IppsBigNumState *pQ, IppsBigNumState *pR );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsECCBAddPoint (  IppsECCBPointState *pP, IppsECCBPointState *pQ, IppsECCBPointState *pR, IppsECCBState *pECC );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsECCBCheckPoint (  IppsECCBPointState *pP, IppECResult *pResult, IppsECCBState *pECC );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsECCBComparePoint (  IppsECCBPointState *pP, IppsECCBPointState *pQ, IppECResult *pResult, IppsECCBState *pECC );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsECCBGenKeyPair (  IppsBigNumState *pPrivate, IppsECCBPointState *pPublic, IppsECCBState *pECC, IppBitSupplier rndFunc, void *pRndParam );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsECCBGet (  IppsBigNumState *pPrime, IppsBigNumState *pA, IppsBigNumState *pB, IppsBigNumState *pGX, IppsBigNumState *pGY, IppsBigNumState *pOrder, int *cofactor, IppsECCBState *pECC );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsECCBGetOrderBitSize (  int *pBitSize, IppsECCBState *pECC );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsECCBGetPoint (  IppsBigNumState *pX, IppsBigNumState *pY, IppsECCBPointState *pPoint, IppsECCBState *pECC );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsECCBGetSize (  int feBitSize, int *pSize );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsECCBInit (  int feBitSize, IppsECCBState *pECC );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsECCBMulPointScalar (  IppsECCBPointState *pP, IppsBigNumState *pK, IppsECCBPointState *pR, IppsECCBState *pECC );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsECCBNegativePoint (  IppsECCBPointState *pP, IppsECCBPointState *pR, IppsECCBState *pECC );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsECCBPointGetSize (  int feBitSize, int *pSize );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsECCBPointInit (  int feBitSize, IppsECCBPointState *pPoint );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsECCBPublicKey (  IppsBigNumState *pPrivate, IppsECCBPointState *pPublic, IppsECCBState *pECC );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsECCBSet (  IppsBigNumState *pPrime, IppsBigNumState *pA, IppsBigNumState *pB, IppsBigNumState *pGX, IppsBigNumState *pGY, IppsBigNumState *pOrder, int cofactor, IppsECCBState *pECC );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsECCBSetKeyPair (  IppsBigNumState *pPrivate, IppsECCBPointState *pPublic, IppBool regular, IppsECCBState *pECC );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsECCBSetPoint (  IppsBigNumState *pX, IppsBigNumState *pY, IppsECCBPointState *pPoint, IppsECCBState *pECC );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsECCBSetPointAtInfinity (  IppsECCBPointState *pPoint, IppsECCBState *pECC );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsECCBSetStd (  IppECCType flag, IppsECCBState *pECC );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsECCBSharedSecretDH (  IppsBigNumState *pPrivateA, IppsECCBPointState *pPublicB, IppsBigNumState *pShare, IppsECCBState *pECC );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsECCBSharedSecretDHC (  IppsBigNumState *pPrivateA, IppsECCBPointState *pPublicB, IppsBigNumState *pShare, IppsECCBState *pECC );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsECCBSignDSA (  IppsBigNumState *pMsgDigest, IppsBigNumState *pPrivate, IppsBigNumState *pSignX, IppsBigNumState *pSignY, IppsECCBState *pECC );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsECCBSignNR (  IppsBigNumState *pMsgDigest, IppsBigNumState *pPrivate, IppsBigNumState *pSignX, IppsBigNumState *pSignY, IppsECCBState *pECC );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsECCBValidate (  int nTrials, IppECResult *pResult, IppsECCBState *pECC, IppBitSupplier rndFunc, void *pRndParam );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsECCBValidateKeyPair (  IppsBigNumState *pPrivate, IppsECCBPointState *pPublic, IppECResult *pResult, IppsECCBState *pECC );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsECCBVerifyDSA (  IppsBigNumState *pMsgDigest, IppsBigNumState *pSignX, IppsBigNumState *pSignY, IppECResult *pResult, IppsECCBState *pECC );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsECCBVerifyNR (  IppsBigNumState *pMsgDigest, IppsBigNumState *pSignX, IppsBigNumState *pSignY, IppECResult *pResult, IppsECCBState *pECC );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
string ippsECCGetResultString (  IppECResult code );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsECCPAddPoint (  IppsECCPPointState *pP, IppsECCPPointState *pQ, IppsECCPPointState *pR, IppsECCPState *pECC );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsECCPCheckPoint (  IppsECCPPointState *pP, IppECResult *pResult, IppsECCPState *pECC );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsECCPComparePoint (  IppsECCPPointState *pP, IppsECCPPointState *pQ, IppECResult *pResult, IppsECCPState *pECC );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsECCPGenKeyPair (  IppsBigNumState *pPrivate, IppsECCPPointState *pPublic, IppsECCPState *pECC, IppBitSupplier rndFunc, void *pRndParam );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsECCPGet (  IppsBigNumState *pPrime, IppsBigNumState *pA, IppsBigNumState *pB, IppsBigNumState *pGX, IppsBigNumState *pGY, IppsBigNumState *pOrder, int *cofactor, IppsECCPState *pECC );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsECCPGetOrderBitSize (  int *pBitSize, IppsECCPState *pECC );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsECCPGetPoint (  IppsBigNumState *pX, IppsBigNumState *pY, IppsECCPPointState *pPoint, IppsECCPState *pECC );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsECCPGetSize (  int feBitSize, int *pSize );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsECCPInit (  int feBitSize, IppsECCPState *pECC );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsECCPMulPointScalar (  IppsECCPPointState *pP, IppsBigNumState *pK, IppsECCPPointState *pR, IppsECCPState *pECC );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsECCPNegativePoint (  IppsECCPPointState *pP, IppsECCPPointState *pR, IppsECCPState *pECC );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsECCPPointGetSize (  int feBitSize, int *pSize );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsECCPPointInit (  int feBitSize, IppsECCPPointState *pPoint );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsECCPPublicKey (  IppsBigNumState *pPrivate, IppsECCPPointState *pPublic, IppsECCPState *pECC );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsECCPSet (  IppsBigNumState *pPrime, IppsBigNumState *pA, IppsBigNumState *pB, IppsBigNumState *pGX, IppsBigNumState *pGY, IppsBigNumState *pOrder, int cofactor, IppsECCPState *pECC );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsECCPSetKeyPair (  IppsBigNumState *pPrivate, IppsECCPPointState *pPublic, IppBool regular, IppsECCPState *pECC );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsECCPSetPoint (  IppsBigNumState *pX, IppsBigNumState *pY, IppsECCPPointState *pPoint, IppsECCPState *pECC );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsECCPSetPointAtInfinity (  IppsECCPPointState *pPoint, IppsECCPState *pECC );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsECCPSetStd (  IppECCType flag, IppsECCPState *pECC );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsECCPSharedSecretDH (  IppsBigNumState *pPrivateA, IppsECCPPointState *pPublicB, IppsBigNumState *pShare, IppsECCPState *pECC );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsECCPSharedSecretDHC (  IppsBigNumState *pPrivateA, IppsECCPPointState *pPublicB, IppsBigNumState *pShare, IppsECCPState *pECC );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsECCPSignDSA (  IppsBigNumState *pMsgDigest, IppsBigNumState *pPrivate, IppsBigNumState *pSignX, IppsBigNumState *pSignY, IppsECCPState *pECC );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsECCPSignNR (  IppsBigNumState *pMsgDigest, IppsBigNumState *pPrivate, IppsBigNumState *pSignX, IppsBigNumState *pSignY, IppsECCPState *pECC );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsECCPValidate (  int nTrials, IppECResult *pResult, IppsECCPState *pECC, IppBitSupplier rndFunc, void *pRndParam );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsECCPValidateKeyPair (  IppsBigNumState *pPrivate, IppsECCPPointState *pPublic, IppECResult *pResult, IppsECCPState *pECC );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsECCPVerifyDSA (  IppsBigNumState *pMsgDigest, IppsBigNumState *pSignX, IppsBigNumState *pSignY, IppECResult *pResult, IppsECCPState *pECC );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsECCPVerifyNR (  IppsBigNumState *pMsgDigest, IppsBigNumState *pSignX, IppsBigNumState *pSignY, IppECResult *pResult, IppsECCPState *pECC );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsExtGet_BN (  IppsBigNumSGN *pSgn, int *pBitSize, uint *pData, IppsBigNumState *pBN );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsGcd_BN (  IppsBigNumState *pA, IppsBigNumState *pB, IppsBigNumState *pGCD );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsGetOctString_BN (  byte *pStr, int strLen, IppsBigNumState *pBN );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsGetOctString_BNU (  uint *pBNU, int bnuSize, byte *pStr, int strLen );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsGetSize_BN (  IppsBigNumState *pBN, int *pSize );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsGet_BN (  IppsBigNumSGN *pSgn, int *pLength, uint *pData, IppsBigNumState *pBN );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsHMACMD5Duplicate (  IppsHMACMD5State *pSrcCtx, IppsHMACMD5State *pDstCtx );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsHMACMD5Final (  byte *pMD, int mdLen, IppsHMACMD5State *pCtx );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsHMACMD5GetSize (  int *size );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsHMACMD5Init (  byte *pKey, int keyLen, IppsHMACMD5State *pCtx );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsHMACMD5MessageDigest (  byte *pMsg, int msgLen, byte *pKey, int keyLen, byte *pMD, int mdLen );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsHMACMD5Update (  byte *pSrc, int len, IppsHMACMD5State *pCtx );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsHMACSHA1Duplicate (  IppsHMACSHA1State *pSrcCtx, IppsHMACSHA1State *pDstCtx );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsHMACSHA1Final (  byte *pMD, int mdLen, IppsHMACSHA1State *pCtx );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsHMACSHA1GetSize (  int *size );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsHMACSHA1Init (  byte *pKey, int keyLen, IppsHMACSHA1State *pCtx );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsHMACSHA1MessageDigest (  byte *pMsg, int msgLen, byte *pKey, int keyLen, byte *pMD, int mdLen );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsHMACSHA1Update (  byte *pSrc, int len, IppsHMACSHA1State *pCtx );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsHMACSHA224Duplicate (  IppsHMACSHA224State *pSrcCtx, IppsHMACSHA224State *pDstCtx );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsHMACSHA224Final (  byte *pMD, int mdLen, IppsHMACSHA224State *pCtx );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsHMACSHA224GetSize (  int *size );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsHMACSHA224Init (  byte *pKey, int keyLen, IppsHMACSHA224State *pCtx );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsHMACSHA224MessageDigest (  byte *pMsg, int msgLen, byte *pKey, int keyLen, byte *pMD, int mdLen );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsHMACSHA224Update (  byte *pSrc, int len, IppsHMACSHA224State *pCtx );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsHMACSHA256Duplicate (  IppsHMACSHA256State *pSrcCtx, IppsHMACSHA256State *pDstCtx );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsHMACSHA256Final (  byte *pMD, int mdLen, IppsHMACSHA256State *pCtx );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsHMACSHA256GetSize (  int *size );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsHMACSHA256Init (  byte *pKey, int keyLen, IppsHMACSHA256State *pCtx );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsHMACSHA256MessageDigest (  byte *pMsg, int msgLen, byte *pKey, int keyLen, byte *pMD, int mdLen );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsHMACSHA256Update (  byte *pSrc, int len, IppsHMACSHA256State *pCtx );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsHMACSHA384Duplicate (  IppsHMACSHA384State *pSrcCtx, IppsHMACSHA384State *pDstCtx );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsHMACSHA384Final (  byte *pMD, int mdLen, IppsHMACSHA384State *pCtx );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsHMACSHA384GetSize (  int *size );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsHMACSHA384Init (  byte *pKey, int keyLen, IppsHMACSHA384State *pCtx );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsHMACSHA384MessageDigest (  byte *pMsg, int msgLen, byte *pKey, int keyLen, byte *pMD, int mdLen );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsHMACSHA384Update (  byte *pSrc, int len, IppsHMACSHA384State *pCtx );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsHMACSHA512Duplicate (  IppsHMACSHA512State *pSrcCtx, IppsHMACSHA512State *pDstCtx );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsHMACSHA512Final (  byte *pMD, int mdLen, IppsHMACSHA512State *pCtx );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsHMACSHA512GetSize (  int *size );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsHMACSHA512Init (  byte *pKey, int keyLen, IppsHMACSHA512State *pCtx );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsHMACSHA512MessageDigest (  byte *pMsg, int msgLen, byte *pKey, int keyLen, byte *pMD, int mdLen );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsHMACSHA512Update (  byte *pSrc, int len, IppsHMACSHA512State *pCtx );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsMACOne_BNU_I (  uint *pA, uint *pR, int n, uint w, uint *carry );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsMAC_BN_I (  IppsBigNumState *pA, IppsBigNumState *pB, IppsBigNumState *pR );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsMD5Duplicate (  IppsMD5State *pSrcCtx, IppsMD5State *pDstCtx );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsMD5Final (  byte *pMD, IppsMD5State *pCtx );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsMD5GetSize (  int *pSize );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsMD5Init (  IppsMD5State *pCtx );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsMD5MessageDigest (  byte *pMsg, int len, byte *pMD );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsMD5Update (  byte *pSrc, int len, IppsMD5State *pCtx );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsMGF_MD5 (  byte *pSeed, int seedLen, byte *pMask, int maskLen );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsMGF_SHA1 (  byte *pSeed, int seedLen, byte *pMask, int maskLen );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsMGF_SHA224 (  byte *pSeed, int seedLen, byte *pMask, int maskLen );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsMGF_SHA256 (  byte *pSeed, int seedLen, byte *pMask, int maskLen );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsMGF_SHA384 (  byte *pSeed, int seedLen, byte *pMask, int maskLen );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsMGF_SHA512 (  byte *pSeed, int seedLen, byte *pMask, int maskLen );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsModInv_BN (  IppsBigNumState *pA, IppsBigNumState *pM, IppsBigNumState *pInv );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsMod_BN (  IppsBigNumState *pA, IppsBigNumState *pM, IppsBigNumState *pR );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsMontExp (  IppsBigNumState *pA, IppsBigNumState *pE, IppsMontState *m, IppsBigNumState *pR );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsMontForm (  IppsBigNumState *pA, IppsMontState *pCtx, IppsBigNumState *pR );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsMontGet (  uint *pModulo, int *pSize, IppsMontState *pCtx );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsMontGetSize (  IppsExpMethod method, int length, int *pSize );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsMontInit (  IppsExpMethod method, int length, IppsMontState *pCtx );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsMontMul (  IppsBigNumState *pA, IppsBigNumState *pB, IppsMontState *m, IppsBigNumState *pR );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsMontSet (  uint *pModulo, int size, IppsMontState *pCtx );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsMulOne_BNU (  uint *pA, uint *pR, int n, uint w, uint *carry );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsMul_BN (  IppsBigNumState *pA, IppsBigNumState *pB, IppsBigNumState *pR );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsMul_BNU4 (  uint *pA, uint *pB, uint *pR );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsMul_BNU8 (  uint *pA, uint *pB, uint *pR );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsPRNGGetSize (  int *pSize );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsPRNGInit (  int seedBits, IppsPRNGState *pCtx );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsPRNGSetAugment (  IppsBigNumState *pAug, IppsPRNGState *pCtx );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsPRNGSetH0 (  IppsBigNumState *pH0, IppsPRNGState *pCtx );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsPRNGSetModulus (  IppsBigNumState *pMod, IppsPRNGState *pCtx );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsPRNGSetSeed (  IppsBigNumState *pSeed, IppsPRNGState *pCtx );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsPRNGen (  uint *pRand, int nBits, void *pCtx );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsPRNGen_BN (  IppsBigNumState *pRand, int nBits, void *pCtx );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsPrimeGen (  int nBits, int nTrials, IppsPrimeState *pCtx, IppBitSupplier rndFunc, void *pRndParam );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsPrimeGet (  uint *pPrime, int *pLen, IppsPrimeState *pCtx );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsPrimeGetSize (  int nMaxBits, int *pSize );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsPrimeGet_BN (  IppsBigNumState *pPrime, IppsPrimeState *pCtx );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsPrimeInit (  int nMaxBits, IppsPrimeState *pCtx );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsPrimeSet (  uint *pPrime, int nBits, IppsPrimeState *pCtx );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsPrimeSet_BN (  IppsBigNumState *pPrime, IppsPrimeState *pCtx );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsPrimeTest (  int nTrials, uint *pResult, IppsPrimeState *pCtx, IppBitSupplier rndFunc, void *pRndParam );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsRSADecrypt (  IppsBigNumState *pX, IppsBigNumState *pY, IppsRSAState *pCtx );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsRSAEncrypt (  IppsBigNumState *pX, IppsBigNumState *pY, IppsRSAState *pCtx );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsRSAGenerate (  IppsBigNumState *pE, int kn, int kp, int nTrials, IppsRSAState *pCtx, IppBitSupplier rndFunc, void *pRndParam );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsRSAGetKey (  IppsBigNumState *pKey, IppRSAKeyTag tag, IppsRSAState *pCtx );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsRSAGetSize (  int kn, int kp, IppRSAKeyType flag, int *pSize );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsRSAInit (  int kn, int kp, IppRSAKeyType flag, IppsRSAState *pCtx );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsRSAOAEPDecrypt (  byte *pSrc, byte *pLabel, int labLen, byte *pDst, int *pDstLen, IppsRSAState *pRSA, IppHASH hashFun, int hashLen, IppMGF mgfFunc );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsRSAOAEPDecrypt_MD5 (  byte *pSrc, byte *pLabel, int labLen, byte *pDst, int *pDstLen, IppsRSAState *pRSA );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsRSAOAEPDecrypt_SHA1 (  byte *pSrc, byte *pLabel, int labLen, byte *pDst, int *pDstLen, IppsRSAState *pRSA );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsRSAOAEPDecrypt_SHA224 (  byte *pSrc, byte *pLabel, int labLen, byte *pDst, int *pDstLen, IppsRSAState *pRSA );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsRSAOAEPDecrypt_SHA256 (  byte *pSrc, byte *pLabel, int labLen, byte *pDst, int *pDstLen, IppsRSAState *pRSA );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsRSAOAEPDecrypt_SHA384 (  byte *pSrc, byte *pLabel, int labLen, byte *pDst, int *pDstLen, IppsRSAState *pRSA );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsRSAOAEPDecrypt_SHA512 (  byte *pSrc, byte *pLabel, int labLen, byte *pDst, int *pDstLen, IppsRSAState *pRSA );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsRSAOAEPEncrypt (  byte *pSrc, int srcLen, byte *pLabel, int labLen, byte *pSeed, byte *pDst, IppsRSAState *pRSA, IppHASH hashFun, int hashLen, IppMGF mgfFunc );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsRSAOAEPEncrypt_MD5 (  byte *pSrc, int srcLen, byte *pLabel, int labLen, byte *pSeed, byte *pDst, IppsRSAState *pRSA );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsRSAOAEPEncrypt_SHA1 (  byte *pSrc, int srcLen, byte *pLabel, int labLen, byte *pSeed, byte *pDst, IppsRSAState *pRSA );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsRSAOAEPEncrypt_SHA224 (  byte *pSrc, int srcLen, byte *pLabel, int labLen, byte *pSeed, byte *pDst, IppsRSAState *pRSA );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsRSAOAEPEncrypt_SHA256 (  byte *pSrc, int srcLen, byte *pLabel, int labLen, byte *pSeed, byte *pDst, IppsRSAState *pRSA );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsRSAOAEPEncrypt_SHA384 (  byte *pSrc, int srcLen, byte *pLabel, int labLen, byte *pSeed, byte *pDst, IppsRSAState *pRSA );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsRSAOAEPEncrypt_SHA512 (  byte *pSrc, int srcLen, byte *pLabel, int labLen, byte *pSeed, byte *pDst, IppsRSAState *pRSA );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsRSASSASign (  byte *pHMsg, int hashLen, byte *pSalt, int saltLen, byte *pSign, IppsRSAState *pRSA, IppHASH hashFun, IppMGF mgfFun );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsRSASSASign_MD5 (  byte *pHMsg, byte *pSalt, int saltLen, byte *pSign, IppsRSAState *pRSA );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsRSASSASign_SHA1 (  byte *pHMsg, byte *pSalt, int saltLen, byte *pSign, IppsRSAState *pRSA );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsRSASSASign_SHA224 (  byte *pHMsg, byte *pSalt, int saltLen, byte *pSign, IppsRSAState *pRSA );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsRSASSASign_SHA256 (  byte *pHMsg, byte *pSalt, int saltLen, byte *pSign, IppsRSAState *pRSA );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsRSASSASign_SHA384 (  byte *pHMsg, byte *pSalt, int saltLen, byte *pSign, IppsRSAState *pRSA );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsRSASSASign_SHA512 (  byte *pHMsg, byte *pSalt, int saltLen, byte *pSign, IppsRSAState *pRSA );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsRSASSAVerify (  byte *pHMsg, int hashLen, byte *pSign, IppBool *pIsValid, IppsRSAState *pRSA, IppHASH hashFun, IppMGF mgfFun );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsRSASSAVerify_MD5 (  byte *pHMsg, byte *pSign, IppBool *pIsValid, IppsRSAState *pRSA );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsRSASSAVerify_SHA1 (  byte *pHMsg, byte *pSign, IppBool *pIsValid, IppsRSAState *pRSA );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsRSASSAVerify_SHA224 (  byte *pHMsg, byte *pSign, IppBool *pIsValid, IppsRSAState *pRSA );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsRSASSAVerify_SHA256 (  byte *pHMsg, byte *pSign, IppBool *pIsValid, IppsRSAState *pRSA );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsRSASSAVerify_SHA384 (  byte *pHMsg, byte *pSign, IppBool *pIsValid, IppsRSAState *pRSA );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsRSASSAVerify_SHA512 (  byte *pHMsg, byte *pSign, IppBool *pIsValid, IppsRSAState *pRSA );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsRSASetKey (  IppsBigNumState *pKey, IppRSAKeyTag tag, IppsRSAState *pCtx );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsRSAValidate (  IppsBigNumState *pE, int nTrials, uint *pResult, IppsRSAState *pCtx, IppBitSupplier rndFunc, void *pRndParam );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsRijndael128DecryptCBC (  byte *pSrc, byte *pDst, int len, IppsRijndael128Spec *pCtx, byte *pIV, IppsCPPadding padding );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsRijndael128DecryptCCM (  byte *pNonce, uint lenN, byte *pHeader, ulong lenH, byte *pCtext, ulong lenC, int macLen, byte *pPtext, IppBool *pResult, IppsRijndael128Spec *pCtx );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsRijndael128DecryptCCM_u8 (  byte *pNonce, int lenN, byte *pHeader, int lenH, byte *pCtext, int lenC, int macLen, byte *pPtext, IppBool *pResult, IppsRijndael128Spec *pCtx );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsRijndael128DecryptCFB (  byte *pSrc, byte *pDst, int len, int cfbBlkSize, IppsRijndael128Spec *pCtx, byte *pIV, IppsCPPadding padding );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsRijndael128DecryptCTR (  byte *pSrc, byte *pDst, int len, IppsRijndael128Spec *pCtx, byte *pCtrValue, int ctrNumBitSize );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsRijndael128DecryptECB (  byte *pSrc, byte *pDst, int len, IppsRijndael128Spec *pCtx, IppsCPPadding padding );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsRijndael128DecryptOFB (  byte *pSrc, byte *pDst, int len, int ofbBlkSize, IppsRijndael128Spec *pCtx, byte *pIV );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsRijndael128EncryptCBC (  byte *pSrc, byte *pDst, int len, IppsRijndael128Spec *pCtx, byte *pIV, IppsCPPadding padding );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsRijndael128EncryptCCM (  byte *pNonce, uint lenN, byte *pHeader, ulong lenH, byte *pPtext, ulong lenP, int macLen, byte *pCtext, IppsRijndael128Spec *pCtx );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsRijndael128EncryptCCM_u8 (  byte *pNonce, int lenN, byte *pHeader, int lenH, byte *pPtext, int lenP, int macLen, byte *pCtext, IppsRijndael128Spec *pCtx );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsRijndael128EncryptCFB (  byte *pSrc, byte *pDst, int len, int cfbBlkSize, IppsRijndael128Spec *pCtx, byte *pIV, IppsCPPadding padding );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsRijndael128EncryptCTR (  byte *pSrc, byte *pDst, int len, IppsRijndael128Spec *pCtx, byte *pCtrValue, int ctrNumBitSize );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsRijndael128EncryptECB (  byte *pSrc, byte *pDst, int len, IppsRijndael128Spec *pCtx, IppsCPPadding padding );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsRijndael128EncryptOFB (  byte *pSrc, byte *pDst, int len, int ofbBlkSize, IppsRijndael128Spec *pCtx, byte *pIV );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsRijndael128GetSize (  int *pSize );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsRijndael128Init (  byte *pKey, IppsRijndaelKeyLength keyLen, IppsRijndael128Spec *pCtx );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsRijndael192DecryptCBC (  byte *pSrc, byte *pDst, int len, IppsRijndael192Spec *pCtx, byte *pIV, IppsCPPadding padding );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsRijndael192DecryptCFB (  byte *pSrc, byte *pDst, int len, int cfbBlkSize, IppsRijndael192Spec *pCtx, byte *pIV, IppsCPPadding padding );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsRijndael192DecryptCTR (  byte *pSrc, byte *pDst, int len, IppsRijndael192Spec *pCtx, byte *pCtrValue, int ctrNumBitSize );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsRijndael192DecryptECB (  byte *pSrc, byte *pDst, int len, IppsRijndael192Spec *pCtx, IppsCPPadding padding );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsRijndael192DecryptOFB (  byte *pSrc, byte *pDst, int len, int ofbBlkSize, IppsRijndael192Spec *pCtx, byte *pIV );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsRijndael192EncryptCBC (  byte *pSrc, byte *pDst, int len, IppsRijndael192Spec *pCtx, byte *pIV, IppsCPPadding padding );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsRijndael192EncryptCFB (  byte *pSrc, byte *pDst, int len, int cfbBlkSize, IppsRijndael192Spec *pCtx, byte *pIV, IppsCPPadding padding );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsRijndael192EncryptCTR (  byte *pSrc, byte *pDst, int len, IppsRijndael192Spec *pCtx, byte *pCtrValue, int ctrNumBitSize );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsRijndael192EncryptECB (  byte *pSrc, byte *pDst, int len, IppsRijndael192Spec *pCtx, IppsCPPadding padding );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsRijndael192EncryptOFB (  byte *pSrc, byte *pDst, int len, int ofbBlkSize, IppsRijndael192Spec *pCtx, byte *pIV );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsRijndael192GetSize (  int *pSize );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsRijndael192Init (  byte *pKey, IppsRijndaelKeyLength keyLen, IppsRijndael192Spec *pCtx );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsRijndael256DecryptCBC (  byte *pSrc, byte *pDst, int len, IppsRijndael256Spec *pCtx, byte *pIV, IppsCPPadding padding );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsRijndael256DecryptCFB (  byte *pSrc, byte *pDst, int len, int cfbBlkSize, IppsRijndael256Spec *pCtx, byte *pIV, IppsCPPadding padding );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsRijndael256DecryptCTR (  byte *pSrc, byte *pDst, int len, IppsRijndael256Spec *pCtx, byte *pCtrValue, int ctrNumBitSize );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsRijndael256DecryptECB (  byte *pSrc, byte *pDst, int len, IppsRijndael256Spec *pCtx, IppsCPPadding padding );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsRijndael256DecryptOFB (  byte *pSrc, byte *pDst, int len, int ofbBlkSize, IppsRijndael256Spec *pCtx, byte *pIV );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsRijndael256EncryptCBC (  byte *pSrc, byte *pDst, int len, IppsRijndael256Spec *pCtx, byte *pIV, IppsCPPadding padding );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsRijndael256EncryptCFB (  byte *pSrc, byte *pDst, int len, int cfbBlkSize, IppsRijndael256Spec *pCtx, byte *pIV, IppsCPPadding padding );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsRijndael256EncryptCTR (  byte *pSrc, byte *pDst, int len, IppsRijndael256Spec *pCtx, byte *pCtrValue, int ctrNumBitSize );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsRijndael256EncryptECB (  byte *pSrc, byte *pDst, int len, IppsRijndael256Spec *pCtx, IppsCPPadding padding );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsRijndael256EncryptOFB (  byte *pSrc, byte *pDst, int len, int ofbBlkSize, IppsRijndael256Spec *pCtx, byte *pIV );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsRijndael256GetSize (  int *pSize );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsRijndael256Init (  byte *pKey, IppsRijndaelKeyLength keyLen, IppsRijndael256Spec *pCtx );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsSHA1Duplicate (  IppsSHA1State *pSrcCtx, IppsSHA1State *pDstCtx );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsSHA1Final (  byte *pMD, IppsSHA1State *pCtx );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsSHA1GetSize (  int *pSize );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsSHA1Init (  IppsSHA1State *pCtx );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsSHA1MessageDigest (  byte *pMsg, int len, byte *pMD );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsSHA1Update (  byte *pSrc, int len, IppsSHA1State *pCtx );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsSHA224Duplicate (  IppsSHA224State *pSrcCtx, IppsSHA224State *pDstCtx );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsSHA224Final (  byte *pMD, IppsSHA224State *pCtx );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsSHA224GetSize (  int *pSize );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsSHA224Init (  IppsSHA224State *pCtx );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsSHA224MessageDigest (  byte *pMsg, int len, byte *pMD );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsSHA224Update (  byte *pSrc, int len, IppsSHA224State *pCtx );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsSHA256Duplicate (  IppsSHA256State *pSrcCtx, IppsSHA256State *pDstCtx );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsSHA256Final (  byte *pMD, IppsSHA256State *pCtx );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsSHA256GetSize (  int *pSize );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsSHA256Init (  IppsSHA256State *pCtx );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsSHA256MessageDigest (  byte *pMsg, int len, byte *pMD );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsSHA256Update (  byte *pSrc, int len, IppsSHA256State *pCtx );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsSHA384Duplicate (  IppsSHA384State *pSrcCtx, IppsSHA384State *pDstCtx );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsSHA384Final (  byte *pMD, IppsSHA384State *pCtx );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsSHA384GetSize (  int *pSize );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsSHA384Init (  IppsSHA384State *pCtx );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsSHA384MessageDigest (  byte *pMsg, int len, byte *pMD );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsSHA384Update (  byte *pSrc, int len, IppsSHA384State *pCtx );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsSHA512Duplicate (  IppsSHA512State *pSrcCtx, IppsSHA512State *pDstCtx );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsSHA512Final (  byte *pMD, IppsSHA512State *pCtx );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsSHA512GetSize (  int *pSize );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsSHA512Init (  IppsSHA512State *pCtx );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsSHA512MessageDigest (  byte *pMsg, int len, byte *pMD );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsSHA512Update (  byte *pSrc, int len, IppsSHA512State *pCtx );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsSetOctString_BN (  byte *pStr, int strLen, IppsBigNumState *pBN );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsSetOctString_BNU (  byte *pStr, int strLen, uint *pBNU, int *pBNUsize );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsSet_BN (  IppsBigNumSGN sgn, int length, uint *pData, IppsBigNumState *pBN );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsSqr_32u64u (  uint *pSrc, int n, ulong *pDst );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsSqr_BNU4 (  uint *pA, uint *pR );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsSqr_BNU8 (  uint *pA, uint *pR );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsSub_BN (  IppsBigNumState *pA, IppsBigNumState *pB, IppsBigNumState *pR );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsSub_BNU (  uint *pA, uint *pB, uint *pR, int n, uint *carry );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsTDESDecryptCBC (  byte *pSrc, byte *pDst, int length, IppsDESSpec *pCtx1, IppsDESSpec *pCtx2, IppsDESSpec *pCtx3, byte *pIV, IppsCPPadding padding );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsTDESDecryptCFB (  byte *pSrc, byte *pDst, int length, int cfbBlkSize, IppsDESSpec *pCtx1, IppsDESSpec *pCtx2, IppsDESSpec *pCtx3, byte *pIV, IppsCPPadding padding );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsTDESDecryptCTR (  byte *pSrc, byte *pDst, int len, IppsDESSpec *pCtx1, IppsDESSpec *pCtx2, IppsDESSpec *pCtx3, byte *pCtrValue, int ctrNumBitSize );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsTDESDecryptECB (  byte *pSrc, byte *pDst, int length, IppsDESSpec *pCtx1, IppsDESSpec *pCtx2, IppsDESSpec *pCtx3, IppsCPPadding padding );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsTDESDecryptOFB (  byte *pSrc, byte *pDst, int len, int ofbBlkSize, IppsDESSpec *pCtx1, IppsDESSpec *pCtx2, IppsDESSpec *pCtx3, byte *pIV );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsTDESEncryptCBC (  byte *pSrc, byte *pDst, int length, IppsDESSpec *pCtx1, IppsDESSpec *pCtx2, IppsDESSpec *pCtx3, byte *pIV, IppsCPPadding padding );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsTDESEncryptCFB (  byte *pSrc, byte *pDst, int length, int cfbBlkSize, IppsDESSpec *pCtx1, IppsDESSpec *pCtx2, IppsDESSpec *pCtx3, byte *pIV, IppsCPPadding padding );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsTDESEncryptCTR (  byte *pSrc, byte *pDst, int len, IppsDESSpec *pCtx1, IppsDESSpec *pCtx2, IppsDESSpec *pCtx3, byte *pCtrValue, int ctrNumBitSize );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsTDESEncryptECB (  byte *pSrc, byte *pDst, int length, IppsDESSpec *pCtx1, IppsDESSpec *pCtx2, IppsDESSpec *pCtx3, IppsCPPadding padding );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsTDESEncryptOFB (  byte *pSrc, byte *pDst, int len, int ofbBlkSize, IppsDESSpec *pCtx1, IppsDESSpec *pCtx2, IppsDESSpec *pCtx3, byte *pIV );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsTwofishDecryptCBC (  byte *pSrc, byte *pDst, int len, IppsTwofishSpec *pCtx, byte *pIV, IppsCPPadding padding );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsTwofishDecryptCFB (  byte *pSrc, byte *pDst, int len, int cfbBlkSize, IppsTwofishSpec *pCtx, byte *pIV, IppsCPPadding padding );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsTwofishDecryptCTR (  byte *pSrc, byte *pDst, int len, IppsTwofishSpec *pCtx, byte *pCtrValue, int ctrNumBitSize );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsTwofishDecryptECB (  byte *pSrc, byte *pDst, int len, IppsTwofishSpec *pCtx, IppsCPPadding padding );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsTwofishDecryptOFB (  byte *pSrc, byte *pDst, int len, int ofbBlkSize, IppsTwofishSpec *pCtx, byte *pIV );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsTwofishEncryptCBC (  byte *pSrc, byte *pDst, int len, IppsTwofishSpec *pCtx, byte *pIV, IppsCPPadding padding );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsTwofishEncryptCFB (  byte *pSrc, byte *pDst, int len, int cfbBlkSize, IppsTwofishSpec *pCtx, byte *pIV, IppsCPPadding padding );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsTwofishEncryptCTR (  byte *pSrc, byte *pDst, int len, IppsTwofishSpec *pCtx, byte *pCtrValue, int ctrNumBitSize );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsTwofishEncryptECB (  byte *pSrc, byte *pDst, int len, IppsTwofishSpec *pCtx, IppsCPPadding padding );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsTwofishEncryptOFB (  byte *pSrc, byte *pDst, int len, int ofbBlkSize, IppsTwofishSpec *pCtx, byte *pIV );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsTwofishGetSize (  int *pSize );

[SuppressUnmanagedCodeSecurityAttribute()]
[DllImport(ipp.cp.libname)] public static extern
IppStatus ippsTwofishInit (  byte *pKey, int keyLen, IppsTwofishSpec *pCtx );
};
};
