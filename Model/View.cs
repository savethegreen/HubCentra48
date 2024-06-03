using LiveChartsCore.SkiaSharpView;
using LiveChartsCore;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static HubCentra_A1.EnumManager;
using System.Net;

namespace HubCentra_A1.Model
{
    public class View
    {

        public event PropertyChangedEventHandler? PropertyChanged;

        public virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #region Common
        public int Common_CellCount { get; set; } = 28;
        public int Common_SystemCellCount { get; set; } = 84;
        public int Common_TotalSystemCellCount { get; set; } = 336;
        #endregion Common

        #region Loading
        public int Loading_formLoadingProgress { get; set; } = 11;
        public int Loading_formLoadingProgress2 { get; set; } = 0;
        #endregion Loading

        #region MainWindow
        public Enum_MainWindow_ButtonFlag MainWindow_ButtonFlag { get; set; } =  Enum_MainWindow_ButtonFlag.Home;
        #endregion MainWindow

        #region MainEngine
        public ConcurrentQueue<int> ConcurrentQueue_MainEngine_Unloading_Positive { get; set; } = new ConcurrentQueue<int>();
        public ConcurrentQueue<int> ConcurrentQueue_MainEngine_Unloading_Negative { get; set; } = new ConcurrentQueue<int>();
        public ConcurrentQueue<int> ConcurrentQueue_MainEngine_Unloading_Incubation { get; set; } = new ConcurrentQueue<int>();

        public List<MainEngine_StatusList> MainEngine_Statuslist { get; set; }

        public int MainEngine_CurrentNum { get; set; } = 0;
        public bool MainEngine_LoadCell_Result { get; set; } = false;
        public bool MainEngine_Incubation_Result { get; set; } = false;
        public bool MainEngine_EmptyEnabled { get; set; } = false;

        public string MainEngine_Result { get; set; } = "Incubation";

        public bool MainEngine_Positive_Index_Left { get; set; } = false;
        public bool MainEngine_Positive_Index_Right { get; set; } = false;

        public bool MainEngine_Error_Index_Left { get; set; } = false;
        public bool MainEngine_Error_Index_Right { get; set; } = false;

        public int MainEngine_index { get; set; } = 0; //Positive, Error 포지션 인덱스


        public double MainEngine_LoadCell_Value { get; set; } = 0; //로드셀 값
        #endregion MainEngine

        #region Login
        public Enum_Login Login { get; set; } = Enum_Login.OPERATOR;
        public string LoginID { get; set; } = "";
        public string Password { get; set; } = "";
        #endregion Login

        #region FASTECH
        #region Connection
        public IPAddress FASTECH_IO_IP { get; set; } = new IPAddress(new byte[] { 192, 168, 0, 3 });
        public bool FASTECH_IO_Connection { get; set; } = false;
        #endregion Connection


        #region IO
        public List<Class_FASTECH_Input> FASTECH_Input { get; set; }
        public List<Class_FASTECH_Output> FASTECH_Output { get; set; }

        public List<Class_FASTECH_Output> FASTECH_Set_Output { get; set; }
        #endregion IO
        #endregion  FASTECH

        #region PCB
        public SerialPort PCB_SerialPort { get; set; }
        public bool PCB_Connection { get; set; } = false;
        public bool PCB_Status { get; set; } = false;
        public string PCB_ID { get; set; } = "$ID1";

        public List<PCB> PCB_Data { get; set; }

        public List<PCB_cell_alive_C> PCB_cell_alive { get; set; }

        public Dictionary<int, Dictionary<int, List<List<double>>>> PCB_CellReadings { get; set; } = new Dictionary<int, Dictionary<int, List<List<double>>>>();


        public List<MatchEquipmentDataWithDB_C> Equipment_DataWithDB_presenceArray { get; set; }

        public int[] PositiveDelay { get; set; } = new int[336];

        public ConcurrentQueue<string> Queue_PCB_Manual { get; set; } = new ConcurrentQueue<string>();
        public int testint { get; set; } = 1;
        public string teststr { get; set; } = "444";

        public bool PCB1_targetvalue_test { get; set; } = false;
        public double PCB_targetvalue { get; set; } = 900;

        #endregion PCB

        #region Barcode
        public SerialPort Barcode_SerialPort { get; set; }
        public string Barcode_ID_Loading { get; set; } = "";
        public string Barcode_ID { get; set; } = "";
        public string Patient_ID { get; set; } = "";

        
        public bool Barcode_Connection { get; set; } = false;
        #endregion Barcode

        #region Temperature
        public SerialPort Temperature_SerialPort { get; set; }
        public double Temperature_ProcessValue { get; set; } = 0;
        public bool Temperature_Connection { get; set; } = false;
        #endregion Temperature

        #region DatabaseManager
        public bool DatabaseManager_Connection { get; set; } = false;
        public List<DatabaseManager_System> SystemInfo { get; set; }

        public List<DatabaseManager_Login> LoginInfo { get; set; }
        public List<DatabaseManager_FASTECH_Parameter> FASTECHInfo { get; set; }

        public List<DatabaseManager_Equipment> EquipmentInfo { get; set; }

        public double DatabaseManager_CmdPos { get; set; } = 0;
        public string _DatabaseManager_CmdPos_Str { get; set; } = "";
        #endregion  DatabaseManager

        #region Home
        public int Home_Available { get; set; } = 0;
        public bool Home_Available_Flag { get; set; } = false;
        public int Home_Incubation { get; set; } = 0;
        public bool Home_Incubation_Flag { get; set; } = false;
        public int Home_Positive { get; set; } = 0;
        public bool Home_Positive_Flag { get; set; } = false;
        public int Home_Negative { get; set; } = 0;
        public bool Home_Negative_Flag { get; set; } = false;
        #endregion Home

        #region Config
        public List<Class_Config> Config { get; set; }
        #endregion Config

        #region Search
        public List<DatabaseManager_Equipment> Search_List { get; set; }
        #endregion Search

        #region Report
        public List<DatabaseManager_BarcodeList> Report_List { get; set; }
        public List<DatabaseManager_EquipmentH> CSV_List { get; set; }

        public Enum_Report_Model Report_Model { get; set; } = Enum_Report_Model.raw;

        public string Report_Barcode { get; set; } = "";

        public int Report_AverageValue { get; set; } = 20;
        public DateTime Report_DatePicke_Start { get; set; } = DateTime.Now;
        public DateTime Report_DatePicke_End { get; set; } = DateTime.Now;

        public DateTime MyDatePicke_Start { get; set; } = DateTime.Today;
        public DateTime MyDatePicke_End { get; set; } = DateTime.Today.AddDays(1).AddTicks(-1);
        #endregion Report

        #region LiveCharts
        public List<DatabaseManager_EquipmentH> LiveCharts_List { get; set; }
        public Dictionary<int, Queue<(DateTime, double)>> LiveCharts_TimeSeries;
        public ISeries[] Series { get; set; }
        public RectangularSection[] Sections { get; set; }
        public Axis[] XAxes { get; set; }
        public Axis[] YAxes { get; set; }

        public int LiveCharts_Positive_Start { get; set; } = 0;
        public int LiveCharts_Positive_End { get; set; } = 0;

        public int LiveCharts_Positive_Index { get; set; } = 0;

        public string LiveCharts_Positive_Time { get; set; } = "";
        #endregion LiveCharts

        #region Calculator
        public string Calculator_DisplayTextBlock { get; set; } = "";
        #endregion  Calculator

        #region System

        #region System1
        public List<PositiveFirst_C> System_PositiveFirst { get; set; }
        public bool System1_HasPositive { get; set; } = false;
        public string System1_Positive_Warning { get; set; } = "";
        public string System1_Positive_Cel { get; set; } = "";
        public int System_PositiveFirstint { get; set; } = -1;
        #endregion System1

        #region System2
        #endregion System2

        #region System3
        #endregion System3

        #region System4
        #endregion System4
        #endregion System

        #region WriteBarcode
        public string WriteBarcode_ID { get; set; } = "";
        public string WritePatient_ID { get; set; } = "";
        #endregion WriteBarcode

        #region Alarm
        #region BottleLoading
        public ConcurrentQueue<Tuple<int>> Alarm_BottleLoading { get; set; } = new ConcurrentQueue<Tuple<int>>();


        public HashSet<int> Alarm_BottleLoading_Set = new HashSet<int>();

        public bool[] BottleLoading_Result { get; set; } = new bool[336];
        public bool BottleLoading_isPopupOpen { get; set; } = false;
        public string BottleLoading_BarcodeID { get; set; } = "";
        public string BottleLoading_Title { get; set; } = "";
        public string BottleLoading_Content { get; set; } = "";
        public string BottleLoading_WhatSystem { get; set; } = "";
        public string BottleLoading_Cell_Num { get; set; } = "";
        #endregion BottleLoading

        #region Barcode
        public ConcurrentQueue<Tuple<string>> Alarm_Barcode { get; set; } = new ConcurrentQueue<Tuple<string>>();
        public bool Alarm_Barcode_isPopupOpen { get; set; } = false;
        public string Barcode_BarcodeID { get; set; } = "";
        public string Barcode_Content { get; set; } = "";
        #endregion Barcode

        #region Incubation
        public string Alarm_Incubation_BarcodeID { get; set; } = "";
        public string Alarm_Incubation_Title { get; set; } = "Bottle Unloading";
        public string Alarm_Incubation_whatSystem { get; set; } = "";
        public string Alarm_Incubation_Cell { get; set; } = "";

        public string Alarm_Incubation_Content { get; set; } = "배양이 종료되지 않았습니다." + "\n" +
                                                    "정말로 제거하시겠습니까?";
        #endregion Incubation

        #region Positive
        public ConcurrentQueue<Tuple<int>> Alarm_Positive { get; set; } = new ConcurrentQueue<Tuple<int>>();
        public string Alarm_Positive_whatSystem { get; set; } = "";
        public string Alarm_Positive_Warning { get; set; } = "";
        public string Alarm_Positive_Cell { get; set; } = "";
        #endregion Positive

        #region Positive_Unloading
        public string Alarm_Positive_Unloading_Title { get; set; } = "Bottle Unloading";
        public string Alarm_Positive_Unloading_whatSystem { get; set; } = "";
        public string Alarm_Positive_Unloading_Cell { get; set; } = "";
        public string Alarm_Positive_Unloading_BarcodeID { get; set; } = "";
        public string Alarm_Positive_Unloading_Warning { get; set; } = "Bottle Unloading";
        #endregion Positive_Unloading


        public bool PopStatus_Positive_Flag { get; set; } = false;
        public string PopStatus_Positive_Title { get; set; } = "Positive 발생!!";



        public string PopStatus_Error_Bottle_Title { get; set; } = "Error Bottle!!";
        public string PopStatus_Error_Bottle { get; set; } = "Error Bottle을 제거 해주세요!!";
        public bool PopStatus_Error_Bottle_Flag { get; set; } = false;


        public string PopStatus_Positive_Bottle_Title { get; set; } = "Positive Bottle!!";
        public string PopStatus_Positive_Bottle { get; set; } = "Positive Bottle을 제거 해주세요!!";
        public bool PopStatus_Positive_Bottle_Flag { get; set; } = false;


        public string PopStatus_TrashCanAlert_Title { get; set; } = "Trash!!";
        public string PopStatus_TrashCanAlert { get; set; } = "휴지통을 제거 해주세요!!";
        public bool PopStatus_TrashCanAlert_Flag { get; set; } = false;
        #endregion Alarm

        #region Class
        #region DatabaseManager_System
        public class DatabaseManager_System : INotifyPropertyChanged
        {
            public event PropertyChangedEventHandler PropertyChanged;
            protected virtual void OnPropertyChanged(string propertyName)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
            public string _FASTECH_Servo_IP;
            public string _FASTECH_IO_Input_IP;
            public string _FASTECH_IO_Output_IP;
            public string _Schneider_Robot_IP;
            public int _Schneider_Robot_Port;
            public string _PCB_ID1;
            public string _PCB_ID2;
            public string _PCB_ID3;
            public string _PCB_ID4;
            public string _PCB_Serial;
            public string _Temperature_Serial;
            public string _Barcode_Serial;
            public string _Loadcell_Serial;
            public string _Spare1;
            public string _Spare2;

            public string FASTECH_Servo_IP
            {
                get => _FASTECH_Servo_IP;
                set
                {
                    if (_FASTECH_Servo_IP != value)
                    {
                        _FASTECH_Servo_IP = value;
                        OnPropertyChanged(nameof(FASTECH_Servo_IP));
                    }
                }
            }
            public string FASTECH_IO_Input_IP
            {
                get => _FASTECH_IO_Input_IP;
                set
                {
                    if (_FASTECH_IO_Input_IP != value)
                    {
                        _FASTECH_IO_Input_IP = value;
                        OnPropertyChanged(nameof(FASTECH_IO_Input_IP));
                    }
                }
            }
            public string FASTECH_IO_Output_IP
            {
                get => _FASTECH_IO_Output_IP;
                set
                {
                    if (_FASTECH_IO_Output_IP != value)
                    {
                        _FASTECH_IO_Output_IP = value;
                        OnPropertyChanged(nameof(FASTECH_IO_Output_IP));
                    }
                }
            }
            public string Schneider_Robot_IP
            {
                get => _Schneider_Robot_IP;
                set
                {
                    if (_Schneider_Robot_IP != value)
                    {
                        _Schneider_Robot_IP = value;
                        OnPropertyChanged(nameof(Schneider_Robot_IP));
                    }
                }
            }
            public int Schneider_Robot_Port
            {
                get => _Schneider_Robot_Port;
                set
                {
                    if (_Schneider_Robot_Port != value)
                    {
                        _Schneider_Robot_Port = value;
                        OnPropertyChanged(nameof(Schneider_Robot_Port));
                    }
                }
            }
            public string PCB_ID1
            {
                get => _PCB_ID1;
                set
                {
                    if (_PCB_ID1 != value)
                    {
                        _PCB_ID1 = value;
                        OnPropertyChanged(nameof(PCB_ID1));
                    }
                }
            }
            public string PCB_ID2
            {
                get => _PCB_ID2;
                set
                {
                    if (_PCB_ID2 != value)
                    {
                        _PCB_ID2 = value;
                        OnPropertyChanged(nameof(PCB_ID2));
                    }
                }
            }
            public string PCB_ID3
            {
                get => _PCB_ID3;
                set
                {
                    if (_PCB_ID3 != value)
                    {
                        _PCB_ID3 = value;
                        OnPropertyChanged(nameof(PCB_ID3));
                    }
                }
            }
            public string PCB_ID4
            {
                get => _PCB_ID4;
                set
                {
                    if (_PCB_ID4 != value)
                    {
                        _PCB_ID4 = value;
                        OnPropertyChanged(nameof(PCB_ID4));
                    }
                }
            }

            public string PCB_Serial
            {
                get => _PCB_Serial;
                set
                {
                    if (_PCB_Serial != value)
                    {
                        _PCB_Serial = value;
                        OnPropertyChanged(nameof(PCB_Serial));
                    }
                }
            }
            public string Temperature_Serial
            {
                get => _Temperature_Serial;
                set
                {
                    if (_Temperature_Serial != value)
                    {
                        _Temperature_Serial = value;
                        OnPropertyChanged(nameof(Temperature_Serial));
                    }
                }
            }
            public string Barcode_Serial
            {
                get => _Barcode_Serial;
                set
                {
                    if (_Barcode_Serial != value)
                    {
                        _Barcode_Serial = value;
                        OnPropertyChanged(nameof(Barcode_Serial));
                    }
                }
            }
            public string Loadcell_Serial
            {
                get => _Loadcell_Serial;
                set
                {
                    if (_Loadcell_Serial != value)
                    {
                        _Loadcell_Serial = value;
                        OnPropertyChanged(nameof(Loadcell_Serial));
                    }
                }
            }
            public string Spare1
            {
                get => _Spare1;
                set
                {
                    if (_Spare1 != value)
                    {
                        _Spare1 = value;
                        OnPropertyChanged(nameof(Spare1));
                    }
                }
            }
            public string Spare2
            {
                get => _Spare2;
                set
                {
                    if (_Spare2 != value)
                    {
                        _Spare2 = value;
                        OnPropertyChanged(nameof(Spare2));
                    }
                }
            }
        }
        #endregion DatabaseManager_System

        #region DatabaseManager_Login
        public class DatabaseManager_Login : INotifyPropertyChanged
        {
            public event PropertyChangedEventHandler PropertyChanged;
            protected virtual void OnPropertyChanged(string propertyName)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
            public string _User_Id;
            public string _User_Level;
            public string _User_Password;
            public bool _User_Enable;

            public string User_Id
            {
                get => _User_Id;
                set
                {
                    if (_User_Id != value)
                    {
                        _User_Id = value;
                        OnPropertyChanged(nameof(User_Id));
                    }
                }
            }
            public string User_Level
            {
                get => _User_Level;
                set
                {
                    if (_User_Level != value)
                    {
                        _User_Level = value;
                        OnPropertyChanged(nameof(User_Level));
                    }
                }
            }
            public string User_Password
            {
                get => _User_Password;
                set
                {
                    if (_User_Password != value)
                    {
                        _User_Password = value;
                        OnPropertyChanged(nameof(User_Password));
                    }
                }
            }

            public bool User_Enable
            {
                get => _User_Enable;
                set
                {
                    if (_User_Enable != value)
                    {
                        _User_Enable = value;
                        OnPropertyChanged(nameof(User_Enable));
                    }
                }
            }

        }
        #endregion DatabaseManager_System

        #region DatabaseManager_FASTECH
        #region Parameter
        public class DatabaseManager_FASTECH_Parameter : INotifyPropertyChanged
        {
            public event PropertyChangedEventHandler PropertyChanged;
            protected virtual void OnPropertyChanged(string propertyName)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
            public int _ID;
            public int _Axis_Acc_Time;
            public int _Axis_Dec_Time;
            public int _Axis_Speed;
            public int _Jog_Speed_Low;
            public int _Jog_Speed_Middle;
            public int _Jog_Speed_High;
            public int _Jog_Acc_Dec_Time;
            public int _Org_Speed;
            public int _Org_Search_Speed;
            public int _Org_Acc_Dec_Time;
            public int _Org_Dethod;
            public int _Org_Dir;
            public int _Org_Offset;
            public int _Motion_Dir;
            public string _Spare1;
            public string _Spare2;
            public string _Spare3;
            public string _Spare4;
            public string _Spare5;

            public int ID
            {
                get => _ID;
                set
                {
                    if (_ID != value)
                    {
                        _ID = value;
                        OnPropertyChanged(nameof(ID));
                    }
                }
            }


            public int Axis_Acc_Time
            {
                get => _Axis_Acc_Time;
                set
                {
                    if (_Axis_Acc_Time != value)
                    {
                        _Axis_Acc_Time = value;
                        OnPropertyChanged(nameof(Axis_Acc_Time));
                    }
                }
            }

            public int Axis_Dec_Time
            {
                get => _Axis_Dec_Time;
                set
                {
                    if (_Axis_Dec_Time != value)
                    {
                        _Axis_Dec_Time = value;
                        OnPropertyChanged(nameof(Axis_Dec_Time));
                    }
                }
            }

            public int Axis_Speed
            {
                get => _Axis_Speed;
                set
                {
                    if (_Axis_Speed != value)
                    {
                        _Axis_Speed = value;
                        OnPropertyChanged(nameof(Axis_Speed));
                    }
                }
            }

            public int Jog_Speed_Low
            {
                get => _Jog_Speed_Low;
                set
                {
                    if (_Jog_Speed_Low != value)
                    {
                        _Jog_Speed_Low = value;
                        OnPropertyChanged(nameof(Jog_Speed_Low));
                    }
                }
            }

            public int Jog_Speed_Middle
            {
                get => _Jog_Speed_Middle;
                set
                {
                    if (_Jog_Speed_Middle != value)
                    {
                        _Jog_Speed_Middle = value;
                        OnPropertyChanged(nameof(Jog_Speed_Middle));
                    }
                }
            }

            public int Jog_Speed_High
            {
                get => _Jog_Speed_High;
                set
                {
                    if (_Jog_Speed_High != value)
                    {
                        _Jog_Speed_High = value;
                        OnPropertyChanged(nameof(Jog_Speed_High));
                    }
                }
            }

            public int Jog_Acc_Dec_Time
            {
                get => _Jog_Acc_Dec_Time;
                set
                {
                    if (_Jog_Acc_Dec_Time != value)
                    {
                        _Jog_Acc_Dec_Time = value;
                        OnPropertyChanged(nameof(Jog_Acc_Dec_Time));
                    }
                }
            }

            public int Org_Speed
            {
                get => _Org_Speed;
                set
                {
                    if (_Org_Speed != value)
                    {
                        _Org_Speed = value;
                        OnPropertyChanged(nameof(Org_Speed));
                    }
                }
            }

            public int Org_Search_Speed
            {
                get => _Org_Search_Speed;
                set
                {
                    if (_Org_Search_Speed != value)
                    {
                        _Org_Search_Speed = value;
                        OnPropertyChanged(nameof(Org_Search_Speed));
                    }
                }
            }

            public int Org_Acc_Dec_Time
            {
                get => _Org_Acc_Dec_Time;
                set
                {
                    if (_Org_Acc_Dec_Time != value)
                    {
                        _Org_Acc_Dec_Time = value;
                        OnPropertyChanged(nameof(Org_Acc_Dec_Time));
                    }
                }
            }
            public int Org_Dethod
            {
                get => _Org_Dethod;
                set
                {
                    if (_Org_Dethod != value)
                    {
                        _Org_Dethod = value;
                        OnPropertyChanged(nameof(Org_Dethod));
                    }
                }
            }
            public int Org_Dir
            {
                get => _Org_Dir;
                set
                {
                    if (_Org_Dir != value)
                    {
                        _Org_Dir = value;
                        OnPropertyChanged(nameof(Org_Dir));
                    }
                }
            }
            public int Org_Offset
            {
                get => _Org_Offset;
                set
                {
                    if (_Org_Offset != value)
                    {
                        _Org_Offset = value;
                        OnPropertyChanged(nameof(Org_Offset));
                    }
                }
            }
            public int Motion_Dir
            {
                get => _Motion_Dir;
                set
                {
                    if (_Motion_Dir != value)
                    {
                        _Motion_Dir = value;
                        OnPropertyChanged(nameof(Motion_Dir));
                    }
                }
            }
            public string Spare1
            {
                get => _Spare1;
                set
                {
                    if (_Spare1 != value)
                    {
                        _Spare1 = value;
                        OnPropertyChanged(nameof(Spare1));
                    }
                }
            }

            public string Spare2
            {
                get => _Spare2;
                set
                {
                    if (_Spare2 != value)
                    {
                        _Spare2 = value;
                        OnPropertyChanged(nameof(Spare2));
                    }
                }
            }

            public string Spare3
            {
                get => _Spare3;
                set
                {
                    if (_Spare3 != value)
                    {
                        _Spare3 = value;
                        OnPropertyChanged(nameof(Spare3));
                    }
                }
            }

            public string Spare4
            {
                get => _Spare4;
                set
                {
                    if (_Spare4 != value)
                    {
                        _Spare4 = value;
                        OnPropertyChanged(nameof(Spare4));
                    }
                }
            }

            public string Spare5
            {
                get => _Spare5;
                set
                {
                    if (_Spare5 != value)
                    {
                        _Spare5 = value;
                        OnPropertyChanged(nameof(Spare5));
                    }
                }
            }

        }
        #endregion Parameter

        #endregion DatabaseManager_FASTECH

        #region DatabaseManager_Equipment
        #region Equipment
        public class DatabaseManager_Equipment : INotifyPropertyChanged
        {
            public event PropertyChangedEventHandler PropertyChanged;
            protected virtual void OnPropertyChanged(string propertyName)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
            public int _ID;
            public string _Cell;
            public string _Barcode;
            public string _Qrcode;
            public DateTime? _Loading;
            public DateTime? _CreDate;
            public DateTime? _PositiveTime;
            public string _Result;
            public int _IncubationTime;
            public bool _Switched;
            public bool _isEnable;
            public bool _isActive;

            public int ID
            {
                get => _ID;
                set
                {
                    if (_ID != value)
                    {
                        _ID = value;
                        OnPropertyChanged(nameof(ID));
                    }
                }
            }
            public string Cell
            {
                get => _Cell;
                set
                {
                    if (_Cell != value)
                    {
                        _Cell = value;
                        OnPropertyChanged(nameof(Cell));
                    }
                }
            }
            public string Barcode
            {
                get => _Barcode;
                set
                {
                    if (_Barcode != value)
                    {
                        _Barcode = value;
                        OnPropertyChanged(nameof(Barcode));
                    }
                }
            }
            public string Qrcode
            {
                get => _Qrcode;
                set
                {
                    if (_Qrcode != value)
                    {
                        _Qrcode = value;
                        OnPropertyChanged(nameof(Qrcode));
                    }
                }
            }
            public DateTime? Loading
            {
                get => _Loading;
                set
                {
                    if (_Loading != value)
                    {
                        _Loading = value;
                        OnPropertyChanged(nameof(Loading));
                    }
                }
            }
            public DateTime? CreDate
            {
                get => _CreDate;
                set
                {
                    if (_CreDate != value)
                    {
                        _CreDate = value;
                        OnPropertyChanged(nameof(CreDate));
                    }
                }
            }
            public DateTime? PositiveTime
            {
                get => _PositiveTime;
                set
                {
                    if (_PositiveTime != value)
                    {
                        _PositiveTime = value;
                        OnPropertyChanged(nameof(PositiveTime));
                    }
                }
            }

            public string Result
            {
                get => _Result;
                set
                {
                    if (_Result != value)
                    {
                        _Result = value;
                        OnPropertyChanged(nameof(Result));
                    }
                }
            }
            public int IncubationTime
            {
                get => _IncubationTime;
                set
                {
                    if (_IncubationTime != value)
                    {
                        _IncubationTime = value;
                        OnPropertyChanged(nameof(IncubationTime));
                    }
                }
            }


            public bool Switched
            {
                get => _Switched;
                set
                {
                    if (_Switched != value)
                    {
                        _Switched = value;
                        OnPropertyChanged(nameof(Switched));
                    }
                }
            }

            public bool isEnable
            {
                get => _isEnable;
                set
                {
                    if (_isEnable != value)
                    {
                        _isEnable = value;
                        OnPropertyChanged(nameof(isEnable));
                    }
                }
            }
            public bool isActive
            {
                get => _isActive;
                set
                {
                    if (_isActive != value)
                    {
                        _isActive = value;
                        OnPropertyChanged(nameof(isActive));
                    }
                }
            }
        }
        #endregion Equipment

        #region EquipmentH
        public class DatabaseManager_EquipmentH : INotifyPropertyChanged
        {
            public event PropertyChangedEventHandler PropertyChanged;
            protected virtual void OnPropertyChanged(string propertyName)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
            public int _ID;
            public string _Barcode;
            public string _Qrcode;
            public DateTime _CreDate;
            public double _PcbADC;
            public double _PcbLED;
            public double _Temperature;

            public int ID
            {
                get => _ID;
                set
                {
                    if (_ID != value)
                    {
                        _ID = value;
                        OnPropertyChanged(nameof(ID));
                    }
                }
            }

            public string Barcode
            {
                get => _Barcode;
                set
                {
                    if (_Barcode != value)
                    {
                        _Barcode = value;
                        OnPropertyChanged(nameof(Barcode));
                    }
                }
            }
            public string Qrcode
            {
                get => _Qrcode;
                set
                {
                    if (_Qrcode != value)
                    {
                        _Qrcode = value;
                        OnPropertyChanged(nameof(Qrcode));
                    }
                }
            }


            public DateTime CreDate
            {
                get => _CreDate;
                set
                {
                    if (_CreDate != value)
                    {
                        _CreDate = value;
                        OnPropertyChanged(nameof(CreDate));
                    }
                }
            }
            public double PcbADC
            {
                get => _PcbADC;
                set
                {
                    if (_PcbADC != value)
                    {
                        _PcbADC = value;
                        OnPropertyChanged(nameof(PcbADC));
                    }
                }
            }
            public double PcbLED
            {
                get => _PcbLED;
                set
                {
                    if (_PcbLED != value)
                    {
                        _PcbLED = value;
                        OnPropertyChanged(nameof(PcbLED));
                    }
                }
            }
            public double Temperature
            {
                get => _Temperature;
                set
                {
                    if (_Temperature != value)
                    {
                        _Temperature = value;
                        OnPropertyChanged(nameof(Temperature));
                    }
                }
            }


        }
        #endregion EquipmentH

        #region Barcode
        public class DatabaseManager_Barcode : INotifyPropertyChanged
        {
            public event PropertyChangedEventHandler PropertyChanged;
            protected virtual void OnPropertyChanged(string propertyName)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
            public int _ID;
            public string _Barcode;
            public string _Qrcode;
            public double _LoadCell;
            public int _IncubationTime;
            public DateTime? _Loading;
            public DateTime? _Unloading;
            public string _Result;
            public DateTime? _PositiveTime;

            public int ID
            {
                get => _ID;
                set
                {
                    if (_ID != value)
                    {
                        _ID = value;
                        OnPropertyChanged(nameof(ID));
                    }
                }
            }

            public string Barcode
            {
                get => _Barcode;
                set
                {
                    if (_Barcode != value)
                    {
                        _Barcode = value;
                        OnPropertyChanged(nameof(Barcode));
                    }
                }
            }
            public string Qrcode
            {
                get => _Qrcode;
                set
                {
                    if (_Qrcode != value)
                    {
                        _Qrcode = value;
                        OnPropertyChanged(nameof(Qrcode));
                    }
                }
            }

            public double LoadCell
            {
                get => _LoadCell;
                set
                {
                    if (_LoadCell != value)
                    {
                        _LoadCell = value;
                        OnPropertyChanged(nameof(LoadCell));
                    }
                }
            }
            public DateTime? Loading
            {
                get => _Loading;
                set
                {
                    if (_Loading != value)
                    {
                        _Loading = value;
                        OnPropertyChanged(nameof(Loading));
                    }
                }
            }
            public DateTime? Unloading
            {
                get => _Unloading;
                set
                {
                    if (_Unloading != value)
                    {
                        _Unloading = value;
                        OnPropertyChanged(nameof(Unloading));
                    }
                }
            }
            public int IncubationTime
            {
                get => _IncubationTime;
                set
                {
                    if (_IncubationTime != value)
                    {
                        _IncubationTime = value;
                        OnPropertyChanged(nameof(IncubationTime));
                    }
                }
            }
            public string Result
            {
                get => _Result;
                set
                {
                    if (_Result != value)
                    {
                        _Result = value;
                        OnPropertyChanged(nameof(Result));
                    }
                }
            }
            public DateTime? PositiveTime
            {
                get => _PositiveTime;
                set
                {
                    if (_PositiveTime != value)
                    {
                        _PositiveTime = value;
                        OnPropertyChanged(nameof(PositiveTime));
                    }
                }
            }

        }

        public class DatabaseManager_BarcodeList : INotifyPropertyChanged
        {
            public event PropertyChangedEventHandler PropertyChanged;
            protected virtual void OnPropertyChanged(string propertyName)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
            public int _ID;
            public string _Barcode;
            public string _Qrcode;
            public double _LoadCell;
            public string _IncubationTime;
            public DateTime? _Loading;
            public DateTime? _Unloading;
            public string _Result;
            public DateTime? _PositiveTime;

            public int ID
            {
                get => _ID;
                set
                {
                    if (_ID != value)
                    {
                        _ID = value;
                        OnPropertyChanged(nameof(ID));
                    }
                }
            }

            public string Barcode
            {
                get => _Barcode;
                set
                {
                    if (_Barcode != value)
                    {
                        _Barcode = value;
                        OnPropertyChanged(nameof(Barcode));
                    }
                }
            }
            public string Qrcode
            {
                get => _Qrcode;
                set
                {
                    if (_Qrcode != value)
                    {
                        _Qrcode = value;
                        OnPropertyChanged(nameof(Qrcode));
                    }
                }
            }

            public double LoadCell
            {
                get => _LoadCell;
                set
                {
                    if (_LoadCell != value)
                    {
                        _LoadCell = value;
                        OnPropertyChanged(nameof(LoadCell));
                    }
                }
            }
            public DateTime? Loading
            {
                get => _Loading;
                set
                {
                    if (_Loading != value)
                    {
                        _Loading = value;
                        OnPropertyChanged(nameof(Loading));
                    }
                }
            }
            public DateTime? Unloading
            {
                get => _Unloading;
                set
                {
                    if (_Unloading != value)
                    {
                        _Unloading = value;
                        OnPropertyChanged(nameof(Unloading));
                    }
                }
            }
            public string IncubationTime
            {
                get => _IncubationTime;
                set
                {
                    if (_IncubationTime != value)
                    {
                        _IncubationTime = value;
                        OnPropertyChanged(nameof(IncubationTime));
                    }
                }
            }
            public string Result
            {
                get => _Result;
                set
                {
                    if (_Result != value)
                    {
                        _Result = value;
                        OnPropertyChanged(nameof(Result));
                    }
                }
            }
            public DateTime? PositiveTime
            {
                get => _PositiveTime;
                set
                {
                    if (_PositiveTime != value)
                    {
                        _PositiveTime = value;
                        OnPropertyChanged(nameof(PositiveTime));
                    }
                }
            }

        }
        #endregion Barcode

        #endregion DatabaseManager_Equipment

        #region MainEngine
        public class MainEngine_StatusList : INotifyPropertyChanged
        {
            private int _ID;
            private Enum_MainEngine_Statuslist _Result;

            public int ID
            {
                get => _ID;
                set
                {
                    if (_ID != value)
                    {
                        _ID = value;
                        OnPropertyChanged(nameof(ID));
                    }
                }
            }


            public Enum_MainEngine_Statuslist Result
            {
                get => _Result;
                set
                {
                    if (_Result != value)
                    {
                        _Result = value;
                        OnPropertyChanged(nameof(Result));
                    }
                }
            }

            public event PropertyChangedEventHandler PropertyChanged;
            protected virtual void OnPropertyChanged(string propertyName)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion MainEngine

        #region FASTECH
        #region Flag
        public class DatabaseManager_FASTECH_Flag : INotifyPropertyChanged
        {
            public event PropertyChangedEventHandler PropertyChanged;
            protected virtual void OnPropertyChanged(string propertyName)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
            private bool _Flag;
            public bool Flag
            {
                get => _Flag;
                set
                {
                    if (_Flag != value)
                    {
                        _Flag = value;
                        OnPropertyChanged(nameof(Flag));
                    }
                }
            }

        }
        public class DatabaseManager_FASTECH_Flag2 : INotifyPropertyChanged
        {
            public event PropertyChangedEventHandler PropertyChanged;
            protected virtual void OnPropertyChanged(string propertyName)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
            public bool _FLAG_ERRORALL;
            public bool _FFLAG_HWPOSILMT;
            public bool _FFLAG_HWNEGALMT;
            public bool _FFLAG_SWPOGILMT;
            public bool _FFLAG_SWNEGALMT;
            public bool _Reserved1;
            public bool _Reserved2;
            public bool _FFLAG_ERRPOSOVERFLOW;
            public bool _FFLAG_ERROVERCURRENT;
            public bool _FFLAG_ERROVERSPEED;
            public bool _FFLAG_ERRPOSTRACKING;
            public bool _FFLAG_ERROVERLOAD;
            public bool _FFLAG_ERROVERHEAT;
            public bool _FFLAG_ERRBACKEMF;
            public bool _FFLAG_ERRMOTORPOWER;
            public bool _FFLAG_ERRINPOSITION;
            public bool _FFLAG_EMGSTOP;
            public bool _FFLAG_SLOWSTOP;
            public bool _FFLAG_ORIGINRETURNING;
            public bool _FFLAG_INPOSITION;
            public bool _FFLAG_SERVOON;
            public bool _FFLAG_ALARMRESET;
            public bool _FFLAG_PTSTOPED;
            public bool _FFLAG_ORIGINSENSOR;
            public bool _FFLAG_ZPULSE;
            public bool _FFLAG_ORIGINRETOK;
            public bool _FFLAG_MOTIONDIR;


            public bool _FFLAG_MOTIONING;
            public bool _FFLAG_MOTIONPAUSE;
            public bool _FFLAG_MOTIONACCEL;
            public bool _FFLAG_MOTIONDECEL;
            public bool _FFLAG_MOTIONCONST;
            public bool FLAG_ERRORALL
            {
                get => _FLAG_ERRORALL;
                set
                {
                    if (_FLAG_ERRORALL != value)
                    {
                        _FLAG_ERRORALL = value;
                        OnPropertyChanged(nameof(FLAG_ERRORALL));
                    }
                }
            }

            public bool FFLAG_HWPOSILMT
            {
                get => _FFLAG_HWPOSILMT;
                set
                {
                    if (_FFLAG_HWPOSILMT != value)
                    {
                        _FFLAG_HWPOSILMT = value;
                        OnPropertyChanged(nameof(FFLAG_HWPOSILMT));
                    }
                }
            }

            public bool FFLAG_HWNEGALMT
            {
                get => _FFLAG_HWNEGALMT;
                set
                {
                    if (_FFLAG_HWNEGALMT != value)
                    {
                        _FFLAG_HWNEGALMT = value;
                        OnPropertyChanged(nameof(FFLAG_HWNEGALMT));
                    }
                }
            }

            public bool FFLAG_SWPOGILMT
            {
                get => _FFLAG_SWPOGILMT;
                set
                {
                    if (_FFLAG_SWPOGILMT != value)
                    {
                        _FFLAG_SWPOGILMT = value;
                        OnPropertyChanged(nameof(FFLAG_SWPOGILMT));
                    }
                }
            }

            public bool FFLAG_SWNEGALMT
            {
                get => _FFLAG_SWNEGALMT;
                set
                {
                    if (_FFLAG_SWNEGALMT != value)
                    {
                        _FFLAG_SWNEGALMT = value;
                        OnPropertyChanged(nameof(FFLAG_SWNEGALMT));
                    }
                }
            }

            public bool Reserved1
            {
                get => _Reserved1;
                set
                {
                    if (_Reserved1 != value)
                    {
                        _Reserved1 = value;
                        OnPropertyChanged(nameof(Reserved1));
                    }
                }
            }

            public bool Reserved2
            {
                get => _Reserved2;
                set
                {
                    if (_Reserved2 != value)
                    {
                        _Reserved2 = value;
                        OnPropertyChanged(nameof(Reserved2));
                    }
                }
            }

            public bool FFLAG_ERROVERCURRENT
            {
                get => _FFLAG_ERROVERCURRENT;
                set
                {
                    if (_FFLAG_ERROVERCURRENT != value)
                    {
                        _FFLAG_ERROVERCURRENT = value;
                        OnPropertyChanged(nameof(FFLAG_ERROVERCURRENT));
                    }
                }
            }

            public bool FFLAG_ERROVERSPEED
            {
                get => _FFLAG_ERROVERSPEED;
                set
                {
                    if (_FFLAG_ERROVERSPEED != value)
                    {
                        _FFLAG_ERROVERSPEED = value;
                        OnPropertyChanged(nameof(FFLAG_ERROVERSPEED));
                    }
                }
            }
            public bool FFLAG_ERRPOSTRACKING
            {
                get => _FFLAG_ERRPOSTRACKING;
                set
                {
                    if (_FFLAG_ERRPOSTRACKING != value)
                    {
                        _FFLAG_ERRPOSTRACKING = value;
                        OnPropertyChanged(nameof(FFLAG_ERRPOSTRACKING));
                    }
                }
            }
            public bool FFLAG_ERROVERLOAD
            {
                get => _FFLAG_ERROVERLOAD;
                set
                {
                    if (_FFLAG_ERROVERLOAD != value)
                    {
                        _FFLAG_ERROVERLOAD = value;
                        OnPropertyChanged(nameof(FFLAG_ERROVERLOAD));
                    }
                }
            }
            public bool FFLAG_ERROVERHEAT
            {
                get => _FFLAG_ERROVERHEAT;
                set
                {
                    if (_FFLAG_ERROVERHEAT != value)
                    {
                        _FFLAG_ERROVERHEAT = value;
                        OnPropertyChanged(nameof(FFLAG_ERROVERHEAT));
                    }
                }
            }
            public bool FFLAG_ERRBACKEMF
            {
                get => _FFLAG_ERRBACKEMF;
                set
                {
                    if (_FFLAG_ERRBACKEMF != value)
                    {
                        _FFLAG_ERRBACKEMF = value;
                        OnPropertyChanged(nameof(FFLAG_ERRBACKEMF));
                    }
                }
            }

            public bool FFLAG_ERRMOTORPOWER
            {
                get => _FFLAG_ERRMOTORPOWER;
                set
                {
                    if (_FFLAG_ERRMOTORPOWER != value)
                    {
                        _FFLAG_ERRMOTORPOWER = value;
                        OnPropertyChanged(nameof(FFLAG_ERRMOTORPOWER));
                    }
                }
            }

            public bool FFLAG_ERRINPOSITION
            {
                get => _FFLAG_ERRINPOSITION;
                set
                {
                    if (_FFLAG_ERRINPOSITION != value)
                    {
                        _FFLAG_ERRINPOSITION = value;
                        OnPropertyChanged(nameof(FFLAG_ERRINPOSITION));
                    }
                }
            }

            public bool FFLAG_EMGSTOP
            {
                get => _FFLAG_EMGSTOP;
                set
                {
                    if (_FFLAG_EMGSTOP != value)
                    {
                        _FFLAG_EMGSTOP = value;
                        OnPropertyChanged(nameof(FFLAG_EMGSTOP));
                    }
                }
            }

            public bool FFLAG_SLOWSTOP
            {
                get => _FFLAG_SLOWSTOP;
                set
                {
                    if (_FFLAG_SLOWSTOP != value)
                    {
                        _FFLAG_SLOWSTOP = value;
                        OnPropertyChanged(nameof(FFLAG_SLOWSTOP));
                    }
                }
            }

            public bool FFLAG_ORIGINRETURNING
            {
                get => _FFLAG_ORIGINRETURNING;
                set
                {
                    if (_FFLAG_ORIGINRETURNING != value)
                    {
                        _FFLAG_ORIGINRETURNING = value;
                        OnPropertyChanged(nameof(FFLAG_ORIGINRETURNING));
                    }
                }
            }

            public bool FFLAG_INPOSITION
            {
                get => _FFLAG_INPOSITION;
                set
                {
                    if (_FFLAG_INPOSITION != value)
                    {
                        _FFLAG_INPOSITION = value;
                        OnPropertyChanged(nameof(FFLAG_INPOSITION));
                    }
                }
            }

            public bool FFLAG_SERVOON
            {
                get => _FFLAG_SERVOON;
                set
                {
                    if (_FFLAG_SERVOON != value)
                    {
                        _FFLAG_SERVOON = value;
                        OnPropertyChanged(nameof(FFLAG_SERVOON));
                    }
                }
            }
            public bool FFLAG_ALARMRESET
            {
                get => _FFLAG_ALARMRESET;
                set
                {
                    if (_FFLAG_ALARMRESET != value)
                    {
                        _FFLAG_ALARMRESET = value;
                        OnPropertyChanged(nameof(FFLAG_ALARMRESET));
                    }
                }
            }
            public bool FFLAG_PTSTOPED
            {
                get => _FFLAG_PTSTOPED;
                set
                {
                    if (_FFLAG_PTSTOPED != value)
                    {
                        _FFLAG_PTSTOPED = value;
                        OnPropertyChanged(nameof(FFLAG_PTSTOPED));
                    }
                }
            }

            public bool FFLAG_ORIGINSENSOR
            {
                get => _FFLAG_ORIGINSENSOR;
                set
                {
                    if (_FFLAG_ORIGINSENSOR != value)
                    {
                        _FFLAG_ORIGINSENSOR = value;
                        OnPropertyChanged(nameof(FFLAG_ORIGINSENSOR));
                    }
                }
            }

            public bool FFLAG_ZPULSE
            {
                get => _FFLAG_ZPULSE;
                set
                {
                    if (_FFLAG_ZPULSE != value)
                    {
                        _FFLAG_ZPULSE = value;
                        OnPropertyChanged(nameof(FFLAG_ZPULSE));
                    }
                }
            }

            public bool FFLAG_ORIGINRETOK
            {
                get => _FFLAG_ORIGINRETOK;
                set
                {
                    if (_FFLAG_ORIGINRETOK != value)
                    {
                        _FFLAG_ORIGINRETOK = value;
                        OnPropertyChanged(nameof(FFLAG_ORIGINRETOK));
                    }
                }
            }

            public bool FFLAG_MOTIONDIR
            {
                get => _FFLAG_MOTIONDIR;
                set
                {
                    if (_FFLAG_MOTIONDIR != value)
                    {
                        _FFLAG_MOTIONDIR = value;
                        OnPropertyChanged(nameof(FFLAG_MOTIONDIR));
                    }
                }
            }
            public bool FFLAG_MOTIONING
            {
                get => _FFLAG_MOTIONING;
                set
                {
                    if (_FFLAG_MOTIONING != value)
                    {
                        _FFLAG_MOTIONING = value;
                        OnPropertyChanged(nameof(FFLAG_MOTIONING));
                    }
                }
            }
            public bool FFLAG_MOTIONPAUSE
            {
                get => _FFLAG_MOTIONPAUSE;
                set
                {
                    if (_FFLAG_MOTIONPAUSE != value)
                    {
                        _FFLAG_MOTIONPAUSE = value;
                        OnPropertyChanged(nameof(FFLAG_MOTIONPAUSE));
                    }
                }
            }

            public bool FFLAG_MOTIONACCEL
            {
                get => _FFLAG_MOTIONACCEL;
                set
                {
                    if (_FFLAG_MOTIONACCEL != value)
                    {
                        _FFLAG_MOTIONACCEL = value;
                        OnPropertyChanged(nameof(FFLAG_MOTIONACCEL));
                    }
                }
            }

            public bool FFLAG_MOTIONDECEL
            {
                get => _FFLAG_MOTIONDECEL;
                set
                {
                    if (_FFLAG_MOTIONDECEL != value)
                    {
                        _FFLAG_MOTIONDECEL = value;
                        OnPropertyChanged(nameof(FFLAG_MOTIONDECEL));
                    }
                }
            }

            public bool FFLAG_MOTIONCONST
            {
                get => _FFLAG_MOTIONCONST;
                set
                {
                    if (_FFLAG_MOTIONCONST != value)
                    {
                        _FFLAG_MOTIONCONST = value;
                        OnPropertyChanged(nameof(FFLAG_MOTIONCONST));
                    }
                }
            }
        }
        #endregion Flag

        #region Input
        public class Class_FASTECH_Input : INotifyPropertyChanged
        {
            public event PropertyChangedEventHandler PropertyChanged;
            protected virtual void OnPropertyChanged(string propertyName)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
            private bool _Flag;
            public bool Flag
            {
                get => _Flag;
                set
                {
                    if (_Flag != value)
                    {
                        _Flag = value;
                        OnPropertyChanged(nameof(Flag));
                    }
                }
            }

        }
        #endregion Input

        #region Output
        public class Class_FASTECH_Output : INotifyPropertyChanged
        {
            public event PropertyChangedEventHandler PropertyChanged;
            protected virtual void OnPropertyChanged(string propertyName)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
            private bool _Flag;
            public bool Flag
            {
                get => _Flag;
                set
                {
                    if (_Flag != value)
                    {
                        _Flag = value;
                        OnPropertyChanged(nameof(Flag));
                    }
                }
            }

        }
        #endregion Output
        #endregion FASTECH

        #region Config
        public class Class_Config : INotifyPropertyChanged
        {
            public event PropertyChangedEventHandler PropertyChanged;
            protected virtual void OnPropertyChanged(string propertyName)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
            public int _ID;
            public double _Temp;
            public int _doorOpenAlarmTrigger;
            public double _LoadCellMin;
            public double _LoadCellMax;
            public int _MaximumTime;
            public bool _UseBuzzer;
            public int _Positive_Wait;
            public int _Positive_Low;
            public int _Positive_High;
            public int _Analysis_Time_Range;
            public int _Analysis_Intervals;
            public double _Threshold;
            public int _BottleExistenceRange;
            public int _DataStorageSave;
            public int _TrashCanFillLevel;
            public string _Spare1;
            public string _Spare2;
            public string _Spare3;
            public string _Spare4;
            public string _Spare5;

            public int ID
            {
                get => _ID;
                set
                {
                    if (_ID != value)
                    {
                        _ID = value;
                        OnPropertyChanged(nameof(ID));
                    }
                }
            }
            public double Temp
            {
                get => _Temp;
                set
                {
                    if (_Temp != value)
                    {
                        _Temp = value;
                        OnPropertyChanged(nameof(Temp));
                    }
                }
            }
            public int doorOpenAlarmTrigger
            {
                get => _doorOpenAlarmTrigger;
                set
                {
                    if (_doorOpenAlarmTrigger != value)
                    {
                        _doorOpenAlarmTrigger = value;
                        OnPropertyChanged(nameof(doorOpenAlarmTrigger));
                    }
                }
            }

            
            public double LoadCellMin
            {
                get => _LoadCellMin;
                set
                {
                    if (_LoadCellMin != value)
                    {
                        _LoadCellMin = value;
                        OnPropertyChanged(nameof(LoadCellMin));
                    }
                }
            }
            public double LoadCellMax
            {
                get => _LoadCellMax;
                set
                {
                    if (_LoadCellMax != value)
                    {
                        _LoadCellMax = value;
                        OnPropertyChanged(nameof(LoadCellMax));
                    }
                }
            }
            public int MaximumTime
            {
                get => _MaximumTime;
                set
                {
                    if (_MaximumTime != value)
                    {
                        _MaximumTime = value;
                        OnPropertyChanged(nameof(MaximumTime));
                    }
                }
            }
            public bool UseBuzzer
            {
                get => _UseBuzzer;
                set
                {
                    if (_UseBuzzer != value)
                    {
                        _UseBuzzer = value;
                        OnPropertyChanged(nameof(UseBuzzer));
                    }
                }
            }
            public int Positive_Wait
            {
                get => _Positive_Wait;
                set
                {
                    if (_Positive_Wait != value)
                    {
                        _Positive_Wait = value;
                        OnPropertyChanged(nameof(Positive_Wait));
                    }
                }
            }
            public int Positive_Low
            {
                get => _Positive_Low;
                set
                {
                    if (_Positive_Low != value)
                    {
                        _Positive_Low = value;
                        OnPropertyChanged(nameof(Positive_Low));
                    }
                }
            }
            public int Positive_High
            {
                get => _Positive_High;
                set
                {
                    if (_Positive_High != value)
                    {
                        _Positive_High = value;
                        OnPropertyChanged(nameof(Positive_High));
                    }
                }
            }
            public int Analysis_Time_Range
            {
                get => _Analysis_Time_Range;
                set
                {
                    if (_Analysis_Time_Range != value)
                    {
                        _Analysis_Time_Range = value;
                        OnPropertyChanged(nameof(Analysis_Time_Range));
                    }
                }
            }
            public int Analysis_Intervals
            {
                get => _Analysis_Intervals;
                set
                {
                    if (_Analysis_Intervals != value)
                    {
                        _Analysis_Intervals = value;
                        OnPropertyChanged(nameof(Analysis_Intervals));
                    }
                }
            }

            public double Threshold
            {
                get => _Threshold;
                set
                {
                    if (_Threshold != value)
                    {
                        _Threshold = value;
                        OnPropertyChanged(nameof(Threshold));
                    }
                }
            }
            public int BottleExistenceRange
            {
                get => _BottleExistenceRange;
                set
                {
                    if (_BottleExistenceRange != value)
                    {
                        _BottleExistenceRange = value;
                        OnPropertyChanged(nameof(BottleExistenceRange));
                    }
                }
            }
            public int DataStorageSave
            {
                get => _DataStorageSave;
                set
                {
                    if (_DataStorageSave != value)
                    {
                        _DataStorageSave = value;
                        OnPropertyChanged(nameof(DataStorageSave));
                    }
                }
            }

            public int TrashCanFillLevel
            {
                get => _TrashCanFillLevel;
                set
                {
                    if (_TrashCanFillLevel != value)
                    {
                        _TrashCanFillLevel = value;
                        OnPropertyChanged(nameof(TrashCanFillLevel));
                    }
                }
            }
            public string Spare1
            {
                get => _Spare1;
                set
                {
                    if (_Spare1 != value)
                    {
                        _Spare1 = value;
                        OnPropertyChanged(nameof(Spare1));
                    }
                }
            }
            public string Spare2
            {
                get => _Spare2;
                set
                {
                    if (_Spare2 != value)
                    {
                        _Spare2 = value;
                        OnPropertyChanged(nameof(Spare2));
                    }
                }
            }
            public string Spare3
            {
                get => _Spare3;
                set
                {
                    if (_Spare3 != value)
                    {
                        _Spare3 = value;
                        OnPropertyChanged(nameof(Spare3));
                    }
                }
            }
            public string Spare4
            {
                get => _Spare4;
                set
                {
                    if (_Spare4 != value)
                    {
                        _Spare4 = value;
                        OnPropertyChanged(nameof(Spare4));
                    }
                }
            }
            public string Spare5
            {
                get => _Spare5;
                set
                {
                    if (_Spare5 != value)
                    {
                        _Spare5 = value;
                        OnPropertyChanged(nameof(Spare5));
                    }
                }
            }
        }
        #endregion Config

        #region System
        public class PositiveFirst_C : INotifyPropertyChanged
        {
            private bool _alive;
            public bool alive
            {
                get => _alive;
                set
                {
                    if (_alive != value)
                    {
                        _alive = value;
                        OnPropertyChanged(nameof(alive));
                    }
                }
            }



            public event PropertyChangedEventHandler PropertyChanged;
            protected virtual void OnPropertyChanged(string propertyName)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion System

        #region ModbusTCP
        public class ModbusTCP_Get_Flag : INotifyPropertyChanged
        {
            public event PropertyChangedEventHandler PropertyChanged;
            protected virtual void OnPropertyChanged(string propertyName)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
            private ushort _Flag;
            public ushort Flag
            {
                get => _Flag;
                set
                {
                    if (_Flag != value)
                    {
                        _Flag = value;
                        OnPropertyChanged(nameof(Flag));
                    }
                }
            }
        }
        public class ModbusTCP_Set_Flag : INotifyPropertyChanged
        {
            public event PropertyChangedEventHandler PropertyChanged;
            protected virtual void OnPropertyChanged(string propertyName)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
            private ushort _Flag;
            public ushort Flag
            {
                get => _Flag;
                set
                {
                    if (_Flag != value)
                    {
                        _Flag = value;
                        OnPropertyChanged(nameof(Flag));
                    }
                }
            }

        }
        #endregion ModbusTCP

        #region Position
        public class Class_Position : INotifyPropertyChanged
        {
            public event PropertyChangedEventHandler PropertyChanged;
            protected virtual void OnPropertyChanged(string propertyName)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }

            public int _ID;
            public double _Standby;
            public double _Loading;
            public double _LoadCell;
            public double _Barcode;
            public double _UnLoading;
            public double _Positive;
            public double _Error;
            public double _Rack1;
            public double _Rack2;
            public double _Rack3;
            public double _Rack4;
            public double _Rack5;
            public double _Rack6;
            public double _Rack7;
            public double _Rack8;
            public double _Rack9;
            public double _Rack10;
            public double _Rack11;
            public double _Rack12;
            public double _Rack13;
            public double _Rack14;
            public double _Rack15;
            public double _Rack16;
            public string _Spare;


            public int ID
            {
                get => _ID;
                set
                {
                    if (_ID != value)
                    {
                        _ID = value;
                        OnPropertyChanged(nameof(ID));
                    }
                }
            }
            public double Standby
            {
                get => _Standby;
                set
                {
                    if (_Standby != value)
                    {
                        _Standby = value;
                        OnPropertyChanged(nameof(Standby));
                    }
                }
            }
            public double Loading
            {
                get => _Loading;
                set
                {
                    if (_Loading != value)
                    {
                        _Loading = value;
                        OnPropertyChanged(nameof(Loading));
                    }
                }
            }
            public double LoadCell
            {
                get => _LoadCell;
                set
                {
                    if (_LoadCell != value)
                    {
                        _LoadCell = value;
                        OnPropertyChanged(nameof(LoadCell));
                    }
                }
            }
            public double Barcode
            {
                get => _Barcode;
                set
                {
                    if (_Barcode != value)
                    {
                        _Barcode = value;
                        OnPropertyChanged(nameof(Barcode));
                    }
                }
            }
            public double UnLoading
            {
                get => _UnLoading;
                set
                {
                    if (_UnLoading != value)
                    {
                        _UnLoading = value;
                        OnPropertyChanged(nameof(UnLoading));
                    }
                }
            }
            public double Positive
            {
                get => _Positive;
                set
                {
                    if (_Positive != value)
                    {
                        _Positive = value;
                        OnPropertyChanged(nameof(Positive));
                    }
                }
            }
            public double Error
            {
                get => _Error;
                set
                {
                    if (_Error != value)
                    {
                        _Error = value;
                        OnPropertyChanged(nameof(Error));
                    }
                }
            }
            public double Rack1
            {
                get => _Rack1;
                set
                {
                    if (_Rack1 != value)
                    {
                        _Rack1 = value;
                        OnPropertyChanged(nameof(Rack1));
                    }
                }
            }


            public double Rack2
            {
                get => _Rack2;
                set
                {
                    if (_Rack2 != value)
                    {
                        _Rack2 = value;
                        OnPropertyChanged(nameof(Rack2));
                    }
                }
            }


            public double Rack3
            {
                get => _Rack3;
                set
                {
                    if (_Rack3 != value)
                    {
                        _Rack3 = value;
                        OnPropertyChanged(nameof(Rack3));
                    }
                }
            }


            public double Rack4
            {
                get => _Rack4;
                set
                {
                    if (_Rack4 != value)
                    {
                        _Rack4 = value;
                        OnPropertyChanged(nameof(Rack4));
                    }
                }
            }


            public double Rack5
            {
                get => _Rack5;
                set
                {
                    if (_Rack5 != value)
                    {
                        _Rack5 = value;
                        OnPropertyChanged(nameof(Rack5));
                    }
                }
            }


            public double Rack6
            {
                get => _Rack6;
                set
                {
                    if (_Rack6 != value)
                    {
                        _Rack6 = value;
                        OnPropertyChanged(nameof(Rack6));
                    }
                }
            }


            public double Rack7
            {
                get => _Rack7;
                set
                {
                    if (_Rack7 != value)
                    {
                        _Rack7 = value;
                        OnPropertyChanged(nameof(Rack7));
                    }
                }
            }


            public double Rack8
            {
                get => _Rack8;
                set
                {
                    if (_Rack8 != value)
                    {
                        _Rack8 = value;
                        OnPropertyChanged(nameof(Rack8));
                    }
                }
            }


            public double Rack9
            {
                get => _Rack9;
                set
                {
                    if (_Rack9 != value)
                    {
                        _Rack9 = value;
                        OnPropertyChanged(nameof(Rack9));
                    }
                }
            }


            public double Rack10
            {
                get => _Rack10;
                set
                {
                    if (_Rack10 != value)
                    {
                        _Rack10 = value;
                        OnPropertyChanged(nameof(Rack10));
                    }
                }
            }


            public double Rack11
            {
                get => _Rack11;
                set
                {
                    if (_Rack11 != value)
                    {
                        _Rack11 = value;
                        OnPropertyChanged(nameof(Rack11));
                    }
                }
            }


            public double Rack12
            {
                get => _Rack12;
                set
                {
                    if (_Rack12 != value)
                    {
                        _Rack12 = value;
                        OnPropertyChanged(nameof(Rack12));
                    }
                }
            }


            public double Rack13
            {
                get => _Rack13;
                set
                {
                    if (_Rack13 != value)
                    {
                        _Rack13 = value;
                        OnPropertyChanged(nameof(Rack13));
                    }
                }
            }


            public double Rack14
            {
                get => _Rack14;
                set
                {
                    if (_Rack14 != value)
                    {
                        _Rack14 = value;
                        OnPropertyChanged(nameof(Rack14));
                    }
                }
            }


            public double Rack15
            {
                get => _Rack15;
                set
                {
                    if (_Rack15 != value)
                    {
                        _Rack15 = value;
                        OnPropertyChanged(nameof(Rack15));
                    }
                }
            }


            public double Rack16
            {
                get => _Rack16;
                set
                {
                    if (_Rack16 != value)
                    {
                        _Rack16 = value;
                        OnPropertyChanged(nameof(Rack16));
                    }
                }
            }

            public string Spare
            {
                get => _Spare;
                set
                {
                    if (_Spare != value)
                    {
                        _Spare = value;
                        OnPropertyChanged(nameof(Spare));
                    }
                }
            }

        }
        #endregion Position

        #region ETC
        public class Class_ETC : INotifyPropertyChanged
        {
            public event PropertyChangedEventHandler PropertyChanged;
            protected virtual void OnPropertyChanged(string propertyName)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
            public int _ID;

            public int _TrashCanProductCount;

            public string _Spare;

            public int ID
            {
                get => _ID;
                set
                {
                    if (_ID != value)
                    {
                        _ID = value;
                        OnPropertyChanged(nameof(ID));
                    }
                }
            }
            public int TrashCanProductCount
            {
                get => _TrashCanProductCount;
                set
                {
                    if (_TrashCanProductCount != value)
                    {
                        _TrashCanProductCount = value;
                        OnPropertyChanged(nameof(TrashCanProductCount));
                    }
                }
            }
            public string Spare
            {
                get => _Spare;
                set
                {
                    if (_Spare != value)
                    {
                        _Spare = value;
                        OnPropertyChanged(nameof(Spare));
                    }
                }
            }

        }
        #endregion ETC

        #region PCB
        public class PCB : INotifyPropertyChanged
        {
            private double _adc;
            public double ADC
            {
                get => _adc;
                set
                {
                    if (_adc != value)
                    {
                        _adc = value;
                        OnPropertyChanged(nameof(ADC));
                    }
                }
            }

            private double _led;
            public double LED
            {
                get => _led;
                set
                {
                    if (_led != value)
                    {
                        _led = value;
                        OnPropertyChanged(nameof(LED));
                    }
                }
            }

            public event PropertyChangedEventHandler PropertyChanged;
            protected virtual void OnPropertyChanged(string propertyName)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public class PCB_cell_alive_C : INotifyPropertyChanged
        {
            private int _alive;
            public int alive
            {
                get => _alive;
                set
                {
                    if (_alive != value)
                    {
                        _alive = value;
                        OnPropertyChanged(nameof(alive));
                    }
                }
            }

            public event PropertyChangedEventHandler PropertyChanged;
            protected virtual void OnPropertyChanged(string propertyName)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public class MatchEquipmentDataWithDB_C : INotifyPropertyChanged
        {
            private bool _alive;
            public bool alive
            {
                get => _alive;
                set
                {
                    if (_alive != value)
                    {
                        _alive = value;
                        OnPropertyChanged(nameof(alive));
                    }
                }
            }



            public event PropertyChangedEventHandler PropertyChanged;
            protected virtual void OnPropertyChanged(string propertyName)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion PCB

        #region SystemRack
        public class SystemRack_Num : INotifyPropertyChanged
        {


            private string _Num;
            public string Num
            {
                get => _Num;
                set
                {
                    if (_Num != value)
                    {
                        _Num = value;
                        OnPropertyChanged(nameof(Num));
                    }
                }
            }

            public event PropertyChangedEventHandler PropertyChanged;
            protected virtual void OnPropertyChanged(string propertyName)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion SystemRack

        public class Cell
        {
            public int CellNum { get; set; }
            public bool IsOccupied { get; set; } = false;

            public Cell(int index)
            {
                CellNum = index;
                IsOccupied = false; // Initially, all cells are not occupied
            }
        }


        #region test
        #region Input
        public class Class_test : INotifyPropertyChanged
        {
            public event PropertyChangedEventHandler PropertyChanged;
            protected virtual void OnPropertyChanged(string propertyName)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
            private string _Flag;
            public string Flag
            {
                get => _Flag;
                set
                {
                    if (_Flag != value)
                    {
                        _Flag = value;
                        OnPropertyChanged(nameof(Flag));
                    }
                }
            }

        }
        #endregion Input
        #endregion test
        #endregion Class
    }
}
