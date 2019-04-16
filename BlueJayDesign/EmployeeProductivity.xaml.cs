/* Title:           Design Productivity 
 * Date:            2-19-19
 * Author:          Terry Holmes
 * 
 * Description:     This is used to enter productivity */

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
using DesignProductivityDLL;
using DataValidationDLL;

namespace BlueJayDesign
{
    /// <summary>
    /// Interaction logic for EmployeeProductivity.xaml
    /// </summary>
    public partial class EmployeeProductivity : Window
    {
        //setting up the classes
        WPFMessagesClass TheMessagesClass = new WPFMessagesClass();
        EventLogClass TheEventLogClass = new EventLogClass();
        DesignProjectsClass TheDesignProjectsClass = new DesignProjectsClass();
        DesignProductivityClass TheDesignProductivityClass = new DesignProductivityClass();
        DataValidationClass TheDataValidationClass = new DataValidationClass();

        //setting up the data
        FindDesignProjectsByAssignedProjectIDDataSet TheFindDesignProjectsByAssignedProjectIDDataSet = new FindDesignProjectsByAssignedProjectIDDataSet();

        //setting global variables
        DateTime gdatStartDate;
        DateTime gdatEndDate;

        public EmployeeProductivity()
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
            dpkStartDate.Text = "";
            dpkEndDate.Text = "";
            txtEndTime.Text = "";
            txtStartTime.Text = "";
            txtProjectID.Text = "";
            txtProjectNotes.Text = "";
        }

        private void Calendar_DisplayDateChanged(object sender, CalendarDateChangedEventArgs e)
        {

        }

        private void DpkStartDate_CalendarClosed(object sender, RoutedEventArgs e)
        {
            string strDatePicked;

            strDatePicked = dpkStartDate.Text;

            gdatStartDate = Convert.ToDateTime(strDatePicked);
        }

        private void DpkEndDate_CalendarClosed(object sender, RoutedEventArgs e)
        {
            string strDatePicked;

            strDatePicked = dpkStartDate.Text;

            gdatEndDate = Convert.ToDateTime(strDatePicked);
        }

        private void BtnProcess_Click(object sender, RoutedEventArgs e)
        {
            bool blnFatalError = false;
            double douHours;
            double douMinutes;
            string strValueForValidation;
            string strErrorMessage = "";
            bool blnThereIsAProblem = false;
            string strAssignedProjectID;
            int intProjectID = 0;
            string strEmployeeNotes;
            DateTime datTempTime;
            int intRecordsReturned;

            try
            {
                strValueForValidation = dpkStartDate.Text;
                blnThereIsAProblem = TheDataValidationClass.VerifyDateData(strValueForValidation);
                if(blnThereIsAProblem == true)
                {
                    blnFatalError = true;
                    strErrorMessage += "The Start Date is not a Date\n";
                }
                else
                {
                    gdatStartDate = Convert.ToDateTime(strValueForValidation);
                }
                strValueForValidation = dpkEndDate.Text;
                blnThereIsAProblem = TheDataValidationClass.VerifyDateData(strValueForValidation);
                if (blnThereIsAProblem == true)
                {
                    blnFatalError = true;
                    strErrorMessage += "The End Date is not a Date\n";
                }
                else
                {
                    gdatEndDate = Convert.ToDateTime(strValueForValidation);
                }
                strValueForValidation = txtStartTime.Text;
                blnThereIsAProblem = TheDataValidationClass.VerifyTime(strValueForValidation);
                if(blnThereIsAProblem == true)
                {
                    blnFatalError = true;
                    strErrorMessage += "Start Time is not a Time\n";
                }
                else
                {
                    datTempTime = Convert.ToDateTime(strValueForValidation);
                    douHours = datTempTime.Hour;
                    douMinutes = datTempTime.Minute;

                    gdatStartDate = gdatStartDate.AddHours(douHours);
                    gdatStartDate = gdatStartDate.AddMinutes(douMinutes);
                }
                strValueForValidation = txtEndTime.Text;
                blnThereIsAProblem = TheDataValidationClass.VerifyTime(strValueForValidation);
                if (blnThereIsAProblem == true)
                {
                    blnFatalError = true;
                    strErrorMessage += "Start Time is not a Time\n";
                }
                else
                {
                    datTempTime = Convert.ToDateTime(strValueForValidation);
                    douHours = datTempTime.Hour;
                    douMinutes = datTempTime.Minute;

                    gdatEndDate = gdatEndDate.AddHours(douHours);
                    gdatEndDate = gdatEndDate.AddMinutes(douMinutes);
                }
                strAssignedProjectID = txtProjectID.Text;
                if(strAssignedProjectID == "")
                {
                    blnFatalError = true;
                    strErrorMessage += "Project ID Not Found\n";
                }
                else
                {
                    TheFindDesignProjectsByAssignedProjectIDDataSet = TheDesignProjectsClass.FindDesignProjectsByAssignedProjectID(strAssignedProjectID);

                    intRecordsReturned = TheFindDesignProjectsByAssignedProjectIDDataSet.FindDesignProjectsByAssignedProjectID.Rows.Count;

                    if(intRecordsReturned < 1)
                    {
                        blnFatalError = true;
                        strErrorMessage += "The Project Was Not Found\n";
                    }
                    else
                    {
                        intProjectID = TheFindDesignProjectsByAssignedProjectIDDataSet.FindDesignProjectsByAssignedProjectID[0].ProjectID;
                    }
                }
                strEmployeeNotes = txtProjectNotes.Text;
                if(strEmployeeNotes == "")
                {
                    blnFatalError = true;
                    strErrorMessage += "Employee Notes Were Not Entered\n";
                }
                if(blnFatalError == true)
                {
                    TheMessagesClass.ErrorMessage(strErrorMessage);
                    return;
                }

                blnFatalError = TheDesignProductivityClass.InsertDesignProductivity(intProjectID, MainWindow.TheVerifyDesignEmployeeLogonDataSet.VerifyDesigEmployeeLogon[0].EmployeeID, gdatStartDate, gdatEndDate, strEmployeeNotes);

                if (blnFatalError == true)
                    throw new Exception();

                TheMessagesClass.ErrorMessage("Productivity Has Been Entered");

                ResetControls();

            }
            catch (Exception Ex)
            {
                TheEventLogClass.InsertEventLogEntry(DateTime.Now, "Blue Jay Design // Employee Productivity // Process Button " + Ex.Message);

                TheMessagesClass.ErrorMessage(Ex.ToString());
            }
        }
        private void BtnMyProductivity_Click(object sender, RoutedEventArgs e)
        {
            MyProductivity MyProductivity = new MyProductivity();
            MyProductivity.ShowDialog();
        }
    }
}
