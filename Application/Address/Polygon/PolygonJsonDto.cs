using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace CleanArchitecture.Application.HrManagment.Polygon;

public partial class PolygonJsonDto
{
    [JsonProperty("type")]
    public string Type { get; set; }

    [JsonProperty("id")]
    public long Id { get; set; }

    [JsonProperty("geometry")]
    public Geometry Geometry { get; set; }

    [JsonProperty("properties")]
    public Properties Properties { get; set; }
}

public partial class Geometry
{
    [JsonProperty("type")]
    public string Type { get; set; }

    [JsonProperty("coordinates")]
    public double[][][] Coordinates { get; set; }
}

public partial class Properties
{
    [JsonProperty("OID")]
    public long Oid { get; set; }

    [JsonProperty("Name")]
    public string Name { get; set; }

    [JsonProperty("FolderPath")]
    public string FolderPath { get; set; }

    [JsonProperty("SymbolID")]
    public long SymbolId { get; set; }

    [JsonProperty("AltMode")]
    public long AltMode { get; set; }

    [JsonProperty("Base")]
    public long Base { get; set; }

    [JsonProperty("Clamped")]
    public long Clamped { get; set; }

    [JsonProperty("Extruded")]
    public long Extruded { get; set; }

    [JsonProperty("Snippet")]
    public string Snippet { get; set; }

    [JsonProperty("PopupInfo")]
    public string PopupInfo { get; set; }

    [JsonProperty("Shape_Length")]
    public double ShapeLength { get; set; }

    [JsonProperty("Shape_Area")]
    public double ShapeArea { get; set; }

    [JsonProperty("JetId")]
    public string JetId { get; set; }
}
