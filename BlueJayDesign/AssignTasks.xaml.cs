/* Title:           Assign Tasks
 * Date:            2-1-19
 * Author:          Terry Holmes
 * 
 * Description:     This is used to assign tasks */

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
using AssignedTasksDLL;

namespace BlueJayDesign
{
    /// <summary>
    /// Interaction logic for AssignTasks.xaml
    /// </summary>
    public partial class AssignTasks : Window
    {
        WPFMessagesClass TheMessagesClass = new WPFMessagesClass();
        EventLogClass TheEventLogClass = new EventLogClass();
        EmployeeClass TheEmployeeClass = new EmployeeClass();
        AssignedTaskClass TheAssignedTaskClass = new AssignedTaskClass();
        SendEmailClass TheSendEmailClass = new SendEmailClass();

        FindAssignedTasksByDateMatchDataSet TheFindAssignedTasksByDateMatchDataSet = new FindAssignedTasksByDateMatchDataSet();

        bool gblnItemSelected;

        public AssignTasks()
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
            int intCounter;
            int intNumberOfRecords;

            txtMessage.Text = "";
            txtTaskSubject.Text = "";

            MainWindow.TheFindDesignEmployeesDataSet = TheEmployeeClass.FindDesignEmployees();

            intNumberOfRecords = MainWindow.TheFindDesignEmployeesDataSet.FindDesignEmployees.Rows.Count - 1;

            if(intNumberOfRecords < -1)
            {
                TheMessagesClass.ErrorMessage("There are no Employees in the Design Department");
                return;
            }

            cboSelectEmployee.Items.Clear();
            cboSelectEmployee.Items.Add("Select Employee");

            for(intCounter = 0; intCounter <= intNumberOfRecords; intCounter++)
            {
                cboSelectEmployee.Items.Add(MainWindow.TheFindDesignEmployeesDataSet.FindDesignEmployees[intCounter].FullName);
            }

            MainWindow.TheEmployeeEmailDataSet.employeeemail.Rows.Clear();
            gblnItemSelected = false;
            cboSelectEmployee.SelectedIndex = 0;
        }

        private void CboSelectEmployee_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int intSelectedIndex;
            int intEmployeeID;
            bool blnItemFound;

            try
            {
                intSelectedIndex = cboSelectEmployee.SelectedIndex - 1;

                if(intSelectedIndex > -1)
                {
                    intEmployeeID = MainWindow.TheFindDesignEmployeesDataSet.FindDesignEmployees[intSelectedIndex].EmployeeID;

                    blnItemFound = CheckForEmailAddress(intEmployeeID);

                    if(blnItemFound == false)
                    {
                        EmployeeEmailDataSet.employeeemailRow NewEmailRow = MainWindow.TheEmployeeEmailDataSet.employeeemail.NewemployeeemailRow();

                        NewEmailRow.EmployeeID = MainWindow.TheFindDesignEmployeesDataSet.FindDesignEmployees[intSelectedIndex].EmployeeID;
                        NewEmailRow.EmailAddress = MainWindow.TheFindDesignEmployeesDataSet.FindDesignEmployees[intSelectedIndex].EmailAddress;

                        MainWindow.TheEmployeeEmailDataSet.employeeemail.Rows.Add(NewEmailRow);

                        TheMessagesClass.InformationMessage("Employee Has Been Added");

                        gblnItemSelected = true;

                        cboSelectEmployee.SelectedIndex = 0;
                    }                    
                }
            }
            catch (Exception Ex)
            {
                TheEventLogClass.InsertEventLogEntry(DateTime.Now, "Blue Jay Design // Assign Tasks // Combo Box Selection " + Ex.Message);

                TheMessagesClass.ErrorMessage(Ex.ToString());
            }
        }
        private bool CheckForEmailAddress(int intEmployeeID)
        {
            bool blnItemFound = false;
            int intNumberOfRecords;
            int intCounter;

            intNumberOfRecords = MainWindow.TheEmployeeEmailDataSet.employeeemail.Rows.Count - 1;

            if(intNumberOfRecords > -1)
            {
                for(intCounter = 0; intCounter <= intNumberOfRecords; intCounter++)
                {
                    if(intEmployeeID == MainWindow.TheEmployeeEmailDataSet.employeeemail[intCounter].EmployeeID)
                    {
                        blnItemFound = true;
                        TheMessagesClass.InformationMessage("Employee Has Been Selected Already");
                        cboSelectEmployee.SelectedIndex = 0;
                    }
                }
            }

            return blnItemFound;
        }

        private void BtnViewProjectInfo_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.ViewProjectInfoWindow.Visibility = Visibility.Visible;
            this.Visibility = Visibility.Hidden;
        }

        private void BtnSend_Click(object sender, RoutedEventArgs e)
        {
            string strMessage;
            string strSubject;
            bool blnFatalError = false;
            string strErrorMessage = "";
            int intCounter;
            int intNumberOfRecords;
            string strEmailAddress;
            string strFinalMessage;
            int intEmployeeID;
            DateTime datTransactionDate = DateTime.Now;
            int intTaskID;

            try
            {
                //data validation;
                strMessage = txtMessage.Text;
                if (strMessage == "")
                {
                    blnFatalError = true;
                    strErrorMessage += "The Message Was Not Entered\n";
                }
                strSubject = txtTaskSubject.Text;
                if (strSubject == "")
                {
                    blnFatalError = true;
                    strErrorMessage += "The Subject Was Not Entered\n";
                }
                
                if (blnFatalError == true)
                {
                    TheMessagesClass.ErrorMessage(strErrorMessage);
                    return;
                }

                strSubject += " - DO NOT REPLY";
                strFinalMessage = "<h1>" + strSubject + "</h1>";
                strFinalMessage += "<h3>This is a Message Sent From " + MainWindow.TheVerifyDesignEmployeeLogonDataSet.VerifyDesigEmployeeLogon[0].FirstName + " " + MainWindow.TheVerifyDesignEmployeeLogonDataSet.VerifyDesigEmployeeLogon[0].LastName + "</h3>";
                strFinalMessage += "<p>" + strMessage + "</p>";

                intNumberOfRecords = MainWindow.TheEmployeeEmailDataSet.employeeemail.Rows.Count - 1;

                blnFatalError = TheAssignedTaskClass.InsertAssignedTask(MainWindow.TheVerifyDesignEmployeeLogonDataSet.VerifyDesigEmployeeLogon[0].EmployeeID, datTransactionDate, strSubject, strMessage);

                if (blnFatalError == true)
                    throw new Exception();




                TheFindAssignedTasksByDateMatchDataSet = TheAssignedTaskClass.FindAssignedTaskByDateMatch(datTransactionDate);

                intTaskID = TheFindAssignedTasksByDateMatchDataSet.FindAssignedTasksByDateMatch[0].TransactionID;

                for (intCounter = 0; intCounter <= intNumberOfRecords; intCounter++)
                {
                    strEmailAddress = MainWindow.TheEmployeeEmailDataSet.employeeemail[intCounter].EmailAddress;
                    intEmployeeID = MainWindow.TheEmployeeEmailDataSet.employeeemail[intCounter].EmployeeID;

                    blnFatalError = !TheSendEmailClass.SendEmail(strEmailAddress, strSubject, strFinalMessage);

                    if (blnFatalError == true)
                        throw new Exception();

                    blnFatalError = TheAssignedTaskClass.InsertAssignedEmployeeTasks(intEmployeeID, intTaskID);

                    if (blnFatalError == true)
                        throw new Exception();
                }

                TheMessagesClass.InformationMessage("Email Sent");
                ResetControls();
            }
            catch (Exception Ex)
            {
                TheEventLogClass.InsertEventLogEntry(DateTime.Now, "Blue Jay Design // Send Email // Send Button " + Ex.Message);

                TheMessagesClass.ErrorMessage(Ex.ToString());
            }


        }
    }
}
