﻿/* Title:           Add Edit Design Project Permits
 * Date:            2-13-19
 * Author:          Terry Holmes
 * 
 * Description:     This is used  to Add or Edit Permits */

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
using NewEventLogDLL;
using DesignPermitsDLL;
using DesignProjectDocumentation;
using DataValidationDLL;
using DesignProjectUpdateDLL;

namespace BlueJayDesign
{
    /// <summary>
    /// Interaction logic for AddEditDesignProjectPermits.xaml
    /// </summary>
    public partial class AddEditDesignProjectPermits : Window
    {
        //setting up the classes
        WPFMessagesClass TheMessagesClass = new WPFMessagesClass();
        EventLogClass TheEventLogClass = new EventLogClass();
        DesignPermitsClass TheDesignPermitsClass = new DesignPermitsClass();
        DesignProjectDocumentationClass TheDesignProjectDocumentationClass = new DesignProjectDocumentationClass();
        DesignProjectUpdateClass TheDesignProjectUpdateClass = new DesignProjectUpdateClass();
        DataValidationClass TheDataValidationClass = new DataValidationClass();

        //setting up data
        FindPermitsByProjectIDDataSet TheFindPermitsByProjectIDDataSet = new FindPermitsByProjectIDDataSet();

        //Setting global Variables
        string gstrPermitStatus;
        bool gblnNewPermit;
        bool gblnItemSelected;
        int gintTransactionID;
        string gstrPermitNotes;

        public AddEditDesignProjectPermits()
        {
            InitializeComponent();
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void BtnHelp_Click(object sender, RoutedEventArgs e)
        {
            TheMessagesClass.LaunchHelpSite();
        }
        private  void ResetrControls()
        {
            //this will load up the controls
            int intRecordsReturned;

            try
            {
                txtProjectID.Text = MainWindow.gstrAssignedProjectID;
                txtProjectName.Text = MainWindow.gstrProjectName;
                txtDateReceived.Text = Convert.ToString(DateTime.Now);
                txtPermitNotes.Text = "";
                txtQuantity.Text = "";
                EnableControls(false);
                gblnItemSelected = false;
                gstrPermitNotes = "";

                cboPermitStatus.Items.Clear();
                cboPermitStatus.Items.Add("Select Permit Status");
                cboPermitStatus.Items.Add("Opened");
                cboPermitStatus.Items.Add("Assigned");
                cboPermitStatus.Items.Add("In Process");
                cboPermitStatus.Items.Add("QC");
                cboPermitStatus.Items.Add("Closed");
                cboPermitStatus.SelectedIndex = 0;

                TheFindPermitsByProjectIDDataSet = TheDesignPermitsClass.FindPermitsByProjectID(MainWindow.gintProjectID);

                intRecordsReturned = TheFindPermitsByProjectIDDataSet.FindPermitsByProjectID.Rows.Count;

                if (intRecordsReturned < 1)
                {
                    TheMessagesClass.InformationMessage("There Are No Permits Assigned To This Project");
                    btnEditPermit.IsEnabled = false;
                }

                dgrExistingPermits.ItemsSource = TheFindPermitsByProjectIDDataSet.FindPermitsByProjectID;
            }
            catch (Exception Ex)
            {
                TheEventLogClass.InsertEventLogEntry(DateTime.Now, "Blue Jay Design // Add Edit Design Permits // Window Loaded " + Ex.Message);

                TheMessagesClass.ErrorMessage(Ex.ToString());
            }
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ResetrControls();
        }
        
        private void CboPermitStatus_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cboPermitStatus.SelectedIndex > 0)
                gstrPermitStatus = cboPermitStatus.SelectedItem.ToString().ToUpper();
        }

        private void BtnNewPermit_Click(object sender, RoutedEventArgs e)
        {
            gblnNewPermit = true;
            EnableControls(true);
        }

        private void BtnEditPermit_Click(object sender, RoutedEventArgs e)
        {
            gblnNewPermit = false;
            EnableControls(true);
        }
       
        private void BtnProcess_Click(object sender, RoutedEventArgs e)
        {
            //setting local variables
            int intQuantity = 0;
            string strValueForValidation;
            string strPermitNotes;
            bool blnFatalError = false;
            bool blnThereIsAProblem = false;
            string strErrorMessage = "";
            DateTime datTransactionDate = DateTime.Now;

            try
            {
                //data validation
                datTransactionDate = Convert.ToDateTime(txtDateReceived.Text);
                if(cboPermitStatus.SelectedIndex < 1)
                {
                    blnFatalError = true;
                    strErrorMessage += "Permit Status Was Not Selected\n";
                }
                strValueForValidation = txtQuantity.Text;
                blnThereIsAProblem = TheDataValidationClass.VerifyIntegerData(strValueForValidation);
                if(blnThereIsAProblem == true)
                {
                    blnFatalError = true;
                    strErrorMessage += "The Quantity is not an Integer\n";
                }
                else
                {
                    intQuantity = Convert.ToInt32(strValueForValidation);
                }
                strPermitNotes = txtPermitNotes.Text;
                if(strPermitNotes == "")
                {
                    blnFatalError = true;
                    strErrorMessage += "Permit Notes Were Not Added\n";
                }
                else
                {
                    gstrPermitNotes += strPermitNotes;
                }
                if(blnFatalError == true)
                {
                    TheMessagesClass.ErrorMessage(strErrorMessage);
                    return;
                }

                if(gblnNewPermit == true)
                {
                    blnFatalError = TheDesignPermitsClass.InsertDesignPermit(MainWindow.gintProjectID, datTransactionDate, intQuantity, gstrPermitStatus, MainWindow.TheVerifyDesignEmployeeLogonDataSet.VerifyDesigEmployeeLogon[0].EmployeeID, gstrPermitNotes);
                }
                if(gblnNewPermit == false)
                {
                    if (gblnItemSelected == false)
                    {
                        TheMessagesClass.ErrorMessage("A Permit Has Not Been Selected");
                        return;
                    }
                    if(gstrPermitStatus == "CLOSED")
                    {
                        blnFatalError = TheDesignPermitsClass.CloseDesignPermit(gintTransactionID, DateTime.Now, gstrPermitNotes);
                    }
                    else
                    {
                        blnFatalError = TheDesignPermitsClass.UpdateDesignPermitStatus(gintTransactionID, gstrPermitStatus, gstrPermitNotes);
                    }
                }

                if (blnFatalError == true)
                    throw new Exception();

                blnFatalError = TheDesignProjectUpdateClass.InsertIntoDesigProjectUpdates(MainWindow.gintProjectID, MainWindow.TheVerifyDesignEmployeeLogonDataSet.VerifyDesigEmployeeLogon[0].EmployeeID, strPermitNotes);

                if (blnFatalError == true)
                    throw new Exception();

                TheMessagesClass.InformationMessage("Permit Has Been Entered/Updated");

                EnterDesignWOVTechPay EnterDesignWOVTechPay = new EnterDesignWOVTechPay();
                EnterDesignWOVTechPay.ShowDialog();

                ResetrControls();
            }
            catch (Exception Ex)
            {
                TheEventLogClass.InsertEventLogEntry(DateTime.Now, "Blue Jay Design // Add Edit Design Project Permits // Process Button " + Ex.Message);

                TheMessagesClass.ErrorMessage(Ex.ToString());
            }
        }

        private void BtnAddDocuments_Click(object sender, RoutedEventArgs e)
        {
            //setting local variables
            string strDocumentPath;
            string strDocumentType = "PERMIT DOCUMENTS";
            string strNewLocation = "";
            bool blnFatalError = false;
            DateTime datTransactionDate = DateTime.Now;

            try
            {

                Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
                dlg.FileName = "Document"; // Default file name

                // Show open file dialog box
                Nullable<bool> result = dlg.ShowDialog();

                // Process open file dialog box results
                if (result == true)
                {
                    // Open document
                    strDocumentPath = dlg.FileName.ToUpper();
                }
                else
                {
                    return;
                }

                //strNewLocation = "\\\\bjc\\shares\\Documents\\";
                //strDocumentPath = strDocumentPath.Replace("\\", "\\\\");
                //strNewLocation += strDocumentPath.Substring(3);
                //TheMessagesClass.ErrorMessage(strNewLocation);

                blnFatalError = TheDesignProjectDocumentationClass.InsertDesignProjectDocumentation(MainWindow.gintProjectID, MainWindow.TheVerifyDesignEmployeeLogonDataSet.VerifyDesigEmployeeLogon[0].EmployeeID, datTransactionDate, strDocumentType, strDocumentPath);

                if (blnFatalError == true)
                    throw new Exception();

                TheMessagesClass.InformationMessage("The Documents have been Added");
            }
            catch (Exception Ex)
            {
                TheEventLogClass.InsertEventLogEntry(DateTime.Now, "Blue Jay Design// Add Edit Design Project Permits // Attach Documents Button " + Ex.Message);

                TheMessagesClass.ErrorMessage(Ex.ToString());
            }
        }
        private void EnableControls(bool blnValueBoolean)
        {
            btnProcess.IsEnabled = blnValueBoolean;
            txtQuantity.IsEnabled = blnValueBoolean;
            txtPermitNotes.IsEnabled = blnValueBoolean;
            cboPermitStatus.IsEnabled = blnValueBoolean;
            dgrExistingPermits.IsEnabled = blnValueBoolean;
        }

        private void DgrExistingPermits_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGrid dataGrid;
            DataGridRow selectedRow;
            DataGridCell TransactionID;
            string strTransactionID;
            DataGridCell PermitNotes;

            try
            {
                if (dgrExistingPermits.SelectedIndex > -1)
                {
                    //setting local variable
                    dataGrid = dgrExistingPermits;
                    selectedRow = (DataGridRow)dataGrid.ItemContainerGenerator.ContainerFromIndex(dataGrid.SelectedIndex);
                    TransactionID = (DataGridCell)dataGrid.Columns[0].GetCellContent(selectedRow).Parent;
                    strTransactionID = ((TextBlock)TransactionID.Content).Text;
                    PermitNotes = (DataGridCell)dataGrid.Columns[6].GetCellContent(selectedRow).Parent;
                    gstrPermitNotes = ((TextBlock)PermitNotes.Content).Text;
                    gstrPermitStatus += " \n";

                    gintTransactionID = Convert.ToInt32(strTransactionID);
                    gblnItemSelected = true;
                }

            }
            catch (Exception Ex)
            {
                TheEventLogClass.InsertEventLogEntry(DateTime.Now, "Blue Jay Design // Add Edit Design Project Permits // Grid Selection " + Ex.Message);

                TheMessagesClass.ErrorMessage(Ex.ToString());
            }
        }
    }
}
