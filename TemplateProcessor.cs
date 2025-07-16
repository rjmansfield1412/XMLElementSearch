using System;
using System.Xml;
using System.Collections.Generic;

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
}