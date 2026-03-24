using Session2_2019.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Session2_2019
{
    public partial class Form2 : Form
    {
        Session2Context db;
        private Employee currentUser;
        public Form2(Employee user)
        {
            InitializeComponent();
            currentUser = user;
            db = new Session2Context();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            LoadAssets();
        }

        private void LoadAssets()
        {
            var assets = db.Assets
                .Where(a => a.EmployeeId == currentUser.Id)
                .Select(a => new
                {
                    a.Id,
                    a.AssetSn,
                    a.AssetName,

                    LastClosedEM = db.EmergencyMaintenances
                    .Where(em => em.AssetId == a.Id && em.EmendDate != null)
                    .OrderByDescending(em => em.EmendDate)
                    .Select(em => em.EmendDate)
                    .FirstOrDefault(),

                    NumberOfEMs = db.EmergencyMaintenances
                    .Count(em => em.AssetId == a.Id && em.EmendDate != null),

                    HasOpenRequest = db.EmergencyMaintenances
                    .Any(em => em.AssetId == a.Id && em.EmendDate == null)
                }).ToList();

            dataGridView1.DataSource = assets;
            dataGridView1.Columns["Id"].Visible = false;
            dataGridView1.Columns["HasOpenRequest"].Visible = false;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int assetId = Convert.ToInt32(dataGridView1.CurrentRow.Cells["AssetId"].Value);
            Form4 request = new Form4(assetId, currentUser);
            this.Hide();
            request.Show();
        }
    }
}
