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
using NewEmployeeDLL;
using NewEventLogDLL;
using DataValidationDLL;

namespace BlueJayDesign
{
    /// <summary>
    /// Interaction logic for SendEmail.xaml
    /// </summary>
    public partial class SendEmail : Window
    {
        //setting up the classes
        WPFMessagesClass TheMessagesClass = new WPFMessagesClass();
        EmployeeClass TheEmployeeClass = new EmployeeClass();
        EventLogClass TheEventLogClass = new EventLogClass();
        SendEmailClass TheSendEmailClass = new SendEmailClass();
        DataValidationClass TheDataValidationClass = new DataValidationClass();

        //setting up the data
        EmployeeEmailDataSet TheEmployeeEmailDataSet = new EmployeeEmailDataSet();

        //setting up variables
        bool gblnEmailSelected;

        public SendEmail()
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
            gblnEmailSelected = true;
            cboSelectEmployee.Items.Clear();
            txtEnterLastName.Text = "";
            txtMessage.Text = "";
            txtSubject.Text = "";
            TheEmployeeEmailDataSet.employeeemail.Rows.Clear();
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

        private void TxtEnterLastName_TextChanged(object sender, TextChangedEventArgs e)
        {
            //setting local variables
            int intCounter;
            int intNumberOfRecords;
            string strLastName;
            int intLength;

            strLastName = txtEnterLastName.Text;
            intLength = strLastName.Length;

            if(intLength > 2)
            {
                MainWindow.TheComboEmployeeDataSet = TheEmployeeClass.FillEmployeeComboBox(strLastName);

                intNumberOfRecords = MainWindow.TheComboEmployeeDataSet.employees.Rows.Count - 1;
                cboSelectEmployee.Items.Clear();
                cboSelectEmployee.Items.Add("Select Employee");
                gblnEmailSelected = false;

                if(intNumberOfRecords < 0)
                {
                    TheMessagesClass.ErrorMessage("Employee Not Found");
                    return;
                }

                for(intCounter = 0; intCounter <= intNumberOfRecords; intCounter++)
                {
                    cboSelectEmployee.Items.Add(MainWindow.TheComboEmployeeDataSet.employees[intCounter].FullName);
                }

                cboSelectEmployee.SelectedIndex = 0;
            }

        }

        private void CboSelectEmployee_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int intEmployeeID;
            int intSelectedIndex;
            string strEmailAddress;
            int intRecordsReturned;
            bool blnFatalError;
            int intCounter;
            int intNumberOfRecords;

            try
            {
                intSelectedIndex = cboSelectEmployee.SelectedIndex - 1;

                if(intSelectedIndex > -1)
                {
                    intEmployeeID = MainWindow.TheComboEmployeeDataSet.employees[intSelectedIndex].EmployeeID;

                    MainWindow.TheFindEmployeeEmailAddressDataSet = TheEmployeeClass.FindEmployeeEmailAddress(intEmployeeID);

                    intRecordsReturned = MainWindow.TheFindEmployeeEmailAddressDataSet.FindEmployeeEmailAddress.Rows.Count;
                    
                    if(intRecordsReturned == 0)
                    {
                        TheMessagesClass.ErrorMessage("Employee Not Found");
                        return;
                    }

                    blnFatalError = TheDataValidationClass.VerifyEmailAddress(MainWindow.TheFindEmployeeEmailAddressDataSet.FindEmployeeEmailAddress[0].EmailAddress);

                    if(blnFatalError == true)
                    {
                        TheMessagesClass.ErrorMessage("Employee Does Not Have An Email Address,\nPlease Submit A Ticket with the Request Email Address");
                        return;
                    }

                    intNumberOfRecords = TheEmployeeEmailDataSet.employeeemail.Rows.Count - 1;

                    if(intNumberOfRecords > -1)
                    {
                        for(intCounter = 0; intCounter <= intNumberOfRecords; intCounter++)
                        {
                            if(intEmployeeID == TheEmployeeEmailDataSet.employeeemail[intCounter].EmployeeID)
                            {
                                TheMessagesClass.InformationMessage("Employee Has Been Entered Already");
                                return;
                            }
                        }
                    }

                    strEmailAddress = MainWindow.TheFindEmployeeEmailAddressDataSet.FindEmployeeEmailAddress[0].EmailAddress;

                    EmployeeEmailDataSet.employeeemailRow NewEmailRow = TheEmployeeEmailDataSet.employeeemail.NewemployeeemailRow();

                    NewEmailRow.EmployeeID = intEmployeeID;
                    NewEmailRow.EmailAddress = strEmailAddress;

                    TheEmployeeEmailDataSet.employeeemail.Rows.Add(NewEmailRow);

                    gblnEmailSelected = true;

                    TheMessagesClass.InformationMessage("Employee Added");

                    cboSelectEmployee.Items.Clear();
                    txtEnterLastName.Text = "";

                }
            }
            catch (Exception Ex)
            {
                TheEventLogClass.InsertEventLogEntry(DateTime.Now, "Blue Jay Design // Send Email // Combo Box Selection " + Ex.Message);

                TheMessagesClass.ErrorMessage(Ex.ToString());
            }
        }

        private void BtnSend_Click(object sender, RoutedEventArgs e)
        {
            string strMessage;
            string strSubject;
            bool blnFatalError = false;
            string strErrorMessage = "";
            int intCounter;
            int intNumberOfRecords;
            string strEmailAddress;
            string strFinalMessage;

            try
            {
                //data validation;
                strMessage = txtMessage.Text;
                if(strMessage == "")
                {
                    blnFatalError = true;
                    strErrorMessage += "The Message Was Not Entered\n";
                }
                strSubject = txtSubject.Text;
                if (strSubject == "")
                {
                    blnFatalError = true;
                    strErrorMessage += "The Subject Was Not Entered\n";
                }
                if(gblnEmailSelected == false)
                {
                    blnFatalError = true;
                    strErrorMessage += "Email Address Have Not Been Selected\n";
                }
                if(blnFatalError == true)
                {
                    TheMessagesClass.ErrorMessage(strErrorMessage);
                    return;
                }

                strSubject += " - DO NOT REPLY";
                strFinalMessage = "<h1>" + strSubject + "</h1>";
                strFinalMessage += "<h3>This is a Message Sent From " + MainWindow.TheVerifyDesignEmployeeLogonDataSet.VerifyDesigEmployeeLogon[0].FirstName + " " + MainWindow.TheVerifyDesignEmployeeLogonDataSet.VerifyDesigEmployeeLogon[0].LastName + "</h3>";
                strFinalMessage += "<p>" + strMessage + "</p>";

                intNumberOfRecords = TheEmployeeEmailDataSet.employeeemail.Rows.Count - 1;

                for(intCounter = 0; intCounter <= intNumberOfRecords; intCounter++)
                {
                    strEmailAddress = TheEmployeeEmailDataSet.employeeemail[intCounter].EmailAddress;

                    blnFatalError = !TheSendEmailClass.SendEmail(strEmailAddress, strSubject, strFinalMessage);

                    if (blnFatalError == true)
                        throw new Exception();
                }

                TheMessagesClass.InformationMessage("Email Sent");
                ResetControls();
            }
            catch (Exception Ex)
            {
                TheEventLogClass.InsertEventLogEntry(DateTime.Now, "Blue Jay Design // Send Email // Send Button " + Ex.Message);

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
