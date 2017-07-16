namespace SensorHubDesktop
{
    partial class SLSettingFrm
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
            this.labelX1 = new DevComponents.DotNetBar.LabelX();
            this.txtColTime = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.labelX2 = new DevComponents.DotNetBar.LabelX();
            this.txtIntr = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.buttonX1 = new DevComponents.DotNetBar.ButtonX();
            this.labelX3 = new DevComponents.DotNetBar.LabelX();
            this.txtCnt = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.txtRept = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.labelX4 = new DevComponents.DotNetBar.LabelX();
            this.SuspendLayout();
            // 
            // labelX1
            // 
            // 
            // 
            // 
            this.labelX1.BackgroundStyle.Class = "";
            this.labelX1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX1.Location = new System.Drawing.Point(31, 22);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(60, 23);
            this.labelX1.TabIndex = 0;
            this.labelX1.Text = "采集时间";
            // 
            // txtColTime
            // 
            // 
            // 
            // 
            this.txtColTime.Border.Class = "TextBoxBorder";
            this.txtColTime.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.txtColTime.Location = new System.Drawing.Point(124, 22);
            this.txtColTime.Name = "txtColTime";
            this.txtColTime.Size = new System.Drawing.Size(163, 21);
            this.txtColTime.TabIndex = 1;
            // 
            // labelX2
            // 
            // 
            // 
            // 
            this.labelX2.BackgroundStyle.Class = "";
            this.labelX2.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX2.Location = new System.Drawing.Point(31, 72);
            this.labelX2.Name = "labelX2";
            this.labelX2.Size = new System.Drawing.Size(60, 23);
            this.labelX2.TabIndex = 2;
            this.labelX2.Text = "采集间隔";
            // 
            // txtIntr
            // 
            // 
            // 
            // 
            this.txtIntr.Border.Class = "TextBoxBorder";
            this.txtIntr.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.txtIntr.Location = new System.Drawing.Point(124, 72);
            this.txtIntr.Name = "txtIntr";
            this.txtIntr.Size = new System.Drawing.Size(163, 21);
            this.txtIntr.TabIndex = 4;
            // 
            // buttonX1
            // 
            this.buttonX1.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.buttonX1.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.buttonX1.Location = new System.Drawing.Point(212, 216);
            this.buttonX1.Name = "buttonX1";
            this.buttonX1.Size = new System.Drawing.Size(75, 23);
            this.buttonX1.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.buttonX1.TabIndex = 6;
            this.buttonX1.Text = "确定";
            this.buttonX1.Click += new System.EventHandler(this.buttonX1_Click);
            // 
            // labelX3
            // 
            // 
            // 
            // 
            this.labelX3.BackgroundStyle.Class = "";
            this.labelX3.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX3.Location = new System.Drawing.Point(31, 126);
            this.labelX3.Name = "labelX3";
            this.labelX3.Size = new System.Drawing.Size(60, 23);
            this.labelX3.TabIndex = 3;
            this.labelX3.Text = "采集次数";
            // 
            // txtCnt
            // 
            // 
            // 
            // 
            this.txtCnt.Border.Class = "TextBoxBorder";
            this.txtCnt.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.txtCnt.Location = new System.Drawing.Point(124, 126);
            this.txtCnt.Name = "txtCnt";
            this.txtCnt.Size = new System.Drawing.Size(163, 21);
            this.txtCnt.TabIndex = 5;
            // 
            // txtRept
            // 
            // 
            // 
            // 
            this.txtRept.Border.Class = "TextBoxBorder";
            this.txtRept.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.txtRept.Location = new System.Drawing.Point(124, 178);
            this.txtRept.Name = "txtRept";
            this.txtRept.Size = new System.Drawing.Size(163, 21);
            this.txtRept.TabIndex = 8;
            // 
            // labelX4
            // 
            // 
            // 
            // 
            this.labelX4.BackgroundStyle.Class = "";
            this.labelX4.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX4.Location = new System.Drawing.Point(31, 178);
            this.labelX4.Name = "labelX4";
            this.labelX4.Size = new System.Drawing.Size(60, 23);
            this.labelX4.TabIndex = 7;
            this.labelX4.Text = "重传次数";
            // 
            // SLSettingFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(332, 270);
            this.Controls.Add(this.txtRept);
            this.Controls.Add(this.labelX4);
            this.Controls.Add(this.buttonX1);
            this.Controls.Add(this.txtCnt);
            this.Controls.Add(this.txtIntr);
            this.Controls.Add(this.labelX3);
            this.Controls.Add(this.labelX2);
            this.Controls.Add(this.txtColTime);
            this.Controls.Add(this.labelX1);
            this.DoubleBuffered = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SLSettingFrm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "渗漏预警仪配置";
            this.Load += new System.EventHandler(this.SLSettingFrm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.LabelX labelX1;
        private DevComponents.DotNetBar.Controls.TextBoxX txtColTime;
        private DevComponents.DotNetBar.LabelX labelX2;
        private DevComponents.DotNetBar.Controls.TextBoxX txtIntr;
        private DevComponents.DotNetBar.ButtonX buttonX1;
        private DevComponents.DotNetBar.LabelX labelX3;
        private DevComponents.DotNetBar.Controls.TextBoxX txtCnt;
        private DevComponents.DotNetBar.Controls.TextBoxX txtRept;
        private DevComponents.DotNetBar.LabelX labelX4;
    }
}