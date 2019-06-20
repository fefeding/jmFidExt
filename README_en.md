# jmFidExt - Fiddler proxy plugin

The function of this plugin is to delegate some requests to the specified IP (terminal number) or file, and the setting is simple.

中文: [https://github.com/jiamao/jmFidExt/blob/master/README.md](https://github.com/jiamao/jmFidExt/blob/master/README.md)

### Screenshot ##

![Screenshot](https://raw.githubusercontent.com/jiamao/jmFidExt/master/imgs/s.jpg)

### Start ##

Download the code, compile jmFidExt.dll with the development tool vs2003 and above, or download: [jmFidExt.dll](https://raw.githubusercontent.com/jiamao/jmFidExt/master/bin/jmFidExt.dll )
Copy the DLL to Fiddler's Scripts and start Fiddler.

### Configure ###

After starting Fiddler, there will be a TAB of jmFidExt on the right side. You can create a matching rule by right-clicking in the table. You can sort the rules by moving the header of each line.

`Note: From top to bottom, after hitting a rule, it will no longer match the following rules, so please put the highest priority first, such as a specific file or service request, and replace the host with this last. `

` After configuration, a jmFidExt.conf file will be generated in the Scripts directory, which can be backed up.

If you want to show which IP the current request points to, you can add the following code to the `Main` function in `FiddlerScript`:
```javascript
FiddlerObject.UI.lvSessions.AddBoundColumn("ServerIP", 120, "X-HostIP");
```


#### Example ####

* **Multiple domain names point to the same ip**
---

**Match:** (regex: represents a regular)
```
Regex:http(s)?://(abc|bcd).(baidu|qq).com/(.*)
```
Or a separate host match:
```
Xxx.qq.com
```

**Action:** (supports multiple ways)

```
127.0.0.1
```

Or with port
```
127.0.0.1:8000
```
Or other domain name
```
Xxx.qq.com
```

### httpsRequest to transfer http
---
**Match:**
```
Regex:https://qian(-img)?.(tenpay|qq|ehowbuy).com/(.*)
```
**Action:**
```
Http://127.0.0.1
```
> `action` is the IP format of `http://127.0.0.1` and the request string is `https`. The transfer will turn the request into `http`, `http` turn `https` and vice versa.

* **A specific request points to a file or a text string**
---

**Match:**

```
Regex:http(s)?://xxx.qq.com/fcg/act.cgi(.*)
```
Match can also be configured as a specific request
For example: http://xxx.qq.com/test.css

**Action:**
Point to a file
```
E:\product\test\a.js
```
Or configure a json string directly
```
{
    "ret": 0,
    "msg": "success"
}
```

* **Point the request to a directory**
---

Can be used to simulate a static site that will point all matching requests to a directory.

The following example will point the request of jmgraph.oa.com to the jmgraph directory.

**Match:**
```
Regex://jmgraph.oa.com/(.*)
```
**Action**
```
D:\javascript\jmgraph
```
Or point a subdirectory of the request to a local directory, here use the filename parameter as the intercept file name.
The following example simply points the requested test path to a directory:

**Match:**
```
Regex://jmgraph.oa.com/test/(?<filename>(.*))
```
**Action**
```
D:\javascript\jmgraph\test
```

### At last ###
"
From the Fiddler request list, the background color is <font bgcolor="#D6FAD6">#D6FAD6</font> to indicate the request in the jmFidExt match.
"