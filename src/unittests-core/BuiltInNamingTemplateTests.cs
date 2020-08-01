﻿using Xunit;
using aggregator.cli;

namespace unittests_core
{
    public class BuiltInNamingTemplateTests
    {
        [Fact]
        public void CustomResourceGroupName()
        {
            var templates = new BuiltInNamingTemplates();
            var names = templates.GetInstanceCreateNames("n", "rg");

            Assert.Equal("n", names.PlainName);
            Assert.Equal("naggregator", names.FunctionAppName);
            Assert.Equal("rg", names.ResourceGroupName);
            Assert.True(names.IsCustom);
        }

        [Fact]
        public void DefaultResourceGroupName()
        {
            var templates = new BuiltInNamingTemplates();
            var names = templates.GetInstanceCreateNames("n", null);

            Assert.Equal("n", names.PlainName);
            Assert.Equal("naggregator", names.FunctionAppName);
            Assert.Equal("aggregator-n", names.ResourceGroupName);
            Assert.False(names.IsCustom);
        }

        [Fact]
        public void FromResourceGroupName_CustomResourceGroupName()
        {
            var templates = new BuiltInNamingTemplates();

            var actual = templates.FromResourceGroupName("aggregator-n");

            Assert.Equal("n", actual.PlainName);
            Assert.Equal("aggregator-n", actual.ResourceGroupName);
        }

        [Fact]
        public void FromResourceGroupName_DefaultResourceGroupName()
        {
            var templates = new BuiltInNamingTemplates();

            var actual = templates.FromResourceGroupName("aggregator-n");

            Assert.Equal("n", actual.PlainName);
            Assert.Equal("aggregator-n", actual.ResourceGroupName);
        }
    }
}