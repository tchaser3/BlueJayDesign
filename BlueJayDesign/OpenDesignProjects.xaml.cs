/* Title:           Open Design Projects Report 
 * Date:            2-20-19
 * Author:          Terry Holmes
 * 
 * Description:     This window is used for showing all open designs */

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
using System.Windows.Threading;
using NewEventLogDLL;
using DesignProjectsDLL;
using NewEmployeeDLL;
using Microsoft.Win32;

namespace BlueJayDesign
{
    /// <summary>
    /// Interaction logic for OpenDesignProjects.xaml
    /// </summary>
    public partial class OpenDesignProjects : Window
    {
        //Setting up the classes
        WPFMessagesClass TheMessagesClass = new WPFMessagesClass();
        EventLogClass TheEventLogClass = new EventLogClass();
        DesignProjectsClass TheDesignProjectsClass = new DesignProjectsClass();
        EmployeeClass TheEmployeeClass = new EmployeeClass();

        //setting up the data
        FindOpenDesignProjectsDataSet TheFindOpenDesignProjectsDataSet = new FindOpenDesignProjectsDataSet();
        FindOpenDesignProjectsByLocationDataSet TheFindOpenDesignProjectsByLocationDataSet = new FindOpenDesignProjectsByLocationDataSet();
        OpenProjectsDataSet TheOpenProjectsDataSet = new OpenProjectsDataSet();

        //setting up the timer
        DispatcherTimer MyTimer = new DispatcherTimer();

        public OpenDesignProjects()
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
            LoadLocation();

            ResetControls();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadLocation();

            ResetControls();

            MyTimer.Tick += new EventHandler(BeginTheProcess);
            MyTimer.Interval = new TimeSpan(0, 3, 0);
            MyTimer.Start();
        }
        private void ResetControls()
        {
            int intSelectedIndex;

            intSelectedIndex = cboSelectLocation.SelectedIndex;

            if (intSelectedIndex < 1)
            {
                TheOpenProjectsDataSet.projects.Rows.Clear();

                dgrResults.ItemsSource = TheOpenProjectsDataSet.projects;
            }
            else if (intSelectedIndex == 1)
            {
                LoadAllOpenProjects();
            }
            else if (intSelectedIndex > 1)
            {
                LoadOpenLocationProjects();
            }
            else
            {
                TheMessagesClass.ErrorMessage("The World is Going To End Because This Can't Happen ");
                return;
            }

            dgrResults.ItemsSource = TheOpenProjectsDataSet.projects;
        }
        private void BeginTheProcess(object sender, EventArgs e)
        {
            TheOpenProjectsDataSet.projects.Rows.Clear();

            ResetControls();
        }
        private void LoadAllOpenProjects()
        {
            int intCounter;
            int intNumberOfRecords;
            DateTime datToday = DateTime.Now;
            DateTime datTransactionDate;
            decimal decTotalHours;

            try
            {
                TheFindOpenDesignProjectsDataSet = TheDesignProjectsClass.FindOpenDesignProjects();                

                intNumberOfRecords = TheFindOpenDesignProjectsDataSet.FindOpenDesignProjects.Rows.Count - 1;

                if(intNumberOfRecords > -1)
                {
                    for(intCounter = 0; intCounter <= intNumberOfRecords; intCounter++)
                    {
                        datTransactionDate = TheFindOpenDesignProjectsDataSet.FindOpenDesignProjects[intCounter].DateReceived;

                        decTotalHours = Convert.ToDecimal((datToday - datTransactionDate).TotalHours);

                        decTotalHours = Math.Round(decTotalHours, 2);

                        OpenProjectsDataSet.projectsRow NewProjectRow = TheOpenProjectsDataSet.projects.NewprojectsRow();

                        NewProjectRow.Coordinator = TheFindOpenDesignProjectsDataSet.FindOpenDesignProjects[intCounter].Coordinator;
                        NewProjectRow.DateReceived = datTransactionDate;
                        NewProjectRow.HoursOpen = decTotalHours;
                        NewProjectRow.AssignedOffice = TheFindOpenDesignProjectsDataSet.FindOpenDesignProjects[intCounter].FirstName;
                        NewProjectRow.ProjectID = TheFindOpenDesignProjectsDataSet.FindOpenDesignProjects[intCounter].AssignedProjectID;
                        NewProjectRow.ProjectName = TheFindOpenDesignProjectsDataSet.FindOpenDesignProjects[intCounter].ProjectName;
                        NewProjectRow.ProjectStatus = TheFindOpenDesignProjectsDataSet.FindOpenDesignProjects[intCounter].JobStatus;

                        TheOpenProjectsDataSet.projects.Rows.Add(NewProjectRow);
                    }
                }

                dgrResults.ItemsSource = TheOpenProjectsDataSet.projects;
            }
            catch (Exception Ex)
            {
                TheEventLogClass.InsertEventLogEntry(DateTime.Now, "Blue Jay Design // Open Design Projects // Load All Open Projects " + Ex.Message);

                TheMessagesClass.ErrorMessage(Ex.ToString());
            }            
        }
        private void LoadOpenLocationProjects()
        {
            int intSelectedIndex;
            int intCounter;
            int intNumberOfRecords;
            DateTime datToday = DateTime.Now;
            DateTime datTransactionDate;
            decimal decTotalHours;

            try
            {
                intSelectedIndex = cboSelectLocation.SelectedIndex - 2;

                MainWindow.gintWarehouseID = MainWindow.TheFindWarehousesDataSet.FindWarehouses[intSelectedIndex].EmployeeID;
                MainWindow.gstrHomeOffice = MainWindow.TheFindWarehousesDataSet.FindWarehouses[intSelectedIndex].FirstName;

                TheFindOpenDesignProjectsByLocationDataSet = TheDesignProjectsClass.FindOpenDesignProjectsByLocation(MainWindow.gintWarehouseID);

                intNumberOfRecords = TheFindOpenDesignProjectsByLocationDataSet.FindOpenDesignProjectsByLocation.Rows.Count - 1;

                if(intNumberOfRecords > -1)
                {
                    for(intCounter = 0; intCounter <= intNumberOfRecords; intCounter++)
                    {
                        datTransactionDate = TheFindOpenDesignProjectsByLocationDataSet.FindOpenDesignProjectsByLocation[intCounter].DateReceived;

                        decTotalHours = Convert.ToDecimal((datToday - datTransactionDate).TotalHours);

                        decTotalHours = Math.Round(decTotalHours, 2);

                        OpenProjectsDataSet.projectsRow NewProjectRow = TheOpenProjectsDataSet.projects.NewprojectsRow();

                        NewProjectRow.Coordinator = TheFindOpenDesignProjectsByLocationDataSet.FindOpenDesignProjectsByLocation[intCounter].Coordinator;
                        NewProjectRow.DateReceived = datTransactionDate;
                        NewProjectRow.HoursOpen = decTotalHours;
                        NewProjectRow.AssignedOffice = MainWindow.gstrHomeOffice;
                        NewProjectRow.ProjectID = TheFindOpenDesignProjectsByLocationDataSet.FindOpenDesignProjectsByLocation[intCounter].AssignedProjectID;
                        NewProjectRow.ProjectName = TheFindOpenDesignProjectsByLocationDataSet.FindOpenDesignProjectsByLocation[intCounter].ProjectName;
                        NewProjectRow.ProjectStatus = TheFindOpenDesignProjectsByLocationDataSet.FindOpenDesignProjectsByLocation[intCounter].JobStatus;

                        TheOpenProjectsDataSet.projects.Rows.Add(NewProjectRow);
                    }
                }

                dgrResults.ItemsSource = TheOpenProjectsDataSet.projects;
            }
            catch (Exception Ex)
            {
                TheEventLogClass.InsertEventLogEntry(DateTime.Now, "Blue Jay Design // Open Design Projects // Load Open Location Projects " + Ex.Message);

                TheMessagesClass.ErrorMessage(Ex.ToString());
            }
        }
        private void LoadLocation()
        {
            int intNumberOfRecords;
            int intComputer;

            cboSelectLocation.Items.Clear();
            cboSelectLocation.Items.Add("Select Location");
            cboSelectLocation.Items.Add("All Locations");

            MainWindow.TheFindWarehousesDataSet = TheEmployeeClass.FindWarehouses();

            intNumberOfRecords = MainWindow.TheFindWarehousesDataSet.FindWarehouses.Rows.Count - 1;

            for(intComputer = 0; intComputer <= intNumberOfRecords; intComputer++)
            {
                cboSelectLocation.Items.Add(MainWindow.TheFindWarehousesDataSet.FindWarehouses[intComputer].FirstName);
            }

            cboSelectLocation.SelectedIndex = 0;
        }

        private void CboSelectLocation_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int intSelectedIndex;

            intSelectedIndex = cboSelectLocation.SelectedIndex;

            TheOpenProjectsDataSet.projects.Rows.Clear();

            if (intSelectedIndex < 1)
            {
                dgrResults.ItemsSource = TheOpenProjectsDataSet.projects;
            }
            else if (intSelectedIndex == 1)
            {
                LoadAllOpenProjects();
            }
            else if(intSelectedIndex > 1)
            {
                LoadOpenLocationProjects();
            }
            else
            {
                TheMessagesClass.ErrorMessage("The World is Going To End Because This Can't Happen ");
                return;
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
                TheEventLogClass.InsertEventLogEntry(DateTime.Now, "Blue Jay Design // Open Design Projects // Export to Excel " + ex.Message);

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

                TheEventLogClass.InsertEventLogEntry(DateTime.Now, "Blue Jay Design // Open Design Projects // Print Button " + Ex.Message);
            }
        }
    }
}
