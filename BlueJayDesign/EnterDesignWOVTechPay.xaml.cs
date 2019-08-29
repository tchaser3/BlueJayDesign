/* Title:           Enter Design WOV Tech Pay
 * Date:            5-24-19
 * Author:          Terry Holmes
 * 
 * Description:     This is used to Enter Design WOV Tech Pay */

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
using WOVInvoicingDLL;
using TechPayDLL;
using DataValidationDLL;
using DesignProductivityDLL;
using DesignProjectDocumentation;
using ProductivityToTechPayDLL;
using NewEmployeeDLL;
using WorkTaskDLL;

namespace BlueJayDesign
{
    /// <summary>
    /// Interaction logic for EnterDesignWOVTechPay.xaml
    /// </summary>
    public partial class EnterDesignWOVTechPay : Window
    {
        //setting up the classes
        WPFMessagesClass TheMessagesClass = new WPFMessagesClass();
        EventLogClass TheEventLogClass = new EventLogClass();
        WOVInvoicingClass TheWOVInvoicingClass = new WOVInvoicingClass();
        TechPayClass TheTechPayClass = new TechPayClass();
        DataValidationClass TheDataValidationClass = new DataValidationClass();
        DesignProductivityClass TheDesignProductivityClass = new DesignProductivityClass();
        DesignProjectDocumentationClass TheDesignProjectDocumentationClass = new DesignProjectDocumentationClass();
        ProductivityToTechPayClass TheProductivityToTechPayClass = new ProductivityToTechPayClass();
        EmployeeClass TheEmployeeClass = new EmployeeClass();
        WorkTaskClass TheWorkTaskClass = new WorkTaskClass();

        //setting up the data variables
        FindWorkTaskByTaskKeywordDataSet TheFindWorkTaskByTaskKeywordDataSet = new FindWorkTaskByTaskKeywordDataSet();
        FindProductivityToTechPayByProductivityIDDataSet TheFindProductivityToTechPayByProductivityIDDataSet = new FindProductivityToTechPayByProductivityIDDataSet();
        FindProjectTechPayItemByDAteTimeDataSet TheFindProjectTechPayItemByDateTimeDataSet = new FindProjectTechPayItemByDAteTimeDataSet();

        //setting up global variables
        int gintTechPayID;
        int gintProductivityID;
        bool gblnTechPayAttached;
        bool gblnHoursComputed;
        bool gblnPoleStick;
        decimal gdecTotalHours;
        bool gblnProductivityOnly;

        public EnterDesignWOVTechPay()
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
            MainWindow.UpdateSurveyWindow.Visibility = Visibility.Hidden;
            this.Close();
        }

        private void BtnAddNewProject_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.AddNewProjectWindow.Visibility = Visibility.Visible;
            MainWindow.UpdateSurveyWindow.Visibility = Visibility.Hidden;
            this.Close();
        }


        private void BtnMyTasks_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.UpdateAssignTasksWindow.Visibility = Visibility.Visible;
            MainWindow.UpdateSurveyWindow.Visibility = Visibility.Hidden;
            this.Close();
        }

        private void BtnAssignTask_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.AssignTasksWindow.Visibility = Visibility.Visible;
            MainWindow.UpdateSurveyWindow.Visibility = Visibility.Hidden;
            this.Close();
        }

        private void BtnAssignSurveyor_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.AssignSurveyorWindow.Visibility = Visibility.Visible;
            MainWindow.UpdateSurveyWindow.Visibility = Visibility.Hidden;
            this.Close();
        }

        private void BtnUpdateSurvey_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void BtnUpdateProject_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.UpdateProjectWindow.Visibility = Visibility.Visible;
            MainWindow.UpdateSurveyWindow.Visibility = Visibility.Hidden;
            this.Close();
        }

        private void BtnCloseProject_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.CloseProjectWindow.Visibility = Visibility.Visible;
            MainWindow.UpdateSurveyWindow.Visibility = Visibility.Hidden;
            this.Close();
        }
        private void BtnViewProjectInfo_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.ViewProjectInfoWindow.Visibility = Visibility.Visible;
            MainWindow.UpdateSurveyWindow.Visibility = Visibility.Hidden;
            this.Close(); ;
        }
        private void BtnEmployeeProductivity_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.EmployeeProductivityWindow.Visibility = Visibility.Visible;
            MainWindow.UpdateSurveyWindow.Visibility = Visibility.Hidden;
            this.Close();
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            TheMessagesClass.CloseTheProgram();
        }

        private void BtnHome_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.UpdateSurveyWindow.Visibility = Visibility.Hidden;
            MainWindow.UpdateSurveyWindow.Visibility = Visibility.Hidden;
            this.Close();
        }
        private void BtnOpenDesignProjects_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.OpenDesignProjectWindow.Visibility = Visibility.Visible;
            MainWindow.UpdateSurveyWindow.Visibility = Visibility.Hidden;
            this.Close();
        }

        private void BtnDesignProjectReport_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.DesignProjectReportwindow.Visibility = Visibility.Visible;
            MainWindow.UpdateSurveyWindow.Visibility = Visibility.Hidden;
            this.Close();
        }

        private void BtnPolePermitReport_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.PolePermitReportWindow.Visibility = Visibility.Visible;
            MainWindow.UpdateSurveyWindow.Visibility = Visibility.Hidden;
            this.Close();
        }

        private void BtnOpenSurveryorReport_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.OpenSurveyorReportWindow.Visibility = Visibility.Visible;
            MainWindow.UpdateSurveyWindow.Visibility = Visibility.Hidden;
            this.Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            gblnTechPayAttached = false;
            gblnHoursComputed = false;
            txtDate.Text = "";
            rdoNo.IsChecked = true;
            rdoContractorNo.IsChecked = true;
        }

        private void TxtEnterTechPayItem_TextChanged(object sender, TextChangedEventArgs e)
        {
            string strWorkTask;
            int intLength;
            int intCounter;
            int intNumberOfRecords;

            try
            {
                strWorkTask = txtEnterTechPayItem.Text;
                intLength = strWorkTask.Length;
                if(intLength > 2)
                {
                    cboSelectTechPayItem.Items.Clear();
                    cboSelectTechPayItem.Items.Add("Select Work Task");

                    TheFindWorkTaskByTaskKeywordDataSet = TheWorkTaskClass.FindWorkTaskByTaskKeyword(strWorkTask);
                    intNumberOfRecords = TheFindWorkTaskByTaskKeywordDataSet.FindWorkTaskByTaskKeyword.Rows.Count - 1;

                    if(intNumberOfRecords < 0)
                    {
                        TheMessagesClass.ErrorMessage("The Work Task Was Not Found");
                        return;
                    }

                    for(intCounter = 0; intCounter <= intNumberOfRecords; intCounter++)
                    {
                        cboSelectTechPayItem.Items.Add(TheFindWorkTaskByTaskKeywordDataSet.FindWorkTaskByTaskKeyword[intCounter].WorkTask);
                    }

                    cboSelectTechPayItem.SelectedIndex = 0;
                }
            }
            catch (Exception Ex)
            {
                TheEventLogClass.InsertEventLogEntry(DateTime.Now, "Blue Jay Design // Enter Design WOV Tech Pay // Enter Tech Pay Item Text " + Ex.Message);

                TheMessagesClass.ErrorMessage(Ex.ToString());
            }
        }

        private void CboSelectTechPayItem_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int intSelectedIndex;
            decimal decTechPayCost = 0;
            int intRecordsReturned;
            string strStartTime;
            string strEndTime;
            string strTechPayCode;
            bool blnThereIsAProblem = false;
            string strErrorMessage = "";
            bool blnFatalError = false;

            try
            {
                intSelectedIndex = cboSelectTechPayItem.SelectedIndex - 1;

                if (intSelectedIndex > -1)
                {
                    gintProductivityID = TheFindWorkTaskByTaskKeywordDataSet.FindWorkTaskByTaskKeyword[intSelectedIndex].WorkTaskID;

                    TheFindProductivityToTechPayByProductivityIDDataSet = TheProductivityToTechPayClass.FindProductivityToTechPayByProductivityID(gintProductivityID);

                    intRecordsReturned = TheFindProductivityToTechPayByProductivityIDDataSet.FindProductivityToTechPayByProductivityID.Rows.Count;

                    if (intRecordsReturned < 1)
                    {
                        gblnProductivityOnly = true;
                    }
                    else
                    {
                        gintTechPayID = TheFindProductivityToTechPayByProductivityIDDataSet.FindProductivityToTechPayByProductivityID[0].TechPayID;
                    }
                }
            }
            catch (Exception Ex)
            {
                TheEventLogClass.InsertEventLogEntry(DateTime.Now, "Blue Jay Design // Enter Design WOV Tech Pay // cboSelectTechPayItem Change Event " + Ex.Message);

                TheMessagesClass.ErrorMessage(Ex.ToString());
            }
        }

        private void BtnAttachTechPaySheet_Click(object sender, RoutedEventArgs e)
        {
            AttachDocuments();
        }
        private void AttachDocuments()
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
                TheEventLogClass.InsertEventLogEntry(DateTime.Now, "Blue Jay Design // Enter Design WOV Tech Pay // Attach Documents " + Ex.Message);

                TheMessagesClass.ErrorMessage(Ex.ToString());
            }
        }

        private void BtnContractorAttachTechPaySheet_Click(object sender, RoutedEventArgs e)
        {
            AttachDocuments();
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
            decimal decTechPayPrice;
            decimal decTotalTechPayPrice;

            try
            {
                strValueForValidation = txtDate.Text;
                blnThereIsAProblem = TheDataValidationClass.VerifyDateData(strValueForValidation);
                if(blnThereIsAProblem == true)
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
                if(blnThereIsAProblem == true)
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
                if(cboSelectTechPayItem.SelectedIndex < 1)
                {
                    blnFatalError = true;
                    strErrorMessage += "The Techpay Item was not Selected\n";
                }
                strValueForValidation = txtQuantity.Text;
                blnThereIsAProblem = TheDataValidationClass.VerifyIntegerData(strValueForValidation);
                if(blnThereIsAProblem == true)
                {
                    blnFatalError = true;
                    strErrorMessage += "The Quantity Entered is not an Integer\n";
                }
                else
                {
                    intQuantity = Convert.ToInt32(strValueForValidation);
                }
                if(blnFatalError == true)
                {
                    TheMessagesClass.ErrorMessage(strErrorMessage);
                    return;
                }

                decTotalHours = CalculateTimeSpan(strStartTime, strEndTime);

                if(gblnHoursComputed == true)
                {
                    decTotalHours = 0;
                }

                if(decTotalHours < 0)
                {
                    TheMessagesClass.ErrorMessage("You Cannot Have Negative Hours");
                    return;
                }

                decTechPayPrice = 0;
                decTotalTechPayPrice = decTechPayPrice * intQuantity;

                if(gblnProductivityOnly == false)
                {
                    blnFatalError = TheTechPayClass.InsertProjectTechpayItem(MainWindow.gintProjectID, false, "DESIGN", MainWindow.TheVerifyDesignEmployeeLogonDataSet.VerifyDesigEmployeeLogon[0].EmployeeID, MainWindow.gintWarehouseID, gintTechPayID, decTechPayPrice, intQuantity, decTotalTechPayPrice, datTransactionDate);

                    if (blnFatalError == true)
                        throw new Exception();

                    TheFindProjectTechPayItemByDateTimeDataSet = TheTechPayClass.FindProjectTechPayItemsByDateTime(datTransactionDate);

                    MainWindow.gintTransactionID = TheFindProjectTechPayItemByDateTimeDataSet.FindProjectTechPayItemByDateTime[0].TransactionID;

                    blnFatalError = TheTechPayClass.UpdateProjectTechPayPoleStick(MainWindow.gintTransactionID, gblnPoleStick);

                    if (blnFatalError == true)
                        throw new Exception();
                }                

                blnFatalError = TheDesignProductivityClass.InsertDesignProductivity(MainWindow.gintProjectID, MainWindow.TheVerifyDesignEmployeeLogonDataSet.VerifyDesigEmployeeLogon[0].EmployeeID, gintProductivityID, decTotalHours, datTransactionDate);

                if (blnFatalError == true)
                    throw new Exception();                

                gblnHoursComputed = true;
                txtEnterTechPayItem.Text = "";
                cboSelectTechPayItem.Items.Clear();
                txtQuantity.Text = "";

            }
            catch (Exception Ex)
            {
                TheEventLogClass.InsertEventLogEntry(DateTime.Now, "Blue Jay Design // Enter Design WOV Tech Pay // Process Button " + Ex.Message);

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

        private void TxtContractorTechPayItem_TextChanged(object sender, TextChangedEventArgs e)
        {
            /*string strTechPayItem;
            int intLength;
            int intCounter;
            int intNumberOfRecords;

            try
            {
                strTechPayItem = txtContractorTechPayItem.Text;
                intLength = strTechPayItem.Length;
                if (intLength > 2)
                {
                    cboContractorTechPayItem.Items.Clear();
                    cboContractorTechPayItem.Items.Add("Select Techpay Item");

                    TheFindTechPayItemByDescriptionDataSet = TheTechPayClass.FindTechPayItemByDescription(strTechPayItem);
                    intNumberOfRecords = TheFindTechPayItemByDescriptionDataSet.FindTechPayItemByDescription.Rows.Count - 1;

                    if (intNumberOfRecords < 0)
                    {
                        TheMessagesClass.ErrorMessage("The Techpay Item Was Not Found");
                        return;
                    }

                    for (intCounter = 0; intCounter <= intNumberOfRecords; intCounter++)
                    {
                        cboContractorTechPayItem.Items.Add(TheFindTechPayItemByDescriptionDataSet.FindTechPayItemByDescription[intCounter].JobDescription);
                    }

                    cboContractorTechPayItem.SelectedIndex = 0;
                }
            }
            catch (Exception Ex)
            {
                TheEventLogClass.InsertEventLogEntry(DateTime.Now, "Blue Jay Design // Enter Design WOV Tech Pay // Enter Tech Pay Item Text " + Ex.Message);

                TheMessagesClass.ErrorMessage(Ex.ToString());
            } */
        }

        private void TxtContractorLastName_TextChanged(object sender, TextChangedEventArgs e)
        {
            string strLastName;
            int intLength;
            int intNumberOfRecords;
            int intCounter;

            try
            {
                strLastName = txtContractorLastName.Text;
                intLength = strLastName.Length;
                if(intLength > 2)
                {
                    MainWindow.TheComboEmployeeDataSet = TheEmployeeClass.FillEmployeeComboBox(strLastName);

                    intNumberOfRecords = MainWindow.TheComboEmployeeDataSet.employees.Rows.Count - 1;

                    if(intNumberOfRecords < 0)
                    {
                        TheMessagesClass.ErrorMessage("Contractor Not Found");
                        return;
                    }

                    cboSelectContractor.Items.Clear();
                    cboSelectContractor.Items.Add("Select Contractor");

                    for(intCounter = 0; intCounter <= intNumberOfRecords; intCounter++)
                    {
                        cboSelectContractor.Items.Add(MainWindow.TheComboEmployeeDataSet.employees[intCounter].FullName);
                    }

                    cboSelectContractor.SelectedIndex = 0;
                }
            }
            catch (Exception Ex)
            {
                TheEventLogClass.InsertEventLogEntry(DateTime.Now, "Blue Jay Design // Enter Design WOV Techpay // Contractor Lastname Text Box " + Ex.Message);
                TheMessagesClass.ErrorMessage(Ex.ToString());
            }
        }

        private void CboSelectContractor_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int intSelectedIndex;

            intSelectedIndex = cboSelectContractor.SelectedIndex - 1;

            if(intSelectedIndex > -1)
            {
                MainWindow.gintEmployeeID = MainWindow.TheComboEmployeeDataSet.employees[intSelectedIndex].EmployeeID;
            }
        }

        private void BtnContratorProcess_Click(object sender, RoutedEventArgs e)
        {
            //setting up local variables
            string strValueForValidation;
            string strErrorMessage = "";
            bool blnThereIsAProblem = false;
            bool blnFatalError = false;
            int intQuantity = 0;
            decimal decTechPayPrice;
            decimal decTotalTechPayPrice;
            DateTime datTransactionDate = DateTime.Now;

            try
            {
                if(gblnTechPayAttached == false)
                {
                    blnFatalError = true;
                    strErrorMessage += "Techpay Sheet was not attached\n";
                }
                if(cboContractorTechPayItem.SelectedIndex < 1)
                {
                    blnFatalError = true;
                    strErrorMessage += "The Tech Pay Item was not Selected\n";
                }
                strValueForValidation = txtContractorQuantity.Text;
                blnThereIsAProblem = TheDataValidationClass.VerifyIntegerData(strValueForValidation);
                if(blnThereIsAProblem == true)
                {
                    blnFatalError = true;
                    strErrorMessage += "The Quantity Entered is not an Integer\n";
                }
                else
                {
                    intQuantity = Convert.ToInt32(strValueForValidation);
                }
                if(cboSelectContractor.SelectedIndex < 1)
                {
                    blnFatalError = true;
                    strErrorMessage += "The Contractor was not Selected\n";
                }
                if(blnFatalError == true)
                {
                    TheMessagesClass.ErrorMessage(strErrorMessage);
                    return;
                }

                decTechPayPrice = Convert.ToDecimal(txtContractorTechPayPrice.Text);
                decTotalTechPayPrice = Convert.ToDecimal(intQuantity) * decTechPayPrice;

                blnFatalError = TheTechPayClass.InsertProjectTechpayItem(MainWindow.gintProjectID, false, "DESIGN", MainWindow.gintEmployeeID, MainWindow.gintWarehouseID, gintTechPayID, decTechPayPrice, intQuantity, decTotalTechPayPrice, datTransactionDate);
                if (blnFatalError == true)
                    throw new Exception();

                TheFindProjectTechPayItemByDateTimeDataSet = TheTechPayClass.FindProjectTechPayItemsByDateTime(datTransactionDate);

                MainWindow.gintTransactionID = TheFindProjectTechPayItemByDateTimeDataSet.FindProjectTechPayItemByDateTime[0].TransactionID;

                blnFatalError = TheTechPayClass.UpdateProjectTechPayPoleStick(MainWindow.gintTransactionID, gblnPoleStick);

                if (blnFatalError == true)
                    throw new Exception();

                TheMessagesClass.InformationMessage("The Contractor Techpay Has Been Entered");

                txtContractorLastName.Text = "";
                txtContractorQuantity.Text = "";
                txtContractorTechPayItem.Text = "";
                txtContractorTechPayPrice.Text = "";
                cboContractorTechPayItem.Items.Clear();
                cboSelectContractor.Items.Clear();

            }
            catch (Exception Ex)
            {
                TheEventLogClass.InsertEventLogEntry(DateTime.Now, "Blue Jay Design // Enter Design WOV Techpay // Contractor Techpay Button " + Ex.Message);

                TheMessagesClass.ErrorMessage(Ex.ToString());
            }
        }

        private void CboContractorTechPayItem_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int intSelectedIndex;
            int intRecordsReturned;

            try
            {
                intSelectedIndex = cboSelectTechPayItem.SelectedIndex - 1;

                if (intSelectedIndex > -1)
                {
                    gintProductivityID = TheFindWorkTaskByTaskKeywordDataSet.FindWorkTaskByTaskKeyword[intSelectedIndex].WorkTaskID;

                    TheFindProductivityToTechPayByProductivityIDDataSet = TheProductivityToTechPayClass.FindProductivityToTechPayByProductivityID(gintProductivityID);

                    intRecordsReturned = TheFindProductivityToTechPayByProductivityIDDataSet.FindProductivityToTechPayByProductivityID.Rows.Count;

                    if (intRecordsReturned < 1)
                    {
                        gblnProductivityOnly = true;
                    }
                    else
                    {
                        gintTechPayID = TheFindProductivityToTechPayByProductivityIDDataSet.FindProductivityToTechPayByProductivityID[0].TechPayID;
                    }
                }

            }
            catch (Exception Ex)
            {
                TheEventLogClass.InsertEventLogEntry(DateTime.Now, "Blue Jay Design // Enter Design WOV Tech Pay // cboSelectTechPayItem Change Event " + Ex.Message);

                TheMessagesClass.ErrorMessage(Ex.ToString());
            }
        }

        private void RdoYes_Checked(object sender, RoutedEventArgs e)
        {
            gblnPoleStick = true;
        }

        private void RdoNo_Checked(object sender, RoutedEventArgs e)
        {
            gblnPoleStick = false;
        }

        private void RdoContractorNo_Checked(object sender, RoutedEventArgs e)
        {
            gblnPoleStick = false;
        }

        private void RdoContractorYes_Checked(object sender, RoutedEventArgs e)
        {
            gblnPoleStick = true;
        }

        private void TxtDate_TextChanged(object sender, TextChangedEventArgs e)
        {
            gblnHoursComputed = false;
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
