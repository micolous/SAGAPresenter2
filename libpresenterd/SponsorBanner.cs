
using System;
using System.Xml.Serialization;

namespace SAGAPresenter.libpresenterd
{

	[XmlType]
	public struct SponsorBanner
	{
		[XmlAttribute("id")]
		public string BannerID;
		[XmlAttribute("weight")]
		public uint Weight;
	}
}
