/* Title:           View Project Info
 * Date:            2-8-19
 * Author:          Terry Holmes
 * 
 * Description:     This is used to view the project information */

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
using DesignPermitsDLL;
using DesignProjectDocumentation;
using DesignProjectsDLL;
using DesignProjectSurveyorDLL;
using DesignProjectUpdateDLL;

namespace BlueJayDesign
{
    /// <summary>
    /// Interaction logic for ViewProjectInfo.xaml
    /// </summary>
    public partial class ViewProjectInfo : Window
    {
        //setting up the classes
        WPFMessagesClass TheMessagesClass = new WPFMessagesClass();
        EventLogClass TheEventLogClass = new EventLogClass();
        DesignPermitsClass TheDesignPermitsClass = new DesignPermitsClass();
        DesignProjectDocumentationClass TheDesignProjectDocumentationClass = new DesignProjectDocumentationClass();
        DesignProjectsClass TheDesignProjectsclass = new DesignProjectsClass();
        DesignProjectsSurveyorClass TheDesignProjectSurveyorClass = new DesignProjectsSurveyorClass();
        DesignProjectUpdateClass TheDesignProjectUpdateClass = new DesignProjectUpdateClass();

        //setting up the data
        FindDesignProjectsByAssignedProjectIDDataSet TheFindDesignProjectsByAssignedProjectIDDataSet = new FindDesignProjectsByAssignedProjectIDDataSet();
        FindDesignProjectDocumentationByProjectIDDataSet TheFindProjectDocumentationByProjectIDDataSet = new FindDesignProjectDocumentationByProjectIDDataSet();
        FindDesignProjectUpdatesByProjectIDDataSet TheFindDesignProjectUpdatesByProjectIDDAtaSet = new FindDesignProjectUpdatesByProjectIDDataSet();

        public ViewProjectInfo()
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

        private void BtnSearch_Click(object sender, RoutedEventArgs e)
        {
            int intRecordsReturned;

            try
            {
                btnFindWOV.IsEnabled = false;
                btnFindPermits.IsEnabled = false;
                MainWindow.gstrAssignedProjectID = txtEnterProjectID.Text;
                if(MainWindow.gstrAssignedProjectID == "")
                {
                    TheMessagesClass.ErrorMessage("Project ID Was Not Entered");
                }

                TheFindDesignProjectsByAssignedProjectIDDataSet = TheDesignProjectsclass.FindDesignProjectsByAssignedProjectID(MainWindow.gstrAssignedProjectID);

                intRecordsReturned = TheFindDesignProjectsByAssignedProjectIDDataSet.FindDesignProjectsByAssignedProjectID.Rows.Count;

                if(intRecordsReturned == 0)
                {
                    TheMessagesClass.ErrorMessage("Project Not Found");
                    return;
                }

                txtProjectName.Text = TheFindDesignProjectsByAssignedProjectIDDataSet.FindDesignProjectsByAssignedProjectID[0].ProjectName;
                txtDateReceived.Text = Convert.ToString(TheFindDesignProjectsByAssignedProjectIDDataSet.FindDesignProjectsByAssignedProjectID[0].DateReceived);
                txtAddress.Text = TheFindDesignProjectsByAssignedProjectIDDataSet.FindDesignProjectsByAssignedProjectID[0].ProjectAddress;
                txtCity.Text = TheFindDesignProjectsByAssignedProjectIDDataSet.FindDesignProjectsByAssignedProjectID[0].City;
                txtJobType.Text = TheFindDesignProjectsByAssignedProjectIDDataSet.FindDesignProjectsByAssignedProjectID[0].JobType;
                txtAssignedOffice.Text = TheFindDesignProjectsByAssignedProjectIDDataSet.FindDesignProjectsByAssignedProjectID[0].FirstName;
                txtCoordinator.Text = TheFindDesignProjectsByAssignedProjectIDDataSet.FindDesignProjectsByAssignedProjectID[0].Coordinator;
                txtJobStatus.Text = TheFindDesignProjectsByAssignedProjectIDDataSet.FindDesignProjectsByAssignedProjectID[0].JobStatus;
                txtProjectNotes.Text = TheFindDesignProjectsByAssignedProjectIDDataSet.FindDesignProjectsByAssignedProjectID[0].ProjectNotes;

                MainWindow.gintProjectID = TheFindDesignProjectsByAssignedProjectIDDataSet.FindDesignProjectsByAssignedProjectID[0].ProjectID;
                MainWindow.gstrProjectName = TheFindDesignProjectsByAssignedProjectIDDataSet.FindDesignProjectsByAssignedProjectID[0].ProjectName;
               
                TheFindDesignProjectUpdatesByProjectIDDAtaSet = TheDesignProjectUpdateClass.FindDesignProjectUpdatesByProjectID(MainWindow.gintProjectID);

                dgrUpdates.ItemsSource = TheFindDesignProjectUpdatesByProjectIDDAtaSet.FindDesignProjectUpdatesByProjectID;

                TheFindProjectDocumentationByProjectIDDataSet = TheDesignProjectDocumentationClass.FindDesignProjectDocumentationByProjectID(MainWindow.gintProjectID);

                dgrDocumentation.ItemsSource = TheFindProjectDocumentationByProjectIDDataSet.FindDesignProjectDocumentationByProjectID;

                btnFindPermits.IsEnabled = true;
                btnFindWOV.IsEnabled = true;
            }
            catch (Exception Ex)
            {
                TheEventLogClass.InsertEventLogEntry(DateTime.Now, "Blue Jay Design // View Project Info // Search Button " + Ex.Message);

                TheMessagesClass.ErrorMessage(Ex.ToString());
            }
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
            txtAddress.Text = "";
            txtAssignedOffice.Text = "";
            txtCity.Text = "";
            txtCoordinator.Text = "";
            txtDateReceived.Text = "";
            txtEnterProjectID.Text = "";
            txtJobStatus.Text = "";
            txtJobType.Text = "";
            txtProjectName.Text = "";
            txtProjectNotes.Text = "";

            TheFindDesignProjectUpdatesByProjectIDDAtaSet = TheDesignProjectUpdateClass.FindDesignProjectUpdatesByProjectID(-1);

            dgrUpdates.ItemsSource = TheFindDesignProjectUpdatesByProjectIDDAtaSet.FindDesignProjectUpdatesByProjectID;

            TheFindProjectDocumentationByProjectIDDataSet = TheDesignProjectDocumentationClass.FindDesignProjectDocumentationByProjectID(-1);

            dgrDocumentation.ItemsSource = TheFindProjectDocumentationByProjectIDDataSet.FindDesignProjectDocumentationByProjectID;

            btnFindPermits.IsEnabled = false;
            btnFindWOV.IsEnabled = false;
        }

        private void DgrDocumentation_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGrid dataGrid;
            DataGridRow selectedRow;
            DataGridCell DocumentPath;
            string strDocumentPath;

            try
            {
                if (dgrDocumentation.SelectedIndex > -1)
                {
                    //setting local variable
                    dataGrid = dgrDocumentation;
                    selectedRow = (DataGridRow)dataGrid.ItemContainerGenerator.ContainerFromIndex(dataGrid.SelectedIndex);
                    DocumentPath = (DataGridCell)dataGrid.Columns[6].GetCellContent(selectedRow).Parent;
                    strDocumentPath = ((TextBlock)DocumentPath.Content).Text;

                    if (strDocumentPath != "NO INVOICE ATTACHED")
                    {
                        System.Diagnostics.Process.Start(strDocumentPath);
                    }
                }
            }
            catch (Exception Ex)
            {
                TheEventLogClass.InsertEventLogEntry(DateTime.Now, "Blue Jay Design // View Project Info // Documentation Grid View Selection " + Ex.Message);

                TheMessagesClass.ErrorMessage(Ex.ToString());
            }
        }

        private void BtnFindWOV_Click(object sender, RoutedEventArgs e)
        {
            FindWOV FindWOV = new FindWOV();
            FindWOV.ShowDialog();
        }

        private void BtnFindPermits_Click(object sender, RoutedEventArgs e)
        {
            AddEditDesignProjectPermits AddEditDesignProjectPermits = new AddEditDesignProjectPermits();
            AddEditDesignProjectPermits.ShowDialog();
        }
    }
}
