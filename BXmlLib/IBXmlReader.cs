using System;
using BXmlLib.DataSource;
using BXmlLib.DocType;

namespace BXmlLib {
    public interface IBXmlReader {
        IBXmlDataSource BaseStream { get; }
        BXmlDocElement DocElement { get; }
        IBXmlDocType DocType { get; }
        BXmlElementHeader Header { get; }
        bool Strict { get; set; }

        IDisposable EnterElement();
        bool Next();
        object RetrieveValue();
        ReadOnlySpan<byte> RetrieveRawValue();
    }
}