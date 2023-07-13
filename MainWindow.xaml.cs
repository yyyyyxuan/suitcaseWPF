using System;
using NModbus;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using suitcaseV2.func;
using System.Timers;
using static suitcaseV2.func.modbuscontroller;
using System.Diagnostics;
using System.Threading;
using static suitcaseV2.func.productcounter;
using static suitcaseV2.func.postfunc;

namespace suitcaseV2
{
    public partial class MainWindow : Window
    {
        private DispatcherTimer timer;
        private int counter, active, notactive, halt,defect,nodefect,haltcount,productInt;
        private bool haltflag = false;
        private string status, producttemp;
        private modbuscontroller modbusController;
        private productcounter productcounter;
        private postfunc postfunc;
        Stopwatch timerStart = new Stopwatch();//Active
        Stopwatch timerStop = new Stopwatch();//NotActive
        Stopwatch timerHalt = new Stopwatch();// Halt Time
        private void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            int.TryParse(inputTextBox.Text, out int parsedValue);
            productInt = parsedValue;
        }

        public MainWindow()
        {
            InitializeComponent();
            InitializeTimer();
            modbusController = new modbuscontroller();
            productcounter = new productcounter();
            postfunc = new postfunc();
            modbusController.Write4012(false, W4012EBlock);
            modbusController.Write4060("stop", W4060Block);
            timerStop.Start();

        }

        private void InitializeTimer()
        {
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(0.5);
            timer.Tick += Timer_Tick;
            timer.Start();
        }
        private void Timer_Tick(object sender, EventArgs e)
        {
            Console.WriteLine(productInt);
            counter++;
            //counterTextBlock.Text = counter.ToString();

            Read4012Result result = modbusController.read4012(W4012EBlock);
            bool onValue = result.ON;
            bool offValue = result.OFF;
            bool conveyorValue = result.CONVEYOR;
            ushort dsValue = result.DS;

            




            if (onValue)
            {
                modbusController.Write4012(true, W4012EBlock);
                status = "start";
                modbusController.Write4060(status, W4060Block);
                timerStart.Start();
                timerStop.Stop();
                timerHalt.Stop();
                haltflag = !haltflag;
            }
            if(offValue)
            {
                modbusController.Write4012(false,W4012EBlock);
                status = "stop";
                modbusController.Write4060(status, W4060Block);
                timerStart.Stop();
                timerStop.Start();
                timerHalt.Stop();
                haltflag = !haltflag;
            }
            if (dsValue >= 2000)
            {
                modbusController.Write4012(false, W4012EBlock);
                status = "halt";
                modbusController.Write4060(status, W4060Block);
                timerStart.Stop();
                timerStop.Stop();
                timerHalt.Start();
                if (!haltflag)
                {
                    haltcount++;
                    haltflag = !haltflag;
                }
            }

            active = Convert.ToInt32(timerStart.Elapsed.TotalSeconds);
            notactive = Convert.ToInt32(timerStop.Elapsed.TotalSeconds);
            halt = Convert.ToInt32(timerHalt.Elapsed.TotalSeconds);

            producttemp = productcounter.productcounterfunc(status,productInt);
            if (producttemp =="defect")
            {
                defect++;
            }
            else if (producttemp=="nodefect") 
            {
                nodefect++;
            }

            postfunc.Post(defect, nodefect, active, notactive, halt, haltcount,WebAccessBlock);

            if (productInt != 0)
            {
                productintBlock.Text = productInt.ToString();
            }
            else { productintBlock.Text = Config.data.productTimer.ToString(); }
            haltBlock.Text = haltcount.ToString();
            defectBlock.Text = defect.ToString();
            nodefectBlock.Text = nodefect.ToString();

            activeBlock.Text = TimeSpan.FromSeconds(active).ToString(@"hh\:mm\:ss");
            notactiveBlock.Text = TimeSpan.FromSeconds(notactive).ToString(@"hh\:mm\:ss");
            halttimeBlock.Text = TimeSpan.FromSeconds(halt).ToString(@"hh\:mm\:ss");


        }
    }
}

