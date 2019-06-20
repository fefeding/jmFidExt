# jmFidExt  - Fiddler 代理插件

本插件作用是把某些请求代理到指定的IP（端号）或文件，设置简便。

English: [https://github.com/jiamao/jmFidExt/blob/master/README_en.md](https://github.com/jiamao/jmFidExt/blob/master/README_en.md)

### 示图 ##

![截图](https://raw.githubusercontent.com/jiamao/jmFidExt/master/imgs/s.jpg)

### 安装 ##
如果有安装donet core则执直接编译
```
npm run build
```

下载代码，用开发工具vs2003及以上的版本编译出jmFidExt.dll，或直接下载：[jmFidExt.dll](https://raw.githubusercontent.com/jiamao/jmFidExt/master/bin/jmFidExt.dll)  
把DLL拷贝到Fiddler的Scripts，启动Fiddler即可。

### 配置 ###

启动Fiddler后，右侧会有一个jmFidExt的TAB，在表格中右健即可创建一个匹配规则，移动每行的header可以对规则进行排序。

`注：从上往下，命中一个规则后不再匹配后面的规则，所以请把优先级最高的放最前面，比如具体的某个文件或服务请求，而替换host的这种放最后。`

> 配置完后会在Scripts目录下生成一个 [jmFidExt.conf](https://raw.githubusercontent.com/jiamao/jmFidExt/master/bin/jmFidExt.conf) 的文件，可以备份此文件

如果要显示当前请求指向了哪个IP，可以在`FiddlerScript`中的`Main`函数下加上如下代码：
```javascript
FiddlerObject.UI.lvSessions.AddBoundColumn("ServerIP", 120, "X-HostIP");
```


#### 示例 ####

* **多个域名指向同一个ip**
---

**Match:** (regex:代表一个正则)
```
regex:http(s)?://(abc|bcd).(baidu|qq).com/(.*)
```
或者单独的host匹配:
```
xxx.qq.com
```

**Action:** (支持多种方式)

```
127.0.0.1
```

或带端口
```
127.0.0.1:8000
```
或其它域名
```
xxx.qq.com
```

### https请求转http
---
**Match:**
```
regex:https://qian(-img)?.(tenpay|qq|ehowbuy).com/(.*)
```
**Action:**
```
http://127.0.0.1
```
 > `action`为 `http://127.0.0.1`这种IP格式，当请求为`https`且命中其`match`时，将会把请求转为`http`，`http`转`https`反过来即可。

* **具体的某个请求指向文件或一个文本串**
---

**Match:** 

```
regex:http(s)?://xxx.qq.com/fcg/act.cgi(.*)
```
Match也可以配成一个具体请求
例如： http://xxx.qq.com/test.css

**Action:**
指向一个文件
```
E:\product\test\a.js
```
或直接配置一个json串
```
{
    "ret": 0,
    "msg": "success"
}
```

* **把请求指向一个目录**
---

可以用于模拟一个静态站点，会把所有匹配的请求指向一个目录。

下面的示例会把jmgraph.oa.com的请求全指向jmgraph目录

**Match:**
```
regex://jmgraph.oa.com/(.*)
```
**Action**
```
D:\javascript\jmgraph
```
或者把请求的某个子目录指向一个本地目录，这里利用filename参数来做为截取文件名。
下面示例只是把请求的test路径指向某个目录：

**Match:**
```
regex://jmgraph.oa.com/test/(?<filename>(.*))
```
**Action**
```
D:\javascript\jmgraph\test
```

#### 配置示例
```json
{
    "enabled": true,
    "groups": [
        {
            "enabled": false,
            "name": "默认",
            "rules": [
                {
                    "action": "127.0.0.1:8010",
                    "enabled": true,
                    "match": "regex:http(s)?://(.*)/fund_act_fcg/node/act.cgi(.*)",
                    "name": "理财通登录态同步本地"
                }
            ]
        },
        {
            "enabled": false,
            "name": "dev_cgi",
            "rules": [                
                {
                    "action": "127.0.0.1",
                    "enabled": true,
                    "match": "regex:http(s)?://(qian|apreqian).(xx|qq|yy).com/(app|fcgi|fund_act_fcg)/(.*).[f]?cgi(.*)",
                    "name": "dev主站"
                },                
                {
                    "action": "{\"ret\":0,\"msg\":\"\",\"rsp\":{ \"iDualRecordCase\": 1, \"iReason\": 0, \"iRet\": 0, \"sReason\": \"当前不在工作时间\" }}",
                    "enabled": false,
                    "match": "regex:http(s)?://(qian|apreqian).(tenpay|qq|ehowbuy).com/fund_act_fcg/action_acc_fcgi.fcgi(.*)",
                    "name": "对某个请求返回固定的json"
                },
                {
                    "action": "E:\\git\\lct_web\\src\\mb\\v4\\js\\page\\store/list.js",
                    "enabled": false,
                    "match": "http://qian-img.qq.com/mb/v4/js/page/store/list.02483ce3.min.js",
                    "name": "把请求指向文件"
                },
                {
                    "action": "127.0.0.1",
                    "enabled": false,
                    "match": "www.qq.com",
                    "name": "设置host"
                }
            ]
        },
        {
            "enabled": true,
            "name": "预发布CGI",
            "rules": [
                {
                    "action": "10.0.0.1",
                    "enabled": false,
                    "match": "regex:http(s)?://(qian|apreqian).(xx|qq|yy).com/app/v2.0/(aaa|bbb|ccc).[f]?cgi(.*)",
                    "name": "指定某些接口到开发机"
                },
                {
                    "action": "http://127.0.0.1",
                    "enabled": true,
                    "match": "regex:http(s)?://(qian|apreqian).(xx|qq|yy).com/fund_act_fcg/(.*).[f]?cgi(.*)",
                    "name": "某些接口https转为http请求"
                }
            ]
        },
        {
            "enabled": true,
            "name": "预发布UI",
            "rules": [               
                {
                    "action": "http://127.0.0.1",
                    "enabled": true,
                    "match": "regex:http(s)?://qian(-img)?.(xx|qq|yy).com/(.*)",
                    "name": "主站https转为http请求"
                },
                {
                    "action": "http://127.0.0.1",
                    "enabled": true,
                    "match": "www.jm47.com",
                    "name": "jm47转为http请求"
                }
            ]
        },
        {
            "enabled": true,
            "name": "测试",
            "rules": [
                {
                    "action": "D:\\javascript\\",
                    "enabled": false,
                    "match": "regex://js.jm.com/(?<filename>(.*))",
                    "name": "把请求指向本地静态文件"
                },
                {
                    "action": "http://qian.qq.com/mb/v4/js/page/vip/config.js",
                    "enabled": false,
                    "match": "https://qian.qq.com/mb/v4/js/page/vip/config.js",
                    "name": "https转向http请求js"
                },
                {
                    "action": "D:\\javascript\\JM.JS\\jmgraph",
                    "enabled": false,
                    "match": "regex://jmgraph.jm.com/(.*)",
                    "name": "jmgraph请求指向本地目录"
                },
                {
                    "action": "D:\\myproject\\nodeMediaServer",
                    "enabled": false,
                    "match": "nms.jm.com",
                    "name": "nms站点"
                },
                {
                    "action": "E:\\github\\",
                    "enabled": true,
                    "match": "regex://www.jm.com/(.*)",
                    "name": "jmSlip"
                },
                {
                    "action": "proxy.jm.com:8080",
                    "enabled": true,
                    "match": "regex://(.*)",
                    "name": "所有没命中的请求都走代理"
                },
                {
                    "action": "127.0.0.1:7001",
                    "enabled": false,
                    "match": "regex://fmp.jm.com/(.*)",
                    "name": "fmp"
                }
            ]
        }
    ]
}
```

### 最后 ###
「
从Fiddler请求列表中，背景色为<font bgcolor="#D6FAD6">#D6FAD6</font>则表示经过jmFidExt匹配中的请求。
」


