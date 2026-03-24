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
    public partial class Form4 : Form
    {
        Session2Context db;
        int assetId;
        Employee currentuser;
        public Form4(int assetId, Employee user)
        {
            InitializeComponent();
            db = new Session2Context();
            this.assetId = assetId;
            currentuser = user;
        }

        private void Form4_Load(object sender, EventArgs e)
        {
            var asset = db.Assets
                .Where(a => a.Id == assetId)
                .Select(a => new
                {
                    a.AssetSn,
                    a.AssetName,
                    Department = a.DepartmentLocation.Department.Name
                })
                .FirstOrDefault();

            label4.Text = asset.AssetSn;
            label5.Text = asset.AssetName;
            label6.Text = asset.Department;

            comboBox1.Items.Add("Very High");
            comboBox1.Items.Add("High");
            comboBox1.Items.Add("Normal");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedValue == null)
            {
                MessageBox.Show("Please select priority");
                return;
            }

            if (string.IsNullOrWhiteSpace(textBox1.Text))
            {
                MessageBox.Show("Description is required");
                return;
            }

            bool hasOpenRequest = db.EmergencyMaintenances
                .Any(em => em.AssetId == assetId && em.EmendDate == null);

            if (hasOpenRequest)
            {
                MessageBox.Show("This asset already has an open request!");
                return;
            }

            try
            {
                EmergencyMaintenance newRequest = new EmergencyMaintenance()
                {
                    AssetId = assetId,
                    PriorityId = Convert.ToInt32(comboBox1.SelectedValue),
                    DescriptionEmergency = textBox1.Text.Trim(),
                    OtherConsiderations = textBox2.Text.Trim(),
                    EmstartDate = DateOnly.FromDateTime(DateTime.Now),
                };

                db.EmergencyMaintenances.Add(newRequest);
                db.SaveChanges();

                MessageBox.Show("Request sent successfully!");
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }
    }
}
