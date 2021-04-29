using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace CompEquip
{
    class ConnectionManager
    {
        private static TcpClient client = new TcpClient();
        private static BinaryReader reader;
        private static BinaryWriter writer;

        public ConnectionManager()
        {
        }

        public void Connect(string ipAddress,int port)
        {
            try
            {
                client.Connect(ipAddress, port);

                reader = new BinaryReader(client.GetStream());
                writer = new BinaryWriter(client.GetStream());
            }

            catch(Exception ex)
            {
                throw new Exception("Error 0x00000000:" + ex.Message);
            }
           
        }
       
        public DataTable Select(string query)
        {
            //создание покета данных
            Packet outPacket = new Packet(OPERATION.Select, false, query);
            //передача серверу json строки
            writer.Write(JsonConvert.SerializeObject(outPacket));

            DataTable table = new DataTable();

            string json = reader.ReadString();

            Packet packet = JsonConvert.DeserializeObject<Packet>(json);

            //проверка на ошибки
            if (!packet.IsError)
            {
                table = JsonConvert.DeserializeObject<DataTable>(packet.Value);
            }
            else
            {
                throw new Exception(packet.Value);
            }

            return table;
        }

        public void Remove(string query)
        {
            Packet outPacket = new Packet(OPERATION.Delete, false, query);
            writer.Write(JsonConvert.SerializeObject(outPacket));

            string json = reader.ReadString();

            Packet packet = JsonConvert.DeserializeObject<Packet>(json);

            if (packet.IsError)
            {
                throw new Exception(packet.Value);
            }
        }

        internal void Add(string query)
        {
            Packet outPacket = new Packet(OPERATION.Add, false, query);
            writer.Write(JsonConvert.SerializeObject(outPacket));

            string json = reader.ReadString();

            Packet packet = JsonConvert.DeserializeObject<Packet>(json);

            if (packet.IsError)
            {
                throw new Exception(packet.Value);
            }
        }

        internal void Update(string query)
        {
            //создание покета данных
            Packet outPacket = new Packet(OPERATION.Update, false, query);
            //передача серверу json строки
            writer.Write(JsonConvert.SerializeObject(outPacket));
            //получение json строки объекта
            string json = reader.ReadString();
            //обработка json строки
            Packet packet = JsonConvert.DeserializeObject<Packet>(json);

            if (packet.IsError)
            {
                throw new Exception(packet.Value);
            }
        }
    }
}
