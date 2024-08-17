using PayRollAPI;
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

namespace PayRollControls
{
    /// <summary>
    /// Interakční logika pro ShiftControlView.xaml
    /// </summary>
    public partial class ShiftControlView : UserControl
    {
        private Shift shift;

        public ShiftControlView()
        {
            InitializeComponent();
        }

        public ShiftControlView(Shift shift)
        {
            InitializeComponent();
            this.shift = shift;
            LoadTextBlocks();
        }

        private void LoadTextBlocks()
        {
            nameTextBlock.Text = shift.Employee.Name + " " + shift.Employee.Surname;
            dateTextBlock.Text = shift.Date.ToString();
            startShiftTextBlock.Text = shift.StartShift.ToString();
            endShiftTextBlock.Text = shift.EndShift.ToString();
            startBreakTextBlock.Text = shift.StartBreak.ToString();
            endBreakTextBlock.Text = shift.EndBreak.ToString();
        }
    }
}
