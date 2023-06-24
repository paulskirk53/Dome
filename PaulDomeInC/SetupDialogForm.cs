using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using ASCOM.Utilities;
using ASCOM.GowerCDome;
using System.Threading;

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
         //   Dome.comPort = (string)comboBoxComPort.SelectedItem;
            Dome.tl.Enabled = chkTrace.Checked;
          //  Dome.CompassComPort = (string)comboBoxComPort.SelectedItem;          //user selected port for compass
            Dome.control_BoxComPort = (string)comboboxcontrol_box.SelectedItem;   //user selected port for stepper
            Dome.ShutterComPort = (string)comboBoxComPortShutter.SelectedItem;   //user selected port for stepper
            Dome.Parkplace = numericUpDownParkAzimuth.Value.ToString();
            Dome.Homeplace = numericUpDownHomeAzimuth.Value.ToString();
          //  System.Windows.Forms.MessageBox.Show(Dome.StepperComPort);            // put in to test -pops up a dialog showing value of comport selected

     

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
            cmdOK.Enabled = false;     // disable the ok button until ports are picked todo pk

            //set the three globals to false - these will be set to true if the com port combo box selection change committed event fires (i.e. a comport is picked by the user)

            myGlobals.check1 = false;
            myGlobals.check2 = false;
            myGlobals.check3 = false;

            chkTrace.Checked = Dome.tl.Enabled;
            // set the list of com ports to those that are currently available
           // comboBoxComPort.Items.Clear();        //compass
            comboboxcontrol_box.Items.Clear(); //pk code
            comboBoxComPortShutter.Items.Clear(); //pk code
            // comboBoxComPort.Items.AddRange(System.IO.Ports.SerialPort.GetPortNames());             // use System.IO because it's static
            comboBoxComPortShutter.Items.AddRange(System.IO.Ports.SerialPort.GetPortNames());      // use System.IO because it's static
            comboboxcontrol_box.Items.AddRange(System.IO.Ports.SerialPort.GetPortNames());      // pk code
            comboboxcontrol_box.Items.Remove("COM1");                                           //com1 not used by MCU
            // comboBoxComPort.Items.Remove("COM1");
            comboBoxComPortShutter.Items.Remove("COM1");

            // select the current port if possible
      //      if (comboBoxComPort.Items.Contains(Dome.CompassComPort))   // see the driver code - this is set to a value of 4 - i.e. com4 is default
      //      {
      //          comboBoxComPort.SelectedItem = Dome.CompassComPort;    // the item that appears in the combobox at form load
      //      }


            if (comboboxcontrol_box.Items.Contains(Dome.control_BoxComPort ))   // see the driver code - this is set in the connected property
            {
                comboboxcontrol_box.SelectedItem = Dome.control_BoxComPort;    // the item that appears in the combobox at form load
            }
            if (comboBoxComPortShutter.Items.Contains(Dome.ShutterComPort))   // see the driver code - this is set in the connected proerty
            {
                comboBoxComPortShutter.SelectedItem = Dome.ShutterComPort;    // the item that appears in the combobox at form load
            }

 




        }
            


            private string checkForNull( string portName, string mcu)
        {
            if (portName== null)
            {
                return mcu + " Unavailable check connection";
            }
            else
            {
                return mcu + " is on " + portName;
            }
        }

        private string portFinder(ASCOM.Utilities.Serial testPort, string mcuName, List<string> portlist)  //mcuName will be e.g "encoder" or "stepper"
        {
            //*
            // * This routine uses a test port to cycle through the portnames (COM1, COM3 etc), checking each port 
             //*  by sending a string recognised by a particular MCU e.g. stepper# or encoder#
             //*  if the mcu is on the port, it responds with stepper# or encoder#
             //*
            setupThePort(testPort);            //set the parameters for testport - baud etc
            bool found = false;
            foreach (string portName in portlist)     // GetUnusedSerialPorts forms a list of COM ports which are available
            {
               // MessageBox.Show("the port being checked is " + portName);        //tis worked
                found = checkforMCU(testPort, portName, mcuName);     // this checks if the current portName responds to mcuName (stepper# / emcoder#)
                if (found)
                {
                  //  MessageBox.Show("the port is found " + portName);
                    testPort.Connected = false;                    //disconnect the port
                    return portName;

                }


            }
            return null;                // if no ports respond to queries (e.g. perhaps mcus are not connected), the nukk return is picked by the try - catch exception
                                        // of encoder connect or stepper connect
          //  throw new NullReferenceException();
        }

        private bool checkforMCU(ASCOM.Utilities.Serial testPort, string portName, string MCUDescription)
        {

            testPort.PortName = portName;  //                      
            testPort.Connected = true;
            Thread.Sleep(500);           // delay (in mS) - essential if the MCU is Arduino with a bootloader. The Arduino requires time after the port is connected before it can respond to serial requests.
            
            // send data to the MCU and see what comes back
            try
            {
                
                // MessageBox.Show("Sending " + MCUDescription + " to port " + portName);
               
                testPort.Transmit(MCUDescription);                   // transmits encoder# or stepper# depending upon where called
                
            
                string  response = testPort.ReceiveTerminated("#");   // not all ports respond to a query and those which don't respond will timeout
             
               // MessageBox.Show("the response from the MCU " + response);
              
                if (response == MCUDescription)
                {
                    testPort.Connected = false;
                    return true;            //mcu response match
                }
                else
                {
                    testPort.Connected = false;
                    return false;
                }

               
               // return false;              // if there was a response it was not the right MCU
            }
            catch (Exception e)     //TimeoutException
            {

                testPort.Connected = false;    // no response
               // MessageBox.Show("the MCU  did not respond to  " + MCUDescription);
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

        

       

      //  private void comboBoxComPort_SelectionChangeCommitted(object sender, EventArgs e)   // bad name for the combo - this is the selection change for the azimuth comport
      //  {
      //      
      //      myGlobals.check1 = true;
      //      overallCheck();
      //  }

        private void comboBoxComPortShutter_SelectionChangeCommitted(object sender, EventArgs e)
        {
            myGlobals.check2 = true;
            overallCheck();
        }

        private void comboboxcontrol_box_SelectionChangeCommitted(object sender, EventArgs e)
        {
            myGlobals.check3 = true;
            overallCheck();
        }
        private void overallCheck()      // a way of checking if all the com ports have been picked by the end user.
        {
            if (myGlobals.check1 && myGlobals.check2 && myGlobals.check3)
            {
                cmdOK.Enabled = true;
            }
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            myGlobals.check1 = true;
            myGlobals.check2 = true;
            myGlobals.check3 = true;
            overallCheck();
        }

        private void BTNidcomports_Click(object sender, EventArgs e)
        {
            BTNidcomports.Text = "Waiting for ID";
            BTNidcomports.Refresh();



            ASCOM.Utilities.Serial tempPort = new ASCOM.Utilities.Serial();     // setup a variable as an ascom utils serial object

            var portlist = new List<string>(tempPort.AvailableCOMPorts);         // create a list of available comports on tempPort
            var busyPorts = new List<string>();
            portlist.Remove("COM1");                                             // COM1 is never used by MCUs


            //write some code to try to connect to the ports and if that fails, the port is busy, so remove it from the list..

            foreach (string port in portlist)
            {
                try
                {
                    tempPort.PortName = port;
                    tempPort.Connected = true;
                    tempPort.Connected = false;
                  //  MessageBox.Show("Try port " + port);
                }
                catch
                {
                    // if we get here connection failed, so remoe thport from the list
                    busyPorts.Add(port);
                   // MessageBox.Show("Catch port to remove " + port);
                }
            }    // end foreach

            //now remove the busy ports from the port list
            foreach (string port in busyPorts)
            {
                portlist.Remove(port);
            }

            portlist.ToArray();

            //now send id messages to each port in the list to find which MCU is attached to which port.
            label1.Text= "Please wait while ID takes place";
            label1.TextAlign = ContentAlignment.MiddleCenter;
            
            label1.BackColor = Color.OrangeRed;
            label1.Refresh();
            try
            {
                LBLShutter.BackColor = Color.DarkGray;
                //LBLAzimuth.BackColor = Color.DarkGray;
                LBLStepper.BackColor = Color.DarkGray;
                LBLShutter.Refresh();
                //LBLAzimuth.Refresh();
                LBLStepper.Refresh();

                LBLShutter.Text = "Shutter id in process....";
                LBLShutter.Refresh();

                string portName;                                                 //used to hold the name of the com port returned by portfinder()

                portName = portFinder(tempPort, "shutter#", portlist);          // this routine returns the port name that replied e.g. COM7
                LBLShutter.Text = checkForNull(portName, "Shutter");

                // check if port is unavailable and if so set the back colour to ornage
                if (LBLShutter.Text.Contains("Unavailable"))
                {
                    LBLShutter.BackColor = Color.Orange;
                }
                else
                {
                    LBLShutter.BackColor = Color.YellowGreen;

                }

                LBLShutter.Refresh();
                portlist.Remove(portName);                                      // remove from the portlist to reduce the list size and future processing time


                
               // LBLAzimuth.Text = "Azimuth id in process....";
               // LBLAzimuth.Refresh();

                portName = portFinder(tempPort, "azimuth#", portlist);
               // LBLAzimuth.Text = checkForNull(portName, "Azimuth encoder");
                
                // check if port is unavailable and if so set the back colour to ornage
               // if (LBLAzimuth.Text.Contains("Unavailable"))
               // {
               //     LBLAzimuth.BackColor = Color.Orange;
               // }
               // else
               // {
               //     
               //     LBLAzimuth.BackColor = Color.YellowGreen;
               // }

               // LBLAzimuth.Refresh();
                portlist.Remove(portName);

                portName = portFinder(tempPort, "controlbox#", portlist);
                
                LBLStepper.Text = "Dome drive id in process....";
                LBLStepper.Refresh();

                LBLStepper.Text = checkForNull(portName, "Dome drive");

                // check if port is unavailable and if so set the back colour to ornage
                if (LBLStepper.Text.Contains("Unavailable") )
                    {
                    LBLStepper.BackColor = Color.Orange;
                    }
                else
                   {
                    LBLStepper.BackColor = Color.YellowGreen;
                }
            }

            catch (Exception ex)
            {

                MessageBox.Show(" connection failed. Check the MCUs are on, connected, and in receive mode." + ex.Message);
            }


            label1.Text = " Port Identification complete";
            label1.BackColor = Color.YellowGreen;
            label1.TextAlign = ContentAlignment.MiddleCenter;

            //reset the command button text
            BTNidcomports.Text = "Identify Comports";

        }  // end id comports

    }  // end public partial class

    public static class myGlobals     // this is a way of making a global variable set, accessible fom anywhere in the namespace. It's used to determine if all the com ports have been selected.
    {
        public static bool check1 = false;
        public static bool check2 = false;
        public static bool check3 = false;

    }
}

       //
       
    
