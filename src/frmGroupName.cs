using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace jmFidExt
{
    public partial class frmGroupName : Form
    {
        public frmGroupName()
        {
            InitializeComponent();
        }

        public override string Text
        {
            get
            {
                return txtName.Text;
            }
            set 
            {
                txtName.Text = value;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }
    }
}
