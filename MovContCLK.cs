using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HidUtilityNuget;

namespace HidDemoWindowsForms
{
    public partial class MovContCLK : Form
    {
        bool ToggleLedD2Pending = false;
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
            HidUtil.RaiseSendPacketEvent += SendPacketHandler;
            //HidUtil.RaisePacketSentEvent += PacketSentHandler;
            //HidUtil.RaiseReceivePacketEvent += ReceivePacketHandler;
            //HidUtil.RaisePacketReceivedEvent += PacketReceivedHandler;
            // Initial attempt to connect
            HidUtil.SelectDevice(new HidUtilityNuget.Device(0x04D8, 0X003F));
        }
        public void SendPacketHandler(object sender, UsbBuffer OutBuffer)
        {
            // Fill entire buffer with 0xFF
            OutBuffer.clear();
            if (ToggleLedD2Pending == true)
            {
                // The first byte is the "Report ID" and does not get sent over the USB bus. Always set = 0.
                OutBuffer.buffer[0] = 0;
                // 0x80 is the "Toggle LED 2" command in the firmware
                OutBuffer.buffer[1] = 0x83;//cambio a 0x83 para probar con el led4. Led2 0x80
                ToggleLedD2Pending = false;
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
            // Request that this buffer be sent
            OutBuffer.RequestTransfer = true;
        }
        

        private void button1_Click(object sender, EventArgs e)
        {
            ToggleLedD2Pending = true;
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
    }

}
