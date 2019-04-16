/* Title:           Assign Surveyor
 * Date:            2-4-19
 * Author:          Terry Holmes
 * 
 * Description:     This Window Will assign the surveryor */

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
using NewEmployeeDLL;
using DesignProjectSurveyorDLL;
using DesignProjectsDLL;
using DataValidationDLL;
using DesignProjectUpdateDLL;

namespace BlueJayDesign
{
    /// <summary>
    /// Interaction logic for AssignSurveyor.xaml
    /// </summary>
    public partial class AssignSurveyor : Window
    {
        //Setting up the classes
        WPFMessagesClass TheMessagesClass = new WPFMessagesClass();
        EventLogClass TheEVentLogClass = new EventLogClass();
        EmployeeClass TheEmployeeClass = new EmployeeClass();
        DesignProjectsSurveyorClass TheDesignProjectsSurveyorClass = new DesignProjectsSurveyorClass();
        DesignProjectsClass TheDesignProjectsClass = new DesignProjectsClass();
        SendEmailClass TheSendEmailClass = new SendEmailClass();
        DataValidationClass TheDataValidationClass = new DataValidationClass();
        DesignProjectUpdateClass TheDesignProjectUpdateClass = new DesignProjectUpdateClass();

        //setting up data
        FindDesignProjectsByAssignedProjectIDDataSet TheFindDesignProjectsByAssignedProjectIDDataSet = new FindDesignProjectsByAssignedProjectIDDataSet();
        ComboEmployeeDataSet TheComboEmployeeDataSet = new ComboEmployeeDataSet();
        FindEmployeeEmailAddressDataSet TheFindEmployeeEmailAddressDataSet = new FindEmployeeEmailAddressDataSet();
        FindOpenDesignProjectSurveyorByProjectIDDataSet TheFindOpenDesignProjectSurveyorByProjectIDDataSet = new FindOpenDesignProjectSurveyorByProjectIDDataSet();

        //setting up global variables
        string gstrProjectNotes;
        string gstrFullName;

        public AssignSurveyor()
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
            txtDateAssigned.Text = Convert.ToString(DateTime.Now);
            txtEnterLastName.Text = "";
            txtEnterProjectID.Text = "";
            txtSurveyorNotes.Text = "";
            txtWOVStatus.Text = "ASSIGNED";
            txtReWalk.Text = "";
            txtProjectName.Text = "";
            txtCity.Text = "";
            txtJobType.Text = "";
            txtDateReceived.Text = "";
            txtCoordinator.Text = "";
            txtStatus.Text = "";
            cboSelectSurveyor.Items.Clear();
            SetControlsEnabled(false);
        }
        private void SetControlsEnabled(bool ValueBoolean)
        {
            txtEnterLastName.IsEnabled = ValueBoolean;
            txtSurveyorNotes.IsEnabled = ValueBoolean;
            cboSelectSurveyor.IsEnabled = ValueBoolean;
            btnSendEmail.IsEnabled = ValueBoolean;
            btnSubmit.IsEnabled = ValueBoolean;
            txtReWalk.IsEnabled = ValueBoolean;
        }

        private void BtnFind_Click(object sender, RoutedEventArgs e)
        {
            //this will find the project
            //setting local variables
            //string strProjectID;
            int intRecordsReturned;

            MainWindow.gstrAssignedProjectID = txtEnterProjectID.Text;
            if(MainWindow.gstrAssignedProjectID == "")
            {
                TheMessagesClass.ErrorMessage("The Project ID Was Not Entered");
                return;
            }

            //getting the data
            TheFindDesignProjectsByAssignedProjectIDDataSet = TheDesignProjectsClass.FindDesignProjectsByAssignedProjectID(MainWindow.gstrAssignedProjectID);

            intRecordsReturned = TheFindDesignProjectsByAssignedProjectIDDataSet.FindDesignProjectsByAssignedProjectID.Rows.Count;

            if(intRecordsReturned == 0)
            {
                TheMessagesClass.ErrorMessage("The Project Was Not Found");
                return;
            }

            MainWindow.gintProjectID = TheFindDesignProjectsByAssignedProjectIDDataSet.FindDesignProjectsByAssignedProjectID[0].ProjectID;

            TheFindOpenDesignProjectSurveyorByProjectIDDataSet = TheDesignProjectsSurveyorClass.FindOpenDesignProjectSurveyorByProjectID(MainWindow.gintProjectID);

            intRecordsReturned = TheFindOpenDesignProjectSurveyorByProjectIDDataSet.FindOpenDesignProjectSurveryorByProjectID.Rows.Count;

            if(intRecordsReturned > 0)
            {
                TheMessagesClass.ErrorMessage("There Is An Open WOV Assigned");
                return;
            }

            MainWindow.gintTransactionID = TheFindDesignProjectsByAssignedProjectIDDataSet.FindDesignProjectsByAssignedProjectID[0].TransactionID;
            gstrProjectNotes = TheFindDesignProjectsByAssignedProjectIDDataSet.FindDesignProjectsByAssignedProjectID[0].ProjectNotes;
            
            txtDateReceived.Text = Convert.ToString(TheFindDesignProjectsByAssignedProjectIDDataSet.FindDesignProjectsByAssignedProjectID[0].DateReceived);
            txtCoordinator.Text = TheFindDesignProjectsByAssignedProjectIDDataSet.FindDesignProjectsByAssignedProjectID[0].Coordinator;
            txtCity.Text = TheFindDesignProjectsByAssignedProjectIDDataSet.FindDesignProjectsByAssignedProjectID[0].City;
            txtProjectName.Text = TheFindDesignProjectsByAssignedProjectIDDataSet.FindDesignProjectsByAssignedProjectID[0].ProjectName;
            txtJobType.Text = TheFindDesignProjectsByAssignedProjectIDDataSet.FindDesignProjectsByAssignedProjectID[0].JobType;
            txtStatus.Text = TheFindDesignProjectsByAssignedProjectIDDataSet.FindDesignProjectsByAssignedProjectID[0].JobStatus;

            SetControlsEnabled(true);
        }

        private void TxtEnterLastName_TextChanged(object sender, TextChangedEventArgs e)
        {
            string strLastName;
            int intLength;
            int intNumberOfRecords;
            int intCounter;

            strLastName = txtEnterLastName.Text;
            intLength = strLastName.Length;
            if(intLength > 2)
            {
                TheComboEmployeeDataSet = TheEmployeeClass.FillEmployeeComboBox(strLastName);
                cboSelectSurveyor.Items.Clear();
                cboSelectSurveyor.Items.Add("Select Surveyor");

                intNumberOfRecords = TheComboEmployeeDataSet.employees.Rows.Count - 1;

                if(intNumberOfRecords < 0)
                {
                    TheMessagesClass.ErrorMessage("Employee Not Found");
                    return;
                }

                for (intCounter = 0; intCounter <= intNumberOfRecords; intCounter++)
                {
                    cboSelectSurveyor.Items.Add(TheComboEmployeeDataSet.employees[intCounter].FullName);
                }

                cboSelectSurveyor.SelectedIndex = 0;

            }
        }

        private void CboSelectSurveyor_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int intSelectedIndex;

            intSelectedIndex = cboSelectSurveyor.SelectedIndex - 1;

            if(intSelectedIndex > -1)
            {
                MainWindow.gintEmployeeID = TheComboEmployeeDataSet.employees[intSelectedIndex].EmployeeID;

                gstrFullName = TheComboEmployeeDataSet.employees[intSelectedIndex].FullName;
            }
        }

        private void BtnSendEmail_Click(object sender, RoutedEventArgs e)
        {
            //this will send an email to the surveyor
            bool blnFatalError = false;
            string strEmailAddress;
            string strHeader;
            string strMessage;
            int intRecordsReturned;
            string strErrorMessage = "";
            string strFirstName;
            string strLastName;

            try
            {
                //data validation
                strMessage = txtSurveyorNotes.Text;
                if(strMessage == "")
                {
                    blnFatalError = true;
                    strErrorMessage += "Surveyor Notes Is Missing\n";
                }
                if(cboSelectSurveyor.SelectedIndex < 1)
                {
                    blnFatalError = true;
                    strErrorMessage += "Employee Was Not Selected\n";
                }
                if(blnFatalError == true)
                {
                    TheMessagesClass.ErrorMessage(strErrorMessage);
                    return;
                }

                TheFindEmployeeEmailAddressDataSet = TheEmployeeClass.FindEmployeeEmailAddress(MainWindow.gintEmployeeID);

                intRecordsReturned = TheFindEmployeeEmailAddressDataSet.FindEmployeeEmailAddress.Rows.Count;

                if(intRecordsReturned < 1)
                {
                    TheMessagesClass.ErrorMessage("Employee Does Not Have A Valid Email Address");
                    return;
                }
                else
                {
                    blnFatalError = TheDataValidationClass.VerifyEmailAddress(TheFindEmployeeEmailAddressDataSet.FindEmployeeEmailAddress[0].EmailAddress);

                    if(blnFatalError == true)
                    {
                        TheMessagesClass.ErrorMessage("Employee Does Not Have A Valid Email Address");
                        return;
                    }
                }

                strHeader = "A New Survey Has Been Assigned For Project " + MainWindow.gstrAssignedProjectID + " To You - Do Not Reply";
                strEmailAddress = TheFindEmployeeEmailAddressDataSet.FindEmployeeEmailAddress[0].EmailAddress;
                strFirstName = MainWindow.TheVerifyDesignEmployeeLogonDataSet.VerifyDesigEmployeeLogon[0].FirstName;
                strLastName = MainWindow.TheVerifyDesignEmployeeLogonDataSet.VerifyDesigEmployeeLogon[0].LastName;

                strMessage =  "<h1> " + strFirstName + " " + strLastName + " Has Assigned Project ID " + MainWindow.gstrAssignedProjectID + " To You</h1>";
                strMessage += "<p>This project notes include " + txtSurveyorNotes.Text + " More information regarding the project will be sent to you.</p>";

                blnFatalError = !TheSendEmailClass.SendEmail(strEmailAddress, strHeader, strMessage);

                if (blnFatalError == true)
                    throw new Exception();

                TheMessagesClass.InformationMessage("Email Message Sent");
            }
            catch (Exception Ex)
            {
                TheEVentLogClass.InsertEventLogEntry(DateTime.Now, Ex.Message);

                TheMessagesClass.ErrorMessage(Ex.ToString());
            }
        }

        private void BtnSubmit_Click(object sender, RoutedEventArgs e)
        {
            string strSurveryorNotes;
            bool blnFatalError = false;
            bool blnThereIsAProblem = false;
            string strErrorMessage = "";
            DateTime datAssignedDate;
            string strWOVStatus;
            string strProjectStatus = "WOV ASSIGNED TO " + gstrFullName;
            string strReWalk;

            try
            {
                //data validation
                strSurveryorNotes = txtSurveyorNotes.Text;
                if (strSurveryorNotes == "")
                {
                    blnFatalError = true;
                    strErrorMessage += "Surveyor Notes Is Missing\n";
                }
                if (cboSelectSurveyor.SelectedIndex < 1)
                {
                    blnFatalError = true;
                    strErrorMessage += "Employee Was Not Selected\n";
                }
                strReWalk = txtReWalk.Text;
                blnThereIsAProblem = TheDataValidationClass.VerifyYesNoData(strReWalk);
                if(blnThereIsAProblem == true)
                {
                    blnFatalError = true;
                    strErrorMessage += "ReWalk is a Yes/No Question\n";
                }
                datAssignedDate = Convert.ToDateTime(txtDateAssigned.Text);
                strWOVStatus = txtWOVStatus.Text;
                if (blnFatalError == true)
                {
                    TheMessagesClass.ErrorMessage(strErrorMessage);
                    return;
                }

                blnFatalError = TheDesignProjectsSurveyorClass.InsertProjectSurveyor(MainWindow.gintProjectID, MainWindow.gintEmployeeID, MainWindow.TheVerifyDesignEmployeeLogonDataSet.VerifyDesigEmployeeLogon[0].EmployeeID, datAssignedDate, strSurveryorNotes, strWOVStatus);

                if (blnFatalError == true)
                    throw new Exception();

                blnFatalError = TheDesignProjectsClass.UpdateDesignProjectJobStatus(MainWindow.gintTransactionID, strProjectStatus);

                if (blnFatalError == true)
                    throw new Exception();

                blnFatalError = TheDesignProjectUpdateClass.InsertIntoDesigProjectUpdates(MainWindow.gintProjectID, MainWindow.TheVerifyDesignEmployeeLogonDataSet.VerifyDesigEmployeeLogon[0].EmployeeID, strProjectStatus);

                if (blnFatalError == true)
                    throw new Exception();

                TheMessagesClass.InformationMessage("The Project Has Been Assigned to a Surveyor");

                ResetControls();
            }
            catch (Exception Ex)
            {
                TheEVentLogClass.InsertEventLogEntry(DateTime.Now, "Blue Jay Design // Assign Surveyor // Submit Button " + Ex.Message);

                TheMessagesClass.ErrorMessage(Ex.ToString());
            }

        }

        private void BtnViewProjectInfo_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.ViewProjectInfoWindow.Visibility = Visibility.Visible;
            Visibility = Visibility.Hidden;
        }

        private void BtnProjectLocations_Click(object sender, RoutedEventArgs e)
        {
            ProjectLocations ProjectLocations = new ProjectLocations();
            ProjectLocations.Show();
        }
    }
}
