using System;
using System.Xml;

class TestRunner
{
    static void Main(string[] args)
    {
        Console.WriteLine("Running manual tests for TemplateProcessor");
        Console.WriteLine("----------------------------------------");
        
        // Create an instance of the processor
        var processor = new TemplateProcessor();
        
        // Test 1: Search text is present in an element
        Console.WriteLine("\nTest 1: Search text is present in an element");
        RunTest(processor, "Template1", CreateXmlWithSearchText(), "Hello World", true);
        
        // Test 2: Search text is not present in any element
        Console.WriteLine("\nTest 2: Search text is not present in any element");
        RunTest(processor, "Template2", CreateXmlWithoutSearchText(), "Nonexistent Text", false);
        
        // Test 3: Search text is only in an attribute (should be ignored)
        Console.WriteLine("\nTest 3: Search text is only in an attribute (should be ignored)");
        RunTest(processor, "Template3", CreateXmlWithSearchTextInAttribute(), "attribute-value", false);
        
        // Test 4: Search text is only in a tag name (should be ignored)
        Console.WriteLine("\nTest 4: Search text is only in a tag name (should be ignored)");
        RunTest(processor, "Template4", CreateXmlWithSearchTextInTagName(), "searchtext", false);
        
        Console.WriteLine("\nAll tests completed.");
    }
    
    static void RunTest(TemplateProcessor processor, string templateName, XmlDocument template, string searchText, bool expectedResult)
    {
        try
        {
            bool result = processor.ProcessTemplate(templateName, template, searchText);
            bool passed = result == expectedResult;
            
            Console.WriteLine($"Expected: {expectedResult}, Actual: {result}, Passed: {passed}");
            
            if (!passed)
            {
                Console.WriteLine("TEST FAILED!");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Test threw an exception: {ex.Message}");
            Console.WriteLine("TEST FAILED!");
        }
    }
    
    static XmlDocument CreateXmlWithSearchText()
    {
        XmlDocument doc = new XmlDocument();
        doc.LoadXml(@"
            <root>
                <header>Template Header</header>
                <content>This contains Hello World in the text</content>
                <footer>Template Footer</footer>
            </root>
        ");
        return doc;
    }
    
    static XmlDocument CreateXmlWithoutSearchText()
    {
        XmlDocument doc = new XmlDocument();
        doc.LoadXml(@"
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
        return doc;
    }
    
    static XmlDocument CreateXmlWithSearchTextInAttribute()
    {
        XmlDocument doc = new XmlDocument();
        doc.LoadXml(@"
            <root>
                <header>Template Header</header>
                <content special-attr=""attribute-value"">This is some sample content</content>
                <footer>Template Footer</footer>
            </root>
        ");
        return doc;
    }
    
    static XmlDocument CreateXmlWithSearchTextInTagName()
    {
        XmlDocument doc = new XmlDocument();
        doc.LoadXml(@"
            <root>
                <header>Template Header</header>
                <searchtext>This is some sample content</searchtext>
                <footer>Template Footer</footer>
            </root>
        ");
        return doc;
    }
}