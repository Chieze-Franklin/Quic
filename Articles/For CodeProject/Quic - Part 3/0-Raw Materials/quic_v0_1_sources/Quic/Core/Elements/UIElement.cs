using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quic
{
    /// <summary>
    /// Represents a UI element
    /// </summary>
    public abstract class UIElement : Element
    {   
        //----BASIC UI PROPS (the intention is to provide as few as possible)----

        /// <summary>
        /// Gets or sets the backgroud color of the control
        /// </summary>
        public virtual Color BackColor { get; set; }
        /// <summary>
        /// Gets or sets the foregroud color of the control
        /// </summary>
        public virtual Color ForeColor { get; set; }
        /// <summary>
        /// Gets or sets the height of the control
        /// </summary>
        public virtual int Height { get; set; }
        /// <summary>
        /// Gets or sets the text of the control
        /// </summary>
        public virtual string Text { get; set; }
        /// <summary>
        /// Gets or sets the width of the control
        /// </summary>
        public virtual int Width { get; set; }
        /// <summary>
        /// Gets or sets a value to determine if the control is visible
        /// </summary>
        public virtual bool Visible { get; set; }
        /// <summary>
        /// Gets or sets the X coordinate of the control
        /// </summary>
        public virtual int X { get; set; }
        /// <summary>
        /// Gets or sets the Y coordinate of the control
        /// </summary>
        public virtual int Y { get; set; }
    }
}
