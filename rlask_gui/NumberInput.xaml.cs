using System.Windows;
using System.Windows.Controls;

namespace rlask_gui
{
    /// <summary>
    /// Interaction logic for NumberInput.xaml
    /// </summary>
    public partial class NumberInput : UserControl
    {
        // private int _numValue = 1;
        public NumberInput()
        {
            InitializeComponent();
            txtNum.Text = NumValue.ToString();
        }


        public static readonly DependencyProperty NumValueProperty = DependencyProperty.Register(
            "NumValue", typeof(int), typeof(NumberInput), new FrameworkPropertyMetadata(defaultValue: 1, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));


        public int NumValue
        {
            get { return (int)GetValue(NumValueProperty); }
            set { 
                SetValue(NumValueProperty, value); 
            }
        }
    


        //public int NumValue
        //{
        //    get { return _numValue; }
        //    set
        //    {
        //        if (value >= 1 && value <= 9999)
        //        {
        //            _numValue = value;
        //            txtNum.Text = value.ToString();
        //        }
        //    }
        //}

        private void CmdUp_Click(object sender, RoutedEventArgs e)
        {
            if ( NumValue < 9999 ) NumValue++;
        }

        private void CmdDown_Click(object sender, RoutedEventArgs e)
        {
            if (NumValue > 1 ) NumValue--;
        }

        private void TxtNum_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (txtNum == null)
            {
                return;
            }

            if (!int.TryParse(txtNum.Text, out int numValue))
                txtNum.Text = numValue.ToString();

        }


    }
}