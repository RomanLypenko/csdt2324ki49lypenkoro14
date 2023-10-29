using System.IO.Ports;
 

string message;
SerialPort serialPort = new SerialPort("COM3", 9600);
serialPort.Open();
serialPort.DataReceived += (sender, e) =>
{
    if (sender is SerialPort port)
    {
        Console.WriteLine($"sender: {port.ReadExisting()}");
    }
    
};
for(int i=0;i<5;i++)
{
    message = Console.ReadLine();
    serialPort.Write(message);
}