/* Title:           Update Survey
 * Date:            2-11-19
 * Author:          Terry Holmes
 * 
 * Description:     This is used to update a survey */

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
using DesignProjectSurveyorDLL;
using DesignProjectDocumentation;
using DesignProjectUpdateDLL;

namespace BlueJayDesign
{
    /// <summary>
    /// Interaction logic for UpdateSurvey.xaml
    /// </summary>
    public partial class UpdateSurvey : Window
    {
        //setting up the classes
        WPFMessagesClass TheMessagesClass = new WPFMessagesClass();
        EventLogClass TheEventLogClass = new EventLogClass();
        DesignProjectsClass TheDesignProjectsClass = new DesignProjectsClass();
        DesignProjectsSurveyorClass TheDesignProjectsSurveyorClass = new DesignProjectsSurveyorClass();
        DesignProjectDocumentationClass TheDesignProjectDocumentationClass = new DesignProjectDocumentationClass();
        DesignProjectUpdateClass TheDesignProjectUpdateClass = new DesignProjectUpdateClass();

        //setting up data
        FindDesignProjectsByAssignedProjectIDDataSet TheFindDesignProjectsByAssignedProjectIDDataSet = new FindDesignProjectsByAssignedProjectIDDataSet();
        FindOpenDesignProjectSurveyorByProjectIDDataSet TheFindOpenDesignProjectSurveyorByProjectIDDataSet = new FindOpenDesignProjectSurveyorByProjectIDDataSet();

        string gstrSurveyorNotes;
        string gstrWOVStatus;
        int gintTransactionID;

        public UpdateSurvey()
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
            txtAddress.Text = "";
            txtCoordinator.Text = "";
            txtDateAssigned.Text = "";
            txtEnterProjectID.Text = "";
            txtJobStatus.Text = "";
            txtJobType.Text = "";
            txtProjectName.Text = "";
            txtSurveyor.Text = "";
            txtSurveyorNotes.Text = "";
            txtWOVStatus.Text = "";

            cboWOVStatus.Items.Clear();
            cboWOVStatus.Items.Add("Select WOV Status");
            cboWOVStatus.Items.Add("Assigned");
            cboWOVStatus.Items.Add("In Process");
            cboWOVStatus.Items.Add("Closed");
            cboWOVStatus.SelectedIndex = 0;
        }

        private void BtnFindProject_Click(object sender, RoutedEventArgs e)
        {
            //setting local variables
            int intRecordsReturned;
            

            try
            {
                MainWindow.gstrAssignedProjectID = txtEnterProjectID.Text;
                if(MainWindow.gstrAssignedProjectID == "")
                {
                    TheMessagesClass.ErrorMessage("The Project ID Was Not Entered");
                    return;
                }

                TheFindDesignProjectsByAssignedProjectIDDataSet = TheDesignProjectsClass.FindDesignProjectsByAssignedProjectID(MainWindow.gstrAssignedProjectID);

                intRecordsReturned = TheFindDesignProjectsByAssignedProjectIDDataSet.FindDesignProjectsByAssignedProjectID.Rows.Count;

                if (intRecordsReturned < 1)
                {
                    TheMessagesClass.ErrorMessage("The Project ID Was Not Found");
                    return;
                }

                MainWindow.gintProjectID = TheFindDesignProjectsByAssignedProjectIDDataSet.FindDesignProjectsByAssignedProjectID[0].ProjectID;

                TheFindOpenDesignProjectSurveyorByProjectIDDataSet = TheDesignProjectsSurveyorClass.FindOpenDesignProjectSurveyorByProjectID(MainWindow.gintProjectID);

                intRecordsReturned = TheFindOpenDesignProjectSurveyorByProjectIDDataSet.FindOpenDesignProjectSurveryorByProjectID.Rows.Count;

                if (intRecordsReturned < 1)
                {
                    TheMessagesClass.ErrorMessage("There Isn't An Open\nSurvey To Be Updated");
                    return;
                }

                gstrSurveyorNotes = TheFindOpenDesignProjectSurveyorByProjectIDDataSet.FindOpenDesignProjectSurveryorByProjectID[0].SurveyorNotes;
                gstrSurveyorNotes += "\n";
                MainWindow.gintTransactionID = TheFindOpenDesignProjectSurveyorByProjectIDDataSet.FindOpenDesignProjectSurveryorByProjectID[0].TransactionID;
                txtAddress.Text = TheFindDesignProjectsByAssignedProjectIDDataSet.FindDesignProjectsByAssignedProjectID[0].ProjectAddress;
                txtCoordinator.Text = TheFindDesignProjectsByAssignedProjectIDDataSet.FindDesignProjectsByAssignedProjectID[0].Coordinator;
                txtDateAssigned.Text = Convert.ToString(TheFindOpenDesignProjectSurveyorByProjectIDDataSet.FindOpenDesignProjectSurveryorByProjectID[0].DateAssigned);
                txtJobStatus.Text = TheFindDesignProjectsByAssignedProjectIDDataSet.FindDesignProjectsByAssignedProjectID[0].JobStatus;
                txtJobType.Text = TheFindDesignProjectsByAssignedProjectIDDataSet.FindDesignProjectsByAssignedProjectID[0].JobType;
                txtProjectName.Text = TheFindDesignProjectsByAssignedProjectIDDataSet.FindDesignProjectsByAssignedProjectID[0].ProjectName;
                txtSurveyor.Text = TheFindOpenDesignProjectSurveyorByProjectIDDataSet.FindOpenDesignProjectSurveryorByProjectID[0].FullName;
                txtWOVStatus.Text = TheFindOpenDesignProjectSurveyorByProjectIDDataSet.FindOpenDesignProjectSurveryorByProjectID[0].WOVStatus;
            }
            catch (Exception Ex)
            {
                TheEventLogClass.InsertEventLogEntry(DateTime.Now, "Blue Jay Design // Update Survey // Find Project Button " + Ex.Message);

                TheMessagesClass.ErrorMessage(Ex.ToString());
            }
        }

        private void BtnUpdate_Click(object sender, RoutedEventArgs e)
        {
            //setting local variables
            string strSurveyorNotes;
            bool blnFatalError = false;
            string strErrorMessage = "";

            try
            {
                if(cboWOVStatus.SelectedIndex < 1)
                {
                    blnFatalError = true;
                    strErrorMessage += "The WOV Status Was not Selected\n";
                }
                strSurveyorNotes = txtSurveyorNotes.Text;
                if(strSurveyorNotes == "")
                {
                    blnFatalError = true;
                    strErrorMessage += "There are No Surveyor Notes Entered";
                }
                if(blnFatalError == true)
                {
                    TheMessagesClass.ErrorMessage(strErrorMessage);
                    return;
                }

                strSurveyorNotes = Convert.ToString(DateTime.Now) + " " + strSurveyorNotes;
                gstrSurveyorNotes += strSurveyorNotes;

                if (gstrWOVStatus == "CLOSED")
                {
                    blnFatalError = TheDesignProjectsSurveyorClass.CloseDesignProjectSurveyorRecord(MainWindow.gintTransactionID, DateTime.Now, gstrSurveyorNotes);
                }
                else
                {
                    blnFatalError = TheDesignProjectsSurveyorClass.UpdateDesignProjectSurveyorWOVStatus(MainWindow.gintTransactionID, gstrWOVStatus, gstrSurveyorNotes);
                }

                if (blnFatalError == true)
                    throw new Exception();

                blnFatalError = TheDesignProjectUpdateClass.InsertIntoDesigProjectUpdates(MainWindow.gintProjectID, MainWindow.TheVerifyDesignEmployeeLogonDataSet.VerifyDesigEmployeeLogon[0].EmployeeID, strSurveyorNotes);

                if (blnFatalError == true)
                    throw new Exception();               

                TheMessagesClass.InformationMessage("The Survey Has Been Updated");

                EnterDesignWOVTechPay EnterDesignWOVTechPay = new EnterDesignWOVTechPay();
                EnterDesignWOVTechPay.ShowDialog();

                ResetControls();
            }
            catch (Exception Ex)
            {
                TheEventLogClass.InsertEventLogEntry(DateTime.Now, "Blue Jay Design // Update Survey // Update Button " + Ex.Message);

                TheMessagesClass.ErrorMessage(Ex.ToString());
            }
        }

        private void CboWOVStatus_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(cboWOVStatus.SelectedIndex > 0)
            {
                gstrWOVStatus = cboWOVStatus.SelectedItem.ToString().ToUpper();
            }
        }

        private void BtnAttachDocuments_Click(object sender, RoutedEventArgs e)
        {
            //setting local variables
            string strDocumentPath;
            string strDocumentType = "SURVEYOR DOCUMENTS";
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
                TheEventLogClass.InsertEventLogEntry(DateTime.Now, "Blue Jay Design// Update Survey // Attach Documents Button " + Ex.Message);

                TheMessagesClass.ErrorMessage(Ex.ToString());
            }
        }
    }
}
