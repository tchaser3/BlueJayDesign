/* Title:           My Productivity
 * Date:            2-20-19
 * Author:          Terry Holmes
 * 
 * Description:     This is the window that a user can see their productivity */

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
using DesignProductivityDLL;
using NewEventLogDLL;
using Microsoft.Win32;

namespace BlueJayDesign
{
    /// <summary>
    /// Interaction logic for MyProductivity.xaml
    /// </summary>
    public partial class MyProductivity : Window
    {
        WPFMessagesClass TheMessagesClass = new WPFMessagesClass();
        EventLogClass TheEventLogClass = new EventLogClass();
        DataValidationClass TheDataValidationClass = new DataValidationClass();
        DesignProductivityClass TheDesignProductivityClass = new DesignProductivityClass();

        //setting up data
        FindDesignEmployeeProductivityByDateRangeDataSet TheFindDesignEmployeeProductivityByDateRangeDataSet = new FindDesignEmployeeProductivityByDateRangeDataSet();
        FindDesignTotalEmployeeProductivityHoursDataSet TheFindDesignTotalEmployeeProductivityHoursDataSet = new FindDesignTotalEmployeeProductivityHoursDataSet();
        
        public MyProductivity()
        {
            InitializeComponent();
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void BtnHelp_Click(object sender, RoutedEventArgs e)
        {
            TheMessagesClass.LaunchHelpSite();
        }

        private void BtnCreateReport_Click(object sender, RoutedEventArgs e)
        {
            string strValueForValidation;
            string strErrorMessage = "";
            bool blnThereIsAProblem = false;
            bool blnFatalError = false;
            DateTime datStartDate = DateTime.Now;
            DateTime datEndDate = DateTime.Now;
            
            try
            {
                //data Valiation
                strValueForValidation = txtStartDate.Text;
                blnThereIsAProblem = TheDataValidationClass.VerifyDateData(strValueForValidation);
                if(blnThereIsAProblem == true)
                {
                    blnFatalError = true;
                    strErrorMessage += "Start Date is not a Date\n";
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
                    strErrorMessage += "End Date is not a Date\n";
                }
                else
                {
                    datEndDate = Convert.ToDateTime(strValueForValidation);
                }
                if(blnFatalError == true)
                {
                    TheMessagesClass.ErrorMessage(strErrorMessage);
                    return;
                }
                else
                {
                    blnFatalError = TheDataValidationClass.verifyDateRange(datStartDate, datEndDate);

                    if(blnFatalError == true)
                    {
                        TheMessagesClass.ErrorMessage("The Start Date is after the End Date");
                        return;
                    }
                }

                //loading the data set
                TheFindDesignEmployeeProductivityByDateRangeDataSet = TheDesignProductivityClass.FindDesignEmployeeProductivityByDateRange(MainWindow.TheVerifyDesignEmployeeLogonDataSet.VerifyDesigEmployeeLogon[0].EmployeeID, datStartDate, datEndDate);
                TheFindDesignTotalEmployeeProductivityHoursDataSet = TheDesignProductivityClass.FindDesignTotalEmployeeProductivityHours(MainWindow.TheVerifyDesignEmployeeLogonDataSet.VerifyDesigEmployeeLogon[0].EmployeeID, datStartDate, datEndDate);

                dgrResults.ItemsSource = TheFindDesignEmployeeProductivityByDateRangeDataSet.FindDesignEmployeeProductivityByDateRange;

                txtTotalHours.Text = Convert.ToString(TheFindDesignTotalEmployeeProductivityHoursDataSet.FindDesignTotalEmployeeProductivityHours[0].TotalHours);
            }
            catch (Exception Ex)
            {
                TheEventLogClass.InsertEventLogEntry(DateTime.Now, "Blue Jay Design // My Productivity // Create Report Button " + Ex.Message);

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
                intRowNumberOfRecords = TheFindDesignEmployeeProductivityByDateRangeDataSet.FindDesignEmployeeProductivityByDateRange.Rows.Count;
                intColumnNumberOfRecords = TheFindDesignEmployeeProductivityByDateRangeDataSet.FindDesignEmployeeProductivityByDateRange.Columns.Count;

                for (intColumnCounter = 0; intColumnCounter < intColumnNumberOfRecords; intColumnCounter++)
                {
                    worksheet.Cells[cellRowIndex, cellColumnIndex] = TheFindDesignEmployeeProductivityByDateRangeDataSet.FindDesignEmployeeProductivityByDateRange.Columns[intColumnCounter].ColumnName;

                    cellColumnIndex++;
                }

                cellRowIndex++;
                cellColumnIndex = 1;

                //Loop through each row and read value from each column. 
                for (intRowCounter = 0; intRowCounter < intRowNumberOfRecords; intRowCounter++)
                {
                    for (intColumnCounter = 0; intColumnCounter < intColumnNumberOfRecords; intColumnCounter++)
                    {
                        worksheet.Cells[cellRowIndex, cellColumnIndex] = TheFindDesignEmployeeProductivityByDateRangeDataSet.FindDesignEmployeeProductivityByDateRange.Rows[intRowCounter][intColumnCounter].ToString();

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
                TheEventLogClass.InsertEventLogEntry(DateTime.Now, "Blue Jay Design // My Productivity // Export to Excel " + ex.Message);

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
                    intColumns = TheFindDesignEmployeeProductivityByDateRangeDataSet.FindDesignEmployeeProductivityByDateRange.Columns.Count;

                    for (int intColumnCounter = 0; intColumnCounter < intColumns; intColumnCounter++)
                    {
                        cancelledTable.Columns.Add(new TableColumn());
                    }
                    cancelledTable.RowGroups.Add(new TableRowGroup());

                    //Title row
                    cancelledTable.RowGroups[0].Rows.Add(new TableRow());
                    TableRow newTableRow = cancelledTable.RowGroups[0].Rows[intCurrentRow];
                    newTableRow.Cells.Add(new TableCell(new Paragraph(new Run("Your Productivity Report"))));
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
                    newTableRow.Cells.Add(new TableCell(new Paragraph(new Run("Transaction Date"))));
                    newTableRow.Cells.Add(new TableCell(new Paragraph(new Run("Project ID"))));
                    newTableRow.Cells.Add(new TableCell(new Paragraph(new Run("Project Name"))));
                    newTableRow.Cells.Add(new TableCell(new Paragraph(new Run("Task ID"))));
                    newTableRow.Cells.Add(new TableCell(new Paragraph(new Run("Task"))));
                    newTableRow.Cells.Add(new TableCell(new Paragraph(new Run("Total Hours"))));

                    //Format Header Row
                    for (intCounter = 0; intCounter < intColumns; intCounter++)
                    {
                        newTableRow.Cells[intCounter].FontSize = 11;
                        newTableRow.Cells[intCounter].FontFamily = new FontFamily("Times New Roman");
                        newTableRow.Cells[intCounter].BorderBrush = Brushes.Black;
                        newTableRow.Cells[intCounter].TextAlignment = TextAlignment.Center;
                        newTableRow.Cells[intCounter].BorderThickness = new Thickness();
                    }

                    intNumberOfRecords = TheFindDesignEmployeeProductivityByDateRangeDataSet.FindDesignEmployeeProductivityByDateRange.Rows.Count;

                    //Data, Format Data

                    for (int intReportRowCounter = 0; intReportRowCounter < intNumberOfRecords; intReportRowCounter++)
                    {
                        cancelledTable.RowGroups[0].Rows.Add(new TableRow());
                        intCurrentRow++;
                        newTableRow = cancelledTable.RowGroups[0].Rows[intCurrentRow];
                        for (int intColumnCounter = 0; intColumnCounter < intColumns; intColumnCounter++)
                        {
                            newTableRow.Cells.Add(new TableCell(new Paragraph(new Run(TheFindDesignEmployeeProductivityByDateRangeDataSet.FindDesignEmployeeProductivityByDateRange[intReportRowCounter][intColumnCounter].ToString()))));


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
                    pdCancelledReport.PrintDocument(((IDocumentPaginatorSource)fdCancelledLines).DocumentPaginator, "My Productivity Report");
                    intCurrentRow = 0;

                }
            }
            catch (Exception Ex)
            {
                TheMessagesClass.ErrorMessage(Ex.ToString());

                TheEventLogClass.InsertEventLogEntry(DateTime.Now, "Blue Jay Design // My Productivity // Print Menu Item " + Ex.Message);
            }
        }
    }
}
