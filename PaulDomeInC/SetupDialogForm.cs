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
using System.IO.Ports;

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
         
            Dome.tl.Enabled = chkTrace.Checked;
          
            Dome.control_BoxComPort = (string)comboboxcontrol_box.SelectedItem;   //user selected port for control box MCU
            Dome.ShutterComPort = (string)comboBoxComPortShutter.SelectedItem;   //user selected port for shutter mcu
            Dome.Parkplace = numericUpDownParkAzimuth.Value.ToString();
            Dome.Homeplace = numericUpDownHomeAzimuth.Value.ToString();
          

     

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

          

            chkTrace.Checked = Dome.tl.Enabled;
            // set the list of com ports to those that are currently available
           // comboBoxComPort.Items.Clear();        //compass
            comboboxcontrol_box.Items.Clear(); //pk code
            comboBoxComPortShutter.Items.Clear(); //pk code
            
            comboBoxComPortShutter.Items.AddRange(System.IO.Ports.SerialPort.GetPortNames());   // use System.IO because it's static
            comboboxcontrol_box.Items.AddRange(System.IO.Ports.SerialPort.GetPortNames());      // pk code
            comboboxcontrol_box.Items.Remove("COM1");                                           // com1 not used by MCU
            
            comboBoxComPortShutter.Items.Remove("COM1");

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

        private string portFinder(System.IO.Ports.SerialPort testPort, string mcuName, List<string> portlist)  //mcuName will be e.g "controlbox" or "shutter"
        {
            //*
            // * This routine uses a test port to cycle through the portnames (COM1, COM3 etc), checking each port 
             //*  by sending a string recognised by a particular MCU e.g. controlbox# or shutter#
             //*  if the mcu is on the port, it responds with controlbox# or shutter#
             //*
            setupThePort(testPort);            //set the parameters for testport - baud etc
            bool found = false;
            foreach (string portName in portlist)     // GetUnusedSerialPorts forms a list of COM ports which are available
            {
               // MessageBox.Show("the port being checked is " + portName);        //tis worked
                found = checkforMCU(testPort, portName, mcuName);     // this checks if the current portName responds to mcuName (controlbox# / shutter#)
                if (found)
                {
                  //  MessageBox.Show("the port is found " + portName);
                    testPort.Close();                    //disconnect the port
                    return portName;

                }


            }
            return null;                // if no ports respond to queries (e.g. perhaps mcus are not connected), the null return is picked by the try - catch exception
                                        // of control box connect or shutter connect
          //  throw new NullReferenceException();
        }

        private bool checkforMCU(System.IO.Ports.SerialPort testPort, string portName, string MCUDescription)     // Note Seril is short form for system.io.ports Serial
        {

          //  MessageBox.Show("Sending " + MCUDescription + " to port " + portName);
            testPort.PortName = portName;

          
            // send data to the MCU and see what comes back

            string response = "";
            
            //     new below

            bool success = false;
            int n = 0;

            // MessageBox.Show("Sending " + MCUDescription + " to port " + testPort.PortName);  // + "received " + response);
            while ((success == false) && (n < 3))   // try each port three times
            {
                try
                {
                    response = "";
                    testPort.Open();
                    Thread.Sleep(500);           // delay (in mS) - essential if the MCU is Arduino with a bootloader. The Arduino requires time after the port is connected before it can respond to serial requests.


                    testPort.Write(MCUDescription);                   // transmits controlbox# or shutter# depending upon where called
                    testPort.ReadTimeout = 1000;                      // milliseconds                
                    response = testPort.ReadTo("#");                  // only one port will respond to the query and those which don't respond will timeout
                                                                      //  MessageBox.Show("this is what the MCU sent back   " + response);


                    //append the # char to response so the comparison strings both have '#'
                    response += "#";
                    // MessageBox.Show("Reponse and mcu description follow  " + response +" " + MCUDescription );
                    if (response == MCUDescription)
                    {
                        success = true;                // the mcu exists on this port
                        testPort.Close();              // finished with the port
                        return true;
                        //   break;
                    }
                }
                catch
                {
                    // the catch is for the timeout exception which occurs when a port tested is unresponsive (all except the correct one will be ) no need to do anything
                }
                n++;

            } // end while



            testPort.Close();
                    return false;
            
        }     //end proc


        
        private void setupThePort(System.IO.Ports.SerialPort testPort)
        {
            //set all the port propereties
            testPort.BaudRate = 19200;
            testPort.Parity = Parity.None; ;
            testPort.DataBits = 8;
            testPort.StopBits = StopBits.One; 
                        
        }

       

        private void BTNidcomports_Click(object sender, EventArgs e)
        {

            bool portsAreAvailable = true;
            BTNidcomports.Text = "Waiting for ID";
            BTNidcomports.Refresh();



            System.IO.Ports.SerialPort tempPort = new System.IO.Ports.SerialPort();     // setup a variable as an ascom utils serial object

            

            var portlist = new List<string>(SerialPort.GetPortNames() );         // create a list of available comports on tempPort
            var busyPorts = new List<string>();
            portlist.Remove("COM1");                                             // COM1 is never used by MCUs


            //write some code to try to connect to the ports and if that fails, the port is busy, so remove it from the list..
            //todo remove below
            //foreach (string port in portlist)
           // {
           //     MessageBox.Show("Port names are " + port);
           // }


                foreach (string port in portlist)
            {
                try
                {
                   // MessageBox.Show("trying " + port);
                    tempPort.PortName = port;
                    tempPort.Open();
                    tempPort.Write("test");
                    tempPort.Close();
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


        //    foreach (string port in portlist)
        //    {
        //        MessageBox.Show("Port names are " + port);
        //    }

            //now send id messages to each port in the list to find which MCU is attached to which port.
            label1.Text= "Please wait while ID takes place";
            label1.TextAlign = ContentAlignment.MiddleCenter;
            
            label1.BackColor = Color.OrangeRed;
            label1.Refresh();
            try
            {
                LBLShutter.BackColor = Color.DarkGray;
                //LBLAzimuth.BackColor = Color.DarkGray;
                LBLcontrolBox.BackColor = Color.DarkGray;
                LBLShutter.Refresh();
                //LBLAzimuth.Refresh();
                LBLcontrolBox.Refresh();

                LBLShutter.Text = "Shutter id in process....";
                LBLShutter.Refresh();

                string portName;                                                 //used to hold the name of the com port returned by portfinder()

                // call portfinder to identify which port the shutter MCU is attached to

                portName = portFinder(tempPort, "shutter#", portlist);          // this routine returns the port name that replied e.g. COM7


                if (portName == null)    // this happens if there are missing MCUs e.g. USB cable not connected
                {

                    throw new ArgumentNullException();
                }

                settheComboboxitem( portName, comboBoxComPortShutter);

                LBLShutter.Text = checkForNull(portName, "shutter");

                // check if port is unavailable and if so set the back colour to orange
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

                


                // call portfinder to identify which port the controlbox MCU is attached to

                portName = portFinder(tempPort, "controlbox#", portlist);
              //  MessageBox.Show("Port name is " + portName);

                if (portName == null )    // this happens if there are missing MCUs e.g. USB cable not connected
                {
                    
                    throw new ArgumentNullException();
                }

                settheComboboxitem(portName, comboboxcontrol_box );
                
                LBLcontrolBox.Text = "Dome drive id in process....";
                LBLcontrolBox.Refresh();

                LBLcontrolBox.Text = checkForNull(portName, "Dome drive");

                // check if port is unavailable and if so set the back colour to ornage
                if (LBLcontrolBox.Text.Contains("Unavailable") )
                    {
                    LBLcontrolBox.BackColor = Color.Orange;
                    }
                else
                   {
                    LBLcontrolBox.BackColor = Color.YellowGreen;
                }
            }

            catch (Exception ex)
            {

                MessageBox.Show(" connection failed. Check the MCUs are on, connected, and in receive mode. " + ex.Message);
                portsAreAvailable = false;
            }


            label1.Text = " Port Identification complete";
            label1.BackColor = Color.YellowGreen;
            label1.TextAlign = ContentAlignment.MiddleCenter;

            //reset the command button text
            BTNidcomports.Text = "Identify Comports";

            if (portsAreAvailable)
            {
                cmdOK.Enabled = true;
            }
        }  // end id comports

        private void settheComboboxitem(string testItem, ComboBox cboTest)

        {
            string item_text = "";
            for (int i = 0; i < cboTest.Items.Count; i++)
            {
                item_text = cboTest.GetItemText(cboTest.Items[i]);
                if (item_text == testItem)
                { 
                    cboTest.SelectedItem = (object)cboTest.Items[i];
                    break;
                }
            }
            
        }

      
    }  // end public partial class

    public static class myGlobals     // this is a way of making a global variable set, accessible fom anywhere in the namespace. 
    {
        public static bool check1 = false;
        public static bool check2 = false;
        public static bool check3 = false;
        public static string pkstring = "eras";

    }
}

       //
       
    
