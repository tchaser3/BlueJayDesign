/* Title:           Blue Jay Design Appplication
 * Date:            1-23-19
 * Author:          Terry Holmes
 * 
 * Description:     This is the program for Design */

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
using System.Windows.Navigation;
using System.Windows.Shapes;
using NewEmployeeDLL;
using NewEventLogDLL;
using DataValidationDLL;
using AssignedTasksDLL;
using System.Windows.Threading;

namespace BlueJayDesign
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //setting up classes
        WPFMessagesClass TheMessagesClass = new WPFMessagesClass();
        EmployeeClass TheEmployeeClass = new EmployeeClass();
        EventLogClass TheEventLogClass = new EventLogClass();
        DataValidationClass TheDataValidationClass = new DataValidationClass();
        SendEmailClass TheSendEmailClass = new SendEmailClass();
        AssignedTaskClass TheAssignedTaskClass = new AssignedTaskClass();

        //setting up data
        public static VerifyDesignEmployeeLogonDataSet TheVerifyDesignEmployeeLogonDataSet = new VerifyDesignEmployeeLogonDataSet();
        public static FindWarehousesDataSet TheFindWarehousesDataSet = new FindWarehousesDataSet();
        public static FindDesignEmployeesDataSet TheFindDesignEmployeesDataSet = new FindDesignEmployeesDataSet();
        public static EmployeeEmailDataSet TheEmployeeEmailDataSet = new EmployeeEmailDataSet();
        public static FindAssignedTasksByAssignedEmployeeIDDataSet TheFindAssignedTasksByAssignedEmployeeIDDataSet = new FindAssignedTasksByAssignedEmployeeIDDataSet();
        public static ComboEmployeeDataSet TheComboEmployeeDataSet = new ComboEmployeeDataSet();
        public static FindAssignedTasksByDateMatchDataSet TheFindAssignedTaskByDateMatchDataSet = new FindAssignedTasksByDateMatchDataSet();
        public static FindEmployeeEmailAddressDataSet TheFindEmployeeEmailAddressDataSet = new FindEmployeeEmailAddressDataSet();

        //setting up variables
        public static int gintEmployeeID;
        public static string gstrFirstName;
        public static string gstrLastName;
        public static int gintProjectID;
        public static string gstrAssignedProjectID;
        public static string gstrProjectName;
        public static DateTime gdatStartDate;
        public static DateTime gdatEndDate;
        public static int gintWarehouseID;
        public static string gstrHomeOffice;
        public static int gintTransactionID;
        public static DateTime gdatTransactionDate;
        int gintNumberOfAssignedTasks;
        public static int gintAssignedTaskID;
        public static int gintJobTypeID;
        public static int gintWorkTaskID;
        public static string gstrCity;
        public static int gintJobType;
        public static string gstrZipCode;
        public static string gstrState;
        public static string gstrAddress;

        //Setting Windows
        public static SendEmail SendEmailWindow = new SendEmail();
        public static UpdateAssignTask UpdateAssignTasksWindow = new UpdateAssignTask();
        public static AssignTasks AssignTasksWindow = new AssignTasks();
        public static AddNewProject AddNewProjectWindow = new AddNewProject();
        public static AssignSurveyor AssignSurveyorWindow = new AssignSurveyor();
        public static UpdateSurvey UpdateSurveyWindow = new UpdateSurvey();
        public static UpdateProject UpdateProjectWindow = new UpdateProject();
        public static CloseProject CloseProjectWindow = new CloseProject();
        public static EmployeeProductivity EmployeeProductivityWindow = new EmployeeProductivity();
        public static OpenDesignProjects OpenDesignProjectWindow = new OpenDesignProjects();
        public static DesignProjectReport DesignProjectReportwindow = new DesignProjectReport();
        public static PolePermitReport PolePermitReportWindow = new PolePermitReport();
        public static OpenSurveyorReport OpenSurveyorReportWindow = new OpenSurveyorReport();
        public static ViewProjectInfo ViewProjectInfoWindow = new ViewProjectInfo();

        DispatcherTimer MyTimer = new DispatcherTimer();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            TheMessagesClass.CloseTheProgram();
        }

        private void BtnHome_Click(object sender, RoutedEventArgs e)
        {
            ResetControls();
        }
        private void ResetControls()
        {
            TheEmployeeEmailDataSet.employeeemail.Rows.Clear();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                //ResetControls();
                EmployeeLogon EmployeeLogon = new EmployeeLogon();
                EmployeeLogon.ShowDialog();

                TheFindAssignedTasksByAssignedEmployeeIDDataSet = TheAssignedTaskClass.FindAssignedTasksByAssignedEmployeeID(TheVerifyDesignEmployeeLogonDataSet.VerifyDesigEmployeeLogon[0].EmployeeID);

                gintNumberOfAssignedTasks = TheFindAssignedTaskByDateMatchDataSet.FindAssignedTasksByDateMatch.Rows.Count;

                if (gintNumberOfAssignedTasks > 0)
                {
                    TheMessagesClass.InformationMessage("You Have Open Assigned Tasks");
                }

                MyTimer.Tick += new EventHandler(BeginTheProcess);
                MyTimer.Interval = new TimeSpan(0, 0, 30);
                MyTimer.Start();
            }
            catch (Exception Ex)
            {
                TheEventLogClass.InsertEventLogEntry(DateTime.Now, "Blue Jay Design // Main Window // Window Loaded " + Ex.Message);

                TheMessagesClass.ErrorMessage(Ex.ToString());
            }
            
        }
        private void BeginTheProcess(object sender, EventArgs e)
        {
            int intHours;
            int intMinutes;
            int intNumberOfRecords;

            DateTime datTodaysDate = DateTime.Now;

            intHours = datTodaysDate.Hour;
            intMinutes = datTodaysDate.Minute;

            if ((intHours >= 1) && (intHours < 4))
            {
                Application.Current.Shutdown();
            }
            if (intHours < 6)
            {
                TheMessagesClass.ErrorMessage("The Program Will Not Be Available until 6:30 am");
                Application.Current.Shutdown();
            }
            else if ((intHours == 6) && (intMinutes < 30))
            {
                TheMessagesClass.ErrorMessage("The Program Will Not Be Available until 6:30 am");
                Application.Current.Shutdown();
            }

            TheFindAssignedTasksByAssignedEmployeeIDDataSet = TheAssignedTaskClass.FindAssignedTasksByAssignedEmployeeID(TheVerifyDesignEmployeeLogonDataSet.VerifyDesigEmployeeLogon[0].EmployeeID);

            intNumberOfRecords = TheFindAssignedTasksByAssignedEmployeeIDDataSet.FindAssignedTasksByAssignedEmployeeID.Rows.Count;

            if (intNumberOfRecords > gintNumberOfAssignedTasks)
            {
                gintNumberOfAssignedTasks = intNumberOfRecords;
                TheMessagesClass.InformationMessage("A New Task Has Been Assigned");
            }
            else if(intNumberOfRecords < gintNumberOfAssignedTasks)
            {
                gintNumberOfAssignedTasks = intNumberOfRecords;
            }
            
        }

        private void BtnHelp_Click(object sender, RoutedEventArgs e)
        {
            TheMessagesClass.LaunchHelpSite();
        }

        private void BtnEmail_Click(object sender, RoutedEventArgs e)
        {
            SendEmailWindow.Visibility = Visibility.Visible;
            
        }

        private void BtnAddNewProject_Click(object sender, RoutedEventArgs e)
        {
            AddNewProjectWindow.Visibility = Visibility.Visible;
        }


        private void BtnMyTasks_Click(object sender, RoutedEventArgs e)
        {
            UpdateAssignTasksWindow.Visibility = Visibility.Visible;
        }

        private void BtnAssignTask_Click(object sender, RoutedEventArgs e)
        {
            AssignTasksWindow.Visibility = Visibility.Visible;
        }

        private void BtnAssignSurveyor_Click(object sender, RoutedEventArgs e)
        {
            AssignSurveyorWindow.Visibility = Visibility.Visible;
        }

        private void BtnUpdateSurvey_Click(object sender, RoutedEventArgs e)
        {
            UpdateSurveyWindow.Visibility = Visibility.Visible;
        }

        private void BtnUpdateProject_Click(object sender, RoutedEventArgs e)
        {
            UpdateProjectWindow.Visibility = Visibility.Visible;
        }

        private void BtnCloseProject_Click(object sender, RoutedEventArgs e)
        {
            CloseProjectWindow.Visibility = Visibility.Visible;
        }

        private void BtnEmployeeProductivity_Click(object sender, RoutedEventArgs e)
        {
            EmployeeProductivityWindow.Visibility = Visibility.Visible;
        }

        private void BtnOpenDesignProjects_Click(object sender, RoutedEventArgs e)
        {
            OpenDesignProjectWindow.Visibility = Visibility.Visible;
        }

        private void BtnDesignProjectReport_Click(object sender, RoutedEventArgs e)
        {
            DesignProjectReportwindow.Visibility = Visibility.Visible;
        }

        private void BtnPolePermitReport_Click(object sender, RoutedEventArgs e)
        {
            PolePermitReportWindow.Visibility = Visibility.Visible;
        }

        private void BtnOpenSurveryorReport_Click(object sender, RoutedEventArgs e)
        {
            OpenSurveyorReportWindow.Visibility = Visibility.Visible;
        }

        private void Window_Loaded_1(object sender, RoutedEventArgs e)
        {
            ResetControls();
            EmployeeLogon EmployeeLogon = new EmployeeLogon();
            EmployeeLogon.ShowDialog();

            TheFindAssignedTasksByAssignedEmployeeIDDataSet = TheAssignedTaskClass.FindAssignedTasksByAssignedEmployeeID(TheVerifyDesignEmployeeLogonDataSet.VerifyDesigEmployeeLogon[0].EmployeeID);

            gintNumberOfAssignedTasks = TheFindAssignedTaskByDateMatchDataSet.FindAssignedTasksByDateMatch.Rows.Count;

            if (gintNumberOfAssignedTasks > 0)
            {
                TheMessagesClass.InformationMessage("You Have Open Assigned Tasks");
            }

            MyTimer.Tick += new EventHandler(BeginTheProcess);
            MyTimer.Interval = new TimeSpan(0, 0, 30);
            MyTimer.Start();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void BtnViewProjectInfo_Click(object sender, RoutedEventArgs e)
        {
            ViewProjectInfoWindow.Visibility = Visibility.Visible;
        }
    }
}
