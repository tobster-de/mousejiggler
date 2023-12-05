
namespace MouseJiggler
{
    partial class MainForm
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
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
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
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            jiggleTimer = new System.Windows.Forms.Timer(components);
            flpLayout = new System.Windows.Forms.FlowLayoutPanel();
            panelBase = new System.Windows.Forms.Panel();
            cmdAbout = new System.Windows.Forms.Button();
            cbSettings = new System.Windows.Forms.CheckBox();
            cbJiggling = new System.Windows.Forms.CheckBox();
            panelSettings = new System.Windows.Forms.Panel();
            lbPeriod = new System.Windows.Forms.Label();
            tbPeriod = new System.Windows.Forms.TrackBar();
            cbMinimize = new System.Windows.Forms.CheckBox();
            cbZen = new System.Windows.Forms.CheckBox();
            niTray = new System.Windows.Forms.NotifyIcon(components);
            trayMenuStrip = new System.Windows.Forms.ContextMenuStrip(components);
            miJiggling = new System.Windows.Forms.ToolStripMenuItem();
            miZenJiggle = new System.Windows.Forms.ToolStripMenuItem();
            separator1 = new System.Windows.Forms.ToolStripSeparator();
            miShutdown = new System.Windows.Forms.ToolStripMenuItem();
            flpLayout.SuspendLayout();
            panelBase.SuspendLayout();
            panelSettings.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)tbPeriod).BeginInit();
            trayMenuStrip.SuspendLayout();
            SuspendLayout();
            // 
            // jiggleTimer
            // 
            jiggleTimer.Interval = 1000;
            jiggleTimer.Tick += jiggleTimer_Tick;
            // 
            // flpLayout
            // 
            flpLayout.AutoSize = true;
            flpLayout.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            flpLayout.Controls.Add(panelBase);
            flpLayout.Controls.Add(panelSettings);
            flpLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            flpLayout.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            flpLayout.Location = new System.Drawing.Point(0, 0);
            flpLayout.Name = "flpLayout";
            flpLayout.Padding = new System.Windows.Forms.Padding(5);
            flpLayout.Size = new System.Drawing.Size(304, 160);
            flpLayout.TabIndex = 2;
            // 
            // panelBase
            // 
            panelBase.Controls.Add(cmdAbout);
            panelBase.Controls.Add(cbSettings);
            panelBase.Controls.Add(cbJiggling);
            panelBase.Location = new System.Drawing.Point(8, 8);
            panelBase.Name = "panelBase";
            panelBase.Size = new System.Drawing.Size(289, 28);
            panelBase.TabIndex = 3;
            // 
            // cmdAbout
            // 
            cmdAbout.Location = new System.Drawing.Point(244, 2);
            cmdAbout.Name = "cmdAbout";
            cmdAbout.Size = new System.Drawing.Size(40, 23);
            cmdAbout.TabIndex = 2;
            cmdAbout.Text = "?";
            cmdAbout.UseVisualStyleBackColor = true;
            cmdAbout.Click += cmdAbout_Click;
            // 
            // cbSettings
            // 
            cbSettings.Location = new System.Drawing.Point(88, 5);
            cbSettings.Name = "cbSettings";
            cbSettings.Size = new System.Drawing.Size(77, 19);
            cbSettings.TabIndex = 1;
            cbSettings.Text = "Settings...";
            cbSettings.UseVisualStyleBackColor = true;
            cbSettings.CheckedChanged += cbSettings_CheckedChanged;
            // 
            // cbJiggling
            // 
            cbJiggling.AutoSize = true;
            cbJiggling.Location = new System.Drawing.Point(10, 5);
            cbJiggling.Name = "cbJiggling";
            cbJiggling.Size = new System.Drawing.Size(67, 19);
            cbJiggling.TabIndex = 0;
            cbJiggling.Text = "Jiggling";
            cbJiggling.UseVisualStyleBackColor = true;
            cbJiggling.CheckedChanged += HandleJigglingChange;
            // 
            // panelSettings
            // 
            panelSettings.AutoSize = true;
            panelSettings.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            panelSettings.Controls.Add(lbPeriod);
            panelSettings.Controls.Add(tbPeriod);
            panelSettings.Controls.Add(cbMinimize);
            panelSettings.Controls.Add(cbZen);
            panelSettings.Location = new System.Drawing.Point(8, 42);
            panelSettings.Name = "panelSettings";
            panelSettings.Size = new System.Drawing.Size(289, 110);
            panelSettings.TabIndex = 2;
            panelSettings.Visible = false;
            // 
            // lbPeriod
            // 
            lbPeriod.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            lbPeriod.AutoSize = true;
            lbPeriod.Location = new System.Drawing.Point(263, 37);
            lbPeriod.Name = "lbPeriod";
            lbPeriod.Size = new System.Drawing.Size(21, 15);
            lbPeriod.TabIndex = 3;
            lbPeriod.Text = "1 s";
            lbPeriod.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tbPeriod
            // 
            tbPeriod.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            tbPeriod.Location = new System.Drawing.Point(4, 62);
            tbPeriod.Maximum = 60;
            tbPeriod.Minimum = 1;
            tbPeriod.Name = "tbPeriod";
            tbPeriod.Size = new System.Drawing.Size(281, 45);
            tbPeriod.TabIndex = 6;
            tbPeriod.TickFrequency = 2;
            tbPeriod.Value = 1;
            tbPeriod.ValueChanged += tbPeriod_ValueChanged;
            // 
            // cbMinimize
            // 
            cbMinimize.AutoSize = true;
            cbMinimize.Location = new System.Drawing.Point(10, 37);
            cbMinimize.Name = "cbMinimize";
            cbMinimize.Size = new System.Drawing.Size(118, 19);
            cbMinimize.TabIndex = 5;
            cbMinimize.Text = "Minimize on start";
            cbMinimize.UseVisualStyleBackColor = true;
            cbMinimize.CheckedChanged += cbMinimize_CheckedChanged;
            // 
            // cbZen
            // 
            cbZen.AutoSize = true;
            cbZen.Location = new System.Drawing.Point(10, 11);
            cbZen.Name = "cbZen";
            cbZen.Size = new System.Drawing.Size(78, 19);
            cbZen.TabIndex = 4;
            cbZen.Text = "Zen jiggle";
            cbZen.UseVisualStyleBackColor = true;
            cbZen.CheckedChanged += HandleZenChange;
            // 
            // niTray
            // 
            niTray.ContextMenuStrip = trayMenuStrip;
            niTray.Icon = (System.Drawing.Icon)resources.GetObject("niTray.Icon");
            niTray.Text = "Mouse Jiggler";
            niTray.DoubleClick += niTray_DoubleClick;
            // 
            // trayMenuStrip
            // 
            trayMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { miJiggling, miZenJiggle, separator1, miShutdown });
            trayMenuStrip.Name = "trayMenuStrip";
            trayMenuStrip.Size = new System.Drawing.Size(129, 76);
            // 
            // miJiggling
            // 
            miJiggling.CheckOnClick = true;
            miJiggling.Name = "miJiggling";
            miJiggling.Size = new System.Drawing.Size(128, 22);
            miJiggling.Text = "Jiggle";
            miJiggling.Click += HandleJigglingChange;
            // 
            // miZenJiggle
            // 
            miZenJiggle.CheckOnClick = true;
            miZenJiggle.Name = "miZenJiggle";
            miZenJiggle.Size = new System.Drawing.Size(128, 22);
            miZenJiggle.Text = "Zen jiggle";
            // 
            // separator1
            // 
            separator1.Name = "separator1";
            separator1.Size = new System.Drawing.Size(125, 6);
            // 
            // miShutdown
            // 
            miShutdown.Name = "miShutdown";
            miShutdown.Size = new System.Drawing.Size(128, 22);
            miShutdown.Text = "Shutdown";
            miShutdown.Click += miShutdown_Click;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            AutoSize = true;
            AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            ClientSize = new System.Drawing.Size(304, 160);
            Controls.Add(flpLayout);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            Name = "MainForm";
            Text = "Mouse Jiggler";
            Load += MainForm_Load;
            Shown += MainForm_Shown;
            Resize += MainForm_Resize;
            flpLayout.ResumeLayout(false);
            flpLayout.PerformLayout();
            panelBase.ResumeLayout(false);
            panelBase.PerformLayout();
            panelSettings.ResumeLayout(false);
            panelSettings.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)tbPeriod).EndInit();
            trayMenuStrip.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Timer jiggleTimer;
        private System.Windows.Forms.FlowLayoutPanel flpLayout;
        private System.Windows.Forms.Panel panelSettings;
        private System.Windows.Forms.TrackBar tbPeriod;
        private System.Windows.Forms.CheckBox cbMinimize;
        private System.Windows.Forms.CheckBox cbZen;
        private System.Windows.Forms.Panel panelBase;
        private System.Windows.Forms.CheckBox cbSettings;
        private System.Windows.Forms.CheckBox cbJiggling;
        private System.Windows.Forms.Label lbPeriod;
        private System.Windows.Forms.Button cmdAbout;
        private System.Windows.Forms.NotifyIcon niTray;
        private System.Windows.Forms.ContextMenuStrip trayMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem miJiggling;
        private System.Windows.Forms.ToolStripMenuItem miZenJiggle;
        private System.Windows.Forms.ToolStripSeparator separator1;
        private System.Windows.Forms.ToolStripMenuItem miShutdown;
    }
}

