using FluentAssertions;

namespace Futurum.WebApiEndpoint.Micro.Tests;

public class DataCollectionDtoTests
{
    [Fact]
    public void when_IEnumerable()
    {
        var numbers = Enumerable.Range(0, 10);

        var dataCollectionDto = numbers.ToDataCollectionDto();

        dataCollectionDto.Count.Should().Be(numbers.Count());
        dataCollectionDto.Data.Should().BeEquivalentTo(numbers);
    }

    [Fact]
    public void when_ICollection()
    {
        var numbers = Enumerable.Range(0, 10)
                                .ToList();

        var dataCollectionDto = numbers.ToDataCollectionDto();

        dataCollectionDto.Count.Should().Be(numbers.Count);
        dataCollectionDto.Data.Should().BeEquivalentTo(numbers);
    }
}