﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HidUtilityNuget;
using ZedGraph;


namespace HidDemoWindowsForms
{
    public partial class MovContCLK : Form

    {
        // Global variables used by the form / application

        uint tick,AdcValue = 0;
        bool WaitingForDevice = false;
        bool Timer0Pending = false;
        bool ToggleLedD4Pending = false;
        byte LastCommand = 0x80;
        bool MarchaPending = false;
        bool ParoPending = true;
        bool CcwPending = false;
        bool CwPending = true;
        byte SentidoGiro = 1;
        byte StepResolution = 8; //FixedStep Full step
        String StepResolutionString=null;
        byte[] contadorTemp0 = new byte[2];
        ControlMovimientoContinuo controlMovimientoContinuo = new ControlMovimientoContinuo(false);
        HidUtility HidUtil;
        GraphPane myPane;
        
        static public short[] DatosVelocidadZed = new short[500];
        static public short[] Dato_VelocidadZed = new short[500];
        public MovContCLK()
        {
            InitializeComponent();
            // Get an instance of HidUtility
            HidUtil = new HidUtility();

            // Initialize user interface
            //SetUserInterfaceStatus(false);

            // Register event handlers
            //HidUtil.RaiseDeviceRemovedEvent += DeviceRemovedHandler;
            //HidUtil.RaiseDeviceAddedEvent += DeviceAddedHandler;
            HidUtil.RaiseConnectionStatusChangedEvent += ConnectionStatusChangedHandler;
            HidUtil.RaiseSendPacketEvent += SendPacketHandlerMovCont;
            HidUtil.RaisePacketSentEvent += PacketSentHandler;
            HidUtil.RaiseReceivePacketEvent += ReceivePacketHandler;
            HidUtil.RaisePacketReceivedEvent += PacketReceivedHandler;
            // Initial attempt to connect
            HidUtil.SelectDevice(new HidUtilityNuget.Device(0x04D8, 0X003F));
            
            //ZedGraph

            myPane = zedGraphControl1.GraphPane;
            //Show the x axis grid
            myPane.XAxis.MajorGrid.IsVisible = true;
            //myPane.XAxis.MinorGrid.IsVisible = true;
            // Make the Y axis scale red
            myPane.YAxis.Scale.FontSpec.FontColor = Color.Black;
            myPane.YAxis.Title.FontSpec.FontColor = Color.Black;

            // turn off the opposite tics so the Y tics don't show up on the Y2 axis
            myPane.YAxis.MajorTic.IsOpposite = true;
            myPane.YAxis.MinorTic.IsOpposite = true;
            // Don't display the Y zero line
            //myPane.YAxis.MajorGrid.IsZeroLine = false;
            // Align the Y axis labels so they are flush to the axis
            myPane.YAxis.Scale.Align = AlignP.Center;
            // Manually set the axis range
            myPane.YAxis.Scale.Min = 0;
            myPane.YAxis.Scale.Max = 1200;
            //myPane.YAxis.Scale.
            myPane.XAxis.Scale.Max = 498;
            //myPane.YAxis.Scale.FormatAuto = true;
            myPane.YAxis.MajorGrid.IsVisible = true;
            myPane.AxisChange();
            //Fin ZedGraph

        }// MovContCLK()

        public void SendPacketHandlerMovCont(object sender, UsbBuffer OutBuffer)
        {
            // Fill entire buffer with 0xFF
            OutBuffer.clear();
            if (ToggleLedD4Pending == true)
            {
                // The first byte is the "Report ID" and does not get sent over the USB bus. Always set = 0.
                OutBuffer.buffer[0] = 0;
                // 0x80 is the "Toggle LED 2" command in the firmware
                OutBuffer.buffer[1] = 0x83;//cambio a 0x83 para probar con el led4. Led2 0x80
                ToggleLedD4Pending = false;
                LastCommand = 0x83;
            }
            else if (MarchaPending == true)
            {// The first byte is the "Report ID" and does not get sent over the USB bus. Always set = 0.
                OutBuffer.buffer[0] = 0;
                // 0x86 is the "Marcha" command in the firmware
                OutBuffer.buffer[1] = 0x86;
                OutBuffer.buffer[2] = contadorTemp0[0];
                OutBuffer.buffer[3] = contadorTemp0[1];
                OutBuffer.buffer[4] = SentidoGiro;
                OutBuffer.buffer[5] = 0;//CLK control mode no serie
                OutBuffer.buffer[6] = StepResolution;// Resolucón del paso
                LastCommand = 0x86;
                MarchaPending = false;
            }
            else if (ParoPending == true)
            {// The first byte is the "Report ID" and does not get sent over the USB bus. Always set = 0.
                OutBuffer.buffer[0] = 0;
                // 0x85 is the "Paro" command in the firmware
                OutBuffer.buffer[1] = 0x85;
                ParoPending = false;
                LastCommand = 0x85;

            }
            
            else if (Timer0Pending == true)
            {
                // The first byte is the "Report ID" and does not get sent over the USB bus.  Always set = 0.
                OutBuffer.buffer[0] = 0x00;
                // READ_POT command (see the firmware source code), gets 10-bit ADC Value
                OutBuffer.buffer[1] = 0x37;
                LastCommand = 0x37;
                Timer0Pending =false;
            }
           
            // Request that this buffer be sent
            OutBuffer.RequestTransfer = true;
        }
        // HidUtility informs us if the requested transfer was successful
        // Schedule to request a packet if the transfer was successful
        public void PacketSentHandler(object sender, UsbBuffer OutBuffer)
        {
             if (LastCommand == 0x83 || LastCommand == 0x85 || LastCommand == 0x86)
            {
                WaitingForDevice = false;
            }
            else
            {
                WaitingForDevice = OutBuffer.TransferSuccessful;
            }
/*
            if (OutBuffer.TransferSuccessful)
            {
                ++TxCount;
            }
            else
            {
                ++TxFailedCount;
            }
            */
        }
        // HidUtility informs us if the requested transfer was successful and provides us with the received packet
        //Aqui se usa para intentar que funcione la barra analógica
        public void PacketReceivedHandler(object sender, UsbBuffer InBuffer)
        {
            //WriteLog(string.Format("PacketReceivedHandler: {0:X2}", InBuffer.buffer[1]), false);
            //WaitingForDevice = false;
            if (InBuffer.buffer[1] == 0x37)
            {
                //Need to reformat the data from two unsigned chars into one unsigned int.
                AdcValue = (uint)(InBuffer.buffer[3] << 8) + InBuffer.buffer[2];
            }

            /*           
            //Fin código nuevo
            if (InBuffer.TransferSuccessful)
            {
                ++RxCount;
            }
            else
            {
                ++RxFailedCount;
            }
            */

        }
        // HidUtility asks if a packet should be requested from the device
        // Request a packet if a packet has been successfully sent to the device before
        public void ReceivePacketHandler(object sender, UsbBuffer InBuffer)
        {
            InBuffer.RequestTransfer = WaitingForDevice;
            //WriteLog(string.Format("ReceivePacketHandler: {0}", WaitingForDevice), false);

        }
        private void button1_Click(object sender, EventArgs e)
        {
            ToggleLedD4Pending = true;
        }

        private void radioButtonMarcha_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonMarcha.Checked == true) {
                if (controlMovimientoContinuo.comprobarFrecuenciaCLK(textBoxFrecuenciaCLK.Text,out float float_frecuenciaCLK) == true &
                    StepResolutionString!=null)
            {
                  
                MarchaPending = true;
                    //obtiene los byte a cargar en TMR0H y el TMR0L para la fr del CLK del TC78H
                    contadorTemp0 = 
                        BitConverter.GetBytes(controlMovimientoContinuo.convertirFrecuenciaCLK(StepResolution, float_frecuenciaCLK));
                    if (this.radioButtonCW.Checked)
                        SentidoGiro = 1;
                    else
                        SentidoGiro = 0;
            }
            else if (controlMovimientoContinuo.comprobarFrecuenciaCLK(textBoxFrecuenciaCLK.Text, out float a) == false &
                        StepResolutionString == null)
                 MessageBox.Show("Dato de frecuencia erroneo y no seleccionada resolución del paso");
               
                else if (controlMovimientoContinuo.comprobarFrecuenciaCLK(textBoxFrecuenciaCLK.Text, out float b) == true &
                        StepResolutionString == null)
               
                    MessageBox.Show("No seleccionada resolución del paso");
              
                else
                    MessageBox.Show("Dato de frecuencia erroneo");

            }
            else
                MarchaPending = false;
        }
            
        
        /* A USB device has been added
        // Update the event log and device list
        void DeviceAddedHandler(object sender, Device dev)
        {
            //WriteLog("Device added: " + dev.ToString(), false);
            RefreshDeviceList();
        }

        private void RefreshDeviceList()
        {
            string txt = "";
            foreach (Device dev in HidUtil.DeviceList)
            {
                string devString = string.Format("VID=0x{0:X4} PID=0x{1:X4}: {2} ({3})", dev.Vid, dev.Pid, dev.Caption, dev.Manufacturer);
                txt += devString + Environment.NewLine;
            }
        DevicesTextBox.Text = txt.TrimEnd('\n');
        }
        */
        // Connection status of our selected device has changed
        // Update the user interface
        void ConnectionStatusChangedHandler(object sender, HidUtility.ConnectionStatusEventArgs e)
        {
            // Write log entry
            //WriteLog("Connection status changed to: " + e.ToString(), false);
            // Update user interface
            switch (e.ConnectionStatus)
            {
                case HidUtility.UsbConnectionStatus.Connected:
                    StatusText.Text = string.Format("18F4550 (Connection status = {0})", e.ConnectionStatus.ToString());
                 //   SetUserInterfaceStatus(true);
                   // ConnectedTimestamp = DateTime.Now;
                    break;
                case HidUtility.UsbConnectionStatus.Disconnected:
                    StatusText.Text = string.Format("18F4550 (Connection status = {0})", e.ConnectionStatus.ToString());
                   // AnalogBar.Value = 0;
                   //SetUserInterfaceStatus(false);

                    break;
                case HidUtility.UsbConnectionStatus.NotWorking:
                    StatusText.Text = string.Format("18F4550 attached but not working (Connection status = {0})", e.ConnectionStatus.ToString());
                  //  AnalogBar.Value = 0;
                  //  SetUserInterfaceStatus(false);
                    break;
            }
            /*
            UpdateStatistics();
            UpdatePushbutton();
            // UpdateTemp0();
            UpdateAdcBar();
            */
        }

        private void radioButtonParo_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonParo.Checked == true)
                ParoPending = true;
            else
                ParoPending = false;
        }

        private void stepResolutionDoubleClick(object sender, EventArgs e)
        {
            StepResolutionString = listBoxStepResolutionMode.SelectedItem.ToString();

           switch (StepResolutionString)
            {
                case "STEP_RESOLUTION_VARIABLE_1_2":
                    StepResolution = 1;
                    break;
                case "STEP_RESOLUTION_VARIABLE_1_4":
                    StepResolution = 2;
                    break;
                case "STEP_RESOLUTION_VARIABLE_1_8":
                    StepResolution = 3;
                    break;
                case "STEP_RESOLUTION_VARIABLE_1_16":
                    StepResolution = 4;
                    break;
                case "STEP_RESOLUTION_VARIABLE_1_32":
                    StepResolution = 5;
                    break;
                case "STEP_RESOLUTION_VARIABLE_1_64":
                    StepResolution = 6;
                    break;
                case "STEP_RESOLUTION_VARIABLE_1_128":
                    StepResolution = 7;
                    break;
                case "STEP_RESOLUTION_FIXED_FULL":
                    StepResolution = 8;
                    break;
                case "STEP_RESOLUTION_FIXED_2":
                    StepResolution = 9;
                    break;
                case "STEP_RESOLUTION_FIXED_4":
                    StepResolution = 10;
                    break;
                case "STEP_RESOLUTION_FIXED_8":
                    StepResolution = 11;
                    break;
                case "STEP_RESOLUTION_FIXED_16":
                    StepResolution = 12;
                    break;
                case "STEP_RESOLUTION_FIXED_32":
                    StepResolution = 13;
                    break;
                case "STEP_RESOLUTION_FIXED_64":
                    StepResolution = 14;
                    break;
                case "STEP_RESOLUTION_FIXED_128":
                    StepResolution = 15;
                    break;
                default:
                    StepResolution = 8;
                    break;
            }

        }
        //Update ADC bar
        private void UpdateAdcBar()
        {
            int i;
            //si no se ha actualizado en 6*20=120 ms para que se actualice
            tick++;
            // Ui operations are relatively costly so only update if the value has changed
            if (AnalogBarMovCont.Value != (int)AdcValue || tick>20)
                
            {
                tick=0;
                AnalogBarMovCont.Value = (int)AdcValue;
                
                //Inicio ZedGraph
                PointPairList list = new PointPairList();

                Array.Copy(DatosVelocidadZed, 1, Dato_VelocidadZed, 0, 499);
                Dato_VelocidadZed[499] = (short)AdcValue; //Guardamos en la matriz el nuevo dato a incluir en la representacion.
                                                   //una vez actualizada la matriz se copia en la que se usa para plotearla.
                Dato_VelocidadZed.CopyTo(DatosVelocidadZed, 0);
                for (i = 0; i < 500; i++)
                {
                    list.Add(i, DatosVelocidadZed[i]);
                }
                // Generate a red curve with diamond symbols, and "Alpha" in the legend
                CurveList curves = myPane.CurveList;
                curves.Clear();
                zedGraphControl1.Refresh();
                LineItem myCurve = myPane.AddCurve(null,
                    list, Color.Red, SymbolType.None);

                myCurve.Line.Width = 2.0F;
                myCurve.Line.IsSmooth = true;
                myCurve.Line.SmoothTension = 0.5F;
                //myCurve.Line.StepType = StepType.ForwardStep;
                zedGraphControl1.Refresh();
                zedGraphControl1.AxisChange();
                myPane.AxisChange();
                // myCurve.Symbol.Fill = new Fill(Color.Red);

                //Fin ZedGraph
            }
        }

        private void FormUpdateTimer_Tick(object sender, EventArgs e)
        {
            Timer0Pending = true;
            UpdateAdcBar();
        }
    }

}
