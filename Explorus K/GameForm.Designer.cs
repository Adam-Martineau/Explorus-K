using Explorus_K.Views;

namespace Explorus_K
{
    partial class GameForm
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
            this.statusBar = new System.Windows.Forms.StatusStrip();
            this.statusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.levelLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.soundOptionsPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.musicLabel = new System.Windows.Forms.Label();
            this.musicTrackBar = new System.Windows.Forms.TrackBar();
            this.musicValueLabel = new System.Windows.Forms.Label();
            this.soundLabel = new System.Windows.Forms.Label();
            this.soundTrackBar = new System.Windows.Forms.TrackBar();
            this.soundValueLabel = new System.Windows.Forms.Label();
            this.muteMusic = new System.Windows.Forms.Button();
            this.muteSound = new System.Windows.Forms.Button();
            this.statusBar.SuspendLayout();
            this.soundOptionsPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.musicTrackBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.soundTrackBar)).BeginInit();
            this.SuspendLayout();
            // 
            // statusBar
            // 
            this.statusBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusLabel,
            this.toolStripStatusLabel1,
            this.levelLabel});
            this.statusBar.Location = new System.Drawing.Point(0, 332);
            this.statusBar.Name = "statusBar";
            this.statusBar.Size = new System.Drawing.Size(401, 22);
            this.statusBar.TabIndex = 0;
            this.statusBar.Text = "YOOOO";
            // 
            // statusLabel
            // 
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(66, 17);
            this.statusLabel.Text = "statusLabel";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(12, 17);
            this.toolStripStatusLabel1.Text = "-";
            // 
            // levelLabel
            // 
            this.levelLabel.Name = "levelLabel";
            this.levelLabel.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.levelLabel.Size = new System.Drawing.Size(59, 17);
            this.levelLabel.Text = "levelLabel";
            // 
            // soundOptionsPanel
            // 
            this.soundOptionsPanel.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.soundOptionsPanel.BackColor = System.Drawing.SystemColors.Window;
            this.soundOptionsPanel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.soundOptionsPanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.soundOptionsPanel.Controls.Add(this.musicLabel);
            this.soundOptionsPanel.Controls.Add(this.musicTrackBar);
            this.soundOptionsPanel.Controls.Add(this.musicValueLabel);
            this.soundOptionsPanel.Controls.Add(this.muteMusic);
            this.soundOptionsPanel.Controls.Add(this.soundLabel);
            this.soundOptionsPanel.Controls.Add(this.soundTrackBar);
            this.soundOptionsPanel.Controls.Add(this.soundValueLabel);
            this.soundOptionsPanel.Controls.Add(this.muteSound);
            this.soundOptionsPanel.Location = new System.Drawing.Point(0, 105);
            this.soundOptionsPanel.Name = "soundOptionsPanel";
            this.soundOptionsPanel.Size = new System.Drawing.Size(400, 100);
            this.soundOptionsPanel.TabIndex = 1;
            this.soundOptionsPanel.Visible = false;
            // 
            // musicLabel
            // 
            this.musicLabel.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.musicLabel.AutoSize = true;
            this.musicLabel.Location = new System.Drawing.Point(3, 19);
            this.musicLabel.Name = "musicLabel";
            this.musicLabel.Size = new System.Drawing.Size(73, 13);
            this.musicLabel.TabIndex = 1;
            this.musicLabel.Text = "Music Volume";
            // 
            // musicTrackBar
            // 
            this.musicTrackBar.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.musicTrackBar.Location = new System.Drawing.Point(82, 3);
            this.musicTrackBar.Name = "musicTrackBar";
            this.musicTrackBar.Size = new System.Drawing.Size(203, 45);
            this.musicTrackBar.TabIndex = 0;
            this.musicTrackBar.TickFrequency = 5;
            this.musicTrackBar.Scroll += new System.EventHandler(this.musicTrackBar_Scroll);
            this.musicTrackBar.KeyDown += new System.Windows.Forms.KeyEventHandler(this.GameForm_KeyDown);
            // 
            // musicValueLabel
            // 
            this.musicValueLabel.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.musicValueLabel.AutoSize = true;
            this.musicValueLabel.Location = new System.Drawing.Point(291, 19);
            this.musicValueLabel.Name = "musicValueLabel";
            this.musicValueLabel.Size = new System.Drawing.Size(35, 13);
            this.musicValueLabel.TabIndex = 2;
            this.musicValueLabel.Text = "label1";
            // 
            // soundLabel
            // 
            this.soundLabel.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.soundLabel.AutoSize = true;
            this.soundLabel.Location = new System.Drawing.Point(3, 70);
            this.soundLabel.Name = "soundLabel";
            this.soundLabel.Size = new System.Drawing.Size(76, 13);
            this.soundLabel.TabIndex = 4;
            this.soundLabel.Text = "Sound Volume";
            // 
            // soundTrackBar
            // 
            this.soundTrackBar.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.soundTrackBar.Location = new System.Drawing.Point(85, 54);
            this.soundTrackBar.Name = "soundTrackBar";
            this.soundTrackBar.Size = new System.Drawing.Size(200, 45);
            this.soundTrackBar.TabIndex = 3;
            this.soundTrackBar.Scroll += new System.EventHandler(this.soundTrackBar_Scroll);
            this.soundTrackBar.KeyDown += new System.Windows.Forms.KeyEventHandler(this.GameForm_KeyDown);
            // 
            // soundValueLabel
            // 
            this.soundValueLabel.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.soundValueLabel.AutoSize = true;
            this.soundValueLabel.Location = new System.Drawing.Point(291, 70);
            this.soundValueLabel.Name = "soundValueLabel";
            this.soundValueLabel.Size = new System.Drawing.Size(35, 13);
            this.soundValueLabel.TabIndex = 5;
            this.soundValueLabel.Text = "label1";
            // 
            // muteMusic
            // 
            this.muteMusic.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.muteMusic.Location = new System.Drawing.Point(332, 14);
            this.muteMusic.Name = "muteMusic";
            this.muteMusic.Size = new System.Drawing.Size(53, 23);
            this.muteMusic.TabIndex = 6;
            this.muteMusic.Text = "Mute";
            this.muteMusic.UseVisualStyleBackColor = true;
            this.muteMusic.Click += new System.EventHandler(this.muteMusic_Click);
            this.muteMusic.KeyDown += new System.Windows.Forms.KeyEventHandler(this.GameForm_KeyDown);
            // 
            // muteSound
            // 
            this.muteSound.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.muteSound.Location = new System.Drawing.Point(332, 65);
            this.muteSound.Name = "muteSound";
            this.muteSound.Size = new System.Drawing.Size(53, 23);
            this.muteSound.TabIndex = 7;
            this.muteSound.Text = "Mute";
            this.muteSound.UseVisualStyleBackColor = true;
            this.muteSound.Click += new System.EventHandler(this.muteSound_Click);
            this.muteSound.KeyDown += new System.Windows.Forms.KeyEventHandler(this.GameForm_KeyDown);
            // 
            // GameForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(401, 354);
            this.Controls.Add(this.soundOptionsPanel);
            this.Controls.Add(this.statusBar);
            this.DoubleBuffered = true;
            this.Name = "GameForm";
            this.Text = "Form1";
            this.SizeChanged += new System.EventHandler(this.GameForm_SizeChanged_1);
            this.GotFocus += new System.EventHandler(this.GameForm_Enter);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.GameForm_KeyDown);
            this.LostFocus += new System.EventHandler(this.GameForm_Leave);
            this.statusBar.ResumeLayout(false);
            this.statusBar.PerformLayout();
            this.soundOptionsPanel.ResumeLayout(false);
            this.soundOptionsPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.musicTrackBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.soundTrackBar)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip statusBar;
        private System.Windows.Forms.ToolStripStatusLabel statusLabel;
        private System.Windows.Forms.ToolStripStatusLabel levelLabel;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.FlowLayoutPanel soundOptionsPanel;
        private System.Windows.Forms.TrackBar musicTrackBar;
        private System.Windows.Forms.Label musicLabel;
        private System.Windows.Forms.Label musicValueLabel;
        private System.Windows.Forms.Label soundLabel;
        private System.Windows.Forms.TrackBar soundTrackBar;
        private System.Windows.Forms.Label soundValueLabel;
        private System.Windows.Forms.Button muteMusic;
        private System.Windows.Forms.Button muteSound;
    }
}

