using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jmFidExt
{
    [Serializable]
    [System.Runtime.Serialization.DataContract]
    public class Rule
    {
        [System.Runtime.Serialization.DataMember]
        public bool enabled { get; set; }

        [System.Runtime.Serialization.DataMember]
        public string match { get; set; }

        [System.Runtime.Serialization.DataMember]
        public string action { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [System.Runtime.Serialization.DataMember]
        public string name { get; set; }
    }

    /// <summary>
    /// 分组
    /// </summary>
    [Serializable]
    [System.Runtime.Serialization.DataContract]
    public class GroupRule
    {
        public GroupRule()
        {
            this.enabled = true;
            this.rules = new List<Rule>();
        }

        [System.Runtime.Serialization.DataMember]
        public bool enabled { get; set; }        

        /// <summary>
        /// 备注
        /// </summary>
        [System.Runtime.Serialization.DataMember]
        public string name { get; set; }

        [System.Runtime.Serialization.DataMember]
        public List<Rule> rules { get; set; }
    }

    /// <summary>
    /// 规则配置
    /// </summary>
    public class RuleConfig
    {
        public RuleConfig() 
        {
            groups = new List<GroupRule>();
        }

        [System.Runtime.Serialization.DataMember]
        public bool enabled { get; set; }

        [System.Runtime.Serialization.DataMember]
        public List<GroupRule> groups { get; set; }
    }
}
