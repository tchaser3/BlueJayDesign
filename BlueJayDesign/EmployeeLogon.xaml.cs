/* Title:           Employee Logon
 * Date:            1-25-19
 * Author:          Terry Holmes
 * 
 * Description:     This is used to log on employees */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using NewEmployeeDLL;
using NewEventLogDLL;
using DataValidationDLL;

namespace BlueJayDesign
{
    /// <summary>
    /// Interaction logic for EmployeeLogon.xaml
    /// </summary>
    public partial class EmployeeLogon : Window
    {
        //setting up classes
        WPFMessagesClass TheMessagesClass = new WPFMessagesClass();
        EmployeeClass TheEmployeeClass = new EmployeeClass();
        EventLogClass TheEventLogClass = new EventLogClass();
        DataValidationClass TheDataValidationClass = new DataValidationClass();

        int gintNoOfMisses;

        public EmployeeLogon()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                TheMessagesClass.CloseTheProgram();
            }
            catch (Exception Ex)
            {
                TheMessagesClass.ErrorMessage(Ex.ToString());
            }

            
        }
        private void LogonFailed()
        {
            gintNoOfMisses++;

            if (gintNoOfMisses == 3)
            {
                TheEventLogClass.InsertEventLogEntry(DateTime.Now, "There Have Been Three Attemps to Sign Into Blue Jay Design Program");

                TheMessagesClass.ErrorMessage("You Have Tried To Sign In Three Times\nThe Program Will Now Close");

                Application.Current.Shutdown();
            }
            else
            {
                TheMessagesClass.InformationMessage("You Have Failed The Sign In Process");
                return;
            }
        }

        private void btnSignIn_Click(object sender, RoutedEventArgs e)
        {
            //setting local variables
            string strValueForValidation;
            int intEmployeeID = 0;
            string strLastName;
            bool blnFatalError = false;
            int intRecordsReturned;
            string strErrorMessage = "";

            try
            {
                //beginning data validation
                strValueForValidation = pbxEmployeeID.Password;
                strLastName = txtLastName.Text;
                blnFatalError = TheDataValidationClass.VerifyIntegerData(strValueForValidation);
                if (blnFatalError == true)
                {
                    strErrorMessage = "The Employee ID is not an Integer\n";
                }
                else
                {
                    intEmployeeID = Convert.ToInt32(strValueForValidation);
                }
                if (strLastName == "")
                {
                    blnFatalError = true;
                    strErrorMessage += "The Last Name Was Not Entered\n";
                }
                if (blnFatalError == true)
                {
                    TheMessagesClass.ErrorMessage(strErrorMessage);
                    return;
                }

                //filling the data set
                MainWindow.TheVerifyDesignEmployeeLogonDataSet = TheEmployeeClass.VerifyDesignEmployeeLogon(intEmployeeID, strLastName);

                intRecordsReturned = MainWindow.TheVerifyDesignEmployeeLogonDataSet.VerifyDesigEmployeeLogon.Rows.Count;


                if (intRecordsReturned == 0)
                {
                    LogonFailed();
                }
                else
                {
                    Close();
                }
            }
            catch (Exception Ex)
            {
                TheEventLogClass.InsertEventLogEntry(DateTime.Now, "Blue Jay Design // Employee Logon Sign In Button " + Ex.Message);

                TheMessagesClass.ErrorMessage(Ex.ToString());
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            gintNoOfMisses = 0;
        }
    }
}
