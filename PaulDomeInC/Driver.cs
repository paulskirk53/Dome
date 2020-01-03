//tabs=4
// --------------------------------------------------------------------------------
// TODO fill in this information for your driver, then remove this line!
//
// ASCOM Dome driver for GowerCDome
//
// Description:	Part one - THIS IS THE WORKING PROJECT FILE. iT has been tested initially with POTH 
//				and a planetarium programme cartes du ciel. Also with SGP
//
//      Code for this revision is informd by 'dome driver program process - google sheets url
//      https://docs.google.com/spreadsheets/d/129XTTVrI_Kxw_0QjSZ3ILjBEWEoGZlBaUu5U1P22TgM/edit#gid=0
//
//
// Implements:	ASCOM Dome interface version: <2>
// Author:		(XXX) Paul Kirk <your@email.here>, base code by Tom How, Curdridge Observatory
//
// Edit Log:
//
// Date			Who	Vers	Description
// -----------	---	-----	-------------------------------------------------------
// 4-3-2017	XXX	6.0.0	Initial edit, created from ASCOM driver template
// --------------------------------------------------------------------------------

// 14-8-17 tested with telescope sim for.net, POTH and C du Ciel - see notes in wordpad file  in folder 'domestuff'
// stepper moves as expected when tracking and doing gotos in this simulator environment


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
    [Guid("0edcf62a-9d7a-4a63-b8c2-f2f151c93ba7")]
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
        internal static string StepperPortProfileName        = "Stepper Port";
        internal static string CompassEncoderPortProfileName = "Encoder Port";
        internal static string SetParkProfilename            = "Set Park";
        internal static string comPortDefault                = "COM4";
        internal static string traceStateProfileName         = "Trace Level";
        internal static string traceStateDefault             = "false";
        internal static string comPort;                  // Variables to hold the currrent device configuration
        internal static string CompassComPort;          // PK ADDED THESE ...
        internal static string StepperComPort;
        internal static string Parkplace;
        internal static double ParkAzimuth;
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

        private ASCOM.Utilities.Serial pkstepper;
        private ASCOM.Utilities.Serial pkcompass;

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

            using (SetupDialogForm F = new SetupDialogForm())
            {
                var result = F.ShowDialog();
                if (result == System.Windows.Forms.DialogResult.OK)
                {
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
 

                if (value)    // Connect requested
                {
                    connectedState = Connect();    
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
            tl.LogMessage("Connected Set", "Connecting to port " + StepperComPort );
            //set the stepper motor connection
            try
            {
                pkstepper = OpenPort(StepperComPort);
                // set the compass (now encoder) connection
                pkcompass = OpenPort(CompassComPort);
                return true;    //pk added cos of build error not all code paths return a value
            }
            catch (Exception ex)
            {
                tl.LogMessage("Connected Set", "Unable to connect to COM ports " + ex.ToString());

                if (pkstepper != null)
                {
                    DisconnectPort(pkstepper);
                }
                
                if (pkcompass != null)
                {
                    DisconnectPort(pkcompass);
                }
                return false;   //pk added cos of build error not all code paths return a value
               
            }
            
           
        }

        private ASCOM.Utilities.Serial OpenPort(string portName)
        {
            ASCOM.Utilities.Serial port = new ASCOM.Utilities.Serial();
            port.PortName = portName;
            port.DTREnable = false;
            port.RTSEnable = false;
            port.ReceiveTimeout = 10000;
            
            port.Speed = SerialSpeed.ps115200;
            port.Connected = true;
            port.ClearBuffers();

            return port;
        }

        private void Disconnect()
        {
            // disconnect the hardware
            DisconnectPort(pkstepper);
            DisconnectPort(pkcompass);
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
            // This is a mandatory parameter but we have no action to take in this simple driver
            tl.LogMessage("AbortSlew", "Completed");
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
                //pk todo
                tl.LogMessage("AtHome Get", "Not implemented");
                throw new ASCOM.PropertyNotImplementedException("AtHome", false);
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
                        pkcompass.ClearBuffers();
                        pkcompass.Transmit("AZ#");            //this is absolutely necessary, even with catch block. If the two Tx statements are 
                        //not included the code hangs at the line below 'string response = pkcompass.ReceiveTerminated("#"); may need to ask ASCOM guys
                    }
                    catch (Exception ex)
                    {
                        tl.LogMessage("Azimuth property failure to Tx AZ#", ex.ToString());
                        pkcompass.Transmit("AZ#");
                    }


                    string response = pkcompass.ReceiveTerminated("#");
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
                tl.LogMessage("CanFindHome Get", false.ToString());
                return false;                                        
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
                tl.LogMessage("CanSetAzimuth Get", false.ToString());
                // pk changed to return true
                return true;
            }
        }

        public bool CanSetPark
        {
            get
            {
                tl.LogMessage("CanSetPark Get", false.ToString());

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
                tl.LogMessage("CanSyncAzimuth Get", false.ToString());
                return false;
            }
        }

        public void CloseShutter()                                           // 13-4-17
        {

            pkcompass.ClearBuffers();
            pkcompass.Transmit("CS#");
            

            tl.LogMessage("CloseShutter", "Shutter has been closed");
           // domeShutterState = false;
        }

        public void FindHome()
        {
            tl.LogMessage("FindHome", "Not implemented");
            throw new ASCOM.MethodNotImplementedException("FindHome");
        }

        public void OpenShutter()                                          // 13-4-17
        {
            pkcompass.ClearBuffers();
            pkcompass.Transmit("OS#");
            

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
                pkcompass.ClearBuffers();
                pkcompass.Transmit("SS#");                            // send the command to trigger the status response from the arduino

                string state = pkcompass.ReceiveTerminated("#");
                state = state.Replace("#", "");

                if (state == "OPEN")
                {
                    return ShutterState.shutterOpen;
                }
                else
                {
                    return ShutterState.shutterClosed;
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


            // this method works out which direction to slew by acquiring the current dome azimuth
            // and comparing it to the target azimuth, then commands the slew and the direction

            // get current Az
            int DiffMod, difference, part1, part2, part3;   //these are all local and used to claculate modulus in a particular way - not like the c# % function

            double CurrentAzimuth =  Azimuth ;  //this gets the current azimuth from the dome.Azimuth property  (see your code in Azimuth)


                difference = (int)(CurrentAzimuth-TargetAzimuth );
                part1 = (int)(difference / 360);
                if (difference < 0)
                {
                    part1 = -1;
                }
                part2 = part1 * 360;
                part3 = difference - part2;
                DiffMod = part3;

                

                // code below optimises movement to take the shortest distance

         
                if (DiffMod >= 180)
                {
                try
                {
                    pkstepper.ClearBuffers();
                    pkstepper.Transmit("CL" + CurrentAzimuth.ToString("0.##") + "#");
                    pkstepper.Transmit("SA" + TargetAzimuth.ToString("0.##") + "#");
                }
                catch (Exception ex)
                {

                    pkstepper.ClearBuffers();
                    pkstepper.Transmit("CL" + CurrentAzimuth.ToString("0.##") + "#");
                    pkstepper.Transmit("SA" + TargetAzimuth.ToString("0.##") + "#");
                    // log
                    tl.LogMessage("Slew to azimuth - attempt to send CL and SA for angle > 180", ex.ToString());
                }
                 
                }

                else   // the less than 180 scenario

                {

                try
                {
                    pkstepper.ClearBuffers();
                    pkstepper.Transmit("CC" + CurrentAzimuth.ToString("0.##") + "#");
                    pkstepper.Transmit("SA" + TargetAzimuth.ToString("0.##") + "#");
                }
                catch (Exception ex)
                {

                    pkstepper.ClearBuffers();
                    pkstepper.Transmit("CC" + CurrentAzimuth.ToString("0.##") + "#");
                    pkstepper.Transmit("SA" + TargetAzimuth.ToString("0.##") + "#");
                    //log
                    tl.LogMessage("Slew to azimuth - attempt to send CL and SA for angle < 180", ex.ToString());
                }

                }
                           
        }

        public bool Slewing
        {
            get
            {

                double CurrentAzimuth = Azimuth;    // new on 20-12-19 see comment immediately below

                // the comment out below was because of the realisation that there is a propety built into the driver which provides the Azimuth
                //so no need to Tx and Rx to get the current Azimuth

                try
                {
                    pkstepper.ClearBuffers();                                         // this cured the receive problem from Arduino             
                    pkstepper.Transmit("SL" + CurrentAzimuth.ToString("0.##") + "#"); // changed from just sending SL, to this new pattern
                                                                                      // which accommodates the SL process in the stepper arduino
                }
                catch (Exception ex)
                {
                    pkstepper.ClearBuffers();                                         
                    pkstepper.Transmit("SL" + CurrentAzimuth.ToString("0.##") + "#");
                    // log failure
                    tl.LogMessage("Slewing Get failed to Tx SL#", ex.ToString());
                }

                string SL_response = pkstepper.ReceiveTerminated("#");            // read what's sent back
                SL_response = SL_response.Replace("#", "");                       // remove the # mark
                if (SL_response == "Moving")                                      // set this condition properly
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
            tl.LogMessage("SyncToAzimuth", "Not implemented");
            throw new ASCOM.MethodNotImplementedException("SyncToAzimuth");
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
            using (Profile driverProfile = new Profile())
            {
                driverProfile.DeviceType = "Dome";
                tl.Enabled = Convert.ToBoolean(driverProfile.GetValue(driverID, traceStateProfileName, string.Empty, traceStateDefault));

                CompassComPort = driverProfile.GetValue(driverID, CompassEncoderPortProfileName, string.Empty, CompassComPort);
                StepperComPort = driverProfile.GetValue(driverID, StepperPortProfileName, string.Empty, StepperComPort);
                temp =  driverProfile.GetValue(driverID, SetParkProfilename, Parkplace);
                double.TryParse(temp, out ParkAzimuth);   // this line sets the initial value of ParkAzimuth
               
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
                
                driverProfile.WriteValue(driverID, StepperPortProfileName, StepperComPort.ToString());
                driverProfile.WriteValue(driverID, CompassEncoderPortProfileName, CompassComPort.ToString());
                driverProfile.WriteValue(driverID, SetParkProfilename, Parkplace.ToString());
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
