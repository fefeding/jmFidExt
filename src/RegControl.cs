using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace jmFidExt
{
    public partial class RegControl : UserControl
    {
        const int CLOSE_SIZE = 10;
        static string CONFIGPATH = "jmFidExt.conf";
        RuleConfig currentConfig = new RuleConfig();

        public RegControl()
        {
            InitializeComponent();
            this.Load += RegControl_Load;
        }

        void RegControl_Load(object sender, EventArgs e)
        {  
            CONFIGPATH = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), CONFIGPATH);
            LoadRules();

            
        }

        void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            //如果值改变
            if (e.ColumnIndex == 0)
            {
                currentConfig.groups = null;
                SaveRules();
            }
        }

        /// <summary>
        /// 如果值发生改变，则保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            //如果值改变
            if (e.ColumnIndex == 0)
            {
                currentConfig.groups = null;
                SaveRules();
            }
        }

        /// <summary>
        /// 选择一进行修改
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            var grid = sender as DataGridView;
            var curRow = grid.SelectedRows.Count > 0 ? grid.SelectedRows[0] : grid.CurrentRow;
            if (curRow != null)
            {
                var r = GetRule(curRow);
                txtName.Text = r.name;
                txtMatch.Text = r.match;
                txtAction.Text = r.action;
                return;
            }
           
        }
        
        /// <summary>
        /// 添加规则
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void m_addmenu_ItemClicked(object sender, EventArgs e)
        {
            try
            {
                var grid = getCurrentGrid();
                if (grid == null) return;
                SetRule(grid, new Rule()
                {
                    enabled = true,
                    match = "",
                    action = "",
                    name = ""
                });
            }
            catch (Exception ex)
            {
                Utils.FiddlerLog(ex.ToString());
            }
        }

        /// <summary>
        /// 添加一条规则
        /// </summary>
        /// <param name="r"></param>
        private void SetRule(DataGridView grid ,Rule r, int index=-1)
        {
            var curRow = grid.SelectedRows.Count > 0 ? grid.SelectedRows[0] : grid.CurrentRow;
            var row = index < 0 || curRow==null ? new DataGridViewRow() : curRow;
            
            if(r != null)
            {
                foreach (DataGridViewColumn c in grid.Columns)
                {  
                    var cell = c.CellTemplate.Clone() as DataGridViewCell;
                    //if (cell.ColumnIndex < 0) continue;

                    foreach (DataGridViewCell cn in row.Cells)
                    {
                        if (cn.OwningColumn == c)
                        {
                            cell = cn;
                            break;
                        }
                    }
                    switch (c.DataPropertyName)
                    {
                        case "enabled":
                            {
                                cell.Value = r.enabled;
                                break;
                            }
                        case "name":
                            {
                                cell.Value = r.name;
                                break;
                            }
                        case "match":
                            {
                                cell.Value = r.match;
                                break;
                            }
                        case "action":
                            {
                                cell.Value = r.action;
                                break;
                            }
                    }
                    if(!row.Cells.Contains(cell)) row.Cells.Add(cell);
                }
                
            }
            if(index < 0)
            {
                grid.Rows.Add(row);
                row.Selected = true;
            }            
        }


        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                var gird = getCurrentGrid();
                if (gird == null) return;

                var curRow = gird.SelectedRows.Count > 0 ? gird.SelectedRows[0] : gird.CurrentRow;
                var r = curRow != null?GetRule(curRow):new Rule();
                r.match = txtMatch.Text.Trim();
                r.action = txtAction.Text.Trim();
                r.name = txtName.Text;

                var tipIndex = r.match.IndexOf(" #");
                if (tipIndex > -1)
                {
                    r.match = r.match.Substring(0, tipIndex);
                }

                if (r.match.StartsWith("regex:"))
                {
                    try
                    {
                        var s = System.Text.RegularExpressions.Regex.Replace(r.match, "^regex:", "", System.Text.RegularExpressions.RegexOptions.IgnorePatternWhitespace);
                        var reg = new System.Text.RegularExpressions.Regex(s, System.Text.RegularExpressions.RegexOptions.IgnorePatternWhitespace);

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("表示式"+r.match+"不正确," + ex.Message);
                        return;
                    }
                }

                if (curRow != null)
                {
                    SetRule(gird, r, curRow.Index);
                }
                else
                {
                    r.enabled = true;
                    SetRule(gird, r, -1);
                }

                currentConfig.groups = null;
                SaveRules();//写入IO
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// 根据配置处理请求
        /// </summary>
        /// <param name="session"></param>
        public void ReplaceRequest(Fiddler.Session session)
        {
            //不采用，则不处理
            if (!chkEnabled.Checked) return;

            var config = GetRules();
            if (config.groups == null || config.groups.Count == 0) return;
            foreach (var g in config.groups)
            {
                if (g.rules == null || g.rules.Count == 0 || !g.enabled) continue;
                foreach (var r in g.rules)
                {
                    if (!r.enabled) continue;
                    //如果是正则，且命中，则处理
                    if (r.match.StartsWith("regex:", StringComparison.OrdinalIgnoreCase))
                    {
                        try
                        {
                            var s = System.Text.RegularExpressions.Regex.Replace(r.match, "^regex:", "", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                            var reg = new System.Text.RegularExpressions.Regex(s, System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                            if (reg.IsMatch(session.fullUrl))
                            {                                
                                Utils.ResetSession(r, session, reg);
                                return;
                            }
                        }
                        catch (Exception ex)
                        {
                            Utils.FiddlerLog("表示式" + r.match + "不正确," + ex.Message);
                            return;
                        }
                    }
                    //hostname
                    else if (session.HostnameIs(r.match) || session.host == r.match)
                    {
                        Utils.ResetSession(r, session);
                        return;
                    }
                    //请求完全等于，也命中
                    else if (session.fullUrl.Equals(r.match, StringComparison.OrdinalIgnoreCase))
                    {
                        Utils.ResetSession(r, session);
                        return;
                    }
                }
            }            
        }

        /// <summary>
        /// 获取所有规则
        /// </summary>
        /// <returns></returns>
        public RuleConfig GetRules()
        {
            currentConfig.enabled = chkEnabled.Checked;
            if (currentConfig.groups == null)
            {
                currentConfig.groups = new List<GroupRule>();
                foreach (TabPage tab in myRuleContainer.TabPages)
                {
                    if (tab == null) continue;
                    var grid = getCurrentGrid(tab);
                    if (grid == null) continue;

                    
                    var g = new GroupRule();
                    g.name = tab.Text.Trim();

                    var chk = getCurrentChk(tab);
                    if (chk != null) g.enabled = chk.Checked;

                    foreach (DataGridViewRow row in grid.Rows)
                    {
                        var r = GetRule(row);
                        g.rules.Add(r);
                    }

                    currentConfig.groups.Add(g);
                }               
            }

            return currentConfig;
        }

        private Rule GetRule(DataGridViewRow row)
        {
            var r = new Rule();
            foreach (DataGridViewCell cn in row.Cells)
            {
                switch (cn.OwningColumn.DataPropertyName)
                {
                    case "enabled":
                        {
                            r.enabled = (bool)cn.Value;
                            break;
                        }
                    case "name":
                        {
                            r.name = cn.Value != null ? cn.Value.ToString() : "";
                            break;
                        }
                    case "match":
                        {
                            r.match = cn.Value != null ? cn.Value.ToString().Trim() : "";
                            break;
                        }
                    case "action":
                        {
                            r.action = cn.Value != null ? cn.Value.ToString().Trim() : "";
                            break;
                        }
                }
            }
            return r;
        }

        /// <summary>
        /// 加载配置文件
        /// </summary>
        private void LoadRules()
        {
            try
            {
                if(System.IO.File.Exists(CONFIGPATH))
                {
                
                    var json = System.IO.File.ReadAllText(CONFIGPATH, Encoding.UTF8);
                    
                    //表示是旧的，为了兼容，重新生成一个
                    if (!string.IsNullOrWhiteSpace(json) && json.Trim().StartsWith("["))
                    {
                        var config = new RuleConfig();
                        config.enabled = currentConfig.enabled;
                        //config.groups = new List<GroupRule>();
                        var g = new GroupRule();
                        g.name = "默认";
                        g.rules = Utils.JsonToModel<List<Rule>>(json);
                        currentConfig.groups.Add(g);
                    }
                    else
                    {
                        currentConfig = Utils.JsonToModel<RuleConfig>(json);
                        chkEnabled.Checked = currentConfig.enabled;
                    }
                }

                if (currentConfig.groups.Count == 0)
                {
                    var g = new GroupRule();
                    g.name = "默认";
                    currentConfig.groups.Add(g);
                }

                if (currentConfig.groups.Count > 0)
                {
                    foreach (var g in currentConfig.groups)
                    {
                        addTab(g);
                    }
                }
            }
            catch (Exception ex)
            {
                Utils.FiddlerLog("加载配置文件失败：" + ex.Message);
            }
        }

        private DataGridView getCurrentGrid(TabPage tab = null)
        {
            if (tab == null) tab = myRuleContainer.SelectedTab;
            if (tab == null) return null;
            foreach (var c in tab.Controls)
            {
                if (c is DataGridView) return c as DataGridView;
            }
            return null;
        }

        private CheckBox getCurrentChk(TabPage tab = null)
        {
            if (tab == null) tab = myRuleContainer.SelectedTab;
            if (tab == null) return null;
            foreach (var c in tab.Controls)
            {
                if (c is CheckBox) return c as CheckBox;
            }
            return null;
        }

        /// <summary>
        /// 把当前的配置保存到文件
        /// </summary>
        private void SaveRules()
        {
            try
            {
                currentConfig.groups = null;
                var rules = GetRules();
                var json = Utils.ModelToJson(rules);
                System.IO.File.WriteAllText(CONFIGPATH, json, Encoding.UTF8);
            }
            catch (Exception ex)
            {
                Utils.FiddlerLog(ex.Message);
            }
        }

        /// <summary>
        /// 添加一个tab
        /// </summary>
        /// <param name="g"></param>
        private TabPage addTab(GroupRule g)
        {
            var tab = new TabPage();
            
            var grid = createRuleGrid();            
            if(!string.IsNullOrWhiteSpace(g.name)) tab.Text = g.name;
            tab.Text += "  ";

            if (g.rules != null)
            {
                foreach (var r in g.rules)
                {
                    SetRule(grid, r, -1);
                }
            }

            var chk = new CheckBox();
            chk.Location = new Point(6,3);
            chk.TabIndex = 1;
            //chk.Text = "生效";
            chk.Checked = g.enabled;
            chk.Size = new System.Drawing.Size(30, 16);
            tab.Controls.Add(chk);

            tab.Controls.Add(grid);

            chk.CheckedChanged += chk_CheckedChanged;

            //可以拖放
           // tab.AllowDrop = true;

            myRuleContainer.TabPages.Add(tab);

            
            return tab;
        }

        void chk_CheckedChanged(object sender, EventArgs e)
        {
            currentConfig.groups = null;
            myRuleContainer.Refresh();
            SaveRules();

        }

        /// <summary>
        /// 生成规则表格
        /// </summary>
        /// <returns></returns>
        private DataGridView createRuleGrid() {
            var g = new DataGridView();
            g.AllowDrop = true;
            g.AllowUserToAddRows = false;
            g.AutoGenerateColumns = false;
            g.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            g.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            g.ContextMenuStrip = this.contextMenuStrip1;
            g.Dock = System.Windows.Forms.DockStyle.Fill;
            g.Location = new System.Drawing.Point(3, 3);
            g.MultiSelect = false;
            //g.Name = "dataGridView1";
            g.RowTemplate.Height = 23;
            g.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;

            // 
            // c_chk
            // 
            var c_chk = new DataGridViewCheckBoxColumn();
            c_chk.DataPropertyName = "enabled";
            c_chk.FillWeight = 24.27272F;
            c_chk.HeaderText = "Enabled";
            c_chk.Name = "c_chk";
            g.Columns.Add(c_chk);
            
            // 
            // c_match
            // 
            var c_match = new DataGridViewTextBoxColumn();
            c_match.DataPropertyName = "match";
            c_match.FillWeight = 147.2545F;
            c_match.HeaderText = "Match";
            c_match.Name = "c_match";
            c_match.ReadOnly = true;
            c_match.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            c_match.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            g.Columns.Add(c_match);
            // 
            // c_action
            // 
            var c_action = new DataGridViewTextBoxColumn();
            c_action.DataPropertyName = "action";
            c_action.FillWeight = 147.2545F;
            c_action.HeaderText = "Action";
            c_action.Name = "c_action";
            c_action.ReadOnly = true;
            c_action.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            c_action.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            g.Columns.Add(c_action);

            // 
            // c_name
            // 
            var c_name = new DataGridViewTextBoxColumn();
            c_name.DataPropertyName = "name";
            c_name.FillWeight = 81.21828F;
            c_name.HeaderText = "Name";
            c_name.Name = "c_name";
            c_name.ReadOnly = true;
            g.Columns.Add(c_name);

            //g.Size = new System.Drawing.Size(927, 388);
            g.TabIndex = 1;
            g.CellMouseMove += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridView_CellMouseMove);
            g.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.dataGridView_RowsAdded);
            g.SelectionChanged += new System.EventHandler(this.dataGridView1_SelectionChanged);
            g.DragDrop += new System.Windows.Forms.DragEventHandler(this.dataGridView_DragDrop);
            g.DragEnter += new System.Windows.Forms.DragEventHandler(this.dataGridView_DragEnter);
            
            g.CellEndEdit += dataGridView1_CellEndEdit;
            return g;
        }

        private void btnAddGroup_Click(object sender, EventArgs e)
        {
            var g = new GroupRule();
            currentConfig.groups.Add(g);
            g.name = "双击改名" + currentConfig.groups.Count;
            var tab = addTab(g);
            myRuleContainer.SelectTab(tab);
            SaveRules();
        }

        private void 移除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var grid = getCurrentGrid();
            if (grid == null) return;
            var curRow = grid.SelectedRows.Count > 0 ? grid.SelectedRows[0] : grid.CurrentRow;
            if (curRow != null)
            {
                if (MessageBox.Show("确定移除？", "删除规则", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    grid.Rows.Remove(curRow);

                    SaveRules();
                }
                
            }
        }

        /// <summary>
        /// 切换TAB
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void myRuleContainer_SelectedIndexChanged(object sender, EventArgs e)
        {
            var grid = getCurrentGrid();
            if (grid == null) return;

            var curRow = grid.SelectedRows.Count > 0 ? grid.SelectedRows[0] : grid.CurrentRow;
            if (curRow != null)
            {
                var r = GetRule(curRow);
                txtName.Text = r.name;
                txtMatch.Text = r.match;
                txtAction.Text = r.action;
                return;
            }
        }


        private void dataGridView_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
        }

        private void dataGridView_CellMouseMove(object sender, DataGridViewCellMouseEventArgs e)
        {
            var grid = sender as DataGridView;
            if (e.Clicks < 2 && e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                //拖动
                if (e.ColumnIndex == -1 && e.RowIndex > -1)
                {
                    grid.DoDragDrop(grid.Rows[e.RowIndex], DragDropEffects.Move);
                }
            }
        }

        int selectionIdx = 0;
        private void dataGridView_DragDrop(object sender, DragEventArgs e)
        {
            try
            {
                var grid = sender as DataGridView;

                DataGridViewRow row = (DataGridViewRow)e.Data.GetData(typeof(DataGridViewRow));
                if (row == null) return;

                int idx = GetRowFromPoint(grid, e.X, e.Y);

                if (idx < 0 || idx == row.Index) return;

                if (e.Data.GetDataPresent(typeof(DataGridViewRow)))
                {
                    grid.Rows.Remove(row);
                    selectionIdx = idx;
                    grid.Rows.Insert(idx, row);
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex);
            }
        }
        private int GetRowFromPoint(DataGridView grid, int x, int y)
        {
            var maxY = 0;
            for (int i = 0; i < grid.RowCount; i++)
            {
                Rectangle rec = grid.GetRowDisplayRectangle(i, false);
                var nrec = grid.RectangleToScreen(rec);
                //指向第一行
                if (y < nrec.Y && i == 0)
                {
                    return 0;
                }
                if (nrec.Contains(x, y))
                    return i;

                maxY = nrec.Bottom;
            }
            //如果是指向最后一行
            if (y >= maxY)
            {
                return grid.RowCount - 1;
            }

            return -1;
        }

        private void dataGridView_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            if (selectionIdx > -1)
            {
                var grid = sender as DataGridView;
                grid.Rows[selectionIdx].Selected = true;
                grid.CurrentCell = grid.Rows[selectionIdx].Cells[0];
            }
        }

        /// <summary>
        /// 左移
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLeft_Click(object sender, EventArgs e)
        {
            if (myRuleContainer.SelectedTab != null && myRuleContainer.SelectedIndex > 0)
            {
                var tab = myRuleContainer.SelectedTab;
                var index = myRuleContainer.SelectedIndex;
                myRuleContainer.TabPages.Remove(tab);
                myRuleContainer.TabPages.Insert(index-1, tab);
                myRuleContainer.SelectTab(tab);

                SaveRules();
            }
        }

        /// <summary>
        /// 右移
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRight_Click(object sender, EventArgs e)
        {
            if (myRuleContainer.SelectedTab != null && myRuleContainer.SelectedIndex < myRuleContainer.TabPages.Count-1)
            {
                var tab = myRuleContainer.SelectedTab;
                var index = myRuleContainer.SelectedIndex;
                myRuleContainer.TabPages.Remove(tab);
                myRuleContainer.TabPages.Insert(index + 1, tab);
                myRuleContainer.SelectTab(tab);

                SaveRules();
            }
        }

        private void myRuleContainer_DoubleClick(object sender, EventArgs e)
        {
            var tab = myRuleContainer.SelectedTab;
            if (tab == null) return;

            var frm = new frmGroupName();
            frm.Text = tab.Text.Trim();
            if (frm.ShowDialog() == DialogResult.OK)
            {
                tab.Text = frm.Text + "  ";
                SaveRules();
            }
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            var tab = myRuleContainer.SelectedTab;
            if (tab == null) return;
            
            if (MessageBox.Show("确定移除？", "删除分组", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                myRuleContainer.TabPages.Remove(tab);
                SaveRules();
            }
        }

        private void myRuleContainer_DrawItem(object sender, DrawItemEventArgs e)
        {
            try
            {
                var c = sender as TabControl;
                var tab = c.TabPages[e.Index];
                var chk = getCurrentChk(tab);
                
                Rectangle tagRect = c.GetTabRect(e.Index);
                e.DrawBackground();
               // e.DrawFocusRectangle();

                /*var bgColor = chk.Checked ? Color.LightGreen : Color.FromArgb(10986917);//生效和不生效的用不同的颜色
                //选中的采用独有的颜色
                if (e.State == DrawItemState.Selected)
                {
                    bgColor = Color.DarkGreen;
                }

                using (var bgBrush = new SolidBrush(bgColor))
                {
                    e.Graphics.FillRectangle(bgBrush, tagRect);
                }*/

                Brush foreBrush = SystemBrushes.ActiveCaptionText;
                if(!chk.Checked)
                {
                    foreBrush = new SolidBrush(Color.FromArgb(100,26,26,26));
                }
                e.Graphics.DrawString(tab.Text, this.Font, foreBrush, tagRect.X + 2, tagRect.Y + 2);
                
                //再画一个矩形框
                using (var p = new Pen(Color.White))
                {
                    tagRect.Offset(tagRect.Width - (CLOSE_SIZE + 3), 2);
                    tagRect.Width = CLOSE_SIZE;
                    tagRect.Height = CLOSE_SIZE;
                    e.Graphics.DrawRectangle(p, tagRect);
                }

                //填充矩形框
                /*Color recColor = e.State == DrawItemState.Selected ? Color.White : Color.DarkSlateGray;
                using (Brush b = new SolidBrush(recColor))
                {
                    e.Graphics.FillRectangle(b, myTabRect);
                }*/

                //画关闭符号
                using (Pen objpen = new Pen(Color.Black))
                {
                    ////=============================================
                    //自己画X
                    ////"\"线
                    Point p1 = new Point(tagRect.X + 3, tagRect.Y + 3);
                    Point p2 = new Point(tagRect.X + tagRect.Width - 3, tagRect.Y + tagRect.Height - 3);
                    e.Graphics.DrawLine(objpen, p1, p2);
                    //"/"线
                    Point p3 = new Point(tagRect.X + 3, tagRect.Y + tagRect.Height - 3);
                    Point p4 = new Point(tagRect.X + tagRect.Width - 3, tagRect.Y + 3);
                    e.Graphics.DrawLine(objpen, p3, p4);

                    ////=============================================
                    //使用图片
                    //Bitmap bt = new Bitmap(image);
                    //Point p5 = new Point(myTabRect.X, 4);
                    //e.Graphics.DrawImage(bt, p5);
                    //e.Graphics.DrawString(this.MainTabControl.TabPages[e.Index].Text, this.Font, objpen.Brush, p5);
                }
                e.Graphics.Dispose();
            }
            catch (Exception)
            { }
        }

        private void myRuleContainer_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                if (e.Button == MouseButtons.Left)
                {
                    int x = e.X, y = e.Y;
                    TabPage tab = null;
                    Rectangle rect = new Rectangle();

                    for (var i = 0; i < myRuleContainer.TabPages.Count; i++)
                    {
                        rect = myRuleContainer.GetTabRect(i);
                        if (rect.Contains(e.Location))
                        {
                            tab = myRuleContainer.TabPages[i] as TabPage;
                            break;
                        }
                    }

                    //计算关闭区域  
                    if (tab == null) return;

                    rect.Offset(rect.Width - (CLOSE_SIZE + 3), 2);
                    rect.Width = CLOSE_SIZE;
                    rect.Height = CLOSE_SIZE;

                    //如果鼠标在区域内就关闭选项卡   
                    bool isClose = x > rect.X && x < rect.Right && y > rect.Y && y < rect.Bottom;
                    if (isClose == true)
                    {
                        if (MessageBox.Show("确定移除？", "删除分组", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            myRuleContainer.TabPages.Remove(tab);
                            SaveRules();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex);
             }
        }
    }
}
