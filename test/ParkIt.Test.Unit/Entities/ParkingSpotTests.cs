using System;
using ParkIt.Core.Entities;
using ParkIt.Core.Enums;
using ParkIt.Core.Exceptions;
using ParkIt.Core.ValueObjects;
using Xunit;

namespace ParkIt.Test.Unit.Entities;

public class ParkingSpotTests
{

    [Fact]
    public void Reserving_parkingSpot_with_overlapping_reservation_periods_should_produce_known_error()
    {
        var today = DateTime.Today;
        // arrange

        var employee1 = new Employee(Guid.NewGuid(), "Test User 1", "test1@testing.com", DateTime.Now);
        var employee2 = new Employee(Guid.NewGuid(), "Test User 2", "test2@testing.com", DateTime.Now);

        var np1 = new NumberPlate("ZXY123");
        var np2 = new NumberPlate("ZXY321");
        employee1.AddVehicle(np1, VehicleType.Car);
        employee2.AddVehicle(np2, VehicleType.Car);
        
        var spot = new CarSpot(Guid.NewGuid(), "A1");

        Reservation reservation1 = new(Guid.NewGuid(), employee1, np1, new ReservationPeriod(today.AddHours(8), today.AddHours(12)));
        Reservation reservation2 = new(Guid.NewGuid(), employee2,  np2, new ReservationPeriod(today.AddHours(10), today.AddHours(13)));

        spot.ReserveParkingSpot(reservation1);

        // act
        var exception = Record.Exception(() => spot.ReserveParkingSpot(reservation2));

        // assert
        Assert.NotNull(exception);
        Assert.IsType<ParkingSpotAlreadyReservedForPeriodException>(exception);
    }








    #region Arrange
    // private readonly ParkingSpot _parkingSpot;

    // public ParkingSpotTests()
    // {
    //     _parkingSpot = new(Guid.NewGuid(), false);
    // }



    #endregion
}