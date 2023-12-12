using System.IO.Ports;
using System.Net;
using System.Windows.Forms;
using System.Xml.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ControledCooler
{
    public partial class Form1 : Form
    {
        public JObject ventilatorSettings;
        public string jsonFilePath = "D:\\robstol\\shpicer\\coolerConfid.json";
        public System.Windows.Forms.Timer timer_to_refresh;
        public CheckBox fanStateCheckBox;
        public TrackBar fanPowerTrackBar;
        public DateTimePicker startTimeDateTimePicker;
        public DateTimePicker endTimeDateTimePicker;
        public CheckBox timerEnabledCheckBox;
        public TextBox timerDurationTextBox;
        public Button updateButton;
        public CheckBox enble;
        public int secondsRemaining = -1;
        public bool enable;
        public Form1()
        {
            InitializeComponent();
            // Завантажити налаштування з файлу
            try
            {
                string jsonContent = File.ReadAllText(jsonFilePath);
                ventilatorSettings = JObject.Parse(jsonContent);
                UpdateForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка при читанні файлу: {ex.Message}", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void Form1_Load(object sender, EventArgs e)
        {

        }

        public void pictureBox1_Click_1(object sender, EventArgs e)
        {
            PictureBox pictureBox1 = new PictureBox();
            pictureBox1.Dock = DockStyle.Fill;
            this.Controls.Add(pictureBox1);
        }

        public void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            timer1 = new System.Windows.Forms.Timer(components);
            timer_to_refresh = new System.Windows.Forms.Timer(components);
            fanStateCheckBox = new CheckBox();
            fanPowerTrackBar = new TrackBar();
            startTimeDateTimePicker = new DateTimePicker();
            endTimeDateTimePicker = new DateTimePicker();
            timerEnabledCheckBox = new CheckBox();
            timerDurationTextBox = new TextBox();
            updateButton = new Button();
            enble = new CheckBox();
            ((System.ComponentModel.ISupportInitialize)fanPowerTrackBar).BeginInit();
            SuspendLayout();
            // 
            // timer1
            // 
            timer1.Enabled = true;
            timer1.Interval = 15000;
            timer1.Tick += timer1_Tick_1;
            // 
            // timer_to_refresh
            // 
            timer_to_refresh.Enabled = true;
            timer_to_refresh.Interval = 1000;
            timer_to_refresh.Tick += timer_to_refresh_Tick;
            // 
            // fanStateCheckBox
            // 
            fanStateCheckBox.AutoSize = true;
            fanStateCheckBox.Location = new Point(12, 12);
            fanStateCheckBox.Name = "fanStateCheckBox";
            fanStateCheckBox.Size = new Size(91, 24);
            fanStateCheckBox.TabIndex = 0;
            fanStateCheckBox.Text = "Fan State";
            fanStateCheckBox.UseVisualStyleBackColor = true;
            // 
            // fanPowerTrackBar
            // 
            fanPowerTrackBar.LargeChange = 10;
            fanPowerTrackBar.Location = new Point(24, 61);
            fanPowerTrackBar.Maximum = 255;
            fanPowerTrackBar.Minimum = 50;
            fanPowerTrackBar.Name = "fanPowerTrackBar";
            fanPowerTrackBar.Size = new Size(372, 56);
            fanPowerTrackBar.TabIndex = 1;
            fanPowerTrackBar.TickFrequency = 10;
            fanPowerTrackBar.Value = 50;
            // 
            // startTimeDateTimePicker
            // 
            startTimeDateTimePicker.Format = DateTimePickerFormat.Time;
            startTimeDateTimePicker.Location = new Point(12, 123);
            startTimeDateTimePicker.Name = "startTimeDateTimePicker";
            startTimeDateTimePicker.Size = new Size(136, 27);
            startTimeDateTimePicker.TabIndex = 2;
            startTimeDateTimePicker.ValueChanged += startTimeDateTimePicker_ValueChanged;
            // 
            // endTimeDateTimePicker
            // 
            endTimeDateTimePicker.Format = DateTimePickerFormat.Time;
            endTimeDateTimePicker.Location = new Point(257, 123);
            endTimeDateTimePicker.Name = "endTimeDateTimePicker";
            endTimeDateTimePicker.Size = new Size(127, 27);
            endTimeDateTimePicker.TabIndex = 3;
            // 
            // timerEnabledCheckBox
            // 
            timerEnabledCheckBox.AutoSize = true;
            timerEnabledCheckBox.Location = new Point(257, 173);
            timerEnabledCheckBox.Name = "timerEnabledCheckBox";
            timerEnabledCheckBox.Size = new Size(127, 24);
            timerEnabledCheckBox.TabIndex = 4;
            timerEnabledCheckBox.Text = "Timer Enabled";
            timerEnabledCheckBox.UseVisualStyleBackColor = true;
            // 
            // timerDurationTextBox
            // 
            timerDurationTextBox.Location = new Point(12, 170);
            timerDurationTextBox.Name = "timerDurationTextBox";
            timerDurationTextBox.Size = new Size(136, 27);
            timerDurationTextBox.TabIndex = 5;
            // 
            // updateButton
            // 
            updateButton.Location = new Point(12, 227);
            updateButton.Name = "updateButton";
            updateButton.Size = new Size(372, 59);
            updateButton.TabIndex = 6;
            updateButton.Text = "Update Settings";
            updateButton.UseVisualStyleBackColor = true;
            updateButton.Click += updateButton_Click;
            // 
            // enble
            // 
            enble.AutoSize = true;
            enble.Location = new Point(154, 123);
            enble.Name = "enble";
            enble.Size = new Size(68, 24);
            enble.TabIndex = 7;
            enble.Text = "enble";
            enble.UseVisualStyleBackColor = true;
            enble.CheckedChanged += checkBox1_CheckedChanged_1;
            // 
            // Form1
            // 
            ClientSize = new Size(408, 300);
            Controls.Add(enble);
            Controls.Add(updateButton);
            Controls.Add(timerDurationTextBox);
            Controls.Add(timerEnabledCheckBox);
            Controls.Add(endTimeDateTimePicker);
            Controls.Add(startTimeDateTimePicker);
            Controls.Add(fanPowerTrackBar);
            Controls.Add(fanStateCheckBox);
            Name = "Form1";
            Load += Form1_Load_1;
            ((System.ComponentModel.ISupportInitialize)fanPowerTrackBar).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        /// <summary>
        /// Checks if the specified time, represented by 'currentTime', is within the time range defined by 'startTime' and 'endTime'.
        /// </summary>
        /// <returns>True if 'currentTime' is within the specified time range, otherwise false.</returns>

        public static bool IsTimeBetween(DateTime currentTime, string startTime, string endTime)
        {
            DateTime start = DateTime.ParseExact(startTime, "HH:mm:ss", null);
            DateTime end = DateTime.ParseExact(endTime, "HH:mm:ss", null);

            return currentTime.TimeOfDay >= start.TimeOfDay && currentTime.TimeOfDay <= end.TimeOfDay;
        }

        void ManageVentilator(JObject ventilatorSettings, DateTime currentTime)
        {

            string startTime = ventilatorSettings["ventilatorControl"]["fanSchedule"]["startTime"].ToString();
            string endTime = ventilatorSettings["ventilatorControl"]["fanSchedule"]["endTime"].ToString();
            bool timerEnabled = (bool)ventilatorSettings["ventilatorControl"]["fanState"];


            if (enble.Checked && IsTimeBetween(currentTime, startTime, endTime))
            {
                Console.WriteLine("Увімкнути вентилятор");
                if (!timerEnabled)
                {

                    JsonSerializerSettings jsonSettings = new JsonSerializerSettings
                    {
                        Formatting = Formatting.None
                    };
                    ventilatorSettings["ventilatorControl"]["fanState"] = true;
                    fanStateCheckBox.Checked = true;

                    string jsonContent = JsonConvert.SerializeObject(ventilatorSettings, jsonSettings);

                    File.WriteAllText(jsonFilePath, jsonContent);
                    using (SerialPort port = new SerialPort("COM4", 9600))
                    {
                        port.Open();
                        string jsonData = File.ReadAllText(jsonFilePath);

                        jsonData = jsonData.Replace("\r", "");
                        jsonData = jsonData.Replace("\n", "");
                        jsonData = jsonData.Replace(" ", "");

                        port.Write(jsonData);
                    }
                }
            }
            else if (enble.Checked && !IsTimeBetween(currentTime, startTime, endTime))
            {

                if (timerEnabled)
                {
                    JsonSerializerSettings jsonSettings = new JsonSerializerSettings
                    {
                        Formatting = Formatting.None
                    };
                    ventilatorSettings["ventilatorControl"]["fanState"] = false;
                    fanStateCheckBox.Checked = false;

                    string jsonContent = JsonConvert.SerializeObject(ventilatorSettings, jsonSettings);

                    File.WriteAllText(jsonFilePath, jsonContent);
                    using (SerialPort port = new SerialPort("COM4", 9600))
                    {
                        port.Open();
                        string jsonData = File.ReadAllText(jsonFilePath);

                        jsonData = jsonData.Replace("\r", "");
                        jsonData = jsonData.Replace("\n", "");
                        jsonData = jsonData.Replace(" ", "");

                        port.Write(jsonData);
                    }
                }
            }
        }
        async void timer1_Tick_1(object sender, EventArgs e)
        {

        }

        public void Form1_Load_1(object sender, EventArgs e)
        {

        }

        public System.Windows.Forms.Timer timer1;
        public System.ComponentModel.IContainer components;

        void timer_to_refresh_Tick(object sender, EventArgs e)
        {
            secondsRemaining--;
            if (secondsRemaining == 0 && timerEnabledCheckBox.Checked == true)
            {
                JsonSerializerSettings jsonSettings = new JsonSerializerSettings
                {
                    Formatting = Formatting.None
                };
                ventilatorSettings["ventilatorControl"]["fanState"] = false;
                fanStateCheckBox.Checked = false;


                string jsonContent = JsonConvert.SerializeObject(ventilatorSettings, jsonSettings);

                File.WriteAllText(jsonFilePath, jsonContent);
                using (SerialPort port = new SerialPort("COM4", 9600))
                {
                    port.Open();
                    string jsonData = File.ReadAllText(jsonFilePath);

                    jsonData = jsonData.Replace("\r", "");
                    jsonData = jsonData.Replace(" ", "");

                    port.Write(jsonData);
                }
            };
            if (enble.Checked)
                ManageVentilator(ventilatorSettings, DateTime.Now);
        }

        public void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox checkBox = (CheckBox)sender;
            enable = checkBox.Checked;
        }

        public void fanPowerTrackBar_Scroll(object sender, EventArgs e)
        {

        }
        public void updateButton_Click(object sender, EventArgs e)
        {
            UpdateSettings();
        }

        public void startTimeDateTimePicker_ValueChanged(object sender, EventArgs e)
        {

        }


        /// <summary>
        /// Updates the form by reading data from a JSON
        /// file that stores ventilator configuration.
        /// </summary>

        public void UpdateForm()
        {
            fanStateCheckBox.Checked = ventilatorSettings["ventilatorControl"]["fanState"].Value<bool>();
            fanPowerTrackBar.Value = ventilatorSettings["ventilatorControl"]["fanPower"].Value<int>();
            startTimeDateTimePicker.Value = DateTime.Parse(ventilatorSettings["ventilatorControl"]["fanSchedule"]["startTime"].Value<string>());
            endTimeDateTimePicker.Value = DateTime.Parse(ventilatorSettings["ventilatorControl"]["fanSchedule"]["endTime"].Value<string>());
            timerEnabledCheckBox.Checked = ventilatorSettings["ventilatorControl"]["fanTimer"]["enabled"].Value<bool>();
            timerDurationTextBox.Text = ventilatorSettings["ventilatorControl"]["fanTimer"]["durationSeconds"].Value<int>().ToString();
        }


        /// <summary>
        /// Updates ventilator settings based on the user input in the form, then saves the updated settings to a JSON file
        /// and sends the serialized JSON data to a connected device via a serial port.
        /// </summary>

        public void UpdateSettings()
        {
            ventilatorSettings["ventilatorControl"]["fanState"] = fanStateCheckBox.Checked;
            ventilatorSettings["ventilatorControl"]["fanPower"] = fanPowerTrackBar.Value;
            ventilatorSettings["ventilatorControl"]["fanSchedule"]["startTime"] = startTimeDateTimePicker.Value.ToString("HH:mm:ss");
            ventilatorSettings["ventilatorControl"]["fanSchedule"]["endTime"] = endTimeDateTimePicker.Value.ToString("HH:mm:ss");
            ventilatorSettings["ventilatorControl"]["fanTimer"]["enabled"] = timerEnabledCheckBox.Checked;
            ventilatorSettings["ventilatorControl"]["fanTimer"]["durationSeconds"] = int.Parse(timerDurationTextBox.Text);
            secondsRemaining = int.Parse(timerDurationTextBox.Text);
            enable = enble.Checked;

            try
            {

                JsonSerializerSettings jsonSettings = new JsonSerializerSettings
                {
                    Formatting = Formatting.None
                };

                string jsonContent = JsonConvert.SerializeObject(ventilatorSettings, jsonSettings);

                File.WriteAllText(jsonFilePath, jsonContent);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка при збереженні файлу: {ex.Message}", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            using (SerialPort port = new SerialPort("COM4", 9600))
            {
                port.Open();
                string jsonData = File.ReadAllText(jsonFilePath);

                jsonData = jsonData.Replace("\r", "");
                jsonData = jsonData.Replace("\n", "");
                jsonData = jsonData.Replace(" ", "");

                port.Write(jsonData);
            }
        }

        public void fanPowerTrackBar_Scroll_1(object sender, EventArgs e)
        {

        }

        public void checkBox1_CheckedChanged_1(object sender, EventArgs e)
        {

        }
    }

}

