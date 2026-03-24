namespace Session2_2019
{
    partial class Form4
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            label4 = new Label();
            label5 = new Label();
            label6 = new Label();
            label7 = new Label();
            label8 = new Label();
            label9 = new Label();
            label10 = new Label();
            label11 = new Label();
            comboBox1 = new ComboBox();
            textBox1 = new TextBox();
            textBox2 = new TextBox();
            button1 = new Button();
            button2 = new Button();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(56, 70);
            label1.Name = "label1";
            label1.Size = new Size(59, 15);
            label1.TabIndex = 0;
            label1.Text = "Asset SN :";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(289, 70);
            label2.Name = "label2";
            label2.Size = new Size(76, 15);
            label2.TabIndex = 1;
            label2.Text = "Asset Name :";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(511, 70);
            label3.Name = "label3";
            label3.Size = new Size(76, 15);
            label3.TabIndex = 2;
            label3.Text = "Department :";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label4.Location = new Point(121, 70);
            label4.Name = "label4";
            label4.Size = new Size(17, 15);
            label4.TabIndex = 3;
            label4.Text = "--";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label5.Location = new Point(371, 70);
            label5.Name = "label5";
            label5.Size = new Size(17, 15);
            label5.TabIndex = 4;
            label5.Text = "--";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label6.Location = new Point(593, 70);
            label6.Name = "label6";
            label6.Size = new Size(17, 15);
            label6.TabIndex = 5;
            label6.Text = "--";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Font = new Font("Stencil", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label7.Location = new Point(52, 35);
            label7.Name = "label7";
            label7.Size = new Size(148, 19);
            label7.TabIndex = 6;
            label7.Text = "Selected Asset";
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Font = new Font("Stencil", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label8.Location = new Point(52, 109);
            label8.Name = "label8";
            label8.Size = new Size(157, 19);
            label8.TabIndex = 7;
            label8.Text = "Request Report";
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Location = new Point(56, 145);
            label9.Name = "label9";
            label9.Size = new Size(51, 15);
            label9.TabIndex = 8;
            label9.Text = "Priority :";
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Location = new Point(56, 180);
            label10.Name = "label10";
            label10.Size = new Size(149, 15);
            label10.TabIndex = 9;
            label10.Text = "Description of Emergency :";
            // 
            // label11
            // 
            label11.AutoSize = true;
            label11.Location = new Point(56, 268);
            label11.Name = "label11";
            label11.Size = new Size(125, 15);
            label11.TabIndex = 10;
            label11.Text = "Other Considerations :";
            // 
            // comboBox1
            // 
            comboBox1.FormattingEnabled = true;
            comboBox1.Location = new Point(121, 142);
            comboBox1.Name = "comboBox1";
            comboBox1.Size = new Size(267, 23);
            comboBox1.TabIndex = 11;
            // 
            // textBox1
            // 
            textBox1.Location = new Point(121, 211);
            textBox1.Multiline = true;
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(591, 43);
            textBox1.TabIndex = 12;
            // 
            // textBox2
            // 
            textBox2.Location = new Point(121, 295);
            textBox2.Multiline = true;
            textBox2.Name = "textBox2";
            textBox2.Size = new Size(591, 43);
            textBox2.TabIndex = 13;
            // 
            // button1
            // 
            button1.Location = new Point(241, 359);
            button1.Name = "button1";
            button1.Size = new Size(113, 28);
            button1.TabIndex = 14;
            button1.Text = "Send Request";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // button2
            // 
            button2.Location = new Point(383, 359);
            button2.Name = "button2";
            button2.Size = new Size(113, 28);
            button2.TabIndex = 15;
            button2.Text = "Cancel";
            button2.UseVisualStyleBackColor = true;
            // 
            // Form4
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(744, 409);
            Controls.Add(button2);
            Controls.Add(button1);
            Controls.Add(textBox2);
            Controls.Add(textBox1);
            Controls.Add(comboBox1);
            Controls.Add(label11);
            Controls.Add(label10);
            Controls.Add(label9);
            Controls.Add(label8);
            Controls.Add(label7);
            Controls.Add(label6);
            Controls.Add(label5);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Name = "Form4";
            Text = "Emergency Maintenance Request";
            Load += Form4_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private Label label6;
        private Label label7;
        private Label label8;
        private Label label9;
        private Label label10;
        private Label label11;
        private ComboBox comboBox1;
        private TextBox textBox1;
        private TextBox textBox2;
        private Button button1;
        private Button button2;
    }
}