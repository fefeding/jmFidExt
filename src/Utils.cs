using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jmFidExt
{
    static class Utils
    {
        //命中的请求背景颜色
        const string REQUICLOR = "#D6FAD6";

        /// <summary>
        /// 根配置重置session请求
        /// </summary>
        /// <param name="r"></param>
        /// <param name="session"></param>
        public static void ResetSession(Rule r, Fiddler.Session session, System.Text.RegularExpressions.Regex reg=null)
        {
            Utils.FiddlerLog("match success match:" + r.match + " action:" + r.action + " fullUrl:" + session.fullUrl);
            
            session["ui-backcolor"] = REQUICLOR;
            session["ui-bold"] = "true";
            
            var action = r.action.Trim();
            //如果是替换为ip，则直接则host
            System.Net.IPAddress ip;

            //var m = System.Text.RegularExpressions.Regex.Match(action, @"^\s*(?<ip>\d+(\.\d+)+)\s*:\s*(\d+)\s*");
            //如果是带端口的格式 127.0.0.1:8000
            if (System.Text.RegularExpressions.Regex.IsMatch(action, @"^\s*(?<ip>\d+(\.\d+)+)\s*:\s*(\d+)\s*$"))
            {
                session["x-overridehost"] = action; 
            }
            else if (System.Net.IPAddress.TryParse(action, out ip))
            {
                session["x-overridehost"] = action; 
            }
                //如果是一个文件路径
            else if (System.Text.RegularExpressions.Regex.IsMatch(action, @"^[A-Za-z]\:(\\|\/)[^\s]+"))
            {                
                if (System.IO.File.Exists(action))
                {
                    LoadFileToResponse(session, action);
                }
                    //如果是一个目录，则当静态服务器用，所有匹配的请求都指向此目录的文件
                else if (System.IO.Directory.Exists(action))
                {
                    var path = ""; //拼接文件地址
                    if (reg != null)
                    {
                        var m = reg.Match(session.fullUrl);
                        if (m != null && m.Groups != null)
                        {
                            try
                            {
                                path = m.Groups["filename"].Value;
                            }
                            catch (Exception ex)
                            {
                                Utils.FiddlerLog(ex.ToString());
                            }
                        }
                    }
                    //如果没有获得文件名，则直接把url域名后的全截取
                    if (string.IsNullOrWhiteSpace(path))
                    {
                        var index = session.fullUrl.IndexOf("/", 10);//取http://后的第一个/
                        if (index > 0) path = session.fullUrl.Substring(index).Trim('/');
                    }
                    
                    if (!string.IsNullOrWhiteSpace(path))
                    {
                        var index = path.IndexOf('?');
                        if (index < 0) index = path.IndexOf('#');
                        if (index > -1) path = path.Substring(0, index);//去除?后面的部分

                        path = System.IO.Path.Combine(action, path);
                    }
                    
                    LoadFileToResponse(session, path);                   
                }
                else
                {
                    Utils.FiddlerLog("文件:" + action + " 不存在");
                    ResetResponse(session, 404);
                }
            }
                //如果是一个URL，则重置请求
            else if (System.Text.RegularExpressions.Regex.IsMatch(action, @"^(http(s)?:)?//([\w-]+\.)+[\w-:]+(/[\w- ./?%&=]*)?$"))
            {
                //如果是严格的 http://xx.xx.com 。则表示把所有匹配的请求提向这个协议和域名
                var protoreg = new System.Text.RegularExpressions.Regex(@"^(?<proto>http(s)?)://(?<domain>([\w-]+\.)+[\w-:]+)$");
                var m = protoreg.Match(action);
                if(m.Success) {
                    var proto = m.Groups["proto"].Value;
                    var domain = m.Groups["domain"].Value;
                    
                    if(proto == "http") {                        
                        if(!session.isHTTPS && session.port == 443) {
                            if (session.HTTPMethodIs("CONNECT"))
                            {
                                session["x-replywithtunnel"] = "FakeTunnel";
                                return;
                            }
                        }
                    }
                    //else if(!session.isHTTPS) {
                    //    session.fullUrl = session.fullUrl.Replace("http://", proto + "://");
                    //}

                    session.oRequest.headers.UriScheme = proto;

                    Utils.FiddlerLog("change prototype to " + proto + " domain " + domain + " isHttps:" + session.isHTTPS + " isConnect:" + session.HTTPMethodIs("CONNECT"));

                    // IP或带端口模式，则投置为host
                    if (System.Text.RegularExpressions.Regex.IsMatch(domain, @"^\s*(?<ip>\d+(\.\d+)+)\s*:\s*(\d+)\s*$"))
                    {
                        Utils.FiddlerLog("set x-overridehost " + domain);
                        session["x-overridehost"] = domain; 
                    }
                    else if (System.Net.IPAddress.TryParse(domain, out ip))
                    {
                        Utils.FiddlerLog("set x-overridehost " + domain);
                        session["x-overridehost"] = domain; 
                    }
                    else {
                        session["x-overrideGateway"]  = domain;
                        Utils.FiddlerLog("set x-overrideGateway " + domain);
                    }
                }
                else {
                    //session["x-overrideGateway"] = action;
                    session.fullUrl = action;
                    Utils.FiddlerLog("set fullUrl " + action);
                }
            }
            //一个域名
            else if(System.Text.RegularExpressions.Regex.IsMatch(action, @"^[\w-]+(\.[\w-]+)+(:\s*(\d+)\s*)?$")) {
                session["x-overrideGateway"] = action;
                Utils.FiddlerLog("set x-overrideGateway " + action);
            }
                //否则直接当结果body返回
            else {
                ResetResponse(session);
                session.utilSetResponseBody(action);
            }
        }

        /// <summary>
        /// 重置请求response
        /// </summary>
        /// <param name="session"></param>
        /// <param name="status"></param>
        /// <param name="contentType"></param>
        public static void ResetResponse(Fiddler.Session session, int status = 200, string contentType = "text/html; charset=utf-8")
        {
            //伪造response
            session.utilCreateResponseAndBypassServer();
            session.oResponse.headers.SetStatus(status, "By jmFidExt");

            session.oResponse["Content-Type"] = contentType;

            session.oResponse["Date"] = DateTime.Now.ToUniversalTime().ToString("r");
        }

        /// <summary>
        /// 用文件替代response
        /// </summary>
        /// <param name="session"></param>
        /// <param name="path"></param>
        public static void LoadFileToResponse(Fiddler.Session session, string path)
        {
            var ct = "text/html; charset=utf-8";
            var ext = System.IO.Path.GetExtension(path);
            //FiddlerLog("file ext:" + ext);
            if (".gif,.png,.jpg,.bmp,.jpeg,.webp".IndexOf(ext, StringComparison.OrdinalIgnoreCase) > -1)
            {
                ct = "image/" + ext.TrimStart('.');
            }

            if (System.IO.File.Exists(path))
            { 
                ResetResponse(session, 200, ct);
                //var content = System.IO.File.ReadAllText(path, System.Text.Encoding.UTF8);
                //session.utilSetResponseBody(content);
                
                session.oResponse.headers.Add("jmFidExt_action", path);

                session.LoadResponseFromFile(path);
            }
            else
            {
                ResetResponse(session, 404, ct);
                session.utilSetResponseBody("文件：" + path + " 未找到");
            }
            //session.Ignore();
        }

        /// <summary>
        /// 输出日志
        /// </summary>
        /// <param name="msg"></param>
        public static void FiddlerLog(string msg)
        {
            Fiddler.FiddlerApplication.Log.LogString("jmFidExt：" + msg);
        }

        public static void FiddlerLog<T>(T msg)
        {
            var str = Utils.ModelToJson(msg);
            Fiddler.FiddlerApplication.Log.LogString("jmFidExt：" + str);
        }

        /// <summary>
        /// 把对象序列化成JSON字符
        /// </summary>
        /// <param name="obj">待序列化的对象</param>
        /// <returns></returns>
        public static string ModelToJson(object obj, params Type[] knowTypes)
        {
            if (obj == null) return "null";
            var ser = new System.Runtime.Serialization.Json.DataContractJsonSerializer(obj.GetType(), knowTypes);
            using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
            {
                ser.WriteObject(ms, obj);
                string js = System.Text.Encoding.UTF8.GetString(ms.ToArray());
                return js;
            }
        }

        /// <summary>
        /// 把JSON反序列化对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="js"></param>
        /// <returns></returns>
        public static T JsonToModel<T>(string js)
        {
            var obj = JsonToModel(typeof(T), js);
            return (T)obj;
        }

        /// <summary>
        /// 把JSON反序列化对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="js"></param>
        /// <returns></returns>
        public static object JsonToModel(Type t, string js)
        {
            if (js == "null") return null;
            using (System.IO.MemoryStream ms = new System.IO.MemoryStream(System.Text.Encoding.UTF8.GetBytes(js)))
            {
                var ser = new System.Runtime.Serialization.Json.DataContractJsonSerializer(t);
                var obj = ser.ReadObject(ms);
                return obj;
            }
        }
    }
}
