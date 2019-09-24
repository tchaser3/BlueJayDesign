/* Title:           Close Design Project
 * Date:            2-14-19
 * Author:          Terry Holmes
 * 
 * Description:     This is the window that a design application will be closed */

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
using DesignProjectsDLL;
using DesignProjectUpdateDLL;
using DesignPermitsDLL;
using DesignProjectSurveyorDLL;
using DataValidationDLL;
using DesignProjectDocumentation;

namespace BlueJayDesign
{
    /// <summary>
    /// Interaction logic for CloseProject.xaml
    /// </summary>
    public partial class CloseProject : Window
    {
        //setting up the classes
        WPFMessagesClass TheMessagesClass = new WPFMessagesClass();
        EventLogClass TheEventLogClass = new EventLogClass();
        DesignProjectsClass TheDesignProjectsClass = new DesignProjectsClass();
        DesignProjectUpdateClass TheDesignProjectUpdateClass = new DesignProjectUpdateClass();
        DesignPermitsClass TheDesignPermitsClass = new DesignPermitsClass();
        DesignProjectsSurveyorClass TheDesignProjectsSurveyorClass = new DesignProjectsSurveyorClass();
        DataValidationClass TheDataValidationClass = new DataValidationClass();
        DesignProjectDocumentationClass TheDesignProjectDocumentationClass = new DesignProjectDocumentationClass();

        //setting up data
        FindDesignProjectsByAssignedProjectIDDataSet TheFindDesignProjectsByAssignedProjectIDDataSet = new FindDesignProjectsByAssignedProjectIDDataSet();
        FindOpenDesignProjectSurveyorByProjectIDDataSet TheFindOpenDesignProjectSurveyorByProjectIDDataSet = new FindOpenDesignProjectSurveyorByProjectIDDataSet();
        FindOpenPermitsByProjectIDDataSet TheFindOpenPermitsByProjectIDDataSet = new FindOpenPermitsByProjectIDDataSet();

        public CloseProject()
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
        private void BtnViewProjectInfo_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.ViewProjectInfoWindow.Visibility = Visibility.Visible;
            this.Visibility = Visibility.Hidden;
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
            txtEnterProjectID.Text = "";
            txtBJCInvoice.Text = "";
            txtCity.Text = "";
            txtCoordinator.Text = "";
            txtDateReceived.Text = "";
            txtEnterProjectID.Text = "";
            txtJobStatus.Text = "";
            txtJobType.Text = "";
            txtProjectAddress.Text = "";
            txtProjectName.Text = "";
            txtProjectNotes.Text = "";
            txtSurveyInvoice.Text = "";
            btnCloseTheProject.IsEnabled = false;
            txtBJCInvoice.IsEnabled = false;
            txtSurveyInvoice.IsEnabled = false;
            btnAddDocuments.IsEnabled = false;
        }
        private void EnableControls()
        {
            btnCloseTheProject.IsEnabled = true;
            txtBJCInvoice.IsEnabled = true;
            txtSurveyInvoice.IsEnabled = true;
            btnAddDocuments.IsEnabled = true;
        }
        private void BtnFind_Click(object sender, RoutedEventArgs e)
        {
            string strAssignedProjectID;
            int intRecordsReturned;

            strAssignedProjectID = txtEnterProjectID.Text;
            if(strAssignedProjectID == "")
            {
                TheMessagesClass.ErrorMessage("The Project ID Was Not Entered");
                return;
            }

            TheFindDesignProjectsByAssignedProjectIDDataSet = TheDesignProjectsClass.FindDesignProjectsByAssignedProjectID(strAssignedProjectID);

            intRecordsReturned = TheFindDesignProjectsByAssignedProjectIDDataSet.FindDesignProjectsByAssignedProjectID.Rows.Count;

            if(intRecordsReturned < 1)
            {
                TheMessagesClass.ErrorMessage("The Project Was Not Found");
                return;
            }

            MainWindow.gintProjectID = TheFindDesignProjectsByAssignedProjectIDDataSet.FindDesignProjectsByAssignedProjectID[0].ProjectID;
            MainWindow.gintTransactionID = TheFindDesignProjectsByAssignedProjectIDDataSet.FindDesignProjectsByAssignedProjectID[0].TransactionID;

            TheFindOpenDesignProjectSurveyorByProjectIDDataSet = TheDesignProjectsSurveyorClass.FindOpenDesignProjectSurveyorByProjectID(MainWindow.gintProjectID);

            intRecordsReturned = TheFindOpenDesignProjectSurveyorByProjectIDDataSet.FindOpenDesignProjectSurveryorByProjectID.Rows.Count;

            if(intRecordsReturned > 0)
            {
                TheMessagesClass.ErrorMessage("There Are Still Open Surveys For This Project");
                return;
            }

            TheFindOpenPermitsByProjectIDDataSet = TheDesignPermitsClass.FindOpenPermitsByProjectID(MainWindow.gintProjectID);

            intRecordsReturned = TheFindOpenPermitsByProjectIDDataSet.FindOpenPermitsByProjectID.Rows.Count;

            if(intRecordsReturned > 0)
            {
                TheMessagesClass.ErrorMessage("There Are Still Open Permits For This Project");
                return;
            }

            txtCity.Text = TheFindDesignProjectsByAssignedProjectIDDataSet.FindDesignProjectsByAssignedProjectID[0].City;
            txtCoordinator.Text = TheFindDesignProjectsByAssignedProjectIDDataSet.FindDesignProjectsByAssignedProjectID[0].Coordinator;
            txtDateReceived.Text = Convert.ToString(TheFindDesignProjectsByAssignedProjectIDDataSet.FindDesignProjectsByAssignedProjectID[0].DateReceived);
            txtJobStatus.Text = TheFindDesignProjectsByAssignedProjectIDDataSet.FindDesignProjectsByAssignedProjectID[0].JobStatus;
            txtJobType.Text = TheFindDesignProjectsByAssignedProjectIDDataSet.FindDesignProjectsByAssignedProjectID[0].JobType;
            txtProjectAddress.Text = TheFindDesignProjectsByAssignedProjectIDDataSet.FindDesignProjectsByAssignedProjectID[0].ProjectAddress;
            txtProjectName.Text = TheFindDesignProjectsByAssignedProjectIDDataSet.FindDesignProjectsByAssignedProjectID[0].ProjectName;
            txtProjectNotes.Text = TheFindDesignProjectsByAssignedProjectIDDataSet.FindDesignProjectsByAssignedProjectID[0].ProjectNotes;

            EnableControls();
        }

        private void BtnAddDocuments_Click(object sender, RoutedEventArgs e)
        {
            //setting local variables
            string strDocumentPath;
            string strDocumentType = "PERMIT DOCUMENTS";
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
            }
            catch (Exception Ex)
            {
                TheEventLogClass.InsertEventLogEntry(DateTime.Now, "Blue Jay Design// Close Project // Attach Documents Button " + Ex.Message);

                TheMessagesClass.ErrorMessage(Ex.ToString());
            }
        }

        private void BtnCloseTheProject_Click(object sender, RoutedEventArgs e)
        {
            string strValueForValidation;
            string strErrorMessage = "";
            bool blnFatalError = false;
            bool blnThereIsAProblem = false;
            decimal decBJCInvoice = 0;
            decimal decSurveyInvoice = 0;
            DateTime datClosingDate = DateTime.Now;

            try
            {
                strValueForValidation = txtBJCInvoice.Text;
                blnThereIsAProblem = TheDataValidationClass.VerifyDoubleData(strValueForValidation);
                if(blnThereIsAProblem == true)
                {
                    blnFatalError = true;
                    strErrorMessage += "BJC Invoice is not Numeric\n";
                }
                else
                {
                    decBJCInvoice = Convert.ToDecimal(strValueForValidation);
                }
                strValueForValidation = txtSurveyInvoice.Text;
                blnThereIsAProblem = TheDataValidationClass.VerifyDoubleData(strValueForValidation);
                if (blnThereIsAProblem == true)
                {
                    blnFatalError = true;
                    strErrorMessage += "Survey Invoice is not Numeric\n";
                }
                else
                {
                    decSurveyInvoice = Convert.ToDecimal(strValueForValidation);
                }
                if(blnFatalError == true)
                {
                    TheMessagesClass.ErrorMessage(strErrorMessage);
                    return;
                }

                blnFatalError = TheDesignProjectsClass.UpdateDesignProjectBJCInvoice(MainWindow.gintTransactionID, decBJCInvoice);

                if (blnFatalError == true)
                    throw new Exception();

                blnFatalError = TheDesignProjectsClass.UpdateDesignProjectSurveyInvoice(MainWindow.gintTransactionID, decSurveyInvoice);

                if (blnFatalError == true)
                    throw new Exception();

                blnFatalError = TheDesignProjectsClass.UpdateDesignProjectJobStatus(MainWindow.gintTransactionID, "INVOICED");

                if (blnFatalError == true)
                    throw new Exception();

                TheMessagesClass.InformationMessage("The Project Has Been Closed");

                ResetControls();
            }
            catch (Exception Ex)
            {
                TheEventLogClass.InsertEventLogEntry(DateTime.Now, "Blue Jay Design // Close Project // Close The Project Button " + Ex.Message);

                TheMessagesClass.ErrorMessage(Ex.ToString());
            }
        }
    }
}
