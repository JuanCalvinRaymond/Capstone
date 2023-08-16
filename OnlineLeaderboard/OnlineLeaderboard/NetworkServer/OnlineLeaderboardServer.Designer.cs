namespace OnlineLeaderboard
{
    partial class COnlineLeaderboardServer
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
            this.Label_IPTitle = new System.Windows.Forms.Label();
            this.Label_ServerIP = new System.Windows.Forms.Label();
            this.ListBox_ServerEvents = new System.Windows.Forms.ListBox();
            this.Button_Exit = new System.Windows.Forms.Button();
            this.Button_Start = new System.Windows.Forms.Button();
            this.Label_ServerStatus = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // Label_IPTitle
            // 
            this.Label_IPTitle.Location = new System.Drawing.Point(13, 13);
            this.Label_IPTitle.Name = "Label_IPTitle";
            this.Label_IPTitle.Size = new System.Drawing.Size(20, 13);
            this.Label_IPTitle.TabIndex = 0;
            this.Label_IPTitle.Text = "IP:";
            this.Label_IPTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // Label_ServerIP
            // 
            this.Label_ServerIP.AutoSize = true;
            this.Label_ServerIP.Location = new System.Drawing.Point(40, 13);
            this.Label_ServerIP.Name = "Label_ServerIP";
            this.Label_ServerIP.Size = new System.Drawing.Size(52, 13);
            this.Label_ServerIP.TabIndex = 1;
            this.Label_ServerIP.Text = "127.0.0.1";
            this.Label_ServerIP.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ListBox_ServerEvents
            // 
            this.ListBox_ServerEvents.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ListBox_ServerEvents.FormattingEnabled = true;
            this.ListBox_ServerEvents.Location = new System.Drawing.Point(13, 30);
            this.ListBox_ServerEvents.Name = "ListBox_ServerEvents";
            this.ListBox_ServerEvents.Size = new System.Drawing.Size(285, 264);
            this.ListBox_ServerEvents.TabIndex = 2;
            // 
            // Button_Exit
            // 
            this.Button_Exit.Location = new System.Drawing.Point(12, 306);
            this.Button_Exit.Name = "Button_Exit";
            this.Button_Exit.Size = new System.Drawing.Size(75, 23);
            this.Button_Exit.TabIndex = 3;
            this.Button_Exit.Text = "Exit";
            this.Button_Exit.UseVisualStyleBackColor = true;
            this.Button_Exit.Click += new System.EventHandler(this.Button_Exit_Click);
            // 
            // Button_Start
            // 
            this.Button_Start.Location = new System.Drawing.Point(223, 306);
            this.Button_Start.Name = "Button_Start";
            this.Button_Start.Size = new System.Drawing.Size(75, 23);
            this.Button_Start.TabIndex = 4;
            this.Button_Start.Text = "Start";
            this.Button_Start.UseVisualStyleBackColor = true;
            this.Button_Start.Click += new System.EventHandler(this.Button_Start_Click);
            // 
            // Label_ServerStatus
            // 
            this.Label_ServerStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label_ServerStatus.Location = new System.Drawing.Point(214, 7);
            this.Label_ServerStatus.Name = "Label_ServerStatus";
            this.Label_ServerStatus.Size = new System.Drawing.Size(84, 20);
            this.Label_ServerStatus.TabIndex = 5;
            this.Label_ServerStatus.Text = "OFFLINE";
            // 
            // COnlineLeaderboardServer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(310, 341);
            this.Controls.Add(this.Label_ServerStatus);
            this.Controls.Add(this.Button_Start);
            this.Controls.Add(this.Button_Exit);
            this.Controls.Add(this.ListBox_ServerEvents);
            this.Controls.Add(this.Label_ServerIP);
            this.Controls.Add(this.Label_IPTitle);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "COnlineLeaderboardServer";
            this.Text = "Online Leaderboard Server";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OnlineLeaderboardServer_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label Label_IPTitle;
        private System.Windows.Forms.Label Label_ServerIP;
        private System.Windows.Forms.ListBox ListBox_ServerEvents;
        private System.Windows.Forms.Button Button_Exit;
        private System.Windows.Forms.Button Button_Start;
        private System.Windows.Forms.Label Label_ServerStatus;
    }
}

