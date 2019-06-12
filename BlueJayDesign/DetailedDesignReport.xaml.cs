/* Title:           Detailed Design Report
 * Date:            6-11-19
 * Author:          Terry Holmes
 * 
 * Description:     This is used to display the detailed report */

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
using DataValidationDLL;
using DesignProjectUpdateDLL;
using NewEventLogDLL;
using NewEmployeeDLL;
using Microsoft.Win32;

namespace BlueJayDesign
{
    /// <summary>
    /// Interaction logic for DetailedDesignReport.xaml
    /// </summary>
    public partial class DetailedDesignReport : Window
    {
        //setting up the classes
        WPFMessagesClass TheMessagesClass = new WPFMessagesClass();
        DataValidationClass TheDataValidationClass = new DataValidationClass();
        DesignProjectUpdateClass TheDesignProjectUpdateClass = new DesignProjectUpdateClass();
        EventLogClass TheEventLogClass = new EventLogClass();
        EmployeeClass TheEmployeeClass = new EmployeeClass();

        FindDetailedDesignProjectReportByLocationDataSet TheFindDetailedDesignProjectReportByLocationDataSet = new FindDetailedDesignProjectReportByLocationDataSet();
        DetailedProjectDataSet TheDetailedprojectDataSet = new DetailedProjectDataSet();

        //setting up variables
        int gintCounter;
        int gintNumberOfRecords;

        public DetailedDesignReport()
        {
            InitializeComponent();
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            int intCounter;
            int intNumberOfRecords;

            try
            {
                MainWindow.TheFindWarehousesDataSet = TheEmployeeClass.FindWarehouses();

                intNumberOfRecords = MainWindow.TheFindWarehousesDataSet.FindWarehouses.Rows.Count - 1;
                cboSelectLocation.Items.Add("Select Location");

                for(intCounter = 0; intCounter <= intNumberOfRecords; intCounter++)
                {
                    cboSelectLocation.Items.Add(MainWindow.TheFindWarehousesDataSet.FindWarehouses[intCounter].FirstName);
                }

                cboSelectLocation.SelectedIndex = 0;
            }
            catch (Exception Ex)
            {
                TheEventLogClass.InsertEventLogEntry(DateTime.Now, "Blue Jay Design // Detailed Design Report // Window Loaded " + Ex.Message);

                TheMessagesClass.ErrorMessage(Ex.ToString());
            }
        }

        private void CboSelectLocation_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int intSelectedIndex;

            intSelectedIndex = cboSelectLocation.SelectedIndex - 1;

            if(intSelectedIndex > -1)
            {
                MainWindow.gintWarehouseID = MainWindow.TheFindWarehousesDataSet.FindWarehouses[intSelectedIndex].EmployeeID;
            }
        }

        private void BtnGenerateReport_Click(object sender, RoutedEventArgs e)
        {
            //setting local variables
            string strValueForValidation;
            bool blnThereIsAProblem = false;
            bool blnFatalError = false;
            int intCounter;
            int intNumberOfRecords;
            DateTime datStartDate = DateTime.Now;
            DateTime datEndDate = DateTime.Now;
            string strErrorMessage = "";
            bool blnItemFound;
            string strProjectID;
            string strProjectUpdate;
            int intSecondCounter;

            try
            {
                strValueForValidation = txtStartDate.Text;
                blnThereIsAProblem = TheDataValidationClass.VerifyDateData(strValueForValidation);
                if(blnThereIsAProblem == true)
                {
                    blnFatalError = true;
                    strErrorMessage += "The Start Date is not a Date\n";
                }
                else
                {
                    datStartDate = Convert.ToDateTime(strValueForValidation);
                }
                strValueForValidation = txtEndDate.Text;
                blnThereIsAProblem = TheDataValidationClass.VerifyDateData(strValueForValidation);
                if (blnThereIsAProblem == true)
                {
                    blnFatalError = true;
                    strErrorMessage += "The End Date is not a Date\n";
                }
                else
                {
                    datEndDate = Convert.ToDateTime(strValueForValidation);
                }
                if(blnFatalError == false)
                {
                    if(datStartDate > datEndDate)
                    {
                        blnFatalError = true;
                        strErrorMessage += "The Start Date Is After The End Date\n";
                    }
                }
                if(cboSelectLocation.SelectedIndex < 1)
                {
                    blnFatalError = true;
                    strErrorMessage += "The Location Was Not Selected\n";
                }
                if(blnFatalError == true)
                {
                    TheMessagesClass.ErrorMessage(strErrorMessage);
                    return;
                }

                TheFindDetailedDesignProjectReportByLocationDataSet = TheDesignProjectUpdateClass.FindDetailedDesignProjectReportByLocation(datStartDate, datEndDate, MainWindow.gintWarehouseID);

                intNumberOfRecords = TheFindDetailedDesignProjectReportByLocationDataSet.FindDetailedDesignProjectReportByLocation.Rows.Count - 1;
                gintCounter = 0;
                gintNumberOfRecords = 0;
                TheDetailedprojectDataSet.detailedproject.Rows.Clear();

                for(intCounter = 0; intCounter <= intNumberOfRecords; intCounter++)
                {
                    strProjectID = TheFindDetailedDesignProjectReportByLocationDataSet.FindDetailedDesignProjectReportByLocation[intCounter].AssignedProjectID;
                    strProjectUpdate = TheFindDetailedDesignProjectReportByLocationDataSet.FindDetailedDesignProjectReportByLocation[intCounter].ProjectUpdate;
                    blnItemFound = false;

                    if(gintCounter > 0)
                    {
                        for(intSecondCounter = 0; intSecondCounter <= gintNumberOfRecords; intSecondCounter++)
                        {
                            if (strProjectID == TheDetailedprojectDataSet.detailedproject[intSecondCounter].ProjectID)
                            {
                                strProjectUpdate = TheDetailedprojectDataSet.detailedproject[intSecondCounter].ProjectUpdate + "\n" + strProjectUpdate;
                                TheDetailedprojectDataSet.detailedproject[intSecondCounter].ProjectUpdate = strProjectUpdate;
                                blnItemFound = true;
                            }
                        }
                    }

                    if(blnItemFound == false)
                    {
                        DetailedProjectDataSet.detailedprojectRow NewProjectRow = TheDetailedprojectDataSet.detailedproject.NewdetailedprojectRow();

                        if(TheFindDetailedDesignProjectReportByLocationDataSet.FindDetailedDesignProjectReportByLocation[intCounter].IsCompleteDateNull() == false)
                        {
                            NewProjectRow.CompleteDate = TheFindDetailedDesignProjectReportByLocationDataSet.FindDetailedDesignProjectReportByLocation[intCounter].CompleteDate;
                        }

                        NewProjectRow.DateReceived = TheFindDetailedDesignProjectReportByLocationDataSet.FindDetailedDesignProjectReportByLocation[intCounter].DateReceived;
                        NewProjectRow.JobStatus = TheFindDetailedDesignProjectReportByLocationDataSet.FindDetailedDesignProjectReportByLocation[intCounter].JobStatus;
                        NewProjectRow.ProjectID = strProjectID;
                        NewProjectRow.ProjectName = TheFindDetailedDesignProjectReportByLocationDataSet.FindDetailedDesignProjectReportByLocation[intCounter].projectname;
                        NewProjectRow.ProjectUpdate = strProjectUpdate;

                        TheDetailedprojectDataSet.detailedproject.Rows.Add(NewProjectRow);
                        gintNumberOfRecords = gintCounter;
                        gintCounter++;
                    }
                }

                dgrResults.ItemsSource = TheDetailedprojectDataSet.detailedproject;
            }
            catch (Exception Ex)
            {
                TheEventLogClass.InsertEventLogEntry(DateTime.Now, "Blue Jay Design // Detailed Design Report // Generate Report Button " + Ex.Message);

                TheMessagesClass.ErrorMessage(Ex.ToString());
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
                intRowNumberOfRecords = TheDetailedprojectDataSet.detailedproject.Rows.Count;
                intColumnNumberOfRecords = TheDetailedprojectDataSet.detailedproject.Columns.Count;

                for (intColumnCounter = 0; intColumnCounter < intColumnNumberOfRecords; intColumnCounter++)
                {
                    worksheet.Cells[cellRowIndex, cellColumnIndex] = TheDetailedprojectDataSet.detailedproject.Columns[intColumnCounter].ColumnName;

                    cellColumnIndex++;
                }

                cellRowIndex++;
                cellColumnIndex = 1;

                //Loop through each row and read value from each column. 
                for (intRowCounter = 0; intRowCounter < intRowNumberOfRecords; intRowCounter++)
                {
                    for (intColumnCounter = 0; intColumnCounter < intColumnNumberOfRecords; intColumnCounter++)
                    {
                        worksheet.Cells[cellRowIndex, cellColumnIndex] = TheDetailedprojectDataSet.detailedproject.Rows[intRowCounter][intColumnCounter].ToString();

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
                TheEventLogClass.InsertEventLogEntry(DateTime.Now, "Blue Jay Design // Detailed Design Report // Export to Excel " + ex.Message);

                MessageBox.Show(ex.ToString());
            }
            finally
            {
                excel.Quit();
                workbook = null;
                excel = null;
            }
        }
    }
}
