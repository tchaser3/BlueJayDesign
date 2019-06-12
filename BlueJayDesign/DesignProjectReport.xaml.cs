/* Title:           Design Project Report
 * Date:            2-22-19
 * Author:          Terry Holmes
 * 
 * Description:     This is used for design project reports */

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
using DesignProjectsDLL;
using Microsoft.Win32;
using NewEmployeeDLL;
using NewEventLogDLL;
using System;

namespace BlueJayDesign
{
    /// <summary>
    /// Interaction logic for DesignProjectReport.xaml
    /// </summary>
    public partial class DesignProjectReport : Window
    {
        //setting up the classes
        WPFMessagesClass TheMessagesClass = new WPFMessagesClass();
        EventLogClass TheEventLogClass = new EventLogClass();
        EmployeeClass TheEmployeeClass = new EmployeeClass();
        DesignProjectsClass TheDesignProjectsClass = new DesignProjectsClass();
        DataValidationClass TheDataValidationClass = new DataValidationClass();

        //setting up the data
        FindDesignProjectsByDateRangeDataSet TheFindDesignProjectsByDateRangeDataSet = new FindDesignProjectsByDateRangeDataSet();
        OpenProjectsDataSet TheOpenProjectsDataSet = new OpenProjectsDataSet();
        FindDesignProjectsByLocationDataSet TheFindDesignProjectsByLocationDataSet = new FindDesignProjectsByLocationDataSet();

        public DesignProjectReport()
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
            //setting up local variables
            int intCounter;
            int intNumberOfRecords;

            //setting up the combo box
            cboSelectReportType.Items.Clear();
            cboSelectReportType.Items.Add("Select Report Type");
            cboSelectReportType.Items.Add("Date Search Report");
            cboSelectReportType.Items.Add("Location Report");
            cboSelectReportType.Items.Add("Detailed Location Report");
            cboSelectReportType.SelectedIndex = 0;

            cboSelectLocation.Items.Clear();
            cboSelectLocation.Items.Add("Select Location");

            MainWindow.TheFindWarehousesDataSet = TheEmployeeClass.FindWarehouses();

            intNumberOfRecords = MainWindow.TheFindWarehousesDataSet.FindWarehouses.Rows.Count - 1;

            for(intCounter = 0; intCounter <= intNumberOfRecords; intCounter++)
            {
                cboSelectLocation.Items.Add(MainWindow.TheFindWarehousesDataSet.FindWarehouses[intCounter].FirstName);
            }

            cboSelectLocation.SelectedIndex = 0;

            txtEndDate.Text = "";
            txtStartDate.Text = "";

            EnableControls(false);

            TheOpenProjectsDataSet.projects.Rows.Clear();

            dgrResults.ItemsSource = TheOpenProjectsDataSet.projects;

        }
        private void EnableControls(bool valueBoolean)
        {
            cboSelectLocation.IsEnabled = valueBoolean;
            txtStartDate.IsEnabled = valueBoolean;
            txtEndDate.IsEnabled = valueBoolean;
            btnSearch.IsEnabled = valueBoolean;
            btnPrint.IsEnabled = valueBoolean;
            btnExportToExcel.IsEnabled = valueBoolean;
        }

        private void CboSelectReportType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int intSelectedIndex;
            EnableControls(false);

            try
            {
                intSelectedIndex = cboSelectReportType.SelectedIndex;

                if(intSelectedIndex == 1)
                {
                    txtEndDate.IsEnabled = true;
                    txtStartDate.IsEnabled = true;
                    btnSearch.IsEnabled = true;
                }
                if(intSelectedIndex == 2)
                {
                    txtEndDate.IsEnabled = true;
                    txtStartDate.IsEnabled = true;
                    btnSearch.IsEnabled = true;
                    cboSelectLocation.IsEnabled = true;
                }
                if(intSelectedIndex == 3)
                {
                    DetailedDesignReport DetailedDesignReport = new DetailedDesignReport();
                    DetailedDesignReport.ShowDialog();
                }
            }
            catch (Exception Ex)
            {
                TheEventLogClass.InsertEventLogEntry(DateTime.Now, "Blue Jay Design // Design Project Report // combo selection " + Ex.Message);

                TheMessagesClass.ErrorMessage(Ex.ToString());
            }
        }

        private void BtnSearch_Click(object sender, RoutedEventArgs e)
        {
            //setting local variables
            int intCounter;
            int intNumberOfRecords;
            string strValueForValidation;
            string strErrorMessage = "";
            bool blnFatalError = false;
            bool blnThereIsAProblem = false;
            DateTime datCloseDate;
            DateTime datReceivedDate = DateTime.Now;
            decimal decHoursOpen;

            try
            {
                TheOpenProjectsDataSet.projects.Rows.Clear();

                if(cboSelectReportType.SelectedIndex == 1)
                {
                    //data validation
                    strValueForValidation = txtStartDate.Text;
                    blnThereIsAProblem = TheDataValidationClass.VerifyDateData(strValueForValidation);
                    if (blnThereIsAProblem == true)
                    {
                        blnFatalError = true;
                        strErrorMessage += "The Start Date is not a Date\n";
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
                        strErrorMessage += "The End Date is not a Date\n";
                    }
                    else
                    {
                        MainWindow.gdatEndDate = Convert.ToDateTime(strValueForValidation);
                    }
                    if (blnFatalError == true)
                    {
                        TheMessagesClass.ErrorMessage(strErrorMessage);
                        return;
                    }

                    TheFindDesignProjectsByDateRangeDataSet = TheDesignProjectsClass.FindDesignProjectsByDateRange(MainWindow.gdatStartDate, MainWindow.gdatEndDate);

                    intNumberOfRecords = TheFindDesignProjectsByDateRangeDataSet.FindDesignProjectsByDateRange.Rows.Count - 1;

                    if(intNumberOfRecords > -1)
                    {
                        for(intCounter = 0; intCounter <= intNumberOfRecords; intCounter++)
                        {
                            datReceivedDate = TheFindDesignProjectsByDateRangeDataSet.FindDesignProjectsByDateRange[intCounter].DateReceived;

                            if(TheFindDesignProjectsByDateRangeDataSet.FindDesignProjectsByDateRange[intCounter].JobStatus == "CLOSED")
                            {
                                datCloseDate = TheFindDesignProjectsByDateRangeDataSet.FindDesignProjectsByDateRange[intCounter].CompleteDate;
                            }
                            else
                            {
                                datCloseDate = DateTime.Now;
                            }

                            decHoursOpen = Convert.ToDecimal((datCloseDate - datReceivedDate).TotalHours);

                            decHoursOpen = Math.Round(decHoursOpen, 2);

                            OpenProjectsDataSet.projectsRow NewProjectRow = TheOpenProjectsDataSet.projects.NewprojectsRow();

                            NewProjectRow.AssignedOffice = TheFindDesignProjectsByDateRangeDataSet.FindDesignProjectsByDateRange[intCounter].FirstName;
                            NewProjectRow.Coordinator = TheFindDesignProjectsByDateRangeDataSet.FindDesignProjectsByDateRange[intCounter].Coordinator;
                            NewProjectRow.DateReceived = TheFindDesignProjectsByDateRangeDataSet.FindDesignProjectsByDateRange[intCounter].DateReceived;
                            NewProjectRow.HoursOpen = decHoursOpen;
                            NewProjectRow.ProjectID = TheFindDesignProjectsByDateRangeDataSet.FindDesignProjectsByDateRange[intCounter].AssignedProjectID;
                            NewProjectRow.ProjectName = TheFindDesignProjectsByDateRangeDataSet.FindDesignProjectsByDateRange[intCounter].ProjectName;
                            NewProjectRow.ProjectStatus = TheFindDesignProjectsByDateRangeDataSet.FindDesignProjectsByDateRange[intCounter].JobStatus;

                            TheOpenProjectsDataSet.projects.Rows.Add(NewProjectRow);
                        }
                    }
                }
                if(cboSelectReportType.SelectedIndex == 2)
                {
                    //data validation
                    strValueForValidation = txtStartDate.Text;
                    blnThereIsAProblem = TheDataValidationClass.VerifyDateData(strValueForValidation);
                    if (blnThereIsAProblem == true)
                    {
                        blnFatalError = true;
                        strErrorMessage += "The Start Date is not a Date\n";
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
                        strErrorMessage += "The End Date is not a Date\n";
                    }
                    else
                    {
                        MainWindow.gdatEndDate = Convert.ToDateTime(strValueForValidation);
                    }
                    if(cboSelectLocation.SelectedIndex < 1)
                    {
                        blnFatalError = true;
                        strErrorMessage += "The Location Was not Selected\n";
                    }
                    if (blnFatalError == true)
                    {
                        TheMessagesClass.ErrorMessage(strErrorMessage);
                        return;
                    }

                    TheFindDesignProjectsByLocationDataSet = TheDesignProjectsClass.FindDesignProjectsByLocation(MainWindow.gintWarehouseID, MainWindow.gdatStartDate, MainWindow.gdatEndDate);

                    intNumberOfRecords = TheFindDesignProjectsByLocationDataSet.FindDesignProjectsByLocation.Rows.Count - 1;

                    if (intNumberOfRecords > -1)
                    {
                        for (intCounter = 0; intCounter <= intNumberOfRecords; intCounter++)
                        {
                            datReceivedDate = TheFindDesignProjectsByLocationDataSet.FindDesignProjectsByLocation[intCounter].DateReceived;

                            if (TheFindDesignProjectsByLocationDataSet.FindDesignProjectsByLocation[intCounter].JobStatus == "CLOSED")
                            {
                                datCloseDate = TheFindDesignProjectsByLocationDataSet.FindDesignProjectsByLocation[intCounter].CompleteDate;
                            }
                            else
                            {
                                datCloseDate = DateTime.Now;
                            }

                            decHoursOpen = Convert.ToDecimal((datCloseDate - datReceivedDate).TotalHours);

                            decHoursOpen = Math.Round(decHoursOpen, 2);

                            OpenProjectsDataSet.projectsRow NewProjectRow = TheOpenProjectsDataSet.projects.NewprojectsRow();

                            NewProjectRow.AssignedOffice = TheFindDesignProjectsByLocationDataSet.FindDesignProjectsByLocation[intCounter].FirstName;
                            NewProjectRow.Coordinator = TheFindDesignProjectsByLocationDataSet.FindDesignProjectsByLocation[intCounter].Coordinator;
                            NewProjectRow.DateReceived = TheFindDesignProjectsByLocationDataSet.FindDesignProjectsByLocation[intCounter].DateReceived;
                            NewProjectRow.HoursOpen = decHoursOpen;
                            NewProjectRow.ProjectID = TheFindDesignProjectsByLocationDataSet.FindDesignProjectsByLocation[intCounter].AssignedProjectID;
                            NewProjectRow.ProjectName = TheFindDesignProjectsByLocationDataSet.FindDesignProjectsByLocation[intCounter].ProjectName;
                            NewProjectRow.ProjectStatus = TheFindDesignProjectsByLocationDataSet.FindDesignProjectsByLocation[intCounter].JobStatus;

                            TheOpenProjectsDataSet.projects.Rows.Add(NewProjectRow);
                        }
                    }
                }

                dgrResults.ItemsSource = TheOpenProjectsDataSet.projects;
                btnPrint.IsEnabled = true;
                btnExportToExcel.IsEnabled = true;
            }
            catch (Exception Ex)
            {
                TheEventLogClass.InsertEventLogEntry(DateTime.Now, "Blue Jay Design // Design Project Report // Search Button " + Ex.Message);

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
                MainWindow.gstrHomeOffice = MainWindow.TheFindWarehousesDataSet.FindWarehouses[intSelectedIndex].FirstName;
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
                intRowNumberOfRecords = TheOpenProjectsDataSet.projects.Rows.Count;
                intColumnNumberOfRecords = TheOpenProjectsDataSet.projects.Columns.Count;

                for (intColumnCounter = 0; intColumnCounter < intColumnNumberOfRecords; intColumnCounter++)
                {
                    worksheet.Cells[cellRowIndex, cellColumnIndex] = TheOpenProjectsDataSet.projects.Columns[intColumnCounter].ColumnName;

                    cellColumnIndex++;
                }

                cellRowIndex++;
                cellColumnIndex = 1;

                //Loop through each row and read value from each column. 
                for (intRowCounter = 0; intRowCounter < intRowNumberOfRecords; intRowCounter++)
                {
                    for (intColumnCounter = 0; intColumnCounter < intColumnNumberOfRecords; intColumnCounter++)
                    {
                        worksheet.Cells[cellRowIndex, cellColumnIndex] = TheOpenProjectsDataSet.projects.Rows[intRowCounter][intColumnCounter].ToString();

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
                TheEventLogClass.InsertEventLogEntry(DateTime.Now, "Blue Jay Design // Design Project Report // Export to Excel " + ex.Message);

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
                    intColumns = TheOpenProjectsDataSet.projects.Columns.Count;

                    for (int intColumnCounter = 0; intColumnCounter < intColumns; intColumnCounter++)
                    {
                        cancelledTable.Columns.Add(new TableColumn());
                    }
                    cancelledTable.RowGroups.Add(new TableRowGroup());

                    //Title row
                    cancelledTable.RowGroups[0].Rows.Add(new TableRow());
                    TableRow newTableRow = cancelledTable.RowGroups[0].Rows[intCurrentRow];
                    newTableRow.Cells.Add(new TableCell(new Paragraph(new Run("Open Design Project Report"))));
                    newTableRow.Cells[0].FontSize = 20;
                    newTableRow.Cells[0].FontFamily = new FontFamily("Times New Roman");
                    newTableRow.Cells[0].ColumnSpan = intColumns;
                    newTableRow.Cells[0].TextAlignment = TextAlignment.Center;
                    newTableRow.Cells[0].Padding = new Thickness(0, 0, 0, 20);

                    //Header Row
                    cancelledTable.RowGroups[0].Rows.Add(new TableRow());
                    intCurrentRow++;
                    newTableRow = cancelledTable.RowGroups[0].Rows[intCurrentRow];
                    newTableRow.Cells.Add(new TableCell(new Paragraph(new Run("Date Received"))));
                    newTableRow.Cells.Add(new TableCell(new Paragraph(new Run("Hours Open"))));
                    newTableRow.Cells.Add(new TableCell(new Paragraph(new Run("Project ID"))));
                    newTableRow.Cells.Add(new TableCell(new Paragraph(new Run("Project Name"))));
                    newTableRow.Cells.Add(new TableCell(new Paragraph(new Run("Project Status"))));
                    newTableRow.Cells.Add(new TableCell(new Paragraph(new Run("Assigned Office"))));
                    newTableRow.Cells.Add(new TableCell(new Paragraph(new Run("Coordinator"))));

                    //Format Header Row
                    for (intCounter = 0; intCounter < intColumns; intCounter++)
                    {
                        newTableRow.Cells[intCounter].FontSize = 11;
                        newTableRow.Cells[intCounter].FontFamily = new FontFamily("Times New Roman");
                        newTableRow.Cells[intCounter].BorderBrush = Brushes.Black;
                        newTableRow.Cells[intCounter].TextAlignment = TextAlignment.Center;
                        newTableRow.Cells[intCounter].BorderThickness = new Thickness();
                    }

                    intNumberOfRecords = TheOpenProjectsDataSet.projects.Rows.Count;

                    //Data, Format Data

                    for (int intReportRowCounter = 0; intReportRowCounter < intNumberOfRecords; intReportRowCounter++)
                    {
                        cancelledTable.RowGroups[0].Rows.Add(new TableRow());
                        intCurrentRow++;
                        newTableRow = cancelledTable.RowGroups[0].Rows[intCurrentRow];
                        for (int intColumnCounter = 0; intColumnCounter < intColumns; intColumnCounter++)
                        {
                            newTableRow.Cells.Add(new TableCell(new Paragraph(new Run(TheOpenProjectsDataSet.projects[intReportRowCounter][intColumnCounter].ToString()))));


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
                    pdCancelledReport.PrintDocument(((IDocumentPaginatorSource)fdCancelledLines).DocumentPaginator, "Open Design Project Report");
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
