namespace jmFidExt
{
    partial class RegControl
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.chkEnabled = new System.Windows.Forms.CheckBox();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.添加规则ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.移除ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.btnRight = new System.Windows.Forms.Button();
            this.btnLeft = new System.Windows.Forms.Button();
            this.btnAddGroup = new System.Windows.Forms.Button();
            this.myRuleContainer = new System.Windows.Forms.TabControl();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.txtMatch = new System.Windows.Forms.ComboBox();
            this.txtAction = new System.Windows.Forms.TextBox();
            this.contextMenuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // chkEnabled
            // 
            this.chkEnabled.AutoSize = true;
            this.chkEnabled.Checked = true;
            this.chkEnabled.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkEnabled.Location = new System.Drawing.Point(172, 9);
            this.chkEnabled.Name = "chkEnabled";
            this.chkEnabled.Size = new System.Drawing.Size(96, 16);
            this.chkEnabled.TabIndex = 0;
            this.chkEnabled.Text = "所有配置生效";
            this.chkEnabled.UseVisualStyleBackColor = true;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.添加规则ToolStripMenuItem,
            this.移除ToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(125, 48);
            this.contextMenuStrip1.Text = "添加规则";
            // 
            // 添加规则ToolStripMenuItem
            // 
            this.添加规则ToolStripMenuItem.Name = "添加规则ToolStripMenuItem";
            this.添加规则ToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.添加规则ToolStripMenuItem.Text = "添加规则";
            this.添加规则ToolStripMenuItem.Click += new System.EventHandler(this.m_addmenu_ItemClicked);
            // 
            // 移除ToolStripMenuItem
            // 
            this.移除ToolStripMenuItem.Name = "移除ToolStripMenuItem";
            this.移除ToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.移除ToolStripMenuItem.Text = "移除";
            this.移除ToolStripMenuItem.Click += new System.EventHandler(this.移除ToolStripMenuItem_Click);
            // 
            // splitContainer1
            // 
            this.splitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.btnRight);
            this.splitContainer1.Panel1.Controls.Add(this.btnLeft);
            this.splitContainer1.Panel1.Controls.Add(this.btnAddGroup);
            this.splitContainer1.Panel1.Controls.Add(this.myRuleContainer);
            this.splitContainer1.Panel1.Controls.Add(this.chkEnabled);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.label3);
            this.splitContainer1.Panel2.Controls.Add(this.label2);
            this.splitContainer1.Panel2.Controls.Add(this.label1);
            this.splitContainer1.Panel2.Controls.Add(this.txtName);
            this.splitContainer1.Panel2.Controls.Add(this.btnSave);
            this.splitContainer1.Panel2.Controls.Add(this.txtMatch);
            this.splitContainer1.Panel2.Controls.Add(this.txtAction);
            this.splitContainer1.Size = new System.Drawing.Size(951, 652);
            this.splitContainer1.SplitterDistance = 471;
            this.splitContainer1.TabIndex = 2;
            // 
            // btnRight
            // 
            this.btnRight.Location = new System.Drawing.Point(124, 5);
            this.btnRight.Name = "btnRight";
            this.btnRight.Size = new System.Drawing.Size(32, 23);
            this.btnRight.TabIndex = 4;
            this.btnRight.Text = ">";
            this.btnRight.UseVisualStyleBackColor = true;
            this.btnRight.Click += new System.EventHandler(this.btnRight_Click);
            // 
            // btnLeft
            // 
            this.btnLeft.Location = new System.Drawing.Point(86, 5);
            this.btnLeft.Name = "btnLeft";
            this.btnLeft.Size = new System.Drawing.Size(32, 23);
            this.btnLeft.TabIndex = 3;
            this.btnLeft.Text = "<";
            this.btnLeft.UseVisualStyleBackColor = true;
            this.btnLeft.Click += new System.EventHandler(this.btnLeft_Click);
            // 
            // btnAddGroup
            // 
            this.btnAddGroup.Location = new System.Drawing.Point(5, 5);
            this.btnAddGroup.Name = "btnAddGroup";
            this.btnAddGroup.Size = new System.Drawing.Size(75, 23);
            this.btnAddGroup.TabIndex = 2;
            this.btnAddGroup.Text = "增加分组";
            this.btnAddGroup.UseVisualStyleBackColor = true;
            this.btnAddGroup.Click += new System.EventHandler(this.btnAddGroup_Click);
            // 
            // myRuleContainer
            // 
            this.myRuleContainer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.myRuleContainer.DrawMode = System.Windows.Forms.TabDrawMode.OwnerDrawFixed;
            this.myRuleContainer.Location = new System.Drawing.Point(-1, 34);
            this.myRuleContainer.Name = "myRuleContainer";
            this.myRuleContainer.SelectedIndex = 0;
            this.myRuleContainer.Size = new System.Drawing.Size(951, 432);
            this.myRuleContainer.TabIndex = 1;
            this.myRuleContainer.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.myRuleContainer_DrawItem);
            this.myRuleContainer.SelectedIndexChanged += new System.EventHandler(this.myRuleContainer_SelectedIndexChanged);
            this.myRuleContainer.DoubleClick += new System.EventHandler(this.myRuleContainer_DoubleClick);
            this.myRuleContainer.MouseDown += new System.Windows.Forms.MouseEventHandler(this.myRuleContainer_MouseDown);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 50);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 12);
            this.label3.TabIndex = 8;
            this.label3.Text = "Match:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 76);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(47, 12);
            this.label2.TabIndex = 7;
            this.label2.Text = "Action:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 12);
            this.label1.TabIndex = 6;
            this.label1.Text = "Name:";
            // 
            // txtName
            // 
            this.txtName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtName.Location = new System.Drawing.Point(56, 15);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(772, 21);
            this.txtName.TabIndex = 5;
            this.txtName.Text = "名称";
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Location = new System.Drawing.Point(845, 14);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 4;
            this.btnSave.Text = "保存";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // txtMatch
            // 
            this.txtMatch.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtMatch.AutoCompleteCustomSource.AddRange(new string[] {
            "reg://(qian|qian-img).(tenpay|qq).com/(.*) #多域名匹配正则",
            "reg://qian.tenpay.com/(app|fcgi)/(.*).[f]?cgi(.*) #域名下指定后缀正则"});
            this.txtMatch.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.txtMatch.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.txtMatch.FormattingEnabled = true;
            this.txtMatch.Items.AddRange(new object[] {
            "regex://(qian|qian-img).(tenpay|qq).com/(.*)  #匹配多个域名",
            "regex:/(app|fcgi|fund_act_fcg)/(.*).[f]?cgi(.*)    #匹配特定的请求",
            "http://qian.tenpay.com/fund_act_fcg/node/act.cgi   #完整匹配"});
            this.txtMatch.Location = new System.Drawing.Point(56, 47);
            this.txtMatch.Name = "txtMatch";
            this.txtMatch.Size = new System.Drawing.Size(864, 20);
            this.txtMatch.TabIndex = 3;
            // 
            // txtAction
            // 
            this.txtAction.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtAction.Location = new System.Drawing.Point(56, 73);
            this.txtAction.Multiline = true;
            this.txtAction.Name = "txtAction";
            this.txtAction.Size = new System.Drawing.Size(864, 90);
            this.txtAction.TabIndex = 1;
            // 
            // RegControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Name = "RegControl";
            this.Size = new System.Drawing.Size(951, 652);
            this.contextMenuStrip1.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.CheckBox chkEnabled;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TextBox txtAction;
        private System.Windows.Forms.ComboBox txtMatch;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 添加规则ToolStripMenuItem;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.TabControl myRuleContainer;
        private System.Windows.Forms.Button btnAddGroup;
        private System.Windows.Forms.ToolStripMenuItem 移除ToolStripMenuItem;
        private System.Windows.Forms.Button btnRight;
        private System.Windows.Forms.Button btnLeft;

    }
}
