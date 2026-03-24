using Session2_2019.Models;

namespace Session2_2019
{
    public partial class Form1 : Form
    {
        Session2Context db;
        public Form1()
        {
            InitializeComponent();
            db = new Session2Context();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string username = textBox1.Text;
            string password = textBox2.Text;

            //Check-case for invalid login the null-case
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Please Enter Valid Username and Password");
                return;
            }

            //Check for user in database
            var user = db.Employees.FirstOrDefault(x => x.Username == username && x.Password == password);

            if (user == null)
            {
                MessageBox.Show("Please Enter Valid Username and Password");
                return;
            }

            MessageBox.Show("Login Successful");

            if (user.IsAdmin == true)
            {
                Form3 manager = new Form3(user);
                this.Hide();
                manager.Show();
            } else
            {
                Form2 employee = new Form2(user);
                this.Hide();
                employee.Show();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
