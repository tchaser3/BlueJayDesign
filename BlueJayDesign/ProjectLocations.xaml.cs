/* Title:           Project Locations
 * Date:            3-28-19
 * Author:          Terry Holmes
 * 
 * Description:     This is used to see the project locations */

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
using Microsoft.Maps.MapControl.WPF;
using System.Xml;
using System.Net;
using NewEventLogDLL;
using DesignProjectsDLL;

namespace BlueJayDesign
{
    /// <summary>
    /// Interaction logic for ProjectLocations.xaml
    /// </summary>
    public partial class ProjectLocations : Window
    {
        //setting up the classes
        WPFMessagesClass TheMessagesClass = new WPFMessagesClass();
        EventLogClass TheEventLogClass = new EventLogClass();
        DesignProjectsClass TheDesignProjectsClass = new DesignProjectsClass();

        //getting data
        FindOpenDesignProjectLocationDataSet TheFindOpenDesignProjectLocationDataSet = new FindOpenDesignProjectLocationDataSet();
        DesignProjectLocationsDataSet TheDesignProjectLocationsDataSet = new DesignProjectLocationsDataSet();

        string BingMapsKey = "rlOQHqvgydklMdwaQpTs~2ABi0R5AuQXzlDyIS5RJwQ~Ajh8Q9JoMtW_PkcY-IQBgLRvc-3SOz8tDR52P-UtRD1uIUksrk0mdpmhNOp8K2Nz";

        public ProjectLocations()
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
            MainWindow.AssignSurveyorWindow.Visibility = Visibility.Hidden;
            Close();
        }

        private void BtnAddNewProject_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.AddNewProjectWindow.Visibility = Visibility.Visible;
            MainWindow.AssignSurveyorWindow.Visibility = Visibility.Hidden;
            Close();
        }


        private void BtnMyTasks_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.UpdateAssignTasksWindow.Visibility = Visibility.Visible;
            MainWindow.AssignSurveyorWindow.Visibility = Visibility.Hidden;
            Close();
        }

        private void BtnAssignTask_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.AssignTasksWindow.Visibility = Visibility.Visible;
            MainWindow.AssignSurveyorWindow.Visibility = Visibility.Hidden;
            Close();
        }

        private void BtnAssignSurveyor_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.AssignSurveyorWindow.Visibility = Visibility.Visible;
            MainWindow.AssignSurveyorWindow.Visibility = Visibility.Hidden;
            Close();
        }

        private void BtnUpdateSurvey_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.UpdateSurveyWindow.Visibility = Visibility.Visible;
            MainWindow.AssignSurveyorWindow.Visibility = Visibility.Hidden;
            Close();
        }

        private void BtnUpdateProject_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.UpdateProjectWindow.Visibility = Visibility.Visible;
            MainWindow.AssignSurveyorWindow.Visibility = Visibility.Hidden;
            Close();
        }

        private void BtnCloseProject_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.CloseProjectWindow.Visibility = Visibility.Visible;
            MainWindow.AssignSurveyorWindow.Visibility = Visibility.Hidden;
            Close();
        }

        private void BtnEmployeeProductivity_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.EmployeeProductivityWindow.Visibility = Visibility.Visible;
            MainWindow.AssignSurveyorWindow.Visibility = Visibility.Hidden;
            Close();
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            TheMessagesClass.CloseTheProgram();
        }

        private void BtnHome_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        private void BtnOpenDesignProjects_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.OpenDesignProjectWindow.Visibility = Visibility.Visible;
            MainWindow.AssignSurveyorWindow.Visibility = Visibility.Hidden;
            Close();
        }

        private void BtnDesignProjectReport_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.DesignProjectReportwindow.Visibility = Visibility.Visible;
            MainWindow.AssignSurveyorWindow.Visibility = Visibility.Hidden;
            Close();
        }

        private void BtnPolePermitReport_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.PolePermitReportWindow.Visibility = Visibility.Visible;
            MainWindow.AssignSurveyorWindow.Visibility = Visibility.Hidden;
            Close();
        }

        private void BtnOpenSurveryorReport_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.OpenSurveyorReportWindow.Visibility = Visibility.Visible;
            MainWindow.AssignSurveyorWindow.Visibility = Visibility.Hidden;
            Close();
        }

        private void BtnViewProjectInfo_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.ViewProjectInfoWindow.Visibility = Visibility.Visible;
            MainWindow.AssignSurveyorWindow.Visibility = Visibility.Hidden;
            Close();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            string strAddress = "";
            int intCounter;
            int intNumberOfRecords;
            int intItemCounter;
            string strItemCounter;
            string strWorkOrder;
            string strState = "OH";

            PleaseWait PleaseWait = new PleaseWait();
            PleaseWait.Show();

            try
            {
                //loading the data set
                TheFindOpenDesignProjectLocationDataSet = TheDesignProjectsClass.FindOpenDesignProjectLocation();

                intItemCounter = 0;

                intNumberOfRecords = TheFindOpenDesignProjectLocationDataSet.FindOpenDesignProjectLocations.Rows.Count - 1;

                if (intNumberOfRecords > -1)
                {
                    for (intCounter = 0; intCounter <= intNumberOfRecords; intCounter++)
                    {
                        if(TheFindOpenDesignProjectLocationDataSet.FindOpenDesignProjectLocations[intCounter].AssignedOffice == "CLEVELAND")
                        {
                            strState = "OH";
                        }
                        else if(TheFindOpenDesignProjectLocationDataSet.FindOpenDesignProjectLocations[intCounter].AssignedOffice == "MILWAUKEE")
                        {
                            strState = "WI";
                        }

                        strAddress = TheFindOpenDesignProjectLocationDataSet.FindOpenDesignProjectLocations[intCounter].ProjectAddress + ", ";
                        strAddress += TheFindOpenDesignProjectLocationDataSet.FindOpenDesignProjectLocations[intCounter].City + ", " + strState;

                        intItemCounter++;
                        strItemCounter = Convert.ToString(intItemCounter);
                        strWorkOrder = strItemCounter + "\n";
                        strWorkOrder += TheFindOpenDesignProjectLocationDataSet.FindOpenDesignProjectLocations[intCounter].AssignedProjectID + "\n";
                        strWorkOrder += strAddress + "\n";
                        strWorkOrder += TheFindOpenDesignProjectLocationDataSet.FindOpenDesignProjectLocations[intCounter].AssignedOffice + "\n";
                        strWorkOrder += TheFindOpenDesignProjectLocationDataSet.FindOpenDesignProjectLocations[intCounter].JobStatus + "\n";

                        DesignProjectLocationsDataSet.projectsRow NewProjectRow = TheDesignProjectLocationsDataSet.projects.NewprojectsRow();

                        NewProjectRow.ProjectLocation = strWorkOrder;

                        TheDesignProjectLocationsDataSet.projects.Rows.Add(NewProjectRow);

                        XmlDocument searchResponse = Geocode(strAddress);

                        FindandDisplayNearbyPOI(searchResponse, strItemCounter);
                    }

                    dgrResults.ItemsSource = TheDesignProjectLocationsDataSet.projects;
                }
            }
            catch (Exception Ex)
            {
                TheEventLogClass.InsertEventLogEntry(DateTime.Now, "MDU Drop Bury Supervisor // Schedule Technicians // Window Loaded " + Ex.Message);

                TheMessagesClass.ErrorMessage(Ex.ToString());
            }

            PleaseWait.Close();
        }
        private XmlDocument GetXmlResponse(string requestUrl)
        {
            System.Diagnostics.Trace.WriteLine("Request URL (XML): " + requestUrl);
            HttpWebRequest request = WebRequest.Create(requestUrl) as HttpWebRequest;
            using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
            {
                if (response.StatusCode != HttpStatusCode.OK)
                    throw new Exception(String.Format("Server error (HTTP {0}: {1}).",
                    response.StatusCode,
                    response.StatusDescription));
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(response.GetResponseStream());
                return xmlDoc;
            }
        }
        private void FindandDisplayNearbyPOI(XmlDocument xmlDoc, string strItemCounter)
        {
            //Get location information from geocode response 

            //Create namespace manager
            XmlNamespaceManager nsmgr = new XmlNamespaceManager(xmlDoc.NameTable);
            nsmgr.AddNamespace("rest", "http://schemas.microsoft.com/search/local/ws/rest/v1");
            XmlNodeList locationElements = xmlDoc.SelectNodes("//rest:Location", nsmgr);

            //Get the geocode location points that are used for display (UsageType=Display)
            XmlNodeList displayGeocodePoints = locationElements[0].SelectNodes(".//rest:GeocodePoint/rest:UsageType[.='Display']/parent::node()", nsmgr);
            string latitude = displayGeocodePoints[0].SelectSingleNode(".//rest:Latitude", nsmgr).InnerText;
            string longitude = displayGeocodePoints[0].SelectSingleNode(".//rest:Longitude", nsmgr).InnerText;


            Location location = new Location(Convert.ToDouble(latitude), Convert.ToDouble(longitude));
            Pushpin pushpin = new Pushpin();
            pushpin.Location = location;
            pushpin.Content = strItemCounter;
            myMap.Children.Add(pushpin);

        }
        public XmlDocument Geocode(string addressQuery)
        {
            //Create REST Services geocode request using Locations API
            string geocodeRequest = "http://dev.virtualearth.net/REST/v1/Locations/" + addressQuery + "?o=xml&key=" + BingMapsKey;

            //Make the request and get the response
            XmlDocument geocodeResponse = GetXmlResponse(geocodeRequest);

            return (geocodeResponse);
        }
    }
}
