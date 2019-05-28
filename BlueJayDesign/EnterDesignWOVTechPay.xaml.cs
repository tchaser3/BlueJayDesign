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

        //setting up the data variables
        FindTechPayItemByDescriptionDataSet TheFindTechPayItemByDescriptionDataSet = new FindTechPayItemByDescriptionDataSet();

        //setting up global variables
        int gintTaskID;
        bool gblnTechPayAttached;

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
        }

        private void TxtEnterTechPayItem_TextChanged(object sender, TextChangedEventArgs e)
        {
            string strTechPayItem;
            int intLength;
            int intCounter;
            int intNumberOfRecords;

            try
            {
                strTechPayItem = txtEnterTechPayItem.Text;
                intLength = strTechPayItem.Length;
                if(intLength > 2)
                {
                    cboSelectTechPayItem.Items.Clear();
                    cboSelectTechPayItem.Items.Add("Select Techpay Item");

                    TheFindTechPayItemByDescriptionDataSet = TheTechPayClass.FindTechPayItemByDescription(strTechPayItem);
                    intNumberOfRecords = TheFindTechPayItemByDescriptionDataSet.FindTechPayItemByDescription.Rows.Count - 1;

                    if(intNumberOfRecords < 0)
                    {
                        TheMessagesClass.ErrorMessage("The Techpay Item Was Not Found");
                        return;
                    }

                    for(intCounter = 0; intCounter <= intNumberOfRecords; intCounter++)
                    {
                        cboSelectTechPayItem.Items.Add(TheFindTechPayItemByDescriptionDataSet.FindTechPayItemByDescription[intCounter].JobDescription);
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

            try
            {
                intSelectedIndex = cboSelectTechPayItem.SelectedIndex - 1;

                if(intSelectedIndex > -1)
                {
                    gintTaskID = TheFindTechPayItemByDescriptionDataSet.FindTechPayItemByDescription[intSelectedIndex].TechPayID;
                    decTechPayCost = TheFindTechPayItemByDescriptionDataSet.FindTechPayItemByDescription[intSelectedIndex].TechPayPrice;

                    txtTechPayPrice.Text = Convert.ToString(decTechPayCost);
                }
            }
            catch (Exception Ex)
            {
                TheEventLogClass.InsertEventLogEntry(DateTime.Now, "Blue Jay Design // Enter Design WOV Tech Pay // cboSelectTechPayItem Change Event " + Ex.Message);

                TheMessagesClass.ErrorMessage(Ex.ToString());
            }
        }
    }
}
