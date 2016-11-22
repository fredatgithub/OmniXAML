﻿namespace OmniXaml.DefaultLoader
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using Ambient;
    using Tests;
    using Tests.Namespaces;

    public class XamlLoader : IXamlLoader
    {
        private readonly ObjectBuilderContext objectBuilderContext;
        private readonly ITypeDirectory directory;
        private readonly AttributeBasedMetadataProvider metadataProvider;

        public XamlLoader(IList<Assembly> assemblies)
        {
            directory = new AttributeBasedTypeDirectory(assemblies);
            metadataProvider = new AttributeBasedMetadataProvider();
            objectBuilderContext = new ObjectBuilderContext(new SourceValueConverter(), metadataProvider);
        }

        public ConstructionResult Load(string xaml)
        {
            var ct = Parse(xaml);
            return Construct(ct.Root);
        }

        public ConstructionResult Load(string xaml, object intance)
        {
            throw new NotImplementedException();
        }

        public static XamlLoader FromAttributes(params Assembly[] assemblies)
        {
            return new XamlLoader(assemblies);
        }

        private ConstructionResult Construct(ConstructionNode ctNode)
        {
            var namescopeAnnotator = new NamescopeAnnotator(metadataProvider);
            var trackingContext = new BuildContext(namescopeAnnotator, new AmbientRegistrator(), new InstanceLifecycleSignaler());
            var instanceCreator = new InstanceCreator(objectBuilderContext.SourceValueConverter, objectBuilderContext, directory);
            var objectConstructor = new ObjectBuilder(instanceCreator, objectBuilderContext, new ContextFactory(directory, objectBuilderContext));
            var construct = objectConstructor.Inflate(ctNode, trackingContext);
            return new ConstructionResult(construct, namescopeAnnotator);
        }

        private ParseResult Parse(string xaml)
        {
            var resolver = new Resolver(directory);
            var sut = new XamlToTreeParser(metadataProvider, new[] {new InlineParser(directory, resolver) }, resolver);

            var tree = sut.Parse(xaml, new PrefixAnnotator());
            return tree;
        }
    }
}