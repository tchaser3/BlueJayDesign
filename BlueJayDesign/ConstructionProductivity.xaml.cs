/* Title:           Construction Productivity
 * Date:            8-23-19
 * Author:          Terry Holmes
 * 
 * Description:     This is used to have the techs enter */

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
using DataValidationDLL;
using ProjectsDLL;
using WorkTaskDLL;
using EmployeeCrewAssignmentDLL;
using ProjectTaskDLL;
using EmployeeProjectAssignmentDLL;
using DateSearchDLL;

namespace BlueJayDesign
{
    /// <summary>
    /// Interaction logic for ConstructionProductivity.xaml
    /// </summary>
    public partial class ConstructionProductivity : Window
    {
        WPFMessagesClass TheMessagesClass = new WPFMessagesClass();
        EventLogClass TheEventLogClass = new EventLogClass();
        DataValidationClass TheDataValidationClass = new DataValidationClass();
        ProjectClass TheProjectClass = new ProjectClass();
        WorkTaskClass TheWorkTaskClass = new WorkTaskClass();
        EmployeeCrewAssignmentClass TheEmployeeCrewAssignmentClass = new EmployeeCrewAssignmentClass();
        ProjectTaskClass TheProjectTaskClass = new ProjectTaskClass();
        EmployeeProjectAssignmentClass TheEmployeeProjectAssignmentClass = new EmployeeProjectAssignmentClass();
        DateSearchClass TheDateSearchClass = new DateSearchClass();

        FindProjectByAssignedProjectIDDataSet TheFindProjectByAssignedProjectIDDataSet = new FindProjectByAssignedProjectIDDataSet();
        FindWorkTaskByTaskKeywordDataSet TheFindWorkTaskByTaskKeyWordDataSet = new FindWorkTaskByTaskKeywordDataSet();

        DateTime gdatTransactionDate;
        bool gblnHoursSet;

        public ConstructionProductivity()
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
            MainWindow.EmployeeProductivityWindow.Visibility = Visibility.Hidden;
            this.Close();
        }

        private void BtnAddNewProject_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.AddNewProjectWindow.Visibility = Visibility.Visible;
            MainWindow.EmployeeProductivityWindow.Visibility = Visibility.Hidden;
            this.Close();
        }


        private void BtnMyTasks_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.UpdateAssignTasksWindow.Visibility = Visibility.Visible;
            MainWindow.EmployeeProductivityWindow.Visibility = Visibility.Hidden;
            this.Close();
        }

        private void BtnAssignTask_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.AssignTasksWindow.Visibility = Visibility.Visible;
            MainWindow.EmployeeProductivityWindow.Visibility = Visibility.Hidden;
            this.Close();
        }

        private void BtnAssignSurveyor_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.AssignSurveyorWindow.Visibility = Visibility.Visible;
            MainWindow.EmployeeProductivityWindow.Visibility = Visibility.Hidden;
            this.Close();
        }

        private void BtnUpdateSurvey_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.UpdateSurveyWindow.Visibility = Visibility.Visible;
            MainWindow.EmployeeProductivityWindow.Visibility = Visibility.Hidden;
            this.Close();
        }

        private void BtnUpdateProject_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.UpdateProjectWindow.Visibility = Visibility.Visible;
            MainWindow.EmployeeProductivityWindow.Visibility = Visibility.Hidden;
            this.Close();
        }

        private void BtnCloseProject_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.CloseProjectWindow.Visibility = Visibility.Visible;
            MainWindow.EmployeeProductivityWindow.Visibility = Visibility.Hidden;
            this.Close();
        }
        private void BtnViewProjectInfo_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.ViewProjectInfoWindow.Visibility = Visibility.Visible;
            MainWindow.EmployeeProductivityWindow.Visibility = Visibility.Hidden;
            this.Close();
        }
        private void BtnEmployeeProductivity_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.EmployeeProductivityWindow.Visibility = Visibility.Visible;
            MainWindow.EmployeeProductivityWindow.Visibility = Visibility.Hidden;
            this.Close();
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void BtnHome_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.EmployeeProductivityWindow.Visibility = Visibility.Hidden;
            this.Close();
        }
        private void BtnOpenDesignProjects_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.OpenDesignProjectWindow.Visibility = Visibility.Visible;
            MainWindow.EmployeeProductivityWindow.Visibility = Visibility.Hidden;
            this.Close();
        }

        private void BtnDesignProjectReport_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.DesignProjectReportwindow.Visibility = Visibility.Visible;
            MainWindow.EmployeeProductivityWindow.Visibility = Visibility.Hidden;
            this.Close();
        }

        private void BtnPolePermitReport_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.PolePermitReportWindow.Visibility = Visibility.Visible;
            MainWindow.EmployeeProductivityWindow.Visibility = Visibility.Hidden;
            this.Close();
        }

        private void BtnOpenSurveryorReport_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.OpenSurveyorReportWindow.Visibility = Visibility.Visible;
            MainWindow.EmployeeProductivityWindow.Visibility = Visibility.Hidden;
            this.Close();
        }
        private decimal CalculateTimeSpan(string strStartTime, string strEndTime)
        {
            decimal decTotalHours = 0;
            TimeSpan tspStartTime;
            TimeSpan tspEndTime;
            TimeSpan tspTotalTime;
            decimal decHours;
            decimal decMinutes;
            int intMinutes;

            try
            {
                tspStartTime = TimeSpan.Parse(strStartTime);

                tspEndTime = TimeSpan.Parse(strEndTime);

                tspTotalTime = tspEndTime - tspStartTime;

                decHours = Convert.ToDecimal(tspTotalTime.Hours);
                intMinutes = tspTotalTime.Minutes;
                decMinutes = Convert.ToDecimal(intMinutes) / 60;

                decTotalHours = decHours + decMinutes;

            }
            catch (Exception Ex)
            {
                TheEventLogClass.InsertEventLogEntry(DateTime.Now, "Blue Jay Design // Enter Design WOV Tech Pay // Calculate Time Span " + Ex.Message);

                TheMessagesClass.ErrorMessage(Ex.ToString());
            }

            return decTotalHours;
        }

        private void TxtWorkTask_TextChanged(object sender, TextChangedEventArgs e)
        {
            //Setting up local variables
            int intRecordsReturned;
            string strWorkTask;
            int intLength;
            int intNumberOfRecords;
            int intCounter;

            try
            {
                MainWindow.gstrAssignedProjectID = txtProjectID.Text;
                TheFindProjectByAssignedProjectIDDataSet = TheProjectClass.FindProjectByAssignedProjectID(MainWindow.gstrAssignedProjectID);

                intRecordsReturned = TheFindProjectByAssignedProjectIDDataSet.FindProjectByAssignedProjectID.Rows.Count;

                if(intRecordsReturned < 1)
                {
                    TheMessagesClass.ErrorMessage("Project Was Not Found");
                    return;
                }
                else
                {
                    MainWindow.gintProjectID = TheFindProjectByAssignedProjectIDDataSet.FindProjectByAssignedProjectID[0].ProjectID;
                }

                strWorkTask = txtWorkTask.Text;
                intLength = strWorkTask.Length;

                if(intLength > 2)
                {
                    TheFindWorkTaskByTaskKeyWordDataSet = TheWorkTaskClass.FindWorkTaskByTaskKeyword(strWorkTask);

                    cboSelectWorkTask.Items.Clear();
                    cboSelectWorkTask.Items.Add("Select Work Task");

                    intNumberOfRecords = TheFindWorkTaskByTaskKeyWordDataSet.FindWorkTaskByTaskKeyword.Rows.Count - 1;

                    if(intNumberOfRecords < 0)
                    {
                        TheMessagesClass.ErrorMessage("The Work Task Entered does not Exist");
                        return;
                    }

                    for(intCounter = 0; intCounter <= intNumberOfRecords; intCounter++)
                    {
                        cboSelectWorkTask.Items.Add(TheFindWorkTaskByTaskKeyWordDataSet.FindWorkTaskByTaskKeyword[intCounter].WorkTask);
                    }

                    cboSelectWorkTask.SelectedIndex = 0;
                }
            }
            catch (Exception Ex)
            {
                TheEventLogClass.InsertEventLogEntry(DateTime.Now, "Blue Jay Design // Construction Productivity // Work Task Text Change Event " + Ex.Message);

                TheMessagesClass.ErrorMessage(Ex.ToString());
            }
        }

        private void TxtDate_TextChanged(object sender, TextChangedEventArgs e)
        {
            gblnHoursSet = false;
        }

        private void TxtProjectID_TextChanged(object sender, TextChangedEventArgs e)
        {
            gblnHoursSet = false;
        }

        private void TxtStartTime_TextChanged(object sender, TextChangedEventArgs e)
        {
            gblnHoursSet = false;
        }

        private void TxtEndTime_TextChanged(object sender, TextChangedEventArgs e)
        {
            gblnHoursSet = false;
        }

        private void CboSelectWorkTask_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int intSelectedIndex;

            intSelectedIndex = cboSelectWorkTask.SelectedIndex - 1;

            if(intSelectedIndex > -1)
            {
                MainWindow.gintWorkTaskID = TheFindWorkTaskByTaskKeyWordDataSet.FindWorkTaskByTaskKeyword[intSelectedIndex].WorkTaskID;
            }
        }

        private void BtnProcess_Click(object sender, RoutedEventArgs e)
        {
            //setting local variables
            string strValueForValidation;
            string strErrorMessage = "";
            bool blnThereIsAProblem = false;
            bool blnFatalError = false;
            DateTime datTransactionDate = DateTime.Now;
            int intQuantity = 0;
            string strStartTime;
            string strEndTime;
            decimal decTotalHours;
            int intEmployeeID;
            string strLastName;

            try
            {
                strValueForValidation = txtDate.Text;
                blnThereIsAProblem = TheDataValidationClass.VerifyDateData(strValueForValidation);
                if (blnThereIsAProblem == true)
                {
                    blnFatalError = true;
                    strErrorMessage += "The Date Entered is not a Date\n";
                }
                else
                {
                    datTransactionDate = Convert.ToDateTime(strValueForValidation);
                }
                strStartTime = txtStartTime.Text;
                blnThereIsAProblem = TheDataValidationClass.VerifyTime(strStartTime);
                if (blnThereIsAProblem == true)
                {
                    blnFatalError = true;
                    strErrorMessage += "The Start Time is not a Time\n";
                }
                strEndTime = txtEndTime.Text;
                blnThereIsAProblem = TheDataValidationClass.VerifyTime(strStartTime);
                if (blnThereIsAProblem == true)
                {
                    blnFatalError = true;
                    strErrorMessage += "The End Time is not a Time\n";
                }
                if (cboSelectWorkTask.SelectedIndex < 1)
                {
                    blnFatalError = true;
                    strErrorMessage += "The Work Task was not Selected\n";
                }
                strValueForValidation = txtQuantity.Text;
                blnThereIsAProblem = TheDataValidationClass.VerifyIntegerData(strValueForValidation);
                if (blnThereIsAProblem == true)
                {
                    blnFatalError = true;
                    strErrorMessage += "The Quantity Entered is not an Integer\n";
                }
                else
                {
                    intQuantity = Convert.ToInt32(strValueForValidation);
                }
                
                if (blnFatalError == true)
                {
                    TheMessagesClass.ErrorMessage(strErrorMessage);
                    return;
                }

                decTotalHours = CalculateTimeSpan(strStartTime, strEndTime);

                decTotalHours = Math.Round(decTotalHours, 3);

                if (gblnHoursSet == true)
                {
                    decTotalHours = 0;
                }

                if (decTotalHours < 0)
                {
                    TheMessagesClass.ErrorMessage("You Cannot Have Negative Hours");
                    return;
                }

                intEmployeeID = MainWindow.TheVerifyDesignEmployeeLogonDataSet.VerifyDesigEmployeeLogon[0].EmployeeID;
                strLastName = MainWindow.TheVerifyDesignEmployeeLogonDataSet.VerifyDesigEmployeeLogon[0].LastName;

                blnFatalError = TheEmployeeCrewAssignmentClass.InsertEmployeeCrewAssignment(strLastName, intEmployeeID, MainWindow.gintProjectID, datTransactionDate);

                if (blnFatalError == true)
                    throw new Exception();

                blnFatalError = TheEmployeeProjectAssignmentClass.InsertEmployeeProjectAssignment(intEmployeeID, MainWindow.gintProjectID, MainWindow.gintWorkTaskID, datTransactionDate, decTotalHours);


                if (blnFatalError == true)
                    throw new Exception();

                blnFatalError = TheProjectTaskClass.InsertProjectTask(MainWindow.gintProjectID, intEmployeeID, MainWindow.gintWorkTaskID, Convert.ToDecimal(intQuantity), datTransactionDate);

                if (blnFatalError == true)
                    throw new Exception();

                gblnHoursSet = true;
                txtQuantity.Text = "";
                txtWorkTask.Text = "";
                cboSelectWorkTask.Items.Clear();

                TheMessagesClass.InformationMessage("The Productivity Has Been Entered");

            }
            catch (Exception Ex)
            {
                TheEventLogClass.InsertEventLogEntry(DateTime.Now, "Blue Jay Design // Employee Productivity // Process Button " + Ex.Message);

                TheMessagesClass.ErrorMessage(Ex.ToString());
            }

        }
    }
}
