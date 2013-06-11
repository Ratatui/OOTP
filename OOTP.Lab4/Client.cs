using OOTP.Lab4.Data;
using OOTP_Server;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OOTP.Lab4
{
	public class Client
	{
		ManualResetEvent allDone = new ManualResetEvent(false);
		ManualResetEvent connect = new ManualResetEvent(false);
		ManualResetEvent send = new ManualResetEvent(false);
		ManualResetEvent getting = new ManualResetEvent(false);

		Status respone;
		Socket sender;
		Status status;

		IPAddress ipaddr;

		public List<Agreement> Agreements { get; private set; }
		public List<Buyer> Buyers { get; private set; }
		public List<Controller> Controllers { get; private set; }
		public List<Organization> Organizations { get; private set; }

		public Client(string ip)
		{
			ipaddr = new IPAddress(0x5555);
			IPAddress.TryParse(ip, out ipaddr);

			this.Agreements = new List<Agreement>(this.GetAllAgreements());
			if (Agreements == null)
				throw new Exception("Not found a server");

			this.Buyers = new List<Buyer>(this.GetAllBuyers());
			if (Buyers == null)
				throw new Exception("Not found a server");

			this.Controllers = new List<Controller>(this.GetAllControllers());
			if (Controllers == null)
				throw new Exception("Not found a server");

			this.Organizations = new List<Organization>(this.GetAllOrganizations());
			if (Organizations == null)
				throw new Exception("Not found a server");
		}
		/// <summary>
		/// Starts the client and attempts to send an object to the server
		/// </summary>
		private bool Start(string message)
		{
			//Console.Out.WriteLine("Waiting for connection...");
			allDone.Reset();
			connect.Reset();
			send.Reset();
			sender = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
			sender.BeginConnect(new IPEndPoint(ipaddr, 1440), Connect, sender);
			connect.WaitOne();

			status.Socket = sender;
			//halts this thread until the connection is accepted
			try
			{
				status.msg = message;

				byte[] buffer = status.Serialize(); //fills the buffer with data

				status.Socket.BeginSend(buffer, 0, buffer.Length, SocketFlags.None, Send, status);
				send.WaitOne();

				//send, start reseive
				Status rcv = new Status();
				rcv.Socket = status.Socket;

				rcv.Socket.BeginReceive(rcv.buffer, 0, rcv.buffer.Length, 0, new AsyncCallback(Recive), rcv);
				allDone.WaitOne();

				return true;
			}
			catch (Exception e)
			{
				allDone.Set();
				return false;
			}
		}


		/// <summary>
		/// Starts when the connection was accepted by the remote hosts and prepares to send data
		/// </summary>
		private void Connect(IAsyncResult ar)
		{
			try
			{
				// Retrieve the socket from the state object.
				Socket client = (Socket)ar.AsyncState;

				// Complete the connection.
				client.EndConnect(ar);

				// Signal that the connection has been made.
				connect.Set();
			}
			catch (Exception e)
			{
				connect.Set();
				getting.Set();
				Console.WriteLine(e.ToString());
			}

		}

		/// <summary>
		/// Ends sending the data, waits for a readline until the thread quits
		/// </summary>
		private void Send(IAsyncResult result)
		{
			Status status = (Status)result.AsyncState;
			int size = status.Socket.EndSend(result);

			send.Set(); //signals thread to continue so it sends another message
		}

		private void Recive(IAsyncResult result)
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
						status.Socket.BeginReceive(status.buffer, 0, status.buffer.Length, SocketFlags.None, Recive, status);
					}
					else
					{
						this.respone = status;
						try
						{
							status.DeSerialize();
							Done(status);
						}
						catch
						{
							status.Socket.BeginReceive(status.buffer, 0, status.buffer.Length, SocketFlags.None, Recive, status);
						}
					}
				}
				else
				{
					this.respone = status;
					Done(status);
				}
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
				allDone.Set();
			}
		}

		private void Done(Status status)
		{

			allDone.Set(); //signals thread to continue 
			getting.Set();
			//So it jumps back to the first while loop and starts waiting for a connection again.
		}

		#region Select

		private List<Agreement> GetAllAgreements()
		{
			status = new Status();
			getting.Reset();
			if (this.Start("select agreements") == true)
			{
				getting.WaitOne();
				Status send = respone.DeSerialize();

				List<Agreement> r;
				var serq = new BinaryFormatter();

				using (var ms = new MemoryStream(send.content))
				{
					r = (List<Agreement>)serq.Deserialize(ms);
				}

				sender.Close();
				return r;
			}
			else
			{
				return null;
			}
		}

		private List<Controller> GetAllControllers()
		{
			status = new Status();
			getting.Reset();

			if (this.Start("select controllers") == true)
			{
				getting.WaitOne();
				Status send = respone.DeSerialize();

				List<Controller> r;
				var serq = new BinaryFormatter();

				using (var ms = new MemoryStream(send.content))
				{
					r = (List<Controller>)serq.Deserialize(ms);
				}

				sender.Close();
				return r;
			}
			return null;
		}

		private List<Organization> GetAllOrganizations()
		{
			status = new Status();
			getting.Reset();

			if (this.Start("select organizations") == true)
			{
				getting.WaitOne();
				Status send = respone.DeSerialize();

				List<Organization> r;
				var serq = new BinaryFormatter();

				using (var ms = new MemoryStream(send.content))
				{
					r = (List<Organization>)serq.Deserialize(ms);
				}

				sender.Close();
				return r;
			}
			return null;
		}

		private List<Buyer> GetAllBuyers()
		{
			status = new Status();
			getting.Reset();

			if (this.Start("select buyers") == true)
			{
				getting.WaitOne();
				Status send = respone.DeSerialize();


				List<Buyer> r;
				var serq = new BinaryFormatter();

				using (var ms = new MemoryStream(send.content))
				{
					r = (List<Buyer>)serq.Deserialize(ms);
				}

				sender.Close();
				return r;
			}
			return null;
		}

		#endregion

		#region Add

		public Agreement AddAgreement(Agreement agreement)
		{
			status = new Status();
			getting.Reset();
			var ser = new BinaryFormatter();
			using (var ms = new MemoryStream())
			{
				ser.Serialize(ms, agreement);
				status.content = ms.ToArray();
			}
			if (this.Start("insert agreement") == true)
			{
				getting.WaitOne();
				Status send = respone.DeSerialize();

				Agreement r;

				using (var ms = new MemoryStream(send.content))
				{
					r = (Agreement)ser.Deserialize(ms);
				}

				sender.Close();
				this.Agreements.Add(r);
				return r;
			}
			return null;
		}

		public Organization AddOrganization(Organization organization)
		{
			status = new Status();
			getting.Reset();
			var ser = new BinaryFormatter();
			using (var ms = new MemoryStream())
			{
				ser.Serialize(ms, organization);
				status.content = ms.ToArray();
			}
			if (this.Start("insert organization") == true)
			{
				getting.WaitOne();
				Status send = respone.DeSerialize();

				Organization r;

				using (var ms = new MemoryStream(send.content))
				{
					r = (Organization)ser.Deserialize(ms);
				}

				sender.Close();
				this.Organizations.Add(r);
				return r;
			}
			return null;
		}

		public Controller AddController(Controller controller)
		{
			status = new Status();
			getting.Reset();
			var ser = new BinaryFormatter();
			using (var ms = new MemoryStream())
			{
				ser.Serialize(ms, controller);
				status.content = ms.ToArray();
			}
			if (this.Start("insert controller") == true)
			{
				getting.WaitOne();
				Status send = respone.DeSerialize();

				Controller r;

				using (var ms = new MemoryStream(send.content))
				{
					r = (Controller)ser.Deserialize(ms);
				}

				sender.Close();
				this.Controllers.Add(r);
				return r;
			}
			return null;
		}

		public Buyer AddBuyer(Buyer buyer)
		{
			status = new Status();
			getting.Reset();
			var ser = new BinaryFormatter();
			using (var ms = new MemoryStream())
			{
				ser.Serialize(ms, buyer);
				status.content = ms.ToArray();
			}
			if (this.Start("insert buyer") == true)
			{
				getting.WaitOne();
				Status send = respone.DeSerialize();
				//Console.Out.WriteLine("Received: " + send.msg);

				Buyer r;

				using (var ms = new MemoryStream(send.content))
				{
					r = (Buyer)ser.Deserialize(ms);
				}

				sender.Close();
				this.Buyers.Add(r);
				return r;
			}
			return null;
		}

		#endregion

		#region Delete
		//public string RemoveTown(Town town)
		//{
		//	status = new Status();
		//	getting.Reset();
		//	var ser = new BinaryFormatter();
		//	using (var ms = new MemoryStream())
		//	{
		//		ser.Serialize(ms, town);
		//		status.content = ms.ToArray();
		//	}
		//	if (this.Start("delete town") == true)
		//	{
		//		getting.WaitOne();
		//		Status send = respone.DeSerialize();
		//		//Console.Out.WriteLine("Received: " + send.msg);
		//		sender.Close();
		//		Town removed = Towns.First(x => x.Id == town.Id);
		//		Towns.Remove(removed);
		//		this.Roads = new List<Road>(this.GetAllRoads());
		//		return send.msg;

		//	}
		//	return "ERROR";
		//}
		//public string RemoveRoad(Road road)
		//{
		//	status = new Status();
		//	getting.Reset();
		//	var ser = new BinaryFormatter();
		//	using (var ms = new MemoryStream())
		//	{
		//		ser.Serialize(ms, road);
		//		status.content = ms.ToArray();
		//	}
		//	if (this.Start("delete road") == true)
		//	{
		//		getting.WaitOne();
		//		Status send = respone.DeSerialize();
		//		//Console.Out.WriteLine("Received: " + send.msg);
		//		sender.Close();
		//		Roads.Remove(road);
		//		return send.msg;

		//	}
		//	return "ERROR";
		//}
		#endregion

		#region Update
		//public Road UpdateRoad(Road road)
		//{
		//	status = new Status();
		//	getting.Reset();
		//	var ser = new BinaryFormatter();
		//	using (var ms = new MemoryStream())
		//	{
		//		ser.Serialize(ms, road);
		//		status.content = ms.ToArray();
		//	}
		//	if (this.Start("update road") == true)
		//	{
		//		getting.WaitOne();
		//		Status send = respone.DeSerialize();
		//		//Console.Out.WriteLine("Received: " + send.msg);

		//		Road r;

		//		using (var ms = new MemoryStream(send.content))
		//		{
		//			r = (Road)ser.Deserialize(ms);
		//		}

		//		sender.Close();
		//		Roads.Remove(Roads.First(x => x.Id == r.Id));
		//		this.Roads.Add(r);
		//		return r;
		//	}
		//	return null;
		//}
		//public Town UpdateTown(Town town)
		//{
		//	status = new Status();
		//	getting.Reset();
		//	var ser = new BinaryFormatter();
		//	using (var ms = new MemoryStream())
		//	{
		//		ser.Serialize(ms, town);
		//		status.content = ms.ToArray();
		//	}
		//	if (this.Start("update town") == true)
		//	{
		//		getting.WaitOne();
		//		Status send = respone.DeSerialize();
		//		//Console.Out.WriteLine("Received: " + send.msg);

		//		Town t;

		//		using (var ms = new MemoryStream(send.content))
		//		{
		//			t = (Town)ser.Deserialize(ms);
		//		}

		//		sender.Close();
		//		Towns.Remove(Towns.First(x => x.Id == t.Id));
		//		this.Towns.Add(t);
		//		this.Roads = new List<Road>(this.GetAllRoads());
		//		return t;
		//	}
		//	return null;
		//}
		#endregion
	}
}
