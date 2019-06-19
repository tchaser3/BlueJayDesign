/* Title:           Add Design Project Items
 * Date:            6-19-2019
 * Author:          Terry Holmes
 * 
 * Description:     This is used to add design project items to for the design projects */

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
using JobTypeDLL;

namespace BlueJayDesign
{
    /// <summary>
    /// Interaction logic for AddDesignProjectItems.xaml
    /// </summary>
    public partial class AddDesignProjectItems : Window
    {
        //setting up the classes
        JobTypeClass TheJobTypeClass = new JobTypeClass();
        WPFMessagesClass TheMessagesclass = new WPFMessagesClass();

        //setting up the data
        FindSortedJobTypeDataSet TheFindSortedJobTypeDataSet = new FindSortedJobTypeDataSet();

        public AddDesignProjectItems()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            int intCounter;
            int intNumberOfRecords;

            TheFindSortedJobTypeDataSet = TheJobTypeClass.FindSortedJobType();

            intNumberOfRecords = TheFindSortedJobTypeDataSet.FindSortedJobType.Rows.Count - 1;

            cboSelectJobType.Items.Add("Select Job Type");

            for(intCounter = 0; intCounter <= intNumberOfRecords; intCounter++)
            {
                cboSelectJobType.Items.Add(TheFindSortedJobTypeDataSet.FindSortedJobType[intCounter].JobType);
            }

            cboSelectJobType.SelectedIndex = 0;
            txtAssignedProjectID.Text = MainWindow.gstrAssignedProjectID;
        }

        private void BtnProcess_Click(object sender, RoutedEventArgs e)
        {
            //setting up variables
            bool blnFatalError = false;
            string strErrorMessage = "";

            MainWindow.gstrCity = txtCity.Text;
            if(MainWindow.gstrCity == "")
            {
                blnFatalError = true;
                strErrorMessage += "The City Was Not Entered\n";
            }
            MainWindow.gstrState = txtState.Text;
            if(MainWindow.gstrState == "")
            {
                blnFatalError = true;
                strErrorMessage += "The State Was Not Entered\n";
            }
            if(cboSelectJobType.SelectedIndex < 1)
            {
                blnFatalError = true;
                strErrorMessage += "The Job Type Was not Selected\n";
            }
            MainWindow.gstrZipCode = txtZipCode.Text;
            if(MainWindow.gstrZipCode == "")
            {
                blnFatalError = true;
                strErrorMessage += "The Zip Code Was Not Entered\n";
            }
            if(blnFatalError == true)
            {
                TheMessagesclass.ErrorMessage(strErrorMessage);
                return;
            }

            this.Close();
        }

        private void CboSelectJobType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int intSelectedID;

            intSelectedID = cboSelectJobType.SelectedIndex - 1;

            if(intSelectedID > -1)
            {
                MainWindow.gintJobTypeID = TheFindSortedJobTypeDataSet.FindSortedJobType[intSelectedID].JobTypeID;
            }           

        }
    }
}
