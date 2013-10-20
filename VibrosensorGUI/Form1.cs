using System;
using System.Windows.Forms;
using System.Threading;
using System.IO.Ports;

namespace Vibro
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            string[] portnames = SerialPort.GetPortNames();

            //serialPort1.PortName = "COM3";
            //serialPort1.Open();
            for (int i = 0; i < portnames.Length; i++)
            {
                comboBox1.Items.Add(portnames[i]);
            }
            
            if (portnames.Length == 0)
            {
                comboBox1.Items.Add("Нет подключенных устройств");
                comboBox1.SelectedItem = "Нет подключенных устройств";
                flag = true;
            }
            else
            { 
                comboBox1.SelectedItem = portnames[0];
                flag = false;
            }
            
        }

        bool flag = false;
        delegate void labelTextDelegateTEMP(string text1, string text2);
        delegate void labelTextDelegateVIBRO(UInt32 text3);
        string InputFromCOM, TempStringInput, HumidityStringInput;
        public UInt32 VibroInputInt;


        public void StartButton_Click(object sender, EventArgs e)
        {
            if (flag) 
            {
                MessageBox.Show("Нет устройств, подключенных к COM порту");
            }
            else
            {
                serialPort1.PortName = comboBox1.SelectedItem.ToString();
                serialPort1.Open();
                Thread th =  new Thread(output);
                th.IsBackground = true;
                th.Start();

                chart1.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;

                chart1.ChartAreas[0].CursorX.IsUserEnabled = true;
                chart1.ChartAreas[0].CursorX.IsUserSelectionEnabled = true;
                chart1.ChartAreas[0].AxisX.ScaleView.Zoomable = true;
            }

        }

        public void timer1_Tick(object sender, EventArgs e)
        {

            /*
            Microsoft.Office.Interop.Excel.Application EXCEL = new Microsoft.Office.Interop.Excel.Application();
            Microsoft.Office.Interop.Excel.Workbook ObjWorkBook;
            Microsoft.Office.Interop.Excel.Worksheet ObjWorkSheet;
            //Книга.
            ObjWorkBook = EXCEL.Workbooks.Add(System.Reflection.Missing.Value);
            //Таблица.
            ObjWorkSheet = (Microsoft.Office.Interop.Excel.Worksheet)ObjWorkBook.Sheets[1];

            //Значения [y - строка,VibroInputInt - столбец]
            ObjWorkSheet.Cells[1, 1] = "Время";
            ObjWorkSheet.Cells[1, 2] = "Температура";
            ObjWorkSheet.Cells[1, 3] = "Влажность";

            
           // ObjWorkSheet.Cells[i, 1] = DateTime.Now.ToString("HH:mm:ss");
            serialPort1.WriteLine("2");

            serialPort1.WriteLine("2");
            //ObjWorkSheet.Cells[i, 2] = serialPort1.ReadLine();//"Температура"; //serialPort1.ReadLine();
            InputFromCOM = serialPort1.ReadLine();
            serialPort1.WriteLine("1");
            h = serialPort1.ReadLine();

            textBox1.Text = "Температура " + InputFromCOM + "\n";
            textBox1.Text =textBox1.Text+ "Влажность " + h + "\n";
            //ObjWorkSheet.Cells[i, 3] = serialPort1.ReadLine();//"Влажность";//serialPort1.ReadLine();

           // i++;
           // EXCEL.Visible = true;
           // EXCEL.UserControl = true;
            */
        }

        private void ClearButton_Click(object sender, EventArgs e)
        {
            chart1.Series[0].Points.Clear();
        }

        public void output()
        {
            while (true)
            {
                   InputFromCOM = serialPort1.ReadLine();
                if (string.Concat(InputFromCOM[0], InputFromCOM[1]) == "TE")
                {
                    TempStringInput = string.Concat(InputFromCOM[4], InputFromCOM[5]);
                    HumidityStringInput = string.Concat(InputFromCOM[9], InputFromCOM[10]);
                    LabelText(TempStringInput, HumidityStringInput);
                }
                else
                {
                    VibroInputInt = Convert.ToUInt32(InputFromCOM);
                    ChartGraph(VibroInputInt);
                }
             
            }
        }

        private void LabelText(string HumidityOutput, string TempOutput)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new labelTextDelegateTEMP(LabelText), new object[] { HumidityOutput, TempOutput});
                return;
            }
            else
            {
                HumTextBlock.Text = "Влажность: "+HumidityOutput;
                TempTextBlock.Text = "Температура: " + TempOutput;
                DataTextBlock.Text = DateTime.Now.ToString("HH:mm:ss");
            }
        }

        private void ChartGraph(UInt32 VibroOutput)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new labelTextDelegateVIBRO(ChartGraph), new object[] { VibroOutput });
                return;
            }
            else
            {            
                DataTextBlock.Text = DateTime.Now.ToString("HH:mm:ss");
                chart1.Series[0].Points.AddXY(DateTime.Now.ToString("HH:mm:ss"), VibroOutput);
            }
        }


    }
}
