namespace OmniXaml.Builder
{
    using System.Reflection;

    public class AssemblyConfiguration
    {
        public ClrNamespaceConfiguration ClrNamespaceConfiguration { get; }
        public Assembly Assembly { get; }

        public AssemblyConfiguration(ClrNamespaceConfiguration clrNamespaceClrNamespaceConfiguration, Assembly assembly)
        {
            ClrNamespaceConfiguration = clrNamespaceClrNamespaceConfiguration;
            Assembly = assembly;
        }

        public XamlNamespace To(string xamlNs)
        {
            return new XamlNamespace(this, xamlNs);
        }
    }
}