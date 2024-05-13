﻿using HubCentra_A1.Model;
using LiveChartsCore;
using Microsoft.VisualBasic.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using static HubCentra_A1.EnumManager;
using static HubCentra_A1.Model.View;

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
            DatabaseManagerInitialize();
            PCBInitialize();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            MessageBoxResult result = System.Windows.MessageBox.Show(
        "Do you really want to exit?",
        "Exit Confirmation",
        MessageBoxButton.YesNo,
        MessageBoxImage.Question);

            if (result == MessageBoxResult.No)
            {
                e.Cancel = true;
                return;
            }
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
                    UpdateConfig_double(DataTransfer, _viewModel.Config[0].Temp);
                    break;
                case Enum_Config_ButtonEvent.doorOpenAlarmTrigger:
                    UpdateConfig_int(DataTransfer, _viewModel.Config[0].doorOpenAlarmTrigger);
                    break;
                case Enum_Config_ButtonEvent.MaximumTime:
                    UpdateConfig_int(DataTransfer, _viewModel.Config[0].MaximumTime);
                    break;
                case Enum_Config_ButtonEvent.UseBuzzer:
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

                default:
                    break;
            }
        }
        #endregion Config
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
                        if (_viewModel.Queue_PCB_Manual.Count > 0 && _viewModel.PCB_Status == false)
                        {
                            await Task.Delay(200, token);
                            PCB_PerformDataAcquisition_Manual();
                            await Task.Delay(200, token);
                        }
                        else
                        {
                            PCB_PerformDataAcquisition();
                            await Task.Delay(10, token);
                        }

                        break;


                    case EnumMStartWorkerThreads.select_Equipment:
                        if (_viewModel.DatabaseManager_Connection)
                        {
                            select_Equipment();
                        }
                        await Task.Delay(100, token);
                        break;

                    case EnumMStartWorkerThreads.PopStatus:

                        await Task.Delay(300, token);
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
            string updateQuery = "UPDATE Equipment SET Switched = @Switched WHERE isEnable = @isEnable";
            Dictionary<string, object> parameters = new Dictionary<string, object>
    {
        { "@Switched", false },
        { "@isEnable", true }
    };
            _viewModel.databaseManagercs[(int)Enum_DatabaseManager.MainWindow_MainEngine].UpdateEquipment(updateQuery, parameters);


            _viewModel.DatabaseManager_Connection = true;
        }



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

        #region Search
        public void select_Equipment_Search()
        {
            try
            {
                var select_Equipment = _viewModel.databaseManagercs[(int)Enum_DatabaseManager.MainWindow_select_Equipment_Search].Select_Equipment_Search();
                _viewModel.Search_List = select_Equipment;


            }
            catch (Exception ex)
            {

            }
        }
        #endregion Search
        #endregion DatabaseManager

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
                var select_Equipment = _viewModel.databaseManagercs[(int)Enum_DatabaseManager.MainWindow_select_Equipment_Search].Select_Barcode(_viewModel.MyDatePicke_Start, _viewModel.MyDatePicke_End);
                _viewModel.Report_List = select_Equipment;
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

                    Microsoft.Win32.SaveFileDialog saveFileDialog = new Microsoft.Win32.SaveFileDialog
                    {
                        Filter = "CSV file (*.csv)|*.csv",
                        DefaultExt = "csv",
                        AddExtension = true
                    };

                    if (saveFileDialog.ShowDialog() == true)
                    {


                        var sb = new StringBuilder();

                        // Add header line (adjust with your properties)
                        sb.AppendLine("Cell, Barcode, PCB Value, Temp Value, Creation Date");

                        foreach (var item in _viewModel.CSV_List)
                        {

                            sb.AppendLine($"{item.ID},{item.Barcode},{item.PcbADC},{item.Temperature}," +
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
                MessageBoxResult result = System.Windows.MessageBox.Show(
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

                var select_Equipment = _viewModel.databaseManagercs[(int)Enum_DatabaseManager.MainWindow_select_Equipment_Search].Select_Barcode(_viewModel.MyDatePicke_Start, _viewModel.MyDatePicke_End);

                if (select_Equipment.Count > 0 && (select_Equipment[0].Result == "Positive" || select_Equipment[0].Result == "Negative"))
                {
                    _viewModel.databaseManagercs[(int)Enum_DatabaseManager.MainWindow_select_Equipment_Search].DeleteRecords(BacodeID, start, end);
                    string UpdateEquipment_Query = "UPDATE Equipment SET " +
        "Barcode = @Barcode, Qrcode = @Qrcode, Loading = @Loading, CreDate = @CreDate, LoadCell = @LoadCell, Result = @Result, IncubationTime = @IncubationTime, isEnable = @isEnable " +
        "WHERE ID = @ID";
                    Dictionary<string, object> UpdateEquipment_parameters = new Dictionary<string, object>
                        {
                            { "@Barcode", null },
                            { "@Qrcode", null },
                            { "@Loading", null },
                            { "@CreDate", null },
                            { "@LoadCell", null },
                            { "@Result", "Null" },
                            { "@IncubationTime", null },
                            { "@isEnable", false },
                            { "@ID", select_Equipment[0].ID }  // 
                        };
                    _viewModel.databaseManagercs[(int)Enum_DatabaseManager.MainWindow_select_Equipment_Search].UpdateEquipment(UpdateEquipment_Query, UpdateEquipment_parameters);
                    ReportSerach();

                }
                else
                {
                }

            }
            catch (Exception e)
            {

            }
        }
        #endregion Report

        #region Config
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
                            _viewModel.Config[0].doorOpenAlarmTrigger = value;
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
                            break;
                        case Enum_Config_ButtonEvent.TrashCanFillLevel:
                            _viewModel.Config[0].TrashCanFillLevel = value;
                            break;
                        case Enum_Config_ButtonEvent.MaximumTime:
                            _viewModel.Config[0].MaximumTime = value;
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
                            _viewModel.Config[0].Temp = value;
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

            }
            _viewModel.databaseManagercs[(int)Enum_DatabaseManager.MainWindow_UpdateConfig].UpdateConfig(_viewModel.Config);
            var select_ConfigInfo = _viewModel.databaseManagercs[(int)Enum_DatabaseManager.MainWindow_UpdateConfig].Select_Config();
            _viewModel.Config = new List<Class_Config>(select_ConfigInfo);
        }
        #endregion Config

        #region LiveCharts
        public void LiveCharts()
        {
            try
            {
                if (_viewModel.Report_Barcode.Length > 0)
                {
                    var select_Equipment = _viewModel.databaseManagercs[(int)Enum_DatabaseManager.MainWindow_select_Equipment_Search].QueryDataForBarcodeAndDateRange(_viewModel.Report_Barcode, _viewModel.Report_DatePicke_Start, _viewModel.Report_DatePicke_End);
                    if (_viewModel.Report_Model == Enum_Report_Model.raw)
                    {
                        _viewModel.LiveCharts_List = select_Equipment;

                    }
                    else if (_viewModel.Report_Model == Enum_Report_Model.SMA)
                    {
                        //if (int.TryParse(txtvalue.Text, out int windowSize))
                        //{
                        //    _viewModel.LiveCharts_List = SmoothPcbValues(select_Equipment, windowSize);
                        //}
                        //else
                        //{
                        //    return null;
                        //}

                    }
                    else
                    {

                    }
                    LiveCharts_PositiveBounddury();

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
                int Analysis_Time_Range = _viewModel.Config[0].Analysis_Time_Range;
                int Number_of_Analysis_Intervals = _viewModel.Config[0].Analysis_Intervals;
                double Voltage_Increase_Threshold = _viewModel.Config[0].Threshold;
                _viewModel.LiveCharts_TimeSeries = new Dictionary<int, Queue<(DateTime, double)>>();
                if (_viewModel.LiveCharts_TimeSeries.ContainsKey(0))
                {
                    _viewModel.LiveCharts_TimeSeries[0].Clear();
                }
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
                    if (shouldStop)
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
                _viewModel.LiveCharts_Positive_Start = 0;
                _viewModel.LiveCharts_Positive_End = 0;
                _viewModel.LiveCharts_Positive_Time = "";
                int analysis_Time_Range = Analysis_Time_Range; ;
                int number_of_Analysis_Intervals = Number_of_Analysis_Intervals;
                double voltage_Increase_Threshold = Voltage_Increase_Threshold;
                double percentageDifference = 0;
                //double percentageThreshold = 1;
                //int windowSize = 180; // Size of rolling window in minutes
                //int numQuarters = 4; // Number of quarters to divide the window into
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
                int rage = (int)(analysis_Time_Range * 0.9);
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
                if (currentTime == new DateTime(2024, 02, 04, 04, 29, 48))
                {

                }
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
                    for (int i = 0; i < _viewModel.LiveCharts_List.Count; i++)
                    {
                        if (_viewModel.LiveCharts_List[i].CreDate == currentTime)
                        {
                            _viewModel.LiveCharts_Positive_Start = i;
                            _viewModel.LiveCharts_Positive_End = i + Math.Max(1, _viewModel.LiveCharts_List.Count / 200);
                            _viewModel.LiveCharts_Positive_Time = _viewModel.LiveCharts_List[i].CreDate.ToString();
                            break;
                        }
                    }
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        #endregion LiveCharts

        #region PCB
        public void PCBInitialize()
        {
            try
            {
                PCBData();
                PCBFrame();
                _viewModel.PCB_Connection = PCB_SerialPortOpen(_viewModel.SystemInfo[0].PCB_Serial);
                if (_viewModel.PCB_Connection)
                {
                    _viewModel.PCB_SerialPort.Open();
                }
            }
            catch (Exception ex)
            {

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

        public void PCBData()
        {
            try
            {
                var pcb = new List<PCB>();
                var cell_alive = new List<PCB_cell_alive_C>();
                var DataWithDB_presenceArray = new List<MatchEquipmentDataWithDB_C>();
                for (int i = 0; i < _viewModel.Common_CellCount * 3; i++)
                {
                    pcb.Add(new PCB { ADC = 0, LED = 0 }); // ADC와 LED 값을 초기화
                    cell_alive.Add(new PCB_cell_alive_C { alive = 0 }); // ADC와 LED 값을 초기화
                    DataWithDB_presenceArray.Add(new MatchEquipmentDataWithDB_C { alive = true });
                }


                for (int system = 0; system < 1; system++)
                {
                    _viewModel.PCB_CellReadings[system] = new Dictionary<int, List<List<double>>>();

                    for (int line = 0; line < 16; line++)
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
            for (int i = 11; i < 16; i++)
            {
                string LAMP = $"$ID1,LINE{i},LAMP,CHALL,ON";
                _viewModel.Queue_PCB_Manual.Enqueue(LAMP);
                string str = $"$ID1,LINE{i},DIMREAD";
                _viewModel.Queue_PCB_Manual.Enqueue(str);
            }
        }


        private void PCB_PerformDataAcquisition()
        {

            try
            {

                if (!_viewModel.PCB_SerialPort.IsOpen && !_viewModel.PCB_Connection && _viewModel.Queue_PCB_Manual.Count > 0)
                    return;

                for (int i = 11; i < 14; i++)
                {
                    var stopwatch = Stopwatch.StartNew();
                    string id =  _viewModel.SystemInfo[0].PCB_ID1;
                    string line = $"{id},LINE{i},ADCREAD";

                    string response = ReadSerialPortResponse(line, true);

                    if (response != null)
                    {
                        PCB_Auto_Data(response.ToString());
                        if (i == 15)
                        {
                            _viewModel.PCB_Status = true;
                        }
                    }
                    else
                    {

                    }
                    stopwatch.Stop();


                    Thread.Sleep(10);
                }

            }
            catch (Exception ex)
            {

            }
        }
        private void PCB_Auto_Data(string response)
        {
            var dataParts = response.Split(',');
            int startIndex = 0;

            if (dataParts.Length == 33)
            {
                startIndex = 5;
            }
            else if (dataParts.Length == 31)
            {
                startIndex = 3;
            }
            else
            {

                return;
            }

            ProcessDataParts(dataParts, startIndex);
        }
        private void ProcessDataParts(string[] dataParts, int startIndex)
        {
            int lineIndex = ExtractLineIndex2(dataParts[1]);
            for (int i = 0; i < 28; i++)
            {
                if (double.TryParse(dataParts[startIndex + i], out double cellValue))
                {
                    _viewModel.PCB_CellReadings[0][lineIndex][i].Add(cellValue);

                    if (_viewModel.PCB_CellReadings[0][lineIndex][i].Count == 5)
                    {
                        _viewModel.PCB_CellReadings[0][lineIndex][i].Sort();
                        double averageValue = _viewModel.PCB_CellReadings[0][lineIndex][i].Skip(1).Take(3).Average();

                        int pcbIndex = lineIndex * 28 + i;
                        if (pcbIndex < _viewModel.PCB_Data.Count)
                        {
                            //Dispatcher.Invoke(DispatcherPriority.Background, new Action(() =>
                            //{
                            //    _viewModel.PCB_Data[pcbIndex].ADC = (int)(averageValue * 1000);

                            //}));
                            _viewModel.PCB_Data[pcbIndex].ADC = (int)(averageValue * 1000);

                        }

                        _viewModel.PCB_CellReadings[0][lineIndex][i].Clear();
                    }
                }
            }
        }


        private void PCB_PerformDataAcquisition_Manual()
        {

            try
            {
                if (!_viewModel.PCB_SerialPort.IsOpen && !_viewModel.PCB_Connection)
                    return;
                if (_viewModel.Queue_PCB_Manual.Count > 0)
                {
                    if (_viewModel.Queue_PCB_Manual.TryDequeue(out string str))
                    {
                        int lineIndex = ExtractLineIndex(str);

                        string line = str + "\r\n";
                        //_viewModel.PCB_SerialPort.Write(str + "\r\n");
                        string response = ReadSerialPortResponse(line, false);
                        if (response != null)
                        {
                            PCB_Manual_Data(response, lineIndex);
                        }
                        else
                        {

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
            var dataParts = response.Split(',');
            var values = dataParts.Skip(5).Take(28).Select(val => double.TryParse(val, out double dVal) ? dVal : 0).ToList();
            int startIndex = lineIndex * 28;
            if (dataParts.Length == 33)
            {
                for (int i = 0; i < values.Count; i++)
                {
                    int pcbIndex = startIndex + i;
                    if (pcbIndex < _viewModel.PCB_Data.Count)
                    {
                        if (dataParts[4] == "ADCREAD")
                        {
                            int reverseIndex = values.Count - 1 - i;
                            _viewModel.PCB_Data[pcbIndex].ADC = values[i] * 1000;
                        }
                        else if (dataParts[4] == "DIMREAD")
                        {
                            int reverseIndex = values.Count - 1 - i;
                            _viewModel.PCB_Data[pcbIndex].LED = values[reverseIndex];
                        }
                    }
                }
            }
        }


        private string ReadSerialPortResponse(string Line, bool mode)
        {
            try
            {
                string line = Line;
                _viewModel.PCB_SerialPort.Write(line + "\r\n");
                if (mode == true)
                {
                    Thread.Sleep(100);
                }
                else
                {
                    Thread.Sleep(200);
                }

                var response = new StringBuilder();
                var stopwatch = Stopwatch.StartNew();
                int timeout = 300;
                while (stopwatch.ElapsedMilliseconds < timeout)
                {
                    if (_viewModel.PCB_SerialPort.BytesToRead > 100)
                    {
                        response.Append(_viewModel.PCB_SerialPort.ReadExisting());
                        ClearSerialPortBuffer_PCB(_viewModel.PCB_SerialPort);
                        return response.ToString();
                    }
                }
                ClearSerialPortBuffer_PCB(_viewModel.PCB_SerialPort);
                return null;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        private void ClearSerialPortBuffer_PCB(SerialPort serialPort)
        {
            if (serialPort.IsOpen)
            {
                serialPort.DiscardInBuffer();
                serialPort.DiscardOutBuffer();
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
            var lineNumberStart = response.IndexOf("LINE") + "LINE".Length;
            var lineNumberEnd = response.Length;

            var lineNumberStr = response.Substring(lineNumberStart, lineNumberEnd - lineNumberStart);

            if (int.TryParse(lineNumberStr, out int lineIndex))
            {
                return lineIndex;
            }

            return -1;
        }

        #endregion PCB
    }
}
