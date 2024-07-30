namespace test_readBinary1
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            textBox_debug = new TextBox();
            button1 = new Button();
            pictureBox1 = new PictureBox();
            groupBox1 = new GroupBox();
            checkBox2 = new CheckBox();
            checkBox3 = new CheckBox();
            checkBox4 = new CheckBox();
            checkBox5 = new CheckBox();
            groupBox2 = new GroupBox();
            checkBox1 = new CheckBox();
            checkBox6 = new CheckBox();
            label1 = new Label();
            textBox1 = new TextBox();
            groupBox3 = new GroupBox();
            groupBox4 = new GroupBox();
            groupBox5 = new GroupBox();
            radioButton_3x3demosaic = new RadioButton();
            radioButton_2x2demosaic = new RadioButton();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            groupBox1.SuspendLayout();
            groupBox2.SuspendLayout();
            groupBox3.SuspendLayout();
            groupBox4.SuspendLayout();
            groupBox5.SuspendLayout();
            SuspendLayout();
            // 
            // textBox_debug
            // 
            textBox_debug.Font = new Font("Courier New", 9.75F, FontStyle.Regular, GraphicsUnit.Point);
            textBox_debug.ForeColor = SystemColors.ControlText;
            textBox_debug.Location = new Point(6, 28);
            textBox_debug.Multiline = true;
            textBox_debug.Name = "textBox_debug";
            textBox_debug.ScrollBars = ScrollBars.Vertical;
            textBox_debug.Size = new Size(659, 208);
            textBox_debug.TabIndex = 0;
            // 
            // button1
            // 
            button1.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            button1.Location = new Point(13, 24);
            button1.Name = "button1";
            button1.Size = new Size(281, 53);
            button1.TabIndex = 1;
            button1.Text = "Open RAW File";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // pictureBox1
            // 
            pictureBox1.Location = new Point(694, 33);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(255, 226);
            pictureBox1.TabIndex = 2;
            pictureBox1.TabStop = false;
            pictureBox1.Click += pictureBox1_Click;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(textBox_debug);
            groupBox1.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            groupBox1.Location = new Point(4, 443);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(673, 242);
            groupBox1.TabIndex = 4;
            groupBox1.TabStop = false;
            groupBox1.Text = "Debug Output";
            // 
            // checkBox2
            // 
            checkBox2.AutoSize = true;
            checkBox2.Checked = true;
            checkBox2.CheckState = CheckState.Checked;
            checkBox2.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            checkBox2.Location = new Point(13, 46);
            checkBox2.Name = "checkBox2";
            checkBox2.Size = new Size(447, 25);
            checkBox2.TabIndex = 5;
            checkBox2.Text = "Save 'step-1' bit-corrected 12-bit RAW values as a binary file";
            checkBox2.UseVisualStyleBackColor = true;
            checkBox2.CheckedChanged += checkBox1_CheckedChanged;
            // 
            // checkBox3
            // 
            checkBox3.AutoSize = true;
            checkBox3.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            checkBox3.Location = new Point(13, 68);
            checkBox3.Name = "checkBox3";
            checkBox3.Size = new Size(609, 25);
            checkBox3.TabIndex = 6;
            checkBox3.Text = "Save 'step-1' bit-corrected 12-bit RAW values in Hex+Binary format (for debugging)";
            checkBox3.UseVisualStyleBackColor = true;
            checkBox3.CheckedChanged += checkBox2_CheckedChanged;
            // 
            // checkBox4
            // 
            checkBox4.AutoSize = true;
            checkBox4.Checked = true;
            checkBox4.CheckState = CheckState.Checked;
            checkBox4.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            checkBox4.Location = new Point(13, 92);
            checkBox4.Name = "checkBox4";
            checkBox4.Size = new Size(437, 25);
            checkBox4.TabIndex = 7;
            checkBox4.Text = "Save 'step-2' demozaiced 12-bit RGB values as a binary file";
            checkBox4.UseVisualStyleBackColor = true;
            checkBox4.CheckedChanged += checkBox3_CheckedChanged;
            // 
            // checkBox5
            // 
            checkBox5.AutoSize = true;
            checkBox5.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            checkBox5.Location = new Point(13, 113);
            checkBox5.Name = "checkBox5";
            checkBox5.Size = new Size(599, 25);
            checkBox5.TabIndex = 8;
            checkBox5.Text = "Save 'step-2' demozaiced 12-bit RGB values in Hex+Binary format (for debugging)";
            checkBox5.UseVisualStyleBackColor = true;
            checkBox5.CheckedChanged += checkBox4_CheckedChanged;
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(checkBox1);
            groupBox2.Controls.Add(checkBox6);
            groupBox2.Controls.Add(checkBox5);
            groupBox2.Controls.Add(checkBox4);
            groupBox2.Controls.Add(checkBox3);
            groupBox2.Controls.Add(checkBox2);
            groupBox2.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            groupBox2.Location = new Point(12, 169);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(665, 170);
            groupBox2.TabIndex = 9;
            groupBox2.TabStop = false;
            groupBox2.Text = "Choose what files to save";
            // 
            // checkBox1
            // 
            checkBox1.AutoSize = true;
            checkBox1.Location = new Point(13, 23);
            checkBox1.Name = "checkBox1";
            checkBox1.Size = new Size(513, 25);
            checkBox1.TabIndex = 10;
            checkBox1.Text = "Save original RAW file values in in Hex+Binary format (for debugging)";
            checkBox1.UseVisualStyleBackColor = true;
            // 
            // checkBox6
            // 
            checkBox6.AutoSize = true;
            checkBox6.Checked = true;
            checkBox6.CheckState = CheckState.Checked;
            checkBox6.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            checkBox6.Location = new Point(13, 138);
            checkBox6.Name = "checkBox6";
            checkBox6.Size = new Size(299, 25);
            checkBox6.TabIndex = 9;
            checkBox6.Text = "Save as a viewable 32bit RGBA TIFF file";
            checkBox6.UseVisualStyleBackColor = true;
            checkBox6.CheckedChanged += checkBox5_CheckedChanged;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(15, 88);
            label1.Name = "label1";
            label1.Size = new Size(0, 21);
            label1.TabIndex = 10;
            // 
            // textBox1
            // 
            textBox1.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point);
            textBox1.Location = new Point(8, 23);
            textBox1.Multiline = true;
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(644, 128);
            textBox1.TabIndex = 11;
            textBox1.Text = resources.GetString("textBox1.Text");
            // 
            // groupBox3
            // 
            groupBox3.Controls.Add(textBox1);
            groupBox3.Controls.Add(label1);
            groupBox3.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            groupBox3.Location = new Point(17, 10);
            groupBox3.Name = "groupBox3";
            groupBox3.Size = new Size(660, 156);
            groupBox3.TabIndex = 12;
            groupBox3.TabStop = false;
            groupBox3.Text = "Program Description";
            // 
            // groupBox4
            // 
            groupBox4.Controls.Add(button1);
            groupBox4.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            groupBox4.Location = new Point(12, 357);
            groupBox4.Name = "groupBox4";
            groupBox4.Size = new Size(312, 82);
            groupBox4.TabIndex = 13;
            groupBox4.TabStop = false;
            groupBox4.Text = "Select the RAW file to be processed";
            // 
            // groupBox5
            // 
            groupBox5.Controls.Add(radioButton_3x3demosaic);
            groupBox5.Controls.Add(radioButton_2x2demosaic);
            groupBox5.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            groupBox5.Location = new Point(342, 359);
            groupBox5.Name = "groupBox5";
            groupBox5.Size = new Size(335, 78);
            groupBox5.TabIndex = 14;
            groupBox5.TabStop = false;
            groupBox5.Text = "Choose Demozaic Method";
            // 
            // radioButton_3x3demosaic
            // 
            radioButton_3x3demosaic.AutoSize = true;
            radioButton_3x3demosaic.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            radioButton_3x3demosaic.Location = new Point(45, 47);
            radioButton_3x3demosaic.Name = "radioButton_3x3demosaic";
            radioButton_3x3demosaic.Size = new Size(117, 25);
            radioButton_3x3demosaic.TabIndex = 1;
            radioButton_3x3demosaic.TabStop = true;
            radioButton_3x3demosaic.Text = "3x3 Demosic";
            radioButton_3x3demosaic.UseVisualStyleBackColor = true;
            radioButton_3x3demosaic.CheckedChanged += radioButton_3x3demosaic_CheckedChanged;
            // 
            // radioButton_2x2demosaic
            // 
            radioButton_2x2demosaic.AutoSize = true;
            radioButton_2x2demosaic.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            radioButton_2x2demosaic.Location = new Point(45, 22);
            radioButton_2x2demosaic.Name = "radioButton_2x2demosaic";
            radioButton_2x2demosaic.Size = new Size(178, 25);
            radioButton_2x2demosaic.TabIndex = 0;
            radioButton_2x2demosaic.TabStop = true;
            radioButton_2x2demosaic.Text = "2x2 Demosaic (faster)";
            radioButton_2x2demosaic.UseVisualStyleBackColor = true;
            radioButton_2x2demosaic.CheckedChanged += radioButton_2x2demosaic_CheckedChanged;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(694, 684);
            Controls.Add(groupBox5);
            Controls.Add(groupBox4);
            Controls.Add(groupBox3);
            Controls.Add(groupBox2);
            Controls.Add(groupBox1);
            Controls.Add(pictureBox1);
            Name = "Form1";
            Text = "JXA2 RAW Image Formatter";
            Load += Form1_Load;
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            groupBox3.ResumeLayout(false);
            groupBox3.PerformLayout();
            groupBox4.ResumeLayout(false);
            groupBox5.ResumeLayout(false);
            groupBox5.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private TextBox textBox_debug;
        private Button button1;
        private PictureBox pictureBox1;
        private GroupBox groupBox1;
        private CheckBox checkBox2;
        private CheckBox checkBox3;
        private CheckBox checkBox4;
        private CheckBox checkBox5;
        private GroupBox groupBox2;
        private CheckBox checkBox6;
        private Label label1;
        private TextBox textBox1;
        private GroupBox groupBox3;
        private GroupBox groupBox4;
        private CheckBox checkBox1;
        private GroupBox groupBox5;
        private RadioButton radioButton_3x3demosaic;
        private RadioButton radioButton_2x2demosaic;
    }
}