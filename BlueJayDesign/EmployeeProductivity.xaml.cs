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
using WorkTaskDLL;
using DateSearchDLL;
using TechPayDLL;
using WOVInvoicingDLL;
using DesignProjectDocumentation;
using ProductivityToTechPayDLL;

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
        WorkTaskClass TheWorkTaskClass = new WorkTaskClass();
        DateSearchClass TheDateSearchClass = new DateSearchClass();
        TechPayClass TheTechPayClass = new TechPayClass();
        WOVInvoicingClass TheWOVInvoicingClass = new WOVInvoicingClass();
        DesignProjectDocumentationClass TheDesignProjectDocumentationClass = new DesignProjectDocumentationClass();
        ProductivityToTechPayClass TheProductivityToTechPayClass = new ProductivityToTechPayClass();

        //setting up the data

        FindDesignProjectsByAssignedProjectIDDataSet TheFindDesignProjectsByAssignedProjectIDDataSet = new FindDesignProjectsByAssignedProjectIDDataSet();
        FindDesignTotalEmployeeProductivityHoursDataSet TheFindEmployeeTotalProductivityHoursDataSet = new FindDesignTotalEmployeeProductivityHoursDataSet();
        FindProductivityToTechPayByProductivityIDDataSet ThefindProductivityToTechPayByProductivityIDDataSet = new FindProductivityToTechPayByProductivityIDDataSet();
        FindProjectTechPayItemByDAteTimeDataSet TheFindProjectTechPayItemByDateTimeDataSet = new FindProjectTechPayItemByDAteTimeDataSet();
        FindWorkTaskByTaskKeywordDataSet TheFindWorkTaskByTaskKeywordDataSet = new FindWorkTaskByTaskKeywordDataSet();
        TechPayItemDataSet TheTechPayItemDataSet = new TechPayItemDataSet();

        //setting up the global variables
        string gstrProjectID;
        bool gblnTechPayAttached;
        bool gblnHoursComputed;
        decimal gdecTechPayPrice;
        int gintProductivityID;
        int gintTechPayID;
        bool gblnProductivityOnly;
        
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
            txtEndTime.Text = "";
            txtEnterDate.Text = "";
            txtProductivityItem.Text = "";
            txtProjectID.Text = "";
            txtStartTime.Text = "";
            gblnHoursComputed = false;
            gblnProductivityOnly = false;
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
            decimal decTotalTechPayPrice;

            try
            {
                strValueForValidation = txtEnterDate.Text;
                blnThereIsAProblem = TheDataValidationClass.VerifyDateData(strValueForValidation);
                gdecTechPayPrice = 0;
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
                if (cboSelectProductivityItem.SelectedIndex < 1)
                {
                    blnFatalError = true;
                    strErrorMessage += "The Techpay Item was not Selected\n";
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

                if (gblnHoursComputed == true)
                {
                    decTotalHours = 0;
                }

                if (decTotalHours < 0)
                {
                    TheMessagesClass.ErrorMessage("You Cannot Have Negative Hours");
                    return;
                }

                decTotalTechPayPrice = gdecTechPayPrice * intQuantity;

                if(gblnProductivityOnly == false)
                {
                    blnFatalError = TheTechPayClass.InsertProjectTechpayItem(MainWindow.gintProjectID, false, "DESIGN", MainWindow.TheVerifyDesignEmployeeLogonDataSet.VerifyDesigEmployeeLogon[0].EmployeeID, MainWindow.gintWarehouseID, gintTechPayID, gdecTechPayPrice, intQuantity, decTotalTechPayPrice, datTransactionDate);

                    if (blnFatalError == true)
                        throw new Exception();

                    TheFindProjectTechPayItemByDateTimeDataSet = TheTechPayClass.FindProjectTechPayItemsByDateTime(datTransactionDate);

                    MainWindow.gintTransactionID = TheFindProjectTechPayItemByDateTimeDataSet.FindProjectTechPayItemByDateTime[0].TransactionID;

                    blnFatalError = TheTechPayClass.UpdateProjectTechPayPoleStick(MainWindow.gintTransactionID, false);

                    if (blnFatalError == true)
                        throw new Exception();
                }          

                blnFatalError = TheDesignProductivityClass.InsertDesignProductivity(MainWindow.gintProjectID, MainWindow.TheVerifyDesignEmployeeLogonDataSet.VerifyDesigEmployeeLogon[0].EmployeeID, gintProductivityID, decTotalHours, datTransactionDate);

                if (blnFatalError == true)
                    throw new Exception();

               

                gblnHoursComputed = true;
                txtProductivityItem.Text = "";
                cboSelectProductivityItem.Items.Clear();
                txtQuantity.Text = "";

            }
            catch (Exception Ex)
            {
                TheEventLogClass.InsertEventLogEntry(DateTime.Now, "Blue Jay Design // Employee Productivity // Process Button " + Ex.Message);

                TheMessagesClass.ErrorMessage(Ex.ToString());
            }
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
        private decimal ComputeTotalHours(string strStartTime, string strEndTime)
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
                TheEventLogClass.InsertEventLogEntry(DateTime.Now, "Blue Jay Design // Employee Productivity // Calculate Time Span " + Ex.Message);

                TheMessagesClass.ErrorMessage(Ex.ToString());
            }

            return decTotalHours;
        }
        private void BtnMyProductivity_Click(object sender, RoutedEventArgs e)
        {
            MyProductivity MyProductivity = new MyProductivity();
            MyProductivity.ShowDialog();
        }

        private void TxtProductivityItem_TextChanged(object sender, TextChangedEventArgs e)
        {
            string strProductivityItem;
            int intLength;
            int intCounter;
            int intNumberOfRecords;
            
            try
            {
                //checking project id
                gstrProjectID = txtProjectID.Text;
                TheFindDesignProjectsByAssignedProjectIDDataSet = TheDesignProjectsClass.FindDesignProjectsByAssignedProjectID(gstrProjectID);
                intNumberOfRecords = TheFindDesignProjectsByAssignedProjectIDDataSet.FindDesignProjectsByAssignedProjectID.Rows.Count;
                if(intNumberOfRecords < 1)
                {
                    TheMessagesClass.ErrorMessage("The Project ID was not Found");
                    return;
                }

                MainWindow.gintProjectID = TheFindDesignProjectsByAssignedProjectIDDataSet.FindDesignProjectsByAssignedProjectID[0].ProjectID;

                //getting productivity item
                strProductivityItem = txtProductivityItem.Text;
                intLength = strProductivityItem.Length;

                if(intLength > 2)
                {
                    cboSelectProductivityItem.Items.Clear();
                    cboSelectProductivityItem.Items.Add("Select Productivity Item");

                    TheFindWorkTaskByTaskKeywordDataSet = TheWorkTaskClass.FindWorkTaskByTaskKeyword(strProductivityItem);

                    intNumberOfRecords = TheFindWorkTaskByTaskKeywordDataSet.FindWorkTaskByTaskKeyword.Rows.Count - 1;

                    if(intNumberOfRecords < 0)
                    {
                        TheMessagesClass.ErrorMessage("The Task Entered is not Correct");
                        return;
                    }

                    for(intCounter = 0; intCounter <= intNumberOfRecords; intCounter++)
                    {
                        cboSelectProductivityItem.Items.Add(TheFindWorkTaskByTaskKeywordDataSet.FindWorkTaskByTaskKeyword[intCounter].WorkTask);
                    }

                    cboSelectProductivityItem.SelectedIndex = 0;
                }
            }
            catch (Exception Ex)
            {
                TheEventLogClass.InsertEventLogEntry(DateTime.Now, "Blue Jay Design // Employee Productivity // Productivity Item Text Box " + Ex.Message);

                TheMessagesClass.ErrorMessage(Ex.ToString());
            }
        }

        private void CboSelectProductivityItem_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int intSelectedIndex;
            int intRecordsReturned;

            try
            {
                intSelectedIndex = cboSelectProductivityItem.SelectedIndex - 1;

                if(intSelectedIndex > -1)
                {
                    gintProductivityID = TheFindWorkTaskByTaskKeywordDataSet.FindWorkTaskByTaskKeyword[intSelectedIndex].WorkTaskID;

                    ThefindProductivityToTechPayByProductivityIDDataSet = TheProductivityToTechPayClass.FindProductivityToTechPayByProductivityID(gintProductivityID);

                    intRecordsReturned = ThefindProductivityToTechPayByProductivityIDDataSet.FindProductivityToTechPayByProductivityID.Rows.Count;

                    if(intRecordsReturned < 1)
                    {
                        gblnProductivityOnly = true;
                    }
                    else
                    {
                        gintTechPayID = ThefindProductivityToTechPayByProductivityIDDataSet.FindProductivityToTechPayByProductivityID[0].TechPayID;
                    }
                }
            }
            catch (Exception Ex)
            {
                TheEventLogClass.InsertEventLogEntry(DateTime.Now, "Blue Jay Design // Enter Design WOV Tech Pay // cboSelectTechPayItem Change Event " + Ex.Message);

                TheMessagesClass.ErrorMessage(Ex.ToString());
            }
        }
        private void GetQuantity()
        {
            //setting up variables
            decimal decTotalHours;
            string strStartTime;
            string strEndTime;

            strStartTime = txtStartTime.Text;
            strEndTime = txtEndTime.Text;

            decTotalHours = CalculateTimeSpan(strStartTime, strEndTime);

            decTotalHours = Math.Round(decTotalHours, 0, MidpointRounding.AwayFromZero);

            txtQuantity.Text = Convert.ToString(decTotalHours);
        }

        private void TxtProjectID_TextChanged(object sender, TextChangedEventArgs e)
        {
            gblnHoursComputed = false;
        }

        private void TxtEnterDate_TextChanged(object sender, TextChangedEventArgs e)
        {
            gblnHoursComputed = false;
        }

        private void BtnAttachDocument_Click(object sender, RoutedEventArgs e)
        {
            //setting local variables
            string strDocumentPath;
            string strDocumentType = "TECHPAY SHEET";
            bool blnFatalError = false;
            DateTime datTransactionDate = DateTime.Now;

            try
            {
                gblnTechPayAttached = true;

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

                blnFatalError = TheDesignProjectDocumentationClass.InsertDesignProjectDocumentation(MainWindow.gintProjectID, MainWindow.TheVerifyDesignEmployeeLogonDataSet.VerifyDesigEmployeeLogon[0].EmployeeID, datTransactionDate, strDocumentType, strDocumentPath);

                if (blnFatalError == true)
                    throw new Exception();

                TheMessagesClass.InformationMessage("The Documents have been Added");
            }
            catch (Exception Ex)
            {
                TheEventLogClass.InsertEventLogEntry(DateTime.Now, "Blue Jay Design // Employee Productivity // Attach Documents " + Ex.Message);

                TheMessagesClass.ErrorMessage(Ex.ToString());
            }
        }

        private void BtnConstructionProductivity_Click(object sender, RoutedEventArgs e)
        {
            ConstructionProductivity ConstructionProductivity = new ConstructionProductivity();
            ConstructionProductivity.ShowDialog();
        }

        private void TxtStartTime_TextChanged(object sender, TextChangedEventArgs e)
        {
            gblnHoursComputed = false;
        }

        private void TxtEndTime_TextChanged(object sender, TextChangedEventArgs e)
        {
            gblnHoursComputed = false;
        }
    }
}
