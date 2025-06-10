using System.Collections;

namespace SharpTestBench.Benches;

public static class FactoryMethod_Bench
{
    public static void Test()
    {
        VehicleFactory[] vehicles = [new BikeFactory(), new CarFactory(), new TruckFactory()];
        foreach (var factory in vehicles)
        {
            factory.Process();
        }
    }
}

public abstract class VehicleFactory
{
    protected abstract IVehicle Vehicle();
    
    public void Process()
    {
        IVehicle vehicle = Vehicle();
        bool isStared = vehicle.Start();
        bool isBreakWork = vehicle.Break();
        string vehicleType = vehicle.GetVehicleType();

        switch (isStared)
        {
            case true when isBreakWork:
                Console.WriteLine($"{vehicle.GetVehicleType()} was Started and Break working properly, so it is ready to go from Showroom.");
                break;
            case false:
                Console.WriteLine($"{vehicleType} : Failed To Start!");
                break;
        }

        if(!isBreakWork) Console.WriteLine($"{vehicleType} : Break Not Working!");
        if(isStared) vehicle.Stop();
    }
}

public interface IVehicle
{
    bool IsVehicleStarted { get; set; }
    bool Start();
    bool Break();
    void Stop();
    string GetVehicleType();
}

public class BikeFactory : VehicleFactory
{
    protected override IVehicle Vehicle()
    {
        return new Bike();
    }
}

public class CarFactory : VehicleFactory
{
    protected override IVehicle Vehicle()
    {
        return new Car();
    }
}

public class TruckFactory : VehicleFactory
{
    protected override IVehicle Vehicle()
    {
        return new Truck();
    }
}

public class Bike : IVehicle
{
    private static readonly Random Random = new Random();

    public bool IsVehicleStarted { get; set; }

    public bool Start()
    {
        IsVehicleStarted = Random.Next(0, 10) >= 5;
        return IsVehicleStarted;
    }

    public bool Break()
    {
        return Random.Next(0, 10) >= 5;
    }

    public void Stop()
    {
        Console.WriteLine(IsVehicleStarted ? $"{nameof(Bike)} was Stoped Successfully" : $"{nameof(Bike)} is not Started, So Noting to Stop");
    }

    public string GetVehicleType()
    {
        return nameof(Bike);
    }
}

public class Car : IVehicle
{
    private static readonly Random Random = new Random();

    public bool IsVehicleStarted { get; set; }

    public bool Start()
    {
        IsVehicleStarted = Random.Next(0, 10) >= 5;
        return IsVehicleStarted;
    }

    public bool Break()
    {
        return Random.Next(0, 10) >= 5;
    }

    public void Stop()
    {
        Console.WriteLine(IsVehicleStarted ? $"{nameof(Car)} was Stoped Successfully" : $"{nameof(Car)} is not Started, So Noting to Stop");
    }

    public string GetVehicleType()
    {
        return nameof(Car);
    }
}

public class Truck : IVehicle
{
    private static readonly Random Random = new Random();

    public bool IsVehicleStarted { get; set; }

    public bool Start()
    {
        IsVehicleStarted = Random.Next(0, 10) >= 5;
        return IsVehicleStarted;
    }

    public bool Break()
    {
        return Random.Next(0, 10) >= 5;
    }

    public void Stop()
    {
        Console.WriteLine(IsVehicleStarted ? $"{nameof(Truck)} was Stoped Successfully" : $"{nameof(Truck)} is not Started, So Noting to Stop");
    }

    public string GetVehicleType()
    {
        return nameof(Truck);
    }
}