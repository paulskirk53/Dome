//tabs=4
// --------------------------------------------------------------------------------
// TODO fill in this information for your driver, then remove this line!
//
// ASCOM Dome driver for GowerCDome
//
// Description:	Part one - THIS IS THE changed ascom dome driver which is modded to accmmodate the new one MCU control boxShas been tested 
//  2022 - we have moved on a lot and my understanding of what's going on is much greater!

//      Code for this revision was initially informd by 'dome driver program process - google sheets url and this is where Tom How helped with the architecture stuff
//      https://docs.google.com/spreadsheets/d/129XTTVrI_Kxw_0QjSZ3ILjBEWEoGZlBaUu5U1P22TgM/edit#gid=0
//
//
// Implements:	ASCOM Dome interface version: <2>
// Author:	 Paul Kirk <your@email.here>, with initial help from Tom How, Curdridge Observatory
//
// Edit Log:
//
// Date			Who	Vers	Description
// -----------	---	-----	-------------------------------------------------------
// 24-6-2023	XXX	6.0.0	new control box, created from ASCOM driver template - many edits since, all in github
// --------------------------------------------------------------------------------

// 14-8-17 tested with telescope sim for.net, POTH and C du Ciel - see notes in wordpad file  in folder 'domestuff'
// 10-3-2022 - five years on....
// 24-6-2023 - six years on


// This is used to define code in the template that is specific to one class implementation
// unused code canbe deleted and this definition removed.
#define Dome

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Runtime.InteropServices;



using ASCOM;
using ASCOM.Astrometry;
using ASCOM.Astrometry.AstroUtils;
using ASCOM.Utilities;
using ASCOM.DeviceInterface;
using System.Globalization;
using System.Collections;
using System.Net.Mail;

namespace ASCOM.GowerCDome
{

    //
    // Your driver's DeviceID is ASCOM.GowerCDome.Dome
    //
    // The Guid attribute sets the CLSID for ASCOM.GowerCDome.Dome
    // The ClassInterface/None addribute prevents an empty interface called
    // _GowerCDome from being created and used as the [default] interface
    //
    // TODO Replace the not implemented exceptions with code to implement the function or
    // throw the appropriate ASCOM exception.
    //

    /// <summary>
    /// ASCOM Dome Driver for GowerCDome.
    /// </summary>
    [Guid("D3A3ADF4-75D2-47C6-958C-FF84332097DD")]    // NUC requires a different guid from the dev machine
    //[Guid("0edcf62a-9d7a-4a63-b8c2-f2f151c93ba7")]  //dev machine guid
    [ClassInterface(ClassInterfaceType.None)]
    public class Dome : IDomeV2
    {
        /// <summary>
        /// ASCOM DeviceID (COM ProgID) for this driver.
        /// The DeviceID is used by ASCOM applications to load the driver at runtime.
        /// </summary>
        internal static string driverID = "ASCOM.GowerCDome.Dome";
        // TODO Change the descriptive string for your driver then remove this line
        /// <summary>
        /// Driver description that displays in the ASCOM Chooser.
        /// </summary>
        private static string driverDescription              = "ASCOM Gower Observatory 2017.";
        internal static string comPortProfileName            = "COM Port"; // Constants used for Profile persistence
        internal static string control_BoxPortProfileName    = "Control Box Port";
       
        internal static string ShutterPortProfileName        = "Shutter Port";
        internal static string SetParkProfilename            = "Set Park";
        internal static string SetHomeProfilename            = "Set Home";
        internal static string comPortDefault                = "COM4";
        internal static string traceStateProfileName         = "Trace Level";
        internal static string traceStateDefault             = "false";
      //  internal static string comPort;                  // Variables to hold the currrent device configuration
      
        internal static string control_BoxComPort;
        internal static string ShutterComPort;
        internal static string Parkplace;
        internal static string Homeplace;
        internal static double ParkAzimuth;
        internal static double HomeAzimuth;
        /// <summary>
        /// Private variable to hold the connected state
        /// </summary>
        //was this private bool connectedState;
        public bool connectedState;

        /// <summary>
        /// Private variable to hold an ASCOM Utilities object
        /// </summary>
        private Util utilities;

        /// <summary>
        /// Private variable to hold an ASCOM AstroUtilities object to provide the Range method
        /// </summary>
        private AstroUtils astroUtilities;

        /// <summary>
        /// Variable to hold the trace logger object (creates a diagnostic log file with information that you specify)
        /// </summary>
        internal static TraceLogger tl;

        private ASCOM.Utilities.Serial control_Box;
   
        private ASCOM.Utilities.Serial pkShutter;

        /// <summary>
        /// Initializes a new instance of the <see cref="GowerCDome"/> class.
        /// Must be public for COM registration.
        /// </summary>
        public Dome()
        {
           
            tl = new TraceLogger("", "GowerCDome");
            ReadProfile(); // Read device configuration from the ASCOM Profile store

            tl.LogMessage("Dome", "Starting initialisation");

            connectedState = false; // Initialise connected to false
            utilities = new Util(); //Initialise util object
            astroUtilities = new AstroUtils(); // Initialise astro utilities object
            //TODO: Implement your additional construction here

            tl.LogMessage("Dome", "Completed initialisation");

        }


        //
        // PUBLIC COM INTERFACE IDomeV2 IMPLEMENTATION
        //

        #region Common properties and methods.

        /// <summary>
        /// Displays the Setup Dialog form.
        /// If the user clicks the OK button to dismiss the form, then
        /// the new settings are saved, otherwise the old values are reloaded.
        /// THIS IS THE ONLY PLACE WHERE SHOWING USER INTERFACE IS ALLOWED!
        /// </summary>
        public void SetupDialog()
        {
            // consider only showing the setup dialog if not connected
            // or call a different dialog if connected
            if (IsConnected)
          
            System.Windows.Forms.MessageBox.Show("Already connected, just press OK");

            using (SetupDialogForm F = new SetupDialogForm())  // this line sets up an instance of the dialog form
            {
                var result = F.ShowDialog();
                if (result == System.Windows.Forms.DialogResult.OK)
                {
                // todo if the comports are null, disable the OK button, so cancel is the only option
                    WriteProfile(); // Persist device configuration values to the ASCOM Profile store
                }
            }
        }

        public ArrayList SupportedActions
        {
            get
            {
                tl.LogMessage("SupportedActions Get", "Returning empty arraylist");
                return new ArrayList();
            }
        }

        public string Action(string actionName, string actionParameters)
        {
            LogMessage("", "Action {0}, parameters {1} not implemented", actionName, actionParameters);
            throw new ASCOM.ActionNotImplementedException("Action " + actionName + " is not implemented by this driver");
        }

        public void CommandBlind(string command, bool raw)
        {
            CheckConnected("CommandBlind");
            // Call CommandString and return as soon as it finishes
            // this.CommandString(command, raw);             //pk took out
            // or
            throw new ASCOM.MethodNotImplementedException("CommandBlind");
            // DO NOT have both these sections!  One or the other
        }

        public bool CommandBool(string command, bool raw)
        {
            CheckConnected("CommandBool");
            string ret = CommandString(command, raw);
            // TODO decode the return string and return true or false
            // or
            throw new ASCOM.MethodNotImplementedException("CommandBool");
            // DO NOT have both these sections!  One or the other
        }

        public string CommandString(string command, bool raw)
        {
            CheckConnected("CommandString");
            // it's a good idea to put all the low level communication with the device here,
            // then all communication calls this function
            // you need something to ensure that only one command is in progress at a time

            throw new ASCOM.MethodNotImplementedException("CommandString");
        }

        public void Dispose()
        {
            // Clean up the tracelogger and util objects
            tl.Enabled = false;
            tl.Dispose();
            tl = null;
            utilities.Dispose();
            utilities = null;
            astroUtilities.Dispose();
            astroUtilities = null;
        }

        public bool Connected
        {
            get { return connectedState; }
            set
            {
 

                if (value)    // Connect to hardware requested
                {
                    connectedState = Connect();

                    //new Nov 2025
                    // Retrieve park azimuth from profile

                    var profile = new Profile();
                    profile.DeviceType = "Dome"; // or "Telescope", "Camera", etc. depending on your driver type
                    string parkAzimuthStr = profile.GetValue(driverID, SetParkProfilename, string.Empty, Parkplace);  
                    // string parkAzimuthStr = profile.GetValue("ASCOM.GowerCDome.Dome", "Parkplace", string.Empty);
                    

                    // Send sync command to microcontroller
                    control_Box.Transmit("STA" + parkAzimuthStr + "#");    // this should be the current value initialised in the setup dialog
                  

                   //end new 2025

                }
                else    // Disconnect requested
                {
                    Disconnect();
                    connectedState = false;  
                }
            }
        }

        // The following helper methods make the code more readable and eliminate redundant statements. 
        // Notice that I also added a try/catch around the port connection to fail cleanly and log the failure. 
        // I also added code to dispose and null each of the ports when they are closed.

        private bool Connect()
        {
            
            tl.LogMessage("Connected Set", "Connecting to port " + control_BoxComPort );
            //set the control_Box motor connection
            try
            {


                // set the control box connection
                control_Box = OpenPort(control_BoxComPort);
                
                pkShutter = OpenPort(ShutterComPort);

                return true;    //pk added cos of build error not all code paths return a value
            }
            catch (Exception ex)
            {
                tl.LogMessage("Connected Set", "Unable to connect to COM ports " + ex.ToString());

                if (control_Box != null)
                  {
                    DisconnectPort(control_Box);
                  }
                
                if (pkShutter != null)
                  {
                    DisconnectPort(pkShutter);
                  }
                return false;   //pk added cos of build error not all code paths return a value
               
            }
            
           
        }

        private void initialise_stepper()   //todo change to control_Box
        {
            double AzimuthInitialise = 261.00;

            try
            {
                control_Box.ClearBuffers();

                control_Box.Transmit("SA" + AzimuthInitialise.ToString("0.##") + "#");
            }
            catch (Exception ex)
            {

                control_Box.ClearBuffers();

                control_Box.Transmit("SA" + AzimuthInitialise.ToString("0.##") + "#");
                // log
                tl.LogMessage("Attempt to initialise azimuth for the control box", ex.ToString());
            }

        }

        private ASCOM.Utilities.Serial OpenPort(string portName)
        {
            ASCOM.Utilities.Serial port = new ASCOM.Utilities.Serial();
            port.PortName = portName;
            port.DTREnable = false;
            port.RTSEnable = false;
            port.ReceiveTimeout = 10000;
            
            port.Speed = SerialSpeed.ps19200;
            port.Connected = true;
            port.ClearBuffers();

            return port;
        }

        private void Disconnect()
        {
            // disconnect the hardware
            DisconnectPort(control_Box);
            
            DisconnectPort(pkShutter);
            //           tl.LogMessage("Connected Set", "Disconnecting from port " + comPort);
        }

        private void DisconnectPort(ASCOM.Utilities.Serial port)
        {
            port.Connected = false;
            port.Dispose();
            port = null;
        }



        // end helper methods


      

        public string Description
        {
            // TODO customise this device description
            get
            {
                driverDescription = "Gower Dome 2017";
                tl.LogMessage("Description Get", driverDescription);
                return driverDescription;
            }
        }

        public string DriverInfo
        {
            get
            {
                Version version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
                // TODO customise this driver description
                string driverInfo = "Information about the driver - Version: " + String.Format(CultureInfo.InvariantCulture, "{0}.{1}", version.Major, version.Minor);
                tl.LogMessage("DriverInfo Get", driverInfo);
                return driverInfo;
            }
        }

        public string DriverVersion
        {
            get
            {
                Version version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
                string driverVersion = String.Format(CultureInfo.InvariantCulture, "{0}.{1}", version.Major, version.Minor);
                tl.LogMessage("DriverVersion Get", driverVersion);
                return driverVersion;
            }
        }

        public short InterfaceVersion
        {
            // set by the driver wizard
            get
            {
                LogMessage("InterfaceVersion Get", "2");
                return Convert.ToInt16("2");
            }
        }

        public string Name
        {
            get
            {
                string name = "Gower Observatory";
                tl.LogMessage("Name Get", name);
                return name;
            }
        }

        #endregion

        #region IDome Implementation

       // private bool domeShutterState = false; // Variable to hold the open/closed status of the shutter, true = Open
       // public double ParkAzimuth;    //var for holding Setpark position PK mimic of above to try to help with park method.
        
        public void AbortSlew()
        {
            //PK - 30-9-23 - not sure what the ASCOM stub comment below means - nothing by the look of it 
            // This is a mandatory parameter but we have no action to take in this simple driver
            tl.LogMessage("AbortSlew", "Completed");
            // send ES to the dome and to the shutter - when the shutter command processor receives this
            // it causes a reset of the BT radio, the command processor mcu and the shutter mcu
            control_Box.Transmit("ES#");    // halt dome slewing
            pkShutter.Transmit("ES#");      // halt shutter and close it safely if open or part open
        }

        public double Altitude
        {
            get
            {
                tl.LogMessage("Altitude Get", "Not implemented");
                throw new ASCOM.PropertyNotImplementedException("Altitude", false);
            }
        }

        public bool AtHome
        {
            get
            {
                tl.LogMessage("AtHome Get", "Is Now implemented Mar '22");
                double CurrentAzimuth = Azimuth;
                //pk todo remove hardcoding of Home position below - done

                if (Math.Abs(CurrentAzimuth - HomeAzimuth) <= 3.0)   // care - assumes a fixed sensor position of 270.0 degrees
                {
                    return true;
                }
                else
                {
                    return false;
                }
                
              //  throw new ASCOM.PropertyNotImplementedException("AtHome", false);
            }
        }

        public bool AtPark
        {
            get
            {
                //mycode
                // check if the current dome azimuth is = to ParkAzimuth
                double CurrentAzimuth = Azimuth;     // note Azimuth is a dome property - see code for it.
               
 
                if (Math.Abs(CurrentAzimuth  - ParkAzimuth) <= 5.0)                      
                    return true;
                else
                    return false;

               // tl.LogMessage("AtPark Get", "Not implemented");
                // pk commented out throw new ASCOM.PropertyNotImplementedException("AtPark", false);
            }
        }

        public double Azimuth 
        {
            get
            {
   
                int trycount = 0;
                bool success = false;
                double az = 0;

                while (success==false)
                {

                    try
                    {
                        control_Box.ClearBuffers();
                 
                        control_Box.Transmit("AZ#");

                    }
                    catch (Exception ex)
                    {
                        tl.LogMessage("Azimuth property failure to Tx AZ#", ex.ToString());
                        control_Box.Transmit("AZ#");
                    }


                    string response = control_Box.ReceiveTerminated("#");
                    response = response.Replace("#", "");
                    
                    success = double.TryParse(response, out az);

                    trycount++;

                    if (trycount>4)
                    {
                        break;
                    }

                }
                    return az;
              }
        }

        public bool CanFindHome
        {
            get
            {
                tl.LogMessage("CanFindHome Get", true.ToString());
                return true;                                        
            }
        }

        public bool CanPark
        {
            get
            {
                tl.LogMessage("CanPark Get", false.ToString());

                  return true;
            }
        }

        public bool CanSetAltitude
        {
            get
            {
                tl.LogMessage("CanSetAltitude Get", false.ToString());
                return false;
            }
        }

        public bool CanSetAzimuth
        {
            get
            {
                tl.LogMessage("CanSetAzimuth Get", true.ToString());
                // pk changed to return true
                return true;
            }
        }

        public bool CanSetPark
        {
            get
            {
                tl.LogMessage("CanSetPark Get", true.ToString());

                return true;
            }
        }

        public bool CanSetShutter
        {
            get
            {
                tl.LogMessage("CanSetShutter Get", true.ToString());
                return true;
            }
        }

        public bool CanSlave
        {
            get
            {
                tl.LogMessage("CanSlave Get", false.ToString());
                return false;                                                   //pk changed to true
            }
        }

        public bool CanSyncAzimuth
        {
            get
            {
                tl.LogMessage("CanSyncAzimuth Get", true.ToString());
                return true;
            }
        }

        public void CloseShutter()                                           // 13-4-17
        {

            pkShutter.ClearBuffers();
            pkShutter.Transmit("CS#");
            

            tl.LogMessage("CloseShutter", "Shutter has been closed");
           // domeShutterState = false;
        }

        public void FindHome()
        {
            // we need to implement this for the remote Observatory because if a power or MCU reset happens, we lose position
            //findhome 'scans' for the fixed azimuth by slewing the dome and checking if the findhome sensor is activated
            //if so, the MCU azimuth is set to that correct azimuth value associated with the sensor.
            tl.LogMessage("FindHome", "Now implemented, March 2022");


            try
            {
                control_Box.ClearBuffers();

                control_Box.Transmit("FH#");
            }
            catch (Exception ex)
            {

                control_Box.ClearBuffers();

                control_Box.Transmit("FH#");
                // log
                tl.LogMessage("Find home ", ex.ToString());
            }


            //            throw new ASCOM.MethodNotImplementedException("FindHome");
        }

        public void OpenShutter()                                          // 13-4-17
        {
            pkShutter.ClearBuffers();
            pkShutter.Transmit("OS#");
            

            tl.LogMessage("OpenShutter", "Shutter has been opened");
          //  domeShutterState = true;
        }

        public void Park()
        {
            if (!AtPark)                     //if we're already there, do nothing
            {
                SlewToAzimuth(ParkAzimuth);
            }
         }

        public void SetPark()
        {
            
            //get the current azimuth

            ParkAzimuth = Azimuth;               
 
            tl.LogMessage("SetPark", " implemented");
           // throw new ASCOM.MethodNotImplementedException("SetPark");
        }

        public ShutterState ShutterStatus                                   //13-4-17
        {
            get
            {
                //thoughts 
                /*
                 
   - Will need to ensure the arduinos do not set the status line to open or closed before the operation is complete
   - Also check by setting up a new ASCOM c# dome template that domeshutterstate is used - i think it's somehing i have incorporated unnecessarily.             
   This section is get which should return ShutterStatus as ShutterState.shutterOpen or  ShutterState.shutterClosed by referencing the command processor
   status line. A client will presumably keep calling get to check. ASCOM client sample code in device access:
   */

                tl.LogMessage("ShutterStatus Get", false.ToString());
                pkShutter.ClearBuffers();
                pkShutter.Transmit("SS#");                            // send the command to trigger the status response from the arduino

                string state = pkShutter.ReceiveTerminated("#");
                state = state.Replace("#", "");

                switch(state)
                {

                    case "open":

                        return ShutterState.shutterOpen;

                    case "opening":
                        return ShutterState.shutterOpening;

                    case "closed":
                        return ShutterState.shutterClosed;

                    case "closing":
                        return ShutterState.shutterClosing;
                    default:
                        return ShutterState.shutterClosed;     // runs if there's no case match
                }


            }


        }

        public bool Slaved
        {
            get
            {
                tl.LogMessage("Slaved Get", false.ToString());
                return false;
            }
            set
            {
                tl.LogMessage("Slaved Set", "not implemented");
                throw new ASCOM.PropertyNotImplementedException("Slaved", true);
                                                                               
            }
        }

        public void SlewToAltitude(double Altitude)
        {
            tl.LogMessage("SlewToAltitude", "Not implemented");
            throw new ASCOM.MethodNotImplementedException("SlewToAltitude");
        }

        public void SlewToAzimuth(double TargetAzimuth)
        {
 
            tl.LogMessage("SA", "Started to implement");
            // throw new ASCOM.MethodNotImplementedException("SlewToAzimuth");


 
                try
                {
                  control_Box.ClearBuffers();

                  control_Box.Transmit("SA" + TargetAzimuth.ToString("0.##") + "#");
                }
                catch (Exception ex)
                {

                  control_Box.ClearBuffers();

                  control_Box.Transmit("SA" + TargetAzimuth.ToString("0.##") + "#");
                    // log
                    tl.LogMessage("Slew to azimuth - attempt to send CL and SA for angle > 180", ex.ToString());
                }
                 
                           
        }

        public bool Slewing
        {
            get
            {         
                try
                {
                    control_Box.ClearBuffers();                                         // this cured the receive problem from Arduino             

                    control_Box.Transmit("SL#");                 //  accommodates the SL process in the control box mcu
                }
                catch (Exception ex)
                {
                    control_Box.ClearBuffers();

                    control_Box.Transmit("SL#");
                    // log failure
                    tl.LogMessage("Slewing Get failed to Tx SL#", ex.ToString());
                }

                string SL_response = control_Box.ReceiveTerminated("#");            // read what's sent back
                SL_response = SL_response.Replace("#", "");                       // remove the # mark
                if (SL_response == "Moving")                                      // set this condition properly.
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public void SyncToAzimuth(double Azimuth)
        {
            tl.LogMessage("SyncToAzimuth", "Now implemented");
            //throw new ASCOM.MethodNotImplementedException("SyncToAzimuth");
            String AzimuthString = Azimuth.ToString("0.##");
            control_Box.Transmit("STA" + AzimuthString +"#");
        }

        #endregion

        #region Private properties and methods
        // here are some useful properties and methods that can be used as required
        // to help with driver development

        #region ASCOM Registration

        // Register or unregister driver for ASCOM. This is harmless if already
        // registered or unregistered. 
        //
        /// <summary>
        /// Register or unregister the driver with the ASCOM Platform.
        /// This is harmless if the driver is already registered/unregistered.
        /// </summary>
        /// <param name="bRegister">If <c>true</c>, registers the driver, otherwise unregisters it.</param>
        private static void RegUnregASCOM(bool bRegister)
        {
            using (var P = new ASCOM.Utilities.Profile())
            {
                P.DeviceType = "Dome";
                if (bRegister)
                {
                    P.Register(driverID, driverDescription);
                }
                else
                {
                    P.Unregister(driverID);
                }
            }
        }

        /// <summary>
        /// This function registers the driver with the ASCOM Chooser and
        /// is called automatically whenever this class is registered for COM Interop.
        /// </summary>
        /// <param name="t">Type of the class being registered, not used.</param>
        /// <remarks>
        /// This method typically runs in two distinct situations:
        /// <list type="numbered">
        /// <item>
        /// In Visual Studio, when the project is successfully built.
        /// For this to work correctly, the option <c>Register for COM Interop</c>
        /// must be enabled in the project settings.
        /// </item>
        /// <item>During setup, when the installer registers the assembly for COM Interop.</item>
        /// </list>
        /// This technique should mean that it is never necessary to manually register a driver with ASCOM.
        /// </remarks>
        [ComRegisterFunction]
        public static void RegisterASCOM(Type t)
        {
            RegUnregASCOM(true);
        }

        /// <summary>
        /// This function unregisters the driver from the ASCOM Chooser and
        /// is called automatically whenever this class is unregistered from COM Interop.
        /// </summary>
        /// <param name="t">Type of the class being registered, not used.</param>
        /// <remarks>
        /// This method typically runs in two distinct situations:
        /// <list type="numbered">
        /// <item>
        /// In Visual Studio, when the project is cleaned or prior to rebuilding.
        /// For this to work correctly, the option <c>Register for COM Interop</c>
        /// must be enabled in the project settings.
        /// </item>
        /// <item>During uninstall, when the installer unregisters the assembly from COM Interop.</item>
        /// </list>
        /// This technique should mean that it is never necessary to manually unregister a driver from ASCOM.
        /// </remarks>
        [ComUnregisterFunction]
        public static void UnregisterASCOM(Type t)
        {
            RegUnregASCOM(false);
        }

        #endregion

        /// <summary>
        /// Returns true if there is a valid connection to the driver hardware
        /// </summary>
        private bool IsConnected
        {
            get
            {
                return connectedState;
            }
        }

        /// <summary>
        /// Use this function to throw an exception if we aren't connected to the hardware
        /// </summary>
        /// <param name="message"></param>
        private void CheckConnected(string message)
        {
            if (!IsConnected)
            {
                throw new ASCOM.NotConnectedException(message);
            }
        }

        /// <summary>
        /// Read the device configuration from the ASCOM Profile store
        /// </summary>
        internal void ReadProfile()
        {
             string temp;
            string temp2;
            //todo add code below for temp2 as per temp, to read the home position - done
            using (Profile driverProfile = new Profile())
            {
                driverProfile.DeviceType = "Dome";
                tl.Enabled = Convert.ToBoolean(driverProfile.GetValue(driverID, traceStateProfileName, string.Empty, traceStateDefault));

                control_BoxComPort = driverProfile.GetValue(driverID,  control_BoxPortProfileName, string.Empty, control_BoxComPort);
                ShutterComPort = driverProfile.GetValue(driverID, ShutterPortProfileName, string.Empty, ShutterComPort);
               
                temp = driverProfile.GetValue(driverID, SetParkProfilename, string.Empty, Parkplace);   //pk changed to add in string.Empty, as with the other lines here
                
                double.TryParse(temp, out ParkAzimuth);   // this line sets the initial value of ParkAzimuth


                temp2 = driverProfile.GetValue(driverID, SetHomeProfilename, string.Empty, Homeplace );   //pk changed to add in string.Empty, as with the other lines here
                double.TryParse(temp2, out HomeAzimuth);
            //   Console.WriteLine($"Parkplace value: {temp}");   // pk -  atest for profile empty error
            //   Console.WriteLine($"Parkplace numeric value: {ParkAzimuth}");   // pk -  atest for profile empty error
            }
        }

        /// <summary>
        /// Write the device configuration to the  ASCOM  Profile store
        /// </summary>
        internal void WriteProfile()
        {
            using (Profile driverProfile = new Profile())
            {
                driverProfile.DeviceType = "Dome";
                driverProfile.WriteValue(driverID, traceStateProfileName, tl.Enabled.ToString());
                
                driverProfile.WriteValue(driverID, control_BoxPortProfileName, control_BoxComPort.ToString());
                driverProfile.WriteValue(driverID, ShutterPortProfileName, ShutterComPort.ToString());
                driverProfile.WriteValue(driverID, SetParkProfilename, Parkplace.ToString());
                driverProfile.WriteValue(driverID, SetHomeProfilename, Homeplace.ToString());
            }
        }

        /// <summary>
        /// Log helper function that takes formatted strings and arguments
        /// </summary>
        /// <param name="identifier"></param>
        /// <param name="message"></param>
        /// <param name="args"></param>
        internal static void LogMessage(string identifier, string message, params object[] args)
        {
            var msg = string.Format(message, args);
            tl.LogMessage(identifier, msg);
        }
        #endregion
    }
}
