using NModbus;
using System.Net.Sockets;
using System;
using System.Net;
using System.Windows.Controls;
using System.Windows.Media;

namespace suitcaseV2.func
{
    internal class modbuscontroller
    {
        public class Read4012Result
        {
            public bool ON { get; set; }
            public bool OFF { get; set; }
            public bool CONVEYOR { get; set; }
            public ushort DS { get; set; }



        }


        public Read4012Result read4012(TextBlock W4012EBlock)
        {
            Read4012Result result = new Read4012Result();

            try
            {
                using (var client = new TcpClient(Config.data.IP_4012E, Config.data.PORT_MOD))
                {
                    var factory = new ModbusFactory();
                    IModbusMaster master = factory.CreateMaster(client);
                    bool[] coils = master.ReadCoils(0, 0, 17);
                    ushort[] register = master.ReadInputRegisters(0, 0, 1);
                    result.ON = coils[0];
                    result.OFF = coils[1];
                    result.CONVEYOR = coils[16];
                    result.DS = register[0];
                }
            }
            catch (Exception ex)
            {
                W4012EBlock.Text = "Connection Failed";
                W4012EBlock.Foreground = Brushes.Red;
                return result;
            }
            W4012EBlock.Text = "Connected";
            W4012EBlock.Foreground = Brushes.Green;
            return result;
        }
        public void Write4012(bool input,TextBlock W4012EBlock)
        {
            try
            {
                using (var client = new TcpClient(Config.data.IP_4012E, Config.data.PORT_MOD))
                {
                    var factory = new ModbusFactory();
                    IModbusMaster master = factory.CreateMaster(client);
                    master.WriteSingleCoil(0, 16, input);
                }
            }
            catch (Exception ex)
            {
                W4012EBlock.Text = "Connection Failed";
                W4012EBlock.Foreground = Brushes.Red;
            }
            W4012EBlock.Text = "Connected";
            W4012EBlock.Foreground = Brushes.Green;


        }
        public void Write4060(string input, TextBlock W4012EBlock)
        {
            try
            {
                using (var client = new TcpClient(Config.data.IP_4060, Config.data.PORT_MOD))
                {
                    var factory = new ModbusFactory();
                    IModbusMaster master = factory.CreateMaster(client);
                    switch (input)
                    {
                        case "start":
                            master.WriteMultipleCoils(0, 17, new bool[3] { false, false, true });
                            break;
                        case "stop":
                            master.WriteMultipleCoils(0, 17, new bool[3] { true, false, false });
                            break;
                        case "halt":
                            master.WriteMultipleCoils(0, 17, new bool[3] { false, true, false });
                            break;
                    }
                }
            }
            catch(Exception ex)
            {
                W4012EBlock.Text = "Connection Failed";
                W4012EBlock.Foreground = Brushes.Red;

            }
            W4012EBlock.Text = "Connected";
            W4012EBlock.Foreground = Brushes.Green;

        }
    }
}
