namespace Chummer
{
    partial class frmSelectDrug
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
            if (disposing)
            {
                components?.Dispose();
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
			this.components = new System.ComponentModel.Container();
			this.cmdOK = new System.Windows.Forms.Button();
			this.cmdCancel = new System.Windows.Forms.Button();
			this.tableLayoutPanel1 = new Chummer.BufferedTableLayoutPanel(this.components);
			this.lblRatingNALabel = new System.Windows.Forms.Label();
			this.nudRating = new System.Windows.Forms.NumericUpDown();
			this.txtSearch = new System.Windows.Forms.TextBox();
			this.chkHideOverAvailLimit = new System.Windows.Forms.CheckBox();
			this.lblSearchLabel = new System.Windows.Forms.Label();
			this.chkHideBannedGrades = new System.Windows.Forms.CheckBox();
			this.lblCyberwareNotes = new System.Windows.Forms.Label();
			this.lblCyberwareNotesLabel = new System.Windows.Forms.Label();
			this.lblSource = new System.Windows.Forms.Label();
			this.lblSourceLabel = new System.Windows.Forms.Label();
			this.chkShowOnlyAffordItems = new System.Windows.Forms.CheckBox();
			this.flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
			this.cmdOKAdd = new System.Windows.Forms.Button();
			this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
			this.cboGrade = new Chummer.ElasticComboBox();
			this.label1 = new System.Windows.Forms.Label();
			this.lstDrug = new System.Windows.Forms.ListBox();
			this.lblAvailLabel = new System.Windows.Forms.Label();
			this.lblAvail = new System.Windows.Forms.Label();
			this.lblTestLabel = new System.Windows.Forms.Label();
			this.lblTest = new System.Windows.Forms.Label();
			this.lblCostLabel = new System.Windows.Forms.Label();
			this.lblCost = new System.Windows.Forms.Label();
			this.lblRatingLabel = new System.Windows.Forms.Label();
			this.flpCheckBoxes = new System.Windows.Forms.FlowLayoutPanel();
			this.chkFree = new System.Windows.Forms.CheckBox();
			this.chkBlackMarketDiscount = new System.Windows.Forms.CheckBox();
			this.flpRating = new System.Windows.Forms.FlowLayoutPanel();
			this.lblMarkupLabel = new System.Windows.Forms.Label();
			this.flpMarkup = new System.Windows.Forms.FlowLayoutPanel();
			this.nudMarkup = new System.Windows.Forms.NumericUpDown();
			this.lblMarkupPercentLabel = new System.Windows.Forms.Label();
			this.tableLayoutPanel1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.nudRating)).BeginInit();
			this.flowLayoutPanel2.SuspendLayout();
			this.tableLayoutPanel2.SuspendLayout();
			this.flpCheckBoxes.SuspendLayout();
			this.flpMarkup.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.nudMarkup)).BeginInit();
			this.SuspendLayout();
			// 
			// cmdOK
			// 
			this.cmdOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.cmdOK.AutoSize = true;
			this.cmdOK.Location = new System.Drawing.Point(162, 0);
			this.cmdOK.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
			this.cmdOK.Name = "cmdOK";
			this.cmdOK.Size = new System.Drawing.Size(75, 23);
			this.cmdOK.TabIndex = 27;
			this.cmdOK.Tag = "String_OK";
			this.cmdOK.Text = "OK";
			this.cmdOK.UseVisualStyleBackColor = true;
			this.cmdOK.Click += new System.EventHandler(this.cmdOK_Click);
			// 
			// cmdCancel
			// 
			this.cmdCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.cmdCancel.AutoSize = true;
			this.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.cmdCancel.Location = new System.Drawing.Point(0, 0);
			this.cmdCancel.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
			this.cmdCancel.Name = "cmdCancel";
			this.cmdCancel.Size = new System.Drawing.Size(75, 23);
			this.cmdCancel.TabIndex = 29;
			this.cmdCancel.Tag = "String_Cancel";
			this.cmdCancel.Text = "Cancel";
			this.cmdCancel.UseVisualStyleBackColor = true;
			this.cmdCancel.Click += new System.EventHandler(this.cmdCancel_Click);
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tableLayoutPanel1.AutoSize = true;
			this.tableLayoutPanel1.ColumnCount = 5;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 301F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel1.Controls.Add(this.txtSearch, 2, 0);
			this.tableLayoutPanel1.Controls.Add(this.chkHideOverAvailLimit, 1, 11);
			this.tableLayoutPanel1.Controls.Add(this.lblSearchLabel, 1, 0);
			this.tableLayoutPanel1.Controls.Add(this.chkHideBannedGrades, 1, 10);
			this.tableLayoutPanel1.Controls.Add(this.lblCyberwareNotes, 2, 9);
			this.tableLayoutPanel1.Controls.Add(this.lblCyberwareNotesLabel, 1, 9);
			this.tableLayoutPanel1.Controls.Add(this.lblSource, 2, 8);
			this.tableLayoutPanel1.Controls.Add(this.lblSourceLabel, 1, 8);
			this.tableLayoutPanel1.Controls.Add(this.chkShowOnlyAffordItems, 1, 12);
			this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel2, 1, 13);
			this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.lblAvailLabel, 3, 1);
			this.tableLayoutPanel1.Controls.Add(this.lblAvail, 4, 1);
			this.tableLayoutPanel1.Controls.Add(this.lblTestLabel, 3, 8);
			this.tableLayoutPanel1.Controls.Add(this.lblTest, 4, 8);
			this.tableLayoutPanel1.Controls.Add(this.flpRating, 1, 6);
			this.tableLayoutPanel1.Controls.Add(this.lblCostLabel, 1, 1);
			this.tableLayoutPanel1.Controls.Add(this.lblCost, 2, 1);
			this.tableLayoutPanel1.Controls.Add(this.lblRatingLabel, 1, 2);
			this.tableLayoutPanel1.Controls.Add(this.nudRating, 2, 2);
			this.tableLayoutPanel1.Controls.Add(this.lblRatingNALabel, 3, 2);
			this.tableLayoutPanel1.Controls.Add(this.flpCheckBoxes, 1, 3);
			this.tableLayoutPanel1.Controls.Add(this.lblMarkupLabel, 1, 4);
			this.tableLayoutPanel1.Controls.Add(this.flpMarkup, 2, 4);
			this.tableLayoutPanel1.Location = new System.Drawing.Point(9, 9);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 14;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(603, 423);
			this.tableLayoutPanel1.TabIndex = 68;
			// 
			// lblRatingNALabel
			// 
			this.lblRatingNALabel.AutoSize = true;
			this.lblRatingNALabel.Location = new System.Drawing.Point(461, 57);
			this.lblRatingNALabel.Margin = new System.Windows.Forms.Padding(3, 6, 3, 7);
			this.lblRatingNALabel.Name = "lblRatingNALabel";
			this.lblRatingNALabel.Size = new System.Drawing.Size(27, 13);
			this.lblRatingNALabel.TabIndex = 15;
			this.lblRatingNALabel.Tag = "String_NotApplicable";
			this.lblRatingNALabel.Text = "N/A";
			this.lblRatingNALabel.Visible = false;
			// 
			// nudRating
			// 
			this.nudRating.Location = new System.Drawing.Point(356, 54);
			this.nudRating.Name = "nudRating";
			this.nudRating.Size = new System.Drawing.Size(97, 20);
			this.nudRating.TabIndex = 3;
			this.nudRating.ValueChanged += new System.EventHandler(this.nudRating_ValueChanged);
			// 
			// txtSearch
			// 
			this.txtSearch.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tableLayoutPanel1.SetColumnSpan(this.txtSearch, 3);
			this.txtSearch.Location = new System.Drawing.Point(356, 3);
			this.txtSearch.Name = "txtSearch";
			this.txtSearch.Size = new System.Drawing.Size(244, 20);
			this.txtSearch.TabIndex = 1;
			this.txtSearch.TextChanged += new System.EventHandler(this.txtSearch_TextChanged);
			this.txtSearch.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtSearch_KeyDown);
			this.txtSearch.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtSearch_KeyUp);
			// 
			// chkHideOverAvailLimit
			// 
			this.chkHideOverAvailLimit.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.chkHideOverAvailLimit.AutoSize = true;
			this.tableLayoutPanel1.SetColumnSpan(this.chkHideOverAvailLimit, 4);
			this.chkHideOverAvailLimit.Location = new System.Drawing.Point(304, 348);
			this.chkHideOverAvailLimit.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.chkHideOverAvailLimit.Name = "chkHideOverAvailLimit";
			this.chkHideOverAvailLimit.Size = new System.Drawing.Size(296, 17);
			this.chkHideOverAvailLimit.TabIndex = 65;
			this.chkHideOverAvailLimit.Tag = "Checkbox_HideOverAvailLimit";
			this.chkHideOverAvailLimit.Text = "Hide Items Over Avail Limit ({0})";
			this.chkHideOverAvailLimit.UseVisualStyleBackColor = true;
			this.chkHideOverAvailLimit.CheckedChanged += new System.EventHandler(this.chkHideOverAvailLimit_CheckedChanged);
			// 
			// lblSearchLabel
			// 
			this.lblSearchLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.lblSearchLabel.AutoSize = true;
			this.lblSearchLabel.Location = new System.Drawing.Point(306, 6);
			this.lblSearchLabel.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
			this.lblSearchLabel.Name = "lblSearchLabel";
			this.lblSearchLabel.Size = new System.Drawing.Size(44, 13);
			this.lblSearchLabel.TabIndex = 0;
			this.lblSearchLabel.Tag = "Label_Search";
			this.lblSearchLabel.Text = "&Search:";
			// 
			// chkHideBannedGrades
			// 
			this.chkHideBannedGrades.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.chkHideBannedGrades.AutoSize = true;
			this.chkHideBannedGrades.Checked = true;
			this.chkHideBannedGrades.CheckState = System.Windows.Forms.CheckState.Checked;
			this.tableLayoutPanel1.SetColumnSpan(this.chkHideBannedGrades, 4);
			this.chkHideBannedGrades.Location = new System.Drawing.Point(304, 323);
			this.chkHideBannedGrades.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.chkHideBannedGrades.Name = "chkHideBannedGrades";
			this.chkHideBannedGrades.Size = new System.Drawing.Size(296, 17);
			this.chkHideBannedGrades.TabIndex = 67;
			this.chkHideBannedGrades.Tag = "Checkbox_HideBannedCyberwareGrades";
			this.chkHideBannedGrades.Text = "Hide Banned Cyberware Grades";
			this.chkHideBannedGrades.UseVisualStyleBackColor = true;
			this.chkHideBannedGrades.CheckedChanged += new System.EventHandler(this.chkHideBannedGrades_CheckedChanged);
			// 
			// lblCyberwareNotes
			// 
			this.lblCyberwareNotes.AutoSize = true;
			this.tableLayoutPanel1.SetColumnSpan(this.lblCyberwareNotes, 3);
			this.lblCyberwareNotes.Location = new System.Drawing.Point(356, 300);
			this.lblCyberwareNotes.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
			this.lblCyberwareNotes.Name = "lblCyberwareNotes";
			this.lblCyberwareNotes.Size = new System.Drawing.Size(41, 13);
			this.lblCyberwareNotes.TabIndex = 31;
			this.lblCyberwareNotes.Text = "[Notes]";
			this.lblCyberwareNotes.Visible = false;
			// 
			// lblCyberwareNotesLabel
			// 
			this.lblCyberwareNotesLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.lblCyberwareNotesLabel.AutoSize = true;
			this.lblCyberwareNotesLabel.Location = new System.Drawing.Point(312, 300);
			this.lblCyberwareNotesLabel.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
			this.lblCyberwareNotesLabel.Name = "lblCyberwareNotesLabel";
			this.lblCyberwareNotesLabel.Size = new System.Drawing.Size(38, 13);
			this.lblCyberwareNotesLabel.TabIndex = 30;
			this.lblCyberwareNotesLabel.Tag = "Menu_Notes";
			this.lblCyberwareNotesLabel.Text = "Notes:";
			this.lblCyberwareNotesLabel.Visible = false;
			// 
			// lblSource
			// 
			this.lblSource.AutoSize = true;
			this.lblSource.Location = new System.Drawing.Point(356, 275);
			this.lblSource.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
			this.lblSource.Name = "lblSource";
			this.lblSource.Size = new System.Drawing.Size(47, 13);
			this.lblSource.TabIndex = 21;
			this.lblSource.Text = "[Source]";
			this.lblSource.Click += new System.EventHandler(this.OpenSourceFromLabel);
			// 
			// lblSourceLabel
			// 
			this.lblSourceLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.lblSourceLabel.AutoSize = true;
			this.lblSourceLabel.Location = new System.Drawing.Point(306, 275);
			this.lblSourceLabel.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
			this.lblSourceLabel.Name = "lblSourceLabel";
			this.lblSourceLabel.Size = new System.Drawing.Size(44, 13);
			this.lblSourceLabel.TabIndex = 20;
			this.lblSourceLabel.Tag = "Label_Source";
			this.lblSourceLabel.Text = "Source:";
			// 
			// chkShowOnlyAffordItems
			// 
			this.chkShowOnlyAffordItems.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.chkShowOnlyAffordItems.AutoSize = true;
			this.tableLayoutPanel1.SetColumnSpan(this.chkShowOnlyAffordItems, 4);
			this.chkShowOnlyAffordItems.Location = new System.Drawing.Point(304, 373);
			this.chkShowOnlyAffordItems.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.chkShowOnlyAffordItems.Name = "chkShowOnlyAffordItems";
			this.chkShowOnlyAffordItems.Size = new System.Drawing.Size(296, 17);
			this.chkShowOnlyAffordItems.TabIndex = 72;
			this.chkShowOnlyAffordItems.Tag = "Checkbox_ShowOnlyAffordItems";
			this.chkShowOnlyAffordItems.Text = "Show Only Items I Can Afford";
			this.chkShowOnlyAffordItems.UseVisualStyleBackColor = true;
			this.chkShowOnlyAffordItems.CheckedChanged += new System.EventHandler(this.chkHideOverAvailLimit_CheckedChanged);
			// 
			// flowLayoutPanel2
			// 
			this.flowLayoutPanel2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.flowLayoutPanel2.AutoSize = true;
			this.tableLayoutPanel1.SetColumnSpan(this.flowLayoutPanel2, 4);
			this.flowLayoutPanel2.Controls.Add(this.cmdOK);
			this.flowLayoutPanel2.Controls.Add(this.cmdOKAdd);
			this.flowLayoutPanel2.Controls.Add(this.cmdCancel);
			this.flowLayoutPanel2.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
			this.flowLayoutPanel2.Location = new System.Drawing.Point(363, 397);
			this.flowLayoutPanel2.Name = "flowLayoutPanel2";
			this.flowLayoutPanel2.Size = new System.Drawing.Size(237, 23);
			this.flowLayoutPanel2.TabIndex = 74;
			// 
			// cmdOKAdd
			// 
			this.cmdOKAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.cmdOKAdd.AutoSize = true;
			this.cmdOKAdd.Location = new System.Drawing.Point(81, 0);
			this.cmdOKAdd.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
			this.cmdOKAdd.Name = "cmdOKAdd";
			this.cmdOKAdd.Size = new System.Drawing.Size(75, 23);
			this.cmdOKAdd.TabIndex = 28;
			this.cmdOKAdd.Tag = "String_AddMore";
			this.cmdOKAdd.Text = "&Add && More";
			this.cmdOKAdd.UseVisualStyleBackColor = true;
			this.cmdOKAdd.Click += new System.EventHandler(this.cmdOKAdd_Click);
			// 
			// tableLayoutPanel2
			// 
			this.tableLayoutPanel2.ColumnCount = 2;
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel2.Controls.Add(this.cboGrade, 1, 1);
			this.tableLayoutPanel2.Controls.Add(this.label1, 0, 1);
			this.tableLayoutPanel2.Controls.Add(this.lstDrug, 0, 2);
			this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
			this.tableLayoutPanel2.Name = "tableLayoutPanel2";
			this.tableLayoutPanel2.RowCount = 3;
			this.tableLayoutPanel1.SetRowSpan(this.tableLayoutPanel2, 14);
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel2.Size = new System.Drawing.Size(301, 423);
			this.tableLayoutPanel2.TabIndex = 75;
			// 
			// cboGrade
			// 
			this.cboGrade.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.cboGrade.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboGrade.FormattingEnabled = true;
			this.cboGrade.Location = new System.Drawing.Point(48, 3);
			this.cboGrade.Name = "cboGrade";
			this.cboGrade.Size = new System.Drawing.Size(250, 21);
			this.cboGrade.TabIndex = 25;
			this.cboGrade.TooltipText = "";
			this.cboGrade.SelectedIndexChanged += new System.EventHandler(this.cboGrade_SelectedIndexChanged);
			this.cboGrade.EnabledChanged += new System.EventHandler(this.cboGrade_EnabledChanged);
			// 
			// label1
			// 
			this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(3, 6);
			this.label1.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(39, 13);
			this.label1.TabIndex = 24;
			this.label1.Tag = "Label_Grade";
			this.label1.Text = "Grade:";
			// 
			// lstDrug
			// 
			this.tableLayoutPanel2.SetColumnSpan(this.lstDrug, 2);
			this.lstDrug.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lstDrug.FormattingEnabled = true;
			this.lstDrug.Location = new System.Drawing.Point(3, 30);
			this.lstDrug.Name = "lstDrug";
			this.lstDrug.Size = new System.Drawing.Size(295, 390);
			this.lstDrug.TabIndex = 26;
			this.lstDrug.SelectedIndexChanged += new System.EventHandler(this.lstDrug_SelectedIndexChanged);
			this.lstDrug.DoubleClick += new System.EventHandler(this.lstDrug_DoubleClick);
			// 
			// lblAvailLabel
			// 
			this.lblAvailLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.lblAvailLabel.AutoSize = true;
			this.lblAvailLabel.Location = new System.Drawing.Point(461, 32);
			this.lblAvailLabel.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
			this.lblAvailLabel.Name = "lblAvailLabel";
			this.lblAvailLabel.Size = new System.Drawing.Size(33, 13);
			this.lblAvailLabel.TabIndex = 11;
			this.lblAvailLabel.Tag = "Label_Avail";
			this.lblAvailLabel.Text = "Avail:";
			// 
			// lblAvail
			// 
			this.lblAvail.AutoSize = true;
			this.lblAvail.Location = new System.Drawing.Point(500, 32);
			this.lblAvail.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
			this.lblAvail.Name = "lblAvail";
			this.lblAvail.Size = new System.Drawing.Size(19, 13);
			this.lblAvail.TabIndex = 12;
			this.lblAvail.Text = "[0]";
			// 
			// lblTestLabel
			// 
			this.lblTestLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.lblTestLabel.AutoSize = true;
			this.lblTestLabel.Location = new System.Drawing.Point(463, 275);
			this.lblTestLabel.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
			this.lblTestLabel.Name = "lblTestLabel";
			this.lblTestLabel.Size = new System.Drawing.Size(31, 13);
			this.lblTestLabel.TabIndex = 13;
			this.lblTestLabel.Tag = "Label_Test";
			this.lblTestLabel.Text = "Test:";
			// 
			// lblTest
			// 
			this.lblTest.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
			this.lblTest.AutoSize = true;
			this.lblTest.Location = new System.Drawing.Point(500, 275);
			this.lblTest.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
			this.lblTest.Name = "lblTest";
			this.lblTest.Size = new System.Drawing.Size(19, 13);
			this.lblTest.TabIndex = 14;
			this.lblTest.Text = "[0]";
			// 
			// lblCostLabel
			// 
			this.lblCostLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.lblCostLabel.AutoSize = true;
			this.lblCostLabel.Location = new System.Drawing.Point(319, 32);
			this.lblCostLabel.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
			this.lblCostLabel.Name = "lblCostLabel";
			this.lblCostLabel.Size = new System.Drawing.Size(31, 13);
			this.lblCostLabel.TabIndex = 15;
			this.lblCostLabel.Tag = "Label_Cost";
			this.lblCostLabel.Text = "Cost:";
			// 
			// lblCost
			// 
			this.lblCost.AutoSize = true;
			this.lblCost.Location = new System.Drawing.Point(356, 32);
			this.lblCost.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
			this.lblCost.Name = "lblCost";
			this.lblCost.Size = new System.Drawing.Size(19, 13);
			this.lblCost.TabIndex = 16;
			this.lblCost.Text = "[0]";
			// 
			// lblRatingLabel
			// 
			this.lblRatingLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.lblRatingLabel.AutoSize = true;
			this.lblRatingLabel.Location = new System.Drawing.Point(309, 57);
			this.lblRatingLabel.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
			this.lblRatingLabel.Name = "lblRatingLabel";
			this.lblRatingLabel.Size = new System.Drawing.Size(41, 13);
			this.lblRatingLabel.TabIndex = 2;
			this.lblRatingLabel.Tag = "Label_Rating";
			this.lblRatingLabel.Text = "Rating:";
			// 
			// flpCheckBoxes
			// 
			this.flpCheckBoxes.AutoSize = true;
			this.flpCheckBoxes.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.tableLayoutPanel1.SetColumnSpan(this.flpCheckBoxes, 4);
			this.flpCheckBoxes.Controls.Add(this.chkFree);
			this.flpCheckBoxes.Controls.Add(this.chkBlackMarketDiscount);
			this.flpCheckBoxes.Location = new System.Drawing.Point(301, 77);
			this.flpCheckBoxes.Margin = new System.Windows.Forms.Padding(0);
			this.flpCheckBoxes.Name = "flpCheckBoxes";
			this.flpCheckBoxes.Size = new System.Drawing.Size(225, 23);
			this.flpCheckBoxes.TabIndex = 77;
			// 
			// chkFree
			// 
			this.chkFree.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.chkFree.AutoSize = true;
			this.chkFree.Location = new System.Drawing.Point(3, 3);
			this.chkFree.Name = "chkFree";
			this.chkFree.Size = new System.Drawing.Size(50, 17);
			this.chkFree.TabIndex = 17;
			this.chkFree.Tag = "Checkbox_Free";
			this.chkFree.Text = "Free!";
			this.chkFree.UseVisualStyleBackColor = true;
			this.chkFree.CheckedChanged += new System.EventHandler(this.chkFree_CheckedChanged);
			// 
			// chkBlackMarketDiscount
			// 
			this.chkBlackMarketDiscount.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.chkBlackMarketDiscount.AutoSize = true;
			this.chkBlackMarketDiscount.Location = new System.Drawing.Point(59, 3);
			this.chkBlackMarketDiscount.Name = "chkBlackMarketDiscount";
			this.chkBlackMarketDiscount.Size = new System.Drawing.Size(163, 17);
			this.chkBlackMarketDiscount.TabIndex = 39;
			this.chkBlackMarketDiscount.Tag = "Checkbox_BlackMarketDiscount";
			this.chkBlackMarketDiscount.Text = "Black Market Discount (10%)";
			this.chkBlackMarketDiscount.UseVisualStyleBackColor = true;
			this.chkBlackMarketDiscount.Visible = false;
			this.chkBlackMarketDiscount.CheckedChanged += new System.EventHandler(this.chkBlackMarketDiscount_CheckedChanged);
			// 
			// flpRating
			// 
			this.flpRating.AutoSize = true;
			this.flpRating.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.tableLayoutPanel1.SetColumnSpan(this.flpRating, 3);
			this.flpRating.Location = new System.Drawing.Point(301, 173);
			this.flpRating.Margin = new System.Windows.Forms.Padding(0);
			this.flpRating.Name = "flpRating";
			this.flpRating.Size = new System.Drawing.Size(0, 0);
			this.flpRating.TabIndex = 73;
			this.flpRating.WrapContents = false;
			// 
			// lblMarkupLabel
			// 
			this.lblMarkupLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.lblMarkupLabel.AutoSize = true;
			this.lblMarkupLabel.Location = new System.Drawing.Point(304, 106);
			this.lblMarkupLabel.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
			this.lblMarkupLabel.Name = "lblMarkupLabel";
			this.lblMarkupLabel.Size = new System.Drawing.Size(46, 13);
			this.lblMarkupLabel.TabIndex = 40;
			this.lblMarkupLabel.Tag = "Label_SelectGear_Markup";
			this.lblMarkupLabel.Text = "Markup:";
			// 
			// flpMarkup
			// 
			this.flpMarkup.AutoSize = true;
			this.flpMarkup.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.tableLayoutPanel1.SetColumnSpan(this.flpMarkup, 3);
			this.flpMarkup.Controls.Add(this.nudMarkup);
			this.flpMarkup.Controls.Add(this.lblMarkupPercentLabel);
			this.flpMarkup.Location = new System.Drawing.Point(353, 100);
			this.flpMarkup.Margin = new System.Windows.Forms.Padding(0);
			this.flpMarkup.Name = "flpMarkup";
			this.flpMarkup.Size = new System.Drawing.Size(127, 26);
			this.flpMarkup.TabIndex = 16;
			// 
			// nudMarkup
			// 
			this.nudMarkup.DecimalPlaces = 2;
			this.nudMarkup.Location = new System.Drawing.Point(3, 3);
			this.nudMarkup.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
			this.nudMarkup.Minimum = new decimal(new int[] {
            99,
            0,
            0,
            -2147483648});
			this.nudMarkup.Name = "nudMarkup";
			this.nudMarkup.Size = new System.Drawing.Size(100, 20);
			this.nudMarkup.TabIndex = 41;
			this.nudMarkup.ValueChanged += new System.EventHandler(this.nudMarkup_ValueChanged);
			// 
			// lblMarkupPercentLabel
			// 
			this.lblMarkupPercentLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
			this.lblMarkupPercentLabel.AutoSize = true;
			this.lblMarkupPercentLabel.Location = new System.Drawing.Point(109, 6);
			this.lblMarkupPercentLabel.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
			this.lblMarkupPercentLabel.Name = "lblMarkupPercentLabel";
			this.lblMarkupPercentLabel.Size = new System.Drawing.Size(15, 14);
			this.lblMarkupPercentLabel.TabIndex = 42;
			this.lblMarkupPercentLabel.Text = "%";
			// 
			// frmSelectDrug
			// 
			this.AcceptButton = this.cmdOK;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoSize = true;
			this.CancelButton = this.cmdCancel;
			this.ClientSize = new System.Drawing.Size(624, 441);
			this.Controls.Add(this.tableLayoutPanel1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MaximumSize = new System.Drawing.Size(640, 10000);
			this.MinimizeBox = false;
			this.Name = "frmSelectDrug";
			this.Padding = new System.Windows.Forms.Padding(9);
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Tag = "Title_SelectCyberware";
			this.Text = "Select Cyberware";
			this.Load += new System.EventHandler(this.frmSelectDrug_Load);
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.nudRating)).EndInit();
			this.flowLayoutPanel2.ResumeLayout(false);
			this.flowLayoutPanel2.PerformLayout();
			this.tableLayoutPanel2.ResumeLayout(false);
			this.tableLayoutPanel2.PerformLayout();
			this.flpCheckBoxes.ResumeLayout(false);
			this.flpCheckBoxes.PerformLayout();
			this.flpMarkup.ResumeLayout(false);
			this.flpMarkup.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.nudMarkup)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label1;
        private ElasticComboBox cboGrade;
        private System.Windows.Forms.ListBox lstDrug;
        private System.Windows.Forms.Label lblAvail;
        private System.Windows.Forms.Label lblAvailLabel;
        private System.Windows.Forms.Label lblCost;
        private System.Windows.Forms.Label lblCostLabel;
        private System.Windows.Forms.Label lblRatingLabel;
        private System.Windows.Forms.NumericUpDown nudRating;
        private System.Windows.Forms.Label lblSource;
        private System.Windows.Forms.Label lblSourceLabel;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Label lblSearchLabel;
        private System.Windows.Forms.CheckBox chkFree;
        private System.Windows.Forms.Label lblTest;
        private System.Windows.Forms.Label lblTestLabel;
        private System.Windows.Forms.Label lblCyberwareNotes;
        private System.Windows.Forms.Label lblCyberwareNotesLabel;
        private System.Windows.Forms.CheckBox chkBlackMarketDiscount;
        private System.Windows.Forms.NumericUpDown nudMarkup;
        private System.Windows.Forms.Label lblMarkupLabel;
        private System.Windows.Forms.Label lblMarkupPercentLabel;
        private System.Windows.Forms.CheckBox chkHideOverAvailLimit;
        private System.Windows.Forms.CheckBox chkHideBannedGrades;
        private Chummer.BufferedTableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.CheckBox chkShowOnlyAffordItems;
        private System.Windows.Forms.FlowLayoutPanel flpRating;
        private System.Windows.Forms.Label lblRatingNALabel;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel2;
        private System.Windows.Forms.Button cmdOK;
        private System.Windows.Forms.Button cmdOKAdd;
        private System.Windows.Forms.Button cmdCancel;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.FlowLayoutPanel flpMarkup;
        private System.Windows.Forms.FlowLayoutPanel flpCheckBoxes;
    }
}
