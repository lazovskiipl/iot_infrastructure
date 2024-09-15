namespace TestProject1;

public class UnitTest1
{
    [Fact]
    public void Test1()
    {
        var queue = new iot_infrastructure.WaitablePriorityQueue<int>(Comparer<int>.Default, 5);
        // Assert.Equal;
    }
}
