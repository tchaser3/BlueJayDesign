/* Title:           Pole Permit Report
 * Date:            2-28-19
 * Author:          Terry Holmes
 * 
 * Description:     This is the permit report */

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
using DesignProjectsDLL;
using DesignPermitsDLL;
using DataValidationDLL;
using Microsoft.Win32;

namespace BlueJayDesign
{
    /// <summary>
    /// Interaction logic for PolePermitReport.xaml
    /// </summary>
    public partial class PolePermitReport : Window
    {
        //setting up the classes
        WPFMessagesClass TheMessagesClass = new WPFMessagesClass();
        EventLogClass TheEventLogClass = new EventLogClass();
        EmployeeClass TheEmployeeClass = new EmployeeClass();
        DesignProjectsClass TheDesignProjectsClass = new DesignProjectsClass();
        DesignPermitsClass TheDesignPermitsClass = new DesignPermitsClass();
        DataValidationClass TheDataValidationClass = new DataValidationClass();

        //setting up the data
        FindOpenPermitsDataSet TheFindOpenPermitsDataSet = new FindOpenPermitsDataSet();
        PermitReportDataSet ThePermitReportDataSet = new PermitReportDataSet();
        FindPermitsByDateRangeDataSet TheFindPermitsByDateRangeDataSet = new FindPermitsByDateRangeDataSet();
        FindPermitsByProjectIDDataSet TheFindPermitsByProjectIDDataSet = new FindPermitsByProjectIDDataSet();
        FindDesignProjectsByAssignedProjectIDDataSet TheFindDesignProjectsByAssignedProjectIDDataSet = new FindDesignProjectsByAssignedProjectIDDataSet();
        FindPermitsByEmployeeDataSet TheFindPermitByEmployeeDataSet = new FindPermitsByEmployeeDataSet();
        FindPermitsByLocationDataSet TheFindPermitsByLocationDataSet = new FindPermitsByLocationDataSet();

        //setting global variables
        string gstrReportType;

        public PolePermitReport()
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

        private void Window_Closed(object sender, EventArgs e)
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
            cboSelectEmployee.Items.Clear();
            cboSelectReportType.Items.Clear();
            cboSelectReportType.Items.Add("Select Report Type");
            cboSelectReportType.Items.Add("Find Open Permits");
            cboSelectReportType.Items.Add("Find Permits By Date Range");
            cboSelectReportType.Items.Add("Find Permits By Project");
            cboSelectReportType.Items.Add("Find Permits By Employee");
            cboSelectReportType.Items.Add("Find Permits By Location");
            cboSelectReportType.SelectedIndex = 0;            
            splEndDate.Visibility = Visibility.Hidden;
            splSelectEmployee.Visibility = Visibility.Hidden;
            splStartDate.Visibility = Visibility.Hidden;
            splEnterLastName.Visibility = Visibility.Hidden;
            splSearchButton.Visibility = Visibility.Hidden;
            ThePermitReportDataSet.permitreport.Rows.Clear();
            dgrResults.ItemsSource = ThePermitReportDataSet.permitreport;

            ClearControls();
        }
        private void ClearControls()
        {
            txtEndDate.Text = "";
            txtEnterLastName.Text = "";
            txtStartDate.Text = "";
        }

        private void CboSelectReportType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int intSelectedIndex;
            int intCounter;
            int intNumberOfRecords;

            intSelectedIndex = cboSelectReportType.SelectedIndex;

            if(intSelectedIndex > 0)
            {
                if(intSelectedIndex == 1)
                {
                    FindOpenPermits();
                }
                else if(intSelectedIndex == 2)
                {
                    gstrReportType = "DATE RANGE";
                    splEndDate.Visibility = Visibility.Visible;
                    splStartDate.Visibility = Visibility.Visible;
                    splSearchButton.Visibility = Visibility.Visible;
                    splSelectEmployee.Visibility = Visibility.Hidden;
                    splEnterLastName.Visibility = Visibility.Hidden;
                    ThePermitReportDataSet.permitreport.Rows.Clear();
                    dgrResults.ItemsSource = ThePermitReportDataSet.permitreport;
                }
                else if(intSelectedIndex == 3)
                {
                    gstrReportType = "PROJECT";
                    splEndDate.Visibility = Visibility.Hidden;
                    splStartDate.Visibility = Visibility.Hidden;
                    splSearchButton.Visibility = Visibility.Visible;
                    splSelectEmployee.Visibility = Visibility.Hidden;
                    splEnterLastName.Visibility = Visibility.Visible;
                    lblEnterLastName.Content = "Enter Project ID";
                    
                    ThePermitReportDataSet.permitreport.Rows.Clear();
                    dgrResults.ItemsSource = ThePermitReportDataSet.permitreport;
                }
                else if(intSelectedIndex == 4)
                {
                    gstrReportType = "EMPLOYEE";
                    splEndDate.Visibility = Visibility.Visible;
                    splStartDate.Visibility = Visibility.Visible;
                    splSearchButton.Visibility = Visibility.Visible;
                    splSelectEmployee.Visibility = Visibility.Visible;
                    splEnterLastName.Visibility = Visibility.Visible;
                    ThePermitReportDataSet.permitreport.Rows.Clear();
                    lblEnterLastName.Content = "Enter Last Name";
                    dgrResults.ItemsSource = ThePermitReportDataSet.permitreport;
                }
                else if (intSelectedIndex == 5)
                {
                    gstrReportType = "LOCATION";
                    splEndDate.Visibility = Visibility.Visible;
                    splStartDate.Visibility = Visibility.Visible;
                    splSearchButton.Visibility = Visibility.Visible;
                    splSelectEmployee.Visibility = Visibility.Visible;
                    splEnterLastName.Visibility = Visibility.Hidden;
                    ThePermitReportDataSet.permitreport.Rows.Clear();
                    lblSelectEmployee.Content = "Select Location";
                    dgrResults.ItemsSource = ThePermitReportDataSet.permitreport;

                    cboSelectEmployee.Items.Clear();
                    cboSelectEmployee.Items.Add("Select Location");

                    MainWindow.TheFindWarehousesDataSet = TheEmployeeClass.FindWarehouses();

                    intNumberOfRecords = MainWindow.TheFindWarehousesDataSet.FindWarehouses.Rows.Count - 1;

                    for(intCounter = 0; intCounter <= intNumberOfRecords; intCounter++)
                    {
                        cboSelectEmployee.Items.Add(MainWindow.TheFindWarehousesDataSet.FindWarehouses[intCounter].FirstName);
                    }

                    cboSelectEmployee.SelectedIndex = 0;
                }
            }
        }
        private void FindOpenPermits()
        {
            int intCounter;
            int intNumberOfRecords;
            string strEmployeeName;
            DateTime datTodaysDate = DateTime.Now;
            decimal decTotalHours;
            DateTime datReceivedDate;

            try
            {
                ClearControls();
                splEndDate.Visibility = Visibility.Hidden;
                splEnterLastName.Visibility = Visibility.Hidden;
                splSearchButton.Visibility = Visibility.Hidden;
                splSelectEmployee.Visibility = Visibility.Hidden;
                splStartDate.Visibility = Visibility.Hidden;
                ThePermitReportDataSet.permitreport.Rows.Clear();

                TheFindOpenPermitsDataSet = TheDesignPermitsClass.FindOpenPermits();

                intNumberOfRecords = TheFindOpenPermitsDataSet.FindOpenPermitsByDateRange.Rows.Count - 1;

                if (intNumberOfRecords > -1)
                {
                    for (intCounter = 0; intCounter <= intNumberOfRecords; intCounter++)
                    {
                        strEmployeeName = TheFindOpenPermitsDataSet.FindOpenPermitsByDateRange[intCounter].FirstName;
                        strEmployeeName += " ";
                        strEmployeeName += TheFindOpenPermitsDataSet.FindOpenPermitsByDateRange[intCounter].LastName;

                        datReceivedDate = TheFindOpenPermitsDataSet.FindOpenPermitsByDateRange[intCounter].DateReceived;

                        decTotalHours = Convert.ToDecimal((datTodaysDate - datReceivedDate).TotalHours);

                        decTotalHours = Math.Round(decTotalHours, 2);

                        PermitReportDataSet.permitreportRow NewPermitRow = ThePermitReportDataSet.permitreport.NewpermitreportRow();

                        NewPermitRow.DateReceived = datReceivedDate;
                        NewPermitRow.Employee = strEmployeeName;
                        NewPermitRow.PermitNotes = TheFindOpenPermitsDataSet.FindOpenPermitsByDateRange[intCounter].PermitNotes;
                        NewPermitRow.PermitStatus = TheFindOpenPermitsDataSet.FindOpenPermitsByDateRange[intCounter].PermitStatus;
                        NewPermitRow.ProjectID = TheFindOpenPermitsDataSet.FindOpenPermitsByDateRange[intCounter].AssignedProjectID;
                        NewPermitRow.ProjectName = TheFindOpenPermitsDataSet.FindOpenPermitsByDateRange[intCounter].ProjectName;
                        NewPermitRow.TimeOpened = decTotalHours;
                        NewPermitRow.TransactionID = TheFindOpenPermitsDataSet.FindOpenPermitsByDateRange[intCounter].TransactionID;

                        ThePermitReportDataSet.permitreport.Rows.Add(NewPermitRow);
                    }
                }

                dgrResults.ItemsSource = ThePermitReportDataSet.permitreport;
            }
            catch(Exception Ex)
            {
                TheEventLogClass.InsertEventLogEntry(DateTime.Now, "Blue Jay Design // Pole Permit Report // Find Open Permits " + Ex.Message);

                TheMessagesClass.ErrorMessage(Ex.ToString());
            }
        }

        private void BtnSearch_Click(object sender, RoutedEventArgs e)
        {
            if (gstrReportType == "DATE RANGE")
                PermitsDateSearch();
            else if (gstrReportType == "PROJECT")
                ProjectPermitReport();
            else if (gstrReportType == "EMPLOYEE")
                EmployeePermitReport();
            else if (gstrReportType == "LOCATION")
                LocationPermitReport();
            
        }
        private void LocationPermitReport()
        {
            //setting local variables
            string strErrorMessage = "";
            string strValueForValidation;
            bool blnFatalError = false;
            bool blnThereIsAProblem = false;
            int intCounter;
            int intNumberOfRecords;
            string strEmployeeName;
            DateTime datReceivedDate = DateTime.Now;
            decimal decTotalHours = 0;
            DateTime datTodaysDate = DateTime.Now;

            try
            {
                ThePermitReportDataSet.permitreport.Rows.Clear();

                strValueForValidation = txtStartDate.Text;
                blnThereIsAProblem = TheDataValidationClass.VerifyDateData(strValueForValidation);
                if (blnThereIsAProblem == true)
                {
                    blnFatalError = true;
                    strErrorMessage += "The Start Date is not a Start Date\n";
                }
                else
                {
                    MainWindow.gdatStartDate = Convert.ToDateTime(strValueForValidation);
                }
                strValueForValidation = txtEndDate.Text;
                blnThereIsAProblem = TheDataValidationClass.VerifyDateData(strValueForValidation);
                if (blnThereIsAProblem == true)
                {
                    blnFatalError = true;
                    strErrorMessage += "The End Date is not a Start Date\n";
                }
                else
                {
                    MainWindow.gdatEndDate = Convert.ToDateTime(strValueForValidation);
                }
                if (cboSelectEmployee.SelectedIndex < 1)
                {
                    blnFatalError = true;
                    strErrorMessage += "The Location Has Not Been Selected\n";
                }
                if (blnFatalError == true)
                {
                    TheMessagesClass.ErrorMessage(strErrorMessage);
                    return;
                }
                else
                {
                    blnFatalError = TheDataValidationClass.verifyDateRange(MainWindow.gdatStartDate, MainWindow.gdatEndDate);

                    if (blnFatalError == true)
                    {
                        TheMessagesClass.ErrorMessage("The Start Date is after the End Date");
                        return;
                    }
                }

                TheFindPermitsByLocationDataSet = TheDesignPermitsClass.FindPermitsByLocation(MainWindow.gintWarehouseID, MainWindow.gdatStartDate, MainWindow.gdatEndDate);

                intNumberOfRecords = TheFindPermitsByLocationDataSet.FindPermitsByLocation.Rows.Count - 1;

                if (intNumberOfRecords > -1)
                {
                    for (intCounter = 0; intCounter <= intNumberOfRecords; intCounter++)
                    {
                        strEmployeeName = TheFindPermitsByLocationDataSet.FindPermitsByLocation[intCounter].FirstName;
                        strEmployeeName += " ";
                        strEmployeeName += TheFindPermitsByLocationDataSet.FindPermitsByLocation[intCounter].LastName;

                        datReceivedDate = TheFindPermitsByLocationDataSet.FindPermitsByLocation[intCounter].DateReceived;

                        decTotalHours = Convert.ToDecimal((datTodaysDate - datReceivedDate).TotalHours);

                        decTotalHours = Math.Round(decTotalHours, 2);

                        PermitReportDataSet.permitreportRow NewPermitRow = ThePermitReportDataSet.permitreport.NewpermitreportRow();

                        NewPermitRow.DateReceived = datReceivedDate;
                        NewPermitRow.Employee = strEmployeeName;
                        NewPermitRow.PermitNotes = TheFindPermitsByLocationDataSet.FindPermitsByLocation[intCounter].PermitNotes;
                        NewPermitRow.PermitStatus = TheFindPermitsByLocationDataSet.FindPermitsByLocation[intCounter].PermitStatus;
                        NewPermitRow.ProjectID = TheFindPermitsByLocationDataSet.FindPermitsByLocation[intCounter].AssignedProjectID;
                        NewPermitRow.ProjectName = TheFindPermitsByLocationDataSet.FindPermitsByLocation[intCounter].ProjectName;
                        NewPermitRow.TimeOpened = decTotalHours;
                        NewPermitRow.TransactionID = TheFindPermitsByLocationDataSet.FindPermitsByLocation[intCounter].TransactionID;

                        ThePermitReportDataSet.permitreport.Rows.Add(NewPermitRow);
                    }
                }
            }
            catch (Exception Ex)
            {
                TheEventLogClass.InsertEventLogEntry(DateTime.Now, "Blue Jay Design // Location Permit Report // Permits Date Search " + Ex.Message);

                TheMessagesClass.ErrorMessage(Ex.ToString());
            }
        }
        private void EmployeePermitReport()
        {
            //setting local variables
            string strErrorMessage = "";
            string strValueForValidation;
            bool blnFatalError = false;
            bool blnThereIsAProblem = false;
            int intCounter;
            int intNumberOfRecords;
            string strEmployeeName;
            DateTime datReceivedDate = DateTime.Now;
            decimal decTotalHours = 0;
            DateTime datTodaysDate = DateTime.Now;

            try
            {
                ThePermitReportDataSet.permitreport.Rows.Clear();

                strValueForValidation = txtStartDate.Text;
                blnThereIsAProblem = TheDataValidationClass.VerifyDateData(strValueForValidation);
                if (blnThereIsAProblem == true)
                {
                    blnFatalError = true;
                    strErrorMessage += "The Start Date is not a Start Date\n";
                }
                else
                {
                    MainWindow.gdatStartDate = Convert.ToDateTime(strValueForValidation);
                }
                strValueForValidation = txtEndDate.Text;
                blnThereIsAProblem = TheDataValidationClass.VerifyDateData(strValueForValidation);
                if (blnThereIsAProblem == true)
                {
                    blnFatalError = true;
                    strErrorMessage += "The End Date is not a Start Date\n";
                }
                else
                {
                    MainWindow.gdatEndDate = Convert.ToDateTime(strValueForValidation);
                }
                if(cboSelectEmployee.SelectedIndex < 1)
                {
                    blnFatalError = true;
                    strErrorMessage += "The Employee Has Not Been Selected\n";
                }
                if (blnFatalError == true)
                {
                    TheMessagesClass.ErrorMessage(strErrorMessage);
                    return;
                }
                else
                {
                    blnFatalError = TheDataValidationClass.verifyDateRange(MainWindow.gdatStartDate, MainWindow.gdatEndDate);

                    if (blnFatalError == true)
                    {
                        TheMessagesClass.ErrorMessage("The Start Date is after the End Date");
                        return;
                    }
                }

                TheFindPermitByEmployeeDataSet = TheDesignPermitsClass.FindPermitsByEmployee(MainWindow.gintEmployeeID, MainWindow.gdatStartDate, MainWindow.gdatEndDate);

                intNumberOfRecords = TheFindPermitByEmployeeDataSet.FindPermitsByEmployee.Rows.Count - 1;

                if (intNumberOfRecords > -1)
                {
                    for (intCounter = 0; intCounter <= intNumberOfRecords; intCounter++)
                    {
                        strEmployeeName = TheFindPermitByEmployeeDataSet.FindPermitsByEmployee[intCounter].FirstName;
                        strEmployeeName += " ";
                        strEmployeeName += TheFindPermitByEmployeeDataSet.FindPermitsByEmployee[intCounter].LastName;

                        datReceivedDate = TheFindPermitByEmployeeDataSet.FindPermitsByEmployee[intCounter].DateReceived;

                        decTotalHours = Convert.ToDecimal((datTodaysDate - datReceivedDate).TotalHours);

                        decTotalHours = Math.Round(decTotalHours, 2);

                        PermitReportDataSet.permitreportRow NewPermitRow = ThePermitReportDataSet.permitreport.NewpermitreportRow();

                        NewPermitRow.DateReceived = datReceivedDate;
                        NewPermitRow.Employee = strEmployeeName;
                        NewPermitRow.PermitNotes = TheFindPermitByEmployeeDataSet.FindPermitsByEmployee[intCounter].PermitNotes;
                        NewPermitRow.PermitStatus = TheFindPermitByEmployeeDataSet.FindPermitsByEmployee[intCounter].PermitStatus;
                        NewPermitRow.ProjectID = TheFindPermitByEmployeeDataSet.FindPermitsByEmployee[intCounter].AssignedProjectID;
                        NewPermitRow.ProjectName = TheFindPermitByEmployeeDataSet.FindPermitsByEmployee[intCounter].ProjectName;
                        NewPermitRow.TimeOpened = decTotalHours;
                        NewPermitRow.TransactionID = TheFindPermitByEmployeeDataSet.FindPermitsByEmployee[intCounter].TransactionID;

                        ThePermitReportDataSet.permitreport.Rows.Add(NewPermitRow);
                    }
                }
            }
            catch (Exception Ex)
            {
                TheEventLogClass.InsertEventLogEntry(DateTime.Now, "Blue Jay Design // Employee Permit Report // Permits Date Search " + Ex.Message);

                TheMessagesClass.ErrorMessage(Ex.ToString());
            }
        }
        private void ProjectPermitReport()
        {
            //setting local variables
            int intCounter;
            int intNumberOfRecords;
            string strEmployeeName;
            DateTime datReceivedDate = DateTime.Now;
            decimal decTotalHours = 0;
            DateTime datTodaysDate = DateTime.Now;
            string strProjectID;
            int intRecordsReturned;
            int intProjectID;
            string strProjectName;

            try
            {
                ThePermitReportDataSet.permitreport.Rows.Clear();

                strProjectID = txtEnterLastName.Text;
                if (strProjectID == "")
                {
                    TheMessagesClass.ErrorMessage("The Project ID Was Not Entered");
                    return;
                }

                TheFindDesignProjectsByAssignedProjectIDDataSet = TheDesignProjectsClass.FindDesignProjectsByAssignedProjectID(strProjectID);

                intRecordsReturned = TheFindDesignProjectsByAssignedProjectIDDataSet.FindDesignProjectsByAssignedProjectID.Rows.Count;

                if (intRecordsReturned < 1)
                {
                    TheMessagesClass.ErrorMessage("The Project ID Was Not Found");
                    return;
                }

                intProjectID = TheFindDesignProjectsByAssignedProjectIDDataSet.FindDesignProjectsByAssignedProjectID[0].ProjectID;
                strProjectName = TheFindDesignProjectsByAssignedProjectIDDataSet.FindDesignProjectsByAssignedProjectID[0].ProjectName;

                TheFindPermitsByProjectIDDataSet = TheDesignPermitsClass.FindPermitsByProjectID(intProjectID);

                intNumberOfRecords = TheFindPermitsByProjectIDDataSet.FindPermitsByProjectID.Rows.Count - 1;

                if (intNumberOfRecords > -1)
                {
                    for (intCounter = 0; intCounter <= intNumberOfRecords; intCounter++)
                    {
                        strEmployeeName = TheFindPermitsByProjectIDDataSet.FindPermitsByProjectID[intCounter].FirstName;
                        strEmployeeName += " ";
                        strEmployeeName += TheFindPermitsByProjectIDDataSet.FindPermitsByProjectID[intCounter].LastName;

                        datReceivedDate = TheFindPermitsByProjectIDDataSet.FindPermitsByProjectID[intCounter].DateReceived;

                        decTotalHours = Convert.ToDecimal((datTodaysDate - datReceivedDate).TotalHours);

                        decTotalHours = Math.Round(decTotalHours, 2);

                        PermitReportDataSet.permitreportRow NewPermitRow = ThePermitReportDataSet.permitreport.NewpermitreportRow();

                        NewPermitRow.DateReceived = datReceivedDate;
                        NewPermitRow.Employee = strEmployeeName;
                        NewPermitRow.PermitNotes = TheFindPermitsByProjectIDDataSet.FindPermitsByProjectID[intCounter].PermitNotes;
                        NewPermitRow.PermitStatus = TheFindPermitsByProjectIDDataSet.FindPermitsByProjectID[intCounter].PermitStatus;
                        NewPermitRow.ProjectID = strProjectID;
                        NewPermitRow.ProjectName = strProjectName;
                        NewPermitRow.TimeOpened = decTotalHours;
                        NewPermitRow.TransactionID = TheFindPermitsByProjectIDDataSet.FindPermitsByProjectID[intCounter].TransactionID;

                        ThePermitReportDataSet.permitreport.Rows.Add(NewPermitRow);
                    }
                }
            }
            catch (Exception Ex)
            {
                TheEventLogClass.InsertEventLogEntry(DateTime.Now, "Blue Jay Design // Pole Permit Report // Project Permit Report " + Ex.Message);

                TheMessagesClass.ErrorMessage(Ex.ToString());
            }
        }
        private void PermitsDateSearch()
        {
            //setting local variables
            string strErrorMessage = "";
            string strValueForValidation;
            bool blnFatalError = false;
            bool blnThereIsAProblem = false;
            int intCounter;
            int intNumberOfRecords;
            string strEmployeeName;
            DateTime datReceivedDate = DateTime.Now;
            decimal decTotalHours = 0;
            DateTime datTodaysDate = DateTime.Now;
            
            try
            {
                ThePermitReportDataSet.permitreport.Rows.Clear();

                strValueForValidation = txtStartDate.Text;
                blnThereIsAProblem = TheDataValidationClass.VerifyDateData(strValueForValidation);
                if(blnThereIsAProblem == true)
                {
                    blnFatalError = true;
                    strErrorMessage += "The Start Date is not a Start Date\n";
                }
                else
                {
                    MainWindow.gdatStartDate = Convert.ToDateTime(strValueForValidation);
                }
                strValueForValidation = txtEndDate.Text;
                blnThereIsAProblem = TheDataValidationClass.VerifyDateData(strValueForValidation);
                if (blnThereIsAProblem == true)
                {
                    blnFatalError = true;
                    strErrorMessage += "The End Date is not a Start Date\n";
                }
                else
                {
                    MainWindow.gdatEndDate = Convert.ToDateTime(strValueForValidation);
                }
                if(blnFatalError == true)
                {
                    TheMessagesClass.ErrorMessage(strErrorMessage);
                    return;
                }
                else
                {
                    blnFatalError = TheDataValidationClass.verifyDateRange(MainWindow.gdatStartDate, MainWindow.gdatEndDate);

                    if(blnFatalError == true)
                    {
                        TheMessagesClass.ErrorMessage("The Start Date is after the End Date");
                        return;
                    }
                }

                TheFindPermitsByDateRangeDataSet = TheDesignPermitsClass.FindPermitsByDateRange(MainWindow.gdatStartDate, MainWindow.gdatEndDate);

                intNumberOfRecords = TheFindPermitsByDateRangeDataSet.FindPermitsByDateRange.Rows.Count - 1;

                if(intNumberOfRecords > -1)
                {
                    for(intCounter = 0; intCounter <= intNumberOfRecords; intCounter++)
                    {
                        strEmployeeName = TheFindPermitsByDateRangeDataSet.FindPermitsByDateRange[intCounter].FirstName;
                        strEmployeeName += " ";
                        strEmployeeName += TheFindPermitsByDateRangeDataSet.FindPermitsByDateRange[intCounter].LastName;

                        datReceivedDate = TheFindPermitsByDateRangeDataSet.FindPermitsByDateRange[intCounter].DateReceived;

                        decTotalHours = Convert.ToDecimal((datTodaysDate - datReceivedDate).TotalHours);

                        decTotalHours = Math.Round(decTotalHours, 2);

                        PermitReportDataSet.permitreportRow NewPermitRow = ThePermitReportDataSet.permitreport.NewpermitreportRow();

                        NewPermitRow.DateReceived = datReceivedDate;
                        NewPermitRow.Employee = strEmployeeName;
                        NewPermitRow.PermitNotes = TheFindPermitsByDateRangeDataSet.FindPermitsByDateRange[intCounter].PermitNotes;
                        NewPermitRow.PermitStatus = TheFindPermitsByDateRangeDataSet.FindPermitsByDateRange[intCounter].PermitStatus;
                        NewPermitRow.ProjectID = TheFindPermitsByDateRangeDataSet.FindPermitsByDateRange[intCounter].AssignedProjectID;
                        NewPermitRow.ProjectName = TheFindPermitsByDateRangeDataSet.FindPermitsByDateRange[intCounter].ProjectName;
                        NewPermitRow.TimeOpened = decTotalHours;
                        NewPermitRow.TransactionID = TheFindPermitsByDateRangeDataSet.FindPermitsByDateRange[intCounter].TransactionID;

                        ThePermitReportDataSet.permitreport.Rows.Add(NewPermitRow);
                    }
                }
            }
            catch (Exception Ex)
            {
                TheEventLogClass.InsertEventLogEntry(DateTime.Now, "Blue Jay Design // Pole Permit Report // Permits Date Search " + Ex.Message);

                TheMessagesClass.ErrorMessage(Ex.ToString());
            }
        }

        private void TxtEnterLastName_TextChanged(object sender, TextChangedEventArgs e)
        {
            string strLastName;
            int intLength;
            int intNumberOfRecords;
            int intCounter;

            if(gstrReportType == "EMPLOYEE")
            {
                cboSelectEmployee.Items.Clear();
                strLastName = txtEnterLastName.Text;
                intLength = strLastName.Length;

                if(intLength > 2)
                {
                    cboSelectEmployee.Items.Clear();
                    cboSelectEmployee.Items.Add("Select Employee");

                    MainWindow.TheComboEmployeeDataSet = TheEmployeeClass.FillEmployeeComboBox(strLastName);

                    intNumberOfRecords = MainWindow.TheComboEmployeeDataSet.employees.Rows.Count - 1;

                    if(intNumberOfRecords < 0)
                    {
                        TheMessagesClass.ErrorMessage("Employee Was Not Found");
                        return;
                    }

                    for(intCounter = 0; intCounter <= intNumberOfRecords; intCounter++)
                    {
                        cboSelectEmployee.Items.Add(MainWindow.TheComboEmployeeDataSet.employees[intCounter].FullName);
                    }

                    cboSelectEmployee.SelectedIndex = 0;
                }
            }
        }

        private void CboSelectEmployee_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int intSelectedIndex;

            intSelectedIndex = cboSelectEmployee.SelectedIndex - 1;

            if(intSelectedIndex > -1)
            {
                if(gstrReportType == "EMPLOYEE")
                {
                    MainWindow.gintEmployeeID = MainWindow.TheComboEmployeeDataSet.employees[intSelectedIndex].EmployeeID;
                }
                else if(gstrReportType == "LOCATION")
                {
                    MainWindow.gintWarehouseID = MainWindow.TheFindWarehousesDataSet.FindWarehouses[intSelectedIndex].EmployeeID;
                }
            }
        }

        private void BtnExportToExcel_Click(object sender, RoutedEventArgs e)
        {
            int intRowCounter;
            int intRowNumberOfRecords;
            int intColumnCounter;
            int intColumnNumberOfRecords;

            // Creating a Excel object. 
            Microsoft.Office.Interop.Excel._Application excel = new Microsoft.Office.Interop.Excel.Application();
            Microsoft.Office.Interop.Excel._Workbook workbook = excel.Workbooks.Add(Type.Missing);
            Microsoft.Office.Interop.Excel._Worksheet worksheet = null;

            try
            {

                worksheet = workbook.ActiveSheet;

                worksheet.Name = "OpenOrders";

                int cellRowIndex = 1;
                int cellColumnIndex = 1;
                intRowNumberOfRecords = ThePermitReportDataSet.permitreport.Rows.Count;
                intColumnNumberOfRecords = ThePermitReportDataSet.permitreport.Columns.Count;

                for (intColumnCounter = 0; intColumnCounter < intColumnNumberOfRecords; intColumnCounter++)
                {
                    worksheet.Cells[cellRowIndex, cellColumnIndex] = ThePermitReportDataSet.permitreport.Columns[intColumnCounter].ColumnName;

                    cellColumnIndex++;
                }

                cellRowIndex++;
                cellColumnIndex = 1;

                //Loop through each row and read value from each column. 
                for (intRowCounter = 0; intRowCounter < intRowNumberOfRecords; intRowCounter++)
                {
                    for (intColumnCounter = 0; intColumnCounter < intColumnNumberOfRecords; intColumnCounter++)
                    {
                        worksheet.Cells[cellRowIndex, cellColumnIndex] = ThePermitReportDataSet.permitreport.Rows[intRowCounter][intColumnCounter].ToString();

                        cellColumnIndex++;
                    }
                    cellColumnIndex = 1;
                    cellRowIndex++;
                }

                //Getting the location and file name of the excel to save from user. 
                SaveFileDialog saveDialog = new SaveFileDialog();
                saveDialog.Filter = "Excel files (*.xlsx)|*.xlsx|All files (*.*)|*.*";
                saveDialog.FilterIndex = 1;

                saveDialog.ShowDialog();

                workbook.SaveAs(saveDialog.FileName);
                MessageBox.Show("Export Successful");

            }
            catch (System.Exception ex)
            {
                TheEventLogClass.InsertEventLogEntry(DateTime.Now, "Blue Jay Design // Pole Permit Report // Export to Excel " + ex.Message);

                MessageBox.Show(ex.ToString());
            }
            finally
            {
                excel.Quit();
                workbook = null;
                excel = null;
            }
        }

        private void BtnPrint_Click(object sender, RoutedEventArgs e)
        {
            int intCurrentRow = 0;
            int intCounter;
            int intColumns;
            int intNumberOfRecords;

            try
            {
                PrintDialog pdCancelledReport = new PrintDialog();

                if (pdCancelledReport.ShowDialog().Value)
                {
                    FlowDocument fdCancelledLines = new FlowDocument();
                    Thickness thickness = new Thickness(100, 50, 50, 50);
                    fdCancelledLines.PagePadding = thickness;

                    //Set Up Table Columns
                    Table cancelledTable = new Table();
                    fdCancelledLines.Blocks.Add(cancelledTable);
                    cancelledTable.CellSpacing = 0;
                    intColumns = ThePermitReportDataSet.permitreport.Columns.Count;

                    for (int intColumnCounter = 0; intColumnCounter < intColumns; intColumnCounter++)
                    {
                        cancelledTable.Columns.Add(new TableColumn());
                    }
                    cancelledTable.RowGroups.Add(new TableRowGroup());

                    //Title row
                    cancelledTable.RowGroups[0].Rows.Add(new TableRow());
                    TableRow newTableRow = cancelledTable.RowGroups[0].Rows[intCurrentRow];
                    newTableRow.Cells.Add(new TableCell(new Paragraph(new Run("The Permits Report"))));
                    newTableRow.Cells[0].FontSize = 20;
                    newTableRow.Cells[0].FontFamily = new FontFamily("Times New Roman");
                    newTableRow.Cells[0].ColumnSpan = intColumns;
                    newTableRow.Cells[0].TextAlignment = TextAlignment.Center;
                    newTableRow.Cells[0].Padding = new Thickness(0, 0, 0, 20);

                    //Header Row
                    cancelledTable.RowGroups[0].Rows.Add(new TableRow());
                    intCurrentRow++;
                    newTableRow = cancelledTable.RowGroups[0].Rows[intCurrentRow];
                    newTableRow.Cells.Add(new TableCell(new Paragraph(new Run("Transaction ID"))));
                    newTableRow.Cells.Add(new TableCell(new Paragraph(new Run("Project ID"))));
                    newTableRow.Cells.Add(new TableCell(new Paragraph(new Run("Project Name"))));
                    newTableRow.Cells.Add(new TableCell(new Paragraph(new Run("Date Received"))));
                    newTableRow.Cells.Add(new TableCell(new Paragraph(new Run("Time Opened"))));
                    newTableRow.Cells.Add(new TableCell(new Paragraph(new Run("Permit Status"))));
                    newTableRow.Cells.Add(new TableCell(new Paragraph(new Run("Employee Name"))));
                    newTableRow.Cells.Add(new TableCell(new Paragraph(new Run("Permit Notes"))));

                    //Format Header Row
                    for (intCounter = 0; intCounter < intColumns; intCounter++)
                    {
                        newTableRow.Cells[intCounter].FontSize = 11;
                        newTableRow.Cells[intCounter].FontFamily = new FontFamily("Times New Roman");
                        newTableRow.Cells[intCounter].BorderBrush = Brushes.Black;
                        newTableRow.Cells[intCounter].TextAlignment = TextAlignment.Center;
                        newTableRow.Cells[intCounter].BorderThickness = new Thickness();
                    }

                    intNumberOfRecords = ThePermitReportDataSet.permitreport.Rows.Count;

                    //Data, Format Data

                    for (int intReportRowCounter = 0; intReportRowCounter < intNumberOfRecords; intReportRowCounter++)
                    {
                        cancelledTable.RowGroups[0].Rows.Add(new TableRow());
                        intCurrentRow++;
                        newTableRow = cancelledTable.RowGroups[0].Rows[intCurrentRow];
                        for (int intColumnCounter = 0; intColumnCounter < intColumns; intColumnCounter++)
                        {
                            newTableRow.Cells.Add(new TableCell(new Paragraph(new Run(ThePermitReportDataSet.permitreport[intReportRowCounter][intColumnCounter].ToString()))));


                            newTableRow.Cells[intColumnCounter].FontSize = 12;
                            newTableRow.Cells[0].FontFamily = new FontFamily("Times New Roman");
                            newTableRow.Cells[intColumnCounter].BorderBrush = Brushes.LightSteelBlue;
                            newTableRow.Cells[intColumnCounter].BorderThickness = new Thickness(0, 0, 0, 1);
                            newTableRow.Cells[intColumnCounter].TextAlignment = TextAlignment.Center;
                        }
                    }

                    //Set up page and print
                    fdCancelledLines.ColumnWidth = pdCancelledReport.PrintableAreaWidth;
                    fdCancelledLines.PageHeight = pdCancelledReport.PrintableAreaHeight;
                    fdCancelledLines.PageWidth = pdCancelledReport.PrintableAreaWidth;
                    pdCancelledReport.PrintDocument(((IDocumentPaginatorSource)fdCancelledLines).DocumentPaginator, "Permit Report");
                    intCurrentRow = 0;

                }
            }
            catch (Exception Ex)
            {
                TheMessagesClass.ErrorMessage(Ex.ToString());

                TheEventLogClass.InsertEventLogEntry(DateTime.Now, "Blue Jay Design // Design Project Report // Print Button " + Ex.Message);
            }
        }
    }
}
