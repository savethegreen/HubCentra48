using HubCentra_A1.Class.FASTECH;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO.Ports;
using System.Net;
using System.Windows.Input;
using static HubCentra_A1.EnumManager;
using static HubCentra_A1.Model.View;

namespace HubCentra_A1.Model
{
    public class ViewModel : INotifyPropertyChanged
    {
        #region PropertyChanged
        public event PropertyChangedEventHandler? PropertyChanged;

        public virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion PropertyChanged

        #region Model
        private View _view;
        public ViewModel(View view)
        {
            _view = view;
            ClassIni();
        }
        #endregion Model

        #region Command
        public class RelayCommand : ICommand
        {
            private readonly Action<object> _execute;

            public event EventHandler CanExecuteChanged
            {
                add { CommandManager.RequerySuggested += value; }
                remove { CommandManager.RequerySuggested -= value; }
            }

            public RelayCommand(Action<object> execute)
            {
                _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            }

            public bool CanExecute(object parameter)
            {
                return true; // 여기서 실행 가능 여부를 항상 참으로 설정하거나, 실제로 실행 가능한 조건을 확인하도록 수정할 수 있습니다.
            }

            public void Execute(object parameter)
            {
                _execute(parameter);
            }
        }



        #region ViewModel->MainWindow
        private ICommand _ICommand_MainWindow;
        public ICommand ICommand_MainWindow => _ICommand_MainWindow ??= new RelayCommand(Command_MainWindow_Click);

        private void Command_MainWindow_Click(object parameter)
        {
            if (parameter is string paramString && int.TryParse(paramString, out int intValue))
            {
                DataTransferEvent_MainWindow((Enum_MainWindow_ButtonEvent)intValue);
            }
        }

        public delegate void ViewModelEventHandler_MainWindow(Enum_MainWindow_ButtonEvent DataTransfer);
        public event ViewModelEventHandler_MainWindow ViewModelDataTransferEvent_MainWindow;
        public void DataTransferEvent_MainWindow(Enum_MainWindow_ButtonEvent DataTransfer)
        {

            ViewModelDataTransferEvent_MainWindow?.Invoke(DataTransfer);
        }
        #endregion ViewModel->MainWindow

        #region ViewModel->Login
        private ICommand _ICommand_Login;
        public ICommand ICommand_Login => _ICommand_Login ??= new RelayCommand(Command_Login_Click);
        private void Command_Login_Click(object parameter)
        {
            if (parameter is string paramString && int.TryParse(paramString, out int intValue))
            {
                DataTransferEvent_Login((Enum_Login_ButtonEvent)intValue);
            }
        }

        public delegate void ViewModelEventHandler_Login(Enum_Login_ButtonEvent DataTransfer);
        public event ViewModelEventHandler_Login ViewModelDataTransferEvent_Login;
        public void DataTransferEvent_Login(Enum_Login_ButtonEvent DataTransfer)
        {

            ViewModelDataTransferEvent_Login?.Invoke(DataTransfer);
        }
        #endregion ViewModel->Login

        #region ViewModel->Search
        private ICommand _ICommand_Search;
        public ICommand ICommand_Search => _ICommand_Search ??= new RelayCommand(Command_Search_Click);
        private void Command_Search_Click(object parameter)
        {
            if (parameter is string paramString && int.TryParse(paramString, out int intValue))
            {
                DataTransferEvent_Search((Enum_Search_ButtonEvent)intValue);
            }
        }

        public delegate void ViewModelEventHandler_Search(Enum_Search_ButtonEvent DataTransfer);
        public event ViewModelEventHandler_Search ViewModelDataTransferEvent_Search;
        public void DataTransferEvent_Search(Enum_Search_ButtonEvent DataTransfer)
        {

            ViewModelDataTransferEvent_Search?.Invoke(DataTransfer);
        }
        #endregion ViewModel->Search

        #region ViewModel->Report
        private ICommand _ICommand_Report;
        public ICommand ICommand_Report => _ICommand_Report ??= new RelayCommand(Command_Report_Click);
        private void Command_Report_Click(object parameter)
        {
            if (parameter is string paramString && int.TryParse(paramString, out int intValue))
            {
                DataTransferEvent_Report((Enum_Report_ButtonEvent)intValue);
            }
        }

        public delegate void ViewModelEventHandler_Report(Enum_Report_ButtonEvent DataTransfer);
        public event ViewModelEventHandler_Report ViewModelDataTransferEvent_Report;
        public void DataTransferEvent_Report(Enum_Report_ButtonEvent DataTransfer)
        {

            ViewModelDataTransferEvent_Report?.Invoke(DataTransfer);
        }
        #endregion ViewModel->Report

        #region ViewModel->Config
        private ICommand _ICommand_Config;
        public ICommand ICommand_Config => _ICommand_Config ??= new RelayCommand(Command_Config_Click);
        private void Command_Config_Click(object parameter)
        {
            if (parameter is string paramString && int.TryParse(paramString, out int intValue))
            {
                DataTransferEvent_Config((Enum_Config_ButtonEvent)intValue);
            }
        }

        public delegate void ViewModelEventHandler_Config(Enum_Config_ButtonEvent DataTransfer);
        public event ViewModelEventHandler_Config ViewModelDataTransferEvent_Config;
        public void DataTransferEvent_Config(Enum_Config_ButtonEvent DataTransfer)
        {

            ViewModelDataTransferEvent_Config?.Invoke(DataTransfer);
        }
        #endregion ViewModel->Config

        #region ViewModel->WriteBarcode
        private ICommand _ICommand_WriteBarcode;
        public ICommand ICommand_WriteBarcode => _ICommand_WriteBarcode ??= new RelayCommand(Command_WriteBarcode_Click);
        private void Command_WriteBarcode_Click(object parameter)
        {
            if (parameter is string paramString && int.TryParse(paramString, out int intValue))
            {
                DataTransferEvent_WriteBarcode((Enum_WriteBarcode_ButtonEvent)intValue);
            }
        }

        public delegate void ViewModelEventHandler_WriteBarcode(Enum_WriteBarcode_ButtonEvent DataTransfer);
        public event ViewModelEventHandler_WriteBarcode ViewModelDataTransferEvent_WriteBarcode;
        public void DataTransferEvent_WriteBarcode(Enum_WriteBarcode_ButtonEvent DataTransfer)
        {

            ViewModelDataTransferEvent_WriteBarcode?.Invoke(DataTransfer);
        }
        #endregion ViewModel->WriteBarcode
        #endregion Command

        #region Common
        public int Common_CellCount
        {
            get => _view.Common_CellCount;
            set
            {
                _view.Common_CellCount = value;
                OnPropertyChanged(nameof(Common_CellCount));
            }
        }
        public int Common_SystemCellCount
        {
            get => _view.Common_SystemCellCount;
            set
            {
                _view.Common_SystemCellCount = value;
                OnPropertyChanged(nameof(Common_SystemCellCount));
            }
        }

        public int Common_TotalSystemCellCount
        {
            get => _view.Common_TotalSystemCellCount;
            set
            {
                _view.Common_TotalSystemCellCount = value;
                OnPropertyChanged(nameof(Common_TotalSystemCellCount));
            }
        }

        public int Common_Minute
        {
            get => _view.Common_Minute;
            set
            {
                _view.Common_Minute = value;
                OnPropertyChanged(nameof(Common_Minute));
            }
        }
        public int Common_Hour
        {
            get => _view.Common_Hour;
            set
            {
                _view.Common_Hour = value;
                OnPropertyChanged(nameof(Common_Hour));
            }
        }
        public int Common_Day
        {
            get => _view.Common_Day;
            set
            {
                _view.Common_Day = value;
                OnPropertyChanged(nameof(Common_Day));
            }
        }
        #endregion Common

        #region MainWindow
        public Enum_MainWindow_ButtonFlag MainWindow_ButtonFlag
        {
            get => _view.MainWindow_ButtonFlag;
            set
            {
                _view.MainWindow_ButtonFlag = value;
                OnPropertyChanged(nameof(MainWindow_ButtonFlag));
            }
        }

        public bool ProgramShotdown
        {
            get => _view.ProgramShotdown;
            set
            {
                _view.ProgramShotdown = value;
                OnPropertyChanged(nameof(ProgramShotdown));
            }
        }
        
        #endregion MainWindow

        #region Loading
        #region variable
        public int Loading_formLoadingProgress
        {
            get => _view.Loading_formLoadingProgress;
            set
            {
                _view.Loading_formLoadingProgress = value;
                OnPropertyChanged(nameof(Loading_formLoadingProgress));
            }
        }
        public int Loading_formLoadingProgress2
        {
            get => _view.Loading_formLoadingProgress2;
            set
            {
                _view.Loading_formLoadingProgress2 = value;
                OnPropertyChanged(nameof(Loading_formLoadingProgress2));
            }
        }
        #endregion variable

        #region Function

        #endregion Function
        #endregion Loading

        #region Login
        public Enum_Login Login
        {
            get => _view.Login;
            set
            {
                _view.Login = value;
                OnPropertyChanged(nameof(Login));
            }
        }
        public string LoginID
        {
            get => _view.LoginID;
            set
            {
                _view.LoginID = value;
                OnPropertyChanged(nameof(LoginID));
            }
        }
        public string Password
        {
            get => _view.Password;
            set
            {
                _view.Password = value;
                OnPropertyChanged(nameof(Password));
            }
        }
        public string Login_LevelC
        {
            get => _view.Login_LevelC;
            set
            {
                _view.Login_LevelC = value;
                OnPropertyChanged(nameof(Login_LevelC));
            }
        }
        #endregion Login

        #region FASTECH
        #region Connection

        public string FASTECH_IO_IP
        {
            get => _view.FASTECH_IO_IP;
            set
            {
                if (_view.FASTECH_IO_IP != value)
                {
                    _view.FASTECH_IO_IP = value;
                    OnPropertyChanged(nameof(FASTECH_IO_IP));
                }
            }
        }


        public bool FASTECH_IO_Connection
        {
            get => _view.FASTECH_IO_Connection;
            set
            {
                _view.FASTECH_IO_Connection = value;
                OnPropertyChanged(nameof(FASTECH_IO_Connection));
            }
        }

        #endregion  Connection



        #region IO
        public List<Class_FASTECH_Input> FASTECH_Input
        {
            get => _view.FASTECH_Input;
            set
            {
                if (_view.FASTECH_Input != value)
                {
                    _view.FASTECH_Input = value;
                    OnPropertyChanged(nameof(FASTECH_Input));
                }
            }
        }
        public List<Class_FASTECH_Output> FASTECH_Output
        {
            get => _view.FASTECH_Output;
            set
            {
                if (_view.FASTECH_Output != value)
                {
                    _view.FASTECH_Output = value;
                    OnPropertyChanged(nameof(FASTECH_Output));
                }
            }
        }

        public List<Class_FASTECH_Output> FASTECH_Set_Output
        {
            get => _view.FASTECH_Set_Output;
            set
            {
                if (_view.FASTECH_Set_Output != value)
                {
                    _view.FASTECH_Set_Output = value;
                    OnPropertyChanged(nameof(FASTECH_Set_Output));
                }
            }
        }

        public int FASTECH_Input_Latch
        {
            get => _view.FASTECH_Input_Latch;
            set
            {
                if (_view.FASTECH_Input_Latch != value)
                {
                    _view.FASTECH_Input_Latch = value;
                    OnPropertyChanged(nameof(FASTECH_Input_Latch));
                }
            }
        }
        #endregion IO
        #endregion FASTECH

        #region PCB
        public SerialPort PCB_SerialPort
        {
            get => _view.PCB_SerialPort;
            set
            {
                _view.PCB_SerialPort = value;
                OnPropertyChanged(nameof(PCB_SerialPort));
            }
        }
        public bool PCB_Connection
        {
            get => _view.PCB_Connection;
            set
            {
                _view.PCB_Connection = value;
                OnPropertyChanged(nameof(PCB_Connection));
            }
        }

        public bool PCB_Status
        {
            get => _view.PCB_Status;
            set
            {
                _view.PCB_Status = value;
                OnPropertyChanged(nameof(PCB_Status));
            }
        }

        public bool PCB_Restart
        {
            get => _view.PCB_Restart;
            set
            {
                _view.PCB_Restart = value;
                OnPropertyChanged(nameof(PCB_Restart));
            }
        }


        public string PCB_ID
        {
            get => _view.PCB_ID;
            set
            {
                _view.PCB_ID = value;
                OnPropertyChanged(nameof(PCB_ID));
            }
        }
        public List<PCB> PCB_Data
        {
            get => _view.PCB_Data;
            set
            {
                if (_view.PCB_Data != value)
                {
                    _view.PCB_Data = value;
                    OnPropertyChanged(nameof(PCB_Data));
                }
            }
        }
        public Dictionary<int, Dictionary<int, List<List<double>>>> PCB_CellReadings
        {
            get => _view.PCB_CellReadings;
            set
            {
                if (_view.PCB_CellReadings != value)
                {
                    _view.PCB_CellReadings = value;
                    OnPropertyChanged(nameof(PCB_CellReadings));
                }
            }
        }
        public ConcurrentQueue<string> Queue_PCB_Manual
        {
            get => _view.Queue_PCB_Manual;
            set
            {
                if (_view.Queue_PCB_Manual != value)
                {
                    _view.Queue_PCB_Manual = value;
                    OnPropertyChanged(nameof(Queue_PCB_Manual));
                }
            }
        }
        public List<PCB_cell_alive_C> PCB_cell_alive
        {
            get => _view.PCB_cell_alive;
            set
            {
                if (_view.PCB_cell_alive != value)
                {
                    _view.PCB_cell_alive = value;
                    OnPropertyChanged(nameof(PCB_cell_alive));
                }
            }
        }

        public List<MatchEquipmentDataWithDB_C> Equipment_DataWithDB_presenceArray
        {
            get => _view.Equipment_DataWithDB_presenceArray;
            set
            {
                if (_view.Equipment_DataWithDB_presenceArray != value)
                {
                    _view.Equipment_DataWithDB_presenceArray = value;
                    OnPropertyChanged(nameof(Equipment_DataWithDB_presenceArray));
                }
            }
        }

        public int[] PositiveDelay
        {
            get => _view.PositiveDelay;
            set
            {
                _view.PositiveDelay = value;
                OnPropertyChanged(nameof(PositiveDelay));
            }
        }


        public string teststr
        {
            get => _view.teststr;
            set
            {
                _view.teststr = value;
                OnPropertyChanged(nameof(teststr));
            }
        }
        public int testint
        {
            get => _view.testint;
            set
            {
                _view.testint = value;
                OnPropertyChanged(nameof(testint));

            }
        }

        public double PCB_targetvalue
        {
            get => _view.PCB_targetvalue;
            set
            {
                _view.PCB_targetvalue = value;
                OnPropertyChanged(nameof(PCB_targetvalue));
            }
        }
        public bool PCB1_targetvalue_test
        {
            get => _view.PCB1_targetvalue_test;
            set
            {
                _view.PCB1_targetvalue_test = value;
                OnPropertyChanged(nameof(PCB1_targetvalue_test));
            }
        }

        public PCB CurrentPCB => PCB_Data.Count > testint ? PCB_Data[testint] : null;
        #endregion PCB

        #region Barcode
        public SerialPort Barcode_SerialPort
        {
            get => _view.Barcode_SerialPort;
            set
            {
                _view.Barcode_SerialPort = value;
                OnPropertyChanged(nameof(Barcode_SerialPort));
            }
        }
        public string Barcode_ID_Loading
        {
            get => _view.Barcode_ID_Loading;
            set
            {
                _view.Barcode_ID_Loading = value;
                OnPropertyChanged(nameof(Barcode_ID_Loading));
            }
        }
        public string Patient_ID_Loading
        {
            get => _view.Patient_ID_Loading;
            set
            {
                _view.Patient_ID_Loading = value;
                OnPropertyChanged(nameof(Patient_ID_Loading));
            }
        }


        
        public string Barcode_ID
        {
            get => _view.Barcode_ID;
            set
            {
                _view.Barcode_ID = value;
                OnPropertyChanged(nameof(Barcode_ID));
            }
        }
        public string Patient_ID
        {
            get => _view.Patient_ID;
            set
            {
                _view.Patient_ID = value;
                OnPropertyChanged(nameof(Patient_ID));
            }
        }

        
        public bool Barcode_Connection
        {
            get => _view.Barcode_Connection;
            set
            {
                _view.Barcode_Connection = value;
                OnPropertyChanged(nameof(Barcode_Connection));
            }
        }
        #endregion Barcode

        #region Temperature
        public SerialPort Temperature_SerialPort
        {
            get => _view.Temperature_SerialPort;
            set
            {
                _view.Temperature_SerialPort = value;
                OnPropertyChanged(nameof(Temperature_SerialPort));
            }
        }
        public double Temperature_ProcessValue
        {
            get => _view.Temperature_ProcessValue;
            set
            {
                _view.Temperature_ProcessValue = value;
                OnPropertyChanged(nameof(Temperature_ProcessValue));
            }
        }
        public bool Temperature_Connection
        {
            get => _view.Temperature_Connection;
            set
            {
                _view.Temperature_Connection = value;
                OnPropertyChanged(nameof(Temperature_Connection));
            }
        }
        public bool Temperature_AL_Connection
        {
            get => _view.Temperature_AL_Connection;
            set
            {
                _view.Temperature_AL_Connection = value;
                OnPropertyChanged(nameof(Temperature_AL_Connection));
            }
        }

        
        #endregion Temperature

        #region MainEngine

        public List<MainEngine_StatusList> MainEngine_Statuslist
        {
            get => _view.MainEngine_Statuslist;
            set
            {
                if (_view.MainEngine_Statuslist != value)
                {
                    _view.MainEngine_Statuslist = value;
                    OnPropertyChanged(nameof(MainEngine_Statuslist));
                }
            }
        }

        public ConcurrentQueue<int> ConcurrentQueue_MainEngine_Unloading_Positive
        {
            get => _view.ConcurrentQueue_MainEngine_Unloading_Positive;
            set
            {
                if (_view.ConcurrentQueue_MainEngine_Unloading_Positive != value)
                {
                    _view.ConcurrentQueue_MainEngine_Unloading_Positive = value;
                    OnPropertyChanged(nameof(ConcurrentQueue_MainEngine_Unloading_Positive));
                }
            }
        }
        public ConcurrentQueue<int> ConcurrentQueue_MainEngine_Unloading_Negative
        {
            get => _view.ConcurrentQueue_MainEngine_Unloading_Negative;
            set
            {
                if (_view.ConcurrentQueue_MainEngine_Unloading_Negative != value)
                {
                    _view.ConcurrentQueue_MainEngine_Unloading_Negative = value;
                    OnPropertyChanged(nameof(ConcurrentQueue_MainEngine_Unloading_Negative));
                }
            }
        }
        public ConcurrentQueue<int> ConcurrentQueue_MainEngine_Unloading_Incubation
        {
            get => _view.ConcurrentQueue_MainEngine_Unloading_Incubation;
            set
            {
                if (_view.ConcurrentQueue_MainEngine_Unloading_Incubation != value)
                {
                    _view.ConcurrentQueue_MainEngine_Unloading_Incubation = value;
                    OnPropertyChanged(nameof(ConcurrentQueue_MainEngine_Unloading_Incubation));
                }
            }
        }
        public int MainEngine_CurrentNum
        {
            get => _view.MainEngine_CurrentNum;
            set
            {
                _view.MainEngine_CurrentNum = value;
                OnPropertyChanged(nameof(MainEngine_CurrentNum));
            }
        }
        public int MainEngine_index
        {
            get => _view.MainEngine_index;
            set
            {
                _view.MainEngine_index = value;
                OnPropertyChanged(nameof(MainEngine_index));
            }
        }


        public bool MainEngine_LoadCell_Result
        {
            get => _view.MainEngine_LoadCell_Result;
            set
            {
                _view.MainEngine_LoadCell_Result = value;
                OnPropertyChanged(nameof(MainEngine_LoadCell_Result));
            }
        }

        public bool MainEngine_Incubation_Result
        {
            get => _view.MainEngine_Incubation_Result;
            set
            {
                _view.MainEngine_Incubation_Result = value;
                OnPropertyChanged(nameof(MainEngine_Incubation_Result));
            }
        }
        public bool MainEngine_EmptyEnabled
        {
            get => _view.MainEngine_EmptyEnabled;
            set
            {
                _view.MainEngine_EmptyEnabled = value;
                OnPropertyChanged(nameof(MainEngine_EmptyEnabled));
            }
        }

        public string MainEngine_Result
        {
            get => _view.MainEngine_Result;
            set
            {
                _view.MainEngine_Result = value;
                OnPropertyChanged(nameof(MainEngine_Result));
            }
        }


        public bool MainEngine_Positive_Index_Left
        {
            get => _view.MainEngine_Positive_Index_Left;
            set
            {
                _view.MainEngine_Positive_Index_Left = value;
                OnPropertyChanged(nameof(MainEngine_Positive_Index_Left));
            }
        }
        public bool MainEngine_Positive_Index_Right
        {
            get => _view.MainEngine_Positive_Index_Right;
            set
            {
                _view.MainEngine_Positive_Index_Right = value;
                OnPropertyChanged(nameof(MainEngine_Positive_Index_Right));
            }
        }


        public bool MainEngine_Error_Index_Left
        {
            get => _view.MainEngine_Error_Index_Left;
            set
            {
                _view.MainEngine_Error_Index_Left = value;
                OnPropertyChanged(nameof(MainEngine_Error_Index_Left));
            }
        }
        public bool MainEngine_Error_Index_Right
        {
            get => _view.MainEngine_Error_Index_Right;
            set
            {
                _view.MainEngine_Error_Index_Right = value;
                OnPropertyChanged(nameof(MainEngine_Error_Index_Right));
            }
        }

        public double MainEngine_LoadCell_Value
        {
            get => _view.MainEngine_LoadCell_Value;
            set
            {
                _view.MainEngine_LoadCell_Value = value;
                OnPropertyChanged(nameof(MainEngine_LoadCell_Value));
            }
        }


        #endregion MainEngine

        #region DatabaseManager
        public bool DatabaseManager_Connection
        {
            get => _view.DatabaseManager_Connection;
            set
            {
                if (_view.DatabaseManager_Connection != value)
                {
                    _view.DatabaseManager_Connection = value;
                    OnPropertyChanged(nameof(DatabaseManager_Connection));
                }
            }
        }
        public List<DatabaseManager_System> SystemInfo
        {
            get => _view.SystemInfo;
            set
            {
                if (_view.SystemInfo != value)
                {
                    _view.SystemInfo = value;
                    OnPropertyChanged(nameof(SystemInfo));
                }
            }
        }

        public List<DatabaseManager_Login> LoginInfo
        {
            get => _view.LoginInfo;
            set
            {
                if (_view.LoginInfo != value)
                {
                    _view.LoginInfo = value;
                    OnPropertyChanged(nameof(LoginInfo));
                }
            }
        }

        public List<DatabaseManager_FASTECH_Parameter> FASTECHInfo
        {
            get => _view.FASTECHInfo;
            set
            {
                if (_view.FASTECHInfo != value)
                {
                    _view.FASTECHInfo = value;
                    OnPropertyChanged(nameof(FASTECHInfo));
                }
            }
        }
        public List<DatabaseManager_Equipment> EquipmentInfo
        {
            get => _view.EquipmentInfo;
            set
            {
                if (_view.EquipmentInfo != value)
                {
                    _view.EquipmentInfo = value;
                    OnPropertyChanged(nameof(EquipmentInfo));
                }
            }
        }


        public double DatabaseManager_CmdPos
        {
            get => _view.DatabaseManager_CmdPos;
            set
            {
                _view.DatabaseManager_CmdPos = value;

                OnPropertyChanged(nameof(DatabaseManager_CmdPos));
            }
        }



        public string DatabaseManager_CmdPos_Str
        {
            get => _view._DatabaseManager_CmdPos_Str;
            set
            {
                if (_view._DatabaseManager_CmdPos_Str != value)
                {
                    _view._DatabaseManager_CmdPos_Str = value;
                    if (double.TryParse(_view._DatabaseManager_CmdPos_Str, NumberStyles.Any, CultureInfo.InvariantCulture, out double validValue))
                    {
                        DatabaseManager_CmdPos = validValue; // 올바른 double 값이면 실제 속성 업데이트
                    }
                    OnPropertyChanged(nameof(DatabaseManager_CmdPos_Str));
                }
            }
        }
        #endregion DatabaseManager

        #region  Result
        public Dictionary<int, Queue<(DateTime, double)>> Result_PositiveTime
        {
            get => _view.Result_PositiveTime;
            set
            {
                if (_view.Result_PositiveTime != value)
                {
                    _view.Result_PositiveTime = value;
                    OnPropertyChanged(nameof(Result_PositiveTime));
                }
            }
        }
        public bool Result_Timer
        {
            get => _view.Result_Timer;
            set
            {
                if (_view.Result_Timer != value)
                {
                    _view.Result_Timer = value;
                    OnPropertyChanged(nameof(Result_Timer));
                }
            }
        }
        #endregion Result

        #region Home
        public int Home_Available
        {
            get => _view.Home_Available;
            set
            {
                _view.Home_Available = value;
                OnPropertyChanged(nameof(Home_Available));
            }
        }
        public bool Home_Available_Flag
        {
            get => _view.Home_Available_Flag;
            set
            {
                _view.Home_Available_Flag = value;
                OnPropertyChanged(nameof(Home_Available_Flag));
            }
        }
        public int Home_Incubation
        {
            get => _view.Home_Incubation;
            set
            {
                _view.Home_Incubation = value;
                OnPropertyChanged(nameof(Home_Incubation));
            }
        }
        public bool Home_Incubation_Flag
        {
            get => _view.Home_Incubation_Flag;
            set
            {
                _view.Home_Incubation_Flag = value;
                OnPropertyChanged(nameof(Home_Incubation_Flag));
            }
        }
        public int Home_Positive
        {
            get => _view.Home_Positive;
            set
            {
                _view.Home_Positive = value;
                OnPropertyChanged(nameof(Home_Positive));
            }
        }
        public bool Home_Positive_Flag
        {
            get => _view.Home_Positive_Flag;
            set
            {
                _view.Home_Positive_Flag = value;
                OnPropertyChanged(nameof(Home_Positive_Flag));
            }
        }
        public int Home_Negative
        {
            get => _view.Home_Negative;
            set
            {
                _view.Home_Negative = value;
                OnPropertyChanged(nameof(Home_Negative));
            }
        }
        public bool Home_Negative_Flag
        {
            get => _view.Home_Negative_Flag;
            set
            {
                _view.Home_Negative_Flag = value;
                OnPropertyChanged(nameof(Home_Negative_Flag));
            }
        }
        #endregion Home

        #region Config
        public List<Class_Config> Config
        {
            get => _view.Config;
            set
            {
                if (_view.Config != value)
                {
                    _view.Config = value;
                    OnPropertyChanged(nameof(Config));
                }
            }
        }

        public int Config_Block
        {
            get => _view.Config_Block;
            set
            {
                _view.Config_Block = value;
                OnPropertyChanged(nameof(Config_Block));
            }
        }

        public bool Config_DataStorageSave_timer_Flag
        {
            get => _view.Config_DataStorageSave_timer_Flag;
            set
            {
                _view.Config_DataStorageSave_timer_Flag = value;
                OnPropertyChanged(nameof(Config_DataStorageSave_timer_Flag));
            }
        }
        #endregion Config

        #region Search
        public List<DatabaseManager_Equipmentstring> Search_List
        {
            get => _view.Search_List;
            set
            {
                if (_view.Search_List != value)
                {
                    _view.Search_List = value;
                    OnPropertyChanged(nameof(Search_List));
                }
            }
        }
        #endregion Search

        #region Report
        public List<DatabaseManager_BarcodeList> Report_List
        {
            get => _view.Report_List;
            set
            {
                if (_view.Report_List != value)
                {
                    _view.Report_List = value;
                    OnPropertyChanged(nameof(Report_List));
                }
            }
        }
        public List<DatabaseManager_EquipmentH> CSV_List
        {
            get => _view.CSV_List;
            set
            {
                if (_view.CSV_List != value)
                {
                    _view.CSV_List = value;
                    OnPropertyChanged(nameof(CSV_List));
                }
            }
        }

        public string Report_Find_Barcode
        {
            get => _view.Report_Find_Barcode;
            set
            {
                _view.Report_Find_Barcode = value;
                OnPropertyChanged(nameof(Report_Find_Barcode));
            }
        }



        public string Report_Barcode
        {
            get => _view.Report_Barcode;
            set
            {
                _view.Report_Barcode = value;
                OnPropertyChanged(nameof(Report_Barcode));
            }
        }


        public int Report_AverageValue
        {
            get => _view.Report_AverageValue;
            set
            {
                _view.Report_AverageValue = value;
                OnPropertyChanged(nameof(Report_AverageValue));
            }
        }
        public Enum_Report_Model Report_Model
        {
            get => _view.Report_Model;
            set
            {
                _view.Report_Model = value;
                OnPropertyChanged(nameof(Report_Model));
            }
        }

        public DateTime Report_DatePicke_Start
        {
            get => _view.Report_DatePicke_Start;
            set
            {
                _view.Report_DatePicke_Start = value;
                OnPropertyChanged(nameof(Report_DatePicke_Start));
            }
        }
        public DateTime Report_DatePicke_End
        {
            get => _view.Report_DatePicke_End;
            set
            {
                _view.Report_DatePicke_End = value;
                OnPropertyChanged(nameof(Report_DatePicke_End));
            }
        }


        public DateTime MyDatePicke_Start
        {
            get => _view.MyDatePicke_Start;
            set
            {
                _view.MyDatePicke_Start = value;
                OnPropertyChanged(nameof(MyDatePicke_Start));
            }
        }

        public DateTime MyDatePicke_End
        {
            get => _view.MyDatePicke_End;
            set
            {
                if (_view.MyDatePicke_End.Date != value.Date)
                {
                    _view.MyDatePicke_End = new DateTime(value.Year, value.Month, value.Day, 23, 59, 59);
                    OnPropertyChanged(nameof(MyDatePicke_End));
                }
            }
        }

        public string Report_Result
        {
            get => _view.Report_Result;
            set
            {
                _view.Report_Result = value;
                OnPropertyChanged(nameof(Report_Result));
            }
        }
        public string Report_Positive_Time
        {
            get => _view.Report_Positive_Time;
            set
            {
                _view.Report_Positive_Time = value;
                OnPropertyChanged(nameof(Report_Positive_Time));
            }
        }

        #endregion Report

        #region LiveCharts
        public List<DatabaseManager_EquipmentH> LiveCharts_List
        {
            get => _view.LiveCharts_List;
            set
            {
                if (_view.LiveCharts_List != value)
                {
                    _view.LiveCharts_List = value;
                    OnPropertyChanged(nameof(LiveCharts_List));
                }
            }
        }

        public Dictionary<int, Queue<(DateTime, double)>> LiveCharts_TimeSeries
        {
            get => _view.LiveCharts_TimeSeries;
            set
            {
                if (_view.LiveCharts_TimeSeries != value)
                {
                    _view.LiveCharts_TimeSeries = value;
                    OnPropertyChanged(nameof(LiveCharts_TimeSeries));
                }
            }
        }
        public ISeries[] Series
        {
            get => _view.Series;
            set
            {
                if (_view.Series != value)
                {
                    _view.Series = value;
                    OnPropertyChanged(nameof(Series));
                }
            }
        }
        public RectangularSection[] Sections
        {
            get => _view.Sections;
            set
            {
                if (_view.Sections != value)
                {
                    _view.Sections = value;
                    OnPropertyChanged(nameof(Sections));
                }
            }
        }
        public Axis[] XAxes
        {
            get => _view.XAxes;
            set
            {
                _view.XAxes = value;
                OnPropertyChanged(nameof(XAxes));
            }
        }

        public Axis[] YAxes
        {
            get => _view.YAxes;
            set
            {
                _view.YAxes = value;
                OnPropertyChanged(nameof(YAxes));
            }
        }
        public int LiveCharts_Positive_Start
        {
            get => _view.LiveCharts_Positive_Start;
            set
            {
                _view.LiveCharts_Positive_Start = value;
                OnPropertyChanged(nameof(LiveCharts_Positive_Start));
            }
        }
        public int LiveCharts_Positive_End
        {
            get => _view.LiveCharts_Positive_End;
            set
            {
                _view.LiveCharts_Positive_End = value;
                OnPropertyChanged(nameof(LiveCharts_Positive_End));
            }
        }
        public int LiveCharts_Positive_Index
        {
            get => _view.LiveCharts_Positive_Index;
            set
            {
                _view.LiveCharts_Positive_Index = value;
                OnPropertyChanged(nameof(LiveCharts_Positive_Index));
            }
        }
        public string LiveCharts_Result
        {
            get => _view.LiveCharts_Result;
            set
            {
                _view.LiveCharts_Result = value;
                OnPropertyChanged(nameof(LiveCharts_Result));
            }
        }
        public string LiveCharts_Positive_Time
        {
            get => _view.LiveCharts_Positive_Time;
            set
            {
                _view.LiveCharts_Positive_Time = value;
                OnPropertyChanged(nameof(LiveCharts_Positive_Time));
            }
        }
        #endregion LiveCharts

        #region Calculator

        public string Calculator_DisplayTextBlock
        {
            get => _view.Calculator_DisplayTextBlock;
            set
            {
                _view.Calculator_DisplayTextBlock = value;
                OnPropertyChanged(nameof(Calculator_DisplayTextBlock));
            }
        }
        #endregion Calculator

        #region System
        public List<PositiveFirst_C> System_PositiveFirst
        {
            get => _view.System_PositiveFirst;
            set
            {
                _view.System_PositiveFirst = value;
                OnPropertyChanged(nameof(System_PositiveFirst));
            }
        }

        #region System1


        public bool System1_HasPositive
        {
            get => _view.System1_HasPositive;
            set
            {
                _view.System1_HasPositive = value;
                OnPropertyChanged(nameof(System1_HasPositive));
            }
        }
        public string System1_Positive_Warning
        {
            get => _view.System1_Positive_Warning;
            set
            {
                _view.System1_Positive_Warning = value;
                OnPropertyChanged(nameof(System1_Positive_Warning));
            }
        }
        public string System1_Positive_Cel
        {
            get => _view.System1_Positive_Cel;
            set
            {
                _view.System1_Positive_Cel = value;
                OnPropertyChanged(nameof(System1_Positive_Cel));
            }
        }
        public int System_PositiveFirstint
        {
            get => _view.System_PositiveFirstint;
            set
            {
                _view.System_PositiveFirstint = value;
                OnPropertyChanged(nameof(System_PositiveFirstint));
            }
        }
        public int System_IncubationFirstint
        {
            get => _view.System_IncubationFirstint;
            set
            {
                _view.System_IncubationFirstint = value;
                OnPropertyChanged(nameof(System_IncubationFirstint));
            }
        }
        
        public int System1_Result
        {
            get => _view.System1_Result;
            set
            {
                _view.System1_Result = value;
                OnPropertyChanged(nameof(System1_Result));
            }
        }
        #endregion System1

        #region System2
        #endregion System2

        #region System3
        #endregion System3

        #region System4
        #endregion System4
        #endregion System

        #region WriteBarcode
        public string WriteBarcode_ID
        {
            get => _view.WriteBarcode_ID;
            set
            {
                _view.WriteBarcode_ID = value;
                OnPropertyChanged(nameof(WriteBarcode_ID));
            }
        }
        public string WritePatient_ID
        {
            get => _view.WritePatient_ID;
            set
            {
                _view.WritePatient_ID = value;
                OnPropertyChanged(nameof(WritePatient_ID));
            }
        }
        #endregion WriteBarcode

        #region PopStatus



        public bool PopStatus_Positive_Flag
        {
            get => _view.PopStatus_Positive_Flag;
            set
            {
                _view.PopStatus_Positive_Flag = value;
                OnPropertyChanged(nameof(PopStatus_Positive_Flag));
            }
        }

        public string PopStatus_Positive_Title
        {
            get => _view.PopStatus_Positive_Title;
            set
            {
                _view.PopStatus_Positive_Title = value;
                OnPropertyChanged(nameof(PopStatus_Positive_Title));
            }
        }


        public string PopStatus_Error_Bottle_Title
        {
            get => _view.PopStatus_Error_Bottle_Title;
            set
            {
                _view.PopStatus_Error_Bottle_Title = value;
                OnPropertyChanged(nameof(PopStatus_Error_Bottle_Title));
            }
        }
        public string PopStatus_Error_Bottle
        {
            get => _view.PopStatus_Error_Bottle;
            set
            {
                _view.PopStatus_Error_Bottle = value;
                OnPropertyChanged(nameof(PopStatus_Error_Bottle));
            }
        }


        public bool PopStatus_Error_Bottle_Flag
        {
            get => _view.PopStatus_Error_Bottle_Flag;
            set
            {
                _view.PopStatus_Error_Bottle_Flag = value;
                OnPropertyChanged(nameof(PopStatus_Error_Bottle_Flag));
            }
        }


        public string PopStatus_Positive_Bottle_Title
        {
            get => _view.PopStatus_Positive_Bottle_Title;
            set
            {
                _view.PopStatus_Positive_Bottle_Title = value;
                OnPropertyChanged(nameof(PopStatus_Positive_Bottle_Title));
            }
        }
        public string PopStatus_Positive_Bottle
        {
            get => _view.PopStatus_Positive_Bottle;
            set
            {
                _view.PopStatus_Positive_Bottle = value;
                OnPropertyChanged(nameof(PopStatus_Positive_Bottle));
            }
        }
        public bool PopStatus_Positive_Bottle_Flag
        {
            get => _view.PopStatus_Positive_Bottle_Flag;
            set
            {
                _view.PopStatus_Positive_Bottle_Flag = value;
                OnPropertyChanged(nameof(PopStatus_Positive_Bottle_Flag));
            }
        }

        public string PopStatus_TrashCanAlert_Title
        {
            get => _view.PopStatus_TrashCanAlert_Title;
            set
            {
                _view.PopStatus_TrashCanAlert_Title = value;
                OnPropertyChanged(nameof(PopStatus_TrashCanAlert_Title));
            }
        }
        public string PopStatus_TrashCanAlert
        {
            get => _view.PopStatus_TrashCanAlert;
            set
            {
                _view.PopStatus_TrashCanAlert = value;
                OnPropertyChanged(nameof(PopStatus_TrashCanAlert));
            }
        }
        public bool PopStatus_TrashCanAlert_Flag
        {
            get => _view.PopStatus_TrashCanAlert_Flag;
            set
            {
                _view.PopStatus_TrashCanAlert_Flag = value;
                OnPropertyChanged(nameof(PopStatus_TrashCanAlert_Flag));
            }
        }
        #endregion PopStatus

        #region Alarm
        #region System
        public string System_Title
        {
            get => _view.System_Title;
            set
            {
                _view.System_Title = value;
                OnPropertyChanged(nameof(System_Title));
            }
        }
        public string System_WhatSystem
        {
            get => _view.System_WhatSystem;
            set
            {
                _view.System_WhatSystem = value;
                OnPropertyChanged(nameof(System_WhatSystem));
            }
        }
        public string System_Content
        {
            get => _view.System_Content;
            set
            {
                _view.System_Content = value;
                OnPropertyChanged(nameof(System_Content));
            }
        }
        #endregion System

        #region BottleLoading
        public ConcurrentQueue<Tuple<int>> Alarm_BottleLoading
        {
            get => _view.Alarm_BottleLoading;
            set
            {
                if (_view.Alarm_BottleLoading != value)
                {
                    _view.Alarm_BottleLoading = value;
                    OnPropertyChanged(nameof(Alarm_BottleLoading));
                }
            }
        }
        public HashSet<int> Alarm_BottleLoading_Set
        {
            get => _view.Alarm_BottleLoading_Set;
            set
            {
                if (_view.Alarm_BottleLoading_Set != value)
                {
                    _view.Alarm_BottleLoading_Set = value;
                    OnPropertyChanged(nameof(Alarm_BottleLoading_Set));
                }
            }
        }
        public bool[] BottleLoading_Result
        {
            get => _view.BottleLoading_Result;
            set
            {
                _view.BottleLoading_Result = value;
                OnPropertyChanged(nameof(BottleLoading_Result));
            }
        }
        public bool BottleLoading_isPopupOpen
        {
            get => _view.BottleLoading_isPopupOpen;
            set
            {
                _view.BottleLoading_isPopupOpen = value;
                OnPropertyChanged(nameof(BottleLoading_isPopupOpen));
            }
        }
        public string BottleLoading_BarcodeID
        {
            get => _view.BottleLoading_BarcodeID;
            set
            {
                _view.BottleLoading_BarcodeID = value;
                OnPropertyChanged(nameof(BottleLoading_BarcodeID));
            }
        }
        public string BottleLoading_PatientID
        {
            get => _view.BottleLoading_PatientID;
            set
            {
                _view.BottleLoading_PatientID = value;
                OnPropertyChanged(nameof(BottleLoading_PatientID));
            }
        }
        
        public string BottleLoading_Title
        {
            get => _view.BottleLoading_Title;
            set
            {
                _view.BottleLoading_Title = value;
                OnPropertyChanged(nameof(BottleLoading_Title));
            }
        }
        public string BottleLoading_Content
        {
            get => _view.BottleLoading_Content;
            set
            {
                _view.BottleLoading_Content = value;
                OnPropertyChanged(nameof(BottleLoading_Content));
            }
        }

        public string BottleLoading_WhatSystem
        {
            get => _view.BottleLoading_WhatSystem;
            set
            {
                _view.BottleLoading_WhatSystem = value;
                OnPropertyChanged(nameof(BottleLoading_WhatSystem));
            }
        }
        public string BottleLoading_Cell_Num
        {
            get => _view.BottleLoading_Cell_Num;
            set
            {
                _view.BottleLoading_Cell_Num = value;
                OnPropertyChanged(nameof(BottleLoading_Cell_Num));
            }
        }
        #endregion BottleLoading

        #region Barcode
        public ConcurrentQueue<Tuple<string>> Alarm_Barcode
        {
            get => _view.Alarm_Barcode;
            set
            {
                if (_view.Alarm_Barcode != value)
                {
                    _view.Alarm_Barcode = value;
                    OnPropertyChanged(nameof(Alarm_Barcode));
                }
            }
        }
        public bool Alarm_Barcode_isPopupOpen
        {
            get => _view.Alarm_Barcode_isPopupOpen;
            set
            {
                _view.Alarm_Barcode_isPopupOpen = value;
                OnPropertyChanged(nameof(Alarm_Barcode_isPopupOpen));
            }
        }
        public string Barcode_BarcodeID
        {
            get => _view.Barcode_BarcodeID;
            set
            {
                _view.Barcode_BarcodeID = value;
                OnPropertyChanged(nameof(Barcode_BarcodeID));
            }
        }
        public string Barcode_Content
        {
            get => _view.Barcode_Content;
            set
            {
                _view.Barcode_Content = value;
                OnPropertyChanged(nameof(Barcode_Content));
            }
        }
        #endregion Barcode

        #region Incubation
        public string Alarm_Incubation_BarcodeID
        {
            get => _view.Alarm_Incubation_BarcodeID;
            set
            {
                _view.Alarm_Incubation_BarcodeID = value;
                OnPropertyChanged(nameof(Alarm_Incubation_BarcodeID));
            }
        }
        public string Alarm_Incubation_PatientID
        {
            get => _view.Alarm_Incubation_PatientID;
            set
            {
                _view.Alarm_Incubation_PatientID = value;
                OnPropertyChanged(nameof(Alarm_Incubation_PatientID));
            }
        }
        
        public string Alarm_Incubation_Title
        {
            get => _view.Alarm_Incubation_Title;
            set
            {
                _view.Alarm_Incubation_Title = value;
                OnPropertyChanged(nameof(Alarm_Incubation_Title));
            }
        }
        public string Alarm_Incubation_whatSystem
        {
            get => _view.Alarm_Incubation_whatSystem;
            set
            {
                _view.Alarm_Incubation_whatSystem = value;
                OnPropertyChanged(nameof(Alarm_Incubation_whatSystem));
            }
        }
        public string Alarm_Incubation_Cell
        {
            get => _view.Alarm_Incubation_Cell;
            set
            {
                _view.Alarm_Incubation_Cell = value;
                OnPropertyChanged(nameof(Alarm_Incubation_Cell));
            }
        }
        public string Alarm_Incubation_Content
        {
            get => _view.Alarm_Incubation_Content;
            set
            {
                _view.Alarm_Incubation_Content = value;
                OnPropertyChanged(nameof(Alarm_Incubation_Content));
            }
        }
        #endregion Incubation

        #region Positive

        public ConcurrentQueue<Tuple<int>> Alarm_Positive
        {
            get => _view.Alarm_Positive;
            set
            {
                if (_view.Alarm_Positive != value)
                {
                    _view.Alarm_Positive = value;
                    OnPropertyChanged(nameof(Alarm_Positive));
                }
            }
        }
        public string Alarm_Positive_whatSystem
        {
            get => _view.Alarm_Positive_whatSystem;
            set
            {
                _view.Alarm_Positive_whatSystem = value;
                OnPropertyChanged(nameof(Alarm_Positive_whatSystem));
            }
        }

        public string Alarm_Positive_Warning
        {
            get => _view.Alarm_Positive_Warning;
            set
            {
                _view.Alarm_Positive_Warning = value;
                OnPropertyChanged(nameof(Alarm_Positive_Warning));
            }
        }

        public string Alarm_Positive_Cell
        {
            get => _view.Alarm_Positive_Cell;
            set
            {
                _view.Alarm_Positive_Cell = value;
                OnPropertyChanged(nameof(Alarm_Positive_Cell));
            }
        }
        #endregion Positive

        #region Negative_Unloading
        public string Alarm_Negative_Unloading_BarcodeID
        {
            get => _view.Alarm_Negative_Unloading_BarcodeID;
            set
            {
                _view.Alarm_Negative_Unloading_BarcodeID = value;
                OnPropertyChanged(nameof(Alarm_Negative_Unloading_BarcodeID));
            }
        }
        public string Alarm_Negative_Unloading_PatientID
        {
            get => _view.Alarm_Negative_Unloading_PatientID;
            set
            {
                _view.Alarm_Negative_Unloading_PatientID = value;
                OnPropertyChanged(nameof(Alarm_Negative_Unloading_PatientID));
            }
        }

        
        public string Alarm_Negative_Unloading_Title
        {
            get => _view.Alarm_Negative_Unloading_Title;
            set
            {
                _view.Alarm_Negative_Unloading_Title = value;
                OnPropertyChanged(nameof(Alarm_Negative_Unloading_Title));
            }
        }
        public string Alarm_Negative_Unloading_whatSystem
        {
            get => _view.Alarm_Negative_Unloading_whatSystem;
            set
            {
                _view.Alarm_Negative_Unloading_whatSystem = value;
                OnPropertyChanged(nameof(Alarm_Negative_Unloading_whatSystem));
            }
        }
        public string Alarm_Negative_Unloading_Cell
        {
            get => _view.Alarm_Negative_Unloading_Cell;
            set
            {
                _view.Alarm_Negative_Unloading_Cell = value;
                OnPropertyChanged(nameof(Alarm_Negative_Unloading_Cell));
            }
        }
        public string Alarm_Negative_Unloading_Content
        {
            get => _view.Alarm_Negative_Unloading_Content;
            set
            {
                _view.Alarm_Negative_Unloading_Content = value;
                OnPropertyChanged(nameof(Alarm_Negative_Unloading_Content));
            }
        }
        #endregion Negative_Unloading

        #region Positive_Unloading


        public string Alarm_Positive_Unloading_Title
        {
            get => _view.Alarm_Positive_Unloading_Title;
            set
            {
                _view.Alarm_Positive_Unloading_Title = value;
                OnPropertyChanged(nameof(Alarm_Positive_Unloading_Title));
            }
        }

        public string Alarm_Positive_Unloading_whatSystem
        {
            get => _view.Alarm_Positive_Unloading_whatSystem;
            set
            {
                _view.Alarm_Positive_Unloading_whatSystem = value;
                OnPropertyChanged(nameof(Alarm_Positive_Unloading_whatSystem));
            }
        }

        public string Alarm_Positive_Unloading_Cell
        {
            get => _view.Alarm_Positive_Unloading_Cell;
            set
            {
                _view.Alarm_Positive_Unloading_Cell = value;
                OnPropertyChanged(nameof(Alarm_Positive_Unloading_Cell));
            }
        }
        public string Alarm_Positive_Unloading_BarcodeID
        {
            get => _view.Alarm_Positive_Unloading_BarcodeID;
            set
            {
                _view.Alarm_Positive_Unloading_BarcodeID = value;
                OnPropertyChanged(nameof(Alarm_Positive_Unloading_BarcodeID));
            }
        }

        public string Alarm_Positive_Unloading_PatientID
        {
            get => _view.Alarm_Positive_Unloading_PatientID;
            set
            {
                _view.Alarm_Positive_Unloading_PatientID = value;
                OnPropertyChanged(nameof(Alarm_Positive_Unloading_PatientID));
            }
        }

        
        public string Alarm_Positive_Unloading_Warning
        {
            get => _view.Alarm_Positive_Unloading_Warning;
            set
            {
                _view.Alarm_Positive_Unloading_Warning = value;
                OnPropertyChanged(nameof(Alarm_Positive_Unloading_Warning));
            }
        }
        #endregion Positive_Unloading

        #region Door
        public string Alarm_Door_Title
        {
            get => _view.Alarm_Door_Title;
            set
            {
                _view.Alarm_Door_Title = value;
                OnPropertyChanged(nameof(Alarm_Door_Title));
            }
        }

        public string Alarm_Door_Content
        {
            get => _view.Alarm_Door_Content;
            set
            {
                _view.Alarm_Door_Content = value;
                OnPropertyChanged(nameof(Alarm_Door_Content));
            }
        }
        #endregion Door

        #region Equipment
        public string Alarm_Equipment_Title
        {
            get => _view.Alarm_Equipment_Title;
            set
            {
                _view.Alarm_Equipment_Title = value;
                OnPropertyChanged(nameof(Alarm_Equipment_Title));
            }
        }

        public string Alarm_Equipment_Content
        {
            get => _view.Alarm_Equipment_Content;
            set
            {
                _view.Alarm_Equipment_Content = value;
                OnPropertyChanged(nameof(Alarm_Equipment_Content));
            }
        }
        #endregion Equipment

        #region Temperature
        public string Alarm_Temperature_Title
        {
            get => _view.Alarm_Temperature_Title;
            set
            {
                _view.Alarm_Temperature_Title = value;
                OnPropertyChanged(nameof(Alarm_Temperature_Title));
            }
        }

        public string Alarm_Temperature_Content
        {
            get => _view.Alarm_Temperature_Content;
            set
            {
                _view.Alarm_Temperature_Content = value;
                OnPropertyChanged(nameof(Alarm_Temperature_Content));
            }
        }
        #endregion Temperature

        #endregion Alarm

        #region Buzzer
        public bool Buzzer
        {
            get => _view.Buzzer;
            set
            {
                _view.Buzzer = value;
                OnPropertyChanged(nameof(Buzzer));
            }
        }
        #endregion Buzzer

        #region Lamp
        public bool Lamp
        {
            get => _view.Lamp;
            set
            {
                _view.Lamp = value;
                OnPropertyChanged(nameof(Lamp));
            }
        }
        #endregion Buzzer


        #region Door
        public bool Door_previousDoorState
        {
            get => _view.Door_previousDoorState;
            set
            {
                _view.Door_previousDoorState = value;
                OnPropertyChanged(nameof(Door_previousDoorState));
            }
        }
        public bool Alarm_Door_Buzzer
        {
            get => _view.Alarm_Door_Buzzer;
            set
            {
                _view.Alarm_Door_Buzzer = value;
                OnPropertyChanged(nameof(Alarm_Door_Buzzer));
            }
        }



        #endregion Door

        #region Calibration
        public bool Calibration_Falg
        {
            get => _view.Calibration_Falg;
            set
            {
                _view.Calibration_Falg = value;
                OnPropertyChanged(nameof(Calibration_Falg));
            }
        }

        public int Calibration_From
        {
            get => _view.Calibration_From;
            set
            {
                _view.Calibration_From = value;
                OnPropertyChanged(nameof(Calibration_From));
            }
        }

        public int Calibration_To
        {
            get => _view.Calibration_To;
            set
            {
                _view.Calibration_To = value;
                OnPropertyChanged(nameof(Calibration_To));
            }
        }
        #endregion Calibration

        #region LOG
        public ConcurrentQueue<KeyValuePair<string, Enum_LOG>> Queue_LOG
        {
            get => _view.Queue_LOG;
            set
            {
                _view.Queue_LOG = value;
                OnPropertyChanged(nameof(Queue_LOG));
            }
        }
        #endregion LOG

        #region
        public void test()
        {
            string asdsadsadsadsa = "dsdf";
        }
        #endregion

        #region Class
        public DatabaseManager[] databaseManagercs = new DatabaseManager[20];
        public FastechDeviceManager fastechDeviceManager = new FastechDeviceManager();

        #region For
        public void ClassIni()
        {
            for (int i = 0; i < 20; i++)
            {
                databaseManagercs[i] = new DatabaseManager();
            }
        }
        #endregion For
        #endregion Class
    }
}
