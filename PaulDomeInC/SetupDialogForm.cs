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
            Dome.ShutterComPort = (string)comboBoxComPortShutter.SelectedItem;   //user selected port for stepper
            Dome.Parkplace = numericUpDownParkAzimuth.Value.ToString();
            //System.Windows.Forms.MessageBox.Show(Dome.StepperComPort);            // put in to test -pops up a dialog showing value of comport selected

            // new code

            // open new gowerdome interface form - probably not here but in the chosser ok button if possible
            //  this.Hide();
            //  GowerDome_interface f2 = new GowerDome_interface();
            //  f2.ShowDialog();


            //end new code

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
            comboBoxComPortShutter.Items.Clear(); //pk code
            comboBoxComPort.Items.AddRange(System.IO.Ports.SerialPort.GetPortNames());             // use System.IO because it's static
            comboBoxComPortShutter.Items.AddRange(System.IO.Ports.SerialPort.GetPortNames());      // use System.IO because it's static
            comboBoxComPortStepper.Items.AddRange(System.IO.Ports.SerialPort.GetPortNames());      // pk code
            // select the current port if possible
            if (comboBoxComPort.Items.Contains(Dome.CompassComPort))   // see the driver code - this is set to a value of 4 - i.e. com4 is default
            {
                comboBoxComPort.SelectedItem = Dome.CompassComPort;    // the item that appears in the combobox at form load
            }
            //new

            if (comboBoxComPortStepper.Items.Contains(Dome.StepperComPort))   // see the driver code - this is set in the connected proerty
            {
                comboBoxComPortStepper.SelectedItem = Dome.StepperComPort;    // the item that appears in the combobox at form load
            }
            if (comboBoxComPortShutter.Items.Contains(Dome.ShutterComPort))   // see the driver code - this is set in the connected proerty
            {
                comboBoxComPortShutter.SelectedItem = Dome.ShutterComPort;    // the item that appears in the combobox at form load
            }
            //end new

            // the following line works to get the value from the ascom profile store into the numeric updown field on the setup dialog
            numericUpDownParkAzimuth.Value = (decimal)Dome.ParkAzimuth;  // ParkAzimuth comes from the driver ReadProfile()
        }

        private void comboBoxComPort_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void numericUpDownParkAzimuth_ValueChanged(object sender, EventArgs e)
        {

        }

        private void SetupDialogForm_Load(object sender, EventArgs e)
        {

        }
    }
}