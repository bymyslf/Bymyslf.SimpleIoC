using System;
using System.Configuration;

namespace SimpleIoC.ConfigFile
{
    public sealed class RegisteredObjectElement : ConfigurationElement
    {
        [ConfigurationProperty("name", IsRequired = true)]
        public string Name
        {
            get
            {
                return (string)this["name"];
            }
            set
            {
                this["name"] = value;
            }
        }

        [ConfigurationProperty("typeToResolve", IsRequired = true)]
        public string TypeToResolve
        {
            get
            {
                return (string)this["typeToResolve"];
            }
            set
            {
                this["typeToResolve"] = value;
            }
        }

        [ConfigurationProperty("concreteType", IsRequired = true)]
        public string ConcreteType
        {
            get
            {
                return (string)this["concreteType"];
            }
            set
            {
                this["concreteType"] = value;
            }
        }

        [ConfigurationProperty("lifeCycle", DefaultValue = "Transient", IsRequired = false)]
        [RegexStringValidator(@"(?i)\b(Transient|Singleton)\b")]
        public string LifeCycle
        {
            get
            {
                return (string)this["lifeCycle"];
            }
            set
            {
                this["lifeCycle"] = value;
            }
        }
    }
}
