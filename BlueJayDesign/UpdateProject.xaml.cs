/* Title:           Update Project
 * Date:            2-13-19
 * Author:          Terry Holmes
 * 
 * Description:     This is where a project would get updated */

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
using DesignProjectDocumentation;

namespace BlueJayDesign
{
    /// <summary>
    /// Interaction logic for UpdateProject.xaml
    /// </summary>
    public partial class UpdateProject : Window
    {
        WPFMessagesClass TheMessagesClass = new WPFMessagesClass();
        EventLogClass TheEventLogClass = new EventLogClass();
        DesignProjectsClass TheDesignProjectsClass = new DesignProjectsClass();
        DesignProjectUpdateClass TheDesignProjectUpdateClass = new DesignProjectUpdateClass();
        DesignProjectDocumentationClass TheDesignProjectDocumentationClass = new DesignProjectDocumentationClass();

        //setting up the data
        FindDesignProjectsByAssignedProjectIDDataSet TheFindDesignProjectsByAssignedProjectIDDataSet = new FindDesignProjectsByAssignedProjectIDDataSet();
        string gstrProjectNotes;

        string gstrJobStatus;

        public UpdateProject()
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
            txtDateReceived.Text = "";
            txtCity.Text = "";
            txtCoordinator.Text = "";
            txtEnterProjectID.Text = "";
            txtJobStatus.Text = "";
            txtJobType.Text = "";
            txtNewProjectNotes.Text = "";
            txtProjectAddress.Text = "";
            txtProjectName.Text = "";
            txtProjectNotes.Text = "";

            cboSelectStatus.Items.Clear();
            cboSelectStatus.Items.Add("Select Status");
            cboSelectStatus.Items.Add("Assigned");
            cboSelectStatus.Items.Add("WIP");
            cboSelectStatus.Items.Add("QC");
            cboSelectStatus.SelectedIndex = 0;
        }

        private void BtnFind_Click(object sender, RoutedEventArgs e)
        {
            int intRecordsReturned;
            bool blnFatalError = false;

            try
            {
                MainWindow.gstrAssignedProjectID = txtEnterProjectID.Text;
                if(MainWindow.gstrAssignedProjectID == "")
                {
                    TheMessagesClass.ErrorMessage("The Project ID Was Not Entered");
                    return;
                }

                //getting the data
                TheFindDesignProjectsByAssignedProjectIDDataSet = TheDesignProjectsClass.FindDesignProjectsByAssignedProjectID(MainWindow.gstrAssignedProjectID);

                intRecordsReturned = TheFindDesignProjectsByAssignedProjectIDDataSet.FindDesignProjectsByAssignedProjectID.Rows.Count;

                if(intRecordsReturned < 1)
                {
                    TheMessagesClass.ErrorMessage("Project Was Not Found");
                    return;
                }

                txtDateReceived.Text = Convert.ToString(TheFindDesignProjectsByAssignedProjectIDDataSet.FindDesignProjectsByAssignedProjectID[0].DateReceived);
                txtCity.Text = TheFindDesignProjectsByAssignedProjectIDDataSet.FindDesignProjectsByAssignedProjectID[0].City;
                txtCoordinator.Text = TheFindDesignProjectsByAssignedProjectIDDataSet.FindDesignProjectsByAssignedProjectID[0].Coordinator;
                txtJobStatus.Text = TheFindDesignProjectsByAssignedProjectIDDataSet.FindDesignProjectsByAssignedProjectID[0].JobStatus;
                txtJobType.Text = TheFindDesignProjectsByAssignedProjectIDDataSet.FindDesignProjectsByAssignedProjectID[0].JobType;
                txtProjectAddress.Text = TheFindDesignProjectsByAssignedProjectIDDataSet.FindDesignProjectsByAssignedProjectID[0].ProjectAddress;
                txtProjectName.Text = TheFindDesignProjectsByAssignedProjectIDDataSet.FindDesignProjectsByAssignedProjectID[0].ProjectName;
                txtProjectNotes.Text = TheFindDesignProjectsByAssignedProjectIDDataSet.FindDesignProjectsByAssignedProjectID[0].ProjectNotes;
                MainWindow.gintProjectID = TheFindDesignProjectsByAssignedProjectIDDataSet.FindDesignProjectsByAssignedProjectID[0].ProjectID;
                MainWindow.gintTransactionID = TheFindDesignProjectsByAssignedProjectIDDataSet.FindDesignProjectsByAssignedProjectID[0].TransactionID;
                MainWindow.gstrProjectName = TheFindDesignProjectsByAssignedProjectIDDataSet.FindDesignProjectsByAssignedProjectID[0].ProjectName;
                gstrProjectNotes = txtProjectNotes.Text + "\n";
            }
            catch (Exception Ex)
            {
                TheEventLogClass.InsertEventLogEntry(DateTime.Now, "Blue Jay Design // Update Project // Find Button " + Ex.Message);

                TheMessagesClass.ErrorMessage(Ex.ToString());
            }
        }

        private void CboSelectStatus_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cboSelectStatus.SelectedIndex > 0)
                gstrJobStatus = cboSelectStatus.SelectedItem.ToString().ToUpper();
        }

        private void BtnPermits_Click(object sender, RoutedEventArgs e)
        {
            AddEditDesignProjectPermits AddEditDesignProjectPermits = new AddEditDesignProjectPermits();
            AddEditDesignProjectPermits.ShowDialog();
        }

        private void BtnAddDocuments_Click(object sender, RoutedEventArgs e)
        {
            //setting local variables
            string strDocumentPath;
            string strDocumentType = "PROJECT DOCUMENTS";
            //string strNewLocation = "";
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
                TheEventLogClass.InsertEventLogEntry(DateTime.Now, "Blue Jay Design// Update Project // Attach Documents Button " + Ex.Message);

                TheMessagesClass.ErrorMessage(Ex.ToString());
            }
        }

        private void BtnUpdate_Click(object sender, RoutedEventArgs e)
        {
            bool blnFatalError = false;
            string strErrorMessage = "";
            string strProjectNotes = "";

            try
            {
                if(cboSelectStatus.SelectedIndex < 1)
                {
                    blnFatalError = true;
                    strErrorMessage += "Status Was Not Selected\n";
                }
                strProjectNotes = txtNewProjectNotes.Text;
                if(strProjectNotes == "")
                {
                    blnFatalError = true;
                    strErrorMessage += "Project Notes Were Not Entered\n";
                }
                if(blnFatalError == true)
                {
                    TheMessagesClass.ErrorMessage(strErrorMessage);
                    return;
                }

                gstrProjectNotes += strProjectNotes;

                blnFatalError = TheDesignProjectsClass.UpdateDesignProjectJobStatus(MainWindow.gintTransactionID, gstrJobStatus);

                if (blnFatalError == true)
                    throw new Exception();

                blnFatalError = TheDesignProjectUpdateClass.InsertIntoDesigProjectUpdates(MainWindow.gintProjectID, MainWindow.TheVerifyDesignEmployeeLogonDataSet.VerifyDesigEmployeeLogon[0].EmployeeID, strProjectNotes);

                if (blnFatalError == true)
                    throw new Exception();

                TheMessagesClass.InformationMessage("The Project Has Been Updated");

                ResetControls();

            }
            catch (Exception Ex)
            {
                TheEventLogClass.InsertEventLogEntry(DateTime.Now, "Blue Jay Design // Update Project // Update Button " + Ex.Message);

                return;
            }
        }
    }
}
