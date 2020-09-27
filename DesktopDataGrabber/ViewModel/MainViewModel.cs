#define CLIENT
//#define GET
#define DYNAMIC

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Timers;
using System.Net.Http;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;


namespace DesktopDataGrabber.ViewModel
{
    using Model;

    /** 
      * @brief View model for MainWindow.xaml 
      */
    public class MainViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<MeasurementViewModel> Measurements { get; set; }

        #region Properties
        private string ipAddress;
        public string IpAddress
        {
            get
            {
                return ipAddress;
            }
            set
            {
                if (ipAddress != value)
                {
                    ipAddress = value;
                    OnPropertyChanged("IpAddress");
                }
            }
        }
        private int sampleTime;
        public string SampleTime
        {
            get
            {
                return sampleTime.ToString();
            }
            set
            {
                if (Int32.TryParse(value, out int st))
                {
                    if (sampleTime != st)
                    {
                        sampleTime = st;
                        OnPropertyChanged("SampleTime");
                    }
                }
            }
        }



        private string temp_read;
        public string Temp_read
        {
            get
            {
                return temp_read;
            }
            set
            {
                if (temp_read != value)
                {
                    temp_read = value;
                    OnPropertyChanged("Temp_read");
                }
            }
        }


        private string humid_read;
        public string Humid_read
        {
            get
            {
                return humid_read;
            }
            set
            {
                if (humid_read != value)
                {
                    humid_read = value;
                    OnPropertyChanged("Humid_read");
                }
            }
        }

        private string press_read;
        public string Press_read
        {
            get
            {
                return press_read;
            }
            set
            {
                if (press_read != value)
                {
                    press_read = value;
                    OnPropertyChanged("Press_read");
                }
            }
        }

        private string yaw_read;
        public string Yaw_read
        {
            get
            {
                return yaw_read;
            }
            set
            {
                if (yaw_read != value)
                {
                    yaw_read = value;
                    OnPropertyChanged("Yaw_read");
                }
            }
        }

        private string pitch_read;
        public string Pitch_read
        {
            get
            {
                return pitch_read;
            }
            set
            {
                if (pitch_read != value)
                {
                    pitch_read = value;
                    OnPropertyChanged("Pitch_read");
                }
            }
        }

        private string roll_read;
        public string Roll_read
        {
            get
            {
                return roll_read;
            }
            set
            {
                if (roll_read != value)
                {
                    roll_read = value;
                    OnPropertyChanged("Roll_read");
                }
            }
        }

        private string x = "0";
        public string X
        {
            get
            {
                return x;
            }
            set
            {
                if (x != value)
                {
                    x = value;
                    OnPropertyChanged("X");
                }
            }
        }
        private string y = "0";
        public string Y
        {
            get
            {
                return y;
            }
            set
            {
                if (y != value)
                {
                    y = value;
                    OnPropertyChanged("Y");
                }
            }
        }
        private string r = "255";
        public string R
        {
            get
            {
                return r;
            }
            set
            {
                if (r != value)
                {
                    r = value;
                    OnPropertyChanged("R");
                }
            }
        }
        private string g = "0";
        public string G
        {
            get
            {
                return g;
            }
            set
            {
                if (g != value)
                {
                    g = value;
                    OnPropertyChanged("G");
                }
            }
        }
        private string b = "0";
        public string B
        {
            get
            {
                return b;
            }
            set
            {
                if (b != value)
                {
                    b = value;
                    OnPropertyChanged("B");
                }
            }
        }
        private string text;
        public string Text
        {
            get
            {
                return text;
            }
            set
            {
                if (text != value)
                {
                    text = value;
                    OnPropertyChanged("Text");
                }
            }
        }

        public PlotModel IMUChart { get; set; }
        public PlotModel ENVChart_temp { get; set; }
        public PlotModel ENVChart_press { get; set; }
        public PlotModel ENVChart_humid { get; set; }
        public ButtonCommand StartButton { get; set; }
        public ButtonCommand StopButton { get; set; }
        public ButtonCommand UpdateConfigButton { get; set; }
        public ButtonCommand DefaultConfigButton { get; set; }
        public ButtonCommand Send_Led { get; set; }
        public ButtonCommand Send_Text { get; set; }

        #endregion

        #region Fields
        private int timeStamp = 0;
        private ConfigParams config = new ConfigParams();
        private Timer RequestTimer;
        private IoTServer Server;
        #endregion

        public MainViewModel()
        {
            Measurements = new ObservableCollection<MeasurementViewModel>();


            IMUChart = new PlotModel { Title = "IMU" }; /*data*/

            ENVChart_temp = new PlotModel { Title = "Temperatura" };
            ENVChart_press = new PlotModel { Title = "Ciśnienie" };
            ENVChart_humid = new PlotModel { Title = "Wilgotność" };

            IMUChart.Axes.Add(new LinearAxis()
            {
                Position = AxisPosition.Bottom,
                Minimum = 0,
                Maximum = config.XAxisMax,
                Key = "Horizontal",
                Unit = "sek",
                Title = "Czas"
            });

            ENVChart_temp.Axes.Add(new LinearAxis()
            {
                Position = AxisPosition.Bottom,
                Minimum = 0,
                Maximum = config.XAxisMax,
                Key = "Horizontal",
                Unit = "sek",
                Title = "Czas"
            });

            ENVChart_temp.Axes.Add(new LinearAxis()
            {
                Position = AxisPosition.Left,
                Minimum = 0,
                Maximum = 100,
                Key = "Vertical",
                Unit = "°C",
                Title = "Temperatura"
            });

            ENVChart_press.Axes.Add(new LinearAxis()
            {
                Position = AxisPosition.Bottom,
                Minimum = 0,
                Maximum = config.XAxisMax,
                Key = "Horizontal",
                Unit = "sek",
                Title = "Czas"
            });

            ENVChart_press.Axes.Add(new LinearAxis()
            {
                Position = AxisPosition.Left,
                Minimum = 250,
                Maximum = 1270,
                Key = "Vertical",
                Unit = "hPa",
                Title = "Ciśnienie"
            });

            ENVChart_humid.Axes.Add(new LinearAxis()
            {
                Position = AxisPosition.Bottom,
                Minimum = 0,
                Maximum = config.XAxisMax,
                Key = "Horizontal",
                Unit = "sek",
                Title = "Czas"
            });

            ENVChart_humid.Axes.Add(new LinearAxis()
            {
                Position = AxisPosition.Left,
                Minimum = 0,
                Maximum = 100,
                Key = "Vertical",
                Unit = "g/m3",
                Title = "Wilgotność"
            });

            IMUChart.Axes.Add(new LinearAxis()
            {
                Position = AxisPosition.Left,
                Minimum = -190,
                Maximum = 190,
                Key = "Vertical",
                Unit = "deg",
                Title = "IMU values"
            });



            IMUChart.Series.Add(new LineSeries() { Title = "roll", Color = OxyColor.Parse("#FFFF0000") });
            IMUChart.Series.Add(new LineSeries() { Title = "pitch", Color = OxyColor.Parse("#0000FF") });
            IMUChart.Series.Add(new LineSeries() { Title = "yaw", Color = OxyColor.Parse("#ffff00") });

            ENVChart_temp.Series.Add(new LineSeries() { Title = "temperatura", Color = OxyColor.Parse("#FFFF0000") });
            ENVChart_press.Series.Add(new LineSeries() { Title = "ciśnienie", Color = OxyColor.Parse("#0000FF") });
            ENVChart_humid.Series.Add(new LineSeries() { Title = "wilgotność", Color = OxyColor.Parse("#FF00FF ") });

            StartButton = new ButtonCommand(StartTimer);
            StopButton = new ButtonCommand(StopTimer);
            UpdateConfigButton = new ButtonCommand(UpdateConfig);
            DefaultConfigButton = new ButtonCommand(DefaultConfig);
            Send_Led = new ButtonCommand(Send_Led_Data);
            Send_Text = new ButtonCommand(Send_Led_Text);

            ipAddress = config.IpAddress;
            sampleTime = config.SampleTime;

            Server = new IoTServer(IpAddress);
        }

        /**
          * @brief Time series plot update procedure.
          * @param t X axis data: Time stamp [ms].
          * @param d Y axis data: Real-time measurement [-].
          */
        private void UpdatePlot_IMU(double t, double d, int which_imu_data)
        {
            LineSeries lineSeries = IMUChart.Series[0] as LineSeries;
            LineSeries lineSeries2 = IMUChart.Series[1] as LineSeries;
            LineSeries lineSeries3 = IMUChart.Series[2] as LineSeries;

            if (which_imu_data == 0) { lineSeries.Points.Add(new DataPoint(t, d)); }
            if (which_imu_data == 1) { lineSeries2.Points.Add(new DataPoint(t, d)); }
            if (which_imu_data == 2) { lineSeries3.Points.Add(new DataPoint(t, d)); }


            if (lineSeries.Points.Count > config.MaxSampleNumber)
                lineSeries.Points.RemoveAt(0);

            if (t >= config.XAxisMax)
            {
                IMUChart.Axes[0].Minimum = (t - config.XAxisMax);
                IMUChart.Axes[0].Maximum = t + config.SampleTime / 1000.0; ;
            }



            IMUChart.InvalidatePlot(true);
        }

        private void UpdatePlot_temp(double t, double d)
        {
            LineSeries lineSeries = ENVChart_temp.Series[0] as LineSeries;

            lineSeries.Points.Add(new DataPoint(t, d));

            if (lineSeries.Points.Count > config.MaxSampleNumber)
                lineSeries.Points.RemoveAt(0);

            if (t >= config.XAxisMax)
            {
                ENVChart_temp.Axes[0].Minimum = (t - config.XAxisMax);
                ENVChart_temp.Axes[0].Maximum = t + config.SampleTime / 1000.0; ;
            }

            ENVChart_temp.InvalidatePlot(true);
        }

        private void UpdatePlot_press(double t, double d)
        {
            LineSeries lineSeries = ENVChart_press.Series[0] as LineSeries;

            lineSeries.Points.Add(new DataPoint(t, d));

            if (lineSeries.Points.Count > config.MaxSampleNumber)
                lineSeries.Points.RemoveAt(0);

            if (t >= config.XAxisMax)
            {
                ENVChart_press.Axes[0].Minimum = (t - config.XAxisMax);
                ENVChart_press.Axes[0].Maximum = t + config.SampleTime / 1000.0; ;
            }

            ENVChart_press.InvalidatePlot(true);
        }

        private void UpdatePlot_humid(double t, double d)
        {
            LineSeries lineSeries = ENVChart_humid.Series[0] as LineSeries;

            lineSeries.Points.Add(new DataPoint(t, d));

            if (lineSeries.Points.Count > config.MaxSampleNumber)
                lineSeries.Points.RemoveAt(0);

            if (t >= config.XAxisMax)
            {
                ENVChart_humid.Axes[0].Minimum = (t - config.XAxisMax);
                ENVChart_humid.Axes[0].Maximum = t + config.SampleTime / 1000.0; ;
            }

            ENVChart_humid.InvalidatePlot(true);
        }
        /**
          * @brief Asynchronous led update procedure with single LED address or whole text send
          //* @param X, Y - position of LED, R,G,B - color set, Text - text send
          */
        private async void Send_Led_Data()
        {
            await Server.POSTwithClient_send_led(X, Y, R, G, B);
        }

        private async void Send_Led_Text()
        {
            await Server.POSTwithClient_send_text(Text, R, G, B);
        }
        /**
         * @brief Asynchronous chart update procedure with
         *        data obtained from IoT server responses.
         * @param ip IoT server IP address.
         */
        private async void UpdatePlotWithServerResponse()
        {
#if CLIENT
#if GET
            string responseText = await Server.GETwithClient();
#else
            string responseText_IMU = await Server.POSTwithClient("rpy");
            string responseText_ENV = await Server.POSTwithClient("env");
#endif
#else
#if GET
            string responseText = await Server.GETwithRequest();
#else
            string responseText = await Server.POSTwithRequest();
#endif
#endif
            try
            {
#if DYNAMIC 
                App.Current.Dispatcher.Invoke((System.Action)delegate
                {
                    JArray measurementsJsonArray = JArray.Parse(responseText_ENV.Replace(']', ',') + responseText_IMU.TrimStart('['));

                    var measurementsList = measurementsJsonArray.ToObject<List<MeasurementModel>>();

                    measurementsList.RemoveAt(3);
                    measurementsList.RemoveAt(6);

                    if (Measurements.Count < measurementsList.Count)
                    {
                        foreach (var m in measurementsList)
                        {
                            Measurements.Add(new MeasurementViewModel(m));
                        }
                    }
                    // Update existing elements in collection
                    else
                    {
                        for (int i = 0; i < Measurements.Count; i++)
                            Measurements[i].UpdateWithModel(measurementsList[i]);
                    }
                });



                JArray array_IMU = JArray.Parse(responseText_IMU);
                JArray array_ENV = JArray.Parse(responseText_ENV);

                foreach (JObject obj in array_IMU.Children<JObject>())
                {
                    foreach (JProperty Jprop in obj.Properties())
                    {

                        if (Jprop.Path == "[0].value")
                        {
                            string x = Jprop.Value.ToString();
                            double result = Convert.ToDouble(x);
                            if (result > 180)
                            {
                                result -= 360;
                            }
                            Roll_read = result.ToString("N1");
                            UpdatePlot_IMU(timeStamp / 1000.0, result, 0);
                        }

                        if (Jprop.Path == "[1].value")
                        {
                            string x = Jprop.Value.ToString();
                            double result = Convert.ToDouble(x);
                            if (result > 180)
                            {
                                result -= 360;
                            }
                            Pitch_read = result.ToString("N1");
                            UpdatePlot_IMU(timeStamp / 1000.0, result, 1);
                        }

                        if (Jprop.Path == "[2].value")
                        {
                            string x = Jprop.Value.ToString();
                            double result = Convert.ToDouble(x);
                            if (result > 180)
                            {
                                result -= 360;
                            }
                            Yaw_read = result.ToString("N1");
                            UpdatePlot_IMU(timeStamp / 1000.0, result, 2);
                        }

                    }
                }
                foreach (JObject obj in array_ENV.Children<JObject>())
                {
                    foreach (JProperty Jprop in obj.Properties())
                    {
                        if (Jprop.Path == "[0].value")
                        {
                            string x = Jprop.Value.ToString();
                            double result = Convert.ToDouble(x);
                            Temp_read = result.ToString("N2");
                            UpdatePlot_temp(timeStamp / 1000.0, result);
                        }
                        if (Jprop.Path == "[1].value")
                        {
                            string x = Jprop.Value.ToString();
                            double result = Convert.ToDouble(x);
                            Press_read = result.ToString("N2");
                            UpdatePlot_press(timeStamp / 1000.0, result);
                        }
                        if (Jprop.Path == "[2].value")
                        {
                            string x = Jprop.Value.ToString();
                            double result = Convert.ToDouble(x);
                            Humid_read = result.ToString("N2");
                            UpdatePlot_humid(timeStamp / 1000.0, result);
                        }

                    }
                }

#else
                ServerData resposneJson = JsonConvert.DeserializeObject<ServerData>(responseText);
                UpdatePlot(timeStamp / 1000.0, resposneJson.data);
#endif
            }
            catch (Exception e)
            {
                Debug.WriteLine("JSON DATA ERROR");
                Debug.WriteLine(responseText_IMU);
                Debug.WriteLine(e);
            }

            timeStamp += config.SampleTime;
        }

        /**
          * @brief Synchronous procedure for request queries to the IoT server.
          * @param sender Source of the event: RequestTimer.
          * @param e An System.Timers.ElapsedEventArgs object that contains the event data.
          */
        private void RequestTimerElapsed(object sender, ElapsedEventArgs e)
        {
            UpdatePlotWithServerResponse();
        }

        #region ButtonCommands

        /**
         * @brief RequestTimer start procedure.
         */
        private void StartTimer()
        {
            if (RequestTimer == null)
            {
                RequestTimer = new Timer(config.SampleTime);
                RequestTimer.Elapsed += new ElapsedEventHandler(RequestTimerElapsed);
                RequestTimer.Enabled = true;

                IMUChart.ResetAllAxes();
            }
        }

        /**
         * @brief RequestTimer stop procedure.
         */
        private void StopTimer()
        {
            if (RequestTimer != null)
            {
                RequestTimer.Enabled = false;
                RequestTimer = null;
            }
        }

        /**
         * @brief Configuration parameters update
         */
        private void UpdateConfig()
        {
            bool restartTimer = (RequestTimer != null);

            if (restartTimer)
                StopTimer();

            config = new ConfigParams(ipAddress, sampleTime);
            Server = new IoTServer(IpAddress);

            if (restartTimer)
                StartTimer();
        }

        /**
          * @brief Configuration parameters defualt values
          */
        private void DefaultConfig()
        {
            bool restartTimer = (RequestTimer != null);

            if (restartTimer)
                StopTimer();

            config = new ConfigParams();
            IpAddress = config.IpAddress;
            SampleTime = config.SampleTime.ToString();
            Server = new IoTServer(IpAddress);

            if (restartTimer)
                StartTimer();
        }

        #endregion

        #region PropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        /**
         * @brief Simple function to trigger event handler
         * @params propertyName Name of ViewModel property as string
         */
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

    }
}
