namespace IMAT2214_1819_502_Assignment_2
{
    partial class Form1
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            this.btnGetData = new System.Windows.Forms.Button();
            this.listBoxProductDestination = new System.Windows.Forms.ListBox();
            this.btnGetDestinationData = new System.Windows.Forms.Button();
            this.btnFactTable = new System.Windows.Forms.Button();
            this.dataGridProduct = new System.Windows.Forms.DataGridView();
            this.labelProductDimension = new System.Windows.Forms.Label();
            this.labelLine = new System.Windows.Forms.Label();
            this.labelTimeDimension = new System.Windows.Forms.Label();
            this.dataGridViewTime = new System.Windows.Forms.DataGridView();
            this.listBoxTimeDimension = new System.Windows.Forms.ListBox();
            this.btnMinimize = new System.Windows.Forms.Button();
            this.btnMaximise = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.dataGridCustomer = new System.Windows.Forms.DataGridView();
            this.listBoxCustomerDimension = new System.Windows.Forms.ListBox();
            this.labelCustomerDimension = new System.Windows.Forms.Label();
            this.dataGridFactTable = new System.Windows.Forms.DataGridView();
            this.labelFactTable = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridProduct)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewTime)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridCustomer)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridFactTable)).BeginInit();
            this.SuspendLayout();
            // 
            // btnGetData
            // 
            this.btnGetData.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnGetData.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btnGetData.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGetData.Location = new System.Drawing.Point(12, 585);
            this.btnGetData.Name = "btnGetData";
            this.btnGetData.Size = new System.Drawing.Size(101, 42);
            this.btnGetData.TabIndex = 0;
            this.btnGetData.Text = "Get Data From Source";
            this.btnGetData.UseVisualStyleBackColor = false;
            this.btnGetData.Click += new System.EventHandler(this.btnGetData_Click);
            // 
            // listBoxProductDestination
            // 
            this.listBoxProductDestination.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.listBoxProductDestination.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.listBoxProductDestination.ForeColor = System.Drawing.Color.LightGreen;
            this.listBoxProductDestination.FormattingEnabled = true;
            this.listBoxProductDestination.HorizontalScrollbar = true;
            this.listBoxProductDestination.Location = new System.Drawing.Point(12, 360);
            this.listBoxProductDestination.Name = "listBoxProductDestination";
            this.listBoxProductDestination.Size = new System.Drawing.Size(212, 195);
            this.listBoxProductDestination.TabIndex = 1;
            // 
            // btnGetDestinationData
            // 
            this.btnGetDestinationData.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnGetDestinationData.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btnGetDestinationData.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGetDestinationData.Location = new System.Drawing.Point(119, 585);
            this.btnGetDestinationData.Name = "btnGetDestinationData";
            this.btnGetDestinationData.Size = new System.Drawing.Size(105, 42);
            this.btnGetDestinationData.TabIndex = 2;
            this.btnGetDestinationData.Text = "Get Data From Destination";
            this.btnGetDestinationData.UseVisualStyleBackColor = false;
            this.btnGetDestinationData.Click += new System.EventHandler(this.btnGetDestinationData_Click);
            // 
            // btnFactTable
            // 
            this.btnFactTable.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnFactTable.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFactTable.Location = new System.Drawing.Point(230, 585);
            this.btnFactTable.Name = "btnFactTable";
            this.btnFactTable.Size = new System.Drawing.Size(105, 42);
            this.btnFactTable.TabIndex = 3;
            this.btnFactTable.Text = "Build Fact Table";
            this.btnFactTable.UseVisualStyleBackColor = true;
            this.btnFactTable.Click += new System.EventHandler(this.btnFactTable_Click);
            // 
            // dataGridProduct
            // 
            this.dataGridProduct.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.dataGridProduct.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dataGridProduct.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridProduct.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.dataGridProduct.Location = new System.Drawing.Point(259, 360);
            this.dataGridProduct.Name = "dataGridProduct";
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.Black;
            this.dataGridProduct.RowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridProduct.Size = new System.Drawing.Size(330, 195);
            this.dataGridProduct.TabIndex = 7;
            // 
            // labelProductDimension
            // 
            this.labelProductDimension.AutoSize = true;
            this.labelProductDimension.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelProductDimension.Location = new System.Drawing.Point(8, 311);
            this.labelProductDimension.Name = "labelProductDimension";
            this.labelProductDimension.Size = new System.Drawing.Size(160, 20);
            this.labelProductDimension.TabIndex = 9;
            this.labelProductDimension.Text = "Product Dimension";
            // 
            // labelLine
            // 
            this.labelLine.AutoSize = true;
            this.labelLine.Location = new System.Drawing.Point(-30, 288);
            this.labelLine.Name = "labelLine";
            this.labelLine.Size = new System.Drawing.Size(1627, 13);
            this.labelLine.TabIndex = 10;
            this.labelLine.Text = resources.GetString("labelLine.Text");
            // 
            // labelTimeDimension
            // 
            this.labelTimeDimension.AutoSize = true;
            this.labelTimeDimension.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelTimeDimension.Location = new System.Drawing.Point(12, 44);
            this.labelTimeDimension.Name = "labelTimeDimension";
            this.labelTimeDimension.Size = new System.Drawing.Size(136, 20);
            this.labelTimeDimension.TabIndex = 11;
            this.labelTimeDimension.Text = "Time Dimension";
            // 
            // dataGridViewTime
            // 
            this.dataGridViewTime.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.dataGridViewTime.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dataGridViewTime.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewTime.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.dataGridViewTime.Location = new System.Drawing.Point(259, 90);
            this.dataGridViewTime.Name = "dataGridViewTime";
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.Black;
            this.dataGridViewTime.RowsDefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridViewTime.Size = new System.Drawing.Size(330, 195);
            this.dataGridViewTime.TabIndex = 13;
            // 
            // listBoxTimeDimension
            // 
            this.listBoxTimeDimension.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.listBoxTimeDimension.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.listBoxTimeDimension.ForeColor = System.Drawing.Color.LightGreen;
            this.listBoxTimeDimension.FormattingEnabled = true;
            this.listBoxTimeDimension.HorizontalScrollbar = true;
            this.listBoxTimeDimension.Location = new System.Drawing.Point(16, 90);
            this.listBoxTimeDimension.Name = "listBoxTimeDimension";
            this.listBoxTimeDimension.Size = new System.Drawing.Size(212, 195);
            this.listBoxTimeDimension.TabIndex = 12;
            // 
            // btnMinimize
            // 
            this.btnMinimize.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnMinimize.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btnMinimize.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMinimize.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.btnMinimize.Location = new System.Drawing.Point(1124, 0);
            this.btnMinimize.Name = "btnMinimize";
            this.btnMinimize.Size = new System.Drawing.Size(32, 23);
            this.btnMinimize.TabIndex = 6;
            this.btnMinimize.Text = "_";
            this.btnMinimize.UseVisualStyleBackColor = false;
            this.btnMinimize.Click += new System.EventHandler(this.btnMinimize_Click);
            // 
            // btnMaximise
            // 
            this.btnMaximise.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnMaximise.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btnMaximise.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMaximise.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.btnMaximise.Location = new System.Drawing.Point(1152, 0);
            this.btnMaximise.Name = "btnMaximise";
            this.btnMaximise.Size = new System.Drawing.Size(29, 23);
            this.btnMaximise.TabIndex = 5;
            this.btnMaximise.Text = "M";
            this.btnMaximise.UseVisualStyleBackColor = false;
            this.btnMaximise.Click += new System.EventHandler(this.btnMaximise_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.btnClose.Location = new System.Drawing.Point(1177, 0);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(28, 23);
            this.btnClose.TabIndex = 4;
            this.btnClose.Text = "X";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.LightGreen;
            this.panel1.Controls.Add(this.btnClose);
            this.panel1.Controls.Add(this.btnMaximise);
            this.panel1.Controls.Add(this.btnMinimize);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1205, 32);
            this.panel1.TabIndex = 8;
            this.panel1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.panel1_MouseMove);
            // 
            // dataGridCustomer
            // 
            this.dataGridCustomer.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.dataGridCustomer.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dataGridCustomer.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridCustomer.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.dataGridCustomer.Location = new System.Drawing.Point(867, 90);
            this.dataGridCustomer.Name = "dataGridCustomer";
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.Black;
            this.dataGridCustomer.RowsDefaultCellStyle = dataGridViewCellStyle3;
            this.dataGridCustomer.Size = new System.Drawing.Size(330, 195);
            this.dataGridCustomer.TabIndex = 16;
            // 
            // listBoxCustomerDimension
            // 
            this.listBoxCustomerDimension.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.listBoxCustomerDimension.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.listBoxCustomerDimension.ForeColor = System.Drawing.Color.LightGreen;
            this.listBoxCustomerDimension.FormattingEnabled = true;
            this.listBoxCustomerDimension.HorizontalScrollbar = true;
            this.listBoxCustomerDimension.Location = new System.Drawing.Point(624, 90);
            this.listBoxCustomerDimension.Name = "listBoxCustomerDimension";
            this.listBoxCustomerDimension.Size = new System.Drawing.Size(212, 195);
            this.listBoxCustomerDimension.TabIndex = 15;
            // 
            // labelCustomerDimension
            // 
            this.labelCustomerDimension.AutoSize = true;
            this.labelCustomerDimension.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelCustomerDimension.Location = new System.Drawing.Point(620, 44);
            this.labelCustomerDimension.Name = "labelCustomerDimension";
            this.labelCustomerDimension.Size = new System.Drawing.Size(175, 20);
            this.labelCustomerDimension.TabIndex = 14;
            this.labelCustomerDimension.Text = "Customer Dimension";
            // 
            // dataGridFactTable
            // 
            this.dataGridFactTable.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.dataGridFactTable.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dataGridFactTable.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridFactTable.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.dataGridFactTable.Location = new System.Drawing.Point(624, 360);
            this.dataGridFactTable.Name = "dataGridFactTable";
            dataGridViewCellStyle4.ForeColor = System.Drawing.Color.Black;
            this.dataGridFactTable.RowsDefaultCellStyle = dataGridViewCellStyle4;
            this.dataGridFactTable.Size = new System.Drawing.Size(573, 195);
            this.dataGridFactTable.TabIndex = 17;
            // 
            // labelFactTable
            // 
            this.labelFactTable.AutoSize = true;
            this.labelFactTable.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelFactTable.Location = new System.Drawing.Point(620, 311);
            this.labelFactTable.Name = "labelFactTable";
            this.labelFactTable.Size = new System.Drawing.Size(94, 20);
            this.labelFactTable.TabIndex = 18;
            this.labelFactTable.Text = "Fact Table";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.ClientSize = new System.Drawing.Size(1205, 639);
            this.Controls.Add(this.labelFactTable);
            this.Controls.Add(this.dataGridFactTable);
            this.Controls.Add(this.dataGridCustomer);
            this.Controls.Add(this.listBoxCustomerDimension);
            this.Controls.Add(this.labelCustomerDimension);
            this.Controls.Add(this.dataGridViewTime);
            this.Controls.Add(this.listBoxTimeDimension);
            this.Controls.Add(this.labelTimeDimension);
            this.Controls.Add(this.labelLine);
            this.Controls.Add(this.labelProductDimension);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.dataGridProduct);
            this.Controls.Add(this.btnFactTable);
            this.Controls.Add(this.btnGetDestinationData);
            this.Controls.Add(this.listBoxProductDestination);
            this.Controls.Add(this.btnGetData);
            this.ForeColor = System.Drawing.Color.LightGreen;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridProduct)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewTime)).EndInit();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridCustomer)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridFactTable)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnGetData;
        private System.Windows.Forms.ListBox listBoxProductDestination;
        private System.Windows.Forms.Button btnGetDestinationData;
        private System.Windows.Forms.Button btnFactTable;
        private System.Windows.Forms.DataGridView dataGridProduct;
        private System.Windows.Forms.Label labelProductDimension;
        private System.Windows.Forms.Label labelLine;
        private System.Windows.Forms.Label labelTimeDimension;
        private System.Windows.Forms.DataGridView dataGridViewTime;
        private System.Windows.Forms.ListBox listBoxTimeDimension;
        private System.Windows.Forms.Button btnMinimize;
        private System.Windows.Forms.Button btnMaximise;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.DataGridView dataGridCustomer;
        private System.Windows.Forms.ListBox listBoxCustomerDimension;
        private System.Windows.Forms.Label labelCustomerDimension;
        private System.Windows.Forms.DataGridView dataGridFactTable;
        private System.Windows.Forms.Label labelFactTable;
    }
}

