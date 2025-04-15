
namespace AsyncTcpSrvr
{
    partial class AsyncTcpSrvr
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
            this.results = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.conStatus = new System.Windows.Forms.TextBox();
            this.btnStop = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // results
            // 
            this.results.FormattingEnabled = true;
            this.results.ItemHeight = 25;
            this.results.Location = new System.Drawing.Point(26, 168);
            this.results.Name = "results";
            this.results.Size = new System.Drawing.Size(897, 254);
            this.results.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(31, 122);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(253, 25);
            this.label1.TabIndex = 1;
            this.label1.Text = "Text received from client:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(31, 23);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(194, 25);
            this.label2.TabIndex = 2;
            this.label2.Text = "Connection Status:";
            // 
            // conStatus
            // 
            this.conStatus.Location = new System.Drawing.Point(26, 72);
            this.conStatus.Name = "conStatus";
            this.conStatus.Size = new System.Drawing.Size(897, 31);
            this.conStatus.TabIndex = 3;
            this.conStatus.Text = "Waiting for client...";
            // 
            // btnStop
            // 
            this.btnStop.Location = new System.Drawing.Point(769, 12);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(154, 45);
            this.btnStop.TabIndex = 4;
            this.btnStop.Text = "Stop Server";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // AsyncTcpSrvr
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(935, 450);
            this.Controls.Add(this.btnStop);
            this.Controls.Add(this.conStatus);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.results);
            this.Name = "AsyncTcpSrvr";
            this.Text = "Asynchronous TCP Server";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox results;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox conStatus;
        private System.Windows.Forms.Button btnStop;
    }
}

