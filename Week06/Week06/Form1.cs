using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using Week06.Entities;
using Week06.MnbServiceReference;

namespace Week06
{
    public partial class Form1 : Form
    {
        string result;
       
        BindingList<string> Currencies = new BindingList<string>();
        BindingList<RateData> Rates = new BindingList<RateData>();
        public Form1()
        {
            InitializeComponent();
            MNBArfolyamServiceSoap.GetCurrencies(GetCurrenciesResponse);
            RefreshData();
            comboBox1.DataSource = Currencies;
        }
        
       private void WebCall() 
        {
            var mnbService = new MNBArfolyamServiceSoapClient();
            var request = new GetExchangeRatesRequestBody()
            {
                currencyNames = (string)comboBox1.SelectedItem,
                startDate = Convert.ToString(dateTimePicker2.Value),
                endDate = Convert.ToString(dateTimePicker1.Value)

            };
            var response = mnbService.GetExchangeRates(request);
            result = response.GetExchangeRatesResult;

        }
        private void RefreshData()
        {
            Rates.Clear();
            
            dataGridView1.DataSource = Rates;

            WebCall();
            Diagram();
            XMLFeldolg();

        }
        private void Diagram()
        {
            chartRateData.DataSource = Rates;
            var series = chartRateData.Series[0];
            series.XValueMember = "Date";
            series.YValueMembers = "Value";
            series.BorderWidth = 2;
            var legend = chartRateData.Legends[0];
            legend.Enabled = false;
            var chartArea = chartRateData.ChartAreas[0];
            chartArea.AxisX.MajorGrid.Enabled = false;
            chartArea.AxisY.MajorGrid.Enabled = false;
            chartArea.AxisY.IsStartedFromZero = false;
        }
       private void XMLFeldolg()
        {

            
            var xml = new XmlDocument();
            xml.LoadXml(result);

            foreach(XmlElement element in xml.DocumentElement)
            {
                var rate = new RateData();
                Rates.Add(rate);
                rate.Date = DateTime.Parse(element.GetAttribute("date"));
                var childElement = (XmlElement)element.ChildNodes[0];
                rate.Currency = childElement.GetAttribute("curr");
                var unit = decimal.Parse(childElement.GetAttribute("unit"));
                var value = decimal.Parse(childElement.InnerText);
                if (unit!=0)
                {
                    rate.Value = value / unit;
                }
            }

        }

        private void chartRateData_Click(object sender, EventArgs e)
        {

        }

        private void rateDataBindingSource_CurrentChanged(object sender, EventArgs e)
        {

        }

        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {
            RefreshData();
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            RefreshData();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            RefreshData();
        }
    }
}
