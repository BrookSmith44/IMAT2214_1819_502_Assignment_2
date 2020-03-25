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
            this.btnGetData = new System.Windows.Forms.Button();
            this.listBoxProductDestination = new System.Windows.Forms.ListBox();
            this.btnGetDestinationData = new System.Windows.Forms.Button();
            this.btnFactTable = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnGetData
            // 
            this.btnGetData.Location = new System.Drawing.Point(12, 396);
            this.btnGetData.Name = "btnGetData";
            this.btnGetData.Size = new System.Drawing.Size(101, 42);
            this.btnGetData.TabIndex = 0;
            this.btnGetData.Text = "Get Data From Source";
            this.btnGetData.UseVisualStyleBackColor = true;
            this.btnGetData.Click += new System.EventHandler(this.btnGetData_Click);
            // 
            // listBoxProductDestination
            // 
            this.listBoxProductDestination.FormattingEnabled = true;
            this.listBoxProductDestination.HorizontalScrollbar = true;
            this.listBoxProductDestination.Location = new System.Drawing.Point(12, 12);
            this.listBoxProductDestination.Name = "listBoxProductDestination";
            this.listBoxProductDestination.Size = new System.Drawing.Size(212, 186);
            this.listBoxProductDestination.TabIndex = 1;
            // 
            // btnGetDestinationData
            // 
            this.btnGetDestinationData.Location = new System.Drawing.Point(119, 396);
            this.btnGetDestinationData.Name = "btnGetDestinationData";
            this.btnGetDestinationData.Size = new System.Drawing.Size(105, 42);
            this.btnGetDestinationData.TabIndex = 2;
            this.btnGetDestinationData.Text = "Get Data From Destination";
            this.btnGetDestinationData.UseVisualStyleBackColor = true;
            this.btnGetDestinationData.Click += new System.EventHandler(this.btnGetDestinationData_Click);
            // 
            // btnFactTable
            // 
            this.btnFactTable.Location = new System.Drawing.Point(230, 396);
            this.btnFactTable.Name = "btnFactTable";
            this.btnFactTable.Size = new System.Drawing.Size(105, 42);
            this.btnFactTable.TabIndex = 3;
            this.btnFactTable.Text = "Build Fact Table";
            this.btnFactTable.UseVisualStyleBackColor = true;
            this.btnFactTable.Click += new System.EventHandler(this.btnFactTable_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.btnFactTable);
            this.Controls.Add(this.btnGetDestinationData);
            this.Controls.Add(this.listBoxProductDestination);
            this.Controls.Add(this.btnGetData);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnGetData;
        private System.Windows.Forms.ListBox listBoxProductDestination;
        private System.Windows.Forms.Button btnGetDestinationData;
        private System.Windows.Forms.Button btnFactTable;
    }
}

