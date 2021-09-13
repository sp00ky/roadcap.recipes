using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace roadcap.recipes.TagHelpers
{
    public class EllipsisTagHelper : TagHelper
    {
        public string DisplayString { get; set; }

        public int DisplayLength { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "span";
            output.TagMode = TagMode.StartTagAndEndTag;
            var display = DisplayString.Substring(0, Math.Min(DisplayString.Length, DisplayLength));
            if (DisplayString.Length > DisplayLength)
            {
                display += "...";
            }

            output.Content.SetContent(display); 

        }
    }
}
