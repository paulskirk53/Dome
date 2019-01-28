using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using ASCOM.Utilities;
using ASCOM.GowerCDome;

namespace ASCOM.GowerCDome
{
    [ComVisible(false)]					// Form not registered for COM!
    public partial class SetupDialogForm : Form
    {
        public SetupDialogForm()
        {
            InitializeComponent();
            // Initialise current values of user settings from the ASCOM Profile
            InitUI();
        }

        private void cmdOK_Click(object sender, EventArgs e) // OK button event handler
        {
            // Place any validation constraint checks here
            // Update the state variables with results from the dialogue
            Dome.comPort = (string)comboBoxComPort.SelectedItem;
            Dome.tl.Enabled = chkTrace.Checked; 
            Dome.CompassComPort = (string)comboBoxComPort.SelectedItem;          //user selected port for compass
            Dome.StepperComPort = (string)comboBoxComPortStepper.SelectedItem;   //user selected port for stepper
        }

        private void cmdCancel_Click(object sender, EventArgs e) // Cancel button event handler
        {
            Close();
        }

        private void BrowseToAscom(object sender, EventArgs e) // Click on ASCOM logo event handler
        {
            try
            {
                System.Diagnostics.Process.Start("http://ascom-standards.org/");
            }
            catch (System.ComponentModel.Win32Exception noBrowser)
            {
                if (noBrowser.ErrorCode == -2147467259)
                    MessageBox.Show(noBrowser.Message);
            }
            catch (System.Exception other)
            {
                MessageBox.Show(other.Message);
            }
        }

        private void InitUI()
        {
            chkTrace.Checked = Dome.tl.Enabled;
            // set the list of com ports to those that are currently available
            comboBoxComPort.Items.Clear();        //compass
            comboBoxComPortStepper.Items.Clear(); //pk code
            comboBoxComPort.Items.AddRange(System.IO.Ports.SerialPort.GetPortNames());      // use System.IO because it's static
            comboBoxComPortStepper.Items.AddRange(System.IO.Ports.SerialPort.GetPortNames());  // pk code
            // select the current port if possible
            if (comboBoxComPort.Items.Contains(Dome.comPort))   // see the driver code - this is set to a value of 4 - i.e. com4 is default
            {
                comboBoxComPort.SelectedItem = Dome.comPort;    // the item that appears in the combobox at form load
            }
        }

        private void comboBoxComPort_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}