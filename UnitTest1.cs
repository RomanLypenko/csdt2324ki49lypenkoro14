using ControledCooler;
using Newtonsoft.Json.Linq;
using System.Windows.Forms;

namespace TestProject2
{
    public class UnitTest1
    {

        Form1 myForm = new Form1();

        [Fact]
        public void Test1()
        {
            string startTime = "15:10:10";
            string endTime = "17:10:10";
            DateTime date = new DateTime(2020, 1, 1, 16, 0, 0);
            bool result = myForm.IsTimeBetween(date, startTime, endTime);

            Assert.True(result);
        }
        [Fact]
        public void Test2()
        {
            myForm.fanStateCheckBox.Checked = true;
            myForm.fanPowerTrackBar.Value = 100;
            myForm.UpdateSettings();
            
            Form1 myForm1 = new Form1();
            bool res1 = myForm1.fanStateCheckBox.Checked == true;
            bool res2 = myForm1.fanPowerTrackBar.Value == 100;
            
            Assert.True(res1 && res2);
        }
        [Fact]
        public void Test3()
        {
            myForm.fanStateCheckBox.Checked = true;
            myForm.fanPowerTrackBar.Value = 100;
            myForm.UpdateSettings();
            myForm.fanStateCheckBox.Checked = false;
            myForm.fanPowerTrackBar.Value = 255;
            myForm.UpdateForm();
            bool res1 = myForm.fanStateCheckBox.Checked;
            bool res2 = myForm.fanPowerTrackBar.Value==100;
            Assert.True(res1 && res2);
        }
    }
}