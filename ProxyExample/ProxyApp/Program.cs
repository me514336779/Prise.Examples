using System;
using Prise.Proxy;
using ProxyLib;

namespace ProxyApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var foreignCalculator = new Calculator();

            var iCalc = ProxyCreator.CreateProxy<ICalculator>(foreignCalculator);
            var iCalc2 = ProxyCreator.CreateGenericProxy(typeof(ICalculator), foreignCalculator) as ICalculator;

            Console.WriteLine(iCalc.Add(10, 20));
            Console.WriteLine(iCalc2.Add(10, 20));
            Console.Read();
        }
    }
}
