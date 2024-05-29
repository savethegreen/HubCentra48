using FASTECH;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using static FASTECH.EziMOTIONPlusELib;
using static HubCentra_A1.EnumManager;
using static HubCentra_A1.Model.View;

namespace HubCentra_A1.Class.FASTECH
{
    public class FastechDeviceManager
    {
        #region Connection

        #region IO
        public bool Connect_IO(Enum_FASTECH_ID ID, IPAddress IP)
        {

            try
            {
                int nBdID = (int)ID;
                if (FAS_Connect(IP, nBdID) == false)
                {
                    Console.WriteLine("TCP Connection Fail!");
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public bool IPAddressExist_IO(IPAddress IP)
        {
            try
            {
                int board = 0;
                bool DDD = FAS_IsIPAddressExist(IP, ref board);
                if (FAS_IsIPAddressExist(IP, ref board))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        #endregion IO
        #endregion Connection

        #region IO
        #region Get
        public List<Class_FASTECH_Input> Get_Input(Enum_FASTECH_ID ID)
        {
            try
            {
                List<Class_FASTECH_Input> lst = new List<Class_FASTECH_Input>();
                int nBdID = (int)ID;
                int nLatchCount = 8;
                bool[] data = new bool[nLatchCount];
                uint put = 0;
                uint Latch = 0;

                if (FAS_GetInput(nBdID, ref put, ref Latch) != FMM_OK)
                {
                    Console.WriteLine("Function(FAS_GetInput) was failed.");
                    return null;
                }
                else
                {
                    for (int i = 0; i < nLatchCount; i++)
                    {
                        bool State = (put & 1u << i) != 0;
                        //_viewModel.Observable_Ezi_Input[i].bools = inputState;
                        Class_FASTECH_Input io = new Class_FASTECH_Input
                        {
                            Flag = State
                        };
                        lst.Add(io);
                    }
                    return lst;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                return null;
            }
        }
        public List<Class_FASTECH_Output> Get_Output(Enum_FASTECH_ID ID)
        {
            try
            {
                List<Class_FASTECH_Output> lst = new List<Class_FASTECH_Output>();
                int nBdID = (int)ID;
                int nLatchCount = 8;
                bool[] data = new bool[nLatchCount];
                uint put = 0;
                uint Latch = 0;


                if (FAS_GetOutput(nBdID, ref put, ref Latch) != FMM_OK)
                {
                    Console.WriteLine("Function(FAS_GetInput) was failed.");
                    return null;
                }
                else
                {
                    for (int i = 0; i < nLatchCount; i++)
                    {
                        bool State = (put & 1u << i) != 0;
                        //_viewModel.Observable_Ezi_Input[i].bools = inputState;
                        Class_FASTECH_Output io = new Class_FASTECH_Output
                        {
                            Flag = State
                        };
                        lst.Add(io);
                    }
                    return lst;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                return null;
            }
        }
        #endregion Get

        #region Set
        public void Set_Output(Enum_FASTECH_ID ID, List<Class_FASTECH_Output> lst)
        {
            try
            {
                int nBdID = (int)ID;
                bool[] data = lst.Select(output => output.Flag).ToArray();
                bool[] bool_Ezi_Output = data; /* Initialize or get your bool array */;
                uint setMask;
                uint clrMask;
                CalculateMasks(bool_Ezi_Output, out setMask, out clrMask);

                //UpdateOutputMasks(bool_Ezi_Output, out uSetMask, out uClrMask);
                //    GenerateMasksFromBoolArray(DATAS, out USetMask, out UClrMask);
                if (EziMOTIONPlusELib.FAS_SetOutput(nBdID, setMask, clrMask) != EziMOTIONPlusELib.FMM_OK)
                {
                    Console.WriteLine("Function(FAS_SetOutput) was failed.");
                }
                else
                {
                    Console.WriteLine("FAS_SetOutput Success!");
                }
            }
            catch (Exception ex)
            {

            }
        }
        public void CalculateMasks(bool[] data, out uint uSetMask, out uint uClrMask)
        {
            uSetMask = 0;
            uClrMask = 0;
            try
            {


                for (int i = 0; i < data.Length; i++)
                {
                    // Shift based on the index. The first element (index 0) corresponds to a shift of 8.
                    int shiftAmount = 8 + i;

                    if (data[i])
                    {
                        // Set the corresponding bit in uSetMask
                        uSetMask |= (uint)(1 << shiftAmount);
                    }
                    else
                    {
                        // Set the corresponding bit in uClrMask
                        uClrMask |= (uint)(1 << shiftAmount);
                    }
                }
            }
            catch (Exception ex)
            {

            }

        }
        #endregion Set
   
        #endregion IO
    }
}
