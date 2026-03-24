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
    public partial class Form3 : Form
    {
        Session2Context db;
        private Employee currentuser;
        public Form3(Employee user)
        {
            InitializeComponent();
            currentuser = user;
            db = new Session2Context();
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            LoadRequests();
        }

        private void LoadRequests()
        {
            var requests = db.EmergencyMaintenances
                .Where(em => em.EmendDate == null)
                .Select(em => new
                {
                    em.Id,
                    em.Asset.AssetSn,
                    em.Asset.AssetName,
                    ReportDate = em.EmstartDate,
                    EmployeeName = em.Asset.Employee.FirstName + " " + em.Asset.Employee.LastName,
                    Department = em.Asset.DepartmentLocation.Department.Name,
                    em.PriorityId
                })
                .ToList();

            var sorted = requests
            .OrderByDescending(em => em.PriorityId)
            .ThenBy(em => em.ReportDate)
            .ToList();

            dataGridView1.DataSource = sorted;
            dataGridView1.Columns["Id"].Visible = false;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }
    }
}
