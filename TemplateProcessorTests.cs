using System;
using System.Xml;
using Microsoft.VisualStudio.TestTools.UnitTesting;

[TestClass]
public class TemplateProcessorTests
{
    private TemplateProcessor _processor;
    
    [TestInitialize]
    public void Setup()
    {
        _processor = new TemplateProcessor();
    }
    
    [TestMethod]
    public void ProcessTemplate_SearchTextFoundInElement_ReturnsTrue()
    {
        // Arrange
        string templateName = "TestTemplate1";
        string searchText = "Hello World";
        
        // Create an XML document with the search text in one element
        XmlDocument template = new XmlDocument();
        template.LoadXml(@"
            <root>
                <header>Template Header</header>
                <content>This contains Hello World in the text</content>
                <footer>Template Footer</footer>
            </root>
        ");
        
        // Act
        bool result = _processor.ProcessTemplate(templateName, template, searchText);
        
        // Assert
        Assert.IsTrue(result, "The search text should be found in the XML document");
    }
    
    [TestMethod]
    public void ProcessTemplate_SearchTextNotFound_ReturnsFalse()
    {
        // Arrange
        string templateName = "TestTemplate2";
        string searchText = "Nonexistent Text";
        
        // Create an XML document without the search text
        XmlDocument template = new XmlDocument();
        template.LoadXml(@"
            <root>
                <header>Template Header</header>
                <content>This is some sample content</content>
                <items>
                    <item id='1'>First item</item>
                    <item id='2'>Second item</item>
                    <item id='3'>Third item</item>
                </items>
                <footer>Template Footer</footer>
            </root>
        ");
        
        // Act
        bool result = _processor.ProcessTemplate(templateName, template, searchText);
        
        // Assert
        Assert.IsFalse(result, "The search text should not be found in the XML document");
    }
    
    [TestMethod]
    public void ProcessTemplate_SearchTextInAttribute_ReturnsFalse()
    {
        // Arrange
        string templateName = "TestTemplate3";
        string searchText = "attribute-value";
        
        // Create an XML document with the search text only in an attribute (should be ignored)
        XmlDocument template = new XmlDocument();
        template.LoadXml(@"
            <root>
                <header>Template Header</header>
                <content special-attr=""attribute-value"">This is some sample content</content>
                <footer>Template Footer</footer>
            </root>
        ");
        
        // Act
        bool result = _processor.ProcessTemplate(templateName, template, searchText);
        
        // Assert
        Assert.IsFalse(result, "The search text in attributes should be ignored");
    }
    
    [TestMethod]
    public void ProcessTemplate_SearchTextInTagName_ReturnsFalse()
    {
        // Arrange
        string templateName = "TestTemplate4";
        string searchText = "searchtext";
        
        // Create an XML document with the search text only in a tag name (should be ignored)
        XmlDocument template = new XmlDocument();
        template.LoadXml(@"
            <root>
                <header>Template Header</header>
                <searchtext>This is some sample content</searchtext>
                <footer>Template Footer</footer>
            </root>
        ");
        
        // Act
        bool result = _processor.ProcessTemplate(templateName, template, searchText);
        
        // Assert
        Assert.IsFalse(result, "The search text in tag names should be ignored");
    }
}