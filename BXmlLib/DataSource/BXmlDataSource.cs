using System;

namespace BXmlLib.DataSource {
	public abstract class BXmlDataBlockSource : IBXmlDataSource {
		public abstract long Position { get; set; }
		public bool IsEndOfStream { get; protected set; }
		public abstract void CommitPosition();

		/// <summary>Gets a Chunk of data at least <paramref name="minLength"/> bytes long. Except when limited by the end of the file. In this case it returns the remaining bytes</summary>
		/// <param name="minLength">Minimum chunk length to return. See exception in summary.</param>
		/// <returns>A chunk of data with at least <paramref name="minLength"/> bytes. See exception in summary.</returns>
		protected abstract ReadOnlySpan<byte> GetDataBlock(int minLength);
		protected abstract void Advance(int length);


		public ReadOnlySpan<byte> ReadData(int bytesToRead) {
			var data = GetDataBlock(bytesToRead);

			if (bytesToRead <= data.Length) {
				Advance(bytesToRead);
				return data.Slice(0, bytesToRead);
			}

			var toRead = bytesToRead;
			var toAdvance = data.Length;

			Span<byte> returnData = new byte[bytesToRead];
			while (!IsEndOfStream && toRead != 0) {
				data.CopyTo(returnData.Slice(bytesToRead - toRead));
				CommitPosition();

				Advance(toAdvance);
				toRead -= toAdvance;

				data = GetDataBlock(toRead);
				toAdvance = Math.Min(toRead, data.Length);
			}
			Advance(toAdvance);

			//CommitPosition();
			return returnData;
		}

		public abstract ReadOnlySpan<byte> ReadIdentifier(ref BXmlElementHeader header);
	}
}
