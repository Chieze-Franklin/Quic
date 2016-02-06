using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using Quic.Messaging;
using Quic.Utils;

namespace Quic
{
    /// <summary>
    /// Represents a Quic document
    /// </summary>
    public sealed class QuicDocument
    {
        //XmlDocument internalXmlDoc;
        XDocument internalXDoc;
        Dictionary<string, Element> namedElements = new Dictionary<string, Element>();
        Dictionary<string, IResource> resDic = new Dictionary<string, IResource>();
        List<Element> headElements = new List<Element>();
        List<Element> bodyElements = new List<Element>();
        Dictionary<string, Assembly> cachedAsm = new Dictionary<string, Assembly>();

        private QuicDocument() 
        {
            this.OutputOptions = new Quic.OutputOptions();
            this.OutputOptions.AllowUnknownAttributes = true;
            this.OutputOptions.AllowUnknownTags = true;
        }

        //_________Load_________
        /// <summary>
        /// Parses an XML file to a QuicDocument object.
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static QuicDocument Load(string filePath) 
        {
            try
            {
                XDocument xDoc = XDocument.Load(filePath, LoadOptions.SetLineInfo);

                QuicDocument doc = new QuicDocument();
                doc.SourcePath = filePath;
                doc.internalXDoc = xDoc;
                Environment.CurrentDirectory = new FileInfo(doc.SourcePath).Directory.FullName;

                //add default property provider
                PropertyVP pp = new PropertyVP();
                //pp.Key = "$";
                pp.Document = doc;
                doc.resDic.Add("$", pp);

                XElement rootTag = xDoc.Root;
                if(rootTag == null || rootTag.Name.LocalName != "html")
                    throw new QuicException("Document does not contain a root <Quic> tag.", filePath);

                XAttribute attri = rootTag.Attribute("AllowUnknownAttributes");
                if (attri != null)
                {
                    string value = attri.Value;
                    doc.OutputOptions.AllowUnknownAttributes = (bool)BoolVP.Singleton().Evaluate(value);
                }
                attri = rootTag.Attribute("AllowUnknownTags");
                if (attri != null)
                {
                    string value = attri.Value;
                    doc.OutputOptions.AllowUnknownTags = (bool)BoolVP.Singleton().Evaluate(value);
                }
                attri = rootTag.Attribute("IgnoreAttributeCase");
                if (attri != null)
                {
                    string value = attri.Value;
                    doc.OutputOptions.IgnoreAttributeCase = (bool)BoolVP.Singleton().Evaluate(value);
                }
                attri = rootTag.Attribute("IgnoreTagCase");
                if (attri != null)
                {
                    string value = attri.Value;
                    doc.OutputOptions.IgnoreTagCase = (bool)BoolVP.Singleton().Evaluate(value);
                }

                //parse head
                XElement headTag = null;
                var defNamespace = rootTag.GetDefaultNamespace();
                string defNamespaceUri = defNamespace != null ? "{" + defNamespace.NamespaceName + "}" : "";
                List<XElement> allHeadTags = rootTag.Elements(defNamespaceUri + "head").ToList();
                if (allHeadTags.Count > 1)
                {
                    var lineInfo = (IXmlLineInfo)allHeadTags[1]; //point to the second <head> tag
                    throw new QuicException("Document cannot have more than one <head> tag.", filePath, 
                        lineInfo.LineNumber, lineInfo.LinePosition);
                }
                else if (allHeadTags.Count == 1)
                {
                    headTag = allHeadTags[0];
                }
                if (headTag != null)
                {
                    foreach (XElement resTag in headTag.Elements())
                    {
                        var headElement = /*(ResourceElement)*/doc.BuildElement(resTag);
                        if (headElement is IResource)
                            if (!string.IsNullOrWhiteSpace(((IResource)headElement).Key))
                                doc.resDic.Add(((IResource)headElement).Key, (IResource)headElement);
                        doc.headElements.Add(headElement);
                    }
                }

                //parse body
                XElement bodyTag = null;
                List<XElement> allBodyTags = rootTag.Elements(defNamespaceUri + "body").ToList();
                if (allBodyTags.Count > 1)
                {
                    var lineInfo = (IXmlLineInfo)allBodyTags[1]; //point to the second <body> tag
                    throw new QuicException("Document cannot have more than one <body> tag.",
                                filePath, lineInfo.LineNumber, lineInfo.LinePosition);
                }
                else if (allBodyTags.Count == 1)
                {
                    bodyTag = allBodyTags[0];
                }
                if (bodyTag != null)
                {
                    foreach (XNode tag in bodyTag.Nodes())
                    {
                        var bodyElement = /*(UIElement)*/doc.BuildElement(tag);
                        doc.bodyElements.Add(bodyElement);
                    }
                }

                return doc;
            }
            catch (QuicException)
            {
                throw;
            }
            catch (XmlException ex)
            {
                throw new QuicException(ex.Message, filePath, ex.LineNumber, ex.LinePosition, ex);
            }
            catch (Exception ex)
            {
                throw new QuicException(ex.Message, filePath, ex);
            }
        }
        ///// <summary>
        ///// Parses an XML file to a QuicDocument object.
        ///// </summary>
        ///// <param name="filePath"></param>
        ///// <returns></returns>
        //public static QuicDocument ParseFile/*change to Load*/(string filePath)
        //{
        //    try
        //    {
        //        XmlDocument xmlDoc = new XmlDocument();
        //        xmlDoc.Load(filePath); 
                
        //        QuicDocument doc = new QuicDocument();
        //        doc.SourcePath = filePath;
        //        doc.internalXmlDoc = xmlDoc;
        //        Environment.CurrentDirectory = new FileInfo(doc.SourcePath).Directory.FullName;

        //        //add default property provider
        //        PropertyVP pp = new PropertyVP();
        //        //pp.Key = "$";
        //        pp.Document = doc;
        //        doc.resDic.Add("$", pp);
        //        doc.resList.Add(pp);

        //        XmlElement quicTag = null;
        //        XmlNodeList allQuicTags = doc.internalXmlDoc.GetElementsByTagName("Quic");
        //        if (allQuicTags.Count > 1)
        //        {
        //            throw new QuicException("Document cannot have more than one <Quic> tag.", doc.SourcePath);
        //        }
        //        else if (allQuicTags.Count == 1)
        //        {
        //            quicTag = (XmlElement)allQuicTags[0];
        //        }
        //        if (quicTag != null) 
        //        {
        //            //UnknownAttri...
        //            if (quicTag.HasAttribute("AllowUnknownAttributes"))
        //            {
        //                string allowAttri = quicTag.GetAttribute("AllowUnknownAttributes");
        //                doc.OutputOptions.AllowUnknownAttributes = (bool)BoolVP.Singleton().Evaluate(allowAttri);
        //            }

        //            //UnknownTag
        //            if (quicTag.HasAttribute("AllowUnknownTags"))
        //            {
        //                string allowTag = quicTag.GetAttribute("AllowUnknownTags");
        //                doc.OutputOptions.AllowUnknownTags = (bool)BoolVP.Singleton().Evaluate(allowTag);
        //            }

        //            //UnknownAttri...
        //            if (quicTag.HasAttribute("IgnoreAttributeCase"))
        //            {
        //                string ignoreAttri = quicTag.GetAttribute("IgnoreAttributeCase");
        //                doc.OutputOptions.IgnoreAttributeCase = (bool)BoolVP.Singleton().Evaluate(ignoreAttri);
        //            }

        //            //UnknownTag
        //            if (quicTag.HasAttribute("IgnoreTagCase"))
        //            {
        //                string ignoreTag = quicTag.GetAttribute("IgnoreTagCase");
        //                doc.OutputOptions.IgnoreTagCase = (bool)BoolVP.Singleton().Evaluate(ignoreTag);
        //            }
        //        }

        //        //parse resources
        //        XmlElement rsrcsTag = null;
        //        XmlNodeList allRscrsTags = xmlDoc.GetElementsByTagName("Resources");
        //        if (allRscrsTags.Count > 1)
        //        {
        //            throw new QuicException("Document cannot have more than one <Resources> tag.", filePath);
        //        }
        //        else if (allRscrsTags.Count == 1)
        //        {
        //            rsrcsTag = (XmlElement)allRscrsTags[0];
        //        }
        //        if (rsrcsTag != null)
        //        {
        //            foreach (XmlNode resTag in rsrcsTag.ChildNodes)
        //            {
        //                var resElement = (ResourceElement)doc.BuildElement(resTag);
        //                if (!string.IsNullOrWhiteSpace(resElement.Key))
        //                    doc.resDic.Add(resElement.Key, resElement);
        //                doc.resList.Add(resElement);
        //            }
        //        }

        //        //parse ui
        //        XmlElement uiTag = null;
        //        XmlNodeList allUiTags = xmlDoc.GetElementsByTagName("UI");
        //        if (allUiTags.Count > 1)
        //        {
        //            throw new QuicException("Document cannot have more than one <UI> tag.", filePath);
        //        }
        //        else if (allUiTags.Count == 1)
        //        {
        //            uiTag = (XmlElement)allUiTags[0];
        //        }
        //        if (uiTag != null)
        //        {
        //            foreach (XmlNode tag in uiTag.ChildNodes)
        //            {
        //                var uiElement = (UIElement)doc.BuildElement(tag);
        //                doc.uiElements.Add(uiElement);
        //            }
        //        }

        //        return doc;
        //    }
        //    catch (QuicException) 
        //    {
        //        throw;
        //    }
        //    catch (Exception ex) 
        //    {
        //        throw new QuicException(ex.Message, filePath, ex);
        //    }
        //}

        /// <summary>
        /// Builds an instance of Quic.IQuicElement from an instance of System.Xml.XmlNode.
        /// If the Quic.IQuicElement object has a name, it is added to dictionary of named objects.
        /// The Quic.IQuicElement object is NOT added to either the resource dictionary or the UI tree of the document.
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        Element BuildElement(XNode node) 
        {
            var lineInfo = (IXmlLineInfo)node;

            if (node is XText)
            {
                Text txt = new Text();
                txt.Document = this;
                txt.Line = lineInfo.LineNumber;
                txt.Column = lineInfo.LinePosition;
                txt.SetProperty("Content", ((XText)node).Value);
                return txt;
            }
            else if (node is XProcessingInstruction)
            {
                Text txt = new Text();
                txt.Document = this;
                txt.Line = lineInfo.LineNumber;
                txt.Column = lineInfo.LinePosition;
                var tgt = ((XProcessingInstruction)node).Target;
                var data = ((XProcessingInstruction)node).Data;
                txt.SetProperty("Content", string.Format("<?{0}\n{1} ?>", tgt, data));
                return txt;
            }
            else if (node is XComment)
            {
                Comment comm = new Comment();
                comm.Document = this;
                comm.Line = lineInfo.LineNumber;
                comm.Column = lineInfo.LinePosition;
                string comment = ((XComment)node).Value.Trim();
                if (comment.ToLower().StartsWith("cdata:"))
                {
                    comment = comment.Substring(6);
                    comm.IsCData = true;
                }
                else if (comment.ToLower().StartsWith("out:"))
                {
                    comment = comment.Substring(4);
                    comm.ToBeOutput = true;
                }
                //comm.SetProperty("Content", comment); //dont call this method so that the comment is not "formatted"
                comm.Content = comment;
                return comm;
            }
            else if (node.NodeType == XmlNodeType.None || node.NodeType == XmlNodeType.Attribute ||
                //node.NodeType == XmlNodeType.ProcessingInstruction || 
                node.NodeType == XmlNodeType.Entity ||
                node.NodeType == XmlNodeType.Document || node.NodeType == XmlNodeType.DocumentFragment ||
                node.NodeType == XmlNodeType.DocumentType || node.NodeType == XmlNodeType.Notation ||
                node.NodeType == XmlNodeType.Whitespace || node.NodeType == XmlNodeType.EndElement ||
                node.NodeType == XmlNodeType.EndEntity || node.NodeType == XmlNodeType.XmlDeclaration
                || node.NodeType == XmlNodeType.SignificantWhitespace)
            {
                Nothing nothing = new Nothing();
                nothing.Document = this;
                nothing.Line = lineInfo.LineNumber;
                nothing.Column = lineInfo.LinePosition;
                return nothing;
            }

            if (!(node is XElement))
                throw new QuicException(string.Format("Unrecognized XML node '{0}'", node), 
                    this.SourcePath, lineInfo.LineNumber, lineInfo.LinePosition);
            XElement elementNode = (XElement)node;
            Element element = null;
            try
            {
                element = (Element)BuildObject(elementNode.Name.LocalName, elementNode.GetPrefixOfNamespace(elementNode.Name.Namespace));
                element.Document = this; //just in case I delete it from BuildObject(...) by mistake
            }
            catch (TypeLoadException ex)
            {
                if (this.OutputOptions.AllowUnknownTags)
                {
                    element = new UnknownElement();
                    element.Document = this;
                    var prefix = elementNode.GetPrefixOfNamespace(elementNode.Name.Namespace);
                    ((UnknownElement)element).Tag = (string.IsNullOrWhiteSpace(prefix) ? "" : prefix + ":") + elementNode.Name.LocalName;
                    ((UnknownElement)element).IsEmptyTag = elementNode.IsEmpty;
                }
                else
                {
                    throw new QuicException(ex.Message, this.SourcePath, lineInfo.LineNumber, lineInfo.LinePosition, ex);
                }
            }
            catch (QuicException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new QuicException(ex.Message, this.SourcePath, lineInfo.LineNumber, lineInfo.LinePosition, ex);
            }
            element.Line = lineInfo.LineNumber;
            element.Column = lineInfo.LinePosition;

            //set its properties
            foreach (XAttribute attri in elementNode.Attributes())
            {
                //try
                //{
                if (!attri.IsNamespaceDeclaration)
                    element.SetProperty(attri.Name.LocalName, attri.Value);
                //}
                //catch (Exception ex)
                //{
                //    var attriLineInfo = (IXmlLineInfo)attri;
                //    throw new QuicException(ex.Message, this.SourcePath, attriLineInfo.LineNumber, attriLineInfo.LinePosition, ex);
                //}
            }
            //record the element if it has a name
            if (element.Name != null)
            {
                if (namedElements.ContainsKey(element.Name))
                    throw new QuicException(string.Format("An element with the name '{0}' already exists.", element.Name),
                        this.SourcePath, element.Line, element.Column);
                namedElements.Add(element.Name, element);
            }
            //child tags
            if ((element).IsContainer)
            {
                foreach (XNode child in elementNode.Nodes())
                {
                    var uiElement = BuildElement(child);
                    (element).Elements.Add(uiElement);
                }
            }
            else
            {
                var reader = node.CreateReader();
                reader.MoveToContent();
                string innerXml = reader.ReadInnerXml();
                //((UIElement)element).Content = innerXml;
                element.SetProperty("Content", innerXml);
            }

            return element;
        }
        ///// <summary>
        ///// Builds an instance of Quic.IQuicElement from an instance of System.Xml.XmlNode.
        ///// If the Quic.IQuicElement object has a name, it is added to dictionary of named objects.
        ///// The Quic.IQuicElement object is NOT added to either the resource dictionary or the UI tree of the document.
        ///// </summary>
        ///// <param name="node"></param>
        ///// <returns></returns>
        //public Element BuildElement(XmlNode node)
        //{
        //    if (node.NodeType == XmlNodeType.None || node.NodeType == XmlNodeType.Attribute || 
        //        node.NodeType == XmlNodeType.ProcessingInstruction || node.NodeType == XmlNodeType.Entity ||
        //        node.NodeType == XmlNodeType.Document || node.NodeType == XmlNodeType.DocumentFragment ||
        //        node.NodeType == XmlNodeType.DocumentType || node.NodeType == XmlNodeType.Notation ||
        //        node.NodeType == XmlNodeType.Whitespace || node.NodeType == XmlNodeType.EndElement ||
        //        node.NodeType == XmlNodeType.EndEntity || node.NodeType == XmlNodeType.XmlDeclaration)
        //    {
        //        Nothing nothing = new Nothing();
        //        nothing.Document = this;
        //        return nothing;
        //    }

        //    if (node.NodeType == XmlNodeType.Text || node.NodeType == XmlNodeType.CDATA || node.NodeType == XmlNodeType.SignificantWhitespace)
        //    {
        //        Text txt = new Text();
        //        txt.Document = this;
        //        txt.SetProperty("Content", node.InnerText);
        //        return txt;
        //    }

        //    if (node.NodeType == XmlNodeType.Comment)
        //    {
        //        Comment comm = new Comment();
        //        comm.Document = this;
        //        string comment = node.InnerText.Trim();
        //        if (comment.ToLower().StartsWith("out:"))
        //        {
        //            comment = comment.Substring(4);
        //            comm.ToBeOutput = true;
        //        }
        //        //comm.SetProperty("Content", comment); //dont call this method so that the comment is not "formatted"
        //        comm.Content = comment;
        //        return comm;
        //    }

        //    Element element = null;
        //    try 
        //    {
        //        element = (Element)BuildObject(node.LocalName, node.Prefix);
        //        element.Document = this; //just in case I delete it from BuildObject(...) by mistake
        //    }
        //    catch (TypeLoadException) 
        //    {
        //        if (this.OutputOptions.AllowUnknownTags)
        //        {
        //            element = new UnknownElement();
        //            element.Document = this;
        //            ((UnknownElement)element).Tag = node.Name;
        //            if (node is XmlElement) 
        //            {
        //                ((UnknownElement)element).IsEmptyTag = ((XmlElement)node).IsEmpty;
        //            }
        //        }
        //        else
        //            throw;
        //    }

        //    //set its properties
        //    foreach (XmlAttribute attri in node.Attributes)
        //    {
        //        element.SetProperty(attri.LocalName, attri.Value);
        //    }
        //    //record the element if it has a name
        //    if (element.Name != null) 
        //    {
        //        if(namedElements.ContainsKey(element.Name))
        //            throw new QuicException(string.Format("An element with the name '{0}' already exists.", element.Name), 
        //                this.SourcePath);
        //        namedElements.Add(element.Name, element);
        //    }
        //    //child tags
        //    if (element is UIElement)
        //    {
        //        if (((UIElement)element).IsContainer)
        //        {
        //            foreach (XmlNode tag in node.ChildNodes)
        //            {
        //                var uiElement = (UIElement)BuildElement(tag);
        //                ((UIElement)element).UIElements.Add(uiElement);
        //            }
        //        }
        //        else
        //        {
        //            //((UIElement)element).Content = node.InnerXml;
        //            element.SetProperty("Content", node.InnerXml);
        //        }
        //    }
        //    else
        //    {
        //        //((ResourceElement)element).Content = node.InnerXml;
        //        element.SetProperty("Content", node.InnerXml);
        //    }

        //    return element;
        //}

        /// <summary>
        /// Builds an object from the specified compound name.
        /// A compound name is always in the format "{prefix}:{name}"
        /// but could also be simply "{name}"
        /// </summary>
        /// <param name="compoundName"></param>
        /// <returns></returns>
        public object BuildObject(string compoundName) 
        {
            string name = null, prefix = null;
            if (compoundName.Contains(':'))
            {
                string[] nameParts = compoundName.Split(':');
                if (nameParts.Length != 2)
                    throw new Exception(string.Format("Badly formed compound name '{0}'", compoundName));
                prefix = nameParts[0].Trim();
                name = nameParts[1].Trim();
            }
            else
            {
                name = compoundName.Trim();
            }

            return BuildObject(name, prefix);
        }
        /// <summary>
        /// Builds an object from the specified name and prefix.
        /// The prefix can be null.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="prefix"></param>
        /// <returns></returns>
        public object BuildObject(string name, string prefix)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException("Object name cannot be null or empty.");

            //see if the name is actually a prefix
            bool nameIsPrefix = true;
            var nmspace = this.internalXDoc.Root.GetNamespaceOfPrefix(name);
            string namespaceUrl = nmspace == null ? null : nmspace.NamespaceName;

            if (string.IsNullOrWhiteSpace(namespaceUrl)) //if true, then name is NOT a prefix
            {
                nameIsPrefix = false;

                if (string.IsNullOrWhiteSpace(prefix))
                {
                    nmspace = this.internalXDoc.Root.GetDefaultNamespace();
                    namespaceUrl = nmspace == null ? null : nmspace.NamespaceName;
                }
                else
                {
                    nmspace = this.internalXDoc.Root.GetNamespaceOfPrefix(prefix);
                    namespaceUrl = nmspace == null ? null : nmspace.NamespaceName;
                }
                
                if (string.IsNullOrWhiteSpace(namespaceUrl))
                    throw new Exception(string.Format("No assembly specified for object '{0}{1}'",
                        (string.IsNullOrWhiteSpace(prefix) ? "" : prefix + ":"), name));
            }

            string assemblyPath = null, className = null;
            if (namespaceUrl.Contains(':'))
            {
                string[] namespaceUrlParts = namespaceUrl.Split(':');
                if (namespaceUrlParts.Length != 2)
                    throw new Exception(string.Format("Badly formed namespace url '{0}'", namespaceUrl));
                assemblyPath = namespaceUrlParts[0].Trim();
                className = namespaceUrlParts[1].Trim();
            }
            else
            {
                assemblyPath = namespaceUrl.Trim();
                className = "";
            }
            if (assemblyPath == string.Empty)
                throw new Exception(string.Format("No assembly path was specified for the object '{0}'", name));
            assemblyPath = FileSystemServices.GetAbsolutePath(assemblyPath);
            if (nameIsPrefix == false)
                className = (className == string.Empty ? className : className + ".") + name;
            if (className == string.Empty)
                throw new Exception(string.Format("No class name was specified for the object '{0}'", name));

            Assembly assembly = GetCachedOrNewAssembly(assemblyPath);

            Type elementType = assembly.GetType(className, true, this.OutputOptions.IgnoreTagCase);
            object obj = Activator.CreateInstance(elementType);

            //set its document (MUST be done b4 setting its properties)
            if (obj is Element)
                ((Element)obj).Document = this;

            return obj;
        }
        ///// <summary>
        ///// Builds an object from the specified name and prefix.
        ///// The prefix can be null.
        ///// </summary>
        ///// <param name="name"></param>
        ///// <param name="prefix"></param>
        ///// <returns></returns>
        //public object BuildObject(string name, string prefix)
        //{
        //    if (string.IsNullOrWhiteSpace(name))
        //        throw new QuicException("Object name cannot be null or empty.", this.SourcePath);

        //    //see if the name is actually a prefix
        //    bool nameIsPrefix = true;
        //    string namespaceUrl = this.internalXmlDoc.LastChild.GetNamespaceOfPrefix(name);

        //    if (string.IsNullOrWhiteSpace(namespaceUrl)) //if true, then name is NOT a prefix
        //    {
        //        nameIsPrefix = false;

        //        if (string.IsNullOrWhiteSpace(prefix))
        //            namespaceUrl = this.internalXmlDoc.LastChild.NamespaceURI;
        //        else
        //            namespaceUrl = this.internalXmlDoc.LastChild.GetNamespaceOfPrefix(prefix);

        //        if (string.IsNullOrWhiteSpace(namespaceUrl))
        //            throw new QuicException(string.Format("Cannot find assembly for object '{0}{1}'",
        //                (string.IsNullOrWhiteSpace(prefix) ? "" : prefix + ":"), name), this.SourcePath); 
        //    }

        //    string assemblyPath = null, className = null;
        //    if (namespaceUrl.Contains(':'))
        //    {
        //        string[] namespaceUrlParts = namespaceUrl.Split(':');
        //        if (namespaceUrlParts.Length != 2)
        //            throw new QuicException(string.Format("Badly formed namespace url '{0}'", namespaceUrl), this.SourcePath);
        //        assemblyPath = namespaceUrlParts[0].Trim();
        //        className = namespaceUrlParts[1].Trim();
        //    }
        //    else
        //    {
        //        assemblyPath = namespaceUrl.Trim();
        //        className = "";
        //    }
        //    if (assemblyPath == string.Empty)
        //        throw new QuicException(string.Format("No assembly path was specified for the object '{0}'", name), this.SourcePath);
        //    assemblyPath = FileSystemServices.GetAbsolutePath(assemblyPath);
        //    if (nameIsPrefix == false)
        //        className = (className == string.Empty ? className : className + ".") + name;
        //    if (className == string.Empty)
        //        throw new QuicException(string.Format("No class name was specified for the object '{0}'", name), this.SourcePath);

        //    Assembly assembly = GetCachedOrNewAssembly(assemblyPath);

        //    Type elementType = assembly.GetType(className, true, this.OutputOptions.IgnoreTagCase);
        //    object obj = Activator.CreateInstance(elementType);

        //    //set its document (MUST be done b4 setting its properties)
        //    if (obj is Element)
        //        ((Element)obj).Document = this;

        //    return obj;
        //}

        /// <summary>
        /// Generates the output files.
        /// </summary>
        /// <param name="outputDir"></param>
        public void Render(string outputDir) 
        {
            try 
            {
                //get the root output dir (we dont expose this dir)
                DirectoryInfo dirInfo = new DirectoryInfo(outputDir);
                if (!dirInfo.Parent.Exists)  //should have a parent that exists
                    throw new DirectoryNotFoundException(string.Format("Cannot find parent directory of output directory:\n'{0}'", outputDir));
                OutputDirectory parentOfOutputDir = new OutputDirectory(dirInfo.Parent.FullName);

                //set the output dir (the one we expose)
                this.OutputDirectory = new OutputDirectory(dirInfo.Name);

                //get the output file //Initializer
                XElement rootTag = this.internalXDoc.Root;

                //OutputFile
                XAttribute attri = rootTag.Attribute("OutputFile");
                IXmlLineInfo attriLineInfo = (IXmlLineInfo)attri;
                string outfileAttri = null;
                if (attri != null)
                {
                    outfileAttri = attri.Value;
                }
                //else
                //{
                //    outfileAttri = "{HtmlFileInitializer " + Path.GetFileNameWithoutExtension(new FileInfo(this.SourcePath).Name) + ".html}";
                //}

                if (outfileAttri != null)
                {
                    OutputFile outputFile = null;
                    if (outfileAttri.StartsWith("{") && outfileAttri.EndsWith("}") && !(outfileAttri.StartsWith("{{")))
                    {
                        //remove first char ('{') and last char ('}')
                        outfileAttri = outfileAttri.Substring(1, outfileAttri.Length - 2);

                        //split into file initializer and file name
                        string fileInitializer = null, filename = null;
                        if (outfileAttri.Contains(' '))
                        {
                            fileInitializer = outfileAttri.Substring(0, outfileAttri.IndexOf(' '));
                            filename = outfileAttri.Substring(outfileAttri.IndexOf(' ') + 1); //file name
                        }
                        else
                        {
                            fileInitializer = outfileAttri;
                            filename = null;
                        }

                        object obj = null;
                        try { obj = this.BuildObject(fileInitializer); }
                        catch (TypeLoadException)
                        {
                            try { obj = this.BuildObject(fileInitializer + "Initializer"); }
                            catch (TypeLoadException) 
                            {
                                try
                                { obj = this.BuildObject(fileInitializer + "Initialiser"); }
                                catch (Exception ex)
                                {
                                    throw new QuicException(ex.Message,
                                      this.SourcePath, attriLineInfo.LineNumber, attriLineInfo.LinePosition);
                                } 
                            }
                        }

                        if (!(obj is FileInitializer))
                        {
                            throw new QuicException(string.Format("Object '{0}' is not a file initializer.", fileInitializer),
                                   this.SourcePath, attriLineInfo.LineNumber, attriLineInfo.LinePosition); 
                        }

                        var fileInitObj = (FileInitializer)obj;
                        outputFile = fileInitObj.InitializeFile(this, filename);
                        if (outputFile.ParentDirectory == null)
                        {
                            this.OutputDirectory.Add(outputFile, true);
                        }
                    }
                    else
                    {
                        //if value starts with "{{" and ends with "}", change that "{{" to "{"
                        if (outfileAttri.StartsWith("{{") && outfileAttri.EndsWith("}"))
                        {
                            outfileAttri = outfileAttri.Substring(1);
                        }

                        outputFile = new TextFile(outfileAttri);
                        this.OutputDirectory.Add(outputFile, true);
                    }
                    this.OutputFile = outputFile;
                }
                else
                {
                    HtmlFileInit fileInitObj = new HtmlFileInit();
                    OutputFile outputFile = fileInitObj.InitializeFile(this,
                        Path.GetFileNameWithoutExtension(new FileInfo(this.SourcePath).Name) + ".html");
                    if (outputFile.ParentDirectory == null)
                    {
                        this.OutputDirectory.Add(outputFile, true);
                    }
                    this.OutputFile = outputFile;
                }

                //enter <head>
                if (this.OutputFile is HtmlOutputFile)
                    ((HtmlOutputFile)this.OutputFile).CurrentSection = ((HtmlOutputFile)this.OutputFile).HeadSection;
                //render head
                foreach (var hd in headElements)//(var res in resDic.Values) 
                {
                    hd.BeginRender();
                }

                //enter <body>
                if (this.OutputFile is HtmlOutputFile)
                    ((HtmlOutputFile)this.OutputFile).CurrentSection = ((HtmlOutputFile)this.OutputFile);
                //render body
                foreach (var el in bodyElements)
                {
                    el.BeginRender();
                }

                //save output dir
                parentOfOutputDir.Add(this.OutputDirectory, true);
                this.OutputDirectory.Commit(false, true);
            }
            catch (QuicException)
            {
                throw;
            }
            catch (XmlException ex)
            {
                throw new QuicException(ex.Message, this.SourcePath, ex.LineNumber, ex.LinePosition, ex);
            }
            catch (Exception ex)
            {
                throw new QuicException(ex.Message, this.SourcePath, ex);
            }
        }
        ///// <summary>
        ///// Generates the output files.
        ///// </summary>
        ///// <param name="outputDir"></param>
        //public void Render(string outputDir)
        //{
        //    try
        //    {
        //        //get the root output dir (we dont expose this dir)
        //        DirectoryInfo dirInfo = new DirectoryInfo(outputDir);
        //        if (!dirInfo.Parent.Exists)  //should have a parent that exists
        //            throw new DirectoryNotFoundException(string.Format("Cannot find parent directory of output directory:\n'{0}'", outputDir));
        //        OutputDirectory parentOfOutputDir = new OutputDirectory(dirInfo.Parent.FullName);

        //        //set the output dir (the one we expose)
        //        this.OutputDirectory = new OutputDirectory(dirInfo.Name);

        //        //get the output file //Initializer
        //        XmlElement quicTag = null;
        //        XmlNodeList allQuicTags = this.internalXmlDoc.GetElementsByTagName("Quic");
        //        if (allQuicTags.Count > 1)
        //        {
        //            throw new QuicException("Document cannot have more than one <Quic> tag.", this.SourcePath);
        //        }
        //        else if (allQuicTags.Count == 1)
        //        {
        //            quicTag = (XmlElement)allQuicTags[0];
        //        }
        //        if (quicTag != null)
        //        {
        //            //OutputFile
        //            string outfileAttri = null;
        //            if (quicTag.HasAttribute("OutputFile"))
        //            {
        //                outfileAttri = quicTag.GetAttribute("OutputFile");
        //            }
        //            //else
        //            //{
        //            //    outfileAttri = "{HtmlFileInitializer " + Path.GetFileNameWithoutExtension(new FileInfo(this.SourcePath).Name) + ".html}";
        //            //}

        //            if (outfileAttri != null)
        //            {
        //                OutputFile outputFile = null;
        //                if (outfileAttri.StartsWith("{") && outfileAttri.EndsWith("}") && !(outfileAttri.StartsWith("{{")))
        //                {
        //                    //remove first char ('{') and last char ('}')
        //                    outfileAttri = outfileAttri.Substring(1, outfileAttri.Length - 2);

        //                    //split into file initializer and file name
        //                    string fileInitializer = null, filename = null;
        //                    if (outfileAttri.Contains(' '))
        //                    {
        //                        fileInitializer = outfileAttri.Substring(0, outfileAttri.IndexOf(' '));
        //                        filename = outfileAttri.Substring(outfileAttri.IndexOf(' ') + 1); //file name
        //                    }
        //                    else
        //                    {
        //                        fileInitializer = outfileAttri;
        //                        filename = null;
        //                    }

        //                    object obj = null;
        //                    try { obj = this.BuildObject(fileInitializer); }
        //                    catch
        //                    {
        //                        try { obj = this.BuildObject(fileInitializer + "Initializer"); }
        //                        catch { obj = this.BuildObject(fileInitializer + "Initialiser"); }
        //                    }

        //                    if (!(obj is FileInitializer))
        //                        throw new QuicException(string.Format("Object '{0}' is not a file initializer.", fileInitializer),
        //                            this.SourcePath);

        //                    var fileInitObj = (FileInitializer)obj;
        //                    outputFile = fileInitObj.InitializeFile(this, filename);
        //                    if (outputFile.ParentDirectory == null)
        //                    {
        //                        this.OutputDirectory.Add(outputFile, true);
        //                    }
        //                }
        //                else
        //                {
        //                    //if value starts with "{{" and ends with "}", change that "{{" to "{"
        //                    if (outfileAttri.StartsWith("{{") && outfileAttri.EndsWith("}"))
        //                    {
        //                        outfileAttri = outfileAttri.Substring(1);
        //                    }

        //                    outputFile = new TextOutputFile(outfileAttri);
        //                    this.OutputDirectory.Add(outputFile, true);
        //                }
        //                this.OutputFile = outputFile;
        //            }
        //            else
        //            {
        //                HtmlFileInit fileInitObj = new HtmlFileInit();
        //                OutputFile outputFile = fileInitObj.InitializeFile(this,
        //                    Path.GetFileNameWithoutExtension(new FileInfo(this.SourcePath).Name) + ".html");
        //                if (outputFile.ParentDirectory == null)
        //                {
        //                    this.OutputDirectory.Add(outputFile, true);
        //                }
        //                this.OutputFile = outputFile;
        //            }
        //        }

        //        //render resources
        //        foreach (var res in resList)//(var res in resDic.Values) 
        //        {
        //            res.BeginRender();
        //        }
        //        //render UI
        //        foreach (var ui in uiElements)
        //        {
        //            ui.BeginRender();
        //        }

        //        //save output dir
        //        parentOfOutputDir.Add(this.OutputDirectory, true);
        //        this.OutputDirectory.Commit(false, true);
        //    }
        //    catch (QuicException)
        //    {
        //        throw;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new QuicException(ex.Message, this.SourcePath, ex);
        //    }
        //}

        ///_________CACHES_________
        Assembly GetCachedOrNewAssembly(string assemblyPath) 
        {
            assemblyPath = assemblyPath.ToLower();

            Assembly asm = null;

            if (cachedAsm.ContainsKey(assemblyPath))
                asm = cachedAsm[assemblyPath];
            else
            {
                //asm = Assembly.LoadFrom(assemblyPath);
                
                AppDomain domain = AppDomain.CurrentDomain;//.CreateDomain("MyDomain");
                asm = domain.Load(AssemblyName.GetAssemblyName(assemblyPath));

                cachedAsm.Add(assemblyPath, asm);
            }

            return asm;
        }

        //_________NAMED ELEMENTS_________
        /// <summary>
        /// Gets an element with the specified name.
        /// Returns null if no such element exists.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Element GetNamedElement(string name) 
        {
            if (namedElements.ContainsKey(name))
                return namedElements[name];

            return null;
        }

        //_________RESOURCES_________
        /// <summary>
        /// Gets a resource element from the resource dictionary with the specified key.
        /// Returns null if no such element exists.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public IResource GetResourceElement(string key)
        {
            if (resDic.ContainsKey(key))
                return resDic[key];

            return null;
        }

        //_________UI_________
        /// <summary>
        /// Gets an array of the root body elements in the document.
        /// </summary>
        public Element[] BodyElements
        {
            get { return bodyElements.ToArray(); }
        }

        //_________PROPERTIES_________
        /// <summary>
        /// Gets the XML file path from which the document was formed.
        /// </summary>
        public string SourcePath { get; private set; }

        //_________OUTPUT_________
        /// <summary>
        /// Gets the output directory 
        /// </summary>
        public OutputDirectory OutputDirectory { get; private set; }
        /// <summary>
        /// Gets the output file 
        /// </summary>
        public OutputFile OutputFile { get; private set; }
        /// <summary>
        /// Gets the options with which this document is outputed
        /// </summary>
        public OutputOptions OutputOptions { get; private set; }
    }
}
