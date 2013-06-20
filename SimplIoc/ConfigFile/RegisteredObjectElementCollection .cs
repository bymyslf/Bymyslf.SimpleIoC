using System;
using System.Configuration;

namespace SimplIoc.ConfigFile
{
    [ConfigurationCollection(typeof(RegisteredObjectElement))]
    public sealed class RegisteredObjectElementCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new RegisteredObjectElement();
        }
        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((RegisteredObjectElement)element).Name;
        }
    }
}
