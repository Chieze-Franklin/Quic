using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Quic.Messaging;

namespace Quic
{
    /// <summary>
    /// Base class for all elements
    /// </summary>
    public abstract class Element
    {
        ElementCollection elements;
        Dictionary<string, string> customProperties = new Dictionary<string, string>();
        Dictionary<string, string> lazilyEvalProperties = new Dictionary<string, string>();
        string name;
        int repeat = 1;

        public Element() 
        {
            elements = new ElementCollection(this);
        }

        /// <summary>
        /// Called to begin the process of rendering
        /// </summary>
        public void BeginRender()
        {
            RepeatCount = 0;
            for (int index = 0; index < Repeat; index++)
            {
                RepeatCount += 1;

                //evaluate lazily evaluated properties
                foreach (KeyValuePair<string, string> keyValue in lazilyEvalProperties)
                {
                    SetProperty(keyValue.Key, keyValue.Value);
                }
                ////clear the dic
                //lazilyEvalProperties.Clear();

                //call Render
                Render(); 
            }
        }

        /// <summary>
        /// Helper method to emit custom properties
        /// </summary>
        protected string EmitCustomProperties()
        {
            StringBuilder sb = new StringBuilder("");

            foreach (KeyValuePair<string, string> keyValue in CustomProperties)
            {
                sb.Append(string.Format(" {0}=\"{1}\"", keyValue.Key, keyValue.Value));
            }

            return sb.ToString();
        }

        /// <summary>
        /// Tries to get the appropriate (implicit) value property for the specifed pair of property name and property type.
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="propertyType"></param>
        /// <returns></returns>
        public virtual IValueProvider GetImplicitValueProvider(string propertyName, Type propertyType)
        {
            if (typeof(string).IsAssignableFrom(propertyType))
                return StringVP.Singleton();
            else if (typeof(bool).IsAssignableFrom(propertyType))
                return BoolVP.Singleton();
            else if (typeof(sbyte).IsAssignableFrom(propertyType))
                return SByteVP.Singleton();
            else if (typeof(byte).IsAssignableFrom(propertyType))
                return ByteVP.Singleton();
            else if (typeof(char).IsAssignableFrom(propertyType))
                return CharVP.Singleton();
            else if (typeof(decimal).IsAssignableFrom(propertyType))
                return DecimalVP.Singleton();
            else if (typeof(short).IsAssignableFrom(propertyType))
                return ShortVP.Singleton();
            else if (typeof(ushort).IsAssignableFrom(propertyType))
                return UShortVP.Singleton();
            else if (typeof(int).IsAssignableFrom(propertyType))
                return IntVP.Singleton();
            else if (typeof(uint).IsAssignableFrom(propertyType))
                return UIntVP.Singleton();
            else if (typeof(long).IsAssignableFrom(propertyType))
                return LongVP.Singleton();
            else if (typeof(ulong).IsAssignableFrom(propertyType))
                return ULongVP.Singleton();
            else if (typeof(float).IsAssignableFrom(propertyType))
                return FloatVP.Singleton();
            else if (typeof(double).IsAssignableFrom(propertyType))
                return DoubleVP.Singleton();

            else if (typeof(System.Drawing.Color).IsAssignableFrom(propertyType))
                return ColorVP.Singleton();
            else if (typeof(System.DateTime).IsAssignableFrom(propertyType))
                return DateTimeVP.Singleton();
            else if (typeof(System.Drawing.Image).IsAssignableFrom(propertyType))
                return ImageVP.Singleton();
            else if (typeof(System.Drawing.Point).IsAssignableFrom(propertyType))
                return PointVP.Singleton();
            else if (typeof(System.Drawing.PointF).IsAssignableFrom(propertyType))
                return PointFVP.Singleton();
            else if (typeof(System.Drawing.Size).IsAssignableFrom(propertyType))
                return SizeVP.Singleton();
            else if (typeof(System.Drawing.SizeF).IsAssignableFrom(propertyType))
                return SizeFVP.Singleton();

            return null;
        }

        /// <summary>
        /// Gets the specified property.
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        public object GetProperty(string property)
        {
            PropertyInfo propertyInfo = null;
            if (this.Document.OutputOptions.IgnoreAttributeCase)
            {
                propertyInfo = this.GetType().GetProperties(BindingFlags.Public | BindingFlags.IgnoreCase).FirstOrDefault
                    (p => p.Name.ToLower() == property.ToLower() /*&& p.CanWrite*/ && p.GetCustomAttribute<NotQuicPropertyAttribute>(true) == null);
            }
            else
            {
                propertyInfo = this.GetType().GetProperties().FirstOrDefault
                    (p => p.Name == property /*&& p.CanWrite*/ && p.GetCustomAttribute<NotQuicPropertyAttribute>(true) == null);
            }
            //this.GetType().GetTypeInfo().GetProperty(property, BindingFlags.)

            if (propertyInfo != null)
            {
                return propertyInfo.GetValue(this);
            }
            else if (customProperties.ContainsKey(property) && this.Document.OutputOptions.AllowUnknownAttributes) 
            {
                return customProperties[property];
            }
            else
            {
                throw new Exception(string.Format("Property '{0}' cannot be found on element '{1}'.", property, this.GetType()));
            }
        }

        /// <summary>
        /// Called when the output files are being generated.
        /// </summary>
        public abstract void Render();

        object Evaluate(string property, Type propertyType, string value) 
        {
            if (value == null)
                return value;

            IValueProvider vp = null;

            //if value starts with "{" (and ends with "}") but doesnt start with "{{" then we are dealing with a named value provider
            if (value.StartsWith("{") && value.EndsWith("}") && !(value.StartsWith("{{")))
            {
                //remove first char ('{') and last char ('}')
                value = value.Substring(1, value.Length - 2);

                //if value starts with '@' then we are dealing with a resource in the document resource dic
                if (value.StartsWith("@"))
                {
                    value = value.Substring(1);

                    //split value into value provider and input
                    string valueProvider = null;
                    if (value.Contains(' '))
                    {
                        valueProvider = value.Substring(0, value.IndexOf(' '));
                        value = value.Substring(value.IndexOf(' ') + 1); //input
                    }
                    else
                    {
                        valueProvider = value;
                        value = null; //""; //input
                    }

                    var res = this.Document.GetResourceElement(valueProvider);

                    if (!(res is IValueProvider))
                        throw new Exception(string.Format("Resource '{0}' is not a value provider.", valueProvider));

                    vp = (IValueProvider)res;
                }
                //if value starts with '$' then we are dealing with PropertyProvider (which has a key of '$')
                else if (value.StartsWith("$"))
                {
                    string valueProvider = "$";
                    value = value.Substring(1).Trim();

                    var res = this.Document.GetResourceElement(valueProvider);

                    if (!(res is IValueProvider))
                        throw new Exception(string.Format("Resource '{0}' is not a value provider.", valueProvider));

                    vp = (IValueProvider)res;
                }
                else
                {
                    //split value into value provider and input
                    string valueProvider = null;
                    if (value.Contains(' '))
                    {
                        valueProvider = value.Substring(0, value.IndexOf(' '));
                        value = value.Substring(value.IndexOf(' ') + 1); //input
                    }
                    else
                    {
                        valueProvider = value;
                        value = null; //input
                    }

                    object obj = null;
                    try { obj = this.Document.BuildObject(valueProvider); }
                    catch
                    {
                        obj = this.Document.BuildObject(valueProvider + "Provider");
                    }

                    if (!(obj is IValueProvider))
                        throw new Exception(string.Format("Object '{0}' is not a value provider.", valueProvider));

                    vp = (IValueProvider)obj;
                }

                vp.PropertyName = property;
                vp.PropertyType = propertyType;
                vp.Element = this;
                return vp.Evaluate(Evaluate(property, propertyType, value));
            }
            //if not, simply return the value
            else
            {
                return value;
            }
        }
        /// <summary>
        /// Sets the specified property to the specified value.
        /// </summary>
        /// <param name="property"></param>
        /// <param name="value"></param>
        public void SetProperty(string property, string value)
        {
            PropertyInfo propertyInfo = null;
            if (this.Document.OutputOptions.IgnoreAttributeCase)
            {
                propertyInfo = this.GetType().GetProperties().FirstOrDefault
                    (p => p.Name.ToLower() == property.ToLower() && p.CanWrite && p.GetCustomAttribute<NotQuicPropertyAttribute>(true) == null);
            }
            else
            {
                propertyInfo = this.GetType().GetProperties().FirstOrDefault
                    (p => p.Name == property && p.CanWrite && p.GetCustomAttribute<NotQuicPropertyAttribute>(true) == null);
            }
            //this.GetType().GetTypeInfo().GetProperty(property, BindingFlags.)

            if (propertyInfo != null)
            {
                if (value.StartsWith("{") && value.EndsWith("}") && !(value.StartsWith("{{")))
                {
                    if (value.StartsWith("{?")) //lazy evaluation
                    {
                        value = "{" + value.Substring(2);//remove the question mark

                        if (lazilyEvalProperties.ContainsKey(property))
                        {
                            lazilyEvalProperties[property] = value;
                        }
                        else if (!lazilyEvalProperties.ContainsKey(property))
                        {
                            lazilyEvalProperties.Add(property, value);
                        }
                        return;
                    }

                    try
                    {
                        object result = Evaluate(propertyInfo.Name, propertyInfo.PropertyType, value);
                        propertyInfo.SetValue(this, result); 
                    }
                    catch (QuicException ex)
                    {
                        Messenger.Notify(ex);
                    }
                    catch (Exception ex)
                    {
                        Messenger.Notify(
                            new QuicException(ex.Message,
                                this.Document.SourcePath, this.Line, this.Column, ex));
                    }
                }
                //if not, we are probably dealing with an implicit value provider
                else
                {
                    //if value starts with "{{" and ends with "}", change that "{{" to "{"
                    if (value.StartsWith("{{") && value.EndsWith("}"))
                    {
                        value = value.Substring(1);
                    }

                    IValueProvider vp = GetImplicitValueProvider(propertyInfo.Name, propertyInfo.PropertyType);

                    if (vp != null)
                    {
                        vp.PropertyName = propertyInfo.Name;
                        vp.PropertyType = propertyInfo.PropertyType;
                        vp.Element = this;
                    }

                    try
                    {
                        propertyInfo.SetValue(this, vp != null ? vp.Evaluate(value) : value);
                    }
                    catch (QuicException ex)
                    {
                        Messenger.Notify(ex);
                    }
                    catch (Exception ex)
                    {
                        Messenger.Notify(
                            new QuicException(ex.Message,
                                this.Document.SourcePath, this.Line, this.Column, ex));
                    }
                }
            }
            else if (customProperties.ContainsKey(property) && this.Document.OutputOptions.AllowUnknownAttributes)
            {
                customProperties[property] = value;
            }
            else if (!customProperties.ContainsKey(property) && this.Document.OutputOptions.AllowUnknownAttributes)
            {
                customProperties.Add(property, value);
            }
            else
            {
                Messenger.Notify(
                            new QuicException(string.Format("Property '{0}' cannot be found on element '{1}'.", property, this.GetType()),
                                this.Document.SourcePath, this.Line, this.Column));
            }
        }

        /// <summary>
        /// Returns true if the specified value is a valid identifier.
        /// A valid identifier contains alphanumeric characters, underscore, but must not start with a number.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        internal static bool IsValidIdentifier(string value) 
        {
            if (string.IsNullOrWhiteSpace(value))
                return false;

            char[] chars = value.ToCharArray();

            //if the first char is a digit, return false
            char c1 = chars[0];
            if (c1 >= '0' && c1 <= '9')
                return false;

            foreach (char c in chars) 
            {
                if ((c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z') || c == '_' || (c >= '0' && c <= '9'))
                {
                    continue;
                }
                else
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Gets or sets the content of the resource.
        /// This is usually got from the text typed within the opening and closing tags of the resource.
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// Gets a dictionary of custom property and value pairs
        /// </summary>
        [NotQuicProperty]
        protected Dictionary<string, string> CustomProperties 
        {
            get { return customProperties; }
        }
        /// <summary>
        /// Gets or sets the parent Quic document of this element.
        /// </summary>
        [NotQuicProperty]
        public QuicDocument Document { get; set; }
        /// <summary>
        /// Gets a collection of child UI elements.
        /// </summary>
        [NotQuicProperty]
        public ElementCollection Elements
        {
            get { return elements; }
        }
        /// <summary>
        /// Gets or sets a value to determine if this element is a container.
        /// Containers can have elements added to their UIElements collection.
        /// </summary>
        [NotQuicProperty]
        public bool IsContainer { get; set; }
        /// <summary>
        /// Gets or sets the name with which the element can be referenced.
        /// </summary>
        public virtual string Name 
        {
            get { return name; }
            set 
            {
                if (IsValidIdentifier(value))
                    name = value;
                else
                    throw new ArgumentException(string.Format("Invalid element name '{0}'", value));
            }
        }
        /// <summary>
        /// Gets or sets the parent element.
        /// </summary>
        [NotQuicProperty]
        public Element ParentElement { get; set; }
        /// <summary>
        /// Gets or sets the number of times this element will call its Render method.
        /// Default is 1
        /// </summary>
        public int Repeat 
        {
            get { return repeat; }
            set { repeat = value; }
        }
        /// <summary>
        /// Gets or the number of times this element has called its Render method.
        /// Default is 1
        /// </summary>
        public int RepeatCount { get; private set; }

        /// <summary>
        /// Gets the line number of the element
        /// </summary>
        public int Line { get; internal set; }
        /// <summary>
        /// Gets the column number of the element
        /// </summary>
        public int Column { get; internal set; }

        public class ElementCollection : IList<Element>
        {
            List<Element> elements = new List<Element>();
            Element parentElement;

            public ElementCollection(Element parent)
            {
                if (parent == null)
                    throw new ArgumentNullException("parent");

                this.parentElement = parent;
            }

            public void Add(Element item)
            {
                item.ParentElement = parentElement;
                elements.Add(item);
            }

            public void Clear()
            {
                foreach (var item in elements)
                {
                    item.ParentElement = null;
                }
                elements.Clear();
            }

            public bool Contains(Element item)
            {
                return elements.Contains(item);
            }

            public void CopyTo(Element[] array, int arrayIndex)
            {
                elements.CopyTo(array, arrayIndex);
            }

            public int Count
            {
                get { return elements.Count; }
            }

            public bool IsReadOnly
            {
                get { return false; }
            }

            public bool Remove(Element item)
            {
                bool remove = elements.Remove(item);

                if (remove)
                    item.ParentElement = null;

                return remove;
            }

            public IEnumerator<Element> GetEnumerator()
            {
                return elements.GetEnumerator();
            }

            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
            {
                return elements.GetEnumerator();
            }

            public int IndexOf(Element item)
            {
                return elements.IndexOf(item);
            }

            public void Insert(int index, Element item)
            {
                item.ParentElement = parentElement;
                elements.Insert(index, item);
            }

            public void RemoveAt(int index)
            {
                var item = elements[index];
                item.ParentElement = null;
                elements.RemoveAt(index);
            }

            public Element this[int index]
            {
                get
                {
                    return elements[index];
                }
                set
                {
                    Insert(index, value);
                }
            }
        }
    }
}
