namespace TestWindow
{
    partial class ClientTasks
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

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.StartCreateDeliveriesBtn = new System.Windows.Forms.Button();
            this.StopCreateDeliveryBtn = new System.Windows.Forms.Button();
            this.ExpireDeliveriesBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // StartCreateDeliveriesBtn
            // 
            this.StartCreateDeliveriesBtn.Location = new System.Drawing.Point(1, 12);
            this.StartCreateDeliveriesBtn.Name = "StartCreateDeliveriesBtn";
            this.StartCreateDeliveriesBtn.Size = new System.Drawing.Size(127, 23);
            this.StartCreateDeliveriesBtn.TabIndex = 0;
            this.StartCreateDeliveriesBtn.Text = "Start create deliveries";
            this.StartCreateDeliveriesBtn.UseVisualStyleBackColor = true;
            this.StartCreateDeliveriesBtn.Click += new System.EventHandler(this.StartCreateDeliveriesBtn_Click);
            // 
            // StopCreateDeliveryBtn
            // 
            this.StopCreateDeliveryBtn.Location = new System.Drawing.Point(1, 70);
            this.StopCreateDeliveryBtn.Name = "StopCreateDeliveryBtn";
            this.StopCreateDeliveryBtn.Size = new System.Drawing.Size(127, 23);
            this.StopCreateDeliveryBtn.TabIndex = 1;
            this.StopCreateDeliveryBtn.Text = "Stop all tasks";
            this.StopCreateDeliveryBtn.UseVisualStyleBackColor = true;
            this.StopCreateDeliveryBtn.Click += new System.EventHandler(this.StopTasksBtn_Click);
            // 
            // ExpireDeliveriesBtn
            // 
            this.ExpireDeliveriesBtn.Location = new System.Drawing.Point(1, 41);
            this.ExpireDeliveriesBtn.Name = "ExpireDeliveriesBtn";
            this.ExpireDeliveriesBtn.Size = new System.Drawing.Size(127, 23);
            this.ExpireDeliveriesBtn.TabIndex = 2;
            this.ExpireDeliveriesBtn.Text = "Expire deliveries";
            this.ExpireDeliveriesBtn.UseVisualStyleBackColor = true;
            this.ExpireDeliveriesBtn.Click += new System.EventHandler(this.ExpireDeliveries_Click);
            // 
            // ClientTasks
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(217, 199);
            this.Controls.Add(this.ExpireDeliveriesBtn);
            this.Controls.Add(this.StopCreateDeliveryBtn);
            this.Controls.Add(this.StartCreateDeliveriesBtn);
            this.Name = "ClientTasks";
            this.Text = "ClientTasks";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button StartCreateDeliveriesBtn;
        private System.Windows.Forms.Button StopCreateDeliveryBtn;
        private System.Windows.Forms.Button ExpireDeliveriesBtn;
    }
}

