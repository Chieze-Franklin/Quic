using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quic;

/// <summary>
/// Base class for containers that can have orientation
/// </summary>
public abstract class OrientationContainer : BootstrapContainerElement
{
    public override IValueProvider GetImplicitValueProvider(string propertyName, Type propertyType)
    {
        if (propertyName == "Orientation")
        {
            return OrientationProvider.Singleton();
        }

        return base.GetImplicitValueProvider(propertyName, propertyType);
    }

    /// <summary>
    /// Gets or sets the orientation
    /// </summary>
    public Orientation Orientation { get; set; }
}