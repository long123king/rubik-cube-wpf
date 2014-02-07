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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Timers;


namespace MagicCube
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private System.Timers.Timer timerClock = new System.Timers.Timer();
        private Operation op = Operation.OP_RED_CLOCKWISE;
        private Sides sides = new Sides();  

        public MainWindow()
        {            
            this.DataContext = sides;
              
            timerClock.Elapsed += new ElapsedEventHandler(OnTimer);
            timerClock.Interval = 100;
            timerClock.Enabled = true;

            InitializeComponent();            
        }

        public void OnTimer(Object source, ElapsedEventArgs e)
        {
            if (op == Operation.OP_YELLOW_CLOCKWISE_COUNTER)
            {
                op = Operation.OP_RED_CLOCKWISE;
            }
            else
            {
                op++;
            }

            sides.DoOperation(op);
            if (sides.Ready())
            {
                timerClock.Enabled = false;
            }
            
        }
    }
}
