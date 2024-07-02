using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HubCentra_A1
{
   public class EnumManager
    {
        #region MainWindow
        public enum Enum_MainWindow_ButtonEvent
        {
            Home = 0,
            Search = 1,
            Report = 2,
            Conguration = 3,
            SystemRack1 = 4, SystemRack2 = 5, SystemRack3 = 6, SystemRack4 = 7,
            Login = 8, Logout = 9, Loading =10, ResetBarcode =11
        }
        public enum Enum_MainWindow_ButtonFlag
        {
            Home = 0,
            Search = 1,
            Report = 2,
            Conguration = 3,
            SystemRack1 = 4, SystemRack2 = 5, SystemRack3 = 6, SystemRack4 = 7,
        }

        public enum Enum_MainEngine_Statuslist
        {
            Positive,
            Negative,
            Incubation
        }
        #endregion MainWindow

        #region Threads
        public enum EnumMStartWorkerThreads
        {
            Loading = 0,
            MainEngine = 1,
            PCB = 2,
            select_Equipment = 3,
            Insert_EquipmentH = 4,
            FASTECH = 5,
            ModbusTCP = 8,
            Barcode = 10,
            Temperature = 11,
            LoadCell = 12,
            Result = 13,
            PopStatus = 14,
            로딩 = 15,
            언로딩 = 16,
            PositiveDelay = 17,
            Lamp =18,
            Tilting = 19,
            Calibration = 20,
        }
        #endregion Threads

        #region Login
        public enum Enum_Login
        {
            MASTER,
            ENGINEER,
            OPERATOR,

        }
        public enum Enum_Login_ButtonEvent
        {
            OK = 0,
            ADD = 1,
            ADD_CANCEL = 2,
            PASSWORD_SAVE = 3,
            PASSWORD_CANCEL = 4,
            ADD_OPERATOR =5,
            ADD_ENGINEER = 6,
            Delete =7,
        }
        #endregion Login

        #region DatabaseManager
        public enum Enum_DatabaseManager
        {
            Common = 0,
            MainWindow_MainEngine = 1,
            MainWindow_select_Equipment = 2,
            MainWindow_select_Equipment_Search = 3,
            MainWindow_UpdateFASTECHInfo_int = 4,
            MainWindow_UpdateConfig = 5,
            MainWindow_Insert_EquipmentH = 6,
            MainWindow_Result = 7,
            MainWindow_Update_ETC = 8,
            MainWindow_searchBarcodeDuplicates=9,
            로딩 =10,
            언로딩 = 11,
            Login = 12,
        }
        #endregion DatabaseManager

        #region Config
        public enum Enum_Config_ButtonEvent
        {
            Temp = 0,
            doorOpenAlarmTrigger = 1,
            MaximumTime = 2,
            UseBuzzer = 3,
            Positive_Wait = 4,
            Positive_Low = 5,
            Positive_High = 6,
            Analysis_Time_Range = 7,
            Analysis_Intervals = 8,
            Threshold = 9,
            BottleExistenceRange = 10,
            DataStorageSave = 11,
            TrashCanFillLevel = 12,
            LoadCellMin = 20,
            LoadCellMax = 21,
            Block_OK = 25,
            Block_Cancel = 26,
            Block_Data = 27,
            Calibration_From = 28,
            Calibration_To = 29,
            Calibration_Start = 30,
            SYSTEM1 =31,
            SYSTEM2 = 32,
            SYSTEM3 = 33,
            SYSTEM4 = 34,
            Calibration_Target = 35,

            Equipment_Change = 36,
            NegativeOnOFF = 37,
        }
        #endregion Config

        #region Search
        public enum Enum_Search_ButtonEvent
        {
            Search = 0,
        }
        #endregion Search

        #region Report
        public enum Enum_Report_ButtonEvent
        {
            Search = 0,
            CSV = 1,
            Chart = 2,
            Delete = 3,
        }
        public enum Enum_Report_Model
        {
            raw,
            SMA,
            NLGM
        }
        #endregion Report

        #region WriteBarcode
        public enum Enum_WriteBarcode_ButtonEvent
        {
            OK = 0,
            CANCEL = 1,

        }
        #endregion WriteBarcode

        #region PopStatus

        public enum Enum_PopStatus_ButtonEvent
        {
            PopStatus_Error_Index = 0,
            PopStatus_Positive_Index = 1,
            PopStatus_TrashCan = 2,
            PopStatus_Positive = 3,
        }
        #endregion PopStatus

        #region FASTECH
        public enum Enum_FASTECH_ID
        {
            IO = 1,
        }
        public enum Enum_FASTECH_Input
        {
            Door = 0,
            Trigger = 1,
        }

        public enum Enum_FASTECH_Output
        {
            RedLamp = 0,
            GreenLamp = 1,
            YellowLamp = 2,
            Tiling = 3,
            Buzzer = 4,
        }
        #endregion FASTECH

        #region PCB
        public enum Enum_PCB
        {
            ADC = 0,
            LED = 1,
            TLED = 1,
        }
        #endregion PCB

        #region LOG
        public enum Enum_LOG
        {
            PCB = 0,
            LED = 1,
            TLED = 1,
        }
        #endregion LOG
    }
}
