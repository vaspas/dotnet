#region Using directives

using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

#endregion

namespace SoundBlasterModules.WaveApi
{
    abstract class WaveAPI
    {
        #region ///structs ///

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public struct WAVEFORMATEX
        {
            public short wFormatTag;
            public short nChannels;
            public int nSamplesPerSec;
            public int nAvgBytesPerSec;
            public short nBlockAlign;
            public short wBitsPerSample;
            public short cbSize;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public struct WAVEHDR
        {
            public IntPtr lpData; // pointer to locked data buffer
            public int dwBufferLength; // length of data buffer
            public int dwBytesRecorded; // used for input only
            public IntPtr dwUser; // for client's use
            public int dwFlags; // assorted flags (see defines)
            public int dwLoops; // loop control counter
            public IntPtr lpNext; // PWaveHdr, reserved for driver
            public int reserved; // reserved for driver
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public struct MIXERLINE
        {
            public int cbStruct;               /* size of MIXERLINE structure */
            public int dwDestination;          /* zero based destination index */
            public int dwSource;               /* zero based source index (if source) */
            public int dwLineID;               /* unique line id for mixer device */
            public int fdwLine;                /* state/information about line */
            public int dwUser;                 /* driver specific information */
            public int dwComponentType;        /* component type line connects to */
            public int cChannels;              /* number of channels line supports */
            public int cConnections;           /* number of connections [possible] */
            public int cControls;              /* number of controls at this line */
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
            public string szShortName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string szName;


            public TARGET Target;

            [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
            public struct TARGET
            {
                public int dwType;                 // MIXERLINE_TARGETTYPE_xxxx 
                public int dwDeviceID;             // target device ID of device type 
                public short wMid;                   // of target device 
                public short wPid;                   //      " 
                public int vDriverVersion;         //      " 
                [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
                public string szPname;   //      " 
            }
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public struct MIXERLINECONTROLS
        {
            public int cbStruct;       /* size in bytes of MIXERLINECONTROLS */
            public int dwLineID;       /* line id (from MIXERLINE.dwLineID) */

            //public int dwControlID;    /* MIXER_GETLINECONTROLSF_ONEBYID */
            public int dwControlType;  /* MIXER_GETLINECONTROLSF_ONEBYTYPE */

            public int cControls;      /* count of controls pmxctrl points to */
            public int cbmxctrl;       /* size in bytes of _one_ MIXERCONTROL */
            public IntPtr pamxctrl;       /* LPMIXERCONTROLW pointer to first MIXERCONTROL array */
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public class MIXERCONTROL
        {
            public int cbStruct;           /* size in bytes of MIXERCONTROL */
            public int dwControlID;        /* unique control id for mixer device */
            public int dwControlType;      /* MIXERCONTROL_CONTROLTYPE_xxx */
            public int fdwControl;         /* MIXERCONTROL_CONTROLF_xxx */
            public int cMultipleItems;     /* if MIXERCONTROL_CONTROLF_MULTIPLE set */
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
            public string szShortName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string szName;

            public int dwMinimum;           /* signed minimum for this control */
            public int dwMaximum;           /* signed maximum for this control */
            //public int Bounds_dwMinimum;          /* unsigned minimum for this control */
            //public int Bounds_dwMaximum;          /* unsigned maximum for this control */
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public int[] Bounds_dwReserved;

            public int cSteps_or_cbCustomData;             /* # of steps between min & max */
            // public int Metrics_cbCustomData;       /* size in bytes of custom data */
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
            public int[] Metrics_dwReserved;      /* !!! needed? we have cbStruct.... */
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public struct MIXERCONTROLDETAILS
        {
            public int cbStruct;       /* size in bytes of MIXERCONTROLDETAILS */
            public int dwControlID;    /* control id to get/set details on */
            public int cChannels;      /* number of channels in paDetails array */

            //public IntPtr hwndOwner;      /* for MIXER_SETCONTROLDETAILSF_CUSTOM */
            //public int cMultipleItems; /* if _MULTIPLE, the number of items per channel */
            public int Item;

            public int cbDetails;      /* size of _one_ details_XX struct */
            public IntPtr paDetails;      /* pointer to array of details_XX structs */
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public class MIXERCONTROLDETAILS_LISTTEXT
        {
            public int dwParam1;
            public int dwParam2;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string szName;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public class MIXERCONTROLDETAILS_UNSIGNED
        {
            public uint dwValue;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public struct MIXERCAPS
        {
            public short wMid;                   /* manufacturer id */
            public short wPid;                   /* product id */
            public short vDriverVersionMajor;         /* version of the driver */
            public short vDriverVersionMinor;         /* version of the driver */
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MAXPNAMELEN)]
            public string szPname;   /* product name */
            public int fdwSupport;             /* misc. support bits */
            public int cDestinations;          /* count of destinations */
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public struct WAVEINCAPS
        {
            public short wMid;                    /* manufacturer ID */
            public short wPid;                    /* product ID */
            public short vDriverVersionMajor;         /* version of the driver */
            public short vDriverVersionMinor;         /* version of the driver */
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MAXPNAMELEN)]
            public string szPname;   /* product name */
            public int dwFormats;               /* formats supported */
            public short wChannels;               /* number of channels supported */
            public short wReserved1;              /* structure packing */
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public struct WAVEOUTCAPS
        {
            public short wMid;                    /* manufacturer ID */
            public short wPid;                    /* product ID */
            public short vDriverVersionMajor;         /* version of the driver */
            public short vDriverVersionMinor;         /* version of the driver */
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MAXPNAMELEN)]
            public string szPname;   /* product name */
            public int dwFormats;               /* formats supported */
            public short wChannels;               /* number of channels supported */
            public short wReserved1;              /* structure packing */
            public int dwSupport;                /*Optional functionality supported by the device*/
        }

        #endregion

        #region /// contstants ///

        #region String resource number bases (internal use)
        const int MMSYSERR_BASE = 0;
        const int WAVERR_BASE = 32;
        const int MIDIERR_BASE = 64;
        const int TIMERR_BASE = 96;
        const int JOYERR_BASE = 160;
        const int MCIERR_BASE = 256;
        const int MIXERR_BASE = 1024;

        const int MCI_STRING_OFFSET = 512;
        const int MCI_VD_OFFSET = 1024;
        const int MCI_CD_OFFSET = 1088;
        const int MCI_WAVE_OFFSET = 1152;
        const int MCI_SEQ_OFFSET = 1216;
        #endregion

        public const int NOERROR = 0;

        public enum Errors
        {
            NOERROR = 0,            /* no error */
            MMSYSERR_ERROR = (MMSYSERR_BASE + 1), /* unspecified error */
            MMSYSERR_BADDEVICEID = (MMSYSERR_BASE + 2), /* device ID out of range */
            MMSYSERR_NOTENABLED = (MMSYSERR_BASE + 3), /* driver failed enable */
            MMSYSERR_ALLOCATED = (MMSYSERR_BASE + 4), /* device already allocated */
            MMSYSERR_INVALHANDLE = (MMSYSERR_BASE + 5), /* device handle is invalid */
            MMSYSERR_NODRIVER = (MMSYSERR_BASE + 6), /* no device driver present */
            MMSYSERR_NOMEM = (MMSYSERR_BASE + 7), /* memory allocation error */
            MMSYSERR_NOTSUPPORTED = (MMSYSERR_BASE + 8), /* function isn't supported */
            MMSYSERR_BADERRNUM = (MMSYSERR_BASE + 9), /* error value out of range */
            MMSYSERR_INVALFLAG = (MMSYSERR_BASE + 10), /* invalid flag passed */
            MMSYSERR_INVALPARAM = (MMSYSERR_BASE + 11), /* invalid parameter passed */
            MMSYSERR_HANDLEBUSY = (MMSYSERR_BASE + 12), /* handle being used */
            /* simultaneously on another */
            /* thread (eg callback) */
            MMSYSERR_INVALIDALIAS = (MMSYSERR_BASE + 13), /* specified alias not found */
            MMSYSERR_BADDB = (MMSYSERR_BASE + 14), /* bad registry database */
            MMSYSERR_KEYNOTFOUND = (MMSYSERR_BASE + 15), /* registry key not found */
            MMSYSERR_READERROR = (MMSYSERR_BASE + 16), /* registry read error */
            MMSYSERR_WRITEERROR = (MMSYSERR_BASE + 17), /* registry write error */
            MMSYSERR_DELETEERROR = (MMSYSERR_BASE + 18), /* registry delete error */
            MMSYSERR_VALNOTFOUND = (MMSYSERR_BASE + 19), /* registry value not found */
            MMSYSERR_NODRIVERCB = (MMSYSERR_BASE + 20), /* driver does not call DriverCallback */
            MMSYSERR_LASTERROR = (MMSYSERR_BASE + 20), /* last error in range */

            /* waveform audio error return values */
 WAVERR_BADFORMAT     = (WAVERR_BASE + 0),    /* unsupported wave format */
 WAVERR_STILLPLAYING  = (WAVERR_BASE + 1),    /* still something playing */
 WAVERR_UNPREPARED    = (WAVERR_BASE + 2),    /* header not prepared */
 WAVERR_SYNC          = (WAVERR_BASE + 3),    /* device is synchronous */
 WAVERR_LASTERROR     = (WAVERR_BASE + 3),    /* last error in range */

            /* MIDI error return values */
 MIDIERR_UNPREPARED   = (MIDIERR_BASE + 0),   /* header not prepared */
 MIDIERR_STILLPLAYING = (MIDIERR_BASE + 1),   /* still something playing */
 MIDIERR_NOMAP        = (MIDIERR_BASE + 2),   /* no configured instruments */
 MIDIERR_NOTREADY     = (MIDIERR_BASE + 3),   /* hardware is still busy */
 MIDIERR_NODEVICE     = (MIDIERR_BASE + 4),   /* port no longer connected */
 MIDIERR_INVALIDSETUP = (MIDIERR_BASE + 5),   /* invalid MIF */
 MIDIERR_BADOPENMODE  = (MIDIERR_BASE + 6),   /* operation unsupported w/ open mode */
 MIDIERR_DONT_CONTINUE= (MIDIERR_BASE + 7),   /* thru device 'eating' a message */
 MIDIERR_LASTERROR    = (MIDIERR_BASE + 7),   /* last error in range */

            /* */
/*  MMRESULT error return values specific to the mixer API */
/* */
/* */
 MIXERR_INVALLINE      =      (MIXERR_BASE + 0),
 MIXERR_INVALCONTROL   =      (MIXERR_BASE + 1),
 MIXERR_INVALVALUE     =      (MIXERR_BASE + 2),
 MIXERR_LASTERROR      =      (MIXERR_BASE + 2),

            /* timer error return values */
 TIMERR_NOCANDO      =  (TIMERR_BASE+1)  ,    /* request not completed */
 TIMERR_STRUCT       =  (TIMERR_BASE+33) ,    /* time struct size */

            /* joystick error return values */
 JOYERR_PARMS      =    (JOYERR_BASE+5)  ,    /* bad parameters */
 JOYERR_NOCANDO    =    (JOYERR_BASE+6)  ,    /* request not completed */
 JOYERR_UNPLUGGED  =    (JOYERR_BASE+7)  ,    /* joystick is unplugged */

            /* MCI error return values */
 MCIERR_INVALID_DEVICE_ID      =  (MCIERR_BASE + 1),
 MCIERR_UNRECOGNIZED_KEYWORD   =  (MCIERR_BASE + 3),
 MCIERR_UNRECOGNIZED_COMMAND   =  (MCIERR_BASE + 5),
 MCIERR_HARDWARE               =  (MCIERR_BASE + 6),
 MCIERR_INVALID_DEVICE_NAME    =  (MCIERR_BASE + 7),
 MCIERR_OUT_OF_MEMORY          =  (MCIERR_BASE + 8),
 MCIERR_DEVICE_OPEN            =  (MCIERR_BASE + 9),
 MCIERR_CANNOT_LOAD_DRIVER     =  (MCIERR_BASE + 10),
 MCIERR_MISSING_COMMAND_STRING =  (MCIERR_BASE + 11),
 MCIERR_PARAM_OVERFLOW         =  (MCIERR_BASE + 12),
 MCIERR_MISSING_STRING_ARGUMENT=  (MCIERR_BASE + 13),
 MCIERR_BAD_INTEGER            =  (MCIERR_BASE + 14),
 MCIERR_PARSER_INTERNAL        =  (MCIERR_BASE + 15),
 MCIERR_DRIVER_INTERNAL        =  (MCIERR_BASE + 16),
 MCIERR_MISSING_PARAMETER      =  (MCIERR_BASE + 17),
 MCIERR_UNSUPPORTED_FUNCTION   =  (MCIERR_BASE + 18),
 MCIERR_FILE_NOT_FOUND         =  (MCIERR_BASE + 19),
 MCIERR_DEVICE_NOT_READY       =  (MCIERR_BASE + 20),
 MCIERR_INTERNAL               =  (MCIERR_BASE + 21),
 MCIERR_DRIVER                 =  (MCIERR_BASE + 22),
 MCIERR_CANNOT_USE_ALL         =  (MCIERR_BASE + 23),
 MCIERR_MULTIPLE               =  (MCIERR_BASE + 24),
 MCIERR_EXTENSION_NOT_FOUND    =  (MCIERR_BASE + 25),
 MCIERR_OUTOFRANGE             =  (MCIERR_BASE + 26),
 MCIERR_FLAGS_NOT_COMPATIBLE   =  (MCIERR_BASE + 28),
 MCIERR_FILE_NOT_SAVED         =  (MCIERR_BASE + 30),
 MCIERR_DEVICE_TYPE_REQUIRED   =  (MCIERR_BASE + 31),
 MCIERR_DEVICE_LOCKED          =  (MCIERR_BASE + 32),
 MCIERR_DUPLICATE_ALIAS        =  (MCIERR_BASE + 33),
 MCIERR_BAD_CONSTANT           =  (MCIERR_BASE + 34),
 MCIERR_MUST_USE_SHAREABLE     =  (MCIERR_BASE + 35),
 MCIERR_MISSING_DEVICE_NAME    =  (MCIERR_BASE + 36),
 MCIERR_BAD_TIME_FORMAT        =  (MCIERR_BASE + 37),
 MCIERR_NO_CLOSING_QUOTE       =  (MCIERR_BASE + 38),
 MCIERR_DUPLICATE_FLAGS        =  (MCIERR_BASE + 39),
 MCIERR_INVALID_FILE           =  (MCIERR_BASE + 40),
 MCIERR_NULL_PARAMETER_BLOCK   =  (MCIERR_BASE + 41),
 MCIERR_UNNAMED_RESOURCE       =  (MCIERR_BASE + 42),
 MCIERR_NEW_REQUIRES_ALIAS     =  (MCIERR_BASE + 43),
 MCIERR_NOTIFY_ON_AUTO_OPEN    =  (MCIERR_BASE + 44),
 MCIERR_NO_ELEMENT_ALLOWED     =  (MCIERR_BASE + 45),
 MCIERR_NONAPPLICABLE_FUNCTION =  (MCIERR_BASE + 46),
 MCIERR_ILLEGAL_FOR_AUTO_OPEN  =  (MCIERR_BASE + 47),
 MCIERR_FILENAME_REQUIRED      =  (MCIERR_BASE + 48),
 MCIERR_EXTRA_CHARACTERS       =  (MCIERR_BASE + 49),
 MCIERR_DEVICE_NOT_INSTALLED   =  (MCIERR_BASE + 50),
 MCIERR_GET_CD                 =  (MCIERR_BASE + 51),
 MCIERR_SET_CD                 =  (MCIERR_BASE + 52),
 MCIERR_SET_DRIVE              =  (MCIERR_BASE + 53),
 MCIERR_DEVICE_LENGTH          =  (MCIERR_BASE + 54),
 MCIERR_DEVICE_ORD_LENGTH      =  (MCIERR_BASE + 55),
 MCIERR_NO_INTEGER             =  (MCIERR_BASE + 56),

 MCIERR_WAVE_OUTPUTSINUSE      =  (MCIERR_BASE + 64),
 MCIERR_WAVE_SETOUTPUTINUSE    =  (MCIERR_BASE + 65),
 MCIERR_WAVE_INPUTSINUSE       =  (MCIERR_BASE + 66),
 MCIERR_WAVE_SETINPUTINUSE     =  (MCIERR_BASE + 67),
 MCIERR_WAVE_OUTPUTUNSPECIFIED =  (MCIERR_BASE + 68),
 MCIERR_WAVE_INPUTUNSPECIFIED  =  (MCIERR_BASE + 69),
 MCIERR_WAVE_OUTPUTSUNSUITABLE =  (MCIERR_BASE + 70),
 MCIERR_WAVE_SETOUTPUTUNSUITABLE= (MCIERR_BASE + 71),
 MCIERR_WAVE_INPUTSUNSUITABLE   = (MCIERR_BASE + 72),
 MCIERR_WAVE_SETINPUTUNSUITABLE = (MCIERR_BASE + 73),

 MCIERR_SEQ_DIV_INCOMPATIBLE   =  (MCIERR_BASE + 80),
 MCIERR_SEQ_PORT_INUSE         =  (MCIERR_BASE + 81),
 MCIERR_SEQ_PORT_NONEXISTENT   =  (MCIERR_BASE + 82),
 MCIERR_SEQ_PORT_MAPNODEVICE   =  (MCIERR_BASE + 83),
 MCIERR_SEQ_PORT_MISCERROR     =  (MCIERR_BASE + 84),
 MCIERR_SEQ_TIMER              =  (MCIERR_BASE + 85),
 MCIERR_SEQ_PORTUNSPECIFIED    =  (MCIERR_BASE + 86),
 MCIERR_SEQ_NOMIDIPRESENT      =  (MCIERR_BASE + 87),

 MCIERR_NO_WINDOW              =  (MCIERR_BASE + 90),
 MCIERR_CREATEWINDOW           =  (MCIERR_BASE + 91),
 MCIERR_FILE_READ              =  (MCIERR_BASE + 92),
 MCIERR_FILE_WRITE             =  (MCIERR_BASE + 93),

 MCIERR_NO_IDENTITY             = (MCIERR_BASE + 94)

        }


        /* general constants */
        public const int MAXPNAMELEN = 32;    /* max product name length (including NULL) */
        public const int MAXERRORLENGTH = 256;   /* max error text length (including NULL) */
        public const int MAX_JOYSTICKOEMVXDNAME = 260; /* max oem vxd name length (including NULL) */

        // Multimedia Extensions Window Messages
        public abstract class WindowMessages
        {

            public const int MM_JOY1MOVE = 0x3A0;           /* joystick */
            public const int MM_JOY2MOVE = 0x3A1;
            public const int MM_JOY1ZMOVE = 0x3A2;
            public const int MM_JOY2ZMOVE = 0x3A3;
            public const int MM_JOY1BUTTONDOWN = 0x3B5;
            public const int MM_JOY2BUTTONDOWN = 0x3B6;
            public const int MM_JOY1BUTTONUP = 0x3B7;
            public const int MM_JOY2BUTTONUP = 0x3B8;

            public const int MM_MCINOTIFY = 0x3B9;           /* MCI */

            public const int MM_WOM_OPEN = 0x3BB;           /* waveform output */
            public const int MM_WOM_CLOSE = 0x3BC;
            public const int MM_WOM_DONE = 0x3BD;

            public const int MM_WIM_OPEN = 0x3BE;           /* waveform input */
            public const int MM_WIM_CLOSE = 0x3BF;
            public const int MM_WIM_DATA = 0x3C0;

            public const int MM_MIM_OPEN = 0x3C1;           /* MIDI input */
            public const int MM_MIM_CLOSE = 0x3C2;
            public const int MM_MIM_DATA = 0x3C3;
            public const int MM_MIM_LONGDATA = 0x3C4;
            public const int MM_MIM_ERROR = 0x3C5;
            public const int MM_MIM_LONGERROR = 0x3C6;

            public const int MM_MOM_OPEN = 0x3C7;           /* MIDI output */
            public const int MM_MOM_CLOSE = 0x3C8;
            public const int MM_MOM_DONE = 0x3C9;

/* these are also in msvideo.h */
//#ifndef MM_DRVM_OPEN
            public const int MM_DRVM_OPEN = 0x3D0;          /* installable drivers */
            public const int MM_DRVM_CLOSE = 0x3D1;
            public const int MM_DRVM_DATA = 0x3D2;
            public const int MM_DRVM_ERROR = 0x3D3;
//#endif

/* these are used by msacm.h */
            public const int MM_STREAM_OPEN = 0x3D4;
            public const int MM_STREAM_CLOSE = 0x3D5;
            public const int MM_STREAM_DONE = 0x3D6;
            public const int MM_STREAM_ERROR = 0x3D7;

        }


        public const int CALLBACK_FUNCTION = 0x00030000;    // dwCallback is a FARPROC 
        public const int CALLBACK_EVENT = 0x00050000;

        public const int TIME_MS = 0x0001;  // time in milliseconds 
        public const int TIME_SAMPLES = 0x0002;  // number of wave samples 
        public const int TIME_BYTES = 0x0004;  // current byte offset 

        public const int WAVE_FORMAT_PCM = 0x0001;
        public const int WAVE_FORMAT_FLOAT = 0x0003;

        public const int WAVE_MAPPER = -1;

        #region  MMRESULT error return values specific to the mixer API

        public const int MIXERR_INVALLINE = (MIXERR_BASE + 0);
        public const int MIXERR_INVALCONTROL = (MIXERR_BASE + 1);
        public const int MIXERR_INVALVALUE = (MIXERR_BASE + 2);
        public const int MIXERR_LASTERROR = (MIXERR_BASE + 2);

        public const int MIXER_OBJECTF_HANDLE = -0;//0x80000000;
        public const int MIXER_OBJECTF_MIXER = 0x00000000;
        public const int MIXER_OBJECTF_HMIXER = (MIXER_OBJECTF_HANDLE | MIXER_OBJECTF_MIXER);
        public const int MIXER_OBJECTF_WAVEOUT = 0x10000000;
        public const int MIXER_OBJECTF_HWAVEOUT = (MIXER_OBJECTF_HANDLE | MIXER_OBJECTF_WAVEOUT);
        public const int MIXER_OBJECTF_WAVEIN = 0x20000000;
        public const int MIXER_OBJECTF_HWAVEIN = (MIXER_OBJECTF_HANDLE | MIXER_OBJECTF_WAVEIN);
        public const int MIXER_OBJECTF_MIDIOUT = 0x30000000;
        public const int MIXER_OBJECTF_HMIDIOUT = (MIXER_OBJECTF_HANDLE | MIXER_OBJECTF_MIDIOUT);
        public const int MIXER_OBJECTF_MIDIIN = 0x40000000;
        public const int MIXER_OBJECTF_HMIDIIN = (MIXER_OBJECTF_HANDLE | MIXER_OBJECTF_MIDIIN);
        public const int MIXER_OBJECTF_AUX = 0x50000000;
        #endregion

        public const int MIXER_GETLINEINFOF_DESTINATION = 0x00000000;
        public const int MIXER_GETLINEINFOF_SOURCE = 0x00000001;
        public const int MIXER_GETLINEINFOF_LINEID = 0x00000002;
        public const int MIXER_GETLINEINFOF_COMPONENTTYPE = 0x00000003;
        public const int MIXER_GETLINEINFOF_TARGETTYPE = 0x00000004;
        public const int MIXER_GETLINEINFOF_QUERYMASK = 0x0000000F;

        public const int MIXER_GETLINECONTROLSF_ALL = 0x00000000;
        public const int MIXER_GETLINECONTROLSF_ONEBYID = 0x00000001;
        public const int MIXER_GETLINECONTROLSF_ONEBYTYPE = 0x00000002;
        public const int MIXER_GETLINECONTROLSF_QUERYMASK = 0x0000000F;

        public const int MIXER_GETCONTROLDETAILSF_VALUE = 0x00000000;
        public const int MIXER_GETCONTROLDETAILSF_LISTTEXT = 0x00000001;

        public const int MIXER_GETCONTROLDETAILSF_QUERYMASK = 0x0000000F;


        public const int MIXER_SETCONTROLDETAILSF_VALUE = 0x00000000;
        public const int MIXER_SETCONTROLDETAILSF_CUSTOM = 0x00000001;

        public const int MIXER_SETCONTROLDETAILSF_QUERYMASK = 0x0000000F;

        public abstract class ComponentType
        {
            public const int MIXERLINE_COMPONENTTYPE_DST_FIRST = 0x00000000;
            public const int MIXERLINE_COMPONENTTYPE_DST_UNDEFINED = (MIXERLINE_COMPONENTTYPE_DST_FIRST + 0);
            public const int MIXERLINE_COMPONENTTYPE_DST_DIGITAL = (MIXERLINE_COMPONENTTYPE_DST_FIRST + 1);
            public const int MIXERLINE_COMPONENTTYPE_DST_LINE = (MIXERLINE_COMPONENTTYPE_DST_FIRST + 2);
            public const int MIXERLINE_COMPONENTTYPE_DST_MONITOR = (MIXERLINE_COMPONENTTYPE_DST_FIRST + 3);
            public const int MIXERLINE_COMPONENTTYPE_DST_SPEAKERS = (MIXERLINE_COMPONENTTYPE_DST_FIRST + 4);
            public const int MIXERLINE_COMPONENTTYPE_DST_HEADPHONES = (MIXERLINE_COMPONENTTYPE_DST_FIRST + 5);
            public const int MIXERLINE_COMPONENTTYPE_DST_TELEPHONE = (MIXERLINE_COMPONENTTYPE_DST_FIRST + 6);
            public const int MIXERLINE_COMPONENTTYPE_DST_WAVEIN = (MIXERLINE_COMPONENTTYPE_DST_FIRST + 7);
            public const int MIXERLINE_COMPONENTTYPE_DST_VOICEIN = (MIXERLINE_COMPONENTTYPE_DST_FIRST + 8);
            public const int MIXERLINE_COMPONENTTYPE_DST_LAST = (MIXERLINE_COMPONENTTYPE_DST_FIRST + 8);

            public const int MIXERLINE_COMPONENTTYPE_SRC_FIRST = 0x00001000;
            public const int MIXERLINE_COMPONENTTYPE_SRC_UNDEFINED = (MIXERLINE_COMPONENTTYPE_SRC_FIRST + 0);
            public const int MIXERLINE_COMPONENTTYPE_SRC_DIGITAL = (MIXERLINE_COMPONENTTYPE_SRC_FIRST + 1);
            public const int MIXERLINE_COMPONENTTYPE_SRC_LINE = (MIXERLINE_COMPONENTTYPE_SRC_FIRST + 2);
            public const int MIXERLINE_COMPONENTTYPE_SRC_MICROPHONE = (MIXERLINE_COMPONENTTYPE_SRC_FIRST + 3);
            public const int MIXERLINE_COMPONENTTYPE_SRC_SYNTHESIZER = (MIXERLINE_COMPONENTTYPE_SRC_FIRST + 4);
            public const int MIXERLINE_COMPONENTTYPE_SRC_COMPACTDISC = (MIXERLINE_COMPONENTTYPE_SRC_FIRST + 5);
            public const int MIXERLINE_COMPONENTTYPE_SRC_TELEPHONE = (MIXERLINE_COMPONENTTYPE_SRC_FIRST + 6);
            public const int MIXERLINE_COMPONENTTYPE_SRC_PCSPEAKER = (MIXERLINE_COMPONENTTYPE_SRC_FIRST + 7);
            public const int MIXERLINE_COMPONENTTYPE_SRC_WAVEOUT = (MIXERLINE_COMPONENTTYPE_SRC_FIRST + 8);
            public const int MIXERLINE_COMPONENTTYPE_SRC_AUXILIARY = (MIXERLINE_COMPONENTTYPE_SRC_FIRST + 9);
            public const int MIXERLINE_COMPONENTTYPE_SRC_ANALOG = (MIXERLINE_COMPONENTTYPE_SRC_FIRST + 10);
            public const int MIXERLINE_COMPONENTTYPE_SRC_LAST = (MIXERLINE_COMPONENTTYPE_SRC_FIRST + 10);
        }

        public abstract class ControlType
        {
            public const int MIXERCONTROL_CT_CLASS_MASK = -0x70000000;
            public const int MIXERCONTROL_CT_CLASS_CUSTOM = 0x00000000;
            public const int MIXERCONTROL_CT_CLASS_METER = 0x10000000;
            public const int MIXERCONTROL_CT_CLASS_SWITCH = 0x20000000;
            public const int MIXERCONTROL_CT_CLASS_NUMBER = 0x30000000;
            public const int MIXERCONTROL_CT_CLASS_SLIDER = 0x40000000;
            public const int MIXERCONTROL_CT_CLASS_FADER = 0x50000000;
            public const int MIXERCONTROL_CT_CLASS_TIME = 0x60000000;
            public const int MIXERCONTROL_CT_CLASS_LIST = 0x70000000;

            public const int MIXERCONTROL_CT_SUBCLASS_MASK = 0x0F000000;

            public const int MIXERCONTROL_CT_SC_SWITCH_BOOLEAN = 0x00000000;
            public const int MIXERCONTROL_CT_SC_SWITCH_BUTTON = 0x01000000;

            public const int MIXERCONTROL_CT_SC_METER_POLLED = 0x00000000;

            public const int MIXERCONTROL_CT_SC_TIME_MICROSECS = 0x00000000;
            public const int MIXERCONTROL_CT_SC_TIME_MILLISECS = 0x01000000;

            public const int MIXERCONTROL_CT_SC_LIST_SINGLE = 0x00000000;
            public const int MIXERCONTROL_CT_SC_LIST_MULTIPLE = 0x01000000;

            public const int MIXERCONTROL_CT_UNITS_MASK = 0x00FF0000;
            public const int MIXERCONTROL_CT_UNITS_CUSTOM = 0x00000000;
            public const int MIXERCONTROL_CT_UNITS_BOOLEAN = 0x00010000;
            public const int MIXERCONTROL_CT_UNITS_SIGNED = 0x00020000;
            public const int MIXERCONTROL_CT_UNITS_UNSIGNED = 0x00030000;
            public const int MIXERCONTROL_CT_UNITS_DECIBELS = 0x00040000; /* in 10ths */
            public const int MIXERCONTROL_CT_UNITS_PERCENT = 0x00050000; /* in 10ths */

            public const int MIXERCONTROL_CONTROLTYPE_CUSTOM = (MIXERCONTROL_CT_CLASS_CUSTOM | MIXERCONTROL_CT_UNITS_CUSTOM);
            public const int MIXERCONTROL_CONTROLTYPE_BOOLEANMETER = (MIXERCONTROL_CT_CLASS_METER | MIXERCONTROL_CT_SC_METER_POLLED | MIXERCONTROL_CT_UNITS_BOOLEAN);
            public const int MIXERCONTROL_CONTROLTYPE_SIGNEDMETER = (MIXERCONTROL_CT_CLASS_METER | MIXERCONTROL_CT_SC_METER_POLLED | MIXERCONTROL_CT_UNITS_SIGNED);
            public const int MIXERCONTROL_CONTROLTYPE_PEAKMETER = (MIXERCONTROL_CONTROLTYPE_SIGNEDMETER + 1);
            public const int MIXERCONTROL_CONTROLTYPE_UNSIGNEDMETER = (MIXERCONTROL_CT_CLASS_METER | MIXERCONTROL_CT_SC_METER_POLLED | MIXERCONTROL_CT_UNITS_UNSIGNED);
            public const int MIXERCONTROL_CONTROLTYPE_BOOLEAN = (MIXERCONTROL_CT_CLASS_SWITCH | MIXERCONTROL_CT_SC_SWITCH_BOOLEAN | MIXERCONTROL_CT_UNITS_BOOLEAN);
            public const int MIXERCONTROL_CONTROLTYPE_ONOFF = (MIXERCONTROL_CONTROLTYPE_BOOLEAN + 1);
            public const int MIXERCONTROL_CONTROLTYPE_MUTE = (MIXERCONTROL_CONTROLTYPE_BOOLEAN + 2);
            public const int MIXERCONTROL_CONTROLTYPE_MONO = (MIXERCONTROL_CONTROLTYPE_BOOLEAN + 3);
            public const int MIXERCONTROL_CONTROLTYPE_LOUDNESS = (MIXERCONTROL_CONTROLTYPE_BOOLEAN + 4);
            public const int MIXERCONTROL_CONTROLTYPE_STEREOENH = (MIXERCONTROL_CONTROLTYPE_BOOLEAN + 5);
            public const int MIXERCONTROL_CONTROLTYPE_BUTTON = (MIXERCONTROL_CT_CLASS_SWITCH | MIXERCONTROL_CT_SC_SWITCH_BUTTON | MIXERCONTROL_CT_UNITS_BOOLEAN);
            public const int MIXERCONTROL_CONTROLTYPE_DECIBELS = (MIXERCONTROL_CT_CLASS_NUMBER | MIXERCONTROL_CT_UNITS_DECIBELS);
            public const int MIXERCONTROL_CONTROLTYPE_SIGNED = (MIXERCONTROL_CT_CLASS_NUMBER | MIXERCONTROL_CT_UNITS_SIGNED);
            public const int MIXERCONTROL_CONTROLTYPE_UNSIGNED = (MIXERCONTROL_CT_CLASS_NUMBER | MIXERCONTROL_CT_UNITS_UNSIGNED);
            public const int MIXERCONTROL_CONTROLTYPE_PERCENT = (MIXERCONTROL_CT_CLASS_NUMBER | MIXERCONTROL_CT_UNITS_PERCENT);
            public const int MIXERCONTROL_CONTROLTYPE_SLIDER = (MIXERCONTROL_CT_CLASS_SLIDER | MIXERCONTROL_CT_UNITS_SIGNED);
            public const int MIXERCONTROL_CONTROLTYPE_PAN = (MIXERCONTROL_CONTROLTYPE_SLIDER + 1);
            public const int MIXERCONTROL_CONTROLTYPE_QSOUNDPAN = (MIXERCONTROL_CONTROLTYPE_SLIDER + 2);
            public const int MIXERCONTROL_CONTROLTYPE_FADER = (MIXERCONTROL_CT_CLASS_FADER | MIXERCONTROL_CT_UNITS_UNSIGNED);
            public const int MIXERCONTROL_CONTROLTYPE_VOLUME = (MIXERCONTROL_CONTROLTYPE_FADER + 1);
            public const int MIXERCONTROL_CONTROLTYPE_BASS = (MIXERCONTROL_CONTROLTYPE_FADER + 2);
            public const int MIXERCONTROL_CONTROLTYPE_TREBLE = (MIXERCONTROL_CONTROLTYPE_FADER + 3);
            public const int MIXERCONTROL_CONTROLTYPE_EQUALIZER = (MIXERCONTROL_CONTROLTYPE_FADER + 4);
            public const int MIXERCONTROL_CONTROLTYPE_SINGLESELECT = (MIXERCONTROL_CT_CLASS_LIST | MIXERCONTROL_CT_SC_LIST_SINGLE | MIXERCONTROL_CT_UNITS_BOOLEAN);
            public const int MIXERCONTROL_CONTROLTYPE_MUX = (MIXERCONTROL_CONTROLTYPE_SINGLESELECT + 1);
            public const int MIXERCONTROL_CONTROLTYPE_MULTIPLESELECT = (MIXERCONTROL_CT_CLASS_LIST | MIXERCONTROL_CT_SC_LIST_MULTIPLE | MIXERCONTROL_CT_UNITS_BOOLEAN);
            public const int MIXERCONTROL_CONTROLTYPE_MIXER = (MIXERCONTROL_CONTROLTYPE_MULTIPLESELECT + 1);
            public const int MIXERCONTROL_CONTROLTYPE_MICROTIME = (MIXERCONTROL_CT_CLASS_TIME | MIXERCONTROL_CT_SC_TIME_MICROSECS | MIXERCONTROL_CT_UNITS_UNSIGNED);
            public const int MIXERCONTROL_CONTROLTYPE_MILLITIME = (MIXERCONTROL_CT_CLASS_TIME | MIXERCONTROL_CT_SC_TIME_MILLISECS | MIXERCONTROL_CT_UNITS_UNSIGNED);
        }

        #endregion

        public delegate void WaveProcDelegate(IntPtr hWave, int uMsg, int dwUser, ref WAVEHDR wavhdr, int dwParam2);

        #region /// functions ///

        private const string mmdll = "winmm.dll";

        #region waveIn functions

        [DllImport(mmdll)]
        public static extern int waveInGetNumDevs();
        [DllImport(mmdll)]
        public static extern int waveInGetDevCaps(int uDeviceID, ref WAVEINCAPS pwic, int cbwic);
        [DllImport(mmdll)]
        public static extern int waveInAddBuffer(IntPtr hwi, IntPtr pwh, int cbwh);
        [DllImport(mmdll)]
        public static extern int waveInClose(IntPtr hwi);
        [DllImport(mmdll)]
        public static extern int waveInOpen(out IntPtr phwi, int uDeviceID, ref WAVEFORMATEX lpFormat, WaveProcDelegate dwCallback, int dwInstance, int dwFlags);
        [DllImport(mmdll)]
        public static extern int waveInPrepareHeader(IntPtr hWaveIn, IntPtr lpWaveInHdr, int cbwh);
        [DllImport(mmdll)]
        public static extern int waveInUnprepareHeader(IntPtr hWaveIn, IntPtr lpWaveInHdr, int cbwh);
        [DllImport(mmdll)]
        public static extern int waveInReset(IntPtr hwi);
        [DllImport(mmdll)]
        public static extern int waveInStart(IntPtr hwi);
        [DllImport(mmdll)]
        public static extern int waveInStop(IntPtr hwi);

        #endregion

        #region waveOut functions

        /// <summary>
        /// Возвращает количество устройств вывода.
        /// </summary>
        /// <returns>Количество устройств.</returns>
        [DllImport(mmdll)]
        public static extern int waveOutGetNumDevs();
        /// <summary>
        /// Запрос параметров и возможностей устройства.
        /// </summary>
        /// <param name="uDeviceID">Номер устройства начиная с нуля, либо ключ ранее открытого устройства, 
        /// либо константа WAVE_MAPPER. В последнем случае возвращаються параметры стандартного системного устройства.</param>
        /// <param name="pwoc">Указатель на структуру <see cref="WAVEOUTCAPS"/>.</param>
        /// <param name="cbwoc">Размер структуры в байтах.</param>
        /// <returns>MMRESULT</returns>
        [DllImport(mmdll)]
        public static extern int waveOutGetDevCaps(int uDeviceID, ref WAVEOUTCAPS pwoc, int cbwoc);
        /// <summary>
        /// Закрытие устройства.
        /// </summary>
        /// <param name="hWaveOut">Указатель ключа открытого устройства.</param>
        /// <returns>MMRESULT</returns>
        [DllImport(mmdll)]
        public static extern int waveOutClose(IntPtr hWaveOut);
        /// <summary>
        /// Открытие устройства.
        /// </summary>
        /// <param name="phwo">Указатель куда при успешном завершении опреации записывается ключ открытого устройства.</param>
        /// <param name="uDeviceID">Номер устройства начиная с нуля, либо ключ ранее открытого устройства, 
        /// либо константа WAVE_MAPPER. В последнем случае службой переназначения выбирается устройство, 
        /// поддерживающее заданный формат, причем поиск начинается со стандартного системного устройства.</param>
        /// <param name="lpFormat">Указатель на структуру <see cref="WAVEFORMATEX"/>, 
        /// описывающую формат потока.</param>
        /// <param name="dwCallback">Делегат функции, куда будут передаваться уведомления о запрошенных опрациях.</param>
        /// <param name="dwInstance">32 разрядное слово, которое будет передаваться драйвером в параметрах 
        /// вызова функции уведомления.</param>
        /// <param name="dwFlags">Флаги режимов открывания и работы устройства.</param>
        /// <returns>MMRESULT</returns>
        [DllImport(mmdll)]
        public static extern int waveOutOpen(out IntPtr phwo, int uDeviceID, ref WAVEFORMATEX lpFormat, WaveProcDelegate dwCallback, int dwInstance, int dwFlags);
        /// <summary>
        /// Подготовка (фиксация в памяти) звукового буфера.
        /// </summary>
        /// <param name="hWaveOut">Указатель ключа открытого устройства.</param>
        /// <param name="lpWaveOutHdr">Указатель на структру <see cref="WAVEHDR"/>.</param>
        /// <param name="cbwh">Размер структуры.</param>
        /// <returns>MMRESULT</returns>
        [DllImport(mmdll)]
        public static extern int waveOutPrepareHeader(IntPtr hWaveOut, IntPtr lpWaveOutHdr, int cbwh);
        /// <summary>
        /// Освобождение (снятие фиксации) звукового буфера.
        /// </summary>
        /// <param name="hWaveOut">Указатель ключа открытого устройства.</param>
        /// <param name="lpWaveOutHdr">Указатель на структру <see cref="WAVEHDR"/>.</param>
        /// <param name="cbwh">Размер структуры.</param>
        /// <returns>MMRESULT</returns>
        [DllImport(mmdll)]
        public static extern int waveOutUnprepareHeader(IntPtr hWaveOut, IntPtr lpWaveOutHdr, int cbwh);
        /// <summary>
        /// Передача очередного буфера драйверу устройства.
        /// </summary>
        /// <param name="hWaveOut">Указатель ключа открытого устройства.</param>
        /// <param name="lpWaveOutHdr">Указатель на структру <see cref="WAVEHDR"/>.</param>
        /// <param name="uSize">Размер структуры.</param>
        /// <returns>MMRESULT</returns>
        [DllImport(mmdll)]
        public static extern int waveOutWrite(IntPtr hWaveOut, IntPtr lpWaveOutHdr, int uSize);
        /// <summary>
        /// Сброс потока.
        /// </summary>
        /// <param name="hWaveOut">Указатель ключа открытого устройства.</param>
        /// <returns>MMRESULT</returns>
        [DllImport(mmdll)]
        public static extern int waveOutReset(IntPtr hWaveOut);
        /// <summary>
        /// Установка громкости воспроизведения.
        /// </summary>
        /// <param name="hWaveOut">Указатель ключа открытого устройства.</param>
        /// <param name="dwVolume">Громкость по левому и правому каналу. Младшее слово задает громкость 
        /// левого канала, страшее - правого. Максимальная громкось 0xFFFF, минимальная 0.</param>
        /// <returns>MMRESULT</returns>
        [DllImport(mmdll)]
        public static extern int waveOutSetVolume(IntPtr hWaveOut, int dwVolume);
        /// <summary>
        /// Запрос громкости воспроизведения.
        /// </summary>
        /// <param name="hWaveOut">Указатель ключа открытого устройства.</param>
        /// <param name="dwVolume">Громкость по левому и правому каналу. Младшее слово задает громкость 
        /// левого канала, страшее - правого. Максимальная громкось 0xFFFF, минимальная 0.</param>
        /// <returns>MMRESULT</returns>
        [DllImport(mmdll)]
        public static extern int waveOutGetVolume(IntPtr hWaveOut, out int dwVolume);

        #endregion

        #region mixer functions

        [DllImport(mmdll)]
        public static extern int mixerOpen(out IntPtr phmx, uint uMxId, IntPtr dwCallback, IntPtr dwInstance, int fdwOpen);
        [DllImport(mmdll)]
        public static extern int mixerClose(IntPtr hmx);
        [DllImport(mmdll)]
        public static extern int mixerMessage(IntPtr hmx, int uMsg, int dwParam1, int dwParam2);
        [DllImport(mmdll)]
        public static extern int mixerGetLineInfo(IntPtr hmxobj, ref MIXERLINE pmxl, int fdwInfo);
        [DllImport(mmdll)]
        public static extern int mixerGetLineControls(IntPtr hmxobj, ref MIXERLINECONTROLS pmxlc, int fdwControls);
        [DllImport(mmdll)]
        public static extern int mixerGetControlDetails(IntPtr hmxobj, ref MIXERCONTROLDETAILS pmxcd, int fdwDetails);
        [DllImport(mmdll)]
        public static extern int mixerSetControlDetails(IntPtr hmxobj, ref MIXERCONTROLDETAILS pmxcd, int fdwDetails);

        #endregion

        #endregion

    }
}
