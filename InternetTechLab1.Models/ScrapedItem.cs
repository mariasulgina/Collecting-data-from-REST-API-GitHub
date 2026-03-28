using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InternetTechLab1.Models;

public record ScrapedItem(
    string Url,
    string DataType,
    string Value
)
{
    public ScrapedItem() : this(string.Empty, "Unknown", string.Empty) { }
}
