/* Title:           Open Surveyor Report
 * Date:            3-13-19
 * Author:          Terry Holmes
 * 
 * Description:     Open Surveyor Report Window */

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
using DesignProjectSurveyorDLL;
using NewEmployeeDLL;
using Microsoft.Win32;

namespace BlueJayDesign
{
    /// <summary>
    /// Interaction logic for OpenSurveyorReport.xaml
    /// </summary>
    public partial class OpenSurveyorReport : Window
    {
        //setting up the classes
        WPFMessagesClass TheMessagesClass = new WPFMessagesClass();
        EventLogClass TheEventLogClass = new EventLogClass();
        DesignProjectsSurveyorClass TheDesignProjectsSurveyorClass = new DesignProjectsSurveyorClass();
        EmployeeClass TheEmployeeClass = new EmployeeClass();

        FindOpenDesignProjectSurveyorRecordsDataSet TheFindOpenDesignProjectSurveyorRecordsDataSet = new FindOpenDesignProjectSurveyorRecordsDataSet();
        OpenWOVReportDataSet TheOpenWOVReportDataSet = new OpenWOVReportDataSet();
        FindEmployeeByEmployeeIDDataSet TheFindEmployeeByEmployeeIDDataSet = new FindEmployeeByEmployeeIDDataSet();

        public OpenSurveyorReport()
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
            int intCounter;
            int intNumberOfRecords;
            DateTime datTodaysDate = DateTime.Now;
            DateTime datTransactionDate = DateTime.Now;
            decimal decTotalHours;
            int intWarehouseID;
            string strAssignedOffice;

            try
            {
                TheOpenWOVReportDataSet.openwov.Rows.Clear();

                TheFindOpenDesignProjectSurveyorRecordsDataSet = TheDesignProjectsSurveyorClass.FindOpenDesignProjectSurveyorRecords();

                intNumberOfRecords = TheFindOpenDesignProjectSurveyorRecordsDataSet.FindOpenDesignProjectSurveyorRecords.Rows.Count - 1;

                for (intCounter = 0; intCounter <= intNumberOfRecords; intCounter++)
                {
                    intWarehouseID = TheFindOpenDesignProjectSurveyorRecordsDataSet.FindOpenDesignProjectSurveyorRecords[intCounter].OfficeID;

                    TheFindEmployeeByEmployeeIDDataSet = TheEmployeeClass.FindEmployeeByEmployeeID(intWarehouseID);

                    strAssignedOffice = TheFindEmployeeByEmployeeIDDataSet.FindEmployeeByEmployeeID[0].FirstName;

                    datTransactionDate = TheFindOpenDesignProjectSurveyorRecordsDataSet.FindOpenDesignProjectSurveyorRecords[intCounter].DateAssigned;

                    decTotalHours = Convert.ToDecimal((datTodaysDate - datTransactionDate).TotalHours);

                    decTotalHours = Math.Round(decTotalHours, 2);

                    OpenWOVReportDataSet.openwovRow NewWOV = TheOpenWOVReportDataSet.openwov.NewopenwovRow();

                    NewWOV.AssignedOffice = strAssignedOffice;
                    NewWOV.AssignedSurveyor = TheFindOpenDesignProjectSurveyorRecordsDataSet.FindOpenDesignProjectSurveyorRecords[intCounter].FullName;
                    NewWOV.DateAssigned = TheFindOpenDesignProjectSurveyorRecordsDataSet.FindOpenDesignProjectSurveyorRecords[intCounter].DateAssigned;
                    NewWOV.ProjectID = TheFindOpenDesignProjectSurveyorRecordsDataSet.FindOpenDesignProjectSurveyorRecords[intCounter].AssignedProjectID;
                    NewWOV.ProjectName = TheFindOpenDesignProjectSurveyorRecordsDataSet.FindOpenDesignProjectSurveyorRecords[intCounter].ProjectName;
                    NewWOV.Status = TheFindOpenDesignProjectSurveyorRecordsDataSet.FindOpenDesignProjectSurveyorRecords[intCounter].WOVStatus;
                    NewWOV.SurveyorNotes = TheFindOpenDesignProjectSurveyorRecordsDataSet.FindOpenDesignProjectSurveyorRecords[intCounter].SurveyorNotes;
                    NewWOV.TimeOpened = decTotalHours;

                    TheOpenWOVReportDataSet.openwov.Rows.Add(NewWOV);
                }

                dgrResults.ItemsSource = TheOpenWOVReportDataSet.openwov;
            }
            catch (Exception Ex)
            {
                TheEventLogClass.InsertEventLogEntry(DateTime.Now, "Blue Jay Design // Open Surveyor Report // Reset Controls " + Ex.Message);

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
                intRowNumberOfRecords = TheOpenWOVReportDataSet.openwov.Rows.Count;
                intColumnNumberOfRecords = TheOpenWOVReportDataSet.openwov.Columns.Count;

                for (intColumnCounter = 0; intColumnCounter < intColumnNumberOfRecords; intColumnCounter++)
                {
                    worksheet.Cells[cellRowIndex, cellColumnIndex] = TheOpenWOVReportDataSet.openwov.Columns[intColumnCounter].ColumnName;

                    cellColumnIndex++;
                }

                cellRowIndex++;
                cellColumnIndex = 1;

                //Loop through each row and read value from each column. 
                for (intRowCounter = 0; intRowCounter < intRowNumberOfRecords; intRowCounter++)
                {
                    for (intColumnCounter = 0; intColumnCounter < intColumnNumberOfRecords; intColumnCounter++)
                    {
                        worksheet.Cells[cellRowIndex, cellColumnIndex] = TheOpenWOVReportDataSet.openwov.Rows[intRowCounter][intColumnCounter].ToString();

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
                TheEventLogClass.InsertEventLogEntry(DateTime.Now, "Blue Jay Design // Open Surveyor Report // Export to Excel " + ex.Message);

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
                    intColumns = TheOpenWOVReportDataSet.openwov.Columns.Count;

                    for (int intColumnCounter = 0; intColumnCounter < intColumns; intColumnCounter++)
                    {
                        cancelledTable.Columns.Add(new TableColumn());
                    }
                    cancelledTable.RowGroups.Add(new TableRowGroup());

                    //Title row
                    cancelledTable.RowGroups[0].Rows.Add(new TableRow());
                    TableRow newTableRow = cancelledTable.RowGroups[0].Rows[intCurrentRow];
                    newTableRow.Cells.Add(new TableCell(new Paragraph(new Run("Open WOV Report"))));
                    newTableRow.Cells[0].FontSize = 20;
                    newTableRow.Cells[0].FontFamily = new FontFamily("Times New Roman");
                    newTableRow.Cells[0].ColumnSpan = intColumns;
                    newTableRow.Cells[0].TextAlignment = TextAlignment.Center;
                    newTableRow.Cells[0].Padding = new Thickness(0, 0, 0, 20);

                    //Header Row
                    cancelledTable.RowGroups[0].Rows.Add(new TableRow());
                    intCurrentRow++;
                    newTableRow = cancelledTable.RowGroups[0].Rows[intCurrentRow];
                    newTableRow.Cells.Add(new TableCell(new Paragraph(new Run("Date Assigned"))));
                    newTableRow.Cells.Add(new TableCell(new Paragraph(new Run("Project ID"))));
                    newTableRow.Cells.Add(new TableCell(new Paragraph(new Run("Project Name"))));
                    newTableRow.Cells.Add(new TableCell(new Paragraph(new Run("Assigned Surveyor"))));
                    newTableRow.Cells.Add(new TableCell(new Paragraph(new Run("Assigned Office"))));
                    newTableRow.Cells.Add(new TableCell(new Paragraph(new Run("Status"))));
                    newTableRow.Cells.Add(new TableCell(new Paragraph(new Run("Time Opened"))));
                    newTableRow.Cells.Add(new TableCell(new Paragraph(new Run("Surveyor Notes"))));

                    //Format Header Row
                    for (intCounter = 0; intCounter < intColumns; intCounter++)
                    {
                        newTableRow.Cells[intCounter].FontSize = 11;
                        newTableRow.Cells[intCounter].FontFamily = new FontFamily("Times New Roman");
                        newTableRow.Cells[intCounter].BorderBrush = Brushes.Black;
                        newTableRow.Cells[intCounter].TextAlignment = TextAlignment.Center;
                        newTableRow.Cells[intCounter].BorderThickness = new Thickness();
                    }

                    intNumberOfRecords = TheOpenWOVReportDataSet.openwov.Rows.Count;

                    //Data, Format Data

                    for (int intReportRowCounter = 0; intReportRowCounter < intNumberOfRecords; intReportRowCounter++)
                    {
                        cancelledTable.RowGroups[0].Rows.Add(new TableRow());
                        intCurrentRow++;
                        newTableRow = cancelledTable.RowGroups[0].Rows[intCurrentRow];
                        for (int intColumnCounter = 0; intColumnCounter < intColumns; intColumnCounter++)
                        {
                            newTableRow.Cells.Add(new TableCell(new Paragraph(new Run(TheOpenWOVReportDataSet.openwov[intReportRowCounter][intColumnCounter].ToString()))));


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
                    pdCancelledReport.PrintDocument(((IDocumentPaginatorSource)fdCancelledLines).DocumentPaginator, "Open WOV Report");
                    intCurrentRow = 0;

                }
            }
            catch (Exception Ex)
            {
                TheMessagesClass.ErrorMessage(Ex.ToString());

                TheEventLogClass.InsertEventLogEntry(DateTime.Now, "Blue Jay Design // Open Surveyor Report // Print Button " + Ex.Message);
            }
        }
    }
}
