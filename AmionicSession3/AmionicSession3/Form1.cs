using AmionicSession3.Models;

namespace AmionicSession3
{
    public partial class Form1 : Form
    {
        Session3Context db;
        public Form1()
        {
            InitializeComponent();
            db = new Session3Context();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadComboBoxes();
            //dateTimePicker1.MinDate = DateTime.Now;
            //dateTimePicker2.MinDate = DateTime.Now;
            radioButton2.Checked = true;

            dateTimePicker1.Value = new DateTime(2017, 10, 04);
            dateTimePicker2.Value = new DateTime(2017, 10, 04);

            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView2.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.MultiSelect = false;
            dataGridView2.MultiSelect = false;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void LoadComboBoxes()
        {
            List<Airport> fromAirports = db.Airports.ToList();
            comboBox1.DataSource = fromAirports;
            comboBox1.ValueMember = "id";
            comboBox1.DisplayMember = "IATACode";
            List<Airport> toAirports = db.Airports.ToList();
            comboBox2.DataSource = toAirports;
            comboBox2.ValueMember = "id";
            comboBox2.DisplayMember = "IATACode";

            List<CabinType> cabinTypes = db.CabinTypes.ToList();
            comboBox3.DataSource = cabinTypes;
            comboBox3.ValueMember = "id";
            comboBox3.DisplayMember = "Name";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void radioButton1_CheckedChanged_1(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                dateTimePicker2.Enabled = true;
                dataGridView2.Visible = true;
            }
        }

        private void radioButton2_CheckedChanged_1(object sender, EventArgs e)
        {
            if (radioButton2.Checked)
            {
                dateTimePicker2.Enabled = false;
                dataGridView2.Visible = false;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int fromAirport = (int)comboBox1.SelectedValue;
            int toAirport = (int)comboBox2.SelectedValue;
            int cabinType = (int)comboBox3.SelectedValue;

            if (comboBox1.SelectedItem == comboBox2.SelectedItem)
            {
                MessageBox.Show("From and two cannot be same");
                return;
            }

            var outboundflights = db.Schedules.Where(x => x.Route.DepartureAirportId == fromAirport && x.Route.ArrivalAirportId == toAirport && x.Date == DateOnly.FromDateTime(dateTimePicker1.Value))
                .Select(x => new
                {
                    Id = x.Id,
                    From = x.Route.DepartureAirport.Iatacode,
                    To = x.Route.ArrivalAirport.Iatacode,
                    Date = x.Date,
                    Time = x.Time,
                    FlightNumber = x.FlightNumber,
                    CabinPrice = x.EconomyPrice
                    // NumberofStops = x.
                }).ToList();

            if (dateTimePicker1.Value.Date > dateTimePicker2.Value.Date)
            {
                MessageBox.Show("Return Should be after outbound");
                return;
            }

            if (checkBox1.Checked)
            {
                outboundflights = db.Schedules.Where(x => x.Route.DepartureAirportId == fromAirport && x.Route.ArrivalAirportId == toAirport && x.Date > DateOnly.FromDateTime(dateTimePicker1.Value.AddDays(-3)) && x.Date < DateOnly.FromDateTime(dateTimePicker1.Value.AddDays(3)))
                .Select(x => new
                {
                    Id = x.Id,
                    From = x.Route.DepartureAirport.Iatacode,
                    To = x.Route.ArrivalAirport.Iatacode,
                    Date = x.Date,
                    Time = x.Time,
                    FlightNumber = x.FlightNumber,
                    CabinPrice = x.EconomyPrice
                    // NumberofStops = x.
                }).ToList();
            }

            var flights = outboundflights.Select(x => new
            {
                Id = x.Id,
                From = x.From,
                To = x.To,
                Date = x.Date,
                Time = x.Time,
                FlightNumber = x.FlightNumber,
                CabinPrice = CalculatePrice(x.CabinPrice, cabinType)
                // NumberofStops = x.
            }).ToList();
            dataGridView1.DataSource = flights;

            if (radioButton1.Checked)
            {
                var returnflights = db.Schedules.Where(x => x.Route.DepartureAirportId == toAirport && x.Route.ArrivalAirportId == fromAirport && x.Date == DateOnly.FromDateTime(dateTimePicker2.Value))
                .Select(x => new
                {
                    Id = x.Id,
                    From = x.Route.DepartureAirport.Iatacode,
                    To = x.Route.ArrivalAirport.Iatacode,
                    Date = x.Date,
                    Time = x.Time,
                    FlightNumber = x.FlightNumber,
                    CabinPrice = x.EconomyPrice
                    // NumberofStops = x.
                }).ToList();

                if (checkBox2.Checked)
                {
                    returnflights = db.Schedules.Where(x => x.Route.DepartureAirportId == toAirport && x.Route.ArrivalAirportId == fromAirport && x.Date > DateOnly.FromDateTime(dateTimePicker2.Value.AddDays(-3)) && x.Date < DateOnly.FromDateTime(dateTimePicker2.Value.AddDays(3)))
                    .Select(x => new
                    {
                        Id = x.Id,
                        From = x.Route.DepartureAirport.Iatacode,
                        To = x.Route.ArrivalAirport.Iatacode,
                        Date = x.Date,
                        Time = x.Time,
                        FlightNumber = x.FlightNumber,
                        CabinPrice = x.EconomyPrice
                        // NumberofStops = x.
                    }).ToList();
                }

                var flightsReturn = returnflights.Select(x => new
                {
                    Id = x.Id,
                    From = x.From,
                    To = x.To,
                    Date = x.Date,
                    Time = x.Time,
                    FlightNumber = x.FlightNumber,
                    CabinPrice = CalculatePrice(x.CabinPrice, cabinType)
                    // NumberofStops = x.
                }).ToList();
                dataGridView2.DataSource = flightsReturn;
            }
        }

        private decimal CalculatePrice(decimal economyPrice, int cabinType)
        {
            decimal price = (decimal)economyPrice;

            if (cabinType == 2)
            {
                price = price + (price * 0.35m);
            }

            if (cabinType == 3)
            {
                price = price + (price * 0.35m);
                price = price + (price * 0.30m);
            }

            return price;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            int returnId = 0;
            int outboundId = 0;
            if (numericUpDown1 == null || numericUpDown1.Value == 0)
            {
                MessageBox.Show("Please enter valid passenger count");
                return;
            }

            if (dataGridView1.SelectedRows.Count == 0 )
            {
                MessageBox.Show("Please Select a Row");
                return;
            }

            if (radioButton1.Checked)
            {
                if (dataGridView2.SelectedRows.Count == 0 )
                {
                    MessageBox.Show("Please Select a return flght");
                    return;
                }

                //return radio button to allow selecting return datagrid
                DataGridViewRow returnRow = dataGridView2.SelectedRows[0];
                returnId = (int)returnRow.Cells["id"].Value;
            }

            //for selecting outbound flights
            DataGridViewRow outboundRow = dataGridView1.SelectedRows[0];
            outboundId = (int)outboundRow.Cells["id"].Value;

            Booking booking = new Booking(outboundId, returnId, numericUpDown1.Value);
            this.Hide();
            booking.Show();

            //MessageBox.Show($"Outbound Id {outboundId},Return Id {returnId}");
        }
    }
}