using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static HubCentra_A1.Model.View;
using static System.Net.Mime.MediaTypeNames;

namespace HubCentra_A1
{
    public class DatabaseManager
    {
        #region Initialize
        private string baseConnectionString;
        private string databaseName = "BloodCultureA1";
        private string connectionString;

        public DatabaseManager()
        {
            databaseName = databaseName;
            baseConnectionString = "Server=127.0.0.1; User ID=sa; Password=12345678; TrustServerCertificate=True;";
            SetConnectionString(databaseName);
        }
        #endregion Initialize

        #region Connection
        private void SetConnectionString(string dbName)
        {
            if (string.IsNullOrEmpty(dbName))
            {
                // Connect without specifying a database
                connectionString = baseConnectionString;
            }
            else
            {
                // Connect to the specified database
                connectionString = $"{baseConnectionString}; Database={dbName};";
            }
        }
        #endregion Connection

        #region Create
        #region Database
        public void CreateDatabase(string databaseName)
        {
            string checkDatabaseExistsQuery = $"SELECT database_id FROM sys.databases WHERE Name = @databaseName";
            SetConnectionString(""); // Connect without specifying a database
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    using (SqlCommand checkCommand = new SqlCommand(checkDatabaseExistsQuery, connection))
                    {
                        checkCommand.Parameters.AddWithValue("@databaseName", databaseName);
                        object result = checkCommand.ExecuteScalar();

                        if (result == null || result == DBNull.Value)
                        {
                            string createDatabaseQuery = $"CREATE DATABASE [{databaseName}]";
                            using (SqlCommand command = new SqlCommand(createDatabaseQuery, connection))
                            {
                                command.ExecuteNonQuery();
                            }
                        }
                    }
                }
                SetConnectionString(databaseName); // Reset connection string to connect to the specific database
            }
            catch (Exception ex)
            {
                // Handle or log the exception as needed
                Console.WriteLine($"Error creating database: {ex.Message}");
            }
        }
        #endregion Database

        #region Table
        public void CreateTable()
        {
            try
            {
                CreateDatabase(databaseName);
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string TableNames_System = "System";
                    if (!CheckIfTableExists(connection, TableNames_System))
                    {
                        CreateTable_Syetem(connection, TableNames_System);
                    }
                    string TableNames_FASTECH = "FASTECH";
                    if (!CheckIfTableExists(connection, TableNames_FASTECH))
                    {
                        CreateTable_FASTECH(connection, TableNames_FASTECH);
                    }
                    string TableNames_Config = "Config";
                    if (!CheckIfTableExists(connection, TableNames_Config))
                    {
                        CreateTable_Config(connection, TableNames_Config);
                    }
                    string TableNames_Position = "Position";
                    if (!CheckIfTableExists(connection, TableNames_Position))
                    {
                        CreateTable_Position(connection, TableNames_Position);
                    }

                    string TableNames_Equipment = "Equipment";
                    if (!CheckIfTableExists(connection, TableNames_Equipment))
                    {
                        CreateTable_Equipment(connection, TableNames_Equipment);
                    }

                    string TableNames_Barcode = "Barcode";
                    if (!CheckIfTableExists(connection, TableNames_Barcode))
                    {
                        CreateTable_Barcode(connection, TableNames_Barcode);
                    }

                    string TableNames_Login = "Login";
                    if (!CheckIfTableExists(connection, TableNames_Login))
                    {
                        CreateTable_Login(connection, TableNames_Login);
                    }

                    string TableNames_ETC = "ETC";
                    if (!CheckIfTableExists(connection, TableNames_ETC))
                    {
                        CreateTable_ETC(connection, TableNames_ETC);
                    }
                }
            }
            catch (SqlException ex)
            {

            }
            catch (Exception ex)
            {

            }
        }
        #endregion Table  
        #endregion Create

        #region TableCheck
        private bool TableExists(SqlConnection connection, string tableName)
        {
            using (SqlCommand command = new SqlCommand(
                           $"SELECT CASE WHEN EXISTS (" +
                           $"SELECT * FROM INFORMATION_SCHEMA.TABLES " +
                           $"WHERE TABLE_NAME = '{tableName}') " +
                           $"THEN 1 ELSE 0 END", connection))
            {
                return (int)command.ExecuteScalar() == 1;
            }
        }
        private bool CheckIfTableExists(SqlConnection connection, string tableName)
        {
            using (SqlCommand command = new SqlCommand(
                                 "SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = @tableName", connection))
            {
                command.Parameters.AddWithValue("@tableName", tableName);
                int count = (int)command.ExecuteScalar();
                return count > 0;
            }
        }
        #endregion TableCheck

        #region Table Initialize
        #region Syetem
        private void CreateTable_Syetem(SqlConnection connection, string tableName)
        {
            try
            {
                using (SqlCommand command = new SqlCommand(
           $"CREATE TABLE {tableName} (" +
           "FASTECH_Servo_IP VARCHAR(255) NULL, " +
           "FASTECH_IO_Input_IP VARCHAR(255) NULL, " +
           "FASTECH_IO_Output_IP VARCHAR(255) NULL, " +
           "Schneider_Robot_IP VARCHAR(255) NULL, " +
           "Schneider_Robot_Port INT NULL, " +
           "PCB_ID1 VARCHAR(255) NULL, " +
           "PCB_ID2 VARCHAR(255) NULL, " +
           "PCB_ID3 VARCHAR(255) NULL, " +
           "PCB_ID4 VARCHAR(255) NULL, " +
           "PCB_Serial VARCHAR(255) NULL, " +
           "Temperature_Serial VARCHAR(255) NULL, " +
           "Barcode_Serial VARCHAR(255) NULL, " +
           "Loadcell_Serial VARCHAR(255) NULL, " +
           "Spare1 VARCHAR(255) NULL, " +
           "Spare2 VARCHAR(255) NULL)", // Change data type to BIT for isActive
           connection))
                {
                    command.ExecuteNonQuery();
                }

                // Populate the 'id' column with values from 1 to 84
                using (SqlCommand populateCommand = new SqlCommand(
                    $"INSERT INTO {tableName} (FASTECH_Servo_IP, FASTECH_IO_Input_IP, FASTECH_IO_Output_IP, Schneider_Robot_IP, Schneider_Robot_Port, PCB_ID1, PCB_ID2, PCB_ID3, PCB_ID4, PCB_Serial, Temperature_Serial, Barcode_Serial, Loadcell_Serial, Spare1, Spare2)" +
                    $" VALUES (@FASTECH_Servo_IP, @FASTECH_IO_Input_IP, @FASTECH_IO_Output_IP, @Schneider_Robot_IP, @Schneider_Robot_Port, @PCB_ID1, @PCB_ID2, @PCB_ID3, @PCB_ID4, @PCB_Serial, @Temperature_Serial, @Barcode_Serial, @Loadcell_Serial, @Spare1, @Spare2)",
                    connection))
                {
                    populateCommand.Parameters.Add(new SqlParameter("@FASTECH_Servo_IP", SqlDbType.VarChar));
                    populateCommand.Parameters.Add(new SqlParameter("@FASTECH_IO_Input_IP", SqlDbType.VarChar));
                    populateCommand.Parameters.Add(new SqlParameter("@FASTECH_IO_Output_IP", SqlDbType.VarChar));
                    populateCommand.Parameters.Add(new SqlParameter("@Schneider_Robot_IP", SqlDbType.VarChar));
                    populateCommand.Parameters.Add(new SqlParameter("@Schneider_Robot_Port", SqlDbType.Int));
                    populateCommand.Parameters.Add(new SqlParameter("@PCB_ID1", SqlDbType.VarChar));
                    populateCommand.Parameters.Add(new SqlParameter("@PCB_ID2", SqlDbType.VarChar));
                    populateCommand.Parameters.Add(new SqlParameter("@PCB_ID3", SqlDbType.VarChar));
                    populateCommand.Parameters.Add(new SqlParameter("@PCB_ID4", SqlDbType.VarChar));
                    populateCommand.Parameters.Add(new SqlParameter("@PCB_Serial", SqlDbType.VarChar));
                    populateCommand.Parameters.Add(new SqlParameter("@Temperature_Serial", SqlDbType.VarChar));
                    populateCommand.Parameters.Add(new SqlParameter("@Barcode_Serial", SqlDbType.VarChar));
                    populateCommand.Parameters.Add(new SqlParameter("@Loadcell_Serial", SqlDbType.VarChar));
                    populateCommand.Parameters.Add(new SqlParameter("@Spare1", SqlDbType.VarChar));
                    populateCommand.Parameters.Add(new SqlParameter("@Spare2", SqlDbType.VarChar));


                    int totalCells = 1;

                    for (int i = 0; i < totalCells; i++)
                    {
                        populateCommand.Parameters["@FASTECH_Servo_IP"].Value = "192.168.0.1";
                        populateCommand.Parameters["@FASTECH_IO_Input_IP"].Value = "192.168.0.2";
                        populateCommand.Parameters["@FASTECH_IO_Output_IP"].Value = "192.168.0.3";
                        populateCommand.Parameters["@Schneider_Robot_IP"].Value = "192.168.0.4";
                        populateCommand.Parameters["@Schneider_Robot_Port"].Value = 6502;
                        populateCommand.Parameters["@PCB_ID1"].Value = "$ID1";
                        populateCommand.Parameters["@PCB_ID2"].Value = "$ID2";
                        populateCommand.Parameters["@PCB_ID3"].Value = "$ID3";
                        populateCommand.Parameters["@PCB_ID4"].Value = "$ID4";
                        populateCommand.Parameters["@PCB_Serial"].Value = "COM2";
                        populateCommand.Parameters["@Temperature_Serial"].Value = "COM3";
                        populateCommand.Parameters["@Barcode_Serial"].Value = "COM7";
                        populateCommand.Parameters["@Loadcell_Serial"].Value = "COM3";
                        populateCommand.Parameters["@Spare1"].Value = "";
                        populateCommand.Parameters["@Spare2"].Value = "";
                        populateCommand.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
        #endregion Syetem

        #region Config
        private void CreateTable_Config(SqlConnection connection, string tableName)
        {
            try
            {
                using (SqlCommand command = new SqlCommand(
           $"CREATE TABLE {tableName} (" +
           "ID INT PRIMARY KEY, " +
           "Temp FLOAT NULL, " +
           "doorOpenAlarmTrigger INT NULL, " +
           "LoadCellMin FLOAT NULL, " +
           "LoadCellMax FLOAT NULL, " +
           "MaximumTime INT NULL, " +
           "UseBuzzer BIT NULL, " +
           "Positive_Wait INT NULL, " +
           "Positive_Low INT NULL, " +
           "Positive_High INT NULL, " +
           "Analysis_Time_Range INT NULL, " +
           "Analysis_Intervals INT NULL, " +
           "Threshold FLOAT NULL, " +
           "BottleExistenceRange INT NULL, " +
           "DataStorageSave INT NULL, " +
           "TrashCanFillLevel  INT NULL, " +
           "SYSTEM1 BIT NULL, " +
           "SYSTEM2 BIT NULL, " +
           "SYSTEM3 BIT NULL, " +
           "SYSTEM4 BIT NULL, " +
           "Spare1 VARCHAR(255) NULL, " +
           "Spare2 VARCHAR(255) NULL, " +
           "Spare3 VARCHAR(255) NULL, " +
           "Spare4 VARCHAR(255) NULL, " +
           "Spare5 VARCHAR(255) NULL)", 
           connection))
                {
                    command.ExecuteNonQuery();
                }
                using (SqlCommand populateCommand = new SqlCommand(
                    $"INSERT INTO {tableName} (ID, Temp, doorOpenAlarmTrigger, LoadCellMin, LoadCellMax, MaximumTime,  UseBuzzer, Positive_Wait, Positive_Low, Positive_High, Analysis_Time_Range, Analysis_Intervals, Threshold, BottleExistenceRange, DataStorageSave, TrashCanFillLevel, SYSTEM1, SYSTEM2, SYSTEM3, SYSTEM4, Spare1, Spare2, Spare3, Spare4, Spare5)" +
                    $" VALUES (@ID, @Temp, @doorOpenAlarmTrigger, @LoadCellMin, @LoadCellMax, @MaximumTime,  @UseBuzzer, @Positive_Wait, @Positive_Low, @Positive_High, @Analysis_Time_Range, @Analysis_Intervals, @Threshold, @BottleExistenceRange, @DataStorageSave, @TrashCanFillLevel, @SYSTEM1, @SYSTEM2, @SYSTEM3, @SYSTEM4, @Spare1, @Spare2, @Spare3, @Spare4, @Spare5)",
                    connection))
                {
                    populateCommand.Parameters.Add(new SqlParameter("@ID", SqlDbType.Int));
                    populateCommand.Parameters.Add(new SqlParameter("@Temp", SqlDbType.Float));
                    populateCommand.Parameters.Add(new SqlParameter("@doorOpenAlarmTrigger", SqlDbType.Int));
                    populateCommand.Parameters.Add(new SqlParameter("@LoadCellMin", SqlDbType.Float));
                    populateCommand.Parameters.Add(new SqlParameter("@LoadCellMax", SqlDbType.Float));
                    populateCommand.Parameters.Add(new SqlParameter("@MaximumTime", SqlDbType.Int));
                    populateCommand.Parameters.Add(new SqlParameter("@UseBuzzer", SqlDbType.Bit));
                    populateCommand.Parameters.Add(new SqlParameter("@Positive_Wait", SqlDbType.Int));
                    populateCommand.Parameters.Add(new SqlParameter("@Positive_Low", SqlDbType.Int));
                    populateCommand.Parameters.Add(new SqlParameter("@Positive_High", SqlDbType.Int));
                    populateCommand.Parameters.Add(new SqlParameter("@Analysis_Time_Range", SqlDbType.Int));
                    populateCommand.Parameters.Add(new SqlParameter("@Analysis_Intervals", SqlDbType.Int));
                    populateCommand.Parameters.Add(new SqlParameter("@Threshold", SqlDbType.Float));
                    populateCommand.Parameters.Add(new SqlParameter("@BottleExistenceRange", SqlDbType.Int));
                    populateCommand.Parameters.Add(new SqlParameter("@DataStorageSave", SqlDbType.Int));
                    populateCommand.Parameters.Add(new SqlParameter("@TrashCanFillLevel", SqlDbType.Int));
                    populateCommand.Parameters.Add(new SqlParameter("@SYSTEM1", SqlDbType.Bit));
                    populateCommand.Parameters.Add(new SqlParameter("@SYSTEM2", SqlDbType.Bit));
                    populateCommand.Parameters.Add(new SqlParameter("@SYSTEM3", SqlDbType.Bit));
                    populateCommand.Parameters.Add(new SqlParameter("@SYSTEM4", SqlDbType.Bit));
                    populateCommand.Parameters.Add(new SqlParameter("@Spare1", SqlDbType.VarChar));
                    populateCommand.Parameters.Add(new SqlParameter("@Spare2", SqlDbType.VarChar));
                    populateCommand.Parameters.Add(new SqlParameter("@Spare3", SqlDbType.VarChar));
                    populateCommand.Parameters.Add(new SqlParameter("@Spare4", SqlDbType.VarChar));
                    populateCommand.Parameters.Add(new SqlParameter("@Spare5", SqlDbType.VarChar));


                    int totalCells = 1;

                    for (int i = 0; i < totalCells; i++)
                    {
                        populateCommand.Parameters["@ID"].Value = i;
                        populateCommand.Parameters["@Temp"].Value = 36.5;
                        populateCommand.Parameters["@doorOpenAlarmTrigger"].Value = 10;
                        populateCommand.Parameters["@LoadCellMin"].Value = 20;
                        populateCommand.Parameters["@LoadCellMax"].Value = 200;
                        populateCommand.Parameters["@MaximumTime"].Value = 10;
                        populateCommand.Parameters["@UseBuzzer"].Value = 1;
                        populateCommand.Parameters["@Positive_Wait"].Value = 5;
                        populateCommand.Parameters["@Positive_Low"].Value = 300;
                        populateCommand.Parameters["@Positive_High"].Value = 400;
                        populateCommand.Parameters["@Analysis_Time_Range"].Value = 180;
                        populateCommand.Parameters["@Analysis_Intervals"].Value = 4;
                        populateCommand.Parameters["@Threshold"].Value = 1.2;
                        populateCommand.Parameters["@BottleExistenceRange"].Value = 700;
                        populateCommand.Parameters["@DataStorageSave"].Value = 1;
                        populateCommand.Parameters["@TrashCanFillLevel"].Value = 10;
                        populateCommand.Parameters["@SYSTEM1"].Value = 1;
                        populateCommand.Parameters["@SYSTEM2"].Value = 0;
                        populateCommand.Parameters["@SYSTEM3"].Value = 0;
                        populateCommand.Parameters["@SYSTEM4"].Value = 0;
                        populateCommand.Parameters["@Spare1"].Value = "";
                        populateCommand.Parameters["@Spare2"].Value = "";
                        populateCommand.Parameters["@Spare3"].Value = "";
                        populateCommand.Parameters["@Spare4"].Value = "";
                        populateCommand.Parameters["@Spare5"].Value = "";
                        populateCommand.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
        #endregion Config

        #region FASTECH
        private void CreateTable_FASTECH(SqlConnection connection, string tableName)
        {
            try
            {
                using (SqlCommand command = new SqlCommand(
           $"CREATE TABLE {tableName} (" +
           "ID INT PRIMARY KEY, " +
           "Axis_Acc_Time INT NULL, " +
           "Axis_Dec_Time INT NULL, " +
           "Axis_Speed INT NULL, " +
           "Jog_Speed_Low INT NULL, " +
           "Jog_Speed_Middle INT NULL, " +
           "Jog_Speed_High INT NULL, " +
           "Jog_Acc_Dec_Time INT NULL, " +
           "Org_Speed INT NULL, " +
           "Org_Search_Speed INT NULL, " +
           "Org_Acc_Dec_Time INT NULL, " +
           "Org_Dethod INT NULL, " +
           "Org_Dir INT NULL, " +
           "Org_Offset INT NULL, " +
           "Motion_Dir INT NULL, " +
           "Spare1 VARCHAR(255) NULL, " +
           "Spare2 VARCHAR(255) NULL, " +
           "Spare3 VARCHAR(255) NULL, " +
           "Spare4 VARCHAR(255) NULL, " +
           "Spare5 VARCHAR(255) NULL)", // Change data type to BIT for isActive
           connection))
                {
                    command.ExecuteNonQuery();
                }

                // Populate the 'id' column with values from 1 to 84
                using (SqlCommand populateCommand = new SqlCommand(
                    $"INSERT INTO {tableName} (ID, Axis_Acc_Time, Axis_Dec_Time, Axis_Speed, Jog_Speed_Low, Jog_Speed_Middle, Jog_Speed_High, Jog_Acc_Dec_Time, Org_Speed, Org_Search_Speed, Org_Acc_Dec_Time, Org_Dethod, Org_Dir,Org_Offset, Motion_Dir, Spare1, Spare2, Spare3, Spare4, Spare5)" +
                    $" VALUES (@ID, @Axis_Acc_Time, @Axis_Dec_Time, @Axis_Speed, @Jog_Speed_Low, @Jog_Speed_Middle, @Jog_Speed_High, @Jog_Acc_Dec_Time, @Org_Speed, @Org_Search_Speed, @Org_Acc_Dec_Time, @Org_Dethod, @Org_Dir, @Org_Offset, @Motion_Dir, @Spare1, @Spare2, @Spare3, @Spare4, @Spare5)",
                    connection))
                {
                    populateCommand.Parameters.Add(new SqlParameter("@ID", SqlDbType.Int));
                    populateCommand.Parameters.Add(new SqlParameter("@Axis_Acc_Time", SqlDbType.Int));
                    populateCommand.Parameters.Add(new SqlParameter("@Axis_Dec_Time", SqlDbType.Int));
                    populateCommand.Parameters.Add(new SqlParameter("@Axis_Speed", SqlDbType.Int));
                    populateCommand.Parameters.Add(new SqlParameter("@Jog_Speed_Low", SqlDbType.Int));
                    populateCommand.Parameters.Add(new SqlParameter("@Jog_Speed_Middle", SqlDbType.Int));
                    populateCommand.Parameters.Add(new SqlParameter("@Jog_Speed_High", SqlDbType.Int));
                    populateCommand.Parameters.Add(new SqlParameter("@Jog_Acc_Dec_Time", SqlDbType.Int));
                    populateCommand.Parameters.Add(new SqlParameter("@Org_Speed", SqlDbType.Int));
                    populateCommand.Parameters.Add(new SqlParameter("@Org_Search_Speed", SqlDbType.Int));
                    populateCommand.Parameters.Add(new SqlParameter("@Org_Acc_Dec_Time", SqlDbType.Int));
                    populateCommand.Parameters.Add(new SqlParameter("@Org_Dethod", SqlDbType.Int));
                    populateCommand.Parameters.Add(new SqlParameter("@Org_Dir", SqlDbType.Int));
                    populateCommand.Parameters.Add(new SqlParameter("@Org_Offset", SqlDbType.Int));
                    populateCommand.Parameters.Add(new SqlParameter("@Motion_Dir", SqlDbType.Int));
                    populateCommand.Parameters.Add(new SqlParameter("@Spare1", SqlDbType.VarChar));
                    populateCommand.Parameters.Add(new SqlParameter("@Spare2", SqlDbType.VarChar));
                    populateCommand.Parameters.Add(new SqlParameter("@Spare3", SqlDbType.VarChar));
                    populateCommand.Parameters.Add(new SqlParameter("@Spare4", SqlDbType.VarChar));
                    populateCommand.Parameters.Add(new SqlParameter("@Spare5", SqlDbType.VarChar));


                    int totalCells = 1;

                    for (int i = 0; i < totalCells; i++)
                    {
                        populateCommand.Parameters["@ID"].Value = i;
                        populateCommand.Parameters["@Axis_Acc_Time"].Value = 200;
                        populateCommand.Parameters["@Axis_Dec_Time"].Value = 200;
                        populateCommand.Parameters["@Axis_Speed"].Value = 10000;
                        populateCommand.Parameters["@Jog_Speed_Low"].Value = 1000;
                        populateCommand.Parameters["@Jog_Speed_Middle"].Value = 5000;
                        populateCommand.Parameters["@Jog_Speed_High"].Value = 10000;
                        populateCommand.Parameters["@Jog_Acc_Dec_Time"].Value = 100;
                        populateCommand.Parameters["@Org_Speed"].Value = 3000;
                        populateCommand.Parameters["@Org_Search_Speed"].Value = 1000;
                        populateCommand.Parameters["@Org_Acc_Dec_Time"].Value = 50;
                        populateCommand.Parameters["@Org_Dethod"].Value = 1;
                        populateCommand.Parameters["@Org_Dir"].Value = 1;
                        populateCommand.Parameters["@Org_Offset"].Value = 0;
                        populateCommand.Parameters["@Motion_Dir"].Value = 0;
                        populateCommand.Parameters["@Spare1"].Value = "";
                        populateCommand.Parameters["@Spare2"].Value = "";
                        populateCommand.Parameters["@Spare3"].Value = "";
                        populateCommand.Parameters["@Spare4"].Value = "";
                        populateCommand.Parameters["@Spare5"].Value = "";
                        populateCommand.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
        #endregion FASTECH31

        #region Equipment
        private void CreateTable_Equipment(SqlConnection connection, string tableName)
        {
            try
            {
                using (SqlCommand command = new SqlCommand(
           $"CREATE TABLE {tableName} (" +
           "ID INT PRIMARY KEY, " +
           "Cell VARCHAR(255) NULL, " +
           "Barcode VARCHAR(255) NULL, " +
           "Qrcode VARCHAR(255) NULL, " +
           "Loading DATETIME2 NULL, " +
           "CreDate DATETIME2 NULL, " +
           "PositiveTime DATETIME2 NULL, " +
           "IncubationTime INT NULL, " +
           "Result VARCHAR(255) NULL, " +
           "switched BIT NULL, " +
           "isEnable BIT NULL, " +
           "isActive BIT NULL)",
           connection))
                {
                    command.ExecuteNonQuery();
                }

                using (SqlCommand populateCommand = new SqlCommand(
                  $"INSERT INTO {tableName} (ID, Cell, Result, Switched,isEnable, isActive) VALUES (@ID, @Cell, @Result, @Switched, @isEnable, @isActive)", connection))
                {
                    populateCommand.Parameters.Add(new SqlParameter("@ID", SqlDbType.Int));
                    populateCommand.Parameters.Add(new SqlParameter("@Cell", SqlDbType.VarChar));
                    populateCommand.Parameters.Add(new SqlParameter("@Result", SqlDbType.VarChar));
                    populateCommand.Parameters.Add(new SqlParameter("@Switched", SqlDbType.Bit));
                    populateCommand.Parameters.Add(new SqlParameter("@isEnable", SqlDbType.Bit));
                    populateCommand.Parameters.Add(new SqlParameter("@isActive", SqlDbType.Bit));
                    int CellCount = 28 * 12;
                    int cellsPerRack = 28;
                    for (int i = 1; i < CellCount + 1; i++)
                    {
                        int rackNumber = (i - 1) / cellsPerRack + 1;
                        int positionInRack = (i - 1) % cellsPerRack + 1;
                        string cellValue = $"{rackNumber}-{positionInRack}";

                        populateCommand.Parameters["@ID"].Value = i;
                        populateCommand.Parameters["@Cell"].Value = cellValue;
                        populateCommand.Parameters["@Result"].Value = "Null";
                        populateCommand.Parameters["@Switched"].Value = false;
                        populateCommand.Parameters["@isEnable"].Value = false;
                        populateCommand.Parameters["@isActive"].Value = true;
                        populateCommand.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }


        public void CreateTable_EquipmentH(DateTime date)
        {
            try
            {
                string tableName = $"H_{date:yy_MM}";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    if (!TableExists(connection, tableName))
                    {
                        using (SqlCommand command = new SqlCommand(
                                       $"CREATE TABLE {tableName} (" +
                                       "ID INT NULL, " +
                                       "Barcode VARCHAR(255) NULL, " +
                                       "Qrcode VARCHAR(255) NULL, " +
                                       "PcbADC FLOAT NULL, " +
                                       "PcbLED FLOAT NULL, " +
                                       "Temperature FLOAT NULL, " +
                                       "CreDate DATETIME2 NULL)",
                                       connection))
                        {
                            command.ExecuteNonQuery();
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
        #endregion Equipment

        #region Login
        private void CreateTable_Login(SqlConnection connection, string tableName)
        {
            try
            {
                using (SqlCommand command = new SqlCommand(
           $"CREATE TABLE {tableName} (" +
           "User_Id VARCHAR(255) PRIMARY KEY, " +
           "User_Level VARCHAR(255) NULL, " +
           "User_Password VARCHAR(255) NULL, " +
           "User_Enable BIT NULL)",
           connection))
                {
                    command.ExecuteNonQuery();
                }

                // Populate the 'id' column with values from 1 to 84
                using (SqlCommand populateCommand = new SqlCommand(
                    $"INSERT INTO {tableName} (User_Id, User_Level, User_Password, User_Enable)" +
                    $" VALUES (@User_Id, @User_Level, @User_Password, @User_Enable)",
                    connection))
                {
                    populateCommand.Parameters.Add(new SqlParameter("@User_Id", SqlDbType.VarChar));
                    populateCommand.Parameters.Add(new SqlParameter("@User_Level", SqlDbType.VarChar));
                    populateCommand.Parameters.Add(new SqlParameter("@User_Password", SqlDbType.VarChar));
                    populateCommand.Parameters.Add(new SqlParameter("@User_Enable", SqlDbType.Bit));


                    int totalCells = 1;

                    for (int i = 0; i < totalCells; i++)
                    {
                        populateCommand.Parameters["@User_Id"].Value = "UserMASTER";
                        populateCommand.Parameters["@User_Level"].Value = "MASTER";
                        populateCommand.Parameters["@User_Password"].Value = "1234";
                        populateCommand.Parameters["@User_Enable"].Value = false;
                        populateCommand.ExecuteNonQuery();
                        populateCommand.Parameters["@User_Id"].Value = "UserENGINEER";
                        populateCommand.Parameters["@User_Level"].Value = "ENGINEER";
                        populateCommand.Parameters["@User_Password"].Value = "1234";
                        populateCommand.Parameters["@User_Enable"].Value = false;
                        populateCommand.ExecuteNonQuery();
                        populateCommand.Parameters["@User_Id"].Value = "UserOPERATOR";
                        populateCommand.Parameters["@User_Level"].Value = "OPERATOR";
                        populateCommand.Parameters["@User_Password"].Value = "1234";
                        populateCommand.Parameters["@User_Enable"].Value = true;
                        populateCommand.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
        #endregion Login

        #region Barcode
        private void CreateTable_Barcode(SqlConnection connection, string tableName)
        {
            try
            {
                using (SqlCommand command = new SqlCommand(
             $"CREATE TABLE {tableName} (" +
             "ID INT NULL, " +
             "Barcode VARCHAR(255) PRIMARY KEY, " +
             "Qrcode VARCHAR(255) NULL, " +
             "LoadCell FLOAT NULL, " +
             "Loading DATETIME2 NULL, " +
             "Unloading DATETIME2 NULL, " +
             "IncubationTime INT NULL, " +
             "Result VARCHAR(255) NULL, " +
             "PositiveTime DATETIME2 NULL)",

             connection))
                {
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception e)
            {

            }
        }
        #endregion Barcode

        #region Position
        private void CreateTable_Position(SqlConnection connection, string tableName)
        {
            try
            {
                using (SqlCommand command = new SqlCommand(
           $"CREATE TABLE {tableName} (" +
           "ID INT PRIMARY KEY, " +
           "Standby FLOAT NULL, " +
           "Loading FLOAT NULL, " +
           "LoadCell FLOAT NULL, " +
           "Barcode FLOAT NULL, " +
           "UnLoading FLOAT NULL, " +
           "Positive FLOAT NULL, " +
           "Error FLOAT NULL, " +
           "Rack1 FLOAT NULL, " +
           "Rack2 FLOAT NULL, " +
           "Rack3 FLOAT NULL, " +
           "Rack4 FLOAT NULL, " +
           "Rack5 FLOAT NULL, " +
           "Rack6 FLOAT NULL, " +
           "Rack7 FLOAT NULL, " +
           "Rack8 FLOAT NULL, " +
           "Rack9 FLOAT NULL, " +
           "Rack10 FLOAT NULL, " +
           "Rack11 FLOAT NULL, " +
           "Rack12 FLOAT NULL, " +
           "Rack13 FLOAT NULL, " +
           "Rack14 FLOAT NULL, " +
           "Rack15 FLOAT NULL, " +
           "Rack16 FLOAT NULL, " +
           "Spare VARCHAR(255) NULL)", // Change data type to BIT for isActive
           connection))
                {
                    command.ExecuteNonQuery();
                }

                // Populate the 'id' column with values from 1 to 84
                using (SqlCommand populateCommand = new SqlCommand(
                    $"INSERT INTO {tableName} (ID, Standby, Loading, LoadCell,  Barcode, UnLoading, Positive, Error,Rack1, Rack2, Rack3, Rack4, Rack5, Rack6, Rack7, Rack8, Rack9, Rack10, Rack11, Rack12, Rack13, Rack14, Rack15, Rack16, Spare)" +
                    $" VALUES (@ID, @Standby, @Loading, @LoadCell, @Barcode, @UnLoading, @Positive, @Error,  @Rack1, @Rack2, @Rack3, @Rack4, @Rack5, @Rack6, @Rack7, @Rack8, @Rack9, @Rack10, @Rack11, @Rack12, @Rack13, @Rack14, @Rack15, @Rack16, @Spare)",
                    connection))
                {
                    populateCommand.Parameters.Add(new SqlParameter("@ID", SqlDbType.Int));
                    populateCommand.Parameters.Add(new SqlParameter("@Standby", SqlDbType.Float));
                    populateCommand.Parameters.Add(new SqlParameter("@Loading", SqlDbType.Float));
                    populateCommand.Parameters.Add(new SqlParameter("@LoadCell", SqlDbType.Float));
                    populateCommand.Parameters.Add(new SqlParameter("@Barcode", SqlDbType.Float));
                    populateCommand.Parameters.Add(new SqlParameter("@UnLoading", SqlDbType.Float));
                    populateCommand.Parameters.Add(new SqlParameter("@Positive", SqlDbType.Float));
                    populateCommand.Parameters.Add(new SqlParameter("@Error", SqlDbType.Float));
                    populateCommand.Parameters.Add(new SqlParameter("@Rack1", SqlDbType.Float));
                    populateCommand.Parameters.Add(new SqlParameter("@Rack2", SqlDbType.Float));
                    populateCommand.Parameters.Add(new SqlParameter("@Rack3", SqlDbType.Float));
                    populateCommand.Parameters.Add(new SqlParameter("@Rack4", SqlDbType.Float));
                    populateCommand.Parameters.Add(new SqlParameter("@Rack5", SqlDbType.Float));
                    populateCommand.Parameters.Add(new SqlParameter("@Rack6", SqlDbType.Float));
                    populateCommand.Parameters.Add(new SqlParameter("@Rack7", SqlDbType.Float));
                    populateCommand.Parameters.Add(new SqlParameter("@Rack8", SqlDbType.Float));
                    populateCommand.Parameters.Add(new SqlParameter("@Rack9", SqlDbType.Float));
                    populateCommand.Parameters.Add(new SqlParameter("@Rack10", SqlDbType.Float));
                    populateCommand.Parameters.Add(new SqlParameter("@Rack11", SqlDbType.Float));
                    populateCommand.Parameters.Add(new SqlParameter("@Rack12", SqlDbType.Float));
                    populateCommand.Parameters.Add(new SqlParameter("@Rack13", SqlDbType.Float));
                    populateCommand.Parameters.Add(new SqlParameter("@Rack14", SqlDbType.Float));
                    populateCommand.Parameters.Add(new SqlParameter("@Rack15", SqlDbType.Float));
                    populateCommand.Parameters.Add(new SqlParameter("@Rack16", SqlDbType.Float));
                    populateCommand.Parameters.Add(new SqlParameter("@Spare", SqlDbType.VarChar));
                    int totalCells = 1;

                    for (int i = 0; i < totalCells; i++)
                    {
                        populateCommand.Parameters["@ID"].Value = i;
                        populateCommand.Parameters["@Standby"].Value = 195.67;
                        populateCommand.Parameters["@Loading"].Value = 195.67;
                        populateCommand.Parameters["@LoadCell"].Value = 195.67;
                        populateCommand.Parameters["@Barcode"].Value = 195.67;
                        populateCommand.Parameters["@UnLoading"].Value = 195.67;
                        populateCommand.Parameters["@Positive"].Value = 318;
                        populateCommand.Parameters["@Error"].Value = 318;
                        populateCommand.Parameters["@Rack1"].Value = 348.11;
                        populateCommand.Parameters["@Rack2"].Value = 348.11;
                        populateCommand.Parameters["@Rack3"].Value = 348.11;
                        populateCommand.Parameters["@Rack4"].Value = 348.11;
                        populateCommand.Parameters["@Rack5"].Value = 348.11;
                        populateCommand.Parameters["@Rack6"].Value = 347.0;
                        populateCommand.Parameters["@Rack7"].Value = 259.0;
                        populateCommand.Parameters["@Rack8"].Value = 171.0;
                        populateCommand.Parameters["@Rack9"].Value = 83.0;
                        populateCommand.Parameters["@Rack10"].Value = 573.7;
                        populateCommand.Parameters["@Rack11"].Value = 485.7;
                        populateCommand.Parameters["@Rack12"].Value = 370.7;
                        populateCommand.Parameters["@Rack13"].Value = 282.7;
                        populateCommand.Parameters["@Rack14"].Value = 194.7;
                        populateCommand.Parameters["@Rack15"].Value = 106.7;
                        populateCommand.Parameters["@Rack16"].Value = 18.7;
                        populateCommand.Parameters["@Spare"].Value = "";
                        populateCommand.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
        #endregion Position

        #region ETC
        private void CreateTable_ETC(SqlConnection connection, string tableName)
        {
            try
            {
                using (SqlCommand command = new SqlCommand(
           $"CREATE TABLE {tableName} (" +
           "ID INT PRIMARY KEY, " +
           "TrashCanProductCount INT NULL, " +
           "Spare VARCHAR(255) NULL)", // Change data type to BIT for isActive
           connection))
                {
                    command.ExecuteNonQuery();
                }

                // Populate the 'id' column with values from 1 to 84
                using (SqlCommand populateCommand = new SqlCommand(
                    $"INSERT INTO {tableName} (ID, TrashCanProductCount, Spare)" +
                    $" VALUES (@ID, @TrashCanProductCount,  @Spare)",
                    connection))
                {
                    populateCommand.Parameters.Add(new SqlParameter("@ID", SqlDbType.Int));
                    populateCommand.Parameters.Add(new SqlParameter("@TrashCanProductCount", SqlDbType.Int));
                    populateCommand.Parameters.Add(new SqlParameter("@Spare", SqlDbType.VarChar));
                    int totalCells = 1;

                    for (int i = 0; i < totalCells; i++)
                    {
                        populateCommand.Parameters["@ID"].Value = i;
                        populateCommand.Parameters["@TrashCanProductCount"].Value = i;
                        populateCommand.Parameters["@Spare"].Value = "";
                        populateCommand.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
        #endregion ETC

        #endregion Table Initialize

        #region Insert
        #region  EquipmentH
        public void InsertEquipmentH(DateTime CreateTableTime, IEnumerable<DatabaseManager_EquipmentH> lst)
        {
            try
            {
                CreateTable_EquipmentH(CreateTableTime);
                string tableName = $"H_{CreateTableTime:yy_MM}";
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    foreach (var item in lst)
                    {
                        var commandText = $@"INSERT INTO {tableName} 
                                    (ID, Barcode, Qrcode, CreDate, PcbADC, PcbLED, Temperature) 
                                    VALUES 
                                    (@ID, @Barcode, @Qrcode, @CreDate, @PcbADC, @PcbLED, @Temperature)";

                        using (var command = new SqlCommand(commandText, connection))
                        {
                            command.Parameters.AddWithValue("@ID", item.ID);
                            command.Parameters.AddWithValue("@Barcode", item.Barcode ?? (object)DBNull.Value);
                            command.Parameters.AddWithValue("@Qrcode", item.Qrcode ?? (object)DBNull.Value);
                            command.Parameters.AddWithValue("@CreDate", item.CreDate);
                            command.Parameters.AddWithValue("@PcbADC", item.PcbADC);
                            command.Parameters.AddWithValue("@PcbLED", item.PcbLED);
                            command.Parameters.AddWithValue("@Temperature", item.Temperature);
                            command.ExecuteNonQuery();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // 오류 처리
                Console.WriteLine(ex.Message);
            }
        }

        #endregion EquipmentH

        #region Barcode
        public void InsertBarcode(IEnumerable<DatabaseManager_Barcode> lst)
        {
            try
            {
                string tableName = "Barcode";
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    foreach (var item in lst)
                    {
                        var commandText = $@"INSERT INTO {tableName} 
                                    (ID, Barcode, Qrcode, LoadCell, Loading, Unloading, IncubationTime, Result, PositiveTime) 
                                    VALUES 
                                    (@ID, @Barcode, @Qrcode, @LoadCell, @Loading, @Unloading, @IncubationTime,  @Result, @PositiveTime)";

                        using (var command = new SqlCommand(commandText, connection))
                        {
                            command.Parameters.AddWithValue("@ID", item.ID);
                            command.Parameters.AddWithValue("@Barcode", item.Barcode ?? (object)DBNull.Value);
                            command.Parameters.AddWithValue("@Qrcode", item.Qrcode ?? (object)DBNull.Value);
                            command.Parameters.AddWithValue("@LoadCell", item.LoadCell);
                            command.Parameters.AddWithValue("@Loading", item.Loading ?? (object)DBNull.Value);
                            command.Parameters.AddWithValue("@Unloading", item.Unloading ?? (object)DBNull.Value);
                            command.Parameters.AddWithValue("@IncubationTime", item.IncubationTime);
                            command.Parameters.AddWithValue("@Result", item.Result ?? (object)DBNull.Value);
                            command.Parameters.AddWithValue("@PositiveTime", item.PositiveTime ?? (object)DBNull.Value);

                            command.ExecuteNonQuery();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // 오류 처리
                Console.WriteLine(ex.Message);
            }
        }
        #endregion Barcode

        #region Login
        public void InsertLogin(IEnumerable<DatabaseManager_Login> lst)
        {
            try
            {
                string tableName = "Login";
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    foreach (var item in lst)
                    {
                        var commandText = $@"INSERT INTO {tableName} 
                                    (User_Id, User_Level, User_Password) 
                                    VALUES 
                                    (@User_Id, @User_Level, @User_Password)";

                        using (var command = new SqlCommand(commandText, connection))
                        {
                            command.Parameters.AddWithValue("@User_Id", item.User_Id ?? (object)DBNull.Value);
                            command.Parameters.AddWithValue("@User_Level", item.User_Level ?? (object)DBNull.Value);
                            command.Parameters.AddWithValue("@User_Password", item.User_Password ?? (object)DBNull.Value);


                            command.ExecuteNonQuery();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // 오류 처리
                Console.WriteLine(ex.Message);
            }
        }
        #endregion Login
        #endregion Insert

        #region Select
        #region System
        public List<DatabaseManager_System> Select_SystemInfo()
        {
            string tableName = "System";
            var configurations = new List<DatabaseManager_System>();
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (var command = new SqlCommand($"SELECT * FROM {tableName}", connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var data = new DatabaseManager_System
                                {
                                    FASTECH_Servo_IP = reader["FASTECH_Servo_IP"].ToString(),
                                    FASTECH_IO_Input_IP = reader["FASTECH_IO_Input_IP"].ToString(),
                                    FASTECH_IO_Output_IP = reader["FASTECH_IO_Output_IP"].ToString(),
                                    Schneider_Robot_IP = reader["Schneider_Robot_IP"].ToString(),
                                    Schneider_Robot_Port = Convert.ToInt32(reader["Schneider_Robot_Port"]),
                                    PCB_ID1 = reader["PCB_ID1"].ToString(),
                                    PCB_ID2 = reader["PCB_ID2"].ToString(),
                                    PCB_ID3 = reader["PCB_ID3"].ToString(),
                                    PCB_ID4 = reader["PCB_ID4"].ToString(),
                                    PCB_Serial = reader["PCB_Serial"].ToString(),
                                    Temperature_Serial = reader["Temperature_Serial"].ToString(),
                                    Barcode_Serial = reader["Barcode_Serial"].ToString(),
                                    Loadcell_Serial = reader["Loadcell_Serial"].ToString(),
                                    Spare1 = reader["Spare1"].ToString(),
                                    Spare2 = reader["Spare2"].ToString()
                                };

                                configurations.Add(data);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return configurations;
        }
        #endregion System

        #region Login
        public List<DatabaseManager_Login> Select_LoginInfo()
        {
            string tableName = "Login";
            var configurations = new List<DatabaseManager_Login>();
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (var command = new SqlCommand($"SELECT * FROM {tableName}", connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var data = new DatabaseManager_Login
                                {
                                    User_Id = reader["User_Id"].ToString(),
                                    User_Level = reader["User_Level"].ToString(),
                                    User_Password = reader["User_Password"].ToString(),
                                    User_Enable = reader.IsDBNull(reader.GetOrdinal("User_Enable")) ? false : reader.GetBoolean(reader.GetOrdinal("User_Enable")),

                                };

                                configurations.Add(data);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return configurations;
        }
        #endregion Login

        #region Config
        public List<Class_Config> Select_Config()
        {
            string tableName = "Config";
            var configurations = new List<Class_Config>();
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (var command = new SqlCommand($"SELECT * FROM {tableName}", connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var data = new Class_Config
                                {
                                    ID = Convert.ToInt32(reader["ID"]),
                                    Temp = Convert.ToDouble(reader["Temp"]),
                                    doorOpenAlarmTrigger = Convert.ToInt32(reader["doorOpenAlarmTrigger"]),
                                    LoadCellMin = Convert.ToDouble(reader["LoadCellMin"]),
                                    LoadCellMax = Convert.ToDouble(reader["LoadCellMax"]),
                                    MaximumTime = Convert.ToInt32(reader["MaximumTime"]),
                                    UseBuzzer = Convert.ToBoolean(reader["UseBuzzer"]),
                                    Positive_Wait = Convert.ToInt32(reader["Positive_Wait"]),
                                    Positive_Low = Convert.ToInt32(reader["Positive_Low"]),
                                    Positive_High = Convert.ToInt32(reader["Positive_High"]),
                                    Analysis_Time_Range = Convert.ToInt32(reader["Analysis_Time_Range"]),
                                    Analysis_Intervals = Convert.ToInt32(reader["Analysis_Intervals"]),
                                    Threshold = Convert.ToDouble(reader["Threshold"]),
                                    BottleExistenceRange = Convert.ToInt32(reader["BottleExistenceRange"]),
                                    DataStorageSave = Convert.ToInt32(reader["DataStorageSave"]),
                                    TrashCanFillLevel = Convert.ToInt32(reader["TrashCanFillLevel"]),
                                    SYSTEM1 = Convert.ToBoolean(reader["SYSTEM1"]),
                                    SYSTEM2 = Convert.ToBoolean(reader["SYSTEM2"]),
                                    SYSTEM3 = Convert.ToBoolean(reader["SYSTEM3"]),
                                    SYSTEM4 = Convert.ToBoolean(reader["SYSTEM4"]),
                                    Spare1 = reader["Spare1"].ToString(),
                                    Spare2 = reader["Spare2"].ToString(),
                                    Spare3 = reader["Spare3"].ToString(),
                                    Spare4 = reader["Spare4"].ToString(),
                                    Spare5 = reader["Spare5"].ToString()

                                };

                                configurations.Add(data);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return configurations;
        }
        #endregion  Config

        #region FASTECH
        public List<DatabaseManager_FASTECH_Parameter> Select_FASTECHInfo()
        {
            string tableName = "FASTECH";
            var configurations = new List<DatabaseManager_FASTECH_Parameter>();
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (var command = new SqlCommand($"SELECT * FROM {tableName}", connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var data = new DatabaseManager_FASTECH_Parameter
                                {
                                    ID = Convert.ToInt32(reader["ID"]),
                                    Axis_Acc_Time = Convert.ToInt32(reader["Axis_Acc_Time"]),
                                    Axis_Dec_Time = Convert.ToInt32(reader["Axis_Dec_Time"]),
                                    Axis_Speed = Convert.ToInt32(reader["Axis_Speed"]),
                                    Jog_Speed_Low = Convert.ToInt32(reader["Jog_Speed_Low"]),
                                    Jog_Speed_Middle = Convert.ToInt32(reader["Jog_Speed_Middle"]),
                                    Jog_Speed_High = Convert.ToInt32(reader["Jog_Speed_High"]),
                                    Jog_Acc_Dec_Time = Convert.ToInt32(reader["Jog_Acc_Dec_Time"]),
                                    Org_Speed = Convert.ToInt32(reader["Org_Speed"]),
                                    Org_Search_Speed = Convert.ToInt32(reader["Org_Search_Speed"]),
                                    Org_Acc_Dec_Time = Convert.ToInt32(reader["Org_Acc_Dec_Time"]),
                                    Org_Dethod = Convert.ToInt32(reader["Org_Dethod"]),
                                    Org_Dir = Convert.ToInt32(reader["Org_Dir"]),
                                    Org_Offset = Convert.ToInt32(reader["Org_Offset"]),
                                    Motion_Dir = Convert.ToInt32(reader["Motion_Dir"]),
                                    Spare1 = reader["Spare1"].ToString(),
                                    Spare2 = reader["Spare2"].ToString(),
                                    Spare3 = reader["Spare3"].ToString(),
                                    Spare4 = reader["Spare4"].ToString(),
                                    Spare5 = reader["Spare5"].ToString()
                                };
                                configurations.Add(data);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return configurations;
        }
        #endregion FASTECH

        #region Position
        public List<Class_Position> Select_Position()
        {
            string tableName = "Position";
            var configurations = new List<Class_Position>();
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (var command = new SqlCommand($"SELECT * FROM {tableName}", connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var data = new Class_Position
                                {
                                    ID = Convert.ToInt32(reader["ID"]),
                                    Standby = Convert.ToDouble(reader["Standby"]),
                                    Loading = Convert.ToDouble(reader["Loading"]),
                                    LoadCell = Convert.ToDouble(reader["LoadCell"]),
                                    Barcode = Convert.ToDouble(reader["Barcode"]),
                                    UnLoading = Convert.ToDouble(reader["UnLoading"]),
                                    Positive = Convert.ToDouble(reader["Positive"]),
                                    Error = Convert.ToDouble(reader["Error"]),
                                    Rack1 = Convert.ToDouble(reader["Rack1"]),
                                    Rack2 = Convert.ToDouble(reader["Rack2"]),
                                    Rack3 = Convert.ToDouble(reader["Rack3"]),
                                    Rack4 = Convert.ToDouble(reader["Rack4"]),
                                    Rack5 = Convert.ToDouble(reader["Rack5"]),
                                    Rack6 = Convert.ToDouble(reader["Rack6"]),
                                    Rack7 = Convert.ToDouble(reader["Rack7"]),
                                    Rack8 = Convert.ToDouble(reader["Rack8"]),
                                    Rack9 = Convert.ToDouble(reader["Rack9"]),
                                    Rack10 = Convert.ToDouble(reader["Rack10"]),
                                    Rack11 = Convert.ToDouble(reader["Rack11"]),
                                    Rack12 = Convert.ToDouble(reader["Rack12"]),
                                    Rack13 = Convert.ToDouble(reader["Rack13"]),
                                    Rack14 = Convert.ToDouble(reader["Rack14"]),
                                    Rack15 = Convert.ToDouble(reader["Rack15"]),
                                    Rack16 = Convert.ToDouble(reader["Rack16"]),

                                    Spare = reader["Spare"].ToString(),

                                };
                                configurations.Add(data);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return configurations;
        }
        #endregion Position

        #region ETC
        public List<Class_ETC> Select_ETC()
        {
            string tableName = "ETC";
            var configurations = new List<Class_ETC>();
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (var command = new SqlCommand($"SELECT * FROM {tableName}", connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var data = new Class_ETC
                                {
                                    ID = Convert.ToInt32(reader["ID"]),
                                    TrashCanProductCount = Convert.ToInt32(reader["TrashCanProductCount"]),
                                    Spare = reader["Spare"].ToString(),

                                };
                                configurations.Add(data);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return configurations;
        }
        #endregion ETC

        #region Equipment

        public List<DatabaseManager_Equipment> Select_Equipment()
        {
            string tableName = "Equipment";
            var configurations = new List<DatabaseManager_Equipment>();
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sqlQuery = $"SELECT * FROM {tableName}";
                    using (var command = new SqlCommand(sqlQuery, connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var data = new DatabaseManager_Equipment
                                {
                                    ID = Convert.ToInt32(reader["ID"]),
                                    Cell = reader.IsDBNull(reader.GetOrdinal("Cell")) ? null : reader.GetString(reader.GetOrdinal("Cell")),
                                    Barcode = reader.IsDBNull(reader.GetOrdinal("Barcode")) ? null : reader.GetString(reader.GetOrdinal("Barcode")),
                                    Qrcode = reader.IsDBNull(reader.GetOrdinal("Qrcode")) ? null : reader.GetString(reader.GetOrdinal("Qrcode")),
                                    Loading = reader.IsDBNull(reader.GetOrdinal("Loading")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("Loading")),
                                    CreDate = reader.IsDBNull(reader.GetOrdinal("CreDate")) ? (DateTime?)null : Convert.ToDateTime(reader["CreDate"]),
                                    PositiveTime = reader.IsDBNull(reader.GetOrdinal("PositiveTime")) ? (DateTime?)null : Convert.ToDateTime(reader["PositiveTime"]),
                                    Result = reader.IsDBNull(reader.GetOrdinal("Result")) ? null : reader.GetString(reader.GetOrdinal("Result")),
                                    IncubationTime = reader["IncubationTime"] != DBNull.Value ? Convert.ToInt32(reader["IncubationTime"]) : 0,
                                    Switched = reader.IsDBNull(reader.GetOrdinal("Switched")) ? false : reader.GetBoolean(reader.GetOrdinal("Switched")),
                                    isEnable = reader.IsDBNull(reader.GetOrdinal("isEnable")) ? false : reader.GetBoolean(reader.GetOrdinal("isEnable")),
                                    isActive = reader.IsDBNull(reader.GetOrdinal("isActive")) ? false : reader.GetBoolean(reader.GetOrdinal("isActive")),
                                };

                                configurations.Add(data);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return configurations;
        }

        public string Select_Equipment_Barcode_Search(int id)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "SELECT Barcode FROM Equipment WHERE ID = @ID";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ID", id);
                        object result = command.ExecuteScalar();

                        if (result != null)
                        {
                            return result.ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }

            return string.Empty;
        }


        public List<DatabaseManager_Equipment> Select_Equipment_Search()
        {
            string tableName = "Equipment";
            var configurations = new List<DatabaseManager_Equipment>();
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sqlQuery = $"SELECT * FROM {tableName} WHERE isActive = 1 AND isEnable = 1";
                    using (var command = new SqlCommand(sqlQuery, connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var data = new DatabaseManager_Equipment
                                {
                                    ID = Convert.ToInt32(reader["ID"]),
                                    Cell = reader.IsDBNull(reader.GetOrdinal("Cell")) ? null : reader.GetString(reader.GetOrdinal("Cell")),

                                    Barcode = reader.IsDBNull(reader.GetOrdinal("Barcode")) ? null : reader.GetString(reader.GetOrdinal("Barcode")),
                                    Qrcode = reader.IsDBNull(reader.GetOrdinal("Qrcode")) ? null : reader.GetString(reader.GetOrdinal("Qrcode")),
                                    Loading = reader.IsDBNull(reader.GetOrdinal("Loading")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("Loading")),
                                    CreDate = reader.IsDBNull(reader.GetOrdinal("CreDate")) ? (DateTime?)null : Convert.ToDateTime(reader["CreDate"]),
                                    PositiveTime = reader.IsDBNull(reader.GetOrdinal("PositiveTime")) ? (DateTime?)null : Convert.ToDateTime(reader["PositiveTime"]),
                                    Result = reader.IsDBNull(reader.GetOrdinal("Result")) ? null : reader.GetString(reader.GetOrdinal("Result")),
                                    IncubationTime = reader["IncubationTime"] != DBNull.Value ? Convert.ToInt32(reader["IncubationTime"]) : 0,
                                    isEnable = reader.IsDBNull(reader.GetOrdinal("isEnable")) ? false : reader.GetBoolean(reader.GetOrdinal("isEnable")),
                                    isActive = reader.IsDBNull(reader.GetOrdinal("isActive")) ? false : reader.GetBoolean(reader.GetOrdinal("isActive")),
                                };

                                configurations.Add(data);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // 에러 처리 로직 추가 (예: 로깅)
                Console.WriteLine("Error in Select_Equipment_Search: " + ex.Message);
            }

            return configurations;
        }


        #endregion Equipment

        #region EquipmentH
        public List<DatabaseManager_EquipmentH> QueryDataForBarcodeAndDateRange(string barcode, DateTime startDate, DateTime endDate)
        {
            var results = new List<DatabaseManager_EquipmentH>();
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    // Prepare to iterate from startDate's month to endDate's month
                    DateTime currentMonth = new DateTime(startDate.Year, startDate.Month, 1);

                    while (currentMonth <= endDate)
                    {
                        string tableName = $"H_{currentMonth:yy_MM}";
                        if (TableExists(connection, tableName))
                        {
                            // Correct the boundaries for the start and end dates within the current month
                            DateTime validStartDate = currentMonth > startDate ? currentMonth : startDate;
                            DateTime validEndDate = currentMonth.AddMonths(1).AddSeconds(-1) < endDate ? currentMonth.AddMonths(1).AddSeconds(-1) : endDate;

                            string sqlQuery = $"SELECT * FROM {tableName} " +
                                              $"WHERE Barcode = @Barcode AND " +
                                              $"CreDate BETWEEN @ValidStartDate AND @ValidEndDate " +
                                              $"ORDER BY CreDate ASC";

                            using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                            {
                                command.Parameters.AddWithValue("@Barcode", barcode);
                                command.Parameters.AddWithValue("@ValidStartDate", validStartDate);
                                command.Parameters.AddWithValue("@ValidEndDate", validEndDate);

                                using (SqlDataReader reader = command.ExecuteReader())
                                {
                                    while (reader.Read())
                                    {
                                        var entry = new DatabaseManager_EquipmentH
                                        {
                                            ID = reader.GetInt32(reader.GetOrdinal("ID")),
                                            Barcode = reader.GetString(reader.GetOrdinal("Barcode")),
                                            Qrcode = reader.IsDBNull(reader.GetOrdinal("Qrcode")) ? null : reader.GetString(reader.GetOrdinal("Qrcode")),
                                            CreDate = reader.GetDateTime(reader.GetOrdinal("CreDate")),
                                            PcbADC = reader.GetDouble(reader.GetOrdinal("PcbADC")),
                                            PcbLED = reader.GetDouble(reader.GetOrdinal("PcbLED")),
                                            Temperature = reader.GetDouble(reader.GetOrdinal("Temperature"))
                                        };
                                        results.Add(entry);
                                    }
                                }
                            }
                        }
                        else
                        {
                            Console.WriteLine($"Table {tableName} does not exist.");
                        }
                        // Move to the first day of the next month
                        currentMonth = currentMonth.AddMonths(1);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during database query: {ex.Message}");
            }
            return results;
        }
        #endregion EquipmentH

        #region Barcode
        public List<DatabaseManager_BarcodeList> Select_Barcode(DateTime startDate, DateTime endDate, string test1)
        {
            string tableName = "Barcode";
            var configurations = new List<DatabaseManager_BarcodeList>();
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {

                    string sqlQuery = $"SELECT * FROM {tableName} WHERE Loading BETWEEN @StartDate AND @EndDate";

                    if (!string.IsNullOrEmpty(test1))
                    {
                        sqlQuery += " AND Barcode LIKE @Barcode";
                    }

                    sqlQuery += " ORDER BY Loading ASC";



                    connection.Open();
                    using (var command = new SqlCommand(sqlQuery, connection))
                    {
                        command.Parameters.AddWithValue("@StartDate", startDate);
                        command.Parameters.AddWithValue("@EndDate", endDate);
                        if (!string.IsNullOrEmpty(test1))
                        {
                            command.Parameters.AddWithValue("@Barcode", "%" + test1 + "%");
                        }
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var data = new DatabaseManager_BarcodeList
                                {
                                    ID = Convert.ToInt32(reader["ID"]),
                                    Barcode = reader.IsDBNull(reader.GetOrdinal("Barcode")) ? null : reader.GetString(reader.GetOrdinal("Barcode")),
                                    Qrcode = reader.IsDBNull(reader.GetOrdinal("Qrcode")) ? null : reader.GetString(reader.GetOrdinal("Qrcode")),
                                    LoadCell = reader["LoadCell"] != DBNull.Value ? Convert.ToDouble(reader["LoadCell"]) : 0,
                                    Loading = reader.IsDBNull(reader.GetOrdinal("Loading")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("Loading")),
                                    Unloading = reader.IsDBNull(reader.GetOrdinal("Unloading")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("Unloading")),
                                    IncubationTime = ConvertSecondsToReadableTime(reader["IncubationTime"] != DBNull.Value ? Convert.ToInt32(reader["IncubationTime"]) : 0),
                                    Result = reader.IsDBNull(reader.GetOrdinal("Result")) ? null : reader.GetString(reader.GetOrdinal("Result")),
                                    PositiveTime = reader.IsDBNull(reader.GetOrdinal("PositiveTime")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("PositiveTime")),
                                };

                                configurations.Add(data);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return configurations;
        }
        private string ConvertSecondsToReadableTime(int totalSeconds)
        {
            TimeSpan time = TimeSpan.FromSeconds(totalSeconds);
            return $"{time.Days}일 {time.Hours}시간 {time.Minutes}분";
        }
        public bool Select_Barcode_Search(string barcodeID)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "SELECT COUNT(1) FROM Barcode WHERE Barcode = @BarcodeID";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@BarcodeID", barcodeID);

                        int count = Convert.ToInt32(command.ExecuteScalar());

                        if (count > 0)
                        {
                            return true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }

            return false;
        }

        public List<DatabaseManager_BarcodeList> Select_Barcode(string BarcodeID)
        {
            string tableName = "Barcode";
            var configurations = new List<DatabaseManager_BarcodeList>();
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    string sqlQuery = $"SELECT * FROM {tableName}";

                    if (!string.IsNullOrEmpty(BarcodeID))
                    {
                        sqlQuery += " WHERE Barcode LIKE @Barcode";
                    }

                    sqlQuery += " ORDER BY Loading ASC";

                    connection.Open();
                    using (var command = new SqlCommand(sqlQuery, connection))
                    {
                        if (!string.IsNullOrEmpty(BarcodeID))
                        {
                            command.Parameters.AddWithValue("@Barcode", "%" + BarcodeID.Trim() + "%");
                        }

                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var data = new DatabaseManager_BarcodeList
                                {
                                    ID = Convert.ToInt32(reader["ID"]),
                                    Barcode = reader.IsDBNull(reader.GetOrdinal("Barcode")) ? null : reader.GetString(reader.GetOrdinal("Barcode")),
                                    Qrcode = reader.IsDBNull(reader.GetOrdinal("Qrcode")) ? null : reader.GetString(reader.GetOrdinal("Qrcode")),
                                    LoadCell = reader["LoadCell"] != DBNull.Value ? Convert.ToDouble(reader["LoadCell"]) : 0,
                                    Loading = reader.IsDBNull(reader.GetOrdinal("Loading")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("Loading")),
                                    Unloading = reader.IsDBNull(reader.GetOrdinal("Unloading")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("Unloading")),
                                    IncubationTime = ConvertSecondsToReadableTime(reader["IncubationTime"] != DBNull.Value ? Convert.ToInt32(reader["IncubationTime"]) : 0),
                                    Result = reader.IsDBNull(reader.GetOrdinal("Result")) ? null : reader.GetString(reader.GetOrdinal("Result")),
                                    PositiveTime = reader.IsDBNull(reader.GetOrdinal("PositiveTime")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("PositiveTime")),
                                };

                                configurations.Add(data);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // 로그 또는 예외 처리
            }
            return configurations;
        }

        #endregion Barcode


        #endregion Select

        #region Update
        #region  Login

        public void UpdateLogin(string query, Dictionary<string, object> parameters)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // Add parameters to the command
                        foreach (var param in parameters)
                        {
                            object value = param.Value ?? DBNull.Value;
                            command.Parameters.AddWithValue(param.Key, value);
                        }

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }
        #endregion Login

        #region  Equipment

        public void UpdateEquipment(string query, Dictionary<string, object> parameters)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // Add parameters to the command
                        foreach (var param in parameters)
                        {
                            object value = param.Value ?? DBNull.Value;
                            command.Parameters.AddWithValue(param.Key, value);
                        }

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }
        #endregion Equipment

        #region Barcode
        public void UpdateBarcode(string query, Dictionary<string, object> parameters)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // Add parameters to the command
                        foreach (var param in parameters)
                        {
                            object value = param.Value ?? DBNull.Value;
                            command.Parameters.AddWithValue(param.Key, value);
                        }

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }
        #endregion Barcode

        #region FASTECH
        public void UpdateFASTECHInfo(IEnumerable<DatabaseManager_FASTECH_Parameter> lst)
        {
            string tableName = "FASTECH";

            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    foreach (var item in lst)
                    {
                        using (var command = new SqlCommand($"UPDATE {tableName} SET " +
                                                            "Axis_Acc_Time = @Axis_Acc_Time, " +
                                                            "Axis_Dec_Time = @Axis_Dec_Time, " +
                                                            "Axis_Speed = @Axis_Speed, " +
                                                            "Jog_Speed_Low = @Jog_Speed_Low, " +
                                                            "Jog_Speed_Middle = @Jog_Speed_Middle, " +
                                                            "Jog_Speed_High = @Jog_Speed_High, " +
                                                            "Jog_Acc_Dec_Time = @Jog_Acc_Dec_Time, " +
                                                            "Org_Speed = @Org_Speed, " +
                                                            "Org_Search_Speed = @Org_Search_Speed, " +
                                                            "Org_Acc_Dec_Time = @Org_Acc_Dec_Time, " +
                                                            "Org_Dethod = @Org_Dethod, " +
                                                            "Org_Dir = @Org_Dir, " +
                                                            "Org_Offset = @Org_Offset, " +
                                                            "Motion_Dir = @Motion_Dir, " +
                                                            "Spare1 = @Spare1, " +
                                                            "Spare2 = @Spare2, " +
                                                            "Spare3 = @Spare3, " +
                                                            "Spare4 = @Spare4, " +
                                                            "Spare5 = @Spare5 " +
                                                            "WHERE ID = @ID", connection))
                        {
                            command.Parameters.AddWithValue("@ID", item.ID);
                            command.Parameters.AddWithValue("@Axis_Acc_Time", item.Axis_Acc_Time);
                            command.Parameters.AddWithValue("@Axis_Dec_Time", item.Axis_Dec_Time);
                            command.Parameters.AddWithValue("@Axis_Speed", item.Axis_Speed);
                            command.Parameters.AddWithValue("@Jog_Speed_Low", item.Jog_Speed_Low);
                            command.Parameters.AddWithValue("@Jog_Speed_Middle", item.Jog_Speed_Middle);
                            command.Parameters.AddWithValue("@Jog_Speed_High", item.Jog_Speed_High);
                            command.Parameters.AddWithValue("@Jog_Acc_Dec_Time", item.Jog_Acc_Dec_Time);
                            command.Parameters.AddWithValue("@Org_Speed", item.Org_Speed);
                            command.Parameters.AddWithValue("@Org_Search_Speed", item.Org_Search_Speed);
                            command.Parameters.AddWithValue("@Org_Acc_Dec_Time", item.Org_Acc_Dec_Time);
                            command.Parameters.AddWithValue("@Org_Dethod", item.Org_Dethod);
                            command.Parameters.AddWithValue("@Org_Dir", item.Org_Dir);
                            command.Parameters.AddWithValue("@Org_Offset", item.Org_Offset);
                            command.Parameters.AddWithValue("@Motion_Dir", item.Motion_Dir);
                            command.Parameters.AddWithValue("@Spare1", item.Spare1);
                            command.Parameters.AddWithValue("@Spare2", item.Spare2);
                            command.Parameters.AddWithValue("@Spare3", item.Spare3);
                            command.Parameters.AddWithValue("@Spare4", item.Spare4);
                            command.Parameters.AddWithValue("@Spare5", item.Spare5);

                            command.ExecuteNonQuery();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // 오류 처리
                Console.WriteLine(ex.Message);
            }
        }

        #endregion FASTECH

        #region Config
        public void UpdateConfig(IEnumerable<Class_Config> lst)
        {
            string tableName = "Config";

            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    foreach (var item in lst)
                    {
                        using (var command = new SqlCommand($"UPDATE {tableName} SET " +
                                                            "Temp = @Temp, " +
                                                            "MaximumTime = @MaximumTime, " +
                                                            "doorOpenAlarmTrigger = @doorOpenAlarmTrigger, " +                                                          
                                                            "LoadCellMin = @LoadCellMin, " +
                                                            "LoadCellMax = @LoadCellMax, " +
                                                            "UseBuzzer = @UseBuzzer, " +
                                                            "Positive_Wait = @Positive_Wait, " +
                                                            "Positive_Low = @Positive_Low, " +
                                                            "Positive_High = @Positive_High, " +
                                                            "Analysis_Time_Range = @Analysis_Time_Range, " +
                                                            "Analysis_Intervals = @Analysis_Intervals, " +
                                                            "Threshold = @Threshold, " +
                                                            "BottleExistenceRange = @BottleExistenceRange, " +
                                                            "DataStorageSave = @DataStorageSave, " +
                                                            "TrashCanFillLevel = @TrashCanFillLevel, " +
                                                            "SYSTEM1 = @SYSTEM1, " +
                                                            "SYSTEM2 = @SYSTEM2, " +
                                                            "SYSTEM3 = @SYSTEM3, " +
                                                            "SYSTEM4 = @SYSTEM4, " +
                                                            "Spare1 = @Spare1, " +
                                                            "Spare2 = @Spare2, " +
                                                            "Spare3 = @Spare3, " +
                                                            "Spare4 = @Spare4, " +
                                                            "Spare5 = @Spare5 " +
                                                            "WHERE ID = @ID", connection))
                        {
                            command.Parameters.AddWithValue("@ID", item.ID);
                            command.Parameters.AddWithValue("@Temp", item.Temp);
                            command.Parameters.AddWithValue("@doorOpenAlarmTrigger", item.doorOpenAlarmTrigger);
                            command.Parameters.AddWithValue("@MaximumTime", item.MaximumTime);
                            command.Parameters.AddWithValue("@LoadCellMin", item.LoadCellMin);
                            command.Parameters.AddWithValue("@LoadCellMax", item.LoadCellMax);
                            command.Parameters.AddWithValue("@UseBuzzer", item.UseBuzzer);
                            command.Parameters.AddWithValue("@Positive_Wait", item.Positive_Wait);
                            command.Parameters.AddWithValue("@Positive_Low", item.Positive_Low);
                            command.Parameters.AddWithValue("@Positive_High", item.Positive_High);
                            command.Parameters.AddWithValue("@Analysis_Time_Range", item.Analysis_Time_Range);
                            command.Parameters.AddWithValue("@Analysis_Intervals", item.Analysis_Intervals);
                            command.Parameters.AddWithValue("@Threshold", item.Threshold);
                            command.Parameters.AddWithValue("@BottleExistenceRange", item.BottleExistenceRange);
                            command.Parameters.AddWithValue("@DataStorageSave", item.DataStorageSave);
                            command.Parameters.AddWithValue("@TrashCanFillLevel", item.TrashCanFillLevel);
                            command.Parameters.AddWithValue("@SYSTEM1", item.SYSTEM1);
                            command.Parameters.AddWithValue("@SYSTEM2", item.SYSTEM2);
                            command.Parameters.AddWithValue("@SYSTEM3", item.SYSTEM3);
                            command.Parameters.AddWithValue("@SYSTEM4", item.SYSTEM4);
                            command.Parameters.AddWithValue("@Spare1", item.Spare1);
                            command.Parameters.AddWithValue("@Spare2", item.Spare2);
                            command.Parameters.AddWithValue("@Spare3", item.Spare3);
                            command.Parameters.AddWithValue("@Spare4", item.Spare4);
                            command.Parameters.AddWithValue("@Spare5", item.Spare5);

                            command.ExecuteNonQuery();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // 오류 처리
                Console.WriteLine(ex.Message);
            }
        }

        #endregion Config

        #region ETC
        public void UpdateETC(string query, Dictionary<string, object> parameters)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // Add parameters to the command
                        foreach (var param in parameters)
                        {
                            object value = param.Value ?? DBNull.Value;
                            command.Parameters.AddWithValue(param.Key, value);
                        }

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }
        #endregion ETC
        #endregion Update

        #region Delete
        public void DeleteRecords(string barcode, DateTime startDate, DateTime endDate)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                DeleteFromBarcodeTable(connection, barcode);
                DeleteFromHistoryTables(connection, barcode, startDate, endDate);
            }
        }



        private void DeleteFromBarcodeTable(SqlConnection connection, string barcode)
        {
            string query = "DELETE FROM Barcode WHERE Barcode = @Barcode";
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Barcode", barcode);
                int result = command.ExecuteNonQuery();
                Console.WriteLine($"Deleted {result} records from the Barcode table.");
            }
        }

        private void DeleteFromHistoryTables(SqlConnection connection, string barcode, DateTime startDate, DateTime endDate)
        {
            for (DateTime date = startDate; date <= endDate; date = date.AddMonths(1))
            {
                string tableName = $"H_{date:yy_MM}";
                if (HistoryTablesExists(connection, tableName))
                {
                    string query = $"DELETE FROM {tableName} WHERE Barcode = @Barcode";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Barcode", barcode);
                        int result = command.ExecuteNonQuery();
                        Console.WriteLine($"Deleted {result} records from the {tableName} table.");
                    }
                }
            }
        }

        private bool HistoryTablesExists(SqlConnection connection, string tableName)
        {
            string query = "SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = @TableName";
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@TableName", tableName);
                using (var reader = command.ExecuteReader())
                {
                    return reader.HasRows;
                }
            }
        }


        public void Delete_Login(string User_Id)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = "DELETE FROM Login WHERE User_Id = @User_Id";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@User_Id", User_Id);
                        int result = command.ExecuteNonQuery();

                    }
                }
                catch (SqlException ex)
                {
                    Console.WriteLine("SQL Error: " + ex.Message);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
            }
        }
        #endregion Delete
    }
}
