Availble services:

/*---------------------CustomizedHeatMap---------------------*/

1) UploadHeatmapPoints(Not tested, waiting for stored procedure to be completed):
- require parameter format:
		{
	"points":  [
				{
					"longitude":"41.817781",
					"latitude":"-87.701545"
				},
				{
					"longitude":"41.862683",
					"latitude":"-87.563021"
				},
				{
					"longitude":"41.726722",
					"latitude":"-87.560988"
				}
		]
}
- return value:  -1 if error occurs, section id(some int) if succeed

2) GetPointsFromSectionId
- require parameter format: integer
	{
	"sectionId":"1"
	}
- return value: count = -1 if error occurs, data.error = erro msg, list of points if succeed


/*---------------------Predefined Maps---------------------*/


Predefined Maps:

1) GetPastWeekPoints
- require parameter format: type of event
	{"type": "crime"} 
- return value: count = -1 if error occurs, data.error = erro msg, list of points if succeed

2) GetPastYearPoints
- require parameter format: type of event
	{"type": "crime"} 
- return value: count = -1 if error occurs, data.error = erro msg, list of points if succeed

3) GetThisYearPoints
- require parameter format: type of event
	{"type": "crime"} 
- return value: count = -1 if error occurs, data.error = erro msg, list of points if succeed


/*---------------------Polygon---------------------*/

1) getCpdBeats
- return list of beats OR null if error occurs

2) getCpdDistricts
- return list of districts OR null if error occurs

3) getCpdSectors
- return list of sectors OR null if error occurs

4) getCpdAreas
- return list of area OR null if error occurs

6) GetPointsForPolygon(Polygon polygon)
- require parameter format: in a object, only fill the require parameter, leave all others blank
Example of GetPointsByArea:
{
	"type":"crime"
	"option":"area",
	"area":"",
	"district":"",
	"sector":"",
	"beat":"",
}

- Provide four options of data: "area","district","sector","beat"
- Different type require different paramter:
	1) area: ONLY needs "area"
	2) district: ONLY needs "district"
	3) sector: Needs "district" + "sector"
	4) beat: Needs "district" + "sector" + "beat"

*Please get all area/district/sector/beat through provided GetArea/GetDistricts/GetSectors/GetBeats
 to ensure validate data
*CRIME does not offer sector attribute to get points. When request CRIME for polygon, please leave it blank
- return value: count = -1 if error occurs, data.error = erro msg, list of points if succeed