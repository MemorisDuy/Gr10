
namespace AsyncTcpClient
{
    partial class AsyncTcpClient
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
            this.btnConnect = new System.Windows.Forms.Button();
            this.btnSend = new System.Windows.Forms.Button();
            this.btnDisConnect = new System.Windows.Forms.Button();
            this.conStatus = new System.Windows.Forms.TextBox();
            this.newText = new System.Windows.Forms.TextBox();
            this.results = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(692, 4);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(160, 51);
            this.btnConnect.TabIndex = 0;
            this.btnConnect.Text = "Connect";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // btnSend
            // 
            this.btnSend.Location = new System.Drawing.Point(870, 74);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(160, 51);
            this.btnSend.TabIndex = 0;
            this.btnSend.Text = "Send";
            this.btnSend.UseVisualStyleBackColor = true;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // btnDisConnect
            // 
            this.btnDisConnect.Location = new System.Drawing.Point(870, 4);
            this.btnDisConnect.Name = "btnDisConnect";
            this.btnDisConnect.Size = new System.Drawing.Size(160, 51);
            this.btnDisConnect.TabIndex = 0;
            this.btnDisConnect.Text = "DisConnect";
            this.btnDisConnect.UseVisualStyleBackColor = true;
            this.btnDisConnect.Click += new System.EventHandler(this.btnDisConnect_Click);
            // 
            // conStatus
            // 
            this.conStatus.Location = new System.Drawing.Point(210, 653);
            this.conStatus.Name = "conStatus";
            this.conStatus.Size = new System.Drawing.Size(893, 31);
            this.conStatus.TabIndex = 1;
            this.conStatus.Text = "Disconnected";
            // 
            // newText
            // 
            this.newText.Location = new System.Drawing.Point(15, 74);
            this.newText.Name = "newText";
            this.newText.Size = new System.Drawing.Size(837, 31);
            this.newText.TabIndex = 2;
            // 
            // results
            // 
            this.results.FormattingEnabled = true;
            this.results.ItemHeight = 25;
            this.results.Location = new System.Drawing.Point(12, 139);
            this.results.Name = "results";
            this.results.Size = new System.Drawing.Size(1091, 504);
            this.results.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(169, 25);
            this.label1.TabIndex = 4;
            this.label1.Text = "Enter text string:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(10, 652);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(194, 25);
            this.label2.TabIndex = 5;
            this.label2.Text = "Connection Status:";
            // 
            // AsyncTcpClient
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1115, 709);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.results);
            this.Controls.Add(this.newText);
            this.Controls.Add(this.conStatus);
            this.Controls.Add(this.btnSend);
            this.Controls.Add(this.btnDisConnect);
            this.Controls.Add(this.btnConnect);
            this.Name = "AsyncTcpClient";
            this.Text = "Asynchronous TCP Client";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.Button btnDisConnect;
        private System.Windows.Forms.TextBox conStatus;
        private System.Windows.Forms.TextBox newText;
        private System.Windows.Forms.ListBox results;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
    }
}

