﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quic;

public class ButtonToolbar : BootstrapContainerElement
{
    public ButtonToolbar()
    {
        CssClasses.Add("btn-toolbar");
    }

    public override void Render()
    {
        if (this.Document.OutputFile is TextFile)
        {
            TextFile outputFile = (TextFile)this.Document.OutputFile;

            //opening tag
            outputFile.Write("<div role=\"toolbar\"");
            outputFile.Write(EmitID());
            outputFile.Write(EmitClasses());

            //custom properties
            outputFile.Write(EmitCustomProperties());

            //opening tag 2
            outputFile.WriteLine(">");

            //child elements
            foreach (var element in Elements)
            {
                element.BeginRender();
            }

            //closing tag
            outputFile.WriteLine("</div>");
        }
    }
}