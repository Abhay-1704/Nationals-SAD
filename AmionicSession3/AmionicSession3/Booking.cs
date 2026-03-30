using AmionicSession3.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AmionicSession3
{
    public partial class Booking : Form
    {
        Session3Context db;
        int outboundFlight = 0;
        int returnFlight = 0;
        decimal passengers = 0;
        public Booking(int outboundFlight, int returnFlight, decimal passengers)
        {
            InitializeComponent();
            db = new Session3Context();
            this.outboundFlight = outboundFlight;
            this.returnFlight = returnFlight;
            this.passengers = passengers;
        }

        private void Booking_Load(object sender, EventArgs e)
        {
            Schedule flightoutbound = GetFlightDetails(outboundFlight);
            label6.Text = flightoutbound.Route.DepartureAirport.Iatacode;
            label7.Text = flightoutbound.Route.ArrivalAirport.Iatacode;
            label8.Text = Convert.ToString(flightoutbound.Date);
            label9.Text = flightoutbound.FlightNumber;

            if (returnFlight != 0)
            {
                Schedule flightreturn = GetFlightDetails(returnFlight);
                label13.Text = flightreturn.Route.DepartureAirport.Iatacode;
                label12.Text = flightreturn.Route.ArrivalAirport.Iatacode;
                label11.Text = Convert.ToString(flightreturn.Date);
                label10.Text = flightreturn.FlightNumber;
            }
            GetFlightDetails(returnFlight);
            PopulateCountries();
        }

        private void PopulateCountries()
        {
            List<Country> countries = db.Countries.ToList();
            comboBox1.DataSource = countries;
            comboBox1.DisplayMember = "Name";
            comboBox1.ValueMember = "Id";
        }

        public Schedule GetFlightDetails(int flightId)
        {
            return db.Schedules
                .Include(x => x.Route)
                    .ThenInclude(r => r.DepartureAirport)
                .Include(x => x.Route)
                    .ThenInclude(r => r.ArrivalAirport)
                .FirstOrDefault(x => x.Id == flightId);
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }
    }
}
