using OOTP.Lab4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestClient
{
	class Program
	{
		static void Main(string[] args)
		{
			var client = new Client();
			client.Start();
			client.Send(null);
		}
	}
}
