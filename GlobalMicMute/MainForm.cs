using GlobalMicMute.Tools.utils;
using GlobalMicMute.Tools.win32;
using NAudio.CoreAudioApi;
using System.IO.Ports;

namespace GlobalMicMute
{
    public partial class MainForm : Form
    {
        private MMDeviceEnumerator deviceEnum = new MMDeviceEnumerator();
        private NotificationClientImplementation notificationClient;
        private NAudio.CoreAudioApi.Interfaces.IMMNotificationClient notifyClient;

        private List<ScreenBoundingRectangle> statusRects;

        private bool dragging = false;
        private Point dragCursorPoint;
        private Point dragFormPoint;
        private bool useArduino = false;
        private bool showOutline = true;
        private Arduino arduino;

        public MainForm()
        {
            notificationClient = new NotificationClientImplementation();
            notificationClient.SetHandleAction(() => updateUI());
            notifyClient = (NAudio.CoreAudioApi.Interfaces.IMMNotificationClient)notificationClient;
            deviceEnum.RegisterEndpointNotificationCallback(notifyClient);

            InitializeComponent();
            HotKeys.RegisterGlobalHotKey(1, this.Handle, Keys.M, NativeMethods.MOD_CONTROL | NativeMethods.MOD_SHIFT);

            Screen[] screens = Screen.AllScreens;
            statusRects = new List<ScreenBoundingRectangle>();
            foreach (Screen screen in screens)
            {
                statusRects.Add(new ScreenBoundingRectangle() { Color = System.Drawing.Color.Red, LineWidth = 10, Location = screen.Bounds, Opacity = 0.5, Visible = true });
            };

          //  arduino = new Arduino("COM6", 9600, new SerialDataReceivedEventHandler(DataReceivedHandler));
            arduino = new Arduino();
            updateUI();
            foreach (InputDevice inputDevice in InputDevice.GetInputDevices())
            {
                var handler = new DeviceVolumeNotificationHandler(inputDevice.Device.ID);
                handler.SetHandleAction((deviceId, data) => updateUI());
                // Bug: Stops working after some time !!!!
                inputDevice.Device.AudioEndpointVolume.OnVolumeNotification += handler.Handle;

            }
            Location = new Point(Screen.PrimaryScreen.Bounds.Right - button2.Width - 20, Screen.PrimaryScreen.Bounds.Top + 20);

        }

        private void DataReceivedHandler(
               object sender,
               SerialDataReceivedEventArgs e)
        {
            SerialPort sp = (SerialPort)sender;
            string indata = sp.ReadLine();
            System.Diagnostics.Debug.WriteLine("Data Received:"+indata);
            muteUnmuteAll();
        }


        public bool allMuted;
        public bool someMuted;

        public void updateUI()
        {

            checkDevicesStatus();
            updateButton();
            updateRectangles();
            arduino.sendCommandToSerial(allMuted);
        }

        private void updateRectangles()
        {
            foreach (ScreenBoundingRectangle sbr in statusRects)
            {

                    sbr.Visible = showOutline;

                    if (allMuted)
                    {
                        sbr.Color = Color.Red;
                    }
                    else if (someMuted)
                    {
                        sbr.Color = Color.Orange;
                    }
                    else
                    {
                        sbr.Color = Color.Green;
                    }
                
            }
        }

        private void checkDevicesStatus()
        {
            allMuted = true;
            someMuted = false;
            foreach (InputDevice inputDevice in InputDevice.GetInputDevices())
            {
                allMuted = allMuted && inputDevice.Muted;
                someMuted = someMuted || inputDevice.Muted;

            }
        }

        private void muteUnmuteAll()
        {
            var mute = !(allMuted);
            //cycle through all audio devices
            foreach (InputDevice inputDevice in InputDevice.GetInputDevices())
            {
                inputDevice.SetMuted(mute);
            }
            updateUI(); // for the cases that OnVolumeNotification does not fire
        }

        private void updateButton()
        {

            if (allMuted)
            {
                button2.BackgroundImage = Properties.Resources.mic_mute_red;

            }
            else if (someMuted)
            {
                button2.BackgroundImage = Properties.Resources.mic_orange;
            }
            else
            {
                button2.BackgroundImage = Properties.Resources.mic_green;
            }

        }


        private void button2_Click(object sender, EventArgs e)
        {
            if (!dragging)
            {
                muteUnmuteAll();
            }
        }

        private void button2_MouseDown(object sender, MouseEventArgs e)
        {

            dragCursorPoint = Cursor.Position;
            dragFormPoint = this.FindForm().Location;

        }

        private void button2_MouseMove(object sender, MouseEventArgs e)
        {
            if (dragging)
            {
                Point dif = Point.Subtract(Cursor.Position, new Size(dragCursorPoint));
                this.FindForm().Location = Point.Add(dragFormPoint, new Size(dif));
            }
        }

        private void button2_MouseUp(object sender, MouseEventArgs e)
        {
            dragging = false;
            this.Cursor = Cursors.Hand;
        }

        protected override void WndProc(ref Message m)
        {

            if (m.Msg == NativeMethods.WM_HOTKEY)
            {
                int id = m.WParam.ToInt32();
                if (id == 1)
                {
                    muteUnmuteAll();
                }
            }

            base.WndProc(ref m);
        }

        private void testToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();   
        }

        private void moveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dragging = !dragging;
            this.Cursor = Cursors.SizeAll;
        }

        private void showOutlineToolStripMenuItem_Click(object sender, EventArgs e)
        {
            showOutlineToolStripMenuItem.Checked = !showOutlineToolStripMenuItem.Checked;
            showOutline = showOutlineToolStripMenuItem.Checked;
            updateRectangles();
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutForm aboutForm = new AboutForm();    
            aboutForm.ShowDialog();    
        }



        /*  Original update which also fills a listview
 *  
/*        public void fillList()
        {
            allMuted = true;
            someMuted = false;

            var isInvokeRequired = listView1.InvokeRequired;

            if (isInvokeRequired)
            {
                listView1.Invoke(new MethodInvoker(delegate () { listView1.Items.Clear(); }));
            }
            else
            {
                listView1.Items.Clear();
            }

            foreach (InputDevice inputDevice in InputDevice.GetInputDevices())
            {
                ListViewItem i = new ListViewItem(inputDevice.Name);
                i.SubItems.Add(inputDevice.Muted.ToString());

                allMuted = allMuted && inputDevice.Muted;
                someMuted = someMuted || inputDevice.Muted;

                if (isInvokeRequired)
                {
                    listView1.Invoke(new MethodInvoker(delegate () { listView1.Items.Add(i); }));
                }
                else
                {
                    listView1.Items.Add(i);
                }
            }

            button2SetText();
            foreach (ScreenBoundingRectangle sbr in statusRects)
            {

                if (allMuted)
                {
                    this.BackColor = Color.Red;
                    sbr.Color = Color.Red;
                    sbr.Visible = true;
                }
                else if (someMuted)
                {
                    this.BackColor = Color.Orange;
                    sbr.Color = Color.Orange;
                    sbr.Visible = true;
                }
                else
                {
                    this.BackColor = Color.Green;
                    sbr.Color = Color.Green;
                    sbr.Visible = true;
                }
            }

        }*/
    }
}