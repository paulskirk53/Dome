// This implements a console application that can be used to test an ASCOM driver
//

// This is used to define code in the template that is specific to one class implementation
// unused code can be deleted and this definition removed.

#define Dome
// remove this to bypass the code that uses the chooser to select the driver
//
#define UseChooser

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ASCOM
{
    class Program
    {
        static void Main(string[] args)
        {
           // Uncomment the code that's required
          #if UseChooser
                      // choose the device
                      string id = ASCOM.DriverAccess.Dome.Choose("");
                      if (string.IsNullOrEmpty(id))
                          return;
                      // create this device
                      ASCOM.DriverAccess.Dome device = new ASCOM.DriverAccess.Dome(id);
          #else
                      // this can be replaced by this code, it avoids the chooser and creates the driver class directly.
                      ASCOM.DriverAccess.Dome device = new ASCOM.DriverAccess.Dome("ASCOM.GowerCDome.Dome");
          #endif
                      // now run some tests, adding code to your driver so that the tests will pass.
                      // these first tests are common to all drivers.
                      Console.WriteLine("name " + device.Name);
                      Console.WriteLine("description " + device.Description);
                      Console.WriteLine("DriverInfo " + device.DriverInfo);
                      Console.WriteLine("driverVersion " + device.DriverVersion);
                      Console.WriteLine("ERAS");
                      // TODO add more code to test the driver.
          

            // trying to display the setup dialog....

            // device.SetupDialog();

            /*
            int difference = 0;
            int diffmod = 0;
            int divider = 360;
            double taz = 350.0;
            double caz = 20.0;
            String direction = "";
            int part1, part2, part3;

            //Console.Write("New Target Azimuth value ? a number > 360 exits the routine :");

            while (taz < 361.0)
            {

                
                Console.Write("New Target Azimuth value ? a number > 360 exits the routine :");                            // invite user input for azimuth tests

                String response = Console.ReadLine();

                if (double.TryParse(response, out taz))

                    Console.Write("New Current Azimuth value ? a number > 360 exits the routine :");                            // invite user input for azimuth tests

                 response = Console.ReadLine();

                if (double.TryParse(response, out caz))

                difference = (int)(caz - taz);
                                                               //difference = Math.Abs(difference);
                part1 = (int)(difference/360);
                if (difference<0)
                {
                    part1 = -1;
                }
                part2 = part1 * 360;
                part3 = difference - part2;

                Console.WriteLine("difference   =   " + difference);
                Console.WriteLine("part1        =   " + part1);
                Console.WriteLine("part2        =   " + part2);
                Console.WriteLine("part3        =   " + part3);
               
                for (int i =1; i<500; i++)
                {
                    difference = difference - 360;
                    if (difference < 360)
                    {
                        break;
                    }
                  
                }
                

                diffmod = part3;

                if (diffmod >= 180)
                {
                     direction = "CL";
                }
                else
                {
                    direction = "CC";
                }

                Console.WriteLine("diffmod   =   " + diffmod);
                Console.WriteLine("direction =   " + direction);

            }

            */
             device.Connected = true;
           double pkazimuth = 2.00;                                               // for tests

           while (pkazimuth < 360.0)                                           // coninuous loop for testing
           {


               Console.WriteLine("connection status  " + device.Connected);          // show connection status

               Console.Write("New Azimuth value ? a number > 360 exits the routine :");                            // invite user input for azimuth tests

               string response = Console.ReadLine();


               double result;

               if (double.TryParse(response, out result))

                   pkazimuth = result;

               Console.WriteLine("Requested Target is  " + pkazimuth);


               if (pkazimuth > 360.0)                                             // leave the while loop if pkazimuth >360
               {
                   Console.WriteLine("Requested Stop  ");
                      break;

               }



               // for tests



               device.SlewToAzimuth(pkazimuth);                                  // test the slew to azimuth 
               System.Threading.Thread.Sleep(500);

               while (device.Slewing)
               {

                   Console.WriteLine("Current Azimuth = ");                                  //  test the dome azimuth
                   Console.WriteLine(device.Azimuth);
                   System.Threading.Thread.Sleep(1000);                                   // serial needs time after each command


                   Console.WriteLine("Slewing Status  " + device.Slewing);           //  test the slewing status
                   System.Threading.Thread.Sleep(500);


                   //device.SlewToAzimuth(pkazimuth);                              // test the slew to azimuth reverse direction
                   // System.Threading.Thread.Sleep(1000);
               }

               Console.WriteLine("Azimuth = ");                                  //  test the dome azimuth
               Console.WriteLine(device.Azimuth);

           }  // endwhle pkazimuth

           

            Console.WriteLine("Press Enter to finish");
            Console.ReadLine();
        device.Connected = false;
        } //end static void main
    } // end class program
}
