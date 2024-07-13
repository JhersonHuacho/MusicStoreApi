namespace MusicStore.UnitTests
{
	public class SimpleTests
	{
		[Fact]
		public void SimpleTest()
		{
			// Arrange
			int x = 1;
			int y = 2;
			
			// Act
			var sum = x + y;
			var expected = 3;

			// Assert
			Assert.Equal(expected, sum);
		}
	}
}