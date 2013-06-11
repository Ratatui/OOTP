using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace OOTP_Server
{
	[Serializable]
	public class Status
	{
		[NonSerialized]
		public Socket Socket;
		[NonSerialized]
		public List<byte> TransmissionBuffer = new List<byte>();
		[NonSerialized]
		public byte[] buffer = new byte[1024];

		public byte[] content;
		public string msg;


		public byte[] Serialize()
		{
			BinaryFormatter bin = new BinaryFormatter();
			MemoryStream mem = new MemoryStream();
			bin.Serialize(mem, this);
			return mem.GetBuffer();
		}

		public Status DeSerialize()
		{
			byte[] dataBuffer = TransmissionBuffer.ToArray();
			BinaryFormatter bin = new BinaryFormatter();
			MemoryStream mem = new MemoryStream();
			mem.Write(dataBuffer, 0, dataBuffer.Length);
			mem.Seek(0, 0);
			return (Status)bin.Deserialize(mem);
		}
	}
}
