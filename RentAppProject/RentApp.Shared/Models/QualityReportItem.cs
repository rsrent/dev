using System;
namespace RentAppProject
{
	public class QualityReportItem
	{
        public int ID { get; set; }
		public int CleaningTaskID { get; set; }
		public int QualityReportID { get; set; }
		public int Rating { get; set; }
		public string Comment { get; set; }
        public string ImageLocation { get; set; }
        public CleaningTask CleaningTask { get; set; }
		
        public QualityReportItem() { }
		public QualityReportItem(CleaningTask area, int reportID)
		{
			this.CleaningTask = area;
			this.CleaningTaskID = area.ID;
			this.QualityReportID = reportID;
		}
	}
}
