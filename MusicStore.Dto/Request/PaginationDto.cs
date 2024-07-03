namespace MusicStore.Dto.Request;

public class PaginationDto
{
	private readonly int _maxRecordsPerPage = 5;
	public int Page { get; set; } = 1;
	
	private int recordsPerPage;
	public int RecordsPerPage
	{
		get { return recordsPerPage; }
		set { recordsPerPage = value > _maxRecordsPerPage ? _maxRecordsPerPage : value; }
	}
}
