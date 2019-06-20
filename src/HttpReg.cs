using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Fiddler;

namespace jmFidExt
{
    public class HttpReg: Fiddler.IAutoTamper
    {
        RegControl oView = new RegControl();

        public void AutoTamperRequestAfter(Session oSession)
        {
            //throw new NotImplementedException();
        }

        public void AutoTamperRequestBefore(Session oSession)
        {
            try
            {
                //throw new NotImplementedException();
                //FiddlerApplication.Log.LogFormat("request {0}: {1}", oSession.id, oSession.fullUrl);
                //FiddlerApplication.Log.LogString(oSession.ToString());
                oView.ReplaceRequest(oSession);
            }
            catch (Exception ex)
            {
                Utils.FiddlerLog(ex.ToString());
            }

        }

        public void AutoTamperResponseAfter(Session oSession)
        {
            //throw new NotImplementedException();
        }

        public void AutoTamperResponseBefore(Session oSession)
        {
            //throw new NotImplementedException();
        }

        public void OnBeforeReturningError(Session oSession)
        {
            //throw new NotImplementedException();
        }

        public void OnBeforeUnload()
        {
            //throw new NotImplementedException();
        }

        public void OnLoad()
        {
            //throw new NotImplementedException();
            var oPage = new TabPage("jmFidExt");
            
            oPage.ImageIndex = (int)Fiddler.SessionIcons.Silverlight;            
            
            oPage.Controls.Add(oView);
            oView.Dock = DockStyle.Fill;
            FiddlerApplication.UI.tabsViews.TabPages.Add(oPage); 
        }
    }
}
