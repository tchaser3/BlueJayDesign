/* Title:           Update Assigned Task
 * Date:            2-1-19
 * Author:          Terry Holmes
 * 
 * Description:     This is where all of your tasks will be seen */

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
using AssignedTasksDLL;

namespace BlueJayDesign
{
    /// <summary>
    /// Interaction logic for UpdateAssignTask.xaml
    /// </summary>
    public partial class UpdateAssignTask : Window
    {
        //setting up the classess
        WPFMessagesClass TheMessagesClass = new WPFMessagesClass();
        EventLogClass TheEventLogClass = new EventLogClass();
        AssignedTaskClass TheAssignedTaskClass = new AssignedTaskClass();

        public UpdateAssignTask()
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
            MainWindow.TheFindAssignedTasksByAssignedEmployeeIDDataSet = TheAssignedTaskClass.FindAssignedTasksByAssignedEmployeeID(MainWindow.TheVerifyDesignEmployeeLogonDataSet.VerifyDesigEmployeeLogon[0].EmployeeID);

            dgrResults.ItemsSource = MainWindow.TheFindAssignedTasksByAssignedEmployeeIDDataSet.FindAssignedTasksByAssignedEmployeeID;
        }

        private void DgrResults_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGrid dataGrid;
            DataGridRow selectedRow;
            DataGridCell TaskID;
            string strTaskID;

            try
            {
                if (dgrResults.SelectedIndex > -1)
                {
                    //setting local variable
                    dataGrid = dgrResults;
                    selectedRow = (DataGridRow)dataGrid.ItemContainerGenerator.ContainerFromIndex(dataGrid.SelectedIndex);
                    TaskID = (DataGridCell)dataGrid.Columns[2].GetCellContent(selectedRow).Parent;
                    strTaskID = ((TextBlock)TaskID.Content).Text;

                    //find the record
                    MainWindow.gintAssignedTaskID = Convert.ToInt32(strTaskID);

                    UpdateSpecificTasks UpdateSpecificTasks = new UpdateSpecificTasks();
                    UpdateSpecificTasks.ShowDialog();

                    MainWindow.TheFindAssignedTasksByAssignedEmployeeIDDataSet = TheAssignedTaskClass.FindAssignedTasksByAssignedEmployeeID(MainWindow.TheVerifyDesignEmployeeLogonDataSet.VerifyDesigEmployeeLogon[0].EmployeeID);

                    dgrResults.ItemsSource = MainWindow.TheFindAssignedTasksByAssignedEmployeeIDDataSet.FindAssignedTasksByAssignedEmployeeID;
                }

            }
            catch (Exception Ex)
            {
                TheEventLogClass.InsertEventLogEntry(DateTime.Now, "Blue Jay Design // Update Assigned Tasks // Grid Selection " + Ex.Message);

                TheMessagesClass.ErrorMessage(Ex.ToString());
            }
        }

        private void BtnViewProjectInfo_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.ViewProjectInfoWindow.Visibility = Visibility.Visible;
            this.Visibility = Visibility.Hidden;
        }
    }
}
