using FluentAssertions;
using SmartHome.BusinessLogic.Homes;
using SmartHome.BusinessLogic.Users;

namespace SmartHome.BusinessLogic.Test.Homes;

[TestClass]
public class Home_Test
{
    [TestMethod]
    public void Create_WhenInfoIsCorrect_ShouldCreateHome()
    {
        var expectedOwner = GetValidUser();
        var expectedLocation = new Location("Golden street", "818");
        var expectedCoordinates = new Coordinates("123", "456");
        const int expectedMemberCount = 3;

        var newHome = new Home()
        {
            OwnerEmail = expectedOwner.Email,
            Location = expectedLocation,
            Coordinates = expectedCoordinates,
            MemberCount = expectedMemberCount,
            Owner = expectedOwner,
        };

        newHome.Should().NotBeNull();
        newHome.Members.Should().NotBeNull();
        newHome.Hardwares.Should().NotBeNull();
        newHome.Owner.Should().Be(expectedOwner);
        newHome.Location.Should().Be(expectedLocation);
        newHome.Coordinates.Should().Be(expectedCoordinates);
        newHome.MemberCount.Should().Be(expectedMemberCount);
        newHome.OwnerEmail.Should().Be(expectedOwner.Email);
    }

    [TestMethod]
    public void MemberCount_WhenValueIsLessThanOne_ShouldThrowException()
    {
        var act = () => new Home()
        {
            Location = new Location() { Address = "some address", DoorNumber = "999" },
            Coordinates = new Coordinates() { Latitude = "25", Longitude = "36" },
            MemberCount = 0,
            OwnerEmail = "some email"
        };

        act.Should().Throw<ArgumentOutOfRangeException>();
    }

    [DataTestMethod]
    [DataRow(null, "someLongitude")]
    [DataRow("someLatitude", null)]
    public void CreateCordinates_WhenParamsAreNull_ShouldThrow(string latitude, string longitude)
    {
        var act = () => new Coordinates(latitude, longitude);

        act.Should().Throw<ArgumentNullException>();
    }

    #region SampleData
    private User GetValidUser()
    {
        return new User()
        {
            Email = "maticor93@gmail.com",
            Name = "Matias",
            Lastname = "Corvetto",
            Password = "#Adf123456"
        };
    }
    #endregion
}
