using BXmlLib.DataSource;
using BXmlLib.DocType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BXmlLib {
    public class BXmlReader : IBXmlReader {
        public class PositionData {
            internal long NextElementPos, LastElementPos;
            public BXmlDocElement DocElement { get; internal set; }
        }

        private PositionData[] positions = Enumerable.Range(0, 32).Select(x => new PositionData()).ToArray();
        private int currentPositionIndex;

        //private long nextElementPos, lastElementPos;

        public BXmlReader(IBXmlDataSource dataSrc, IBXmlDocType bxmlDocType) {
            BaseStream = dataSrc;
            DocType = bxmlDocType;

            enterDisposable = new EnterDisposable(this);
            positions[0].LastElementPos = ~BXmlElementHeader.UnknownLength;
        }

        public IBXmlDataSource BaseStream { get; }
        public IBXmlDocType DocType { get; }
        public BXmlElementHeader Header => header; private BXmlElementHeader header;
        public BXmlDocElement DocElement => positions[currentPositionIndex].DocElement;

        public object RetrieveValue() => DocType.DecodeData(positions[currentPositionIndex].DocElement, RetrieveRawValue());
        public ReadOnlySpan<byte> RetrieveRawValue() {
            if(header.DataLength < 0) throw new InvalidOperationException("Cannot retrieve value: Length unknown");
            if(header.DataLength == 0) return Array.Empty<byte>();

            if(BaseStream.Position != header.DataOffset) throw new InvalidOperationException("Cannot retrieve value: Current position doesn't match element data position");

            return BaseStream.ReadData(checked((int)header.DataLength));
        }

        public bool Strict { get; set; }

        //public void Reset() {
        //	BaseStream.Position = nextElementPos = 0;
        //	lastElementPos = BaseStream.HasKnownLength ? BaseStream.Length : ~VIntConsts.UNKNOWN_LENGTH;
        //}
        //public ElementInfo JumpToElementAt(Int64 elemPos) {
        //	BaseStream.Position = elemPos;
        //	nextElementPos = elemPos;
        //	lastElementPos = BaseStream.HasKnownLength ? BaseStream.Length : ~VIntConsts.UNKNOWN_LENGTH;

        //	return Next();
        //}


        public bool Next() {
            BaseStream.CommitPosition();

            var pos = positions[currentPositionIndex];
            header.Mutate();
            pos.DocElement = null;

            if(
                (pos.LastElementPos != ~BXmlElementHeader.UnknownLength && pos.NextElementPos >= pos.LastElementPos) ||
                pos.NextElementPos == ~BXmlElementHeader.UnknownLength ||
                BaseStream.IsEndOfStream
            ) return false;

            if(BaseStream.Position != pos.NextElementPos) BaseStream.Position = pos.NextElementPos;

            var encodedIdentifier = BaseStream.ReadIdentifier(ref header);
            var isHeaderInvalid =
                (header.DataLength < 0 && (~header.DataLength & BXmlElementHeader.Mask & BXmlElementHeader.Error) != 0) ||
                (pos.LastElementPos != ~BXmlElementHeader.UnknownLength && header.DataOffset + header.DataLength > pos.LastElementPos);


            if(isHeaderInvalid) {
                if(Strict) {
                    throw new InvalidOperationException("Invalid data in strict mode");
                }

                header.Mutate(pos.NextElementPos, pos.NextElementPos, 0);
                pos.DocElement = DocType.GetDocElement(encodedIdentifier, ref header, positions);
                pos.NextElementPos = BaseStream.Position;
                //BaseStream.SyncTo(bytePatterns, -1);

                return true;
            }


            pos.DocElement = DocType.GetDocElement(encodedIdentifier, ref header, positions);
            pos.NextElementPos = header.DataLength < 0 ? -1 : header.DataOffset + header.DataLength;

            //Trace.WriteLine(elemInfo.ToDetailedString());
            return true;
        }

        private readonly EnterDisposable enterDisposable;


        public IDisposable EnterElement() {
            currentPositionIndex++;
            if(positions.Length == currentPositionIndex) {
                Array.Resize(ref positions, positions.Length * 2);
                for(int i = positions.Length / 2; i < positions.Length; i++) positions[i] = new PositionData();
            }

            var pos = positions[currentPositionIndex];
            pos.NextElementPos = header.DataOffset;
            pos.LastElementPos = pos.NextElementPos + header.DataLength;
            pos.DocElement = null;

            header.Mutate();
            return enterDisposable;
        }


        protected sealed class EnterDisposable : IDisposable {
            private readonly BXmlReader reader;
            public EnterDisposable(BXmlReader reader) { this.reader = reader; }

            public void Dispose() {
                var pos = reader.positions[--reader.currentPositionIndex];
                reader.BaseStream.Position = pos.NextElementPos;
                pos.DocElement = null;
            }
        }
    }
}
