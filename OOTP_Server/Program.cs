using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOTP_Server
{
	class Program
	{
		static void Main(string[] args)
		{
			var server = new Server();
			server.Start();
			server.Receive(null);
		}
	}
}
