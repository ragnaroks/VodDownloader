using System;
using VodDownloader.Entities;

namespace VodDownloader.Interfaces;

public interface ISiteParserService {
    public VodDetailLite? GetVodDetail (UInt32 vid);
}
