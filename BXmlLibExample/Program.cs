using BXmlLib;
using BXmlLib.DocTypes.Ebml;
using BXmlLib.DocTypes.Matroska;
using BXmlLib.DocTypes.MP4;
using BXmlLib.DocTypes.MP4.Boxes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

namespace BXmlLibExample {


	public class EbmlBlockDataSourceExample : EbmlBlockDataSource {
		private readonly byte[] data;
		private int offset;

		public EbmlBlockDataSourceExample(byte[] data) => this.data = data;

		public override long Position {
			get => offset;
			set => Advance((int)value - offset);
		}

		public override void CommitPosition() { }
		protected override void Advance(int length) {
			offset += length;
			IsEndOfStream = offset == data.Length;
		}

		protected override ReadOnlySpan<byte> GetDataBlock(int minLength) => new ReadOnlySpan<byte>(data).Slice(offset);
	}
	public class MP4BlockDataSourceExample : MP4BlockDataSource {
		private readonly byte[] data;
		private int offset;

		public MP4BlockDataSourceExample(byte[] data) => this.data = data;

		public override long Position {
			get => offset;
			set => Advance((int)value - offset);
		}

		public override void CommitPosition() { }
		protected override void Advance(int length) {
			offset += length;
			IsEndOfStream = offset == data.Length;
		}

		protected override ReadOnlySpan<byte> GetDataBlock(int minLength) => new ReadOnlySpan<byte>(data).Slice(offset);
	}

	//public class EbmlBlockDataSourceExample : EbmlBlockDataSource {
	//    private byte[] data;
	//    private int offset;

	//    public EbmlBlockDataSourceExample(byte[] data) => this.data = data;

	//    public override long Position {
	//        get => offset;
	//        set {
	//            IsEndOfStream = !Advance((int)value - offset);
	//            offset = (int)value;
	//        }
	//    }

	//    protected override void Advance(int length) { offset += length; return (int)Position < data.Length; }
	//    protected override ReadOnlySpan<byte> Data(int minLength) => new ReadOnlySpan<byte>(data).Slice(offset);
	//}

	public class Program {
		private static void Main(string[] args) {
			var path = args[0];



			var ebmlDataSource = new MP4BlockDataSourceExample(File.ReadAllBytes(path));
			var mkvDocType = new MP4DocType();




			var reader = new BXmlReader(ebmlDataSource, mkvDocType);


			var node = MP4Node.Read(reader, 0);
			var mp4Data = new MP4Provider(node);
			return;
			//if(reader.DocElement.Name.Equals("DocTypeReadVersion")) {
			//    var asdfgh = 536;
			//}

			string handlerType = "";
			void Traverse(int depth) {
				while (reader.Next()) {
					Console.Write("".PadLeft(depth) + $"{reader.DocElement.Name}({reader.DocElement.IdentifierString}) Pos({reader.Header.Offset}) Len({reader.Header.DataLength})");
					if (!reader.DocElement.IsContainer) {
						string valueStr = null;
						if (reader.DocElement == MP4DocType.MovieHeader) {
							var aserg = new MovieHeaderBox(reader.RetrieveRawValue());
						} else if (reader.DocElement == MP4DocType.Handler) {
							valueStr = MP4DocType.KeyToString(new HandlerBox(reader.RetrieveRawValue()).HandlerType);
							handlerType = valueStr;

						} else if (reader.DocElement == MP4DocType.SampleDescription) {
							var sampleDescriptionBox = new SampleDescriptionBox(reader.RetrieveRawValue());
							if ("vide".Equals(handlerType)) {
								var videoEntry = sampleDescriptionBox.GetVideoEntry(0);
								var format = MP4DocType.KeyToString(videoEntry.Format);

							} else if ("soun".Equals(handlerType)) {
								var audioEntry = sampleDescriptionBox.GetAudioEntry(0);
								var format = MP4DocType.KeyToString(audioEntry.Format);

							} else if ("hint".Equals(handlerType)) {

							}


						} else if(reader.Header.DataLength > 0) {
							var sdrgh = reader.RetrieveRawValue();
						}
						Console.Write($" Value({valueStr})");
						//SampleDesciptionBox
					}
					Console.WriteLine();

					if (reader.DocElement.IsContainer) {
						using (reader.EnterElement()) {
							Traverse(depth + 1);
						}
					}
				}
			}

			Traverse(0);
		}






		public class MP4Node {
			private static readonly HashSet<MP4DocElement> DocElementsWithData = new HashSet<MP4DocElement>() {
				MP4DocType.MovieHeader, MP4DocType.TrackHeader, MP4DocType.MediaHeader, MP4DocType.Handler,
				MP4DocType.VideoMediaHeader, MP4DocType.SoundMediaHeader, MP4DocType.HintMediaHeader,
				MP4DocType.DataReference, MP4DocType.SampleDescription, MP4DocType.TimeToSample,
				MP4DocType.CompositionOffset, MP4DocType.SyncSample, MP4DocType.SampleToChunk, MP4DocType.SampleSize,
				MP4DocType.ChunkOffset, MP4DocType.ChunkLargeOffset, MP4DocType.DataEntryUrl, MP4DocType.DataEntryUrn,
				MP4DocType.Copyright, MP4DocType.FileType, MP4DocType.MovieExtendsHeader, MP4DocType.TrackFragmentHeader,
				MP4DocType.MovieFragmentRandomAccessOffset, MP4DocType.NullMediaHeader, MP4DocType.PaddingBits,
				MP4DocType.OriginalFormat, MP4DocType.PrimaryItem, MP4DocType.ProgressiveDownloadInfo, MP4DocType.SampleGroupDescription,
				MP4DocType.SchemeInformation, MP4DocType.SchemeType, MP4DocType.ShadowSyncSample, MP4DocType.SubSampleInformation,
				MP4DocType.TrackRun
			};


			public MP4DocElement DocElement { get; private set; }
			public ReadOnlyMemory<byte> Data { get; private set; } = ReadOnlyMemory<byte>.Empty;
			public long Size { get; private set; }
			public MP4Node Parent { get; private set; }
			public IReadOnlyList<MP4Node> Children => children; private List<MP4Node> children = new List<MP4Node>();

			public IEnumerable<MP4Node> Descendents(MP4DocElement docElement) {
				var toVisit = new Queue<MP4Node>(Children);

				while (toVisit.Count > 0) {
					var current = toVisit.Dequeue();

					if (current.DocElement == docElement) {
						yield return current;
					}

					foreach (var child in current.Children) {
						toVisit.Enqueue(child);
					}
				}
			}

			public static MP4Node Read(BXmlReader reader, long fileSize) {
				var box = new MP4Node {
					DocElement = MP4DocType.Root,
					Size = fileSize
				};

				Read(reader, box);

				return box;
			}
			private static void Read(BXmlReader reader, MP4Node box) {
				while (reader.Next()) {
					var child = new MP4Node {
						Parent = box,
						DocElement = (MP4DocElement)reader.DocElement,
						Size = reader.Header.DataLength
					};
					box.children.Add(child);

					if (reader.DocElement.IsContainer) {
						using (reader.EnterElement()) {
							Read(reader, child);
						}

					} else if (DocElementsWithData.Contains(child.DocElement)) {
						child.Data = reader.RetrieveRawValue().ToArray();
					}
				}
			}
		}


		public class MP4Provider {
			public MP4Node RootBox { get; private set; }

			public MP4Provider(MP4Node box) { Populate(box); }

			private void Populate(MP4Node box) {
				RootBox = box;
				if (RootBox == null) {
					return;
				}

				var fileTypeBox = new FileTypeBox(RootBox.Descendents(MP4DocType.FileType).First().Data.Span);
				var movieHeaderBox = new MovieHeaderBox(RootBox.Descendents(MP4DocType.MovieHeader).First().Data.Span);
				var tracksBox = RootBox.Descendents(MP4DocType.Track).ToArray();

				foreach (var trackBox in tracksBox) PopulateTrack(trackBox);
			}

			private void PopulateTrack(MP4Node track) {
				var trackHeaderBox = new TrackHeaderBox(track.Descendents(MP4DocType.TrackHeader).Single().Data.Span);
				var mediaHeaderBox = new MediaHeaderBox(track.Descendents(MP4DocType.MediaHeader).Single().Data.Span);
				var sampleDescriptionBox = new SampleDescriptionBox(track.Descendents(MP4DocType.SampleDescription).Single().Data.Span);

				var handlerBox = new HandlerBox(track.Descendents(MP4DocType.Handler).Single().Data.Span);
				switch (MP4DocType.KeyToString(handlerBox.HandlerType)) {
					case "vide":
						PopulateVideoTrack(track, in trackHeaderBox, in mediaHeaderBox, in sampleDescriptionBox);
						break;

					case "soun":
						break;

					case "hint":
						break;
				}

			}



			private class TrackData {
				public SampleToIndex[] SampleToIndexMap;
				public uint[] SampleSizes;
				public SampleToChunkBox.SampleToChunkItem[] SampleToChunkItems;
				public CompositionOffsetBox.SampleItem[] CompositionOffsetItems;
				public TimeToSampleBox.SampleItem[] TimeToSampleItems;

			}
			private struct SampleToIndex {
				public uint ChunkIndex;
				public uint SampleDescriptionIndex;
				public uint SampleToChunkIndex;
				public uint CompositionOffsetIndex;
				public uint TimeToSampleIndex;
			}

			private void PopulateVideoTrack(MP4Node trackNode, in TrackHeaderBox trackHeaderBox, in MediaHeaderBox mediaHeaderBox, in SampleDescriptionBox sampleDescriptionBox) {
				var trackData = BuildTrackData(trackNode);


				var frameCountPerSample = new int[sampleDescriptionBox.EntryCount];
				for (int i = 0; i < sampleDescriptionBox.EntryCount; i++) {
					var visualSampleEntry = sampleDescriptionBox.GetVideoEntry(i);
					frameCountPerSample[i] = visualSampleEntry.FrameCount;

				}


			}

			private static TrackData BuildTrackData(MP4Node trackNode) {
				var trackData = new TrackData();

				var sampleSizeNode = trackNode.Descendents(MP4DocType.SampleSize).FirstOrDefault();
				if (sampleSizeNode != null) {
					var sampleSizeBox = new SampleSizeBox(sampleSizeNode.Data.Span);
					trackData.SampleToIndexMap = new SampleToIndex[sampleSizeBox.Samples.Length];

					if (sampleSizeBox.SampleSize == 0) {
						trackData.SampleSizes = sampleSizeBox.Samples.ToArray();
					} else {

						var sampleSize = sampleSizeBox.SampleSize;
						trackData.SampleSizes = new uint[trackData.SampleSizes.Length];
						for (int i = 0; i < trackData.SampleSizes.Length; i++) {
							trackData.SampleSizes[i] = sampleSize;
						}
					}
				}

				var compactSampleSizeNode = trackNode.Descendents(MP4DocType.CompactSampleSize).FirstOrDefault();
				if (sampleSizeNode == null && compactSampleSizeNode != null) {
					var compactSampleSizeBox = new CompactSampleSizeBox(compactSampleSizeNode.Data.Span);
					trackData.SampleToIndexMap = new SampleToIndex[compactSampleSizeBox.Samples.Length];

					trackData.SampleSizes = new uint[compactSampleSizeBox.Samples.Length];
					for (int i = 0; i < trackData.SampleSizes.Length; i++) {
						trackData.SampleSizes[i] = compactSampleSizeBox.Samples[i];
					}
				}


				if (trackData.SampleToIndexMap != null) {
					var sampleToChunkNode = trackNode.Descendents(MP4DocType.SampleToChunk).FirstOrDefault();
					if (sampleToChunkNode != null) {
						var sampleSizeBox = new SampleToChunkBox(sampleToChunkNode.Data.Span);
						trackData.SampleToChunkItems = sampleSizeBox.Items.ToArray();



						var currentSampleIndex = 0;
						void setSampleToIndexItems(uint chunkCount, uint sampleToChunkIndex) {
							var sampleToChunk = trackData.SampleToChunkItems[sampleToChunkIndex];

							for (uint j = 0; j < chunkCount; j++) {
								for (int k = 0; k < sampleToChunk.SamplesPerChunk; k++) {
									ref var sampleToIndex = ref trackData.SampleToIndexMap[currentSampleIndex++];
									sampleToIndex.SampleDescriptionIndex = sampleToChunk.SampleDescriptionIndex;
									sampleToIndex.ChunkIndex = sampleToChunk.FirstChunk + j;
									sampleToIndex.SampleToChunkIndex = sampleToChunkIndex;
								}
							}
						}

						var curSampleToChunk = trackData.SampleToChunkItems[0];
						for (uint i = 1; i < trackData.SampleToChunkItems.Length; i++) {
							var nextSampleToChunk = trackData.SampleToChunkItems[i];

							var chunkCount = nextSampleToChunk.FirstChunk - curSampleToChunk.FirstChunk;
							setSampleToIndexItems(chunkCount, i - 1);
							curSampleToChunk = nextSampleToChunk;
						}
						setSampleToIndexItems(
							(uint)(trackData.SampleToIndexMap.Length - currentSampleIndex) / trackData.SampleToChunkItems.Last().SamplesPerChunk,
							(uint)trackData.SampleToChunkItems.Length - 1
						);
					}

					var compositionOffsetNode = trackNode.Descendents(MP4DocType.CompositionOffset).FirstOrDefault();
					if (compositionOffsetNode != null) {
						var compositionOffsetBox = new CompositionOffsetBox(compositionOffsetNode.Data.Span);
						trackData.CompositionOffsetItems = compositionOffsetBox.Samples.ToArray();

						var currentSampleIndex = 0;
						for (uint i = 0; i < trackData.CompositionOffsetItems.Length; i++) {
							var sampleItem = trackData.CompositionOffsetItems[i];

							for (int j = 0; j < sampleItem.Count; j++) {
								ref var sampleToIndex = ref trackData.SampleToIndexMap[currentSampleIndex++];
								sampleToIndex.CompositionOffsetIndex = i;
							}
						}
					}


					var timeToSampleNode = trackNode.Descendents(MP4DocType.TimeToSample).FirstOrDefault();
					if (timeToSampleNode != null) {
						var timeToSampleBox = new TimeToSampleBox(compositionOffsetNode.Data.Span);
						trackData.TimeToSampleItems = timeToSampleBox.Samples.ToArray();

						var currentSampleIndex = 0;
						for (uint i = 0; i < trackData.TimeToSampleItems.Length; i++) {
							var sampleItem = trackData.TimeToSampleItems[i];

							for (int j = 0; j < sampleItem.Count; j++) {
								ref var sampleToIndex = ref trackData.SampleToIndexMap[currentSampleIndex++];
								sampleToIndex.TimeToSampleIndex = i;
							}
						}
					}



				}

				return trackData;
			}
		}

	}
}
