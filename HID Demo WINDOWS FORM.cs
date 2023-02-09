
using HidUtilityNuget;
using System;
using System.Windows.Forms;

namespace HidDemoWindowsForms
{
    public partial class Form1 : Form
    {
        // An instance of HidUtility that will do all the heavy lifting
        HidUtility HidUtil;

        // The device we are currently connected to
        Device ConnectedDevice = null;


        // Vendor and Product ID of the device we want to connect to
        ushort VID = 0x04D8;
        ushort PID = 0x003F;
       
        // Global variables used by the form / application
        byte LastCommand = 0x81;
        bool WaitingForDevice = false;
        bool PushbuttonPressed = false;
        bool PushbuttonS3Pressed = false;//Código nuevo
        bool ToggleLedD2Pending = false;
        bool ToggleLedD4Pending = false; //Código nuevo
        //bool Timer0Pending = false; //Código nuevo
        //bool Temp0Contando = false; //Código nuevo
        uint Temp0Contador = 0;//Código nuevo
        uint AdcValue = 0;
        DateTime ConnectedTimestamp = DateTime.Now;
        uint TxCount = 0;
        uint TxFailedCount = 0;
        uint RxCount = 0;
        uint RxFailedCount = 0;

        /*
         * Local function definitions
         */

        // Populate the device list
        // This function is called when the program is started and every time a device is connected or disconnected
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

        private void UpdateStatistics()
        {
            if(HidUtil.ConnectionStatus== HidUtility.UsbConnectionStatus.Connected)
            {
                //Save time elapsed since the device was connected
                TimeSpan uptime = DateTime.Now - ConnectedTimestamp;
                //Update uptime
                string tmp = string.Format("Uptime: {0}", uptime.ToString(@"hh\:mm\:ss\.f"));
                if (this.UptimeText.Text != tmp)
                {
                    this.UptimeText.Text = tmp;
                }
                //Update TX statistics
                tmp = string.Format("Packets sent (failed): {0} ({1})", TxCount, TxFailedCount);
                if(TxCountText.Text!=tmp)
                {
                    TxCountText.Text = tmp;
                }
                if (TxCount != 0)
                {
                    
                    tmp = string.Format("TX Speed: {0:0.00} packets per second", TxCount/uptime.TotalSeconds);
                    if (TxSpeedText.Text != tmp)
                    {
                        TxSpeedText.Text = tmp;
                    }
                }
                //Update RX statistics
                tmp = string.Format("Packets received (failed): {0} ({1})", RxCount, RxFailedCount);
                if (RxCountText.Text != tmp)
                {
                    RxCountText.Text = tmp;
                }
                if (RxCount != 0)
                {

                    tmp = string.Format("RX Speed: {0:0.00} packets per second", RxCount / uptime.TotalSeconds);
                    if (RxSpeedText.Text != tmp)
                    {
                        RxSpeedText.Text = tmp;
                    }
                }
            }
            else
            {
                UptimeText.Text = "Uptime: -";
                TxCountText.Text = "Packets sent (failed): -";
                TxSpeedText.Text = "Rx Speed: n/a";
                RxCountText.Text = "Packets received (failed): -";
                RxSpeedText.Text = "RX Speed: n/a";
            }
        }

        // Update Pushbutton status
        private void UpdatePushbutton()
        {
            string tmp;
            if (PushbuttonPressed)
            {
                tmp = "Pushbuttton State: Pressed";
            }
            else
            {
                tmp = "Pushbuttton State: Not Pressed";
            }
            // Only update if value has changed. Otherwise the text will flicker
            if (PushbuttonText.Text != tmp)
            {
                PushbuttonText.Text = tmp;
            }
            //Código nuevo
            if (PushbuttonS3Pressed)
            {
                ToggleLedD4Pending = true;
            }
            //Fin de código nuevo
        }
        /*Update estado del Temp0 de la PIC
        private void UpdateTemp0()
        {
            string tmp;
               uint tmpCont;
            if (Temp0Contando)
            {
                tmp = "1";
            }
            else
            {
                tmp = "0";
            }
            tmpCont = Temp0Contador;
            // Only update if value has changed. Otherwise the text will flicker
            if (Temp_0_activoText.Text != tmp)
            {
                Temp_0_activoText.Text = tmp;
            }
            // Only update if value has changed. Otherwise the text will flicker
            if (TempContLabel.Text != tmpCont.ToString())
            {
                TempContLabel.Text = tmpCont.ToString();
            }

        }
        */

        //Update ADC bar
        private void UpdateAdcBar()
        {
            // Ui operations are relatively costly so only update if the value has changed
            if (AnalogBar.Value != (int)AdcValue)
            {
                AnalogBar.Value = (int)AdcValue;
            }
        }

        //Enable or disable controls
        private void SetUserInterfaceStatus(bool enabled)
        {
            this.ToggleLedD2Button.Enabled = enabled;
            this.AnalogLabel.Enabled = enabled;
            this.AnalogBar.Enabled = enabled;
            this.PushbuttonText.Enabled = enabled;
            this.TxCountText.Enabled = enabled;
            this.RxCountText.Enabled = enabled;
            this.UptimeText.Enabled = enabled;
            this.RxSpeedText.Enabled = enabled;
            this.TxSpeedText.Enabled = enabled;
            if(!enabled)
            {
                TxCount = 0;
                TxFailedCount = 0;
                RxCount = 0;
                RxFailedCount = 0;
            }
        }

        // Add a line to the activity log text box
        void WriteLog(string message, bool clear)
        {
            // Replace content
            if(clear)
            {
                LogTextBox.Text = string.Format("{0}: {1}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"), message);
            }
            // Add new line
            else
            {
                LogTextBox.Text += Environment.NewLine + string.Format("{0}: {1}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"), message);
            }
            // Scroll to bottom
            LogTextBox.SelectionStart = LogTextBox.Text.Length;
            LogTextBox.ScrollToCaret();
        }

        // Try to convert a (hexadecimal) string to an unsigned 16-bit integer
        // Return 0 if the conversion fails
        // This function is used to parse the PID and VID text boxes
        private ushort ParseHex(string input)
        {
            input = input.ToLower();
            if (input.Length >= 2)
            {
                if (input.Substring(0, 2) == "0x")
                {
                    input = input.Substring(2);
                }
            }
            try
            {
                ushort value = ushort.Parse(input, System.Globalization.NumberStyles.HexNumber);
                return value;
            }
            catch
            {
                return 0;
            }
        }


        /*
         * Form callback functions
         */
         
        // Check if the ENTER key has been pressed inside the VID text box
        // Parse the string if that is the case
        private void VidTextBox_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                VidTextBox_LostFocus(sender, e);
            }
        }

        // Parse the content of the VID text box when focus is lost
        private void VidTextBox_LostFocus(object sender, EventArgs e)
        {
            ushort vid = ParseHex(VidTextBox.Text);
            if (vid != VID)
            {
                // Save the new VID
                VID = vid;
                // Try to connect using the new PID
                WriteLog(string.Format("Attempt to connect with Vid=0x{0:X4}, Pid=0x{1:X4}", VID, PID), false);
                HidUtil.SelectDevice(new Device(VID, PID));
            }
            // Nicely format the string
            VidTextBox.Text = string.Format("0x{0:X4}", VID);
        }

        // Check if the ENTER key has been pressed inside the PID text box
        // Parse the string if that is the case
        private void PidTextBox_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                PidTextBox_LostFocus(sender, e);
            }
        }

        // Parse the content of the PID text box when focus is lost
        private void PidTextBox_LostFocus(object sender, EventArgs e)
        {
            ushort pid = ParseHex(PidTextBox.Text);
            //Check if PID has changed
            if (pid != PID)
            {
                // Save the new PID
                PID = pid;
                // Try to connect using the new PID
                WriteLog(string.Format("Attempt to connect with Vid=0x{0:X4}, Pid=0x{1:X4}", VID, PID), false);
                HidUtil.SelectDevice(new Device(VID, PID));
            }
            // Nicely format the string
            PidTextBox.Text = string.Format("0x{0:X4}", pid);
        }

        // Schedule to toggle LED D2 if the corresponding button has been clicked
        private void ToggleLedD2Button_Click(object sender, EventArgs e)
        {
            ToggleLedD2Pending = true;
        }
        // Schedule to toggle LED D4 if the corresponding button has been clicked
        private void ToggleLedD4Button_Click(object sender, EventArgs e)
        {
            ToggleLedD4Pending = true;
        }
        //Schedule to start Timer0 count
        private void IniciaTempor0Button_Click(object sender, EventArgs e)
        {
            //Timer0Pending = true;     
        }
       
       
         /* HidUtility callback functions
         */

        // A USB device has been removed
        // Update the event log and device list
        void DeviceRemovedHandler(object sender, Device dev)
        {
            WriteLog("Device removed: " + dev.ToString(), false);
            RefreshDeviceList();
        }

        // A USB device has been added
        // Update the event log and device list
        void DeviceAddedHandler(object sender, Device dev)
        {
            WriteLog("Device added: " + dev.ToString(), false);
            RefreshDeviceList();
        }

        // Connection status of our selected device has changed
        // Update the user interface
        void ConnectionStatusChangedHandler(object sender, HidUtility.ConnectionStatusEventArgs e)
        {
            // Write log entry
            WriteLog("Connection status changed to: " + e.ToString(), false);
            // Update user interface
            switch (e.ConnectionStatus)
            {
                case HidUtility.UsbConnectionStatus.Connected:
                    StatusText.Text = string.Format("Device Found (Connection status = {0})", e.ConnectionStatus.ToString());
                    SetUserInterfaceStatus(true);
                    ConnectedTimestamp = DateTime.Now;
                    break;
                case HidUtility.UsbConnectionStatus.Disconnected:
                    StatusText.Text = string.Format("Device Not Detected (Connection status = {0})", e.ConnectionStatus.ToString());
                    AnalogBar.Value = 0;
                    SetUserInterfaceStatus(false);
                    
                    break;
                case HidUtility.UsbConnectionStatus.NotWorking:
                    StatusText.Text = string.Format("Device attached but not working (Connection status = {0})", e.ConnectionStatus.ToString());
                    AnalogBar.Value = 0;
                    SetUserInterfaceStatus(false);
                    break;
            }
            UpdateStatistics();
            UpdatePushbutton();
            UpdateAdcBar();
        }

        // HidUtility asks if a packet should be sent to the device
        // Prepare the buffer and request a transfer
        public void SendPacketHandler(object sender, UsbBuffer OutBuffer)
        {
            // Fill entire buffer with 0xFF
            OutBuffer.clear();
            if (ToggleLedD2Pending == true)
            {
                // The first byte is the "Report ID" and does not get sent over the USB bus. Always set = 0.
                OutBuffer.buffer[0] = 0;
                // 0x80 is the "Toggle LED 2" command in the firmware
                OutBuffer.buffer[1] = 0x80; 
                ToggleLedD2Pending = false;
                LastCommand = 0x80;
            }
            //Codigo nuevo en esta parte
            else if (ToggleLedD4Pending == true)
            {
                // The first byte is the "Report ID" and does not get sent over the USB bus. Always set = 0.
                OutBuffer.buffer[0] = 0;
                // 0x83 is the "Toggle LED 4" command in the firmware
                OutBuffer.buffer[1] = 0x83;
                ToggleLedD4Pending = false;
                LastCommand = 0x83;
            }
            //La orden de marcha del motor no se usa ya en esta form
            /*
            else if (Timer0Pending == true)
            {
                OutBuffer.buffer[0] = 0;
                // 0x86 is the "Marcha" command in the firmware
                OutBuffer.buffer[1] = 0x86;
                OutBuffer.buffer[2] = 0xfA;
                OutBuffer.buffer[3] = 0xf6;
                OutBuffer.buffer[4] = 1;
                Timer0Pending = false;
                LastCommand = 0x86;
                
            }
            */
            else if (LastCommand == 0x85)
            {
                // The first byte is the "Report ID" and does not get sent over the USB bus.  Always set = 0.
                OutBuffer.buffer[0] = 0x00;
                // 0x82 is the "Get Pushbutton S3 State para encender Led D4" command in the firmware
                OutBuffer.buffer[1] = 0x82;
                LastCommand = 0x82;
            }

            else if (LastCommand == 0x82)
            {
                // The first byte is the "Report ID" and does not get sent over the USB bus.  Always set = 0.
                OutBuffer.buffer[0] = 0x00;
                // 0x81 is the Get Pushbutton S2 State para mostrar "Pushbutton:pressed" in the screen
                OutBuffer.buffer[1] = 0x81;
                LastCommand = 0x81;
            }
           
            else if (LastCommand==0x81)
            {
                // The first byte is the "Report ID" and does not get sent over the USB bus.  Always set = 0.
                OutBuffer.buffer[0] = 0x00;
                // READ_POT command (see the firmware source code), gets 10-bit ADC Value
                OutBuffer.buffer[1] = 0x37;    
                LastCommand = 0x37;
            }
            else
            {
                // The first byte is the "Report ID" and does not get sent over the USB bus.  Always set = 0.
                OutBuffer.buffer[0] = 0x00;
                
                OutBuffer.buffer[1] = 0x81;  // OutBuffer.buffer[1] = 0x81;        
                LastCommand = 0x81;          //LastCommand = 0x81;        
            }
            // Request that this buffer be sent
            OutBuffer.RequestTransfer = true;
        }

        // HidUtility informs us if the requested transfer was successful
        // Schedule to request a packet if the transfer was successful
        public void PacketSentHandler(object sender, UsbBuffer OutBuffer)
        {
           // if (LastCommand == 0x80 || LastCommand == 0x83 || LastCommand == 0x86)

                if (LastCommand == 0x80 || LastCommand == 0x83)
            {
                WaitingForDevice = false;
            }
            else
            {
                WaitingForDevice = OutBuffer.TransferSuccessful;
            }
            
            if(OutBuffer.TransferSuccessful)
            {
                ++TxCount;
            }
            else
            {
                ++TxFailedCount;
            }
        }

        // HidUtility asks if a packet should be requested from the device
        // Request a packet if a packet has been successfully sent to the device before
        public void ReceivePacketHandler(object sender, UsbBuffer InBuffer)
        {
            InBuffer.RequestTransfer = WaitingForDevice;
            //WriteLog(string.Format("ReceivePacketHandler: {0}", WaitingForDevice), false);

        }

        // HidUtility informs us if the requested transfer was successful and provides us with the received packet
        public void PacketReceivedHandler(object sender, UsbBuffer InBuffer)
        {
            //WriteLog(string.Format("PacketReceivedHandler: {0:X2}", InBuffer.buffer[1]), false);
            WaitingForDevice = false;
            if (InBuffer.buffer[1] == 0x37)
            {
                //Need to reformat the data from two unsigned chars into one unsigned int.
                AdcValue = (uint)(InBuffer.buffer[3] << 8) + InBuffer.buffer[2];
            }
            if (InBuffer.buffer[1] == 0x81)
            {
                if (InBuffer.buffer[2] == 0x01)
                {
                    PushbuttonPressed = false;
                }
                if (InBuffer.buffer[2] == 0x00)
                {
                    PushbuttonPressed = true;
                }
            }
            //Código nuevo
            if (InBuffer.buffer[1] == 0x82)
            {
                if (InBuffer.buffer[2] == 0x01)
                {
                    PushbuttonS3Pressed = false;
                }
                if (InBuffer.buffer[2] == 0x00)
                {
                    PushbuttonS3Pressed = true;
                }
            }
            /*Temporizador
            if (InBuffer.buffer[1] == 0x85)
            {
                if (InBuffer.buffer[2] == 0x01)
                {
                    Temp0Contando = true;
                }
                if (InBuffer.buffer[2] == 0x00)
                {
                    Temp0Contando = false;
                }
                Temp0Contador=InBuffer.buffer[3];
            }
            */
            //Fin código nuevo
            if (InBuffer.TransferSuccessful)
            {
                ++RxCount;
            }
            else
            {
                ++RxFailedCount;
            }
        }


        public unsafe Form1()
        {
            InitializeComponent();

            // Get an instance of HidUtility
            HidUtil = new HidUtility();

            // Initialize user interface
            SetUserInterfaceStatus(false);

            // Register event handlers
            HidUtil.RaiseDeviceRemovedEvent += DeviceRemovedHandler;
            HidUtil.RaiseDeviceAddedEvent += DeviceAddedHandler;
            HidUtil.RaiseConnectionStatusChangedEvent += ConnectionStatusChangedHandler;
            HidUtil.RaiseSendPacketEvent += SendPacketHandler;
            HidUtil.RaisePacketSentEvent += PacketSentHandler;
            HidUtil.RaiseReceivePacketEvent += ReceivePacketHandler;
            HidUtil.RaisePacketReceivedEvent += PacketReceivedHandler;

            // Fill the PID and VID text boxes
            VidTextBox.Text = string.Format("0x{0:X4}", VID);
            PidTextBox.Text = string.Format("0x{0:X4}", PID);

            // Initialize tool tips, to provide pop up help when the mouse cursor is moved over objects on the form.
            ANxVoltageToolTip.SetToolTip(this.AnalogLabel, "If using a board/PIM without a potentiometer, apply an adjustable voltage to the I/O pin.");
            ANxVoltageToolTip.SetToolTip(this.AnalogBar, "If using a board/PIM without a potentiometer, apply an adjustable voltage to the I/O pin.");
            ToggleLEDToolTip.SetToolTip(this.ToggleLedD2Button, "Sends a packet of data to the USB device.");
            PushbuttonStateTooltip.SetToolTip(this.PushbuttonText, "Try pressing pushbuttons on the USB demo board.");

            // Populate device list TextBox
            RefreshDeviceList();

            // Initiate Log TextBox
            WriteLog("System started", true);

            // Initial attempt to connect
            HidUtil.SelectDevice(new HidUtilityNuget.Device(VID, PID));
        } //Form1()


        private void FormUpdateTimer_Tick(object sender, EventArgs e)
        {
            // Update user interface based on the attachment state of the USB device.
            switch (HidUtil.ConnectionStatus)
            {
                case HidUtility.UsbConnectionStatus.Connected:
                    //Timer0Pending =true;
                    UpdateStatistics();
                    UpdatePushbutton();
                    //UpdateTemp0(); PASA A CONTROLAR EL clk DE MARCHA
                    UpdateAdcBar();
                    break;
                case HidUtility.UsbConnectionStatus.Disconnected:
                    // Nothing to do
                    break;
                case HidUtility.UsbConnectionStatus.NotWorking:
                    // Nothing to do
                    break;
            }
        }

        private void Exit_Clik(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void MovContCLK_Click(object sender, EventArgs e)
        {
            MovContCLK movContCLK = new MovContCLK();
            movContCLK.ShowDialog();
        }
    } //public partial class Form1 : Form
} //namespace HidDemoWindowsForms