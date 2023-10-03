namespace CloudDrive.Domain.Entities
{
	public class Data
	{
		public Data()
		{
			Name = string.Empty;
		}

		public int Id { get; set; }
		public string Name { get; set; }
	}
}