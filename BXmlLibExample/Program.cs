using BXmlLib;
using BXmlLib.DocTypes.Ebml;
using BXmlLib.DocTypes.Matroska;
using System;
using System.IO;
using System.Threading;

namespace BXmlLibExample {
    public class EbmlBlockDataSourceExample : EbmlBlockDataSource {
        private byte[] data;
        private int offset;

        public EbmlBlockDataSourceExample(byte[] data) => this.data = data;

        public override long Position {
            get => offset;
            set {
                IsEndOfStream = !Advance((int)value - offset);
                offset = (int)value;
            }
        }

        protected override bool Advance(int length) { offset += length; return (int)Position < data.Length; }
        protected override ReadOnlySpan<byte> Data(int minLength) => new ReadOnlySpan<byte>(data).Slice(offset);
    }

    public class Program {
        private static void Main(string[] args) {
            var path = args[0];
            var ebmlDataSource = new EbmlBlockDataSourceExample(File.ReadAllBytes(path));
            var mkvDocType = new MatroskaDocType();


            var reader = new BXmlReader(ebmlDataSource, mkvDocType);

            //if(reader.DocElement.Name.Equals("DocTypeReadVersion")) {
            //    var asdfgh = 536;
            //}

            void Traverse(int depth) {
                while(reader.Next()) {
                    Console.WriteLine("".PadLeft(depth) + reader.DocElement.Name + " #" + reader.Header.Offset);
                    if(reader.DocElement.IsContainer) {
                        using(reader.EnterElement()) {
                            Traverse(depth + 1);
                        }
                    }
                }
            }

            Traverse(0);
        }
    }
}
