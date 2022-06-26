using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.Drawing.Drawing2D;

namespace ConfigurationLib
{
    public delegate void LangEventHandler();
    public delegate void CompleteProcessEventHandler();
    public delegate void SuccessEventHandler();
    public delegate void CancelEventHandler();
    public delegate void SpeechRecognitionEventHandler(string pText);
    public delegate void dlgReturnString(string pText);
    public delegate void dlgExitHandler();
    public delegate void FuncKeyReturn(string pFuncGrp,string pKeyCode);

    public class SYSGlobalVariable
    {
        public static string _ConnectionString = "";
        /* SYSTEM SETTING  */
        public static string CompNo = "0";
        public static string cmCode; // COMPUTER CODE
        public static string usCode; // USER CODE
        public static string lgCode; 
        public static string nlCode; // LANGUAGE CODE
        public static string shpCode;  // SHOP CODE
        public static string posCode;
        public static string SysRegKey = "";
        public static string roleName = "";

        public static string RS_URL = "";
        public static string RS_SERVER = "";
        public static string RS_USER = "";
        public static string RS_PASSWORD = "";
        public static string RS_RPTPATH = "";


        public static string _WebCam1 = "";

        public static string _SoundRecordPath = "C:\\TEMP\\SOUNDRECORD\\";

        public static string _ReportFolder = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\REPORT";
        public static string _WebServicePath = "";
        public static bool _IsWebServiceMode = false;

        public static bool _IsTestMode = false;
        public static bool _IsSysRunning = false;
        public static bool  _IsUATMode = false;
        public static Font _SysGeneralFont = new Font(FontFamily.GenericSansSerif, 10F);
        public static string _PLAY_SPEECH_PATH = "";
        public static string _IS_AUTO_RESTART = "F";
     
        public static string _SystemEncodeKey = "ShutDownCm";
        public static int _DecimalLength = 2;
        public static string _DateTimeFormat = "yyyy-MM-dd'T'HH:mm:ss";  //1/1/1900 12:00:00
        public static string _SDateTimeFormat = "yyyy-MM-dd'T'HH:mm:ss";
        public static string _LDateTimeFormat = "dd/MM/yyyy";
        public static string _SDateFormat = "yyyyMMdd";   //Short Date Format
        public static string _SDateFormatDisplay = "yyyy/MM/dd";
        public static string _LDateFormat = "YYYYMMDDHHMMSS";   //Long Date Format
        public static string _CurrPrefix = "$";
        public static int _iCurrDigitalPosition = 2;
        public static int _iCurrDisplayLen = 1000;

        public static Int32 QU_MAIN_SCREEN_DISPLAY_INDEX = 0;

        public static Font QU_Ticket_Font = new Font(FontFamily.GenericSansSerif, 48.0F);
        public static string QU_Ticket_Logo_Path = "";
        public static Int32 QU_Ticket_Logo_Height = 350;
        public static Int32 QU_Ticket_Logo_Width = 195;
        public static string QU_Ticket_Message = "";
        public static Font QU_Ticket_Message_Font = new Font(FontFamily.GenericSansSerif, 12.0F);
        public static string QU_Queue_Speak_Message = "";
        public static string QU_Display_TicketNo_Image_Path = "";
        public static Image QU_Display_TicketNo_Image;

        public static string LangSpeaker_Chinese = "ScanSoft Sin-Ji_Full_22kHz";
        public static string LangSpeaker_English = "ScanSoft Sin-Ji_Full_22kHz";

        public static Color _BtnGlowColor = ColorTranslator.FromHtml("#90bdff");
        public static Color _BtnButtonColor = ColorTranslator.FromHtml("#1e90ff");
        public static Color _BtnBaseColor = ColorTranslator.FromHtml("#afeeee");

        public static Color _ColorTitleText = ColorTranslator.FromHtml("#175673");
        public static Color _Color = Color.LightBlue;
        public static Color _Color0 = ColorTranslator.FromHtml("#f7f7f7");
        public static Color _Color1 = ColorTranslator.FromHtml("#eef3f4");
        public static Color _Color2 = ColorTranslator.FromHtml("#9fcde2");
        public static Color _Color3 = ColorTranslator.FromHtml("#def0f8");
        public static Color _Color4 = ColorTranslator.FromHtml("#bde5f1");
        public static Color _ColorBackground1;
        public static Color _ColorBackground2;
        public static Color _ColorButtonBackground1;
        public static Color _ColorButtonBackground2;
        public static Color _ColorButtonBorderColor;
        public static Color _ColorMin = Color.LightBlue;
        public static Color _ColorMinBg = ColorTranslator.FromHtml("#6e95bf");
        public static Color _ColorMin1 = ColorTranslator.FromHtml("#cfd2d7");
        public static Color _ColorMin2 = ColorTranslator.FromHtml("#9fcde2");
        public static Color _ColorMin3 = ColorTranslator.FromHtml("#ecedef");
        public static Color _ColorBox1 = ColorTranslator.FromHtml("#ecf5f7");
        public static Color _ColorBox2 = ColorTranslator.FromHtml("#c5dde2");
        public static Color _ColorBox3 = ColorTranslator.FromHtml("#5db6d7");
        public static Color _ColorMinButtonBorderColor;
        public static Color _ColorBrushColor = ColorTranslator.FromHtml("#175673");
        public static Color _ColorGrpBox1;
        public static Color _ColorGrpBox2;
        public static Color _ColorGrpBox3;
        public static Color _ColorButton1;
        public static Color _ColorButton2;
        public static Color _ColorButton3;
        public static Color _ColorTop;
        public static Color _ColorBelow;


    }




}
