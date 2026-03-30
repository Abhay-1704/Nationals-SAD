using APIWinforms.DataModel;
using System.Text.Json;

namespace APIWinforms
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        // Thread like function
        public async Task<List<CategoryModel>> GetCategories()
        {
            HttpClient client = new HttpClient();
            var response = await client.GetAsync("https://localhost:7290/api/Category");
            response.EnsureSuccessStatusCode();
            var jsonString = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            List<CategoryModel> categories = JsonSerializer.Deserialize<List<CategoryModel>>(jsonString, options);
            return categories;
        }

        private async void Form1_Load_1(object sender, EventArgs e)
        {
            List<CategoryModel> categories = await GetCategories();
            comboBox1.DataSource = categories;
            comboBox1.DisplayMember = "Name";
            comboBox1.ValueMember = "Id";

            List<ProductModel> products = await GetProducts((int)comboBox1.SelectedValue);
            DataGridViewCheckBoxColumn chk = new DataGridViewCheckBoxColumn();
            chk.Name = "Select";
            chk.HeaderText = "Select";
            dataGridView1.Columns.Insert(0, chk);
            dataGridView1.DataSource = products;

            //for update
            dataGridView1.ReadOnly = false;
        }

        public async Task<List<ProductModel>> GetProducts(int categoryId)
        {
            HttpClient client = new HttpClient();
            var response = await client.GetAsync($"https://localhost:7290/api/Product/{categoryId}");
            var jsonString = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                List<ProductModel> products = JsonSerializer.Deserialize<List<ProductModel>>(jsonString, options);
                return products;
            }
            else
            {
                MessageBox.Show(jsonString);
                return null;
            }
        }

        public async Task DeleteProduct(int productId)
        {
            HttpClient client = new HttpClient();

            var response = await client.DeleteAsync($"https://localhost:7290/api/Product/{productId}");

            if (!response.IsSuccessStatusCode)
            {
                var msg = await response.Content.ReadAsStringAsync();
                MessageBox.Show(msg);
            }
        }

        public async Task UpdateProduct(ProductModel product)
        {
            HttpClient client = new HttpClient();

            var json = JsonSerializer.Serialize(product);

            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

            var response = await client.PutAsync(
                $"https://localhost:7290/api/Product/{product.Id}",
                content
            );

            if (!response.IsSuccessStatusCode)
            {
                var msg = await response.Content.ReadAsStringAsync();
                MessageBox.Show(msg);
            }
        }

        //To update data grid if the value of combobox filter is changed
        private async void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            CategoryModel category = (CategoryModel)comboBox1.SelectedItem;
            List<ProductModel> products = await GetProducts(category.Id);
            dataGridView1.DataSource = products;
        }

        private void FormatDataGridView()
        {
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.Cells["Price"].Value != null && Convert.ToDouble(row.Cells["Price"].Value) < 10)
                {
                    //row.DefaultCellStyle.BackColor = Color.Red;
                    //row.DefaultCellStyle.ForeColor = Color.White;
                    row.Cells["Price"].Style.BackColor = Color.Red;
                    row.Cells["Price"].Style.ForeColor = Color.White;
                }
            }
        }

        private void dataGridView1_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            if (dataGridView1.Columns["Id"] != null)
            {
                dataGridView1.Columns["Id"].Visible = false;
            }

            FormatDataGridView();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                bool isChecked = Convert.ToBoolean(row.Cells["Select"].Value);

                if (isChecked)
                {
                    int productId = Convert.ToInt32(row.Cells["Id"].Value);

                    await DeleteProduct(productId);
                }
            }

            // Refresh data after delete
            CategoryModel category = (CategoryModel)comboBox1.SelectedItem;
            var products = await GetProducts(category.Id);

            dataGridView1.DataSource = null;
            dataGridView1.DataSource = products;
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.IsNewRow) continue;

                bool isChecked = row.Cells["Select"].Value != null && (bool)row.Cells["Select"].Value;

                if (isChecked)
                {
                    ProductModel product = new ProductModel
                    {
                        Id = Convert.ToInt32(row.Cells["Id"].Value),
                        Name = row.Cells["Name"].Value?.ToString(),
                        Price = row.Cells["Price"].Value != null
            ? Convert.ToDecimal(row.Cells["Price"].Value)
            : 0m,
                        Description = row.Cells["Description"].Value?.ToString(),
                        Image = row.Cells["Image"].Value?.ToString(),
                        CategoryId = Convert.ToInt32(row.Cells["CategoryId"].Value)
                    };

                    await UpdateProduct(product);
                }
            }

            // Refresh grid
            CategoryModel category = (CategoryModel)comboBox1.SelectedItem;
            var products = await GetProducts(category.Id);

            dataGridView1.DataSource = null;
            dataGridView1.DataSource = products;
        }
    }
}