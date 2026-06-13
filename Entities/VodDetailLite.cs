using System;
using System.Collections.Generic;

namespace VodDownloader.Entities;

public class VodDetailLite {
    public UInt32 Id { get; set; }
    public String Name { get; set; } = String.Empty;
    public String Date { get; set; } = String.Empty;
    public String Cover { get; set; } = String.Empty;
    public String Description { get; set; } = String.Empty;
    public Queue<String> M3U8Queue { get; set; } = [];
}
