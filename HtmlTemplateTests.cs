using System;
using System.Xml;
using Microsoft.VisualStudio.TestTools.UnitTesting;

[TestClass]
public class HtmlTemplateTests
{
    private TemplateProcessor _processor;
    
    [TestInitialize]
    public void Setup()
    {
        _processor = new TemplateProcessor();
    }
    
    [TestMethod]
    public void ProcessHtmlTemplate_SearchTextFoundInElement_ReturnsTrue()
    {
        // Arrange
        string templateName = "HtmlTemplate1";
        string searchText = "Hello World";
        
        // Create an HTML document with the search text in one element
        string htmlContent = @"
            <!DOCTYPE html>
            <html>
            <head>
                <title>Test HTML Template</title>
            </head>
            <body>
                <header>Template Header</header>
                <div class='content'>This contains Hello World in the text</div>
                <footer>Template Footer</footer>
            </body>
            </html>
        ";
        
        // Act
        bool result = _processor.ProcessHtmlTemplate(templateName, htmlContent, searchText);
        
        // Assert
        Assert.IsTrue(result, "The search text should be found in the HTML document");
    }
    
    [TestMethod]
    public void ProcessHtmlTemplate_SearchTextNotFound_ReturnsFalse()
    {
        // Arrange
        string templateName = "HtmlTemplate2";
        string searchText = "Nonexistent Text";
        
        // Create an HTML document without the search text
        string htmlContent = @"
            <!DOCTYPE html>
            <html>
            <head>
                <title>Test HTML Template</title>
            </head>
            <body>
                <header>Template Header</header>
                <div class='content'>This is some sample content</div>
                <ul>
                    <li data-id='1'>First item</li>
                    <li data-id='2'>Second item</li>
                    <li data-id='3'>Third item</li>
                </ul>
                <footer>Template Footer</footer>
            </body>
            </html>
        ";
        
        // Act
        bool result = _processor.ProcessHtmlTemplate(templateName, htmlContent, searchText);
        
        // Assert
        Assert.IsFalse(result, "The search text should not be found in the HTML document");
    }
    
    [TestMethod]
    public void ProcessHtmlTemplate_SearchTextInAttribute_ReturnsFalse()
    {
        // Arrange
        string templateName = "HtmlTemplate3";
        string searchText = "attribute-value";
        
        // Create an HTML document with the search text only in an attribute (should be ignored)
        string htmlContent = @"
            <!DOCTYPE html>
            <html>
            <head>
                <title>Test HTML Template</title>
            </head>
            <body>
                <header>Template Header</header>
                <div data-special=""attribute-value"">This is some sample content</div>
                <footer>Template Footer</footer>
            </body>
            </html>
        ";
        
        // Act
        bool result = _processor.ProcessHtmlTemplate(templateName, htmlContent, searchText);
        
        // Assert
        Assert.IsFalse(result, "The search text in attributes should be ignored");
    }
    
    [TestMethod]
    public void ProcessHtmlTemplate_SearchTextInTagName_ReturnsFalse()
    {
        // Arrange
        string templateName = "HtmlTemplate4";
        string searchText = "searchtext";
        
        // Create an HTML document with the search text only in a tag name (should be ignored)
        string htmlContent = @"
            <!DOCTYPE html>
            <html>
            <head>
                <title>Test HTML Template</title>
            </head>
            <body>
                <header>Template Header</header>
                <searchtext>This is some sample content</searchtext>
                <footer>Template Footer</footer>
            </body>
            </html>
        ";
        
        // Act
        bool result = _processor.ProcessHtmlTemplate(templateName, htmlContent, searchText);
        
        // Assert
        Assert.IsFalse(result, "The search text in tag names should be ignored");
    }
    
    [TestMethod]
    public void ProcessHtmlTemplate_MalformedHtml_StillProcesses()
    {
        // Arrange
        string templateName = "HtmlTemplate5";
        string searchText = "still works";
        
        // Create a malformed HTML document with unclosed tags and other issues
        string htmlContent = @"
            <html>
            <head>
                <title>Malformed HTML
            </head>
            <body>
                <div>This test still works with malformed HTML</div>
                <p>Another paragraph
                <ul>
                    <li>Item 1
                    <li>Item 2
                </body>
            </html>
        ";
        
        // Act
        bool result = _processor.ProcessHtmlTemplate(templateName, htmlContent, searchText);
        
        // Assert
        Assert.IsTrue(result, "The search text should be found even in malformed HTML");
    }
}