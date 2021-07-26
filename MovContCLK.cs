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

namespace USB_HID_con_la_PICDEM_FSUSB_18F4550
{
    public partial class MovContCLK : Form
    {
        bool ToggleLedD2Pending = false;
        byte LastCommand = 0x80;

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
            //HidUtil.RaiseConnectionStatusChangedEvent += ConnectionStatusChangedHandler;
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
                OutBuffer.buffer[1] = 0x80;
                ToggleLedD2Pending = false;
                LastCommand = 0x80;
            }
            // Request that this buffer be sent
            OutBuffer.RequestTransfer = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ToggleLedD2Pending = true;
        }
    }
        
}
