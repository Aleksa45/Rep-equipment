using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using System.Data;
using Newtonsoft.Json;
using System.Data.SqlClient;

namespace Server
{
    class ClientThread
    {
        private Thread _thread;
        private TcpClient _client;
        private BinaryReader _reader;
        private BinaryWriter _writer;
        public ClientThread(TcpClient client)
        {
            _client = client;
            _thread = new Thread(new ThreadStart(_Work));
            Stream stream = client.GetStream();
            _reader = new BinaryReader(stream);
            _writer = new BinaryWriter(stream);
        }

        public void Start()
        {
            _thread.Start();

        }

        private void _Work()
        {
            Console.WriteLine("Start client thread");
            try
            {
                while (true)
                {
                    //получает json строку от клиента
                    string json = _reader.ReadString();
                    Packet inputPacket = JsonConvert.DeserializeObject<Packet>(json);

                    Console.Write($"operation '{inputPacket.Operation}': ");
                    Console.WriteLine(inputPacket.Value);

                    Packet outPacket = null;

                    switch (inputPacket.Operation)
                    {
                        case OPERATION.Select:
                            {
                                DataSet dst = new DataSet();

                                SqlDataAdapter adapter = new SqlDataAdapter(inputPacket.Value, Program.connectionString);

                                try
                                {
                                    adapter.Fill(dst);
                                    DataTable table = dst.Tables[0];
                                    table.TableName = "table";

                                    outPacket = new Packet(inputPacket.Operation, false, JsonConvert.SerializeObject(table));

                                }
                                catch(Exception e)
                                {
                                    outPacket = new Packet(inputPacket.Operation, true, "Error 0x00000001:" + e.Message);
                                }

                                break;
                            }
                        case OPERATION.Delete:
                            {
                                try
                                {
                                    SqlCommand sqlcom = new SqlCommand(inputPacket.Value, new SqlConnection(Program.connectionString));
                                    sqlcom.Connection.Open();
                                    sqlcom.ExecuteNonQuery();
                                    sqlcom.Connection.Close();

                                    outPacket = new Packet(inputPacket.Operation, false, "");
                                }
                                catch (Exception e)
                                {
                                    outPacket = new Packet(inputPacket.Operation, true, "Error 0x00000002:" + e.Message);
                                }

                                break;
                            }
                        case OPERATION.Add:
                            {
                                try
                                {
                                    SqlCommand sqlcom = new SqlCommand(inputPacket.Value, new SqlConnection(Program.connectionString));
                                    sqlcom.Connection.Open();
                                    sqlcom.ExecuteNonQuery();
                                    sqlcom.Connection.Close();

                                    outPacket = new Packet(inputPacket.Operation, false, "");
                                }
                                catch (Exception e)
                                {
                                    outPacket = new Packet(inputPacket.Operation, true, "Error 0x00000003:" + e.Message);
                                }

                                break;
                            }
                        case OPERATION.Update:
                            {
                                try
                                {
                                    SqlCommand sqlcom = new SqlCommand(inputPacket.Value, new SqlConnection(Program.connectionString));
                                    sqlcom.Connection.Open();
                                    sqlcom.ExecuteNonQuery();
                                    sqlcom.Connection.Close();

                                    outPacket = new Packet(inputPacket.Operation, false, "");
                                }
                                catch (Exception e)
                                {
                                    outPacket = new Packet(inputPacket.Operation, true, "Error 0x00000004:" + e.Message);
                                }

                                break;
                            }
                    }

                    //отправка json строки клиенту
                    string outJSON = JsonConvert.SerializeObject(outPacket);

                    _writer.Write(outJSON);

                    Console.WriteLine("Sned Packet");
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine("Close client thread. Error:");
                Console.WriteLine(ex.ToString());
            }
           
        }
    }
}
