using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net.Sockets;
using System.Net;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Xml.Serialization;
using OOTP.Lab4.Data;
using Mindscape.LightSpeed;

namespace OOTP_Server
{

	public class Server
	{
		StreamWriter f;
		ManualResetEvent allDone = new ManualResetEvent(false);
		string path = Environment.CurrentDirectory + "\\log.txt";
		List<string> log = new List<string>();

		protected LightSpeedContext<ModelUnitOfWork> context = new LightSpeedContext<ModelUnitOfWork>("Default");

		public Server()
		{
			WriteLog("Server started ");
		}

		private void WriteLog(string msg)
		{
			log.Add(msg + " " + DateTime.Now);
			Console.WriteLine(msg + " " + DateTime.Now);
		}

		private void Log()
		{
			f = new StreamWriter(path, true);
			foreach (var s in log)
			{
				f.WriteLine(s);
			}
			f.Close();
		}

		/// <summary>
		/// Starts a server that listens to connections
		/// </summary>
		public void Start()
		{
			Socket listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
			listener.Bind(new IPEndPoint(IPAddress.Any, 1440));
			while (true)
			{
				WriteLog("Waiting for connection...");
				allDone.Reset();
				listener.Listen(100);

				listener.BeginAccept(Accept, listener);
				allDone.WaitOne(); //halts this thread
			}
		}

		/// <summary>
		/// Starts when an incomming connection was requested
		/// </summary>            
		public void Accept(IAsyncResult result)
		{
			Status status = new Status();
			try
			{
				allDone.Set();

				status.Socket = ((Socket)result.AsyncState).EndAccept(result);
				WriteLog(status.Socket.RemoteEndPoint.ToString() + " connected");

				status.Socket.BeginReceive(status.buffer, 0, status.buffer.Length, SocketFlags.None, Receive, status);
			}
			catch (Exception e)
			{
				WriteLog(e.Message);
				Done(status);
				allDone.Set();
			}
		}

		/// <summary>
		/// Receives the data, puts it in a buffer and checks if we need to receive again.
		/// </summary>            
		public void Receive(IAsyncResult result)
		{
			Status status = (Status)result.AsyncState;
			try
			{
				int read = status.Socket.EndReceive(result);
				if (read > 0)
				{
					for (int i = 0; i < read; i++)
					{
						status.TransmissionBuffer.Add(status.buffer[i]);
					}

					//we need to read again if this is true
					if (read == status.buffer.Length)
					{
						status.Socket.BeginReceive(status.buffer, 0, status.buffer.Length, SocketFlags.None, Receive, status);
					}
					else
					{
						try
						{
							status.DeSerialize();
							Done(status);
						}
						catch
						{
							status.Socket.BeginReceive(status.buffer, 0, status.buffer.Length, SocketFlags.None, Receive, status);
						}
					}
				}
				else
				{
					Done(status);
				}
			}
			catch (Exception e)
			{
				WriteLog(e.Message);
				Done(status);
				allDone.Set();
			}
		}

		/// <summary>
		/// Deserializes and outputs the received object
		/// </summary>            
		public void Done(Status status)
		{
			Status send = status.DeSerialize();
			WriteLog("Received: " + send.msg + " from " + status.Socket.RemoteEndPoint.ToString());

			string[] commands = send.msg.Split(new char[] { ' ' });

			var uow = context.CreateUnitOfWork();

			List<Agreement> agreements = new List<Agreement>(uow.Agreements);
			List<Controller> controllers = new List<Controller>(uow.Controllers);
			List<Buyer> buyers = new List<Buyer>(uow.Buyers);
			List<Organization> organizations = new List<Organization>(uow.Organizations);

			var ser = new BinaryFormatter();
			byte[] bytes = null;

			try
			{
				switch (commands[0])
				{
					case "select":
						{
							if (commands[1] == "agreements")
							{
								List<Agreement> r = new List<Agreement>(agreements);
								using (var ms = new MemoryStream())
								{
									ser.Serialize(ms, r);
									bytes = ms.ToArray();
								}
								WriteLog(send.msg + " OK");
								send.msg = "request agreements";
							}

							if (commands[1] == "buyers")
							{
								List<Buyer> r = new List<Buyer>(buyers);

								using (var ms = new MemoryStream())
								{
									ser.Serialize(ms, r);
									bytes = ms.ToArray();
								}
								WriteLog(send.msg + " OK");
								send.msg = "request buyers";

							}

							if (commands[1] == "controllers")
							{
								List<Controller> r = new List<Controller>(controllers);

								using (var ms = new MemoryStream())
								{
									ser.Serialize(ms, r);
									bytes = ms.ToArray();
								}
								WriteLog(send.msg + " OK");
								send.msg = "request controllers";
							}

							if (commands[1] == "organizations")
							{

								List<Organization> r = new List<Organization>(organizations);

								using (var ms = new MemoryStream())
								{
									ser.Serialize(ms, r);
									bytes = ms.ToArray();
								}
								WriteLog(send.msg + " OK");
								send.msg = "request organizations";
							}

							break;
						}
					case "insert":
						{
							if (commands[1] == "agreement")
							{
								Agreement a;
								var serq = new BinaryFormatter();

								using (var ms = new MemoryStream(send.content))
								{
									a = (Agreement)serq.Deserialize(ms);
								}

								uow.Context.
								
								uow.Add(a);
								uow.SaveChanges();

								using (var ms = new MemoryStream())
								{
									ser.Serialize(ms, a);
									bytes = ms.ToArray();
									send.content = bytes;
								}
								WriteLog(send.msg + " OK");
								send.msg = "OK";

							}
							if (commands[1] == "controller")
							{
								Controller c;
								var serq = new BinaryFormatter();

								using (var ms = new MemoryStream(send.content))
								{
									c = (Controller)serq.Deserialize(ms);
								}

								uow.Add(c);
								uow.SaveChanges();

								using (var ms = new MemoryStream())
								{
									ser.Serialize(ms, c);
									bytes = ms.ToArray();
									send.content = bytes;
								}
								WriteLog(send.msg + " OK");
								send.msg = "OK";
							}
							if (commands[1] == "organization")
							{
								Organization o;
								var serq = new BinaryFormatter();

								using (var ms = new MemoryStream(send.content))
								{
									o = (Organization)serq.Deserialize(ms);
								}

								uow.Add(o);
								uow.SaveChanges();
								using (var ms = new MemoryStream())
								{
									ser.Serialize(ms, o);
									bytes = ms.ToArray();
									send.content = bytes;
								}
								WriteLog(send.msg + " OK");
								send.msg = "OK";
							}

							if (commands[1] == "buyer")
							{
								Buyer b;
								var serq = new BinaryFormatter();

								using (var ms = new MemoryStream(send.content))
								{
									b = (Buyer)serq.Deserialize(ms);
								}

								uow.Add(b);
								uow.SaveChanges();
								using (var ms = new MemoryStream())
								{
									ser.Serialize(ms, b);
									bytes = ms.ToArray();
									send.content = bytes;
								}
								WriteLog(send.msg + " OK");
								send.msg = "OK";
							}
						}
						break;
					//}//insert
					//case "update":
					//	{
					//		if (commands[1] == "road")
					//		{
					//			Road r;
					//			var serq = new BinaryFormatter();

					//			using (var ms = new MemoryStream(send.content))
					//			{
					//				r = (Road)serq.Deserialize(ms);
					//			}
					//			var rts = from rt in _ctx.Routes
					//					  where rt.RoadId == r.Id
					//					  select rt;
					//			foreach (var ro in rts)
					//			{
					//				_ctx.Routes.Remove(ro);
					//			}
					//			List<Route> query = new List<Route>(r.Routes.Where(x => x.Town != null));
					//			int n = query.Count();
					//			for (int j = 0; j < n; j++)
					//			{
					//				r.Routes.Remove(query[0] as Route);
					//				query.RemoveAt(0);
					//			}

					//			_ctx.SaveChanges();
					//			Road ur = _ctx.Roads.First(x => x.Id == r.Id);

					//			ur.Index = r.Index;
					//			ur.Length = r.Length;
					//			ur.Name = r.Name;
					//			foreach (var rt in r.Routes)
					//			{
					//				ur.Routes.Add(new Route { TownId = rt.TownId, RoadId = rt.RoadId });
					//			}

					//			_ctx.SaveChanges();
					//			using (var ms = new MemoryStream())
					//			{
					//				ser.Serialize(ms, ur);
					//				bytes = ms.ToArray();
					//				send.content = bytes;
					//			}
					//			WriteLog(send.msg + " OK");
					//			send.msg = "OK";
					//		}
					//		if (commands[1] == "town")
					//		{
					//			Town t;
					//			var serq = new BinaryFormatter();

					//			using (var ms = new MemoryStream(send.content))
					//			{
					//				t = (Town)serq.Deserialize(ms);
					//			}

					//			Town ut = _ctx.Towns.First(x => x.Id == t.Id);
					//			ut.Inhabitants = t.Inhabitants;
					//			ut.Name = t.Name;
					//			ut.Photo = t.Photo;
					//			_ctx.SaveChanges();

					//			using (var ms = new MemoryStream())
					//			{
					//				ser.Serialize(ms, ut);
					//				bytes = ms.ToArray();
					//				send.content = bytes;
					//			}
					//			WriteLog(send.msg + " OK");
					//			send.msg = "OK";
					//		}
					//		break;
					//	}
					//case "delete":
					//	{
					//		if (commands[1] == "road")
					//		{
					//			Road r;
					//			var serq = new BinaryFormatter();

					//			using (var ms = new MemoryStream(send.content))
					//			{
					//				r = (Road)serq.Deserialize(ms);
					//			}

					//			var query = from rd in _ctx.Routes
					//						where rd.RoadId == r.Id
					//						select rd;
					//			foreach (var rd in query)
					//			{
					//				_ctx.Routes.Remove(rd);
					//			}
					//			var removed = _ctx.Roads.First(x => x.Id == r.Id);
					//			_ctx.Roads.Remove(removed);
					//			_ctx.SaveChanges();
					//			WriteLog(send.msg + " OK");
					//			send.msg = "OK";
					//		}
					//		if (commands[1] == "town")
					//		{
					//			Town t;
					//			var serq = new BinaryFormatter();

					//			using (var ms = new MemoryStream(send.content))
					//			{
					//				t = (Town)serq.Deserialize(ms);
					//			}
					//			var query = from r in _ctx.Routes
					//						where r.TownId == t.Id
					//						select r;
					//			foreach (var r in query)
					//			{
					//				_ctx.Routes.Remove(r);
					//			}
					//			var removed = _ctx.Towns.First(x => x.Id == t.Id);
					//			_ctx.Towns.Remove(removed);
					//			_ctx.SaveChanges();
					//			WriteLog(send.msg + " OK");
					//			send.msg = "OK";
					//		}
					//		break;
					//	}
				}
			}
			catch (Exception ex)
			{
				WriteLog(send.msg + " ERROR " + ex.Message);
				send.msg = send.msg + " ERROR " + ex.Message;
			}
			finally
			{
				send.Socket = status.Socket;
				send.content = bytes;

				byte[] data = send.Serialize();
				status.Socket.BeginSend(data, 0, data.Length, 0, new AsyncCallback(SendCallback), send);
			}

		}

		public void SendCallback(IAsyncResult ar)
		{
			try
			{
				// Retrieve the socket from the state object.
				Status status = (Status)ar.AsyncState;

				// Complete sending the data to the remote device.
				int bytesSent = status.Socket.EndSend(ar);
				WriteLog("Sent " + bytesSent.ToString() + " bytes to client");
			}
			catch (Exception e)
			{
				Console.WriteLine(e.ToString());
			}
			finally
			{
				allDone.Set();
				Log();
				this.log.Clear();
			}
		}
	}

}
