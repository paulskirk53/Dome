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
            Dome.Homeplace = numericUpDownHomeAzimuth.Value.ToString();
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
            numericUpDownHomeAzimuth.Value = (decimal)Dome.HomeAzimuth;  // HomeAzimuth comes from the driver ReadProfile()


            


        }

        //new code sept '22


        private string portFinder(ASCOM.Utilities.Serial testPort, string mcuName)  //mcuName will be e.g "encoder" or "stepper"
        {
            /*
             * This routine uses a test port to cycle through the portnames (COM1, COM3 etc), checking each port 
             *  by sending a string recognised by a particular MCU e.g. stepper# or encoder#
             *  if the mcu is on the port, it responds with stepper# or encoder#
             * */
            setupThePort(testPort);            //set the parameters for testport - baud etc
            bool found = false;
            foreach (string portName in GetUnusedSerialPorts())     // GetUnusedSerialPorts forms a list of COM ports which are available
            {
                found = checkforMCU(testPort, portName, mcuName);     // this checks if the current portName responds to mcuName (stepper# / emcoder#)
                if (found)
                {

                    testPort.Connected = false;                    //disconnect the port
                    return portName;

                }


            }
            return null;                // if no ports respond to queries (e.g. perhaps mcus are not connected), the nukk return is picked by the try - catch exception
                                        // of encoder connect or stepper connect
        }

        private bool checkforMCU(ASCOM.Utilities.Serial testPort, string portName, string MCUDescription)
        {

            testPort.PortName = portName;  //                      
            testPort.Connected = true;

            //now send data and see what comes back
            try
            {

                testPort.Transmit(MCUDescription);            // transmits encoder# or stepper# depending upon where called
                string response = testPort.ReceiveTerminated("#");   // not all ports respond to a query and those which don't respond will timeout


                if (response == MCUDescription)
                {

                    return true;            //mcu response match
                }

                testPort.Connected = false;
                return false;              // if there was a response it was not the right MCU
            }
            catch (Exception e)     //TimeoutException
            {

                testPort.Connected = false;    // no response

            }

            return false;
        }
        private void setupThePort(ASCOM.Utilities.Serial testPort)
        {
            //set all the port propereties

            testPort.DTREnable = false;
            testPort.RTSEnable = false;
            testPort.ReceiveTimeout = 5;

            testPort.Speed = ASCOM.Utilities.SerialSpeed.ps19200;



        }



        private string[] GetUnusedSerialPorts()                     //string[] is a string array
        {
            using (ASCOM.Utilities.Serial temp = new ASCOM.Utilities.Serial())
            {
                var ports = new List<string>(temp.AvailableCOMPorts); // List<T> class constructor is used to create a List object of type T. So in this case, available comports
                var busyPorts = new List<string>();

                foreach (var port in ports)
                {
                    try
                    {
                        temp.PortName = port;

                        temp.Connected = true;
                        temp.Connected = false;
                    }
                    catch (Exception)
                    {

                        // If we get here then the current port is currently in use so add it to the busy ports list.

                        busyPorts.Add(port);
                    }
                }

                // Remove the busy ports from the return list.

                foreach (var busyPort in busyPorts)
                {
                    ports.Remove(busyPort);
                }

                return ports.ToArray();               // I think this returns a clean sequential list - no gaps  
            }
        }





    }  // end public partial class


}

       //
       
    
