using Azure;
using HarfBuzzSharp;
using HubCentra_A1.Model;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView.Painting;
using LiveChartsCore.SkiaSharpView.Painting.Effects;
using Microsoft.VisualBasic.Logging;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Runtime.Intrinsics.Arm;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using static HubCentra_A1.EnumManager;
using static HubCentra_A1.Model.View;
using static SkiaSharp.HarfBuzz.SKShaper;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Application = System.Windows.Application;
using Path = System.IO.Path;

namespace HubCentra_A1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ViewModel _viewModel;
        #region window
        public MainWindow(ViewModel viewModel) 
        {
            InitializeComponent();
            this._viewModel = viewModel;
            DataContext = this._viewModel;
            StartWorkerThreads();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DataTransferEvent();
            ViewModel();
            Alarmini();
            DatabaseManagerInitialize();
            ResultInitialize();
            FASTECHInitialize();
            PCBInitialize();
            SystemInitialize();
            TimerInitialize();
            BarcodeInitialize();
            TemperatureInitialize();
            LEDini();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            MessageBoxResult result = System.Windows.MessageBox.Show(this,
        "Do you really want to exit?",
        "Exit Confirmation",
        MessageBoxButton.YesNo,
        MessageBoxImage.Question);

            if (result == MessageBoxResult.No)
            {
                e.Cancel = true;
                return;
            }
            _viewModel.ProgramShotdown = true;
            Thread.Sleep(1000);
            PCB_SerialPortClose();
            System.Diagnostics.Process.GetCurrentProcess().Kill();
        }
        #endregion window

        #region ViewModel
        public void ViewModel()
        {
            HomeF.ViewModel(_viewModel);
            SearchF.ViewModel(_viewModel);
            ReportF.ViewModel(_viewModel);
            ConfigF.ViewModel(_viewModel);
            System_1F.ViewModel(_viewModel);

        }
        #endregion ViewModel

        #region Event
        public void DataTransferEvent()
        {
            _viewModel.ViewModelDataTransferEvent_MainWindow += ViewModel_MainWindow_ButtonEvent_TransferEvent;
            _viewModel.ViewModelDataTransferEvent_Login += ViewModel_Login_ButtonEvent_TransferEvent;
            _viewModel.ViewModelDataTransferEvent_Search += ViewModel_Search_ButtonEvent_TransferEvent;
            _viewModel.ViewModelDataTransferEvent_Report += ViewModel_Report_ButtonEvent_TransferEvent;
            _viewModel.ViewModelDataTransferEvent_Config += ViewModel_Config_ButtonEvent_TransferEvent;
            _viewModel.ViewModelDataTransferEvent_WriteBarcode += ViewModel_WriteBarcode_ButtonEvent_TransferEvent;
        }
        #endregion Event

        #region ButtonEvent
        #region MainWindow
        private void ViewModel_MainWindow_ButtonEvent_TransferEvent(Enum_MainWindow_ButtonEvent DataTransfer)
        {
            switch (DataTransfer)
            {
                case Enum_MainWindow_ButtonEvent.Home:
                    _viewModel.MainWindow_ButtonFlag = Enum_MainWindow_ButtonFlag.Home;
                    break;
                case Enum_MainWindow_ButtonEvent.Search:
                    _viewModel.MainWindow_ButtonFlag = Enum_MainWindow_ButtonFlag.Search;
                    select_Equipment_Search();
                    break;
                case Enum_MainWindow_ButtonEvent.Report:
                    _viewModel.MainWindow_ButtonFlag = Enum_MainWindow_ButtonFlag.Report;
                    break;
                case Enum_MainWindow_ButtonEvent.Conguration:
                    _viewModel.MainWindow_ButtonFlag = Enum_MainWindow_ButtonFlag.Conguration;
                    break;
                case Enum_MainWindow_ButtonEvent.SystemRack1:
                    _viewModel.MainWindow_ButtonFlag = Enum_MainWindow_ButtonFlag.SystemRack1;
                    break;
                case Enum_MainWindow_ButtonEvent.SystemRack2:
                    _viewModel.MainWindow_ButtonFlag = Enum_MainWindow_ButtonFlag.SystemRack2;
                    break;
                case Enum_MainWindow_ButtonEvent.SystemRack3:
                    _viewModel.MainWindow_ButtonFlag = Enum_MainWindow_ButtonFlag.SystemRack3;
                    break;
                case Enum_MainWindow_ButtonEvent.SystemRack4:
                    _viewModel.MainWindow_ButtonFlag = Enum_MainWindow_ButtonFlag.SystemRack4;
                    break;

                case Enum_MainWindow_ButtonEvent.Login:
                    LoginPage();
                    break;
                case Enum_MainWindow_ButtonEvent.Logout:
                    LogoutPage();
                    break;

                case Enum_MainWindow_ButtonEvent.Loading:
                    WriteBarcodepage();
                    break;

                case Enum_MainWindow_ButtonEvent.ResetBarcode:
                    ResetBarcode();
                    break;

                default:
                    break;
            }
        }
        #endregion MainWindow

        #region Login
        private void ViewModel_Login_ButtonEvent_TransferEvent(Enum_Login_ButtonEvent DataTransfer)
        {
            switch (DataTransfer)
            {
                case Enum_Login_ButtonEvent.OK:
                    UpdateLogin();
                    UpdateUserLoginStatus();
                    break;

                case Enum_Login_ButtonEvent.ADD:
                    Login_Add();
                    UpdateUserLoginStatus();
                    break;

                case Enum_Login_ButtonEvent.PASSWORD_SAVE:

                    break;
                case Enum_Login_ButtonEvent.ADD_CANCEL:

                    break;
                case Enum_Login_ButtonEvent.ADD_OPERATOR:
                    _viewModel.Login_LevelC = "OPERATOR";
                    break;

                case Enum_Login_ButtonEvent.ADD_ENGINEER:
                    _viewModel.Login_LevelC = "ENGINEER";
                    break;
                case Enum_Login_ButtonEvent.Delete:
                    Login_Delete();
                    break;

                default:
                    break;
            }
        }
        #endregion Login

        #region Search
        private void ViewModel_Search_ButtonEvent_TransferEvent(Enum_Search_ButtonEvent DataTransfer)
        {
            switch (DataTransfer)
            {
                case Enum_Search_ButtonEvent.Search:
                    select_Equipment_Search();
                    break;

                default:
                    break;
            }
        }
        #endregion Search

        #region Report
        private void ViewModel_Report_ButtonEvent_TransferEvent(Enum_Report_ButtonEvent DataTransfer)
        {
            switch (DataTransfer)
            {
                case Enum_Report_ButtonEvent.Search:
                    ReportSerach();
                    break;

                case Enum_Report_ButtonEvent.CSV:
                    //기울기();
                    CSV();
                    break;


                case Enum_Report_ButtonEvent.Chart:
                    LiveCharts();
                    break;

                case Enum_Report_ButtonEvent.Delete:
                    Delete();
                    break;
                default:
                    break;
            }
        }
        #endregion Report

        #region Config
        private void ViewModel_Config_ButtonEvent_TransferEvent(Enum_Config_ButtonEvent DataTransfer)
        {
            switch (DataTransfer)
            {
                case Enum_Config_ButtonEvent.Temp:
                    MessageBoxResult Templ_result = System.Windows.MessageBox.Show(
                        "작업을 실행하시겠습니까?",
                        "",
                        MessageBoxButton.YesNo,
                        MessageBoxImage.Question);

                    if (Templ_result == MessageBoxResult.No)
                    {
                        return;
                    }
                    UpdateConfig_double(DataTransfer, _viewModel.Config[0].Temp);
                    break;
                case Enum_Config_ButtonEvent.doorOpenAlarmTrigger:
                    MessageBoxResult doorOpenAlarmTrigger_result = System.Windows.MessageBox.Show(
             "작업을 실행하시겠습니까?",
             "",
             MessageBoxButton.YesNo,
             MessageBoxImage.Question);

                    if (doorOpenAlarmTrigger_result == MessageBoxResult.No)
                    {
                        return;
                    }
                    UpdateConfig_int(DataTransfer, _viewModel.Config[0].doorOpenAlarmTrigger);
                    break;
                case Enum_Config_ButtonEvent.MaximumTime:
                    MessageBoxResult MaximumTime_result = System.Windows.MessageBox.Show(
                        "작업을 실행하시겠습니까?",
                        "",
                        MessageBoxButton.YesNo,
                        MessageBoxImage.Question);

                    if (MaximumTime_result == MessageBoxResult.No)
                    {
                        return;
                    }
                    UpdateConfig_int(DataTransfer, _viewModel.Config[0].MaximumTime);
                    break;
                case Enum_Config_ButtonEvent.UseBuzzer:
                    MessageBoxResult UseBuzzer_result = System.Windows.MessageBox.Show(
                "작업을 실행하시겠습니까?",
                "",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

                    if (UseBuzzer_result == MessageBoxResult.No)
                    {
                        return;
                    }
                    UpdateConfig_bool(DataTransfer, _viewModel.Config[0].UseBuzzer);
                    break;
                case Enum_Config_ButtonEvent.Positive_Wait:
                    UpdateConfig_int(DataTransfer, _viewModel.Config[0].Positive_Wait);
                    break;
                case Enum_Config_ButtonEvent.Positive_Low:
                    UpdateConfig_int(DataTransfer, _viewModel.Config[0].Positive_Low);
                    break;
                case Enum_Config_ButtonEvent.Positive_High:
                    UpdateConfig_int(DataTransfer, _viewModel.Config[0].Positive_High);
                    break;
                case Enum_Config_ButtonEvent.Analysis_Time_Range:
                    UpdateConfig_int(DataTransfer, _viewModel.Config[0].Analysis_Time_Range);
                    break;
                case Enum_Config_ButtonEvent.Analysis_Intervals:
                    UpdateConfig_int(DataTransfer, _viewModel.Config[0].Analysis_Intervals);
                    break;
                case Enum_Config_ButtonEvent.Threshold:
                    UpdateConfig_double(DataTransfer, _viewModel.Config[0].Threshold);
                    break;
                case Enum_Config_ButtonEvent.BottleExistenceRange:
                    UpdateConfig_int(DataTransfer, _viewModel.Config[0].BottleExistenceRange);
                    break;
                case Enum_Config_ButtonEvent.DataStorageSave:
                    UpdateConfig_int(DataTransfer, _viewModel.Config[0].DataStorageSave);
                    break;
                case Enum_Config_ButtonEvent.Block_OK:
                    MessageBoxResult Block_OK_result = System.Windows.MessageBox.Show(
                    "해당 Cell을 Block 하시겠습니까?",
                    "",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);

                    if (Block_OK_result == MessageBoxResult.No)
                    {
                        return;
                    }
                    Blcok_OK(_viewModel.Config_Block);
                    break;
                case Enum_Config_ButtonEvent.Block_Cancel:
                    MessageBoxResult Block_Cancel_result = System.Windows.MessageBox.Show(
            "해당 Cell을 UnBlock 하시겠습니까?",
            "",
            MessageBoxButton.YesNo,
            MessageBoxImage.Question);

                    if (Block_Cancel_result == MessageBoxResult.No)
                    {
                        return;
                    }
                    Blcok_Cancel(_viewModel.Config_Block);
                    break;
                case Enum_Config_ButtonEvent.Block_Data:
                    UpdateConfig_int(DataTransfer, _viewModel.Config_Block);
                    break;
                case Enum_Config_ButtonEvent.Calibration_From:
                    UpdateConfig_int(DataTransfer, _viewModel.Calibration_From);
                    break;
                case Enum_Config_ButtonEvent.Calibration_To:
                    UpdateConfig_int(DataTransfer, _viewModel.Calibration_To);
                    break;
                case Enum_Config_ButtonEvent.Calibration_Start:
                    MessageBoxResult Calibration_result = System.Windows.MessageBox.Show(
                            "캘리브레이션을 실행하시겠습니까?",
                            "",
                            MessageBoxButton.YesNo,
                            MessageBoxImage.Question);

                    if (Calibration_result == MessageBoxResult.No)
                    {
                        return;
                    }
                    _viewModel.Calibration_Falg = true;
                    break;
                case Enum_Config_ButtonEvent.Calibration_Target:
                    UpdateConfig_int(DataTransfer, _viewModel.PCB_targetvalue);
                    break;
                case Enum_Config_ButtonEvent.SYSTEM1:
                    UpdateConfig_bool(DataTransfer, _viewModel.Config[0].SYSTEM1);
                    break;
                case Enum_Config_ButtonEvent.SYSTEM2:
                    UpdateConfig_bool(DataTransfer, _viewModel.Config[0].SYSTEM2);
                    break;
                case Enum_Config_ButtonEvent.SYSTEM3:
                    UpdateConfig_bool(DataTransfer, _viewModel.Config[0].SYSTEM3);
                    break;
                case Enum_Config_ButtonEvent.SYSTEM4:
                    UpdateConfig_bool(DataTransfer, _viewModel.Config[0].SYSTEM4);
                    break;


                case Enum_Config_ButtonEvent.Equipment_Change:
                    MessageBoxResult Equipment_Change_result = System.Windows.MessageBox.Show(
                     "작업을 실행하시겠습니까?",
                     "",
                     MessageBoxButton.YesNo,
                     MessageBoxImage.Question);

                    if (Equipment_Change_result == MessageBoxResult.No)
                    {
                        return;
                    }
                    Equipment_Change();
                    break;

                case Enum_Config_ButtonEvent.NegativeOnOFF:
                    UpdateConfig_bool(DataTransfer, _viewModel.Config[0].NegativeOnOFF);
                    break;
                default:
                    break;
            }
        }

        private void UpdateConfig_int(Enum_Config_ButtonEvent DataTransfer, double transferValue)
        {
            Calculator calculator = new Calculator(_viewModel, transferValue.ToString());
            calculator.ShowDialog();
            if (calculator.DialogResult.HasValue && calculator.DialogResult.Value)
            {
                if (int.TryParse(_viewModel.Calculator_DisplayTextBlock, out int value))
                {
                    switch (DataTransfer)
                    {
                        case Enum_Config_ButtonEvent.doorOpenAlarmTrigger:
                            if (value > 0 && value < 100)
                            {
                                _viewModel.Config[0].doorOpenAlarmTrigger = value;
                                Door_timer_Stop();
                                Door_timer_Start();
                            }

                            break;
                        case Enum_Config_ButtonEvent.Positive_Wait:
                            _viewModel.Config[0].Positive_Wait = value;
                            break;
                        case Enum_Config_ButtonEvent.Positive_Low:
                            _viewModel.Config[0].Positive_Low = value;
                            break;
                        case Enum_Config_ButtonEvent.Positive_High:
                            _viewModel.Config[0].Positive_High = value;
                            break;
                        case Enum_Config_ButtonEvent.Analysis_Time_Range:
                            _viewModel.Config[0].Analysis_Time_Range = value;
                            break;
                        case Enum_Config_ButtonEvent.Analysis_Intervals:
                            _viewModel.Config[0].Analysis_Intervals = value;
                            break;
                        case Enum_Config_ButtonEvent.BottleExistenceRange:
                            _viewModel.Config[0].BottleExistenceRange = value;
                            break;
                        case Enum_Config_ButtonEvent.DataStorageSave:
                            _viewModel.Config[0].DataStorageSave = value;
                            DataStorageSave_timer_Stop();
                            DataStorageSave_timer_Start();
                            break;
                        case Enum_Config_ButtonEvent.TrashCanFillLevel:
                            _viewModel.Config[0].TrashCanFillLevel = value;
                            break;
                        case Enum_Config_ButtonEvent.MaximumTime:
                            if (value > 0 && value < 100)
                            {
                                _viewModel.Config[0].MaximumTime = value;
                            }        
                            break;
                        case Enum_Config_ButtonEvent.Block_Data:
                            _viewModel.Config_Block = value;
                            break;
                        case Enum_Config_ButtonEvent.Calibration_From:
                            if (value > 0 && value < 85)
                            {
                                _viewModel.Calibration_From = value;
                            }
                            break;
                        case Enum_Config_ButtonEvent.Calibration_To:
                            if (value > 0 && value < 85)
                            {
                                _viewModel.Calibration_To = value;
                            }
                            break;
                        case Enum_Config_ButtonEvent.Calibration_Target:
                            if (value > 0 && value < 500)
                            {
                                _viewModel.PCB_targetvalue = value;
                            }
                            break;

                            
                    }
                    _viewModel.databaseManagercs[(int)Enum_DatabaseManager.MainWindow_UpdateConfig].UpdateConfig(_viewModel.Config);
                    var select_ConfigInfo = _viewModel.databaseManagercs[(int)Enum_DatabaseManager.MainWindow_UpdateConfig].Select_Config();
                    _viewModel.Config = new List<Class_Config>(select_ConfigInfo);

                }
                else
                {
                }
            }
        }
        private void UpdateConfig_double(Enum_Config_ButtonEvent DataTransfer, double transferValue)
        {
            Calculator calculator = new Calculator(_viewModel, transferValue.ToString());
            calculator.ShowDialog();
            if (calculator.DialogResult.HasValue && calculator.DialogResult.Value)
            {
                if (double.TryParse(_viewModel.Calculator_DisplayTextBlock, out double value))
                {
                    switch (DataTransfer)
                    {
                        case Enum_Config_ButtonEvent.Temp:
                            if (value >= 30 && value <= 45)
                            {
                                _viewModel.Config[0].Temp = value;
                            }
                            break;
                        case Enum_Config_ButtonEvent.LoadCellMin:
                            _viewModel.Config[0].LoadCellMin = value;
                            break;
                        case Enum_Config_ButtonEvent.LoadCellMax:
                            _viewModel.Config[0].LoadCellMax = value;
                            break;
                        case Enum_Config_ButtonEvent.Threshold:
                            _viewModel.Config[0].Threshold = value;
                            break;
                    }
                    _viewModel.databaseManagercs[(int)Enum_DatabaseManager.MainWindow_UpdateConfig].UpdateConfig(_viewModel.Config);
                    var select_ConfigInfo = _viewModel.databaseManagercs[(int)Enum_DatabaseManager.MainWindow_UpdateConfig].Select_Config();
                    _viewModel.Config = new List<Class_Config>(select_ConfigInfo);
                }
                else
                {
                }
            }
        }
        private void UpdateConfig_bool(Enum_Config_ButtonEvent DataTransfer, bool transferValue)
        {
            switch (DataTransfer)
            {
                case Enum_Config_ButtonEvent.UseBuzzer:
                    _viewModel.Config[0].UseBuzzer = !transferValue;
                    break;
                case Enum_Config_ButtonEvent.SYSTEM1:
                    _viewModel.Config[0].SYSTEM1 = !transferValue;
                    break;
                case Enum_Config_ButtonEvent.SYSTEM2:
                    _viewModel.Config[0].SYSTEM2 = !transferValue;
                    break;
                case Enum_Config_ButtonEvent.SYSTEM3:
                    _viewModel.Config[0].SYSTEM3 = !transferValue;
                    break;
                case Enum_Config_ButtonEvent.SYSTEM4:
                    _viewModel.Config[0].SYSTEM4 = !transferValue;
                    break;
                case Enum_Config_ButtonEvent.NegativeOnOFF:
                    _viewModel.Config[0].NegativeOnOFF = !transferValue;
                    break;
            }
            _viewModel.databaseManagercs[(int)Enum_DatabaseManager.MainWindow_UpdateConfig].UpdateConfig(_viewModel.Config);
            var select_ConfigInfo = _viewModel.databaseManagercs[(int)Enum_DatabaseManager.MainWindow_UpdateConfig].Select_Config();
            _viewModel.Config = new List<Class_Config>(select_ConfigInfo);
        }
        #endregion Config

        #region WriteBarcode
        private void ViewModel_WriteBarcode_ButtonEvent_TransferEvent(Enum_WriteBarcode_ButtonEvent DataTransfer)
        {
            switch (DataTransfer)
            {
                case Enum_WriteBarcode_ButtonEvent.OK:
                    select_Equipment_Search();
                    break;

                default:
                    break;
            }
        }
        #endregion WriteBarcode
        #endregion ButtonEvent

        #region Thread
        private CancellationTokenSource _cancellationTokenSource;
        public void StartWorkerThreads()
        {
            _cancellationTokenSource = new CancellationTokenSource();
            var token = _cancellationTokenSource.Token;

            for (int i = 0; i < 30; i++)
            {
                EnumMStartWorkerThreads threadName = (EnumMStartWorkerThreads)(i);
                Task.Run(async () => await DoWorkAsync(threadName, token), token);
            }
        }
        public void StopWorkerThreads()
        {
            _cancellationTokenSource?.Cancel();
        }
        private async Task DoWorkAsync(EnumMStartWorkerThreads threadName, CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                switch (threadName)
                {
                    case EnumMStartWorkerThreads.Loading:
                        Loading_formLoadingProgressset();
                        await Task.Delay(10, token);
                        break;

                    case EnumMStartWorkerThreads.PCB:
                        if(!_viewModel.ProgramShotdown)
                        {
                            PCB_PerformDataAcquisition_Manual();
                            Thread.Sleep(10);
                        }
                        break;
                    case EnumMStartWorkerThreads.select_Equipment:
                        if (_viewModel.DatabaseManager_Connection)
                        {
                            select_Equipment();
                        }
                        await Task.Delay(10, token);
                        break;

                    case EnumMStartWorkerThreads.Insert_EquipmentH:
                        await Task.Delay(100, token);
                        break;

                    case EnumMStartWorkerThreads.FASTECH:
                        FASTECH();
                        await Task.Delay(1, token);
                        break;

                    case EnumMStartWorkerThreads.Barcode:
                        if (_viewModel.Barcode_Connection)
                        {
                            await Barcode_WriteAsync();
                        }
                        await Task.Delay(100, token);
                        break;

                    case EnumMStartWorkerThreads.Temperature:
                        if (_viewModel.Temperature_Connection)
                        {
                            await Task.Delay(100, token);
                            await Temperature_WriteAsync();
                        }
                        await Task.Delay(10, token);
                        break;


                    case EnumMStartWorkerThreads.Result:
                        await Result();
                        await Task.Delay(100, token);
                        break;

                    case EnumMStartWorkerThreads.PopStatus:

                        await Task.Delay(300, token);
                        break;
                    case EnumMStartWorkerThreads.로딩:
                        로딩();
                        Thread.Sleep(100);
                        break;

                    case EnumMStartWorkerThreads.언로딩:
                        언로딩();
                        Thread.Sleep(100);
                        break;
                    case EnumMStartWorkerThreads.PositiveDelay:
                        PositiveDelay();
                        Thread.Sleep(1000);
                        break;
                    case EnumMStartWorkerThreads.Lamp:
                        Lamp();
                        Thread.Sleep(500);
                        break;

                    case EnumMStartWorkerThreads.Tilting:
                        문상태확인및틸팅제어();
                        Thread.Sleep(10);
                        break;
                    case EnumMStartWorkerThreads.Calibration:
                        if (_viewModel.Calibration_Falg == true)
                        {
                            Calibration(); 
                        }
                        Thread.Sleep(100);
                        break;

                    default:
                        Console.WriteLine($"스레드 {threadName} 작업 중...");
                        await Task.Delay(100, token);
                        break;
                }
            }
            Console.WriteLine($"스레드 {threadName}가 취소 요청으로 중단됨.");
        }

        #endregion Thread

        #region Timer
        private DispatcherTimer Door_timer = new DispatcherTimer();
        private DispatcherTimer Alarm_Positive_timer = new DispatcherTimer();
        private DispatcherTimer BottleLoading_timer = new DispatcherTimer();
        private DispatcherTimer Barcode_timer = new DispatcherTimer();
        private DispatcherTimer LOG_timer = new DispatcherTimer();
        private DispatcherTimer DataStorageSave_timer = new DispatcherTimer();
        private DispatcherTimer Buzzer_timer = new DispatcherTimer();
        private DispatcherTimer Alarm_System_timer = new DispatcherTimer();
        private DispatcherTimer FASTECH_Pingcheck = new DispatcherTimer();
        private DispatcherTimer Temperature_timer = new DispatcherTimer();
        

        public async void TimerInitialize()
        {
            BottleLoading_timer.Tick += TimerCallback_BottleLoading;
            BottleLoading_timer.Interval = TimeSpan.FromMilliseconds(100);
            BottleLoading_timer.Start();

            Barcode_timer.Tick += TimerCallback_Barcode;
            Barcode_timer.Interval = TimeSpan.FromMilliseconds(100);
            Barcode_timer.Start();

            Alarm_Positive_timer.Tick += TimerCallback_Alarm_Positive;
            Alarm_Positive_timer.Interval = TimeSpan.FromMilliseconds(100);
            Alarm_Positive_timer.Start();

            LOG_timer.Tick += TimerCallback_LOG_timer;
            LOG_timer.Interval = TimeSpan.FromMilliseconds(1000);
            LOG_timer.Start();

            Buzzer_timer.Tick += TimerCallbacks_Buzzer_timer;
            Buzzer_timer.Interval = TimeSpan.FromMilliseconds(300);
            Buzzer_timer.Start();

            //Alarm_System_timer.Tick += TimerCallbacks_Alarm_System_timer;
            //Alarm_System_timer.Interval = TimeSpan.FromMilliseconds(1000);
            //Alarm_System_timer.Start();


            FASTECH_Pingcheck.Tick += TimerCallbacks_FASTECH_Pingcheck_timer;
            FASTECH_Pingcheck.Interval = TimeSpan.FromMilliseconds(10);
            FASTECH_Pingcheck.Start();

            Temperature_timer.Tick += TimerCallbacks_Temperature_timer;
            Temperature_timer.Interval = TimeSpan.FromSeconds(1);
            Temperature_timer.Start();


            Door_timer.Tick += TimerCallbacks_Door_timer;


            DataStorageSave_timer_Start();
        }


        #endregion Timer

        #region FASTECH
        public async void FASTECHInitialize()
        {

            try
            {
                _viewModel.FASTECH_Set_Output = new List<Class_FASTECH_Output>();
                for (int i = 0; i < 8; i++)
                {
                    _viewModel.FASTECH_Set_Output.Add(new Class_FASTECH_Output { Flag = false });
                }
                IPAddress ipAddress = IPAddress.Parse(_viewModel.SystemInfo[0]._FASTECH_IO_Input_IP);
                _viewModel.FASTECH_IO_Connection = _viewModel.fastechDeviceManager.Connect_IO(Enum_FASTECH_ID.IO, ipAddress);
                _viewModel.FASTECH_Set_Output[(int)Enum_FASTECH_Output.GreenLamp].Flag = true;
                Thread.Sleep(100);
            }
            catch(Exception ex)
            {

            }
        }

        public void FASTECH()
        {
            try
            {
                if (_viewModel.FASTECH_IO_Connection)
                {
                    FASTECH_Get();
                    FASTECH_Set();
                }
                else
                {
                    IPAddress ipAddress = IPAddress.Parse(_viewModel.SystemInfo[0]._FASTECH_IO_Input_IP);
                    _viewModel.FASTECH_IO_Connection = _viewModel.fastechDeviceManager.Connect_IO(Enum_FASTECH_ID.IO, ipAddress);
                    Thread.Sleep(1000);
                }
            }
            catch(Exception ex)
            {

            }
        }

      
        public void FASTECH_Get()
        {        
            if (_viewModel.FASTECH_IO_Connection)
            {
                var GetStatus = _viewModel.fastechDeviceManager.Get_Input(Enum_FASTECH_ID.IO);
                _viewModel.FASTECH_Input = GetStatus;
                var GetStatus2 = _viewModel.fastechDeviceManager.Get_Output(Enum_FASTECH_ID.IO);
                _viewModel.FASTECH_Output = GetStatus2;
                if (_viewModel.FASTECH_Input[(int)Enum_FASTECH_Input.Trigger].Latch >10000)
                {
                    _viewModel.fastechDeviceManager.ClearLatch(Enum_FASTECH_ID.IO);
                    _viewModel.FASTECH_Input_Latch = 0;
                }
                //if(GetStatus == null)
                //{
                //    _viewModel.FASTECH_Input_Latch = -1;
                //    _viewModel.FASTECH_IO_Connection = false;
                //    _viewModel.PCB_Restart = false; // 통신이 끊어졌음을 표시
                //    _viewModel.PCB_Status = false;
                //}
                //else
                //{
                //    if (!_viewModel.PCB_Restart)
                //    {
                //        Thread.Sleep(100);
                //        PCBFrame(); // 처음 연결되었을 때만 호출
                //        LEDini();
                //        _viewModel.PCB_Restart = true; // 통신이 연결되었음을 표시
                //    }
                //}
            }
        }

        public void FASTECH_Set()
        {
            _viewModel.fastechDeviceManager.Set_Output(Enum_FASTECH_ID.IO, _viewModel.FASTECH_Set_Output);
        }


        public void TimerCallbacks_FASTECH_Pingcheck_timer(object sender, EventArgs e)
        {
            try
            {
                //Pingcheck(_viewModel.SystemInfo[0].FASTECH_IO_Input_IP);
            }
            catch (Exception ex)
            {
            }
        }

        public void Pingcheck(string ping)
        {
            string host = ping;
            int timeout = 200;
            int requiredSuccesses = 5;
            int successCount = 0;
            try
            {
                Ping pingSender = new Ping();
                PingOptions options = new PingOptions();
                options.DontFragment = true;

                string data = "aaaaaaaaaaaaaaaa";
                byte[] buffer = System.Text.Encoding.ASCII.GetBytes(data);

                for (int i = 0; i < requiredSuccesses; i++)
                {
                    PingReply reply = pingSender.Send(host, timeout, buffer, options);

                    if (reply.Status == IPStatus.Success && reply.RoundtripTime <= timeout)
                    {

                        successCount++;
                    }
                    else
                    {
                        break;
                    }
                }

                if (successCount >= 1)
                {
                    if (!_viewModel.PCB_Restart)
                    {
                        Thread.Sleep(100);
                        PCBFrame(); // 처음 연결되었을 때만 호출
                        LEDini();
                        _viewModel.PCB_Restart = true; // 통신이 연결되었음을 표시
                    }
                }
                else
                {
                    _viewModel.FASTECH_Input_Latch = -1;
                    _viewModel.FASTECH_IO_Connection = false;
                    _viewModel.PCB_Restart = false; // 통신이 끊어졌음을 표시
                    _viewModel.PCB_Status = false;
                }


            }
            catch (Exception ex)
            {

            }
        }

        #endregion FASTECH

        #region DatabaseManager
        public void DatabaseManagerInitialize()
        {
            _viewModel.databaseManagercs[(int)Enum_DatabaseManager.Common].CreateTable();
            var select_SystemInfo = _viewModel.databaseManagercs[(int)Enum_DatabaseManager.Common].Select_SystemInfo();
            _viewModel.SystemInfo = new List<DatabaseManager_System>(select_SystemInfo);

            var select_LoginInfo = _viewModel.databaseManagercs[(int)Enum_DatabaseManager.Common].Select_LoginInfo();
            _viewModel.LoginInfo = new List<DatabaseManager_Login>(select_LoginInfo);
            UpdateUserLoginStatus();


            var select_FASTECHInfo = _viewModel.databaseManagercs[(int)Enum_DatabaseManager.Common].Select_FASTECHInfo();
            _viewModel.FASTECHInfo = new List<DatabaseManager_FASTECH_Parameter>(select_FASTECHInfo);

            var select_ConfigInfo = _viewModel.databaseManagercs[(int)Enum_DatabaseManager.Common].Select_Config();
            _viewModel.Config = new List<Class_Config>(select_ConfigInfo);


            var select_Equipment = _viewModel.databaseManagercs[(int)Enum_DatabaseManager.Common].Select_Equipment();
            _viewModel.EquipmentInfo = select_Equipment;
            string updateQuery = "UPDATE Equipment SET Switched = @Switched WHERE isEnable = @isEnable AND Result = @Result";
            Dictionary<string, object> parameters = new Dictionary<string, object>
                {
                    { "@Switched", true },
                    { "@isEnable", true },
                    { "@Result", "Incubation" }
                };
            _viewModel.databaseManagercs[(int)Enum_DatabaseManager.MainWindow_MainEngine].UpdateEquipment(updateQuery, parameters);


            _viewModel.DatabaseManager_Connection = true;
        }

        #region Equipment
        public void select_Equipment()
        {
            try
            {
                var select_Equipment = _viewModel.databaseManagercs[(int)Enum_DatabaseManager.MainWindow_select_Equipment].Select_Equipment();
                _viewModel.EquipmentInfo = select_Equipment;

                for (int i = 0; i < _viewModel.EquipmentInfo.Count; i++)
                {
                    if (_viewModel.EquipmentInfo[i].Result == "Incubation")
                    {

                    }

                }
                int totalEquipment = _viewModel.Common_CellCount * 3;
                int incubationCount = _viewModel.EquipmentInfo.Count(e => e.isActive && e.isEnable && e.Result == "Incubation");
                int positiveCount = _viewModel.EquipmentInfo.Count(e => e.isActive && e.isEnable && e.Result == "Positive");
                int negativeCount = _viewModel.EquipmentInfo.Count(e => e.isActive && e.isEnable && e.Result == "Negative");
                bool hasAnyEmptyEnabled = _viewModel.EquipmentInfo.Any(e => e.isActive && !e.isEnable);
                // Available 계산
                int available = totalEquipment - incubationCount - positiveCount - negativeCount;
                int incubation = incubationCount + positiveCount + negativeCount;
                _viewModel.Home_Available = available;
                _viewModel.Home_Incubation = incubation;
                _viewModel.Home_Positive = positiveCount;
                _viewModel.Home_Negative = negativeCount;

                _viewModel.Home_Incubation_Flag = incubationCount > 0;
                _viewModel.Home_Positive_Flag = positiveCount > 0;
                _viewModel.Home_Negative_Flag = negativeCount > 0;




            }
            catch (Exception ex)
            {

            }
        }


        #endregion Equipment


        #region EquipmentH

        public void Insert_EquipmentH(List<PCB> pcb_data, DateTime Now)
        {
            try
            {
               int incubationTime = _viewModel.Config[0].DataStorageSave * 60;
                var select_Equipment = _viewModel.databaseManagercs[(int)Enum_DatabaseManager.MainWindow_Insert_EquipmentH].Select_Equipment_Search();
                DateTime now = Now;
                for (int i = 0; i < select_Equipment.Count; i++)
                { 
                    int IncubationTime = select_Equipment[i].IncubationTime + incubationTime;
                    int index = select_Equipment[i].ID - 1;
                    int ID = select_Equipment[i].ID;
                    string QrcodeID = select_Equipment[i].Qrcode;
                    string BarcodeID = select_Equipment[i].Barcode;
                    string Result = select_Equipment[i].Result;
                    double PcbADC = pcb_data[index].ADC;
                    double PcbLED = pcb_data[index].LED;
                    if (PcbADC >= _viewModel.Config[0].BottleExistenceRange || PcbADC <= 0)
                    {
                        continue;
                    }
                    double Temperature_ProcessValue = _viewModel.Temperature_ProcessValue;
                    string updateQuery_Equipment = "UPDATE Equipment SET IncubationTime = @IncubationTime WHERE ID = @ID";
                    Dictionary<string, object> UpdateEquipment_parameters = new Dictionary<string, object>
                        {
                            { "@IncubationTime", IncubationTime },
                            { "@ID", ID }  // 
                        };
                    _viewModel.databaseManagercs[(int)Enum_DatabaseManager.MainWindow_Insert_EquipmentH].UpdateEquipment(updateQuery_Equipment, UpdateEquipment_parameters);


                    string updateQuery_Barcode = "UPDATE Barcode SET IncubationTime = @IncubationTime WHERE Barcode = @Barcode";
                    Dictionary<string, object> UpdateEBarcode_parameters = new Dictionary<string, object>
                        {
                            { "@IncubationTime", IncubationTime },
                            { "@Barcode", BarcodeID }  // 
                        };
                    _viewModel.databaseManagercs[(int)Enum_DatabaseManager.MainWindow_Insert_EquipmentH].UpdateBarcode(updateQuery_Barcode, UpdateEBarcode_parameters);

                    if (Result == "Negative" && !_viewModel.Config[0].NegativeOnOFF )
                    {

                    }
                    else
                    {
                        List<DatabaseManager_EquipmentH> EquipmentH = new List<DatabaseManager_EquipmentH>();
                        EquipmentH.Add(new DatabaseManager_EquipmentH { ID = ID, Barcode = BarcodeID, Qrcode = QrcodeID, PcbADC = PcbADC, PcbLED = PcbLED, Temperature = Temperature_ProcessValue, CreDate = now });
                        _viewModel.databaseManagercs[(int)Enum_DatabaseManager.MainWindow_Insert_EquipmentH].InsertEquipmentH(now, EquipmentH);
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
        #endregion EquipmentH

        #region Search
        public void select_Equipment_Search()
        {
            try
            {
                var select_Equipment = _viewModel.databaseManagercs[(int)Enum_DatabaseManager.MainWindow_select_Equipment_Search].Select_Equipment_Search();

                var configurations = new List<DatabaseManager_Equipmentstring>();

                for (int i = 0; i < select_Equipment.Count; i++)
                {
                    var equipment = select_Equipment[i];
                    var data = new DatabaseManager_Equipmentstring
                    {
                        ID = equipment.ID,
                        Cell = equipment.Cell ?? null,
                        Barcode = equipment.Barcode ?? null,
                        Qrcode = equipment.Qrcode ?? null,
                        Loading = equipment.Loading ?? (DateTime?)null,
                        CreDate = equipment.CreDate ?? (DateTime?)null,
                        PositiveTime = equipment.PositiveTime ?? (DateTime?)null,
                        Result = equipment.Result ?? null,
                        IncubationTime = ConvertSecondsToReadableTime(equipment.IncubationTime),
                        isEnable = equipment.isEnable,
                        isActive = equipment.isActive ,
                    };
                    configurations.Add(data);
                }
                _viewModel.Search_List = configurations;

            }
            catch (Exception ex)
            {

            }
        }
        private string ConvertSecondsToReadableTime(int totalSeconds)
        {
            TimeSpan time = TimeSpan.FromSeconds(totalSeconds);
            return $"{time.Days}일 {time.Hours}시간 {time.Minutes}분";
        }
        #endregion Search
        #endregion DatabaseManager

        #region Result
        public void ResultInitialize()
        {
            try
            {
                _viewModel.Result_PositiveTime = new Dictionary<int, Queue<(DateTime, double)>>();

                for (int i = 0; i < _viewModel.Common_TotalSystemCellCount; i++)
                {
                    _viewModel.Result_PositiveTime[i] = new Queue<(DateTime, double)>();

                }
            }
            catch(Exception ex)
            {

            }
        }
        public async Task Result()
        {
            try
            {
                var filteredItemsSwitched = _viewModel.EquipmentInfo.Where(e => e.isActive && e.isEnable && e.Switched).ToList();
                int MaximumTime = _viewModel.Config[0].MaximumTime * _viewModel.Common_Day;
                DateTime now = DateTime.Now;
                List<PCB> pcb_data = new List<PCB>();
                pcb_data = _viewModel.PCB_Data;
                foreach (var item in filteredItemsSwitched)
                {
                    int ID = item.ID;
                    int index = item.ID - 1;
                    double IncubationTime = item.IncubationTime;
                    string barcodeID = _viewModel.EquipmentInfo[index].Barcode;
                    string Qrcode = _viewModel.EquipmentInfo[index].Qrcode;
                    double PcbADC = pcb_data[index].ADC;
                    double PcbLED = pcb_data[index].LED;
                    string result = DetermineResult(index, IncubationTime, MaximumTime, now, PcbADC, PcbLED);
                    if (!string.IsNullOrEmpty(result))
                    {

                        Enum_MainEngine_Statuslist status = result == "Positive" ? Enum_MainEngine_Statuslist.Positive : Enum_MainEngine_Statuslist.Negative;

                        double Temperature_ProcessValue = _viewModel.Temperature_ProcessValue;
                        List<DatabaseManager_EquipmentH> Equipment = new List<DatabaseManager_EquipmentH>();
                        Equipment.Add(new DatabaseManager_EquipmentH { ID = item.ID, Barcode = barcodeID, Qrcode = Qrcode, PcbADC = PcbADC, PcbLED = PcbLED, Temperature = Temperature_ProcessValue, CreDate = now });
                        _viewModel.databaseManagercs[(int)Enum_DatabaseManager.MainWindow_Insert_EquipmentH].InsertEquipmentH(now, Equipment);

                        string UpdateBarcode_Query = "UPDATE Barcode SET " +
                         "PositiveTime = @PositiveTime, " +
                         "Result = @Result " +
                         "WHERE Barcode = @Barcode";

                        Dictionary<string, object> UpdateBarcode_parameters = new Dictionary<string, object>
                        {
                            { "@PositiveTime", now},
                            { "@Result", result},
                            { "@Barcode", barcodeID },
                        };
                        _viewModel.databaseManagercs[(int)Enum_DatabaseManager.MainWindow_Result].UpdateBarcode(UpdateBarcode_Query, UpdateBarcode_parameters);




                        string UpdateEquipment_Query = "UPDATE Equipment SET " +
                             "PositiveTime = @PositiveTime, " +
                             "Result = @Result, " +
                             "Switched = @Switched " +            
                             "WHERE ID = @ID";
                        Dictionary<string, object> UpdateEquipment_parameters = new Dictionary<string, object>
                        {   
                            { "@PositiveTime", now},
                            { "@Result", result},
                            { "@Switched", false},
                            { "@ID", item.ID },
                        };
                        _viewModel.databaseManagercs[(int)Enum_DatabaseManager.MainWindow_Result].UpdateEquipment(UpdateEquipment_Query, UpdateEquipment_parameters);


                        PCB_LED(_viewModel.SystemInfo[0].PCB_ID1, index, status.ToString());
                    }
                }
                if(_viewModel.Result_Timer)
                {
                    Insert_EquipmentH(pcb_data, now);
                    _viewModel.Result_Timer = false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

      

        private string DetermineResult(int idx, double incubationTime, int maximumTime, DateTime date, double adc, double led)
        {
            try
            {
                DateTime Date = date;
                int Positive_Delay = _viewModel.Config[0].Positive_Wait;
                double IncubationTime = incubationTime;
<<<<<<< HEAD
                double ADC = adc;
                double LED = led;
                if (ADC <= 0)
                {
                    return "";
                }
                if (_viewModel.Result_Timer)
                {
               
                    if (IncubationTime >= _viewModel.Common_Hour && ADC < _viewModel.Config[0].BottleExistenceRange)
                    {
                        bool result1 = ReceiveNewVoltageValue(idx, ADC, Date);
                        if (result1 )
                        {
                            return "Positive";
                        }
                    }

=======
                double ADC = _viewModel.PCB_Data[idx].ADC;

                if (ADC <= 0)
                {
                    return "";
                }
                if (_viewModel.Result_Timer)
                {
                    if (IncubationTime >= _viewModel.Common_Hour && ADC < _viewModel.Config[0].BottleExistenceRange)
                    {
                        bool result1 = ReceiveNewVoltageValue(idx, ADC, Date);
 
                        if (result1 )
                        {
                            return "Positive";
                        }
                    }
>>>>>>> 499d608513e1843495adb00cd694c16041388782
                }
                if (_viewModel.PositiveDelay[idx] > Positive_Delay)
                {
                    return "Positive";
                }
                if (IncubationTime >= maximumTime)
                {
                    return "Negative";
                }
                return "";
            }
            catch( Exception ex)
            {
                return "";
            }
        
        }

        private bool ReceiveNewVoltageValue(int pcbIndex, double currentvalue, DateTime TIME)

        {
            try
            {
                DateTime now = TIME;
                int index = pcbIndex;
                double Currentvalue = currentvalue;
                int analysis_Time_Range = _viewModel.Config[0].Analysis_Time_Range;
                int number_of_Analysis_Intervals = _viewModel.Config[0].Analysis_Intervals;
                double voltage_Increase_Threshold = _viewModel.Config[0].Threshold;
                double percentageDifference = 0;

                if (!_viewModel.Result_PositiveTime.ContainsKey(pcbIndex))
                {
                    _viewModel.Result_PositiveTime[pcbIndex] = new Queue<(DateTime, double)>();
                }

                _viewModel.Result_PositiveTime[pcbIndex].Enqueue((now, Currentvalue));
                while (_viewModel.Result_PositiveTime[pcbIndex].Count > 0 && (now - _viewModel.Result_PositiveTime[pcbIndex].Peek().Item1).TotalMinutes > analysis_Time_Range)
                {
                    _viewModel.Result_PositiveTime[pcbIndex].Dequeue();
                }

                var data = _viewModel.Result_PositiveTime[pcbIndex];
                int rage = (int)(analysis_Time_Range );

                if (data.Count < rage)
                {
                    return false;
                }

                int Size = analysis_Time_Range / number_of_Analysis_Intervals;
                List<double> Averages = new List<double>();

                for (int i = 0; i < number_of_Analysis_Intervals; i++)
                {
                    List<double> Data = data.Skip(i * Size).Take(Size).Select(d => d.Item2).ToList();
                    double average = Data.Average();
                    Averages.Add(average);
                }
                bool allQuartersPositiveIncrease = true;
                for (int i = 1; i < number_of_Analysis_Intervals; i++)
                {
                    percentageDifference = ((Averages[i] - Averages[i - 1]) / Averages[i - 1]) * 100.0;

                    if (percentageDifference < voltage_Increase_Threshold)
                    {
                        allQuartersPositiveIncrease = false;
                        break;
                    }
                }
                if (allQuartersPositiveIncrease)
                {             
                    return true;
                }

                return false;
            }
            catch(Exception ex)
            {
                return false;
            }
        }

<<<<<<< HEAD
=======


>>>>>>> 499d608513e1843495adb00cd694c16041388782
        #endregion  Result

        #region Login  
        Login login;
        public void LoginPage()
        {
            login = new Login(_viewModel);
            login.Show();
        }


        public void UpdateUserLoginStatus()
        {
            try
            {
                foreach (var userInfo in _viewModel.LoginInfo)
                {
                    if (userInfo.User_Enable && userInfo.User_Level == "MASTER")
                    {
                        _viewModel.Login = Enum_Login.MASTER;
                        break;
                    }
                    if (userInfo.User_Enable && userInfo.User_Level == "ENGINEER")
                    {
                        _viewModel.Login = Enum_Login.ENGINEER;
                        break;
                    }
                    if (userInfo.User_Enable && userInfo.User_Level == "OPERATOR")
                    {
                        _viewModel.Login = Enum_Login.OPERATOR;
                        break;
                    }

                }

                _viewModel.MainWindow_ButtonFlag = Enum_MainWindow_ButtonFlag.Home;
            }
            catch (Exception ex)
            {

            }
        }

        public void UpdateLogin()
        {
            try
            {
                string id = _viewModel.LoginID;
                string password = "";
                _viewModel.Password = login.ValidateAndAssignUser(_viewModel.LoginID);

                foreach (DatabaseManager_Login login in _viewModel.LoginInfo)
                {
                    if (login._User_Id == id)
                    {
                        password = login.User_Password;
                        break;
                    }
                }

                if (_viewModel.Password == password)
                {

                }
                else
                {
                    System.Windows.MessageBox.Show("암호가 일치하지 않습니다.");
                    return;
                }


                string updateQuery = "UPDATE Login SET User_Enable = @User_Enable WHERE User_Id = @User_Id";
                Dictionary<string, object> parameters = new Dictionary<string, object>
                    {
                        { "@User_Enable", true },
                        { "@User_Id", id }
                    };
                _viewModel.databaseManagercs[(int)Enum_DatabaseManager.Common].UpdateLogin(updateQuery, parameters);

                string updateRestQuery = "UPDATE Login SET User_Enable = @User_Enable WHERE User_Id <> @User_Id";
                Dictionary<string, object> restParameters = new Dictionary<string, object>
                    {
                        { "@User_Enable", false },
                        { "@User_Id", id }
                    };
                _viewModel.databaseManagercs[(int)Enum_DatabaseManager.Common].UpdateLogin(updateRestQuery, restParameters);

                var select_LoginInfo = _viewModel.databaseManagercs[(int)Enum_DatabaseManager.Common].Select_LoginInfo();
                _viewModel.LoginInfo = new List<DatabaseManager_Login>(select_LoginInfo);

            }
            catch (Exception ex)
            {

            }
        }

        public void LogoutPage()
        {
            _viewModel.Login = Enum_Login.OPERATOR;


            string updateRestQuery = "UPDATE Login SET User_Enable = @User_Enable";
            Dictionary<string, object> restParameters = new Dictionary<string, object>
                {
                    { "@User_Enable", false }
                };
            _viewModel.databaseManagercs[(int)Enum_DatabaseManager.Common].UpdateLogin(updateRestQuery, restParameters);

            var select_LoginInfo = _viewModel.databaseManagercs[(int)Enum_DatabaseManager.Common].Select_LoginInfo();
            _viewModel.LoginInfo = new List<DatabaseManager_Login>(select_LoginInfo);
            _viewModel.MainWindow_ButtonFlag = Enum_MainWindow_ButtonFlag.Home;
        }

        public void Login_Add()
        {
            try
            {
                string id = _viewModel.LoginID;
                Enum_Login level = _viewModel.Login;
                if (level != Enum_Login.MASTER)
                {
                    System.Windows.MessageBox.Show("게정을 생성 할려면 MASTER ID로 로그인 해야 합니다..");
                    return;
                }
                string NEWID = login.NEWID_ADD.Text.Trim(); ;
                string PASSWORD = login.PASSWORD_ADD.Password.Trim();
                List<DatabaseManager_Login> list = new List<DatabaseManager_Login>();
                list.Add(new DatabaseManager_Login {  User_Id = NEWID,  User_Level = _viewModel.Login_LevelC,  User_Password = PASSWORD });
                _viewModel.databaseManagercs[(int)Enum_DatabaseManager.Common].InsertLogin(list);
                var select_LoginInfo = _viewModel.databaseManagercs[(int)Enum_DatabaseManager.Common].Select_LoginInfo();
                _viewModel.LoginInfo = new List<DatabaseManager_Login>(select_LoginInfo);
            }
            catch(Exception ex)
            {

            }
        }

        public void Login_Delete()
        {
            try
            {
                string id = _viewModel.LoginID;
                Enum_Login level = _viewModel.Login;
                if (id == "MASTER")
                {
                    System.Windows.MessageBox.Show("MASTER ID는 삭제할 수 없습니다.");
                    return;
                }
                if (level !=  Enum_Login.MASTER)
                {
                    System.Windows.MessageBox.Show("게정을 삭제 할려면 MASTER ID로 로그인 해야 합니다..");
                    return;
                }
                _viewModel.databaseManagercs[(int)Enum_DatabaseManager.Common].Delete_Login(id);
                var select_LoginInfo = _viewModel.databaseManagercs[(int)Enum_DatabaseManager.Common].Select_LoginInfo();
                _viewModel.LoginInfo = new List<DatabaseManager_Login>(select_LoginInfo);
            }
            catch (Exception ex)
            {

            }
        }

        #endregion Login

        #region Loading
        public void Loading_formLoadingProgressset()
        {
            if (_viewModel.Loading_formLoadingProgress < 100)
            {
                _viewModel.Loading_formLoadingProgress++;
            }
            _viewModel.Loading_formLoadingProgress2++;
        }
        #endregion Loading

        #region Report
        public void ReportSerach()
        {
            try
            {
                var select_Equipment = _viewModel.databaseManagercs[(int)Enum_DatabaseManager.MainWindow_select_Equipment_Search].Select_Barcode(_viewModel.MyDatePicke_Start, _viewModel.MyDatePicke_End, _viewModel.Report_Find_Barcode);
                _viewModel.Report_List = select_Equipment;
                _viewModel.Report_Find_Barcode = "";
                DateTime date = _viewModel.MyDatePicke_Start;
                DateTime date2 = _viewModel.MyDatePicke_End;
    
            }
            catch (Exception ex)
            {

            }
        }
        public void CSV()
        {
            try
            {
                if (_viewModel.Report_Barcode.Length > 0)
                {
                    var select_Equipment = _viewModel.databaseManagercs[(int)Enum_DatabaseManager.MainWindow_select_Equipment_Search].QueryDataForBarcodeAndDateRange(_viewModel.Report_Barcode, _viewModel.Report_DatePicke_Start, _viewModel.Report_DatePicke_End);
                    var select_barcode = _viewModel.databaseManagercs[(int)Enum_DatabaseManager.MainWindow_select_Equipment_Search].Select_Barcode(_viewModel.Report_Barcode);
                    _viewModel.Report_Result = select_barcode[0].Result;
                    if (select_barcode[0].PositiveTime.HasValue)
                    {
                        _viewModel.Report_Positive_Time = select_barcode[0].PositiveTime.Value.ToString("yyyy-MM-dd HH:mm");
                    }
                    else
                    {
                        _viewModel.Report_Positive_Time = string.Empty; // 또는 기본값을 설정
                    }


                    if (_viewModel.Report_Model == Enum_Report_Model.raw)
                    {
           
                        _viewModel.CSV_List = select_Equipment;

                    }
                    else if (_viewModel.Report_Model == Enum_Report_Model.SMA)
                    {
                        int windowSize = _viewModel.Report_AverageValue;
                        _viewModel.CSV_List = SmoothPcbValues(select_Equipment, windowSize);
                    }
                    else
                    {

                    }
                    //Report_PositiveBounddury();
                    Microsoft.Win32.SaveFileDialog saveFileDialog = new Microsoft.Win32.SaveFileDialog
                    {
                        Filter = "CSV file (*.csv)|*.csv",
                        DefaultExt = "csv",
                        AddExtension = true
                    };

                    if (saveFileDialog.ShowDialog() == true)
                    {


                        var sb = new StringBuilder();

                        sb.AppendLine($"Result  : ,  {_viewModel.Report_Result}, Event Time  :   , {_viewModel.Report_Positive_Time}");
                        sb.AppendLine(); // 첫 번째 공백 줄 추가
                        sb.AppendLine(); // 두 번째 공백 줄 추가
                        sb.AppendLine("Cell, Barcode ID, Patient ID,  PCB Value, Temp Value, Creation Date");

                        foreach (var item in _viewModel.CSV_List)
                        {

                            sb.AppendLine($"{item.ID},{item.Barcode},{item.Qrcode},{item.PcbADC},{item.Temperature}," +
                           $"{item.CreDate.ToString("yyyy-MM-dd HH:mm:ss")},");


                        }

                        // Write the CSV data to file
                        File.WriteAllText(saveFileDialog.FileName, sb.ToString());


                    }

                }



            }
            catch (Exception ex)
            {

            }
        }
        public List<DatabaseManager_EquipmentH> SmoothPcbValues(List<DatabaseManager_EquipmentH> rawData, int windowSize)
        {
            List<DatabaseManager_EquipmentH> smoothedData = new List<DatabaseManager_EquipmentH>();

            for (int i = 0; i < rawData.Count; i++)
            {
                double sum = 0;
                int actualWindowSize = 0;
                for (int j = i - windowSize / 2; j <= i + windowSize / 2; j++)
                {
                    if (j >= 0 && j < rawData.Count)
                    {
                        sum += rawData[j].PcbADC;
                        actualWindowSize++;
                    }
                }
                double average = sum / actualWindowSize;
                average = Math.Round(average, 0);
                DatabaseManager_EquipmentH smoothedPoint = new DatabaseManager_EquipmentH()
                {
                    Barcode = rawData[i].Barcode,
                    Qrcode = rawData[i].Qrcode,
                    PcbADC = average,
                    Temperature = rawData[i].Temperature, // You might want to smooth this as well if needed
                    CreDate = rawData[i].CreDate,
                    ID = rawData[i].ID
                };
                smoothedData.Add(smoothedPoint);
            }
            return smoothedData;
        }

        public void Delete()
        {
            try
            {
                MessageBoxResult result = System.Windows.MessageBox.Show(this,
           "Are you sure you want to delete?",
           "Delete Confirmation",
           MessageBoxButton.YesNo,
           MessageBoxImage.Question);

                if (result == MessageBoxResult.No)
                {
                    return;
                }

                string BacodeID = _viewModel.Report_Barcode;



                DateTime start = _viewModel.MyDatePicke_Start;
                DateTime end = _viewModel.MyDatePicke_End;

                var select_Equipment = _viewModel.databaseManagercs[(int)Enum_DatabaseManager.MainWindow_select_Equipment_Search].Select_Barcode( _viewModel.Report_Barcode);
                if (select_Equipment.Count > 0 && select_Equipment[0].Unloading != null)
                {
                    _viewModel.databaseManagercs[(int)Enum_DatabaseManager.MainWindow_select_Equipment_Search].DeleteRecords(BacodeID, start, end);

                    ReportSerach();

                }
                else
                {
                    System.Windows.MessageBox.Show("배양이 끝나지 않았습니다. 배양을 정상적으로 종료하십시오.");
                }

            }
            catch (Exception e)
            {

            }
        }
        #endregion Report

        #region Config
        public void DataStorageSave_timer_Start()
        {
            try
            {
                int Interval = _viewModel.Config[0].DataStorageSave;
                DataStorageSave_timer.Interval = TimeSpan.FromMinutes(Interval);
                DataStorageSave_timer.Tick += TimerCallback_DataStorageSave_timer;
                DataStorageSave_timer.Start();
            }
            catch (Exception ex)
            {

            }
        }

        public void DataStorageSave_timer_Stop()
        {
            if (DataStorageSave_timer.IsEnabled)
            {
                DataStorageSave_timer.Tick -= TimerCallback_DataStorageSave_timer;
                DataStorageSave_timer.Stop();
            }
        }
        public void TimerCallback_DataStorageSave_timer(object sender, EventArgs e)
        {
            try
            {
                if(!_viewModel.Result_Timer)
                {
                    _viewModel.Result_Timer = true;

                }
  
            }
            catch (Exception ex)
            {

            }
        }

        public void Blcok_OK(int idx)
        {
            try
            {
                int index = idx;
                if (_viewModel.EquipmentInfo[index].isEnable == true)
                {
                    return;
                }
                string UpdateEquipment_Query = "UPDATE Equipment SET " +
                                            "Barcode = @Barcode, Qrcode = @Qrcode, Loading = @Loading, CreDate = @CreDate, PositiveTime = @PositiveTime, Result = @Result, IncubationTime = @IncubationTime, switched = @switched, isEnable = @isEnable, isActive = @isActive " +
                                            "WHERE ID = @ID";
                Dictionary<string, object> UpdateEquipment_parameters = new Dictionary<string, object>
                        {
                            { "@Barcode", null },
                            { "@Qrcode", null },
                            { "@Loading", null },
                            { "@CreDate", null },
                            { "@PositiveTime", null },
                            { "@Result", "Block" },
                            { "@IncubationTime", null },
                            { "@switched", false },
                            { "@isEnable", false },
                            { "@isActive", false },
                            { "@ID", index }  // 
                        };
                _viewModel.databaseManagercs[(int)Enum_DatabaseManager.MainWindow_UpdateConfig].UpdateEquipment(UpdateEquipment_Query, UpdateEquipment_parameters);
            }
            catch(Exception e)
            {

            }
        }

        public void Blcok_Cancel(int idx)
        {
            try
            {
                int index = idx;
                if (_viewModel.EquipmentInfo[index].isEnable == true)
                {
                    return;
                }
                string UpdateEquipment_Query = "UPDATE Equipment SET " +
                                            "Barcode = @Barcode, Qrcode = @Qrcode, Loading = @Loading, CreDate = @CreDate, PositiveTime = @PositiveTime, Result = @Result, IncubationTime = @IncubationTime, switched = @switched, isEnable = @isEnable, isActive = @isActive " +
                                            "WHERE ID = @ID";
                Dictionary<string, object> UpdateEquipment_parameters = new Dictionary<string, object>
                        {
                            { "@Barcode", null },
                            { "@Qrcode", null },
                            { "@Loading", null },
                            { "@CreDate", null },
                            { "@PositiveTime", null },
                            { "@Result", "Null" },
                            { "@IncubationTime", null },
                            { "@switched", false },
                            { "@isEnable", false },
                            { "@isActive", true },
                            { "@ID", index }  // 
                        };
                _viewModel.databaseManagercs[(int)Enum_DatabaseManager.MainWindow_UpdateConfig].UpdateEquipment(UpdateEquipment_Query, UpdateEquipment_parameters);
            }
            catch (Exception e)
            {

            }
        }
        Alarm_Equipment Equipment;
        public void Equipment_Change()
        {
            try
            {
                if (Equipment != null && Equipment.IsVisible)
                {
                    return; // 팝업이 열려 있으면 아무것도 하지 않음
                }

                Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() =>
                {
                    Equipment = new Alarm_Equipment(_viewModel);

                    if (Application.Current.MainWindow != null && Application.Current.MainWindow != Equipment)
                    {
                        Equipment.Owner = Application.Current.MainWindow;
                    }

                    Equipment.Topmost = true;
                    Equipment.Show();
                    Equipment.Activate();

                }));
            }
            catch (Exception e)
            {
            }
        }


        #endregion Config

        #region LiveCharts
        public void LiveCharts()
        {
            try
            {
                if (_viewModel.Report_Barcode.Length > 0)
                {
                    _viewModel.LiveCharts_Result = "";
                    _viewModel.LiveCharts_Positive_Time = "";
                    var select_Equipment = _viewModel.databaseManagercs[(int)Enum_DatabaseManager.MainWindow_select_Equipment_Search].QueryDataForBarcodeAndDateRange(_viewModel.Report_Barcode, _viewModel.Report_DatePicke_Start, _viewModel.Report_DatePicke_End);

                    if (_viewModel.Report_Model == Enum_Report_Model.raw)
                    {
                        var select_barcode = _viewModel.databaseManagercs[(int)Enum_DatabaseManager.MainWindow_select_Equipment_Search].Select_Barcode(_viewModel.Report_Barcode);

                        _viewModel.LiveCharts_List = select_Equipment;
                        _viewModel.LiveCharts_Result = select_barcode[0].Result;
                        if (select_barcode[0].PositiveTime.HasValue)
                        {
                            _viewModel.LiveCharts_Positive_Time = select_barcode[0].PositiveTime.Value.ToString("yyyy-MM-dd HH:mm");
                        }
                        else
                        {
                            _viewModel.LiveCharts_Positive_Time = string.Empty; // 또는 기본값을 설정
                        }


                    }
                    else if (_viewModel.Report_Model == Enum_Report_Model.SMA)
                    {
                        _viewModel.LiveCharts_List = SmoothPcbValues(select_Equipment, _viewModel.Report_AverageValue);
                    }
                    else
                    {

                    }
<<<<<<< HEAD
                    //LiveCharts_PositiveBounddury();
=======

>>>>>>> 499d608513e1843495adb00cd694c16041388782

                    LiveCharts liveCharts = new LiveCharts(_viewModel);
                    liveCharts.ShowDialog();

                }

            }
            catch (Exception ex)
            {

            }
        }

        public void LiveCharts_PositiveBounddury()
        {
            try
            {
                _viewModel.LiveCharts_Positive_Start = 0;
                _viewModel.LiveCharts_Positive_End = 0;
                int Analysis_Time_Range = _viewModel.Config[0].Analysis_Time_Range;
                int Number_of_Analysis_Intervals = _viewModel.Config[0].Analysis_Intervals;

                double Voltage_Increase_Threshold = _viewModel.Config[0].Threshold;


                _viewModel.LiveCharts_TimeSeries = new Dictionary<int, Queue<(DateTime, double)>>();
<<<<<<< HEAD

=======
  
>>>>>>> 499d608513e1843495adb00cd694c16041388782
                if (_viewModel.LiveCharts_TimeSeries.ContainsKey(0))
                {
                    _viewModel.LiveCharts_TimeSeries[0].Clear();
                }
<<<<<<< HEAD
 
=======
   
>>>>>>> 499d608513e1843495adb00cd694c16041388782
                DateTime startTime = _viewModel.LiveCharts_List[0].CreDate.AddMinutes(60);
                for (int i = 0; i < _viewModel.LiveCharts_List.Count; i++)
                {
                    int pcbIndex = 0; // 예시를 위한 PCB 인덱스, 실제 인덱스 사용 필요

                    DateTime timestamp = _viewModel.LiveCharts_List[i].CreDate;
                    if (timestamp < startTime)
                    {
                        continue; // 처음 100분 동안의 데이터는 스킵
                    }

                    double pcbValue = _viewModel.LiveCharts_List[i].PcbADC; // PCB 전압값

                    // 해당 인덱스의 큐가 없으면 생성
                    //if (!voltageTimeSeries.ContainsKey(pcbIndex))
                    //{
                    //    voltageTimeSeries[pcbIndex] = new Queue<(DateTime, double)>();
                    //}

                    // 시계열 데이터에 시간과 전압값의 쌍 추가
                    if (i == 549)
                    {

                    }
                    bool shouldStop = LiveCharts_VoltageValue(0, pcbValue, timestamp, Analysis_Time_Range, Number_of_Analysis_Intervals, Voltage_Increase_Threshold);
<<<<<<< HEAD

=======
      
>>>>>>> 499d608513e1843495adb00cd694c16041388782

                    if (shouldStop )
                    {
                        break; // for 루프 중단
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        private bool LiveCharts_VoltageValue(int pcbIndex, double currentVoltage, DateTime TIME, int Analysis_Time_Range, int Number_of_Analysis_Intervals, double Voltage_Increase_Threshold)
        {
            try
            {
                int analysis_Time_Range = Analysis_Time_Range;
                int number_of_Analysis_Intervals = Number_of_Analysis_Intervals;
                double voltage_Increase_Threshold = Voltage_Increase_Threshold;
                double percentageDifference = 0;


                DateTime currentTime = TIME;

                if (!_viewModel.LiveCharts_TimeSeries.ContainsKey(pcbIndex))
                {
                    _viewModel.LiveCharts_TimeSeries[pcbIndex] = new Queue<(DateTime, double)>();
                }

                // Rolling window update
                Queue<(DateTime, double)> window = _viewModel.LiveCharts_TimeSeries[pcbIndex];
                window.Enqueue((currentTime, currentVoltage));

                // Maintain window size
                while (window.Count > 0 && (currentTime - window.Peek().Item1).TotalMinutes > analysis_Time_Range)
                {
                    window.Dequeue();
                }

                var data = _viewModel.LiveCharts_TimeSeries[pcbIndex];
                int rage = (int)(analysis_Time_Range * 0.99);
                if (data.Count < rage)
                {
                    return false;
                }

                int quarterSize = analysis_Time_Range / number_of_Analysis_Intervals;
                List<double> quarterAverages = new List<double>();

                for (int i = 0; i < number_of_Analysis_Intervals; i++)
                {
                    List<double> quarterData = data.Skip(i * quarterSize).Take(quarterSize).Select(d => d.Item2).ToList();
                    double quarterAverage = quarterData.Average();
                    quarterAverages.Add(quarterAverage);
                }


                bool allQuartersPositiveIncrease = true;
        
                for (int i = 1; i < number_of_Analysis_Intervals; i++)
                {
                    percentageDifference = ((quarterAverages[i] - quarterAverages[i - 1]) / quarterAverages[i - 1]) * 100.0;

                    if (percentageDifference < voltage_Increase_Threshold)
                    {
                        allQuartersPositiveIncrease = false;
                        break;
                    }
                }

                if (allQuartersPositiveIncrease)
                {
<<<<<<< HEAD
=======
                    for (int i = 0; i < _viewModel.LiveCharts_List.Count; i++)
                    {
                        if (_viewModel.LiveCharts_List[i].CreDate == currentTime)
                        {
                            _viewModel.LiveCharts_ColorPaint = new SolidColorPaint
                            {
                                Color = SKColors.Red,
                                StrokeThickness = 2,
                                PathEffect = new DashEffect(new float[] { 6, 6 })
                            };
                            _viewModel.LiveCharts_Positive_Start = i;
                            _viewModel.LiveCharts_Positive_End = i + Math.Max(1, _viewModel.LiveCharts_List.Count / 200);
                            if (_viewModel.Report_Model == Enum_Report_Model.SMA)
                            {
                                _viewModel.LiveCharts_Result = "Positive";
                                _viewModel.LiveCharts_Positive_Time = _viewModel.LiveCharts_List[i].CreDate.ToString();
                            }
                            break;
                        }
                    }
>>>>>>> 499d608513e1843495adb00cd694c16041388782
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

<<<<<<< HEAD

=======
      
>>>>>>> 499d608513e1843495adb00cd694c16041388782
        #endregion LiveCharts

        #region PCB
        public void PCBInitialize()
        {
            try
            {
                PCBData();
                PCBFrame();
                _viewModel.PCB_Connection = PCB_SerialPortOpen(_viewModel.SystemInfo[0].PCB_Serial);
     
            }
            catch (Exception ex)
            {

            }
        }

        public void PCBData()
        {
            try
            {
                var pcb = new List<PCB>();
                var cell_alive = new List<PCB_cell_alive_C>();
                var DataWithDB_presenceArray = new List<MatchEquipmentDataWithDB_C>();
                for (int i = 0; i < _viewModel.Common_TotalSystemCellCount; i++)
                {
                    pcb.Add(new PCB { ADC = 0, LED = 0 }); // ADC와 LED 값을 초기화
                    cell_alive.Add(new PCB_cell_alive_C { alive = 0 }); // ADC와 LED 값을 초기화
                    DataWithDB_presenceArray.Add(new MatchEquipmentDataWithDB_C { alive = false });
                    _viewModel.PositiveDelay[i] = 0;
                }


                for (int system = 0; system < 1; system++)
                {
                    _viewModel.PCB_CellReadings[system] = new Dictionary<int, List<List<double>>>();

                    for (int line = 0; line < 3; line++)
                    {
                        _viewModel.PCB_CellReadings[system][line] = new List<List<double>>();
                        for (int cell = 0; cell < 28; cell++)
                        {
                            _viewModel.PCB_CellReadings[system][line].Add(new List<double>());
                        }
                    }
                }

                _viewModel.PCB_Data = pcb;
                _viewModel.PCB_cell_alive = cell_alive;
                _viewModel.Equipment_DataWithDB_presenceArray = DataWithDB_presenceArray;
            }
            catch (Exception ex)
            {

            }
        }

        public void PCBFrame()
        {
            bool[] ledidx = new bool[84];
            string id = _viewModel.SystemInfo[0].PCB_ID1;
            for (int i = 0; i < 3; i++)
            {
                string LAMP = $"{id},LINE{i},LAMP,CHALL,ON";
                _viewModel.Queue_PCB_Manual.Enqueue(LAMP);
                Thread.Sleep(100);
            }

            Thread.Sleep(2000);

            for (int i = 0; i < 3; i++)
            {

                string ACD = $"{id},LINE{i},ADCREAD";
                _viewModel.Queue_PCB_Manual.Enqueue(ACD);
            }

            for (int i = 0; i < 3; i++)
            {
                //string LED = $"{id},LINE{i},TLED,ALL,ON,0,255,0";
                string LED = $"{id},LINE{i},TLED,DIM,ALL,200";
                _viewModel.Queue_PCB_Manual.Enqueue(LED);

                string DIM = $"{id},LINE{i},DIMREAD";
                _viewModel.Queue_PCB_Manual.Enqueue(DIM);
            }

            var filteredItems = _viewModel.EquipmentInfo.Where(e => e.isActive && e.isEnable).ToList();
            foreach (var item in filteredItems)
            {

                int index = item.ID - 1;
                string result = item.Result;
                PCB_LED(id, index, result);
                ledidx[index] = true;
            }

            for (int i = 0; i < _viewModel.Common_SystemCellCount; i++)
            {
                if (ledidx[i] == false)
                {
                    PCB_LED(id, i, "Null");
                }
            }
        }



        public bool PCB_SerialPortOpen(string portName)
        {
            try
            {
                _viewModel.PCB_SerialPort = new SerialPort(portName)
                {
                    BaudRate = 115200,
                    DataBits = 8,
                    Parity = Parity.None,
                    StopBits = StopBits.One,
                };
                _viewModel.PCB_SerialPort.Open();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }
        public void PCB_SerialPortClose()
        {
            try
            {
                if (_viewModel.PCB_SerialPort.IsOpen)
                {
                    _viewModel.PCB_SerialPort.Close();
                }
            }
            catch (Exception ex)
            {

            }
        }

   
        public void PCB_LAMP_ON()
        {
            try
            {
                string id = _viewModel.SystemInfo[0].PCB_ID1;
                for (int i = 0; i < 3; i++)
                {
                    string LAMP = $"{id},LINE{i},LAMP,CHALL,ON";
                    _viewModel.Queue_PCB_Manual.Enqueue(LAMP);
                }
            }
            catch (Exception ex)
            {

            }
        }



        private void PCB_PerformDataAcquisition_Manual()
        {

            try
            {
                if (!_viewModel.PCB_SerialPort.IsOpen && !_viewModel.PCB_Connection)
                {
                    return;
                }

                if (_viewModel.Queue_PCB_Manual.Count > 0 )
                {
                    //_viewModel.PCB_Status = false;
                    if (_viewModel.Queue_PCB_Manual.TryDequeue(out string str))
                    {
                        int lineIndex = ExtractLineIndex(str);
                        string line = str;
                        string response = ReadSerialPortResponse(line, true);

                        if (response != null)
                        {
                            PCB_Manual_Data(response, lineIndex);
                        }
                    }
                }
                else
                {
                    if (_viewModel.Calibration_Falg || !_viewModel.FASTECH_IO_Connection )
                    {
                        return;
                    }
                    for (int i = 0; i < 3; i++)
                    {
                        string id = _viewModel.SystemInfo[0].PCB_ID1;
                        string line = $"{id},LINE{i},ADCREAD";
                        string response = ReadSerialPortResponse(line, false);
                        if (response != null)
                        {
                            PCB_Auto_Data(response.ToString());
                        }
                    }
                    if(_viewModel.PCB_Status == false)
                    {
                        _viewModel.PCB_Status = true;

                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void PCB_Auto_Data(string response)
        {
            var dataParts = response.Split(',');
            //var dataParts = dataPartsf.Where(part => !part.Contains('?')).ToArray();
            ProcessDataParts(dataParts);
        }
        private void ProcessDataParts(string[] dataParts)
        {
            try
            {
                if (dataParts.Length != 31)
                {
                    return;
                }
                int lineIndex = ExtractLineIndex2(dataParts[1]);
                int startIndex = 3;
                for (int i = 0; i < 28; i++)
                {
                    if (double.TryParse(dataParts[i + startIndex], out double cellValue))

                    {
                        _viewModel.PCB_CellReadings[0][lineIndex][i].Add(cellValue);

                        if (_viewModel.PCB_CellReadings[0][lineIndex][i].Count == 5)
                        {
                            _viewModel.PCB_CellReadings[0][lineIndex][i].Sort();
                            double averageValue = _viewModel.PCB_CellReadings[0][lineIndex][i].Skip(1).Take(3).Average();

                            int pcbIndex = lineIndex * 28 + i;
                            if (pcbIndex < _viewModel.PCB_Data.Count)
                            {
                                _viewModel.PCB_Data[pcbIndex].ADC = (int)(averageValue * 1000);
                            }
                            _viewModel.PCB_CellReadings[0][lineIndex][i].Clear();
                        }
                    }
                }

            }
            catch (Exception ex)
            {
            }          
        }


      
        private void PCB_Manual_Data(string response, int lineIndex)
        {
            try
            {
                var dataPartsf = response.Split(',');
                var dataParts = dataPartsf.Where(part => !part.Contains('?')).ToArray();
                int startIndex = lineIndex * 28;

                var values = dataParts.Skip(3).Take(28).Select(val => double.TryParse(val, out double dVal) ? dVal : 0).ToList();
                for (int i = 0; i < values.Count; i++)
                {
                    int pcbIndex = startIndex + i;
                    if (pcbIndex < _viewModel.PCB_Data.Count)
                    {
                        if (dataParts[2] == "ADCREAD")
                        {
                            int reverseIndex = values.Count - 1 - i;
                            _viewModel.PCB_Data[pcbIndex].ADC = values[i] * 1000;
                            //Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() =>
                            //{
                            //    _viewModel.PCB_Data[pcbIndex].ADC = values[i] * 1000;
                            //}));

                        }
                        else if (dataParts[2] == "DIMREAD")
                        {
                            //int reverseIndex = values.Count - 1 - i;
                            int reverseIndex = i;
                            _viewModel.PCB_Data[pcbIndex].LED = values[reverseIndex];
                        }
                    }
                }
            }
            catch(Exception e) 
            { 

            }      
        }


        private string ReadSerialPortResponse(string line, bool type)
        {
            const int maxRetries = 1;
            const int readTimeout = 500;
            const int sleepInterval = 20;
            bool Type = type;

            try
            {            
                string frame = line + "\r\n";
                _viewModel.PCB_SerialPort.Write(frame);

                var response = new StringBuilder();
                var stopwatch = Stopwatch.StartNew();
                while (stopwatch.ElapsedMilliseconds < readTimeout)
                {
                    if (_viewModel.PCB_SerialPort.BytesToRead > 0)
                    {
                        Thread.Sleep(sleepInterval);
                        string data = _viewModel.PCB_SerialPort.ReadExisting();
                        int lastIndex = data.LastIndexOf("$");
                        if (lastIndex != -1)
                        {
                            int endIndex = data.IndexOf("\r\n", lastIndex);
                            if (endIndex != -1 && data.Length > 50)
                            {
                                string tempData = data.Substring(lastIndex, endIndex - lastIndex);
                                ClearSerialPortBuffer(_viewModel.PCB_SerialPort);
                                return response.Append(tempData).ToString();
                            }
                            else
                            {
                                Thread.Sleep(100);
                                ClearSerialPortBuffer(_viewModel.PCB_SerialPort);
                                return null;
                            }
                        }
                        else
                        {
                            ClearSerialPortBuffer(_viewModel.PCB_SerialPort);
                            return null;
                        }
                    }
                    Thread.Sleep(sleepInterval);
                }
                ClearSerialPortBuffer(_viewModel.PCB_SerialPort);
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        private void ClearSerialPortBuffer(SerialPort serialPort)
        {
            if (serialPort.IsOpen)
            {
                serialPort.DiscardInBuffer();
                serialPort.DiscardOutBuffer();
            }
        }


        private void ClearSerialPortBuffer_PCB(SerialPort port)
        {
            try
            {
                if (port.IsOpen)
                {
                    Thread.Sleep(20);
                    port.DiscardInBuffer();
                    port.DiscardOutBuffer();
                }
            }
            catch (Exception ex)
            {
                // 예외 처리 로직 추가 (로그 기록 등)
            }
        }


        private int ExtractLineIndex(string response)
        {
            var lineNumberStart = response.IndexOf("LINE") + "LINE".Length;
            var lineNumberEnd = response.IndexOf(",", lineNumberStart);
            var lineNumberStr = response.Substring(lineNumberStart, lineNumberEnd - lineNumberStart);
            if (int.TryParse(lineNumberStr, out int lineIndex))
            {
                return lineIndex;
            }

            return -1;
        }

        private int ExtractLineIndex2(string response)
        {
            try
            {
                var lineNumberStart = response.IndexOf("LINE") + "LINE".Length;
                var lineNumberEnd = response.Length;

                var lineNumberStr = response.Substring(lineNumberStart, lineNumberEnd - lineNumberStart);

                if (int.TryParse(lineNumberStr, out int lineIndex))
                {
                    return lineIndex;
                }

                return -1;
            }
            catch (Exception ex)
            {
                return -1;
            }
        }




        public void PositiveDelay()
        {
            try
            {
                var filteredItems = _viewModel.EquipmentInfo.Where(e => e.isActive && e.isEnable && e.Switched).ToList();
                int Positive_Low = _viewModel.Config[0].Positive_Low;
                int Positive_High = _viewModel.Config[0].Positive_High;

                foreach (var item in filteredItems)
                {
                    double adc = _viewModel.PCB_Data[item.ID - 1].ADC;
                    if (adc >= Positive_Low && adc <= Positive_High)
                    {
                        _viewModel.PositiveDelay[item.ID - 1] += 1;
                    }
                }
        
            }
            catch(Exception ex)
            {

            }
        }
        #endregion PCB

        #region System
        public void SystemInitialize()
        {
            try
            {
                System1ini();
            }
            catch (Exception ex)
            {

            }
        }
        #region System1
        public void System1ini()
        {
            var System_PositiveFirst = new List<PositiveFirst_C>();
            for (int i = 0; i < _viewModel.Common_TotalSystemCellCount; i++)
            {
                System_PositiveFirst.Add(new PositiveFirst_C { alive = false });
            }
            _viewModel.System_PositiveFirst = System_PositiveFirst;
        }
        #endregion System1

        #region System2
        #endregion System2

        #region System3
        #endregion System3

        #region System3
        #endregion System3
        #endregion System

        #region Barcode
        public void BarcodeInitialize()
        {
            try
            {
                _viewModel.Barcode_Connection = Barcode_SerialPortOpen(_viewModel.SystemInfo[0].Barcode_Serial);
                if (_viewModel.Barcode_Connection)
                {
                    _viewModel.Barcode_SerialPort.Open();
                }
            }
            catch (Exception ex)
            {

            }
        }
        public bool Barcode_SerialPortOpen(string portName)
        {
            try
            {
                _viewModel.Barcode_SerialPort = new SerialPort(portName)
                {
                    BaudRate = 9600,
                    DataBits = 8,
                    Parity = Parity.None,
                    StopBits = StopBits.One,
                    RtsEnable = true,
                    DtrEnable = true
                };
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public void Barcode_SerialPortClose()
        {
            try
            {
                if (_viewModel.Barcode_SerialPort.IsOpen)
                {
                    _viewModel.Barcode_SerialPort.Close();
                }
            }
            catch (Exception ex)
            {

            }
        }
        public async Task Barcode_WriteAsync()
        {
            ClearSerialPortBuffer_Barcode(_viewModel.Barcode_SerialPort);
            await Barcode_ReadAsync(2000);

        }
        public async Task Barcode_ReadAsync(double timeout)
        {
            try
            {
                if (!_viewModel.Barcode_SerialPort.IsOpen)
                    return;

                _viewModel.Barcode_SerialPort.DiscardInBuffer();
                _viewModel.Barcode_SerialPort.DiscardOutBuffer();
                var stopwatch = Stopwatch.StartNew();
                while (stopwatch.ElapsedMilliseconds < timeout)
                {
                    if (_viewModel.Barcode_SerialPort.BytesToRead > 3)
                    {
                        Thread.Sleep(50);
                        byte[] response = new byte[_viewModel.Barcode_SerialPort.BytesToRead];

                        _viewModel.Barcode_SerialPort.Read(response, 0, response.Length);
                        string asciiResponse = Encoding.ASCII.GetString(response);
                        string[] barcodes = asciiResponse.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                
                        foreach (var barcode in barcodes)
                        {
                         
                            if (barcode.Length > 4) 
                            {
                                if(_viewModel.Equipment_DataWithDB_presenceArray.Any(x => x.alive))
                                {
                        
                                    _viewModel.Barcode_ID = "";
                                    _viewModel.Patient_ID = "";
                                    _viewModel.Barcode_ID_Loading = "";
                                    _viewModel.Patient_ID_Loading = "";
                                }
                                else
                                {
                                    if (!searchBarcodeDuplicates(barcode))
                                    {
                                        if (barcode.All(char.IsDigit))
                                        {
                                            _viewModel.Patient_ID = barcode;
                                            _viewModel.Patient_ID_Loading = barcode;
                                        }
                                        else
                                        {
                                            _viewModel.Barcode_ID = barcode;
                                            _viewModel.Barcode_ID_Loading = barcode;
                                        }
                                    }
                                    else
                                    {
                                        _viewModel.Alarm_Barcode.Enqueue(new Tuple<string>(barcode));
                                        _viewModel.Barcode_ID = "";
                                        _viewModel.Patient_ID = "";
                                        _viewModel.Barcode_ID_Loading = "";
                                        _viewModel.Patient_ID_Loading = "";
                                    }
                                    ClearSerialPortBuffer_Barcode(_viewModel.Barcode_SerialPort);

                                }
                              
                                return;
                            }
                        }

                        return;
                    }
                    await Task.Delay(1);
                }
      
            }
            catch (Exception ex)
            {

            }
        }

        private void ClearSerialPortBuffer_Barcode(SerialPort serialPort)
        {
            if (serialPort.IsOpen)
            {
                serialPort.DiscardInBuffer();
                serialPort.DiscardOutBuffer();
            }
        }

        public bool searchBarcodeDuplicates(string barcode)
        {
            try
            {
                string barcode_id = barcode;
                bool result = _viewModel.databaseManagercs[(int)Enum_DatabaseManager.MainWindow_searchBarcodeDuplicates].Select_Barcode_Search(barcode_id);
                return result;
            }
            catch (Exception ex)
            {
                return false;

            }
        }


        private void TimerCallback_Barcode(object sender, EventArgs e)
        {
            // 타이머 콜백이 호출될 때 수행할 작업을 여기에 작성합니다.

                if (!_viewModel.Alarm_Barcode_isPopupOpen)
                {
                    if (_viewModel.Alarm_Barcode.Count > 0)
                    {
                        if (_viewModel.Alarm_Barcode.TryDequeue(out Tuple<string> command))
                        {
                            _viewModel.Alarm_Barcode_isPopupOpen = true;
                            string item1 = command.Item1;
                            _viewModel.Barcode_BarcodeID = item1;
                            _viewModel.Barcode_Content = "Bottle  " + _viewModel.Barcode_BarcodeID + "  already exists.";
                            Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() =>
                            {
                                Alarm_Barcode alarm_Barcode = new Alarm_Barcode(_viewModel);
                                alarm_Barcode.ClosedEvent += Alarm_Barcode_Closed;

                                if (Application.Current.MainWindow != null && Application.Current.MainWindow != alarm_Barcode)
                                {
                                    alarm_Barcode.Owner = Application.Current.MainWindow;
                                }

                                alarm_Barcode.Topmost = true;
                                alarm_Barcode.Show();
                                alarm_Barcode.Activate();      
                            }));
                        }
                    }
                }
            
        }

        private void Alarm_Barcode_Closed(object sender, bool? result)
        {
            if (result == true)
            {
                _viewModel.Alarm_Barcode_isPopupOpen = false;
            }
            else
            {

            }
        }

        public void ResetBarcode()
        {
            try
            {
                _viewModel.Barcode_ID = "";
                _viewModel.Patient_ID = "";
                _viewModel.Barcode_ID_Loading = "";
                _viewModel.Patient_ID_Loading = "";
            }
            catch(Exception ex)
            {

            }
        }

        #endregion Barcode

        #region Temperature
        public void TemperatureInitialize()
        {
            try
            {
                _viewModel.Temperature_Connection = Temperature_SerialPortOpen(_viewModel.SystemInfo[0].Temperature_Serial);
                if (_viewModel.Temperature_Connection)
                {
                    _viewModel.Temperature_SerialPort.Open();
                }
            }
            catch (Exception ex)
            {

            }
        }
        public bool Temperature_SerialPortOpen(string portName)
        {
            try
            {
                _viewModel.Temperature_SerialPort = new SerialPort(portName)
                {
                    BaudRate = 19200,
                    DataBits = 8,
                    Parity = Parity.None,
                    StopBits = StopBits.One,
                };
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public void Temperature_SerialPortClose()
        {
            try
            {
                if (_viewModel.Temperature_SerialPort.IsOpen)
                {
                    _viewModel.Temperature_SerialPort.Close();
                }
            }
            catch (Exception ex)
            {

            }
        }

        public async Task Temperature_WriteAsync()
        {

            int sv = (int)(_viewModel.Config[0].Temp * 10);
            byte[] PV_Temp = new byte[8] { 0x01, 0x04, 0x03, 0xE8, 0x00, 0x01, 0xB1, 0xBA };
            //byte[] PV_Temp = { 0x02, 0x04, 0x03, 0xE8, 0x00, 0x01, 0xB1, 0x89 };
            byte[] SV_Temp = GenerateFrame(sv, 1);
           // byte[] AL_Temp = { 0x02, 0x02, 0x00, 0x04, 0x00, 0x01, 0xF8, 0x38 };

            await Temperature_ReadAsync(PV_Temp, SV_Temp, 500, (temp) => _viewModel.Temperature_ProcessValue = temp);
            //await Temperature_ReadAsyncAL(AL_Temp, 500, (AL) => _viewModel.Temperature_AL_Connection = AL);

        }
        public async Task Temperature_ReadAsync(byte[] PV, byte[] SV, double timeout, Action<double> updateTempValue)
        {
            try
            {
                if (!_viewModel.Temperature_SerialPort.IsOpen)
                    throw new InvalidOperationException("Serial port is not open.");

                _viewModel.Temperature_SerialPort.Write(PV, 0, PV.Length);

                var stopwatch = Stopwatch.StartNew();
                while (stopwatch.ElapsedMilliseconds < timeout)
                {
                    if (_viewModel.Temperature_SerialPort.BytesToRead > 5)
                    {
                        Thread.Sleep(50);
                        byte[] response = new byte[_viewModel.Temperature_SerialPort.BytesToRead];
                        _viewModel.Temperature_SerialPort.Read(response, 0, response.Length);

                        if (response.Length >= 7 && response[1] == 4 && response[2] == 2)
                        {
                            int temperatureRaw = (response[3] << 8) | response[4];
                            double temperature = temperatureRaw / 10.0;
                            updateTempValue(temperature);
                            break;
                        }
                    }
                    await Task.Delay(10); 
                }


                _viewModel.Temperature_SerialPort.Write(SV, 0, SV.Length);
                var stopwatch2 = Stopwatch.StartNew();
                while (stopwatch2.ElapsedMilliseconds < timeout)
                {
                    if (_viewModel.Temperature_SerialPort.BytesToRead > 5)
                    {
                        Thread.Sleep(50);
                        byte[] response = new byte[_viewModel.Temperature_SerialPort.BytesToRead];
                        _viewModel.Temperature_SerialPort.Read(response, 0, response.Length);

                        if (response.Length >= 8 && response[0] == 1 && response[1] == 6 && response[2] == 0 && response[3] == 0 && response[4] == 1)
                        {
                            break;
                        }
                    }
                    await Task.Delay(10);  
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        public async Task Temperature_ReadAsyncAL(byte[] Data, double timeout, Action<bool> AL)
        {
            try
            {
                if (!_viewModel.Temperature_SerialPort.IsOpen)
                    throw new InvalidOperationException("Serial port is not open.");

                _viewModel.Temperature_SerialPort.Write(Data, 0, Data.Length);



                var stopwatch = Stopwatch.StartNew();
                while (stopwatch.ElapsedMilliseconds < timeout)
                {
                    if (_viewModel.Temperature_SerialPort.BytesToRead > 1)
                    {
                        Thread.Sleep(50);
                        byte[] response = new byte[_viewModel.Temperature_SerialPort.BytesToRead];
                        _viewModel.Temperature_SerialPort.Read(response, 0, response.Length);

                        if (response.Length == 6 && response[3] == 1 )
                        {
                            _viewModel.Temperature_AL_Connection = true;
                        }
                        else
                        {
                            _viewModel.Temperature_AL_Connection = false;
                        }

                        break;
                    }
                    await Task.Delay(10);
                }
            }
            catch (Exception ex)
            {
            }
        }
        public byte[] GenerateFrame(int temperature, int deviceAddress)
        {
            byte[] frame = new byte[8];
            frame[0] = (byte)deviceAddress;
            frame[1] = 0x06;
            frame[2] = 0x00;
            frame[3] = 0x00;


            frame[4] = (byte)((temperature >> 8) & 0xFF);
            frame[5] = (byte)(temperature & 0xFF);


            ushort crc = CalculateCrc16(frame, 6);
            frame[6] = (byte)(crc & 0xFF);
            frame[7] = (byte)((crc >> 8) & 0xFF);

            return frame;
        }

        private ushort CalculateCrc16(byte[] buffer, int length)
        {
            ushort crc = 0xFFFF;

            for (int pos = 0; pos < length; pos++)
            {
                crc ^= (ushort)buffer[pos];

                for (int i = 8; i != 0; i--)
                {
                    if ((crc & 0x0001) != 0)
                    {
                        crc >>= 1;
                        crc ^= 0xA001;
                    }
                    else
                    {
                        crc >>= 1;
                    }
                }
            }

            return crc;
        }


        Alarm_Temperature alarm_Temperature;
        public void Temperature_timer_Stop()
        {
            if (Temperature_timer.IsEnabled)
            {
                Temperature_timer.Stop();
            }
        }
        public void TimerCallbacks_Temperature_timer(object sender, EventArgs e)
        {
            try
            {
                if (alarm_Temperature != null && alarm_Temperature.IsVisible)
                {
                    return;
                }
                if(_viewModel.Temperature_AL_Connection == true)
                {
                    Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() =>
                    {
                        alarm_Temperature = new Alarm_Temperature(_viewModel);
                        if (Application.Current.MainWindow != null && Application.Current.MainWindow != alarm_Temperature)
                        {
                            alarm_Temperature.Owner = Application.Current.MainWindow;
                        }

                        alarm_Temperature.Topmost = true;
                        alarm_Temperature.Show();
                        alarm_Temperature.Activate();

                    }));
                }
            }
            catch (Exception ex)
            {

            }


        }
        #endregion Temperature

        #region 로딩언로딩
        #region 로딩
        public void 로딩()
        {
            try
            {

                var Equipment = _viewModel.EquipmentInfo.Where(equipment => !equipment.isEnable && equipment.isActive && equipment.Result != "Block").ToList();
                foreach (var equipments in Equipment)
                {
                    var equipment = equipments;
                    int ID = equipment.ID;
                    int i = ID - 1;

                    var pcbData = _viewModel.PCB_Data[i];
                    int limit = _viewModel.Config[0].BottleExistenceRange;
                    string Result = "Incubation";
                    if (pcbData.ADC <= 0 )
                    {
                        return;
                    }
                    if (pcbData.ADC > 0 && pcbData.ADC < limit )
                    {
                        if (_viewModel.Barcode_ID_Loading != "" && _viewModel.Patient_ID_Loading != "")
                        {
                            string barcodeid = _viewModel.Barcode_ID_Loading;
                            string patientid = _viewModel.Patient_ID_Loading;
                            DateTime now = DateTime.Now;
                            List<DatabaseManager_Barcode> list = new List<DatabaseManager_Barcode>();
                            list.Add(new DatabaseManager_Barcode { ID = ID, Barcode = barcodeid, Qrcode = patientid,   Loading = now, Result = Result });
                            _viewModel.databaseManagercs[(int)Enum_DatabaseManager.로딩].InsertBarcode(list);



                            string query = "UPDATE Equipment SET barcode = @barcode, Qrcode = @Qrcode, Loading = @Loading, Result = @Result,  switched = @switched, isEnable = @isEnable WHERE ID = @ID";
                            var parameters = new Dictionary<string, object>
                                        {
                                            { "@Barcode", barcodeid },
                                            { "@Qrcode", patientid },
                                            { "@Loading", now },
                                            { "@Result", "Incubation" },
                                            { "@switched", true },
                                            { "@isEnable", true },
                                            { "@ID", i + 1 }
                                        };
                            _viewModel.databaseManagercs[(int)Enum_DatabaseManager.로딩].UpdateEquipment(query, parameters);
                            _viewModel.Barcode_ID_Loading = "";
                            _viewModel.Patient_ID_Loading = "";
                            Thread.Sleep(100);
                            if (!_viewModel.Alarm_BottleLoading_Set.Contains(i))
                            {
                                _viewModel.BottleLoading_Result[i] = true;
                                _viewModel.Alarm_BottleLoading_Set.Add(i);
                                _viewModel.Alarm_BottleLoading.Enqueue(new Tuple<int>(i));
                                PCB_LED(_viewModel.SystemInfo[0].PCB_ID1, i, "Incubation");
                            }

                        }
                        else
                        {
                            if (!_viewModel.Alarm_BottleLoading_Set.Contains(i))
                            {
                                _viewModel.BottleLoading_Result[i] = true;
                                _viewModel.Equipment_DataWithDB_presenceArray[i].alive = true;
                                _viewModel.Alarm_BottleLoading_Set.Add(i);
                                _viewModel.Alarm_BottleLoading.Enqueue(new Tuple<int>(i));
                                PCB_LED(_viewModel.SystemInfo[0].PCB_ID1, i, "Error");
                            }
                        }

                    }
                    else
                    {
                        if (_viewModel.Equipment_DataWithDB_presenceArray[i].alive == true)
                        {
                            PCB_LED(_viewModel.SystemInfo[0].PCB_ID1, i, "Null");
                        }
                        if (_viewModel.EquipmentInfo[i].Result == "Null")
                        {
                            _viewModel.BottleLoading_Result[i] = false;
                            _viewModel.Equipment_DataWithDB_presenceArray[i].alive = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }


        private  void TimerCallback_BottleLoading(object sender, EventArgs e)
        {
            try
            {

                // 타이머 콜백이 호출될 때 수행할 작업을 여기에 작성합니다.
                if (!_viewModel.BottleLoading_isPopupOpen)
                {
                    if (_viewModel.Queue_PCB_Manual.Count <= 0 )
                    {
                        if (_viewModel.Alarm_BottleLoading.Count > 0)
                        {
                            if (_viewModel.Alarm_BottleLoading.TryDequeue(out Tuple<int> command))
                            {
                                _viewModel.BottleLoading_isPopupOpen = true;
                                int item1 = command.Item1;
                                _viewModel.Alarm_BottleLoading_Set.Remove(item1);
                                string sysyem = "1";
                                string cell = (item1 + 1).ToString();

                                if (item1 >= 0 && item1 <= 83)
                                {
                                    sysyem = "1";
                                    cell = (item1 + 1).ToString();
                                }
                                else if (item1 >= 84 && item1 <= 167)
                                {
                                    sysyem = "2";
                                    cell = ((item1 + 1) - 84).ToString();
                                }
                                else if (item1 >= 168 && item1 <= 251)
                                {
                                    sysyem = "3";
                                    cell = ((item1 + 1) - 168).ToString();
                                }
                                else if (item1 >= 252 && item1 <= 335)
                                {
                                    sysyem = "4";
                                    cell = ((item1 + 1) - 252).ToString();
                                }
                                if (_viewModel.EquipmentInfo[item1].isActive && _viewModel.EquipmentInfo[item1].isEnable && _viewModel.EquipmentInfo[item1].Switched)
                                {
                                    _viewModel.BottleLoading_Title = "Bottle Loading";
                                    _viewModel.BottleLoading_Content = "정상적으로 로딩되었습니다." + "\n" +
                                                        "배양을 시작합니다.";

                                    _viewModel.BottleLoading_WhatSystem = "System" + sysyem.ToString();
                                    _viewModel.BottleLoading_Cell_Num = "Cell : " + cell.ToString();
                                    _viewModel.BottleLoading_BarcodeID = "Bottle : " + _viewModel.Barcode_ID;
                                    _viewModel.BottleLoading_PatientID = "Clinical sample : " + _viewModel.Patient_ID;
                                    _viewModel.Barcode_ID = "";
                                    _viewModel.Patient_ID = "";
                                    _viewModel.Barcode_ID_Loading = "";
                                    _viewModel.Patient_ID_Loading = "";
                                }
                                else
                                {
                                    _viewModel.BottleLoading_Title = "Bottle Loading";
                                    _viewModel.BottleLoading_Content = "바코드 정보가 확인되지 않았습니다." + "\n" + "바코드를 스캔/입력 후 다시 넣어주세요.";
                                    _viewModel.BottleLoading_WhatSystem = "System" + sysyem.ToString();
                                    _viewModel.BottleLoading_Cell_Num = "Cell : " + cell.ToString();
                                    _viewModel.Barcode_ID = "";
                                    _viewModel.Patient_ID = "";
                                    _viewModel.Barcode_ID_Loading = "";
                                    _viewModel.Patient_ID_Loading = "";
                                }
                                Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() =>
                                {
                                    try
                                    {
                                        BottleLoading bottleLoading = new BottleLoading(_viewModel, item1);
                                        if (Application.Current.MainWindow != null && Application.Current.MainWindow != bottleLoading)
                                        {
                                            bottleLoading.Owner = Application.Current.MainWindow;
                                        }


                                        if (_viewModel.PCB_Status == true)
                                        {
                                            bottleLoading.Topmost = true;
                                            bottleLoading.Show();
                                            bottleLoading.Activate();
                                        }
                                        else
                                        {
                                        }
  

                                        //Window currentWindow = Window.GetWindow(this);
                                        //if (currentWindow != null && currentWindow != Application.Current.MainWindow)
                                        //{
                                        //    popup.Owner = currentWindow;
                                        //}

                                        //popup.Show(); // Show the popup

                                    }
                                    catch (Exception ex)
                                    {

                                    }
                                }));

                            
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
              
        }



        #endregion 로딩

        #region 언로딩
        public void 언로딩()
        {
            try
            {

                var Equipment = _viewModel.EquipmentInfo.Where(equipment => equipment.isEnable && equipment.isActive ).ToList();
                var oldestPositiveEquipment = Equipment
                    .Where(equipment => equipment.Result == "Positive")
                    .OrderBy(equipment => equipment.PositiveTime)
                    .FirstOrDefault();

                int fitst = oldestPositiveEquipment != null ? oldestPositiveEquipment.ID : 0;
                if( fitst > 0 )
                {
                    DateTime? positiveTimeNullable = oldestPositiveEquipment.PositiveTime;
                    string formattedDate = "";
                    if (positiveTimeNullable.HasValue)
                    {
                        DateTime positiveTime = positiveTimeNullable.Value;
                        formattedDate = positiveTime.ToString("yyyy-MM-dd HH:mm");


                    }
                    else
                    {
                        formattedDate = string.Empty;
                    }

                    _viewModel.System1_Positive_Warning = "Positive가 감지되었습니다." + " (발생 시간 : " + formattedDate + ")";
                    _viewModel.System1_Positive_Cel = cellidx(fitst - 1);
                    _viewModel.System1_HasPositive = true;
                    //_viewModel.System_PositiveFirst[fitst - 1].alive = true;
                    _viewModel.System_PositiveFirstint = fitst - 1;
                    _viewModel.System1_Result = 1;
                }
                else
                {
                    _viewModel.System1_HasPositive = false;
                    _viewModel.System_PositiveFirstint = -1;
                    _viewModel.System1_Result = 0;
                    //for (int i = 0; i < _viewModel.System_PositiveFirst.Count; i++)
                    //{
                    //    _viewModel.System_PositiveFirst[i].alive = false;
                    //}
                }

                foreach (var equipments in Equipment)
                {
                    var equipment = equipments;
                    int idx = equipment.ID - 1;
                    var pcbData = _viewModel.PCB_Data[idx];
                    int limit = _viewModel.Config[0].BottleExistenceRange;
                    string barcodeID = equipment.Barcode;
                    string PatientID = equipment.Qrcode;
                    int IncubationTime = equipment.IncubationTime;
                    string Result = equipment.Result;
                    if (pcbData.ADC > 0 && pcbData.ADC > limit )
                    {
                        if(equipment.Result == "Positive")
                        {        
                            Positive_Delete(idx, IncubationTime, equipment.ID, barcodeID, PatientID, fitst);
                        }
                        else if(equipment.Result == "Incubation")
                        {
                            Incubation_Delete(idx, IncubationTime, equipment.ID, barcodeID, PatientID);
                        }
                        else
                        {
                            Negative_Unloading_Delete(idx, IncubationTime, equipment.ID, barcodeID, PatientID);
                        }
                    }
     
                }


            }
            catch (Exception ex)
            {

            }
        }

        #region Negative_Unloading
        private Alarm_Negative Negative_Unloading;
        public void Negative_Unloading_Delete(int idx, int IncubationTime, int ID, string barcodeID, string patientID)
        {
            try
            {
                if (Negative_Unloading != null && Negative_Unloading.IsVisible)
                {
                    return; // 팝업이 열려 있으면 아무것도 하지 않음
                }
                _viewModel.Alarm_Negative_Unloading_whatSystem = systemidx(idx);
                _viewModel.Alarm_Negative_Unloading_Cell = cellidx(idx);
                _viewModel.Alarm_Negative_Unloading_BarcodeID = "Bottle  :  " + barcodeID;
                _viewModel.Alarm_Negative_Unloading_PatientID = "Clinical sample  :  " + patientID;
                Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() =>
                {
                    Negative_Unloading = new Alarm_Negative(_viewModel, idx);
                    Negative_Unloading.OKClicked += (s, e) => Negative_Unloading_OKClicked(s, e, idx, IncubationTime, ID, barcodeID);
                    Negative_Unloading.CancelClicked += (s, e) => Negative_Unloading_CancelClicked(s, e, idx, IncubationTime, ID, barcodeID);

                    if (Application.Current.MainWindow != null && Application.Current.MainWindow != Negative_Unloading)
                    {
                        Negative_Unloading.Owner = Application.Current.MainWindow;
                    }

                    Negative_Unloading.Topmost = true;
                    Negative_Unloading.Show();
                    Negative_Unloading.Activate();         
                }));
            }
            catch (Exception ex)
            {

            }
        }


        private void Negative_Unloading_OKClicked(object sender, Alarm_Negative_UnloadingEventArgs e, int idx, int incubationTime, int id, string barcodeID)
        {
            try
            {
                int IDX = idx;
                int IncubationTime = incubationTime;
                int ID = id;
                string BarcodeID = barcodeID;
                DateTime now = DateTime.Now;
                string FormattedNow = now.ToString("yyyy-MM-dd HH:mm:ss");


                string UpdateBarcode_Query = "UPDATE Barcode SET " +
                                  "Unloading = @Unloading, IncubationTime = @IncubationTime " +
                                  "WHERE Barcode = @Barcode";



                Dictionary<string, object> UpdateBarcode_parameters = new Dictionary<string, object>
                        {

                            { "@Unloading", now },
                            { "@IncubationTime", IncubationTime },
                            { "@Barcode", BarcodeID },
                        };
                _viewModel.databaseManagercs[(int)Enum_DatabaseManager.언로딩].UpdateBarcode(UpdateBarcode_Query, UpdateBarcode_parameters);

                string UpdateEquipment_Query = "UPDATE Equipment SET " +
                                                 "Barcode = @Barcode, Qrcode = @Qrcode, Loading = @Loading, CreDate = @CreDate, PositiveTime = @PositiveTime, Result = @Result, IncubationTime = @IncubationTime, switched = @switched, isEnable = @isEnable " +
                                                 "WHERE ID = @ID";
                Dictionary<string, object> UpdateEquipment_parameters = new Dictionary<string, object>
                        {
                            { "@Barcode", null },
                            { "@Qrcode", null },
                            { "@Loading", null },
                            { "@CreDate", null },
                            { "@PositiveTime", null },
                            { "@Result", "Null" },
                            { "@IncubationTime", null },
                            { "@switched", false },
                            { "@isEnable", false },
                            { "@ID", ID }  // 
                        };
                _viewModel.databaseManagercs[(int)Enum_DatabaseManager.언로딩].UpdateEquipment(UpdateEquipment_Query, UpdateEquipment_parameters);
                _viewModel.PositiveDelay[IDX] = 0;
                _viewModel.Result_PositiveTime[IDX] = new Queue<(DateTime, double)>();
<<<<<<< HEAD

=======
  
>>>>>>> 499d608513e1843495adb00cd694c16041388782

                PCB_LED(_viewModel.SystemInfo[0].PCB_ID1, IDX, "Null");
            }
            catch (Exception ex)
            {
            }
        }

        private void Negative_Unloading_CancelClicked(object sender, Alarm_Negative_UnloadingEventArgs e, int idx, int IncubationTime, int ID, string barcodeI)
        {

        }
        #endregion Negative_Unloading

        #region Positive_Unloading
        private Alarm_Positive_Unloading Positive_Unloading;
        public void Positive_Delete(int idx, int IncubationTime, int ID, string barcodeID, string PatientID, int first)
        {
            try
            {
                if (Positive_Unloading != null && Positive_Unloading.IsVisible)
                {
                    return;
                }
                _viewModel.Alarm_Positive_Unloading_whatSystem = systemidx(idx);
                _viewModel.Alarm_Positive_Unloading_Cell = cellidx(idx);
                _viewModel.Alarm_Positive_Unloading_BarcodeID = "Bottle :  " + barcodeID;
                _viewModel.Alarm_Positive_Unloading_PatientID = "Clinical sample  :  " + PatientID;

                if (idx == first - 1)
                {
                    _viewModel.Alarm_Positive_Unloading_Warning = "해당 Positive를 제거합니다.";
                }
                else
                {
                    _viewModel.Alarm_Positive_Unloading_Warning = "해당 Positive를 먼저 제거하시겠습니까?";
                }

                Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() =>
                {
                    Positive_Unloading = new Alarm_Positive_Unloading(_viewModel);
                    Positive_Unloading.OKClicked += (s, e) => Positive_Unloading_OKClicked(s, e, idx, IncubationTime, ID, barcodeID);
                    Positive_Unloading.CancelClicked += (s, e) => Positive_Unloading_CancelClicked(s, e, idx, IncubationTime, ID, barcodeID);
                    if (Application.Current.MainWindow != null && Application.Current.MainWindow != Positive_Unloading)
                    {
                        Positive_Unloading.Owner = Application.Current.MainWindow;
                    }

                    Positive_Unloading.Topmost = true;
                    Positive_Unloading.Show();
                    Positive_Unloading.Activate();
                }));

            }
            catch (Exception ex)
            {

            }
        }
        private void Positive_Unloading_OKClicked(object sender, Alarm_Positive_UnloadingEventArgs e, int idx, int incubationTime, int id, string barcodeID)
        {
            try
            {
                int IDX = idx;
                int IncubationTime = incubationTime;
                int ID = id;
                string BarcodeID = barcodeID;
                DateTime now = DateTime.Now;
                string FormattedNow = now.ToString("yyyy-MM-dd HH:mm:ss");


                string UpdateBarcode_Query = "UPDATE Barcode SET " +
                                  "Unloading = @Unloading, IncubationTime = @IncubationTime " +
                                  "WHERE Barcode = @Barcode";

                Dictionary<string, object> UpdateBarcode_parameters = new Dictionary<string, object>
                        {

                            { "@Unloading", now },
                            { "@IncubationTime", IncubationTime },
                            { "@Barcode", BarcodeID },
                        };
                _viewModel.databaseManagercs[(int)Enum_DatabaseManager.언로딩].UpdateBarcode(UpdateBarcode_Query, UpdateBarcode_parameters);

                string UpdateEquipment_Query = "UPDATE Equipment SET " +
                                                 "Barcode = @Barcode, Qrcode = @Qrcode, Loading = @Loading, CreDate = @CreDate, PositiveTime = @PositiveTime, Result = @Result, IncubationTime = @IncubationTime, switched = @switched, isEnable = @isEnable " +
                                                 "WHERE ID = @ID";
                Dictionary<string, object> UpdateEquipment_parameters = new Dictionary<string, object>
                        {
                            { "@Barcode", null },
                            { "@Qrcode", null },
                            { "@Loading", null },
                            { "@CreDate", null },
                            { "@PositiveTime", null },
                            { "@Result", "Null" },
                            { "@IncubationTime", null },
                            { "@switched", false },
                            { "@isEnable", false },
                            { "@ID", ID }  // 
                        };
                _viewModel.databaseManagercs[(int)Enum_DatabaseManager.언로딩].UpdateEquipment(UpdateEquipment_Query, UpdateEquipment_parameters);





                _viewModel.PositiveDelay[IDX] = 0;
                _viewModel.Result_PositiveTime[IDX] = new Queue<(DateTime, double)>();
<<<<<<< HEAD
=======
      
>>>>>>> 499d608513e1843495adb00cd694c16041388782
                PCB_LED(_viewModel.SystemInfo[0].PCB_ID1, IDX, "Null");
            }
            catch (Exception ex)
            {
            }  
        }

        private void Positive_Unloading_CancelClicked(object sender, Alarm_Positive_UnloadingEventArgs e, int idx, int IncubationTime, int ID, string barcodeI)
        {

        }
        #endregion Positive_Unloading

        #region Incubation
        private Alarm_Incubation Incubation;
        public void Incubation_Delete(int idx, int IncubationTime, int ID, string barcodeID, string PatientID)
        {
            try
            {
                if (Incubation != null && Incubation.IsVisible)
                {
                    return; // 팝업이 열려 있으면 아무것도 하지 않음
                }
                _viewModel.Alarm_Incubation_whatSystem = systemidx(idx);
                _viewModel.Alarm_Incubation_Cell = cellidx(idx);
                _viewModel.Alarm_Incubation_BarcodeID = "Bottle : " + barcodeID;
                _viewModel.Alarm_Incubation_PatientID = "Clinical sample : " + PatientID;
                _viewModel.System_IncubationFirstint = idx;
  
                Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() =>
                {
                    Incubation = new Alarm_Incubation(_viewModel, idx);
                    Incubation.OKClicked += (s, e) => Incubation_OKClicked(s, e, idx, IncubationTime, ID, barcodeID);
                    Incubation.CancelClicked += (s, e) => Incubation_CancelClicked(s, e, idx, IncubationTime, ID, barcodeID);
                    if (Application.Current.MainWindow != null && Application.Current.MainWindow != Incubation)
                    {
                        Incubation.Owner = Application.Current.MainWindow;
                    }

                    Incubation.Topmost = true;
                    Incubation.Show();
                    Incubation.Activate();

                }));
            }
            catch (Exception ex)
            {

            }
        }
        private void Incubation_OKClicked(object sender, PopupEventArgs e, int idx, int incubationTime, int id, string barcodeID)
        {
            try
            {
                int IDX = idx;
                int IncubationTime = incubationTime;
                int ID = id;
                string BarcodeID = barcodeID;
                DateTime now = DateTime.Now;
                string FormattedNow = now.ToString("yyyy-MM-dd HH:mm:ss");

                string UpdateBarcode_Query = "UPDATE Barcode SET " +
                                         "Unloading = @Unloading, " +
                                         "IncubationTime = @IncubationTime, " +
                                         "Result = @Result " +
                                         "WHERE Barcode = @Barcode";



                Dictionary<string, object> UpdateBarcode_parameters = new Dictionary<string, object>
                        {

                            { "@Unloading", now },
                            { "@IncubationTime", IncubationTime },
                            { "@Result", "Removed" },
                            { "@Barcode", BarcodeID },
                        };
                _viewModel.databaseManagercs[(int)Enum_DatabaseManager.언로딩].UpdateBarcode(UpdateBarcode_Query, UpdateBarcode_parameters);

                string UpdateEquipment_Query = "UPDATE Equipment SET " +
                                                 "Barcode = @Barcode, Qrcode = @Qrcode, Loading = @Loading, CreDate = @CreDate, PositiveTime = @PositiveTime, Result = @Result, IncubationTime = @IncubationTime, switched = @switched, isEnable = @isEnable " +
                                                 "WHERE ID = @ID";
                Dictionary<string, object> UpdateEquipment_parameters = new Dictionary<string, object>
                        {
                            { "@Barcode", null },
                            { "@Qrcode", null },
                            { "@Loading", null },
                            { "@CreDate", null },
                            { "@PositiveTime", null },
                            { "@Result", "Null" },
                            { "@IncubationTime", null },
                            { "@switched", false },
                            { "@isEnable", false },
                            { "@ID", ID }  // 
                        };
                _viewModel.databaseManagercs[(int)Enum_DatabaseManager.언로딩].UpdateEquipment(UpdateEquipment_Query, UpdateEquipment_parameters);
                _viewModel.System_IncubationFirstint = -1;
                _viewModel.PositiveDelay[IDX] = 0;
                _viewModel.Result_PositiveTime[IDX] = new Queue<(DateTime, double)>();
        
                PCB_LED(_viewModel.SystemInfo[0].PCB_ID1, IDX, "Null");
            }
            catch (Exception ex)
            {
                _viewModel.System_IncubationFirstint = -1;
            }
        }

        private void Incubation_CancelClicked(object sender, PopupEventArgs e, int idx, int IncubationTime, int ID, string barcodeI)
        {
            _viewModel.System_IncubationFirstint = -1;
        }
        #endregion Incubation

        #region spare
        #endregion spare

        #endregion 언로딩
        #endregion 로딩언로딩

        #region WriteBarcodepage
        public void WriteBarcodepage()
        {
            try
            {
                WriteBarcode writeBarcode = new WriteBarcode(_viewModel);
                writeBarcode.ShowDialog();
            }
            catch (Exception ex)
            {

            }
        }
        #endregion WriteBarcode

        #region Tilting

        public void  문상태확인및틸팅제어()
        {       
                try
            {
                if (_viewModel.FASTECH_IO_Connection == false)
                {
                    return;
                }
                bool currentDoorState = _viewModel.FASTECH_Input[(int)Enum_FASTECH_Input.Door].Flag;

                    if (currentDoorState != _viewModel.Door_previousDoorState)
                    {
                        if (currentDoorState == true)
                        {
                            _viewModel.MainWindow_ButtonFlag = Enum_MainWindow_ButtonFlag.Home;
                             Door_timer_Stop(); 
                    }
                        else
                        {
                        _viewModel.FASTECH_Input_Latch = _viewModel.FASTECH_Input[(int)Enum_FASTECH_Input.Trigger].Latch;

                        _viewModel.MainWindow_ButtonFlag = Enum_MainWindow_ButtonFlag.SystemRack1;
                            Door_timer_Start();
                    }
                        _viewModel.Door_previousDoorState = currentDoorState;
                    }

                    if (currentDoorState == true)
                    {
                        틸팅시작();
                    }
                    else
                    {
                        if (_viewModel.FASTECH_Input[(int)Enum_FASTECH_Input.Trigger].Flag == false)
                        {
                            if (!_viewModel.Buzzer)
                        {
                            _viewModel.Buzzer = true;
              
                            }
                        }
                        if (틸팅중지())
                        {
                            _viewModel.Buzzer = false;
                        }
                    }
                }
                catch (Exception ex)
                {
                    // 예외 처리 로직
                }
            }
        public void 틸팅시작()
        {
            try
            {
                _viewModel.FASTECH_Set_Output[(int)Enum_FASTECH_Output.Tiling].Flag = true;
            }
            catch (Exception ex)
            {

            }
        }

        public bool 틸팅중지()
        {
            try
            {
                if (_viewModel.FASTECH_Input[(int)Enum_FASTECH_Input.Trigger].Flag == true)
                {
                    _viewModel.FASTECH_Set_Output[(int)Enum_FASTECH_Output.Tiling].Flag = false;
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
               
                return false;
            }
        }

        private async Task ToggleBuzzer(CancellationToken token)
        {
            try
            {
                while (true)
                {
                    if (_viewModel.Config[0].UseBuzzer)
                    {
                        _viewModel.FASTECH_Set_Output[(int)Enum_FASTECH_Output.Buzzer].Flag = true;
                    }
                    await Task.Delay(500, token); 

                  
                    _viewModel.FASTECH_Set_Output[(int)Enum_FASTECH_Output.Buzzer].Flag = false;
                    await Task.Delay(500, token);
                }
            }
            catch (OperationCanceledException)
            {
               
            }
            catch (Exception ex)
            {
              
            }
        }

        private async Task<bool> WaitForTrigger(int timeout)
        {
            var stopwatch = Stopwatch.StartNew();

            try
            {
                while (stopwatch.ElapsedMilliseconds < timeout)
                {
                    if (_viewModel.FASTECH_Input[(int)Enum_FASTECH_Input.Trigger].Flag == true)
                    {
                        return true;
                    }
                    await Task.Delay(1); // 아주 짧은 시간 대기 (센서 실시간 감지)
                }
                return false;
            }
            catch (Exception ex)
            {
                // Handle exception if necessary
                return false;
            }
        }




        //public async Task<bool> 틸팅중지()
        //{
        //    try
        //    {

        //        var stopwatch = Stopwatch.StartNew();
        //        int timeout = 5000;
        //        while (stopwatch.ElapsedMilliseconds < timeout)
        //        {
        //            if (_viewModel.FASTECH_Input[(int)Enum_FASTECH_Input.Trigger].Flag == true)
        //            {
        //                _viewModel.FASTECH_Set_Output[(int)Enum_FASTECH_Output.Tiling].Flag = false;
        //                return true;
        //            }
        //            await Task.Delay(1);
        //        }
        //        return false;

        //    }
        //    catch (Exception ex)
        //    {
        //        return false;
        //    }
        //}
        #endregion Tilting

        #region Door

        public void Door_timer_Start()
        {
           int Interval = _viewModel.Config[0].doorOpenAlarmTrigger;
            Door_timer.Interval = TimeSpan.FromMinutes(Interval);
            Door_timer.Start();
        }
        public void Door_timer_Stop()
        {
            if (Door_timer.IsEnabled)
            {
                Door_timer.Stop();
            }
        }
        public void TimerCallbacks_Door_timer(object sender, EventArgs e)
        {
           try
            {
                _viewModel.Alarm_Door_Buzzer = true; 
                Door_timer_Stop();
                Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() =>
                {
                    Alarm_Door alarm_Door = new Alarm_Door(_viewModel);
                    if (Application.Current.MainWindow != null && Application.Current.MainWindow != alarm_Door)
                    {
                        alarm_Door.Owner = Application.Current.MainWindow;
                    }

                    alarm_Door.Topmost = true;
                    alarm_Door.Show();
                    alarm_Door.Activate();

                }));
            }
            catch(Exception ex)
            {

            }


        }
        #endregion Door

        #region Lamp
        public void Lamp()
        {
            try
            {
                List<DatabaseManager_Equipment> equ = _viewModel.EquipmentInfo;
                bool hasPositive = equ.Any(e => e.Result == "Positive");
                bool hasNegative = equ.Any(e => e.Result == "Negative");
                bool Dooropen = _viewModel.FASTECH_Input[(int)Enum_FASTECH_Input.Door].Flag;
                bool Indi = _viewModel.Equipment_DataWithDB_presenceArray.Any(data => data.alive == true);
                if (Indi)
                {
                    if(_viewModel.Lamp)
                    {
                        Lamp_Yellow_On();
                        _viewModel.Lamp = !_viewModel.Lamp;
                    }
                    else
                    {
                        Lamp_Yellow_Off();
                        _viewModel.Lamp = !_viewModel.Lamp;
                    }

                }
                else if(hasPositive )
                {
                    Lamp_Red();
                }
                else if (hasNegative || Dooropen == false)
                {
                    Lamp_Yellow();
                }
         
                else
                {
                    Lamp_Green();
                }
            }
            catch (Exception ex)
            {

            }
        }

        public void Lamp_Red()
        {
            try
            {
                _viewModel.FASTECH_Set_Output[(int)Enum_FASTECH_Output.GreenLamp].Flag = false;
                _viewModel.FASTECH_Set_Output[(int)Enum_FASTECH_Output.RedLamp].Flag = true;
                _viewModel.FASTECH_Set_Output[(int)Enum_FASTECH_Output.YellowLamp].Flag = false;

            }
            catch (Exception ex)
            {

            }
        }
        public void Lamp_Green()
        {
            try
            {
                _viewModel.FASTECH_Set_Output[(int)Enum_FASTECH_Output.GreenLamp].Flag = true;
                _viewModel.FASTECH_Set_Output[(int)Enum_FASTECH_Output.RedLamp].Flag = false;
                _viewModel.FASTECH_Set_Output[(int)Enum_FASTECH_Output.YellowLamp].Flag = false;
            }
            catch (Exception ex)
            {

            }
        }
        public void Lamp_Yellow()
        {
            try
            {
                _viewModel.FASTECH_Set_Output[(int)Enum_FASTECH_Output.GreenLamp].Flag = true;
                _viewModel.FASTECH_Set_Output[(int)Enum_FASTECH_Output.RedLamp].Flag = true;
                _viewModel.FASTECH_Set_Output[(int)Enum_FASTECH_Output.YellowLamp].Flag = false;
            }
            catch (Exception ex)
            {

            }
        }
        public void Lamp_Blinking()
        {
            try
            {
     


                _viewModel.FASTECH_Set_Output[(int)Enum_FASTECH_Output.RedLamp].Flag = !_viewModel.FASTECH_Set_Output[(int)Enum_FASTECH_Output.RedLamp].Flag;
            }
            catch (Exception ex)
            {

            }
        }

        public void Lamp_Yellow_On()
        {
            try
            {
                _viewModel.FASTECH_Set_Output[(int)Enum_FASTECH_Output.GreenLamp].Flag = true;
                _viewModel.FASTECH_Set_Output[(int)Enum_FASTECH_Output.RedLamp].Flag = true;
                _viewModel.FASTECH_Set_Output[(int)Enum_FASTECH_Output.YellowLamp].Flag = false;
            }
            catch (Exception ex)
            {

            }
        }

        public void Lamp_Yellow_Off()
        {
            try
            {
                _viewModel.FASTECH_Set_Output[(int)Enum_FASTECH_Output.GreenLamp].Flag = false;
                _viewModel.FASTECH_Set_Output[(int)Enum_FASTECH_Output.RedLamp].Flag = false;
                _viewModel.FASTECH_Set_Output[(int)Enum_FASTECH_Output.YellowLamp].Flag = false;
            }
            catch (Exception ex)
            {

            }
        }

        #endregion Lamp

        #region Buzzer
        public void Buzzer_timer_Stop()
        {
            if (Buzzer_timer.IsEnabled)
            {
                Buzzer_timer.Stop();
            }
        }
        private void TimerCallbacks_Buzzer_timer(object sender, EventArgs e)
        {
            try
            {
                if (_viewModel.Config[0].UseBuzzer == false)
                {
                    _viewModel.FASTECH_Set_Output[(int)Enum_FASTECH_Output.Buzzer].Flag = false;
                    return;
                }
                 if (_viewModel.Equipment_DataWithDB_presenceArray.Any(data => data.alive == true) && _viewModel.PCB_Restart)
                {
                    _viewModel.FASTECH_Set_Output[(int)Enum_FASTECH_Output.Buzzer].Flag = !_viewModel.FASTECH_Set_Output[(int)Enum_FASTECH_Output.Buzzer].Flag;
                }
                else if (_viewModel.System_PositiveFirstint != -1)
                {
                    _viewModel.FASTECH_Set_Output[(int)Enum_FASTECH_Output.Buzzer].Flag = !_viewModel.FASTECH_Set_Output[(int)Enum_FASTECH_Output.Buzzer].Flag;
                }

                else if (_viewModel.Buzzer && _viewModel.FASTECH_Input[(int)Enum_FASTECH_Input.Door].Flag == false || _viewModel.Alarm_Door_Buzzer == true)
                {
                    if (_viewModel.FASTECH_Input_Latch >= _viewModel.FASTECH_Input[(int)Enum_FASTECH_Input.Trigger].Latch)
                    {
                        _viewModel.FASTECH_Set_Output[(int)Enum_FASTECH_Output.Buzzer].Flag = !_viewModel.FASTECH_Set_Output[(int)Enum_FASTECH_Output.Buzzer].Flag;
                    }
                    else
                    {
                        _viewModel.FASTECH_Set_Output[(int)Enum_FASTECH_Output.Buzzer].Flag = false;
                    }

                }
     

                else
                {
                    _viewModel.FASTECH_Set_Output[(int)Enum_FASTECH_Output.Buzzer].Flag = false;
                }
            }
            catch (Exception ex)
            {
                _viewModel.FASTECH_Set_Output[(int)Enum_FASTECH_Output.Buzzer].Flag = false;
            }      
        }
        #endregion Buzzer

        #region Alarm
        public void Alarmini()
        {
            Alarm_BottleLoading_ini();
        }

        #region BottleLoading
        public void Alarm_BottleLoading_ini()
        {
            for(int i=0; i< _viewModel.Common_TotalSystemCellCount; i++)
            {
                _viewModel.BottleLoading_Result[i] = false;
            }
        }
        #endregion BottleLoading

        #region Positive
        private Alarm_Positive alarm_positive;
        public void TimerCallback_Alarm_Positive(object sender, EventArgs e)
        {
            try
            {
                if (alarm_positive != null && alarm_positive.IsVisible || (Positive_Unloading != null && Positive_Unloading.IsVisible))
                {
                    return; // 팝업이 열려 있으면 아무것도 하지 않음
                }

                if(_viewModel.System_PositiveFirstint != -1 && _viewModel.MainWindow_ButtonFlag !=  Enum_MainWindow_ButtonFlag.SystemRack1 && _viewModel.MainWindow_ButtonFlag != Enum_MainWindow_ButtonFlag.Report && _viewModel.MainWindow_ButtonFlag != Enum_MainWindow_ButtonFlag.Conguration && _viewModel.FASTECH_IO_Connection && _viewModel.PCB_Connection)
                {
                    int item1 = _viewModel.System_PositiveFirstint;

                    DateTime? positiveTimeNullable = _viewModel.EquipmentInfo[item1].PositiveTime;
                    string formattedDate = "";
                    if (positiveTimeNullable.HasValue)
                    {
                        DateTime positiveTime = positiveTimeNullable.Value;
                         formattedDate = positiveTime.ToString("yyyy-MM-dd HH:mm");

                    }
                    else
                    {
                        formattedDate = string.Empty; 
                    }

                    string DateNow = formattedDate;  
                    _viewModel.Alarm_Positive_whatSystem = systemidx(item1);
                    _viewModel.Alarm_Positive_Cell = cellidx(item1);
                    _viewModel.Alarm_Positive_Warning = "시간 : " + DateNow + "\n" +
                     "Positive가 감지되었습니다." + "\n" +
                     "해당 cell에 삽입된 bottle을 제거해 주세요." + "\n";
                    Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() =>
                    {
                        alarm_positive = new Alarm_Positive(_viewModel);
                        if (Application.Current.MainWindow != null && Application.Current.MainWindow != alarm_positive)
                        {
                            alarm_positive.Owner = Application.Current.MainWindow;
                        }

                        //alarm_positive.Topmost = true;
                        alarm_positive.Show();
                        alarm_positive.Activate();
                    }));
                }
            }
            catch(Exception ex)
            {

            }
        }
        #endregion Positive

        #region System
        private Alarm_System alarm_System;
        public void TimerCallbacks_Alarm_System_timer(object sender, EventArgs e)
        {
            try
            {
                if (alarm_System != null && alarm_System.IsVisible)
                {
                    return; // 팝업이 열려 있으면 아무것도 하지 않음
                }
                if(!_viewModel.FASTECH_IO_Connection || !_viewModel.PCB_Connection)
                {
                    _viewModel.System_Title = "Connect Error";
                    _viewModel.System_Content = "System1 is not Connected.";
                    Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() =>
                    {
                        alarm_System = new Alarm_System(_viewModel);
                        if (Application.Current.MainWindow != null && Application.Current.MainWindow != alarm_System)
                        {
                            alarm_System.Owner = Application.Current.MainWindow;
                        }
                        alarm_System.Topmost = true;
                        alarm_System.Show();
                        alarm_System.Activate();
                    }));
                }
            }
            catch (Exception ex)
            {
            }
        }
        #endregion System
        #endregion Alarm

        #region Calibration

        public bool test5 = false;
        public void Calibration()
        {
            try
            {
                Thread.Sleep(2000);
                int Calibration_From = _viewModel.Calibration_From;
                int Calibration_To = _viewModel.Calibration_To;
                for (int i = Calibration_From; i < Calibration_To + 1; i++)
                {

                    for (int k = 1; k <= 60; k++)
                    {
                        int ini = 0;
                        int line = (i - 1) / 28;
                        int channel = (i - 1) % 28 + 1;
                        string commandBase_CH = $"{_viewModel.SystemInfo[0].PCB_ID1},LINE{line},DIM,CH{channel}";
                        string commandBase_ADCREAD = $"{_viewModel.SystemInfo[0].PCB_ID1},LINE{line},ADCREAD";
                        string commandBase_DIMREAD = $"{_viewModel.SystemInfo[0].PCB_ID1},LINE{line},DIMREAD";


                        string command = $"{commandBase_CH},{k}";

                        _viewModel.Queue_PCB_Manual.Enqueue(command);
                        Thread.Sleep(300);
                        _viewModel.Queue_PCB_Manual.Enqueue(commandBase_ADCREAD);
                        Thread.Sleep(300);
                        _viewModel.Queue_PCB_Manual.Enqueue(commandBase_DIMREAD);
                        Thread.Sleep(300);

                        int lints = line * 28;
                        var stopwatch = Stopwatch.StartNew();
                        const int readTimeout = 2000;
                        while (_viewModel.PCB_Data[i - 1].LED != k)
                        {
                            if(stopwatch.ElapsedMilliseconds > readTimeout)
                            {
                                _viewModel.Calibration_Falg = false;
                                return;
                            }
                        }

                        double averageADC = _viewModel.PCB_Data[i - 1].ADC;
                        double lowerBound = _viewModel.PCB_targetvalue;
                        double upperBound = _viewModel.PCB_targetvalue + 0.01;

                        if (averageADC >= lowerBound)
                        {
                            break;
                        }
                        if (averageADC > upperBound)
                        {
                            break;
                        }
                    }

                }
                _viewModel.Calibration_Falg = false;
            }
            catch(Exception ex)
            {
                _viewModel.Calibration_Falg = false;
            }
        }
        #endregion Calibration

        #region LED
        public void LEDini()
        {
            try
            {
                string id = _viewModel.SystemInfo[0].PCB_ID1;
                for (int i = 0; i < _viewModel.Common_SystemCellCount; i++)
                {
                    if (_viewModel.Equipment_DataWithDB_presenceArray[i].alive == true)
                    {
                        PCB_LED(id, i, "Error");
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        public void PCB_LED(string boardID, int idx, string result)
        {
            try
            {
                string BoardID = boardID;
                string Result = result;
                int index = idx;

                string line = DetermineLine(index);
                string channel = DetermineChannel(index);

                if (Result == "Positive")
                {
                    string color = GetRgbColor(2); // Assuming this method returns the correct color format
                    string command = $"{BoardID},{line},TLED,{channel},ON,{color}";
                    _viewModel.Queue_PCB_Manual.Enqueue(command);
                }
                else if (Result == "Negative")
                {
                    string color = GetRgbColor(3); // Assuming this method returns the correct color format
                    string command = $"{BoardID},{line},TLED,{channel},ON,{color}";
                    _viewModel.Queue_PCB_Manual.Enqueue(command);
                }
                else if (Result == "Incubation")
                {
                    string color = GetRgbColor(1); // Assuming this method returns the correct color format
                    string command = $"{BoardID},{line},TLED,{channel},ON,{color}";
                    _viewModel.Queue_PCB_Manual.Enqueue(command);
                }
                else if (Result == "Block")
                {
                    string color = GetRgbColor(4); // Assuming this method returns the correct color format
                    string command = $"{BoardID},{line},TLED,{channel},ON,{color}";
                    _viewModel.Queue_PCB_Manual.Enqueue(command);
                }
                else if (Result == "Error")
                {
                    string color = GetRgbColor(5); // Assuming this method returns the correct color format
                    string command = $"{BoardID},{line},TLED,{channel},ON,{color}";
                    _viewModel.Queue_PCB_Manual.Enqueue(command);
                }
                else
                {
                    string color = GetRgbColor(0);
                    string command = $"{BoardID},{line},TLED,{channel},OFF";
                    _viewModel.Queue_PCB_Manual.Enqueue(command);
                }
            }
            catch (Exception ex)
            {

            }
        }
        private string DetermineLine(int index)
        {
            if (index >= 0 && index <= 27) return "LINE0";
            else if (index >= 28 && index <= 55) return "LINE1";
            else return "LINE2";
        }

        private string DetermineChannel(int index)
        {
            return $"CH{index % 28 + 1}";
        }

        private string GetRgbColor(int aliveValue)
        {
            string hexColor = aliveValue switch
            {
                0 => "#FFAF9D9D", // Null
                1 => "#FF00FF00", //Incubation
                2 => "#FFFF0000", //Positive
                3 => "#FFFFA500", //Negative
                4 => "#FF191311", //Black
                5 => "#FFFFA500", //Error
                _ => "#FFFFFFFF"
            };


            if (hexColor.StartsWith("#") && (hexColor.Length == 7 || hexColor.Length == 9))
            {
                try
                {
                    int startIndex = hexColor.Length == 9 ? 3 : 1;
                    int r = Convert.ToInt32(hexColor.Substring(startIndex, 2), 16);
                    int g = Convert.ToInt32(hexColor.Substring(startIndex + 2, 2), 16);
                    int b = Convert.ToInt32(hexColor.Substring(startIndex + 4, 2), 16);
                    return $"{r},{g},{b}";
                }
                catch
                {

                    return "255,255,255";
                }
            }
            else
            {
                return "255,255,255";
            }
        }
        #endregion LED

        #region LOG
        public void TimerCallback_LOG_timer(object sender, EventArgs e)
        {
            try
            {
                LOG();
            }
            catch (Exception ex)
            {

            }
        }

        public void LOG()
        {
            try
            {
                // 큐에 항목이 있는지 확인
                if (!_viewModel.Queue_LOG.IsEmpty)
                {
                    // 큐에 항목이 있으면 반복 처리
                    while (_viewModel.Queue_LOG.TryDequeue(out var logEntry))
                    {
                        string data = logEntry.Key;
                        Enum_LOG logType = logEntry.Value;

                        string dateFolder = Path.Combine("D:\\LOG", DateTime.Now.ToString("yyyy"), DateTime.Now.ToString("MM"), DateTime.Now.ToString("dd"));
                        Directory.CreateDirectory(dateFolder);

                        string fileName = logType.ToString() + ".txt";
                        string filePath = Path.Combine(dateFolder, fileName);

                        // 시간을 포함한 데이터를 파일에 기록
                        string timeStamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        string logData = $"{timeStamp} : {data}";

                        using (StreamWriter writer = new StreamWriter(filePath, true))
                        {
                            writer.WriteLine(logData);
                        }

                        //File.AppendAllText(filePath, logData + Environment.NewLine);
                    }

                    // 모든 항목을 처리한 후 새로운 로그 항목을 추가

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error during logging: " + ex.Message);
            }
        }
        #endregion LOG

        #region Function
        public string systemidx(int idx)
        {
            try
            {
                int index = idx;
                string data = "";
                if (index >= 0 && index <= 83)
                {
                    data = "1";
                }
                else if (index >= 84 && index <= 167)
                {
                    data = "2";
                }
                else if (index >= 168 && index <= 251)
                {
                    data = "3";
                }
                else if (index >= 252 && index <= 335)
                {
                    data = "4";
                }
                return "System" + data;

            }
            catch (Exception ex)
            {
                return "System" + "1";
            }
        }
        public string cellidx(int idx)
        {
            try
            {
                int index = idx;
                string data = "";
                if (index >= 0 && index <= 83)
                {
                    data = (index + 1).ToString();
                }
                else if (index >= 84 && index <= 167)
                {
                    data = ((index + 1) - 84).ToString();
                }
                else if (index >= 168 && index <= 251)
                {
                    data = ((index + 1) - 168).ToString();
                }
                else if (index >= 252 && index <= 335)
                {
                    data = ((index + 1) - 252).ToString();
                }
                return "Cell : " + data;

            }
            catch (Exception ex)
            {
                return "Cell : " + "1";
            }
        }

        #endregion Function


        double adc = 250;

        private string DetermineResult2(int idx, double adc, double incubationTime, DateTime date)
        {
            DateTime Date = date;
            int Positive_Delay = _viewModel.Config[0].Positive_Wait;
            double IncubationTime = incubationTime;
            double ADC = adc;

            if (ADC <= 0)
            {
                return "";
            }
            if (IncubationTime >= _viewModel.Common_Hour)
            {
                if (ReceiveNewVoltageValue(idx, ADC, Date))
                {
                    return "Positive";
                }
            }

            return "";
        }

        private string DetermineResult3(int idx, double incubationTime, int maximumTime, DateTime date, double  adc)
        {
            DateTime Date = date;
            int Positive_Delay = _viewModel.Config[0].Positive_Wait;
            double IncubationTime = incubationTime;
            double ADC = adc;

            if (ADC <= 0)
            {
                return "";
            }
            if (IncubationTime >= _viewModel.Common_Hour)
            {
                if (ReceiveNewVoltageValue(idx, ADC, Date))
                {
                    return "Positive";
                }
            }
            if (_viewModel.PositiveDelay[idx] > Positive_Delay)
            {
                return "Positive";
            }
            if (IncubationTime >= maximumTime)
            {
                return "Negative";
            }
            return "";
        }
        DateTime NOW;
        double ACD = 200;
        int iNCUBATIONT = 3600;

        public void createGradient(int idx, string barcodeid, string qrcode)
        {
            int index = idx;
            int ID = idx + 1;
            string Barcodeid = barcodeid;
            string Qrcode = qrcode;
            string Result = "Incubation";

            int Incubationtime = 0;
            int LED = 40;
            double Temperature = 36.5;


            double initialADC = 210;
            double finalADC = 220;
            int startPoint = 330;
            int endPoint = 390;
            int totalPoints = 1000;

            double rangeIncrement = (finalADC - initialADC) / (endPoint - startPoint);
            double totalIncrement = rangeIncrement * (endPoint - startPoint);
            double requiredIncrement = totalIncrement * 1.1; // 20% increase

            DateTime loadingTimes = DateTime.Now;
            DateTime cretime = loadingTimes;
            DateTime unloadingTime = DateTime.Now;

            for (int i = 0; i < totalPoints; i++)
            {
                double adcValue = initialADC;


                if (i >= startPoint && i < endPoint)
                {
                    double adjustedIncrement = (requiredIncrement / (endPoint - startPoint)) * (i - startPoint);
                    adcValue += adjustedIncrement;
                }
                else if (i >= endPoint)
                {
                    adcValue += requiredIncrement;
                }


                adcValue = Math.Round(adcValue);

                if (i == 0)
                {

                    List<DatabaseManager_Barcode> list = new List<DatabaseManager_Barcode>();
                    list.Add(new DatabaseManager_Barcode { ID = ID, Barcode = Barcodeid, Qrcode = Qrcode, Loading = loadingTimes, Result = Result });
                    _viewModel.databaseManagercs[(int)Enum_DatabaseManager.로딩].InsertBarcode(list);

                    string query = "UPDATE Equipment SET barcode = @barcode, Qrcode = @Qrcode, Loading = @Loading, Result = @Result,  switched = @switched, isEnable = @isEnable WHERE ID = @ID";
                    var parameters = new Dictionary<string, object>
                                        {
                                            { "@Barcode", barcodeid },
                                            { "@Qrcode", Qrcode },
                                            { "@Loading", loadingTimes },
                                            { "@Result", "Incubation" },
                                            { "@switched", true },
                                            { "@isEnable", true },
                                            { "@ID", i + 1 }
                                        };
                    _viewModel.databaseManagercs[(int)Enum_DatabaseManager.로딩].UpdateEquipment(query, parameters);

                    string updateQuery_Equipment = "UPDATE Equipment SET IncubationTime = @IncubationTime WHERE ID = @ID";
                    Dictionary<string, object> UpdateEquipment_parameters = new Dictionary<string, object>
                        {
                            { "@IncubationTime", Incubationtime },
                            { "@ID", ID }  // 
                        };
                    _viewModel.databaseManagercs[(int)Enum_DatabaseManager.MainWindow_Insert_EquipmentH].UpdateEquipment(updateQuery_Equipment, UpdateEquipment_parameters);


                    string updateQuery_Barcode = "UPDATE Barcode SET IncubationTime = @IncubationTime WHERE Barcode = @Barcode";
                    Dictionary<string, object> UpdateEBarcode_parameters = new Dictionary<string, object>
                        {
                            { "@IncubationTime", Incubationtime },
                            { "@Barcode", Barcodeid }  // 
                        };
                    _viewModel.databaseManagercs[(int)Enum_DatabaseManager.MainWindow_Insert_EquipmentH].UpdateBarcode(updateQuery_Barcode, UpdateEBarcode_parameters);


                    List<DatabaseManager_EquipmentH> Equipment = new List<DatabaseManager_EquipmentH>();
                    Equipment.Add(new DatabaseManager_EquipmentH { ID = ID, Barcode = Barcodeid, Qrcode = Qrcode, PcbADC = adcValue, PcbLED = LED, Temperature = Temperature, CreDate = cretime });
                    _viewModel.databaseManagercs[(int)Enum_DatabaseManager.MainWindow_Insert_EquipmentH].InsertEquipmentH(cretime, Equipment);
                    Incubationtime += 60;
                }
                else if (i == totalPoints - 1)
                {
                    unloadingTime = cretime.AddSeconds(1);



                    string UpdateBarcode_Query = "UPDATE Barcode SET " +
                   "Unloading = @Unloading, " +
                   "IncubationTime = @IncubationTime, " +
                   "Result = @Result " +
                   "WHERE Barcode = @Barcode";

                    Dictionary<string, object> UpdateBarcode_parameters = new Dictionary<string, object>
                        {

                            { "@Unloading", unloadingTime },
                            { "@Result", "Positive"},
                            { "@IncubationTime", Incubationtime },
                            { "@Barcode", barcodeid },
                        };
                    _viewModel.databaseManagercs[(int)Enum_DatabaseManager.언로딩].UpdateBarcode(UpdateBarcode_Query, UpdateBarcode_parameters);


                    string UpdateEquipment_Query = "UPDATE Equipment SET " +
                                         "Barcode = @Barcode, Qrcode = @Qrcode, Loading = @Loading, CreDate = @CreDate, PositiveTime = @PositiveTime, Result = @Result, IncubationTime = @IncubationTime, switched = @switched, isEnable = @isEnable " +
                                         "WHERE ID = @ID";
                    Dictionary<string, object> UpdateEquipment_parameters = new Dictionary<string, object>
                        {
                            { "@Barcode", null },
                            { "@Qrcode", null },
                            { "@Loading", null },
                            { "@CreDate", null },
                            { "@PositiveTime", null },
                            { "@Result", "Null" },
                            { "@IncubationTime", null },
                            { "@switched", false },
                            { "@isEnable", false },
                            { "@ID", ID }  // 
                        };
                    _viewModel.databaseManagercs[(int)Enum_DatabaseManager.언로딩].UpdateEquipment(UpdateEquipment_Query, UpdateEquipment_parameters);


                    List<DatabaseManager_EquipmentH> Equipment = new List<DatabaseManager_EquipmentH>();
                    Equipment.Add(new DatabaseManager_EquipmentH { ID = ID, Barcode = Barcodeid, Qrcode = Qrcode, PcbADC = adcValue, PcbLED = LED, Temperature = Temperature, CreDate = cretime });
                    _viewModel.databaseManagercs[(int)Enum_DatabaseManager.MainWindow_Insert_EquipmentH].InsertEquipmentH(cretime, Equipment);
                    Incubationtime += 60;
                }
                else
                {
                    cretime = cretime.AddMinutes(1);

                    string updateQuery_Equipment = "UPDATE Equipment SET IncubationTime = @IncubationTime WHERE ID = @ID";
                    Dictionary<string, object> UpdateEquipment_parameters = new Dictionary<string, object>
                        {
                            { "@IncubationTime", Incubationtime },
                            { "@ID", ID }  // 
                        };
                    _viewModel.databaseManagercs[(int)Enum_DatabaseManager.MainWindow_Insert_EquipmentH].UpdateEquipment(updateQuery_Equipment, UpdateEquipment_parameters);


                    string updateQuery_Barcode = "UPDATE Barcode SET IncubationTime = @IncubationTime WHERE Barcode = @Barcode";
                    Dictionary<string, object> UpdateEBarcode_parameters = new Dictionary<string, object>
                        {
                            { "@IncubationTime", Incubationtime },
                            { "@Barcode", Barcodeid }  // 
                        };
                    _viewModel.databaseManagercs[(int)Enum_DatabaseManager.MainWindow_Insert_EquipmentH].UpdateBarcode(updateQuery_Barcode, UpdateEBarcode_parameters);

                    List<DatabaseManager_EquipmentH> Equipment = new List<DatabaseManager_EquipmentH>();
                    Equipment.Add(new DatabaseManager_EquipmentH { ID = ID, Barcode = Barcodeid, Qrcode = Qrcode, PcbADC = adcValue, PcbLED = LED, Temperature = Temperature, CreDate = cretime });
                    _viewModel.databaseManagercs[(int)Enum_DatabaseManager.MainWindow_Insert_EquipmentH].InsertEquipmentH(cretime, Equipment);
                    Incubationtime += 60;
                }
            }

        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            PCB_LED(_viewModel.SystemInfo[0].PCB_ID1, 5, "Incubation");
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < _viewModel.LiveCharts_List.Count; i++)
            {
                if (ReceiveNewVoltageValue(_viewModel.LiveCharts_List[i].ID, _viewModel.LiveCharts_List[i].PcbADC, _viewModel.LiveCharts_List[i].CreDate))
                {

                }   
                else
                {

                }
            }

        }
    }
}
