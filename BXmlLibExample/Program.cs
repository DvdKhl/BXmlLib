using BXmlLib;
using BXmlLib.DocTypes.Ebml;
using BXmlLib.DocTypes.Matroska;
using BXmlLib.DocTypes.MP4;
using BXmlLib.DocTypes.MP4.Boxes;
using System;
using System.IO;
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


						} else {
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
	}
}
