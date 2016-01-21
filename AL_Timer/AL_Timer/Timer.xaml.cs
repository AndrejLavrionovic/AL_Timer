using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace AL_Timer {
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Timer : Page {

        // Instances
        DispatcherTimer stopWatchTimer;
        Stopwatch stopWatch;
        private long ms, ss, mm, hh, dd, lastLapTime;


        List<long> lapTimes;

        // Constructor
        public Timer() {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e) {

            if(stopWatch == null) {
                stopWatch = new Stopwatch();
            }
            // check for the timer and then set up
            if(stopWatchTimer == null) {
                ms = ss = mm = hh = dd = 0;
                stopWatchTimer = new DispatcherTimer();
                stopWatchTimer.Tick += StopWatchTimer_Tick;
                stopWatchTimer.Interval = new TimeSpan(0, 0, 0, 0, 1); // 1 milisecond
            }
            base.OnNavigatedTo(e);
        }

        private void StopWatchTimer_Tick(object sender, object e) {
            // update the textblock with the time elapsed
            // figure out the elapsed time using the timer properties
            // some maths division and modules
            ms = stopWatch.ElapsedMilliseconds;

            ss = ms / 1000;
            ms = ms % 1000;

            mm = ss / 60;
            ss = ss % 60;

            hh = mm / 60;
            mm = mm % 60;

            dd = hh / 24;
            hh = hh % 24;

            tblDD.Text = dd.ToString("00");
            tblHH.Text = hh.ToString("00");
            tblMM.Text = mm.ToString("00");
            tblSS.Text = ss.ToString("00");
            tblMS.Text = ms.ToString("000");
        }

        // Start/Stop Button
        private void btnStartStop_Click(object sender, RoutedEventArgs e) {
            
            // start the stopwatch
            // kick off a timer
            if(btnStartStop.Content.ToString() == "Start") {
                // start the timer, change the text
                stopWatchTimer.Start();
                stopWatch.Start();
                btnResetLap.Content = "Lap";
                btnStartStop.Content = "Stop";
                btnStartStop.Background = new SolidColorBrush(Colors.Red);
            }
            else { // stop
                stopWatchTimer.Stop();
                stopWatch.Stop();
                btnResetLap.Content = "Reset";
                btnStartStop.Content = "Start";
                btnStartStop.Background = new SolidColorBrush(Colors.Green);
            }
        }

        // Reset/Lap Button
        private void btnResetLap_Click(object sender, RoutedEventArgs e) {
            long currentTime;
            TextBlock tblLapTime;

            if(btnResetLap.Content.ToString() == "Reset") {
                // rezero all timers
                stopWatch.Reset();
                dd = hh = mm = ss = ms = 0;
                tblDD.Text = "00";
                tblHH.Text = "00";
                tblMM.Text = "00";
                tblSS.Text = "00";
                tblMS.Text = "00";
            }
            else { // Text = "Lap"
                // save the current time, add to list
                if(lapTimes == null) {
                    lapTimes = new List<long>();
                    lastLapTime = 0;
                }

                // get the ellapsed milliseconde
                // substract the last one and then store the difference
                currentTime = stopWatch.ElapsedMilliseconds;
                lapTimes.Add(currentTime - lastLapTime);
                lastLapTime = currentTime;

                tblLapTime = new TextBlock();
                tblLapTime.Text = lapTimes.Last().ToString();
                tblLapTime.HorizontalAlignment = HorizontalAlignment.Center;

                spLapTimes.Children.Add(tblLapTime);
            }
        }
    }
}
