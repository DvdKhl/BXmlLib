using BXmlLib.DataSource;
using BXmlLib.DocType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace BXmlLib.DocTypes.MP4 {
	public class MP4DocType : IBXmlDocType {
		public static string KeyToString(ReadOnlySpan<byte> key) {
			return "" + (char)key[0] + (char)key[1] + (char)key[2] + (char)key[3];
		}

		#region DocTypes
		public static readonly MP4DocElement Root = new MP4DocElement("<ROOT>", true, nameof(Root));
		public static readonly MP4DocElement Any = new MP4DocElement("<ANY>", true, nameof(Any));
		public static readonly MP4DocElement Unknown = new MP4DocElement("<UNKNOWN>", false, nameof(Unknown));

		public static readonly MP4DocElement FileType = new MP4DocElement("ftyp", false, nameof(FileType)); // *
		public static readonly MP4DocElement ProgressiveDownloadInfo = new MP4DocElement("pdin", false, nameof(ProgressiveDownloadInfo)); //
		public static readonly MP4DocElement Movie = new MP4DocElement("moov", true, nameof(Movie)); // *
		public static readonly MP4DocElement MovieHeader = new MP4DocElement("mvhd", false, nameof(MovieHeader)); // *
		public static readonly MP4DocElement Track = new MP4DocElement("trak", true, nameof(Track)); // *
		public static readonly MP4DocElement TrackHeader = new MP4DocElement("tkhd", false, nameof(TrackHeader)); // *
		public static readonly MP4DocElement TrackReference = new MP4DocElement("tref", false, nameof(TrackReference)); //
		public static readonly MP4DocElement Edit = new MP4DocElement("edts", true, nameof(Edit)); //
		public static readonly MP4DocElement EditList = new MP4DocElement("elst", false, nameof(EditList)); //
		public static readonly MP4DocElement Media = new MP4DocElement("mdia", true, nameof(Media)); // *
		public static readonly MP4DocElement MediaHeader = new MP4DocElement("mdhd", false, nameof(MediaHeader)); // *
		public static readonly MP4DocElement HandlerReference = new MP4DocElement("hdlr", false, nameof(HandlerReference)); // *
		public static readonly MP4DocElement MediaInformation = new MP4DocElement("minf", true, nameof(MediaInformation)); // *
		public static readonly MP4DocElement VideoMediaHeader = new MP4DocElement("vmhd", false, nameof(VideoMediaHeader)); //
		public static readonly MP4DocElement SoundMediaHeader = new MP4DocElement("smhd", false, nameof(SoundMediaHeader)); //
		public static readonly MP4DocElement HintMediaHeader = new MP4DocElement("hmhd", false, nameof(HintMediaHeader)); //
		public static readonly MP4DocElement NullMediaHeader = new MP4DocElement("nmhd", false, nameof(NullMediaHeader)); //
		public static readonly MP4DocElement DataInformation = new MP4DocElement("dinf", true, nameof(DataInformation)); // *
		public static readonly MP4DocElement DataEntryUrl = new MP4DocElement("url ", false, nameof(DataEntryUrl)); // *
		public static readonly MP4DocElement DataEntryUrn = new MP4DocElement("urn ", false, nameof(DataEntryUrn)); // *
		public static readonly MP4DocElement DataReference = new MP4DocElement("dref", false, nameof(DataReference)); // *
		public static readonly MP4DocElement SampleTable = new MP4DocElement("stbl", true, nameof(SampleTable)); // *
		public static readonly MP4DocElement SampleDescription = new MP4DocElement("stsd", false, nameof(SampleDescription)); // *
		public static readonly MP4DocElement TimeToSample = new MP4DocElement("stts", false, nameof(TimeToSample)); // *
		public static readonly MP4DocElement CompositionOffset = new MP4DocElement("ctts", false, nameof(CompositionOffset)); //
		public static readonly MP4DocElement SampleToChunk = new MP4DocElement("stsc", false, nameof(SampleToChunk)); // *
		public static readonly MP4DocElement SampleSize = new MP4DocElement("stsz", false, nameof(SampleSize)); //
		public static readonly MP4DocElement CompactSampleSize = new MP4DocElement("stz2", false, nameof(CompactSampleSize)); //
		public static readonly MP4DocElement ChunkOffset = new MP4DocElement("stco", false, nameof(ChunkOffset)); // *
		public static readonly MP4DocElement ChunkLargeOffset = new MP4DocElement("co64", false, nameof(ChunkLargeOffset)); //
		public static readonly MP4DocElement SyncSample = new MP4DocElement("stss", false, nameof(SyncSample)); //
		public static readonly MP4DocElement ShadowSyncSample = new MP4DocElement("stsh", false, nameof(ShadowSyncSample)); //
		public static readonly MP4DocElement PaddingBits = new MP4DocElement("padb", false, nameof(PaddingBits)); //
		public static readonly MP4DocElement DegradationPriority = new MP4DocElement("stdp", false, nameof(DegradationPriority)); //
		public static readonly MP4DocElement SampleDependencyType = new MP4DocElement("sdtp", false, nameof(SampleDependencyType)); //
		public static readonly MP4DocElement SampleToGroup = new MP4DocElement("sbgp", false, nameof(SampleToGroup)); //
		public static readonly MP4DocElement SampleGroupDescription = new MP4DocElement("sgpd", false, nameof(SampleGroupDescription)); //
		public static readonly MP4DocElement MovieExtends = new MP4DocElement("mvex", true, nameof(MovieExtends)); //
		public static readonly MP4DocElement MovieExtendsHeader = new MP4DocElement("mehd", false, nameof(MovieExtendsHeader)); //
		public static readonly MP4DocElement TrackExtends = new MP4DocElement("trex", false, nameof(TrackExtends)); // *
		public static readonly MP4DocElement IPMPControl = new MP4DocElement("ipmc", false, nameof(IPMPControl)); //
		public static readonly MP4DocElement MovieFragment = new MP4DocElement("moof", true, nameof(MovieFragment)); //
		public static readonly MP4DocElement MovieFragmentHeader = new MP4DocElement("mfhd", false, nameof(MovieFragmentHeader)); // *
		public static readonly MP4DocElement TrackFragment = new MP4DocElement("traf", true, nameof(TrackFragment)); //
		public static readonly MP4DocElement TrackFragmentHeader = new MP4DocElement("tfhd", false, nameof(TrackFragmentHeader)); // *
		public static readonly MP4DocElement TrackRun = new MP4DocElement("trun", false, nameof(TrackRun)); //
		public static readonly MP4DocElement SubSampleInformation = new MP4DocElement("subs", false, nameof(SubSampleInformation)); //
		public static readonly MP4DocElement MovieFragmentRandomAccess = new MP4DocElement("mfra", true, nameof(MovieFragmentRandomAccess)); //
		public static readonly MP4DocElement TrackFragmentRandomAccess = new MP4DocElement("tfra", false, nameof(TrackFragmentRandomAccess)); //
		public static readonly MP4DocElement MovieFragmentRandomAccessOffset = new MP4DocElement("mfro", false, nameof(MovieFragmentRandomAccessOffset)); // *
		public static readonly MP4DocElement MediaData = new MP4DocElement("mdat", false, nameof(MediaData)); //
		public static readonly MP4DocElement FreeSpace = new MP4DocElement("free", false, nameof(FreeSpace)); //
		public static readonly MP4DocElement SkipSpace = new MP4DocElement("skip", true, nameof(SkipSpace)); //
		public static readonly MP4DocElement UserData = new MP4DocElement("udta", true, nameof(UserData)); //
		public static readonly MP4DocElement Copyright = new MP4DocElement("cprt", false, nameof(Copyright)); //
		public static readonly MP4DocElement Meta = new MP4DocElement("meta", true, nameof(Meta)); //
		public static readonly MP4DocElement Handler = new MP4DocElement("hdlr", false, nameof(Handler)); // *
		public static readonly MP4DocElement ItemLocation = new MP4DocElement("iloc", false, nameof(ItemLocation)); //
		public static readonly MP4DocElement ItemProtection = new MP4DocElement("ipro", true, nameof(ItemProtection)); //
		public static readonly MP4DocElement ProtectionSchemeInfo = new MP4DocElement("sinf", true, nameof(ProtectionSchemeInfo)); //
		public static readonly MP4DocElement OriginalFormat = new MP4DocElement("frma", false, nameof(OriginalFormat)); //
		public static readonly MP4DocElement IPMPInfo = new MP4DocElement("imif", false, nameof(IPMPInfo)); //
		public static readonly MP4DocElement SchemeType = new MP4DocElement("schm", false, nameof(SchemeType)); //
		public static readonly MP4DocElement SchemeInformation = new MP4DocElement("schi", false, nameof(SchemeInformation)); //
		public static readonly MP4DocElement ItemInfo = new MP4DocElement("iinf", false, nameof(ItemInfo)); //
		public static readonly MP4DocElement Xml = new MP4DocElement("xml ", false, nameof(Xml)); //
		public static readonly MP4DocElement BXml = new MP4DocElement("bxml", false, nameof(BXml)); //
		public static readonly MP4DocElement PrimaryItem = new MP4DocElement("pitm", false, nameof(PrimaryItem)); //	
		#endregion


		private readonly Dictionary<int, MP4DocElement> docElementMap;

		private static int KeyToInt(string key) { return (key[0] << 0) | (key[1] << 8) | (key[2] << 16) | (key[3] << 24); }


		public MP4DocType() {
			docElementMap = EnumerateMetaData().Select(x => x.DocElement).ToDictionary(x => KeyToInt(x.Id));
		}

		public object DecodeData(BXmlDocElement docElement, ReadOnlySpan<byte> encodedData) {
			return encodedData.ToArray();
		}

		public BXmlDocElement GetDocElement(ReadOnlySpan<byte> encodedIdentifier, ref BXmlElementHeader header, ReadOnlySpan<BXmlReader.PositionData> parents) {
			if (encodedIdentifier.Length == 4 && docElementMap.TryGetValue(MemoryMarshal.Read<int>(encodedIdentifier), out var docElem)) {
				//var key = KeyToString(encodedIdentifier);

				return docElem;
			} else {
				return Unknown;
			}
		}

		private static IEnumerable<MP4DocMetaElement> EnumerateMetaData() {
			bool notNull(object obj) => obj is ulong ? ((ulong)obj != 0) : (obj is double ? ((double)obj != 0) : (obj is float ? ((float)obj != 0) : (obj is long ? ((long)obj != 0) : false)));
			bool greaterNull(object obj) => obj is ulong ? ((ulong)obj > 0) : (obj is double ? ((double)obj > 0) : (obj is float ? ((float)obj > 0) : (obj is long ? ((long)obj > 0) : false)));
			bool zeroOrOne(object obj) => obj is ulong ? ((ulong)obj == 0 || (ulong)obj == 1) : (obj is double ? ((double)obj == 0 || (double)obj == 1) : (obj is float ? ((float)obj == 0 || (float)obj == 1) : (obj is long ? ((long)obj == 0 || (long)obj == 1) : false)));

			yield return new MP4DocMetaElement("1Ma  ", FileType, null, null, new[] { Root }, "");
			yield return new MP4DocMetaElement("1    ", ProgressiveDownloadInfo, null, null, new[] { Root }, "");
			yield return new MP4DocMetaElement("1Ma  ", Movie, null, null, new[] { Root }, "");
			yield return new MP4DocMetaElement("1Ma  ", MovieHeader, null, null, new[] { Movie }, "");
			yield return new MP4DocMetaElement("1Ma  ", Track, null, null, new[] { Movie }, "");
			yield return new MP4DocMetaElement("1Ma  ", TrackHeader, null, null, new[] { Track }, "");
			yield return new MP4DocMetaElement("1    ", TrackReference, null, null, new[] { Track }, "");
			yield return new MP4DocMetaElement("1    ", Edit, null, null, new[] { Track }, "");
			yield return new MP4DocMetaElement("1    ", EditList, null, null, new[] { Edit }, "");
			yield return new MP4DocMetaElement("1Ma  ", Media, null, null, new[] { Track }, "");
			yield return new MP4DocMetaElement("1Ma  ", MediaHeader, null, null, new[] { Media }, "");
			yield return new MP4DocMetaElement("1Ma  ", MediaInformation, null, null, new[] { Media }, "");
			yield return new MP4DocMetaElement("1    ", VideoMediaHeader, null, null, new[] { MediaInformation }, "");
			yield return new MP4DocMetaElement("1    ", SoundMediaHeader, null, null, new[] { MediaInformation }, "");
			yield return new MP4DocMetaElement("1    ", HintMediaHeader, null, null, new[] { MediaInformation }, "");
			yield return new MP4DocMetaElement("1    ", NullMediaHeader, null, null, new[] { MediaInformation }, "");
			yield return new MP4DocMetaElement("1Ma  ", DataInformation, null, null, new[] { MediaInformation, Meta }, "");
			yield return new MP4DocMetaElement("1Ma  ", DataEntryUrl, null, null, new[] { DataInformation }, "");
			yield return new MP4DocMetaElement("1Ma  ", DataEntryUrn, null, null, new[] { DataInformation }, "");
			yield return new MP4DocMetaElement("1Ma  ", DataReference, null, null, new[] { DataInformation }, "");
			yield return new MP4DocMetaElement("1Ma  ", SampleTable, null, null, new[] { MediaInformation }, "");
			yield return new MP4DocMetaElement("1Ma  ", SampleDescription, null, null, new[] { SampleTable }, "");
			yield return new MP4DocMetaElement("1Ma  ", TimeToSample, null, null, new[] { SampleTable }, "");
			yield return new MP4DocMetaElement("1    ", CompositionOffset, null, null, new[] { SampleTable }, "");
			yield return new MP4DocMetaElement("1Ma  ", SampleToChunk, null, null, new[] { SampleTable }, "");
			yield return new MP4DocMetaElement("1    ", SampleSize, null, null, new[] { SampleTable }, "");
			yield return new MP4DocMetaElement("1    ", CompactSampleSize, null, null, new[] { SampleTable }, "");
			yield return new MP4DocMetaElement("1Ma  ", ChunkOffset, null, null, new[] { SampleTable }, "");
			yield return new MP4DocMetaElement("1    ", ChunkLargeOffset, null, null, new[] { SampleTable }, "");
			yield return new MP4DocMetaElement("1    ", SyncSample, null, null, new[] { SampleTable }, "");
			yield return new MP4DocMetaElement("1    ", ShadowSyncSample, null, null, new[] { SampleTable }, "");
			yield return new MP4DocMetaElement("1    ", PaddingBits, null, null, new[] { SampleTable }, "");
			yield return new MP4DocMetaElement("1    ", DegradationPriority, null, null, new[] { SampleTable }, "");
			yield return new MP4DocMetaElement("1    ", SampleDependencyType, null, null, new[] { SampleTable, TrackFragment }, ""); //
			yield return new MP4DocMetaElement("1    ", SampleToGroup, null, null, new[] { MediaInformation, TrackFragment }, ""); //
			yield return new MP4DocMetaElement("1    ", SampleGroupDescription, null, null, new[] { MediaInformation }, "");
			yield return new MP4DocMetaElement("1    ", MovieExtends, null, null, new[] { Movie }, "");
			yield return new MP4DocMetaElement("1    ", MovieExtendsHeader, null, null, new[] { MovieExtends }, "");
			yield return new MP4DocMetaElement("1Ma  ", TrackExtends, null, null, new[] { Movie }, "");
			yield return new MP4DocMetaElement("1    ", IPMPControl, null, null, new[] { Movie }, "");
			yield return new MP4DocMetaElement("1    ", MovieFragment, null, null, new[] { Root }, "");
			yield return new MP4DocMetaElement("1Ma  ", MovieFragmentHeader, null, null, new[] { MovieFragment }, "");
			yield return new MP4DocMetaElement("1    ", TrackFragment, null, null, new[] { MovieFragment }, "");
			yield return new MP4DocMetaElement("1Ma  ", TrackFragmentHeader, null, null, new[] { TrackFragment }, "");
			yield return new MP4DocMetaElement("1    ", TrackRun, null, null, new[] { TrackFragment }, "");
			yield return new MP4DocMetaElement("1    ", SubSampleInformation, null, null, new[] { SampleTable, TrackFragment }, ""); //
			yield return new MP4DocMetaElement("1    ", MovieFragmentRandomAccess, null, null, new[] { Root }, "");
			yield return new MP4DocMetaElement("1    ", TrackFragmentRandomAccess, null, null, new[] { MovieFragmentRandomAccess }, "");
			yield return new MP4DocMetaElement("1Ma  ", MovieFragmentRandomAccessOffset, null, null, new[] { MovieFragmentRandomAccess }, "");
			yield return new MP4DocMetaElement("1    ", MediaData, null, null, new[] { Root }, "");
			yield return new MP4DocMetaElement("1    ", FreeSpace, null, null, new[] { Any }, "");
			yield return new MP4DocMetaElement("1    ", SkipSpace, null, null, new[] { Any }, "");
			yield return new MP4DocMetaElement("1    ", UserData, null, null, new[] { SkipSpace }, "");
			yield return new MP4DocMetaElement("1    ", Copyright, null, null, new[] { UserData }, "");
			yield return new MP4DocMetaElement("1    ", Meta, null, null, new[] { Root }, "");
			yield return new MP4DocMetaElement("1Ma  ", Handler, null, null, new[] { Media, Meta }, "");
			yield return new MP4DocMetaElement("1    ", ItemLocation, null, null, new[] { Meta }, "");
			yield return new MP4DocMetaElement("1    ", ItemProtection, null, null, new[] { Meta }, "");
			yield return new MP4DocMetaElement("1    ", ProtectionSchemeInfo, null, null, new[] { ItemProtection }, "");
			yield return new MP4DocMetaElement("1    ", OriginalFormat, null, null, new[] { ProtectionSchemeInfo }, "");
			yield return new MP4DocMetaElement("1    ", IPMPInfo, null, null, new[] { ProtectionSchemeInfo }, "");
			yield return new MP4DocMetaElement("1    ", SchemeType, null, null, new[] { ProtectionSchemeInfo }, "");
			yield return new MP4DocMetaElement("1    ", SchemeInformation, null, null, new[] { ProtectionSchemeInfo }, "");
			yield return new MP4DocMetaElement("1    ", ItemInfo, null, null, new[] { Meta }, "");
			yield return new MP4DocMetaElement("1    ", Xml, null, null, new[] { Meta }, "");
			yield return new MP4DocMetaElement("1    ", BXml, null, null, new[] { Meta }, "");
			yield return new MP4DocMetaElement("1    ", PrimaryItem, null, null, new[] { Meta }, "");
		}
	}
}