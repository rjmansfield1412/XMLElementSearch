using System;
using System.Xml;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Net;

public class TemplateProcessor
{
    /// <summary>
    /// Processes a template XML document and searches for specified text within element content only.
    /// Tags and attributes are ignored in the search.
    /// </summary>
    /// <param name="templateName">The name of the template</param>
    /// <param name="template">The XML document containing the template</param>
    /// <param name="searchText">The text to search for within the template elements</param>
    /// <returns>True if the search text is found within any element content, otherwise false</returns>
    public bool ProcessTemplate(string templateName, XmlDocument template, string searchText)
    {
        // Validate inputs
        if (string.IsNullOrEmpty(templateName))
        {
            throw new ArgumentNullException(nameof(templateName), "Template name cannot be null or empty");
        }

        if (template == null)
        {
            throw new ArgumentNullException(nameof(template), "Template XML document cannot be null");
        }

        if (string.IsNullOrEmpty(searchText))
        {
            throw new ArgumentNullException(nameof(searchText), "Search text cannot be null or empty");
        }

        Console.WriteLine($"Processing template: {templateName}");

        // Get all elements in the document
        XmlNodeList allElements = template.SelectNodes("//*");
        
        // Search through each element's text content
        foreach (XmlNode node in allElements)
        {
            // Check if the node has any text content
            if (!string.IsNullOrEmpty(node.InnerText))
            {
                // Check if the search text is found within the element's content
                if (node.InnerText.Contains(searchText))
                {
                    Console.WriteLine($"Search text '{searchText}' found in element <{node.Name}> in template {templateName}");
                    return true;
                }
            }
        }
        
        Console.WriteLine($"Search text '{searchText}' not found in any element in template {templateName}");
        return false;
    }

    /// <summary>
    /// Processes an HTML document and searches for specified text within element content only.
    /// Tags and attributes are ignored in the search.
    /// </summary>
    /// <param name="templateName">The name of the template</param>
    /// <param name="htmlContent">The HTML content as a string</param>
    /// <param name="searchText">The text to search for within the HTML elements</param>
    /// <returns>True if the search text is found within any element content, otherwise false</returns>
    public bool ProcessHtmlTemplate(string templateName, string htmlContent, string searchText)
    {
        // Validate inputs
        if (string.IsNullOrEmpty(templateName))
        {
            throw new ArgumentNullException(nameof(templateName), "Template name cannot be null or empty");
        }

        if (string.IsNullOrEmpty(htmlContent))
        {
            throw new ArgumentNullException(nameof(htmlContent), "HTML content cannot be null or empty");
        }

        if (string.IsNullOrEmpty(searchText))
        {
            throw new ArgumentNullException(nameof(searchText), "Search text cannot be null or empty");
        }

        Console.WriteLine($"Processing HTML template: {templateName}");

        try
        {
            // Convert HTML to XML document for processing
            XmlDocument xmlDoc = new XmlDocument();
            
            // Create an XML reader with HTML parsing capabilities
            using (StringReader sr = new StringReader(htmlContent))
            {
                // Use XmlReaderSettings to handle HTML-specific issues
                XmlReaderSettings settings = new XmlReaderSettings();
                settings.DtdProcessing = DtdProcessing.Parse;
                settings.ValidationType = ValidationType.None;
                settings.XmlResolver = null; // Don't resolve external entities
                settings.ConformanceLevel = ConformanceLevel.Fragment; // Allow HTML fragments
                
                // Try to load the HTML as XML
                try
                {
                    using (XmlReader reader = XmlReader.Create(sr, settings))
                    {
                        xmlDoc.Load(reader);
                    }
                }
                catch (XmlException)
                {
                    // If direct loading fails, try to clean the HTML first
                    string cleanedHtml = CleanHtmlForXml(htmlContent);
                    using (StringReader cleanSr = new StringReader(cleanedHtml))
                    using (XmlReader reader = XmlReader.Create(cleanSr, settings))
                    {
                        xmlDoc.Load(reader);
                    }
                }
            }
            
            // Now that we have the HTML as an XML document, use the same search logic
            XmlNodeList allElements = xmlDoc.SelectNodes("//*");
            
            // Search through each element's text content
            foreach (XmlNode node in allElements)
            {
                // Check if the node has any text content
                if (!string.IsNullOrEmpty(node.InnerText))
                {
                    // Check if the search text is found within the element's content
                    if (node.InnerText.Contains(searchText))
                    {
                        Console.WriteLine($"Search text '{searchText}' found in HTML element <{node.Name}> in template {templateName}");
                        return true;
                    }
                }
            }
            
            Console.WriteLine($"Search text '{searchText}' not found in any HTML element in template {templateName}");
            return false;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error processing HTML template: {ex.Message}");
            
            // Fallback to simple string search if XML parsing fails
            bool found = htmlContent.Contains(searchText);
            Console.WriteLine($"Fallback search: Text '{searchText}' {(found ? "found" : "not found")} in template {templateName}");
            
            // Note: This fallback doesn't distinguish between content and tags/attributes
            return found;
        }
    }
    
    /// <summary>
    /// Cleans HTML content to make it more XML-friendly
    /// </summary>
    private string CleanHtmlForXml(string html)
    {
        // Replace problematic HTML entities with XML-compatible versions
        html = html.Replace("&nbsp;", "&#160;")
                   .Replace("&copy;", "&#169;")
                   .Replace("&reg;", "&#174;")
                   .Replace("&trade;", "&#8482;")
                   .Replace("&mdash;", "&#8212;")
                   .Replace("&ndash;", "&#8211;")
                   .Replace("&ldquo;", "&#8220;")
                   .Replace("&rdquo;", "&#8221;")
                   .Replace("&lsquo;", "&#8216;")
                   .Replace("&rsquo;", "&#8217;");
        
        // Add a root element if one doesn't exist
        if (!html.TrimStart().StartsWith("<html", StringComparison.OrdinalIgnoreCase))
        {
            html = "<html>" + html + "</html>";
        }
        
        return html;
    }
}