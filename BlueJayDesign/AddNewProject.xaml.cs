/* Title:           Add New Project
 * Date:            2-4-19
 * Author:          Terry Holmes
 * 
 * Description:     This is used to add a new project */

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
using ProjectsDLL;
using DesignProjectsDLL;
using DesignProjectUpdateDLL;
using NewEventLogDLL;
using DataValidationDLL;
using NewEmployeeDLL;
using JobTypeDLL;
using DesignProjectDocumentation;
using WOVInvoicingDLL;

namespace BlueJayDesign
{
    /// <summary>
    /// Interaction logic for AddNewProject.xaml
    /// </summary>
    public partial class AddNewProject : Window
    {
        //setting up classes
        WPFMessagesClass TheMessagesClass = new WPFMessagesClass();
        ProjectClass TheProjectClass = new ProjectClass();
        DesignProjectsClass TheDesignProjectsClass = new DesignProjectsClass();
        DesignProjectUpdateClass TheDesignProjectUpdateClass = new DesignProjectUpdateClass();
        EventLogClass TheEventLogClass = new EventLogClass();
        DataValidationClass TheDataValidationClass = new DataValidationClass();
        EmployeeClass TheEmployeeClass = new EmployeeClass();
        JobTypeClass TheJobTypeClass = new JobTypeClass();
        DesignProjectDocumentationClass TheDesignProjectDocumentationClass = new DesignProjectDocumentationClass();
        WOVInvoicingClass TheWOVInvoicingClass = new WOVInvoicingClass();

        FindProjectByAssignedProjectIDDataSet TheFindProjectByAssignedProjectIDDataSet = new FindProjectByAssignedProjectIDDataSet();
        FindDesignProjectsByAssignedProjectIDDataSet TheFindDesignProjectByAssignedProjectIDDataSet = new FindDesignProjectsByAssignedProjectIDDataSet();
        FindSortedJobTypeDataSet TheFindSortedJobTypeDataSet = new FindSortedJobTypeDataSet();
        DesignDocumentsDataSet TheDesignDocumentsDataSet = new DesignDocumentsDataSet();
        FindSortedWOVBillingCodesDataSet TheFindSortedWOVBillingCodesDataSet = new FindSortedWOVBillingCodesDataSet();

        //setting global variables
        string gstrDocumentType;
        int gintBillingID;
              
        public AddNewProject()
        {
            InitializeComponent();
        }
        private void BtnHelp_Click(object sender, RoutedEventArgs e)
        {
            TheMessagesClass.LaunchHelpSite();
        }

        private void BtnEmail_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.SendEmailWindow.Visibility = Visibility.Visible;
            Visibility = Visibility.Hidden;
        }

        private void BtnAddNewProject_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.AddNewProjectWindow.Visibility = Visibility.Visible;
            Visibility = Visibility.Hidden;
        }


        private void BtnMyTasks_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.UpdateAssignTasksWindow.Visibility = Visibility.Visible;
            Visibility = Visibility.Hidden;
        }

        private void BtnAssignTask_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.AssignTasksWindow.Visibility = Visibility.Visible;
            Visibility = Visibility.Hidden;
        }

        private void BtnAssignSurveyor_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.AssignSurveyorWindow.Visibility = Visibility.Visible;
            Visibility = Visibility.Hidden;
        }

        private void BtnUpdateSurvey_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.UpdateSurveyWindow.Visibility = Visibility.Visible;
            Visibility = Visibility.Hidden;
        }

        private void BtnUpdateProject_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.UpdateProjectWindow.Visibility = Visibility.Visible;
            Visibility = Visibility.Hidden;
        }

        private void BtnCloseProject_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.CloseProjectWindow.Visibility = Visibility.Visible;
            Visibility = Visibility.Hidden;
        }

        private void BtnEmployeeProductivity_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.EmployeeProductivityWindow.Visibility = Visibility.Visible;
            Visibility = Visibility.Hidden;
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            TheMessagesClass.CloseTheProgram();
        }

        private void BtnHome_Click(object sender, RoutedEventArgs e)
        {
            Visibility = Visibility.Hidden;
        }
        private void BtnOpenDesignProjects_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.OpenDesignProjectWindow.Visibility = Visibility.Visible;
            Visibility = Visibility.Hidden;
        }

        private void BtnDesignProjectReport_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.DesignProjectReportwindow.Visibility = Visibility.Visible;
            Visibility = Visibility.Hidden;
        }

        private void BtnPolePermitReport_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.PolePermitReportWindow.Visibility = Visibility.Visible;
            Visibility = Visibility.Hidden;
        }

        private void BtnOpenSurveryorReport_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.OpenSurveyorReportWindow.Visibility = Visibility.Visible;
            Visibility = Visibility.Hidden;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Window_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            ResetControls();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ResetControls();
            
        }
        private void ResetControls()
        {
            //setting local variables
            int intCounter;
            int intNumberOfRecords;

            //clearing Controls
            txtAddress.Text = "";
            txtCity.Text = "";
            txtCoordinator.Text = "";
            txtDateReceived.Text = "";
            txtProjectID.Text = "";
            txtProjectName.Text = "";
            txtProjectNotes.Text = "";
            cboSelectOffice.Items.Clear();
            cboSelectJobType.Items.Clear();
            txtState.Text = "";
            txtZip.Text = "";

            //loading combo boxes
            cboSelectJobType.Items.Add("Select Job Type");

            TheFindSortedJobTypeDataSet = TheJobTypeClass.FindSortedJobType();

            intNumberOfRecords = TheFindSortedJobTypeDataSet.FindSortedJobType.Rows.Count - 1;

            for(intCounter = 0; intCounter <= intNumberOfRecords; intCounter++)
            {
                cboSelectJobType.Items.Add(TheFindSortedJobTypeDataSet.FindSortedJobType[intCounter].JobType);
            }
            
            cboSelectJobType.SelectedIndex = 0;

            MainWindow.TheFindWarehousesDataSet = TheEmployeeClass.FindWarehouses();

            intNumberOfRecords = MainWindow.TheFindWarehousesDataSet.FindWarehouses.Rows.Count - 1;
            cboSelectOffice.Items.Add("Select Office");

            for(intCounter = 0; intCounter <= intNumberOfRecords; intCounter++)
            {
                cboSelectOffice.Items.Add(MainWindow.TheFindWarehousesDataSet.FindWarehouses[intCounter].FirstName);
            }

            cboSelectOffice.SelectedIndex = 0;
            txtDateReceived.Text = Convert.ToString(DateTime.Now);
            txtDateReceived.IsReadOnly = true;
            txtDateReceived.Background = Brushes.LightGray;
            txtProjectNotes.Text = "CREATED NEW PROJECT";

            btnAttachDocuments.IsEnabled = false;
            TheDesignDocumentsDataSet.designdocuments.Rows.Clear();
            dgrResults.ItemsSource = TheDesignDocumentsDataSet.designdocuments;

            cboSelectBillingCode.Items.Clear();
            cboSelectBillingCode.Items.Add("Select Billing Codes");

            TheFindSortedWOVBillingCodesDataSet = TheWOVInvoicingClass.FindSortedWOVBillingCodes();

            intNumberOfRecords = TheFindSortedWOVBillingCodesDataSet.FindSortedWOVBillingCodes.Rows.Count - 1;

            for(intCounter = 0; intCounter <= intNumberOfRecords; intCounter++)
            {
                cboSelectBillingCode.Items.Add(TheFindSortedWOVBillingCodesDataSet.FindSortedWOVBillingCodes[intCounter].BillingDescription);
            }

            cboSelectBillingCode.SelectedIndex = 0;
        }

        private void BtnSubmit_Click(object sender, RoutedEventArgs e)
        {
            //setting local variables
            string strAssignedProjectID;
            string strProjectName;
            string strAddress;
            string strCity;
            DateTime datReceivedDate = DateTime.Now;
            string strCoordinator;
            string strNotes;
            string strErrorMessage = "";
            bool blnFatalError = false;
            int intRecordsReturned;
            string strZip;
            string strState;

            try
            {
                strAssignedProjectID = txtProjectID.Text;
                if(strAssignedProjectID == "")
                {
                    blnFatalError = true;
                    strErrorMessage += "The Project ID Was Not Entered\n";
                }
                strProjectName = txtProjectName.Text;
                if(strProjectName == "")
                {
                    blnFatalError = true;
                    strErrorMessage += "The Project Name Was Not Entered\n"; 
                }
                strAddress = txtAddress.Text;
                if(strAddress == "")
                {
                    blnFatalError = true;
                    strErrorMessage += "The Address Was Not Entered\n";
                }
                strCity = txtCity.Text;
                if(strCity == "")
                {
                    blnFatalError = true;
                    strErrorMessage += "The City Was Not Entered\n";
                }
                strState = txtState.Text;
                if(strState == "")
                {
                    blnFatalError = true;
                    strErrorMessage += "The State Was Not Entered\n";
                }
                strZip = txtZip.Text;
                if(strZip == "")
                {
                    blnFatalError = true;
                    strErrorMessage += "The Zip Was Not Entered\n";
                }
                if(cboSelectOffice.SelectedIndex < 1)
                {
                    blnFatalError = true;
                    strErrorMessage += "The Office Was Not Selected\n";
                }
                if(cboSelectJobType.SelectedIndex < 1)
                {
                    blnFatalError = true;
                    strErrorMessage += "The Job Type Was Not Selected\n";
                }
                strCoordinator = txtCoordinator.Text;
                if(strCoordinator == "")
                {
                    blnFatalError = true;
                    strErrorMessage += "Coordinate Was Not Entered\n";
                }

                datReceivedDate = Convert.ToDateTime(txtDateReceived.Text);

                if(txtProjectNotes.Text != "")
                {
                    strNotes = txtProjectNotes.Text;
                }
                else
                {
                    strNotes = "CREATED NEW PROJECT";
                }
                if(blnFatalError == true)
                {
                    TheMessagesClass.ErrorMessage(strErrorMessage);
                    return;
                }

                TheFindProjectByAssignedProjectIDDataSet = TheProjectClass.FindProjectByAssignedProjectID(strAssignedProjectID);

                intRecordsReturned = TheFindProjectByAssignedProjectIDDataSet.FindProjectByAssignedProjectID.Rows.Count;

                if(intRecordsReturned == 0)
                {
                    blnFatalError = TheProjectClass.InsertProject(strAssignedProjectID, strProjectName);

                    if (blnFatalError == true)
                        throw new Exception();

                    TheFindProjectByAssignedProjectIDDataSet = TheProjectClass.FindProjectByAssignedProjectID(strAssignedProjectID);
                }

                TheFindDesignProjectByAssignedProjectIDDataSet = TheDesignProjectsClass.FindDesignProjectsByAssignedProjectID(strAssignedProjectID);

                intRecordsReturned = TheFindDesignProjectByAssignedProjectIDDataSet.FindDesignProjectsByAssignedProjectID.Rows.Count;

                if(intRecordsReturned > 0)
                {
                    TheMessagesClass.ErrorMessage("Project Has Been Entered Already");
                    return;
                }

                MainWindow.gintProjectID = TheFindProjectByAssignedProjectIDDataSet.FindProjectByAssignedProjectID[0].ProjectID;

                blnFatalError = TheDesignProjectsClass.InsertDesignProject(MainWindow.gintProjectID, strAddress, strCity, MainWindow.gintWarehouseID, datReceivedDate, MainWindow.gintJobTypeID, strCoordinator, strNotes, strState, strZip);

                if (blnFatalError == true)
                    throw new Exception();

                blnFatalError = TheDesignProjectUpdateClass.InsertIntoDesigProjectUpdates(MainWindow.gintProjectID, MainWindow.TheVerifyDesignEmployeeLogonDataSet.VerifyDesigEmployeeLogon[0].EmployeeID, "NEW PROJECT ENTERED");

                if (blnFatalError == true)
                    throw new Exception();

                blnFatalError = TheDesignProjectsClass.UpdateDesignProjectBillingID(MainWindow.gintProjectID, gintBillingID);

                if (blnFatalError == true)
                    throw new Exception();

                TheMessagesClass.InformationMessage("The Project Has Been Entered");

                btnAttachDocuments.IsEnabled = true;

                
            }
            catch (Exception Ex)
            {
                TheEventLogClass.InsertEventLogEntry(DateTime.Now, "Blue Jay Design // Add New Project // Submit Button " + Ex.Message);

                TheMessagesClass.ErrorMessage(Ex.ToString());
            }

        }

        private void CboSelectJobType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int intSelectedIndex;

            intSelectedIndex = cboSelectJobType.SelectedIndex - 1;

            if (intSelectedIndex > -1)
            {
                MainWindow.gintJobTypeID = TheFindSortedJobTypeDataSet.FindSortedJobType[intSelectedIndex].JobTypeID;
            }
        }

        private void CboSelectOffice_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int intSelectedIndex;

            intSelectedIndex = cboSelectOffice.SelectedIndex - 1;

            if(intSelectedIndex > -1)
            {
                MainWindow.gintWarehouseID = MainWindow.TheFindWarehousesDataSet.FindWarehouses[intSelectedIndex].EmployeeID;
            }
        }

        private void BtnAttachDocuments_Click(object sender, RoutedEventArgs e)
        {
            //setting local variables
            string strDocumentPath;
            string strDocumentType = "PERMIT DOCUMENTS";
            string strNewLocation = "";
            bool blnFatalError = false;
            DateTime datTransactionDate = DateTime.Now;
            int intCounter;
            int intNumberOfRecords;

            try
            {

                Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
                dlg.Multiselect = true;
                dlg.FileName = "Document"; // Default file name

                // Show open file dialog box
                Nullable<bool> result = dlg.ShowDialog();

                // Process open file dialog box results
                if (result == true)
                {
                    intNumberOfRecords = dlg.FileNames.Length - 1;

                    if (intNumberOfRecords > -1)
                    {
                        for (intCounter = 0; intCounter <= intNumberOfRecords; intCounter++)
                        {
                            strDocumentPath = dlg.FileNames[intCounter].ToUpper();

                            blnFatalError = TheDesignProjectDocumentationClass.InsertDesignProjectDocumentation(MainWindow.gintProjectID, MainWindow.TheVerifyDesignEmployeeLogonDataSet.VerifyDesigEmployeeLogon[0].EmployeeID, datTransactionDate, strDocumentType, strDocumentPath);

                            if (blnFatalError == true)
                                throw new Exception();
                        }
                    }
                }
                else
                {
                    return;
                }

                TheMessagesClass.InformationMessage("The Documents have been Added");
                ResetControls();
            }
            catch (Exception Ex)
            {
                TheEventLogClass.InsertEventLogEntry(DateTime.Now, "Blue Jay Design// Add New Project // Attach Documents Button " + Ex.Message);

                TheMessagesClass.ErrorMessage(Ex.ToString());
            }
        }        

        private void BtnViewProjectInfo_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.ViewProjectInfoWindow.Visibility = Visibility.Visible;
            this.Visibility = Visibility.Hidden;
        }

        private void CboSelectBillingCode_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int intSelectedIndex;

            intSelectedIndex = cboSelectBillingCode.SelectedIndex - 1;

            if(intSelectedIndex > -1)
            {
                gintBillingID = TheFindSortedWOVBillingCodesDataSet.FindSortedWOVBillingCodes[intSelectedIndex].BillingID;
            }
        }
    }
}
