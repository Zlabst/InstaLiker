using MetroFramework.Controls;
using System.Windows.Forms;

namespace InstaLiker
{
    partial class FrmMain
    {
        /// <summary>
        /// Требуется переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
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
        /// Обязательный метод для поддержки конструктора - не изменяйте
        /// содержимое данного метода при помощи редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.wbMain = new System.Windows.Forms.WebBrowser();
            this.dgrTagsInfo = new MetroFramework.Controls.MetroGrid();
            this.TagName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Interval = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.NeedCountLikes = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CompCountLikes = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Status = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tbTagName = new MetroFramework.Controls.MetroTextBox();
            this.lblTagName = new MetroFramework.Controls.MetroLabel();
            this.btnAddTag = new MetroFramework.Controls.MetroButton();
            this.btnUpdateLinks = new MetroFramework.Controls.MetroButton();
            this.pbMain = new MetroFramework.Controls.MetroProgressBar();
            this.btnStart = new MetroFramework.Controls.MetroButton();
            this.lblInterval = new MetroFramework.Controls.MetroLabel();
            this.tbInterval = new MetroFramework.Controls.MetroTextBox();
            this.lblCountLike = new MetroFramework.Controls.MetroLabel();
            this.tbCountLike = new MetroFramework.Controls.MetroTextBox();
            this.btnStop = new MetroFramework.Controls.MetroButton();
            ((System.ComponentModel.ISupportInitialize)(this.dgrTagsInfo)).BeginInit();
            this.SuspendLayout();
            // 
            // wbMain
            // 
            this.wbMain.Location = new System.Drawing.Point(24, 64);
            this.wbMain.MinimumSize = new System.Drawing.Size(20, 20);
            this.wbMain.Name = "wbMain";
            this.wbMain.ScriptErrorsSuppressed = true;
            this.wbMain.Size = new System.Drawing.Size(220, 370);
            this.wbMain.TabIndex = 1;
            // 
            // dgrTagsInfo
            // 
            this.dgrTagsInfo.AllowUserToAddRows = false;
            this.dgrTagsInfo.AllowUserToDeleteRows = false;
            this.dgrTagsInfo.AllowUserToResizeRows = false;
            this.dgrTagsInfo.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.dgrTagsInfo.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgrTagsInfo.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.dgrTagsInfo.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(174)))), ((int)(((byte)(219)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(174)))), ((int)(((byte)(219)))));
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgrTagsInfo.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgrTagsInfo.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgrTagsInfo.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.TagName,
            this.Interval,
            this.NeedCountLikes,
            this.CompCountLikes,
            this.Status});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(136)))), ((int)(((byte)(136)))), ((int)(((byte)(136)))));
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(174)))), ((int)(((byte)(219)))));
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgrTagsInfo.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgrTagsInfo.EnableHeadersVisualStyles = false;
            this.dgrTagsInfo.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.dgrTagsInfo.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.dgrTagsInfo.Location = new System.Drawing.Point(250, 64);
            this.dgrTagsInfo.MultiSelect = false;
            this.dgrTagsInfo.Name = "dgrTagsInfo";
            this.dgrTagsInfo.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(174)))), ((int)(((byte)(219)))));
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(174)))), ((int)(((byte)(219)))));
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgrTagsInfo.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dgrTagsInfo.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgrTagsInfo.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgrTagsInfo.Size = new System.Drawing.Size(544, 331);
            this.dgrTagsInfo.TabIndex = 2;
            this.dgrTagsInfo.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgrTagsInfo_CellValueChanged);
            // 
            // TagName
            // 
            this.TagName.HeaderText = "Tag";
            this.TagName.Name = "TagName";
            // 
            // Interval
            // 
            this.Interval.HeaderText = "Interval (min)";
            this.Interval.Name = "Interval";
            // 
            // NeedCountLikes
            // 
            this.NeedCountLikes.HeaderText = "Required number of likes";
            this.NeedCountLikes.Name = "NeedCountLikes";
            // 
            // CompCountLikes
            // 
            this.CompCountLikes.HeaderText = "Complete count likes";
            this.CompCountLikes.Name = "CompCountLikes";
            // 
            // Status
            // 
            this.Status.HeaderText = "Status";
            this.Status.Name = "Status";
            // 
            // tbTagName
            // 
            this.tbTagName.Lines = new string[0];
            this.tbTagName.Location = new System.Drawing.Point(250, 414);
            this.tbTagName.MaxLength = 32767;
            this.tbTagName.Name = "tbTagName";
            this.tbTagName.PasswordChar = '\0';
            this.tbTagName.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.tbTagName.SelectedText = "";
            this.tbTagName.Size = new System.Drawing.Size(100, 20);
            this.tbTagName.TabIndex = 3;
            this.tbTagName.UseSelectable = true;
            // 
            // lblTagName
            // 
            this.lblTagName.AutoSize = true;
            this.lblTagName.Location = new System.Drawing.Point(250, 393);
            this.lblTagName.Name = "lblTagName";
            this.lblTagName.Size = new System.Drawing.Size(31, 19);
            this.lblTagName.TabIndex = 4;
            this.lblTagName.Text = "Tag";
            // 
            // btnAddTag
            // 
            this.btnAddTag.Location = new System.Drawing.Point(508, 411);
            this.btnAddTag.Name = "btnAddTag";
            this.btnAddTag.Size = new System.Drawing.Size(144, 23);
            this.btnAddTag.TabIndex = 9;
            this.btnAddTag.Text = "Add tag";
            this.btnAddTag.UseSelectable = true;
            this.btnAddTag.UseVisualStyleBackColor = true;
            this.btnAddTag.Click += new System.EventHandler(this.btnAddTag_Click);
            // 
            // btnUpdateLinks
            // 
            this.btnUpdateLinks.Location = new System.Drawing.Point(658, 411);
            this.btnUpdateLinks.Name = "btnUpdateLinks";
            this.btnUpdateLinks.Size = new System.Drawing.Size(136, 23);
            this.btnUpdateLinks.TabIndex = 10;
            this.btnUpdateLinks.Text = "Update link tags";
            this.btnUpdateLinks.UseSelectable = true;
            this.btnUpdateLinks.UseVisualStyleBackColor = true;
            this.btnUpdateLinks.Click += new System.EventHandler(this.btnUpdateLinks_Click_1);
            // 
            // pbMain
            // 
            this.pbMain.Location = new System.Drawing.Point(250, 440);
            this.pbMain.Name = "pbMain";
            this.pbMain.Size = new System.Drawing.Size(544, 23);
            this.pbMain.TabIndex = 11;
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(10, 440);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(150, 23);
            this.btnStart.TabIndex = 12;
            this.btnStart.Text = "Start";
            this.btnStart.UseSelectable = true;
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // lblInterval
            // 
            this.lblInterval.AutoSize = true;
            this.lblInterval.Location = new System.Drawing.Point(429, 393);
            this.lblInterval.Name = "lblInterval";
            this.lblInterval.Size = new System.Drawing.Size(85, 19);
            this.lblInterval.TabIndex = 8;
            this.lblInterval.Text = "Interval (min)";
            // 
            // tbInterval
            // 
            this.tbInterval.Lines = new string[0];
            this.tbInterval.Location = new System.Drawing.Point(432, 414);
            this.tbInterval.MaxLength = 32767;
            this.tbInterval.Name = "tbInterval";
            this.tbInterval.PasswordChar = '\0';
            this.tbInterval.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.tbInterval.SelectedText = "";
            this.tbInterval.Size = new System.Drawing.Size(70, 20);
            this.tbInterval.TabIndex = 7;
            this.tbInterval.UseSelectable = true;
            // 
            // lblCountLike
            // 
            this.lblCountLike.AutoSize = true;
            this.lblCountLike.Location = new System.Drawing.Point(353, 393);
            this.lblCountLike.Name = "lblCountLike";
            this.lblCountLike.Size = new System.Drawing.Size(72, 19);
            this.lblCountLike.TabIndex = 6;
            this.lblCountLike.Text = "Count likes";
            // 
            // tbCountLike
            // 
            this.tbCountLike.Lines = new string[0];
            this.tbCountLike.Location = new System.Drawing.Point(356, 414);
            this.tbCountLike.MaxLength = 32767;
            this.tbCountLike.Name = "tbCountLike";
            this.tbCountLike.PasswordChar = '\0';
            this.tbCountLike.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.tbCountLike.SelectedText = "";
            this.tbCountLike.Size = new System.Drawing.Size(70, 20);
            this.tbCountLike.TabIndex = 5;
            this.tbCountLike.UseSelectable = true;
            // 
            // btnStop
            // 
            this.btnStop.Location = new System.Drawing.Point(169, 440);
            this.btnStop.Name = "btnStop";
            this.btnStop.TabIndex = 13;
            this.btnStop.Text = "Clear cache";
            this.btnStop.UseSelectable = true;
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // FrmMain
            // 
            this.AcceptButton = this.btnStart;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(841, 480);
            this.Controls.Add(this.btnStop);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.pbMain);
            this.Controls.Add(this.btnUpdateLinks);
            this.Controls.Add(this.btnAddTag);
            this.Controls.Add(this.lblInterval);
            this.Controls.Add(this.tbInterval);
            this.Controls.Add(this.lblCountLike);
            this.Controls.Add(this.tbCountLike);
            this.Controls.Add(this.lblTagName);
            this.Controls.Add(this.tbTagName);
            this.Controls.Add(this.dgrTagsInfo);
            this.Controls.Add(this.wbMain);
            this.MaximizeBox = false;
            this.Name = "FrmMain";
            this.Resizable = false;
            this.Text = "InstaLiker";
            this.Load += new System.EventHandler(this.FrmMain_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgrTagsInfo)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public MetroGrid dgrTagsInfo;
        public WebBrowser wbMain;
        public DataGridViewTextBoxColumn TagName;
        public DataGridViewTextBoxColumn Interval;
        public DataGridViewTextBoxColumn NeedCountLikes;
        public DataGridViewTextBoxColumn CompCountLikes;
        public DataGridViewTextBoxColumn Status;
        public MetroLabel lblTagName;
        public MetroLabel lblInterval;
        public MetroLabel lblCountLike;
        public MetroButton btnStop;
        public MetroButton btnAddTag;
        public MetroButton btnUpdateLinks;
        public MetroButton btnStart;
        public MetroProgressBar pbMain;
        public MetroTextBox tbTagName;
        public MetroTextBox tbInterval;
        public MetroTextBox tbCountLike;

    }
}

